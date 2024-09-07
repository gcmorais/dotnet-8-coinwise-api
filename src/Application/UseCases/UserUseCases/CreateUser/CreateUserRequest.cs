using MediatR;

namespace Application.UseCases.UserUseCases.CreateUser
{
    public sealed record CreateUserRequest(string Email, string Name): IRequest<CreateUserResponse>;
}
