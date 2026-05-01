using BlueWave.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueWave.Core.Interfaces
{
    public interface IProduitRepository
    {
        Task<Produit?> GetProduitByCode(int codeProduit);
        Task<IEnumerable<Produit>> GetAllProduit();
        Task AddProduit(Produit produit);
        Task UpdateProduit(Produit produit);
        Task DeleteProduit(Produit produit);
    }
}