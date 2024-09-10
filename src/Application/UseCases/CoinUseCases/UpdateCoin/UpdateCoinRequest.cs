using Application.UseCases.CoinUseCases.Common;
using MediatR;

namespace Application.UseCases.CoinUseCases.UpdateCoin
{
    public sealed record UpdateCoinRequest(Guid Id, string Name, string Abbreviation, decimal Price) : IRequest<CoinResponse>;
}
