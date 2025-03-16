using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using UserManagement.API.Core.Domain.Entities;

namespace UserManagement.API.Infrastructure.MessageBroker;

public class RabbitMQService : IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private const string ExchangeName = "user_notifications";
    private bool _disposed;

    public RabbitMQService(IConfiguration configuration)
    {
        var factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:Host"] ?? "localhost",
            Port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672"),
            UserName = configuration["RabbitMQ:Username"] ?? "guest",
            Password = configuration["RabbitMQ:Password"] ?? "guest"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, durable: true);
        _channel.QueueDeclare("user_created", durable: true, exclusive: false, autoDelete: false);
        _channel.QueueDeclare("user_updated", durable: true, exclusive: false, autoDelete: false);
        
        _channel.QueueBind("user_created", ExchangeName, "created");
        _channel.QueueBind("user_updated", ExchangeName, "updated");
    }

    public void PublishUserCreated(User user)
    {
        var message = JsonSerializer.Serialize(new
        {
            user.Id,
            user.Username,
            user.Email,
            user.CreatedAt
        });

        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(ExchangeName, "created", null, body);
    }

    public void PublishUserUpdated(User user)
    {
        var message = JsonSerializer.Serialize(new
        {
            user.Id,
            user.Username,
            user.Email,
            user.UpdatedAt
        });

        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(ExchangeName, "updated", null, body);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }

        _disposed = true;
    }

    ~RabbitMQService()
    {
        Dispose(false);
    }
} 