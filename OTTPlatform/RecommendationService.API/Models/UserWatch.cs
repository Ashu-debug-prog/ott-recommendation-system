namespace RecommendationService.API.Models;

public class UserWatch
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int MovieId { get; set; }
    public DateTime WatchedAt { get; set; } = DateTime.UtcNow;
}