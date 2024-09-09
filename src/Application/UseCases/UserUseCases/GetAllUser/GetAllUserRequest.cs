using Application.UseCases.UserUseCases.Common;
using MediatR;

namespace Application.UseCases.UserUseCases.GetAllUser
{
    public sealed record GetAllUserRequest : IRequest<List<UserResponse>>;
}
