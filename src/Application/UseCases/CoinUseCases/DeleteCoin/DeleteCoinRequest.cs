using Application.UseCases.CoinUseCases.Common;
using MediatR;

namespace Application.UseCases.CoinUseCases.DeleteCoin
{
    public sealed record DeleteCoinRequest(Guid Id) : IRequest<CoinResponse>;
}
