using Application.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;


public class RabbitMQProducer : IMessagePublisher
{
    private readonly RabbitMQSettings _settings;

    public RabbitMQProducer(RabbitMQSettings settings)
    {
        _settings = settings;
    }

    public void SendMessage<T>(T message)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _settings.HostName,
            UserName = _settings.UserName,
            Password = _settings.Password
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: _settings.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        channel.BasicPublish(exchange: "", routingKey: _settings.QueueName, basicProperties: null, body: body);
    }
}
