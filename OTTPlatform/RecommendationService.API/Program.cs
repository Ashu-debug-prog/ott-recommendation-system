using Microsoft.EntityFrameworkCore;
using RecommendationService.API.Data;
using RecommendationService.API.Services;

namespace RecommendationService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ✅ DB Context
            builder.Services.AddDbContext<RecommendationDbContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
                ));

            // ✅ RabbitMQ Consumer
            builder.Services.AddSingleton<RabbitMqConsumer>();

            // ✅ Controllers
            builder.Services.AddControllers();

            // 🔥 FIX: ML Service should be SINGLETON (IMPORTANT)
            builder.Services.AddSingleton<MLModelService>();

            // 🔥 Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ✅ CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReact",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:3000")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            var app = builder.Build();

            // 🔥 TRAIN MODEL AT STARTUP (FIXED)
            var mlService = app.Services.GetRequiredService<MLModelService>();
            mlService.TrainModel();

            app.UseCors("AllowReact");

            // 🔥 Start RabbitMQ Consumer
            var consumer = app.Services.GetRequiredService<RabbitMqConsumer>();
            consumer.Start();

            // 🔥 Swagger UI
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            // ✅ Root endpoint
            app.MapGet("/", () => "Recommendation Service Running 🚀");

            app.Run();
        }
    }
}