using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using RecommendationService.API.Data;
using RecommendationService.API.Models;
using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace RecommendationService.API.Services;

public class RabbitMqConsumer
{
    private readonly IServiceProvider _serviceProvider;

    public RabbitMqConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Start()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost"
        };

        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: "watch-history",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"🔥 Received Event: {message}");

            var data = JsonSerializer.Deserialize<UserWatchedMovieEvent>(message);

            if (data == null)
                return;

            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<RecommendationDbContext>();

            // ✅ 1. Save Watch History
            db.UserWatches.Add(new UserWatch
            {
                UserId = data.UserId,
                MovieId = data.MovieId
            });

            await db.SaveChangesAsync();

            // ✅ 2. ADD ML RATING (IMPORTANT)
            var exists = await db.UserRatings
                .AnyAsync(x => x.UserId == data.UserId && x.MovieId == data.MovieId);

            if (!exists)
            {
                var rating = new UserRating
                {
                    UserId = data.UserId,
                    MovieId = data.MovieId,
                    Rating = 4 // ⭐ Implicit rating
                };

                db.UserRatings.Add(rating);
                await db.SaveChangesAsync();

                Console.WriteLine($"✅ Rating added for User {data.UserId}, Movie {data.MovieId}");
            }
        };

        channel.BasicConsume(
            queue: "watch-history",
            autoAck: true,
            consumer: consumer
        );
    }
}