using Application.Communication.Requests;
using Application.Communication.Responses;
using Core.Entities;

namespace Application.Services.User.Login
{
    public interface ILoginInterface
    {
        Task<ResponseModel<UserModel>> Login(RequestUserLogin requestUserLogin);
    }
}
