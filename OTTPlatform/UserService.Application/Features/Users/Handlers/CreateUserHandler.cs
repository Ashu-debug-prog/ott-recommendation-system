using MediatR;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Users.Commands;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly IUserRepository _repository;

    public CreateUserHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Name = request.Name,
            Age = request.Age,
            PreferredLanguage = request.PreferredLanguage
        };

        return await _repository.AddUserAsync(user);
    }
}