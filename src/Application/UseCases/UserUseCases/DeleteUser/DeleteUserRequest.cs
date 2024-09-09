using Application.UseCases.UserUseCases.Common;
using MediatR;

namespace Application.UseCases.UserUseCases.DeleteUser
{
    public sealed record DeleteUserRequest(Guid Id) : IRequest<UserResponse>;
}
