using System.Text.Json.Serialization;

namespace Core.Entities
{
    public class UserModel
    {
        public Guid Id { get; init; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public byte[] HashPassword { get; private set; }
        public byte[] SaltPassword { get; private set; }
        public ICollection<Coin> Coins { get; set; }

        public UserModel(Guid id, string name, string email, byte[] hashPassword, byte[] saltPassword)
        {
            Id = id;
            Name = name;
            Email = email;
            HashPassword = hashPassword;
            SaltPassword = saltPassword;
        }
    }
}
