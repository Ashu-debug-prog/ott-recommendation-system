namespace UserService.Application.Features.WatchHistories.Events;

public class UserWatchedMovieEvent
{
    public int UserId { get; set; }
    public int MovieId { get; set; }
}