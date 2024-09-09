using Application.UseCases.CoinUseCases.Common;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.CoinUseCases.CreateCoin
{
    public sealed record CreateCoinRequest(string Name, string Abbreviation, decimal Price) : IRequest<CoinResponse>;
}
