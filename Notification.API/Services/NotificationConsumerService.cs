using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Notification.API.Data;

namespace Notification.API.Services;

public class NotificationConsumerService : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IServiceProvider _serviceProvider;
    private const string ExchangeName = "banking_exchange";

    public NotificationConsumerService(IConfiguration configuration, IServiceProvider serviceProvider)
    {
        var factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:Host"] ?? "localhost"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _serviceProvider = serviceProvider;

        _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, durable: true);

        // Declare queues for different events
        _channel.QueueDeclare("user_notifications", durable: true, exclusive: false, autoDelete: false);
        _channel.QueueDeclare("customer_notifications", durable: true, exclusive: false, autoDelete: false);
        _channel.QueueDeclare("transaction_notifications", durable: true, exclusive: false, autoDelete: false);

        // Bind queues to exchange with routing keys
        _channel.QueueBind("user_notifications", ExchangeName, "user.created");
        _channel.QueueBind("customer_notifications", ExchangeName, "customer.registered");
        _channel.QueueBind("transaction_notifications", ExchangeName, "transaction.created");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();

            var notification = new Models.Notification
            {
                Message = message,
                CreatedAt = DateTime.UtcNow,
                Type = ea.RoutingKey,
                IsRead = false,
                Status = "Pending"
            };

            dbContext.Notifications.Add(notification);
            await dbContext.SaveChangesAsync();

            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(queue: "user_notifications", autoAck: false, consumer: consumer);
        _channel.BasicConsume(queue: "customer_notifications", autoAck: false, consumer: consumer);
        _channel.BasicConsume(queue: "transaction_notifications", autoAck: false, consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
} 