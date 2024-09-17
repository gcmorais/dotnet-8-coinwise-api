using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public sealed class User : BaseEntity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public byte[] HashPassword { get; private set; }
        public byte[] SaltPassword { get; private set; }
        [JsonIgnore]
        public ICollection<Coin> Coins { get; private set; } = new HashSet<Coin>();

        private User() { }

        // Construtor para inicializar propriedades
        public User(string name, string email, byte[] hashPassword, byte[] saltPassword)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.", nameof(email));

            //if (hashPassword == null || hashPassword.Length == 0)
            //    throw new ArgumentException("HashPassword is required.", nameof(hashPassword));

            //if (saltPassword == null || saltPassword.Length == 0)
            //    throw new ArgumentException("SaltPassword is required.", nameof(saltPassword));

            Name = name;
            Email = email;
            HashPassword = hashPassword;
            SaltPassword = saltPassword;
        }

        // Método para atualizar email de um usuário
        public void UpdateEmail(string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail))
                throw new ArgumentException("Email cannot be empty.", nameof(newEmail));

            Email = newEmail;
            UpdateDate();
        }

        // Método para atualizar nome de um usuário
        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Email cannot be empty.", nameof(newName));

            Name = newName;
            UpdateDate();
        }

        // Método para adicionar uma moeda à coleção
        public void AddCoin(Coin coin)
        {
            if (coin == null)
                throw new ArgumentNullException(nameof(coin));

            if (Coins == null)
                Coins = new HashSet<Coin>();

            Coins.Add(coin);
        }

        // Método para remover uma moeda da coleção
        public void RemoveCoin(Coin coin)
        {
            if (coin == null)
                throw new ArgumentNullException(nameof(coin));

            Coins?.Remove(coin);
        }
    }
}
