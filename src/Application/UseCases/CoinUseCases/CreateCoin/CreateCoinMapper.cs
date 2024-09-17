using Application.UseCases.CoinUseCases.Common;
using Application.UseCases.UserUseCases.Common;
using AutoMapper;
using Domain.Entities;

namespace Application.UseCases.CoinUseCases.CreateCoin
{
    public sealed class CreateCoinMapper : Profile
    {
        public CreateCoinMapper()
        {
            CreateMap<CreateCoinRequest, Coin>()
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<Coin, CoinResponse>()
                .ForMember(dest => dest.UserData, opt => opt.MapFrom(src => src.User));

            CreateMap<User, UserShortResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
