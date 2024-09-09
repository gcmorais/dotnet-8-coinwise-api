using Application.UseCases.UserUseCases.Common;
using AutoMapper;
using Domain.Entities;

namespace Application.UseCases.UserUseCases.GetAllUser;

public sealed class GetAllUserMapper : Profile
{
    public GetAllUserMapper()
    {
        CreateMap<User, UserResponse>();
    }
}
