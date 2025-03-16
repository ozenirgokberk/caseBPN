using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using UserManagement.API.Shared.Messages;

namespace UserManagement.API.Services;

public class RabbitMQService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private const string ExchangeName = "banking_exchange";

    public RabbitMQService(IConfiguration configuration)
    {
        var factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:Host"] ?? "localhost"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, durable: true);
    }

    public void PublishUserCreated(UserCreatedMessage message)
    {
        const string routingKey = "user.created";
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        _channel.BasicPublish(
            exchange: ExchangeName,
            routingKey: routingKey,
            basicProperties: null,
            body: body);
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
} 