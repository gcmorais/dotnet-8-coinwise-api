using Application.UseCases.UserUseCases.Common;
using Domain.Entities;

namespace Application.UseCases.CoinUseCases.Common
{
    public class CoinResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Abbreviation { get; set; }
        public decimal? Price { get; set; }
        public UserShortResponse UserData { get; set; }
    }
}
