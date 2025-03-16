using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Notification.API.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BankingSystem.Shared.Events;

namespace Notification.API.Services;

public class NotificationConsumerService : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<NotificationConsumerService> _logger;
    private const string ExchangeName = "banking_exchange";

    public NotificationConsumerService(
        IConfiguration configuration,
        IServiceProvider serviceProvider,
        ILogger<NotificationConsumerService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;

        var factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:Host"] ?? "localhost",
            UserName = configuration["RabbitMQ:Username"] ?? "guest",
            Password = configuration["RabbitMQ:Password"] ?? "guest"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // Declare exchange
        _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, durable: true);

        // Declare queues
        _channel.QueueDeclare("transfer_notifications", durable: true, exclusive: false, autoDelete: false);
        _channel.QueueDeclare("user_notifications", durable: true, exclusive: false, autoDelete: false);

        // Bind queues to exchange with routing keys
        _channel.QueueBind("transfer_notifications", ExchangeName, "transfer.created");
        _channel.QueueBind("user_notifications", ExchangeName, "user.registered");
        _channel.QueueBind("user_notifications", ExchangeName, "user.updated");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Configure consumer for transfer notifications
        var transferConsumer = new EventingBasicConsumer(_channel);
        transferConsumer.Received += async (model, ea) =>
        {
            try
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var transferEvent = JsonSerializer.Deserialize<TransferCreatedEvent>(message);

                if (transferEvent != null)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();

                    var notification = new Models.Notification
                    {
                        Type = "TransferCreated",
                        Message = $"Transfer completed: {transferEvent.Amount} {transferEvent.Currency} from {transferEvent.FromIBAN} to {transferEvent.ToIBAN}",
                        CreatedAt = DateTime.UtcNow,
                        IsRead = false,
                        Status = "Pending",
                        Data = message
                    };

                    dbContext.Notifications.Add(notification);
                    await dbContext.SaveChangesAsync();

                    _logger.LogInformation("Processed transfer notification for transaction {TransactionId}", transferEvent.TransactionId);
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing transfer notification");
                _channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        // Configure consumer for user notifications
        var userConsumer = new EventingBasicConsumer(_channel);
        userConsumer.Received += async (model, ea) =>
        {
            try
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var routingKey = ea.RoutingKey;

                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();

                var notification = new Models.Notification
                {
                    Type = routingKey,
                    Message = routingKey switch
                    {
                        "user.registered" => CreateUserRegisteredMessage(message),
                        "user.updated" => CreateUserUpdatedMessage(message),
                        _ => "User event received"
                    },
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false,
                    Status = "Pending",
                    Data = message
                };

                dbContext.Notifications.Add(notification);
                await dbContext.SaveChangesAsync();

                _logger.LogInformation("Processed user notification for routing key {RoutingKey}", routingKey);
                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing user notification");
                _channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        _channel.BasicConsume(queue: "transfer_notifications", autoAck: false, consumer: transferConsumer);
        _channel.BasicConsume(queue: "user_notifications", autoAck: false, consumer: userConsumer);

        return Task.CompletedTask;
    }

    private string CreateUserRegisteredMessage(string message)
    {
        try
        {
            var userEvent = JsonSerializer.Deserialize<UserRegisteredEvent>(message);
            return userEvent != null
                ? $"New user registered: {userEvent.Username} ({userEvent.Email})"
                : "New user registered";
        }
        catch
        {
            return "New user registered";
        }
    }

    private string CreateUserUpdatedMessage(string message)
    {
        try
        {
            var userEvent = JsonSerializer.Deserialize<UserUpdatedEvent>(message);
            return userEvent != null
                ? $"User updated: {userEvent.Username} ({userEvent.Email})"
                : "User updated";
        }
        catch
        {
            return "User updated";
        }
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
} 