using Application.UseCases.CoinUseCases.CreateCoin;
using FluentValidation;

namespace Application.UseCases.CoinUseCases.Common
{
    public sealed class CoinValidator : AbstractValidator<CreateCoinRequest>
    {
        public CoinValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(20);
            RuleFor(x => x.Abbreviation).NotEmpty().MinimumLength(3).MaximumLength(3);
        }
    }
}