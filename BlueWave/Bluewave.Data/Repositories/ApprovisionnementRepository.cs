using BlueWave.Core.Models;
using BlueWave.Core.Interfaces;
using BlueWave.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BlueWave.Data.Repositories
{
    public class ApprovisionnementRepository : IApprovisionnementRepository{
        private readonly AppDbContext _context;

        public ApprovisionnementRepository(AppDbContext context){
            _context = context;
        }


        public async Task<Approvisionnement?> GetApprovisionnementById(int idApp) => await _context.Approvisionnement.FindAsync(idApp);


        public async Task<IEnumerable<Approvisionnement>> GetAllApprovisionnement() => await _context.Approvisionnement.ToListAsync();


        public async Task<IEnumerable<Approvisionnement>> GetApprovisionnementByRefFournisseur(int RefFournisseur) => await _context.Approvisionnement
                .Where(a => a.RefFournisseur == RefFournisseur)
                .ToListAsync();

        public async Task AddApprovisionnement(Approvisionnement approvisionnement){
            await _context.Approvisionnement.AddAsync(approvisionnement);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateApprovisionnement(Approvisionnement approvisionnement){
            _context.Approvisionnement.Update(approvisionnement);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteApprovisionnement(Approvisionnement approvisionnement){
            _context.Approvisionnement.Remove(approvisionnement);
            await _context.SaveChangesAsync();
        }
    }
}