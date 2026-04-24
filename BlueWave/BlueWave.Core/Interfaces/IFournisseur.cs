using BlueWave.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueWave.Core.Interfaces{
    public interface IFournisseurRepository{
        Task<Fournisseur> GetFournisseurByref (int RefFournisseur);

        Task<IEnumerable<Fournisseur> > GetAllFournisseur();

        Task AddFournisseur(Fournisseur fournisseur);

        Task UpdateFournisseur(Fournisseur fournisseur);

        Task DeleteFournisseur(Fournisseur fournisseur);
    }
}