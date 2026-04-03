using UserService.Domain.Entities;

namespace UserService.Application.Interfaces;

public interface IUserRepository
{
    Task<int> AddUserAsync(User user);
    Task<User?> GetUserByIdAsync(int userId);
}