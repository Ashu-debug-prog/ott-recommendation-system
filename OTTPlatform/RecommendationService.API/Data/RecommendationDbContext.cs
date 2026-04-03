//using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RecommendationService.API.Models;


namespace RecommendationService.API.Data;

public class RecommendationDbContext : DbContext
{
    public RecommendationDbContext(DbContextOptions<RecommendationDbContext> options)
    : base(options)
    {
    }

    public DbSet<UserWatch> UserWatches { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<UserRating> UserRatings { get; set; }
}