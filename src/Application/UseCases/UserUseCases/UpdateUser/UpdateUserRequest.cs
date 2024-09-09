using Application.UseCases.UserUseCases.Common;
using MediatR;

namespace Application.UseCases.UserUseCases.UpdateUser
{
    public sealed record UpdateUserRequest(Guid Id, string Email, string Name) : IRequest<UserResponse>;
}
