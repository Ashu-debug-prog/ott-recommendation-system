using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Features.Users.Commands;
using UserService.Application.Features.Users.Queries;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserCommand command)
    {
        var userId = await _mediator.Send(command);
        return Ok(userId);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var result = await _mediator.Send(new GetUserQuery(id));
        return Ok(result);
    }
}