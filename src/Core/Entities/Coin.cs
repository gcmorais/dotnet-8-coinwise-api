namespace Domain.Entities
{
    public class Coin : BaseEntity
    {
        public string? Name { get; set; }
        public string? Abbreviation { get; set; }
        public decimal? Price { get; set; }
        public User UserData { get; set; }
    }
}
