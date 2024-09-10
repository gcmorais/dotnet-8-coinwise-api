using Application.UseCases.CoinUseCases.Common;
using Application.UseCases.UserUseCases.Common;
using MediatR;

namespace Application.UseCases.CoinUseCases.CreateCoin
{
    public sealed record CreateCoinRequest(string Name, string Abbreviation, decimal Price, ShortUserRequest UserData) : IRequest<CoinResponse>;
}
