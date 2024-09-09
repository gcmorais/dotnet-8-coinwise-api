using Application.UseCases.CoinUseCases.Common;
using AutoMapper;
using Domain.Entities;

namespace Application.UseCases.CoinUseCases.CreateCoin
{
    public sealed class CreateCoinMapper : Profile
    {
        public CreateCoinMapper()
        {
            CreateMap<CreateCoinRequest, Coin>();
            CreateMap<Coin, CoinResponse>();
        }
    }
}
