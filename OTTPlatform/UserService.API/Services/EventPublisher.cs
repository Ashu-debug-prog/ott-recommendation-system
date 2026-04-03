using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using UserService.Application.Features.WatchHistories.Events;

namespace UserService.API.Services;

public class RabbitMqPublisher
{
    private readonly ConnectionFactory _factory;

    public RabbitMqPublisher()
    {
        _factory = new ConnectionFactory()
        {
            HostName = "localhost"
        };
    }

    public void Publish(UserWatchedMovieEvent message)
    {
        using var connection = _factory.CreateConnection();
        using var channel = connection.CreateModel();

        // Create queue if not exists
        channel.QueueDeclare(
            queue: "watch-history",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(
            exchange: "",
            routingKey: "watch-history",
            basicProperties: null,
            body: body
        );
    }
}