using Application.UseCases.CoinUseCases.Common;
using AutoMapper;
using Domain.Entities;

namespace Application.UseCases.CoinUseCases.UpdateCoin;

public sealed class UpdateCoinMapper : Profile
{
    public UpdateCoinMapper()
    {
        CreateMap<UpdateCoinRequest, Coin>();
        CreateMap<Coin, CoinResponse>();
    }
}
