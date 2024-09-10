using MediatR;

namespace Application.UseCases.UserUseCases.Common
{
    public sealed record ShortUserRequest(Guid Id, string Email, string Name) : IRequest<UserShortResponse>;
}
