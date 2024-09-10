using Application.UseCases.UserUseCases.Common;
using Application.UseCases.UserUseCases.CreateUser;
using AutoMapper;
using Domain.Entities;

namespace Application.UseCases.UserUseCases.GetAllUser;

public sealed class GetAllUserMapper : Profile
{
    public GetAllUserMapper()
    {
        CreateMap<ShortUserRequest, User>();
        CreateMap<User, UserShortResponse>();
        CreateMap<User, UserResponse>();
    }
}
