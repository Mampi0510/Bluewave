using BlueWave.Core.Models;
using BlueWave.Core.Interfaces;
using BlueWave.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BlueWave.Data.Repositories
{
    public class CommandeRepository : ICommandeRepository
    {
        private readonly AppDbContext _context;

        public CommandeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Commande>> GetAllCommande() =>
            await _context.Commande
                .Include(c => c.Client)
                .ToListAsync();

        public async Task<Commande?> GetCommandeByNum(int num) =>
            await _context.Commande.FindAsync(num);

        public async Task<Commande?> GetCommandeWithDetails(int numeroCommande) =>
            await _context.Commande
                .Include(c => c.Client)
                .Include(c => c.Achats)
                    .ThenInclude(a => a.Produit)
                .FirstOrDefaultAsync(c => c.NumeroCommande == numeroCommande);

        public async Task AddCommande(Commande commande)
        {
            await _context.Commande.AddAsync(commande);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommande(Commande commande)
        {
            _context.Commande.Remove(commande);
            await _context.SaveChangesAsync();
        }
    }
}