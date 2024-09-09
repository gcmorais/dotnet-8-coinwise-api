using Application.UseCases.UserUseCases.Common;
using AutoMapper;
using Domain.Entities;

namespace Application.UseCases.UserUseCases.UpdateUser
{
    public sealed class UpdateUserMapper : Profile
    {
        public UpdateUserMapper()
        {
            CreateMap<UpdateUserRequest, User>();
            CreateMap<User, UserResponse>();
        }
    }
}
