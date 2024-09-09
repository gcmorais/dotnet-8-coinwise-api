using Application.UseCases.UserUseCases.CreateUser;
using FluentValidation;

namespace Application.UseCases.UserUseCases.Common
{
    public sealed class UserValidator : AbstractValidator<CreateUserRequest>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().MaximumLength(50).EmailAddress();
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(50);
        }
    }
}
