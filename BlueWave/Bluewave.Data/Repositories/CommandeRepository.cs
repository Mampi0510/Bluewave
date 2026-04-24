using BlueWave.Core.Models;
using BlueWave.Core.Interfaces;
using BlueWave.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BlueWave.Data.Repositories
{
    public class CommandeRepository : ICommandeRepository
    {
        private readonly AppDbContext _context;

        public CommandeRepository(AppDbContext context){
            _context = context;
        }


        public async Task<Commande?> GetCommandeByNum(int num) {
            return await _context.Commande.FindAsync(num); 
        }

        public async Task<IEnumerable<Commande>> GetAllCommande() => await _context.Commande.ToListAsync();

        public async Task<IEnumerable<Commande>> GetCommandeByRefClient(int refClient) => await _context.Commande
                .Where(c => c.RefClient == refClient)
                .ToListAsync();

        public async Task<IEnumerable<Commande>> GetCommandeByCodeExport(int codeExport) => await _context.Commande
                .Where(c => c.CodeExport == codeExport)
                .ToListAsync();

        public async Task AddCommande(Commande commande){
            await _context.Commande.AddAsync(commande);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateCommande(Commande commande){
            _context.Commande.Update(commande);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommande(Commande commande) {
            _context.Commande.Remove(commande);
            await _context.SaveChangesAsync();
        }
    }
}