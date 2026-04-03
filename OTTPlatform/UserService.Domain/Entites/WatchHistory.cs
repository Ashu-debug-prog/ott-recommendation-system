namespace UserService.Domain.Entities;

public class WatchHistory
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int MovieId { get; set; }

    public int WatchTime { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // Navigation
    public User User { get; set; } = null!;
}