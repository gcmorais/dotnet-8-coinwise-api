using Domain.Entities;

namespace Application.UseCases.CoinUseCases.Common
{
    public class CoinResponse : BaseEntity
    {
        public string? Name { get; set; }
        public string? Abbreviation { get; set; }
        public decimal? Price { get; set; }
    }
}
