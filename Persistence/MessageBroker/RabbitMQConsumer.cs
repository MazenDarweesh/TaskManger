using Application.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MimeKit;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

public class RabbitMQConsumer : IHostedService
{
    private readonly RabbitMQSettings _settings;
    private readonly SmtpClient _smtpClient;
    private readonly IConfiguration _configuration;
    private IConnection _connection;
    private IModel _channel;

    public RabbitMQConsumer(RabbitMQSettings settings, SmtpClient smtpClient, IConfiguration configuration)
    {
        _settings = settings;
        _smtpClient = smtpClient;
        _configuration = configuration;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _settings.HostName,
            UserName = _settings.UserName,
            Password = _settings.Password
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: _settings.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(_channel); // to consume events from the channel
        consumer.Received += async (model, ea) =>  // when a message is received what we want to do
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var emailMessage = JsonSerializer.Deserialize<EmailMessage>(message);

            await SendEmailAsync(emailMessage);
        };

        _channel.BasicConsume(queue: _settings.QueueName, autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _channel?.Close();
        _connection?.Close();
        return Task.CompletedTask;
    }

    private async Task SendEmailAsync(EmailMessage emailMessage)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("ofssInc", "ofssInc-email@example.com"));
        message.To.Add(new MailboxAddress(emailMessage.ToName, emailMessage.ToEmail));
        message.Subject = emailMessage.Subject;
        message.Body = new TextPart("Hello to our system") { Text = emailMessage.Body };

        var smtpSettings = _configuration.GetSection("SmtpSettings");
        var host = smtpSettings["Host"];
        var port = int.Parse(smtpSettings["Port"]);
        var username = smtpSettings["Username"];
        var password = smtpSettings["Password"];

        await _smtpClient.ConnectAsync(host, port, false);
        await _smtpClient.AuthenticateAsync(username, password);
        await _smtpClient.SendAsync(message);
        await _smtpClient.DisconnectAsync(true);
    }
}
