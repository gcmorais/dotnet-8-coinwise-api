using System.ComponentModel.DataAnnotations;

namespace Application.Communication.Requests
{
    public class RequestRegisterUserJson
    {
        [Required(ErrorMessage = "The name is required.")]
        public string Name { get; private set; }

        [Required(ErrorMessage = "The Email is required.")]
        public string Email { get; private set; }

        [Required(ErrorMessage = "The Password is required.")]
        public string Password { get; private set; }

        [Required(ErrorMessage = "Please, confirm the password.")]
        public string ConfirmPassword { get; private set; }

        public RequestRegisterUserJson(string name, string email, string password, string confirmPassword)
        {
            Name = name;
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
        }
    }
}
