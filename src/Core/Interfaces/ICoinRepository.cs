using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICoinRepository : IBaseRepository<Coin>
    {
        Task<Coin> GetByAbbreviation(string abbreviation, CancellationToken cancellationToken);
        Task<List<Coin>> GetAllCoins(CancellationToken cancellationToken);
    }
}
