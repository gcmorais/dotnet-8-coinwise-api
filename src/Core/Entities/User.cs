
using System.Text.Json.Serialization;

namespace Domain.Entities;

public sealed class User : BaseEntity
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public byte[] HashPassword { get; set; }
    public byte[] SaltPassword { get; set; }
    [JsonIgnore]
    public ICollection<Coin> Coins { get; set; } = [];

}
