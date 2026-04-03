using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.API.Services;
using UserService.Application.Features.WatchHistories.Commands;
using UserService.Application.Features.WatchHistories.Events;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/watchhistory")]
public class WatchHistoryController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly RabbitMqPublisher _publisher;

    public WatchHistoryController(IMediator mediator, RabbitMqPublisher publisher)
    {
        _mediator = mediator;
        _publisher = publisher;
    }

    [HttpPost]
    public async Task<IActionResult> AddWatchHistory(AddWatchHistoryCommand command)
    {
        var id = await _mediator.Send(command);

        // 🔥 Publish event to RabbitMQ
        _publisher.Publish(new UserWatchedMovieEvent
        {
            UserId = command.UserId,
            MovieId = command.MovieId
        });

        return Ok(id);
    }
}