using BlueWave.Core.Models;
using BlueWave.Core.Interfaces;
using BlueWave.Data.Context;
using Microsoft.EntityFrameworkCore;


namespace BlueWave.Data.Repositories{
    public class FournisseurRepository: IFournisseurRepository{
        private readonly AppDbContext _context;

        public FournisseurRepository(AppDbContext context){
            _context = context;
        }


        public async Task<Fournisseur?> GetFournisseurByref(int refFournisseur) =>
            
            await _context.Fournisseur.FindAsync(refFournisseur);


        public async Task<IEnumerable<Fournisseur>> GetAllFournisseur()
        {
            var founisseurs = await _context.Fournisseur
            .OrderByDescending(f => f.RefFournisseur)
            .ToListAsync();

            foreach (var f in founisseurs)
            {
                f.NomFournisseur = f.NomFournisseur ?? "Inconnu";
                f.PrenomsFournisseur = f.PrenomsFournisseur ?? "Inconnu";
                f.TelephoneFournisseur = f.TelephoneFournisseur ?? "Non renseigné";
            }

            return founisseurs;
        }


        public async Task AddFournisseur(Fournisseur fournisseur){
            await _context.Fournisseur.AddAsync(fournisseur);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFournisseur(Fournisseur fournisseur){
            _context.Fournisseur.Update(fournisseur);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFournisseur(Fournisseur fournisseur){
            _context.Fournisseur.Remove(fournisseur);
            await _context.SaveChangesAsync();
        }
    }
}