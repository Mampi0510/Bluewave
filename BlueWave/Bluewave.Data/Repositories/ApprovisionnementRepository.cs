using BlueWave.Core.Models;
using BlueWave.Core.Interfaces;
using BlueWave.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BlueWave.Data.Repositories
{
    public class ApprovisionnementRepository : IApprovisionnementRepository
    {
        private readonly AppDbContext _context;

        public ApprovisionnementRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Approvisionnement>> GetAllApprovisionnement() =>
            await _context.Approvisionnement
            .OrderByDescending(a => a.IdApp)
                .Include(a => a.Fournisseur)
                .Include(a => a.Produit)  
                .Include(a => a.Stock)    
                .ToListAsync();

        public async Task AddApprovisionnement(Approvisionnement appro)
        {
            await _context.Approvisionnement.AddAsync(appro);

            var stock = await _context.Stock.FindAsync(appro.NumeroStock);
            if (stock != null)
            {
                stock.Quantite += appro.Quantite;
                _context.Stock.Update(stock);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteApprovisionnement(Approvisionnement appro)
        {
            var stock = await _context.Stock.FindAsync(appro.NumeroStock);
            if (stock != null)
            {
                stock.Quantite -= appro.Quantite;
                if (stock.Quantite < 0) stock.Quantite = 0;
                _context.Stock.Update(stock);
            }

            _context.Approvisionnement.Remove(appro);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateApprovisionnement(Approvisionnement appro)
        {
            var local = _context.Approvisionnement.Local
                            .FirstOrDefault(a => a.IdApp == appro.IdApp);
            if (local != null)
                _context.Entry(local).State = EntityState.Detached;

            _context.Approvisionnement.Update(appro);
            await _context.SaveChangesAsync();
        }

        public async Task<Approvisionnement?> GetLatestByProduit(int codeProduit)
        {
            return await _context.Approvisionnement
                .AsNoTracking()     // empêche le tracking
                .Where(a => a.CodeProduit == codeProduit)
                .OrderByDescending(a => a.DateReception)
                .FirstOrDefaultAsync();
        }
    }
}