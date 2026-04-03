using MediatR;

namespace UserService.Application.Features.WatchHistories.Commands;

public record AddWatchHistoryCommand(
    int UserId,
    int MovieId,
    int WatchTime
) : IRequest<int>;