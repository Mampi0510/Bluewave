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