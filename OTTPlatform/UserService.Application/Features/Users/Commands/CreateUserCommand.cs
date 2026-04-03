using MediatR;

namespace UserService.Application.Features.Users.Commands;

public record CreateUserCommand(string Name, int Age, string PreferredLanguage) : IRequest<int>;