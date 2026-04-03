using UserService.Domain.Entities;

namespace UserService.Application.Interfaces;

public interface IWatchHistoryRepository
{
    Task<int> AddAsync(WatchHistory history);
}