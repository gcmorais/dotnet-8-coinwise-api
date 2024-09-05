using Application.Communication.Requests;
using Application.Communication.Responses;
using Core.Entities;

namespace Application.Services.User.Login
{
    public class LoginService : ILoginInterface
    {
        public Task<ResponseModel<UserModel>> Login(RequestUserLogin requestUserLogin)
        {
            throw new NotImplementedException();
        }
    }
}
