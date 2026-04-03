using MediatR;
using UserService.Domain.Entities;
using UserService.Application.Interfaces;

namespace UserService.Application.Features.WatchHistories.Commands;

public class AddWatchHistoryHandler
    : IRequestHandler<AddWatchHistoryCommand, int>
{
    private readonly IWatchHistoryRepository _repository;

    public AddWatchHistoryHandler(IWatchHistoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(AddWatchHistoryCommand request, CancellationToken cancellationToken)
    {
        var history = new WatchHistory
        {
            UserId = request.UserId,
            MovieId = request.MovieId,
            WatchTime = request.WatchTime,
            Timestamp = DateTime.UtcNow
        };

        return await _repository.AddAsync(history);
    }
}