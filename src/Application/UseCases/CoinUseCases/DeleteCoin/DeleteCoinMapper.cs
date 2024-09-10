using Application.UseCases.CoinUseCases.Common;
using AutoMapper;
using Domain.Entities;

namespace Application.UseCases.CoinUseCases.DeleteCoin;

public sealed class DeleteCoinMapper : Profile
{
    public DeleteCoinMapper()
    {
        CreateMap<DeleteCoinRequest, Coin>();
        CreateMap<Coin, CoinResponse>();
    }
}
