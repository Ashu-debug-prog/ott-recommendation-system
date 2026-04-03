using MediatR;
using Microsoft.EntityFrameworkCore;
using UserService.Infrastructure.Persistence;
using UserService.Application.Interfaces;
using UserService.Infrastructure.Repositories;
using UserService.Application;
using UserService.API.Services;

namespace UserService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ---------------- DB CONTEXT ----------------
            builder.Services.AddDbContext<UserDbContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
                ));

            // ---------------- REPOSITORY ----------------
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IWatchHistoryRepository, WatchHistoryRepository>();

            // ---------------- MEDIATR ----------------
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);
            });

            // ---------------- RABBITMQ ----------------
            builder.Services.AddScoped<RabbitMqPublisher>();

            // ---------------- CONTROLLERS ----------------
            builder.Services.AddControllers();

            // ---------------- SWAGGER ----------------
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
            app.UseCors("AllowReact");

            // ---------------- MIDDLEWARE ----------------
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}