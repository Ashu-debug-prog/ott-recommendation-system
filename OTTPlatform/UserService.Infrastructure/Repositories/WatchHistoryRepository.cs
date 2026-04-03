using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Repositories;

public class WatchHistoryRepository : IWatchHistoryRepository
{
    private readonly UserDbContext _context;

    public WatchHistoryRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(WatchHistory history)
    {
        _context.WatchHistories.Add(history);
        await _context.SaveChangesAsync();
        return history.Id;
    }
}