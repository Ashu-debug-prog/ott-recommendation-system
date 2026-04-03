namespace UserService.Domain.Entities;

public class UserPreference
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string FavoriteGenre { get; set; } = string.Empty;

    public User User { get; set; } = null!;
}