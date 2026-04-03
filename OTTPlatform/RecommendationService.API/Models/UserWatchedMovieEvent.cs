namespace RecommendationService.API.Models;

public class UserWatchedMovieEvent
{
    public int UserId { get; set; }
    public int MovieId { get; set; }
}