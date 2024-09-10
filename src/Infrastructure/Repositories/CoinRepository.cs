using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class CoinRepository : BaseRepository<Coin>, ICoinRepository
    {
        public CoinRepository(AppDbContext context) : base(context) { }

        public async Task<List<Coin>> GetAllCoins(CancellationToken cancellationToken)
        {
            return await _context.Coins.Include(c => c.UserData).ToListAsync();
        }

        public async Task<Coin> GetByAbbreviation(string abbreviation, CancellationToken cancellationToken)
        {
            return await _context.Coins.FirstOrDefaultAsync(x => x.Abbreviation == abbreviation, cancellationToken);
        }
    }
}
