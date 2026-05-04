using BlueWave.Core.Models;
using BlueWave.Core.Interfaces;
using BlueWave.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BlueWave.Data.Repositories
{
    public class ProduitRepository : IProduitRepository
    {
        private readonly AppDbContext _context;

        public ProduitRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Produit?> GetProduitByCode(int codeProduit) =>
            await _context.Produit.FindAsync(codeProduit);

        public async Task<IEnumerable<Produit>> GetAllProduit()
        {
            var produits = await _context.Produit
                .OrderByDescending(p => p.CodeProduit)
                .ToListAsync();
            return produits;
        }
        public async Task AddProduit(Produit produit)
        {
            await _context.Produit.AddAsync(produit);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProduit(Produit produit)
        {
            var tracked = _context.Produit.Local
                              .FirstOrDefault(p => p.CodeProduit == produit.CodeProduit);
            if (tracked != null)
                _context.Entry(tracked).CurrentValues.SetValues(produit);
            else
                _context.Produit.Update(produit);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduit(Produit produit)
        {
            _context.Produit.Remove(produit);
            await _context.SaveChangesAsync();
        }
    }
}