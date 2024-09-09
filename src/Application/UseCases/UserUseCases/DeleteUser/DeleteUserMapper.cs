using Application.UseCases.UserUseCases.Common;
using AutoMapper;
using Domain.Entities;

namespace Application.UseCases.UserUseCases.DeleteUser
{
    public sealed class DeleteUserMapper : Profile
    {
        public DeleteUserMapper()
        {
            CreateMap<DeleteUserRequest, User>();
            CreateMap<User, UserResponse>();
        }
    }
}
