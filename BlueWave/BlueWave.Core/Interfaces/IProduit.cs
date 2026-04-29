using BlueWave.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueWave.Core.Interfaces{
    public interface IProduitRepository{
        Task<Produit?> GetProduitByCode (int CodeProduit);

        Task<IEnumerable<Produit>> GetAllProduit();

        Task<IEnumerable<Produit>> GetProduitByNumStock(int numStock);

        Task AddProduit(Produit produit);

        Task UpdateProduit(Produit produit);

        Task DeleteProduit(Produit produit);
    }
}