namespace UserService.Domain.Entities;

public class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    public string PreferredLanguage { get; set; } = string.Empty;

    // Navigation Property
    public ICollection<WatchHistory> WatchHistories { get; set; } = new List<WatchHistory>();
}