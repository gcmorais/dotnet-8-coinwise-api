using System.ComponentModel.DataAnnotations;
using Application.UseCases.UserUseCases.Common;
using Domain.Models;
using MediatR;

namespace Application.UseCases.UserUseCases.CreateUser
{
    public sealed record CreateUserRequest : IRequest<UserResponse>
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords are not the same")]
        public string ConfirmPassword { get; set; }
    };
}
