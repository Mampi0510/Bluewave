using BlueWave.Core.Models;
using BlueWave.Core.Interfaces;
using BlueWave.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BlueWave.Data.Repositories
{
    public class AchatRepository : IAchatRepository
    {
        private readonly AppDbContext _context;

        public AchatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Achat?> GetAchatById(int Id) => await _context.Achat.FindAsync(Id);

        public async Task<IEnumerable<Achat>> GetAllAchat() => await _context.Achat.ToListAsync();

        public async Task<IEnumerable<Achat>> GetAchatByNumeroCommande(int numCommande) =>
            await _context.Achat
                .Where(a => a.NumeroCommande == numCommande)
                .ToListAsync();

        public async Task AddAchat(Achat achat)
        {
            await _context.Achat.AddAsync(achat);

            // Diminuer la quantité du stock dans produit
            var appro = await _context.Approvisionnement
                .Where(a => a.CodeProduit == achat.CodeProduit)
                .OrderByDescending(a => a.DateReception)
                .FirstOrDefaultAsync();

            if (appro != null)
            {
                var stock = await _context.Stock.FindAsync(appro.NumeroStock);
                if (stock != null)
                {
                    stock.Quantite -= achat.Quantite;
                    if (stock.Quantite < 0) stock.Quantite = 0;
                    _context.Stock.Update(stock);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAchat(Achat achat)
        {
            _context.Achat.Update(achat);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAchat(Achat achat)
        {
            _context.Achat.Remove(achat);
            await _context.SaveChangesAsync();
        }
    }
}