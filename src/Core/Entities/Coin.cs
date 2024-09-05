using Core.Enums;

namespace Core.Entities
{
    public class Coin
    {
        public Guid Id { get; init; }
        public string Name { get; private set; }
        public string Abbreviation { get; private set; }
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        public DateTime StartDate { get; init; }
        public CoinStatus Status { get; init; }

        public Coin(Guid id, string name, string abbreviation, string? description, decimal price, DateTime startDate, CoinStatus status)
        {
            Id = id;
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
            Price = price;
            StartDate = startDate;
            Status = CoinStatus.Active;
        }
    }
}
