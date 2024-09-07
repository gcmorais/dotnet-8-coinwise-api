using Application.UseCases.UserUseCases.Common;
using AutoMapper;
using Domain.Entities;

namespace Application.UseCases.UserUseCases.CreateUser
{
    public sealed class CreateUserMapper : Profile
    {
        public CreateUserMapper()
        {
            CreateMap<CreateUserRequest, User>();
            CreateMap<User, UserResponse>();
        }
    }
}
