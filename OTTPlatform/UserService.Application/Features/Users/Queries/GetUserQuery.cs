using MediatR;
using UserService.Application.Features.Users.DTOs;

namespace UserService.Application.Features.Users.Queries;

public record GetUserQuery(int UserId) : IRequest<UserDto>;