using Application.UseCases.CoinUseCases.Common;
using AutoMapper;
using Domain.Entities;

namespace Application.UseCases.CoinUseCases.GetAllCoin;

public sealed class GetAllCoinMapper : Profile
{
    public GetAllCoinMapper()
    {
        CreateMap<Coin, CoinResponse>();
    }
}
