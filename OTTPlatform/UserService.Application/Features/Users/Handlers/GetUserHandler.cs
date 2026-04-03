using MediatR;
using UserService.Application.Interfaces;
using UserService.Application.Features.Users.DTOs;

namespace UserService.Application.Features.Users.Queries;

public class GetUserHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IUserRepository _repository;

    public GetUserHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetUserByIdAsync(request.UserId);

        if (user == null)
            throw new Exception("User not found");

        return new UserDto
        {
            UserId = user.UserId,
            Name = user.Name
        };
    }
}