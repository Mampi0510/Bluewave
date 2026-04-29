using BlueWave.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueWave.Core.Interfaces{
    public interface IApprovisionnementRepository{
        Task<Approvisionnement?> GetApprovisionnementById(int IdApp);

        Task<IEnumerable<Approvisionnement>> GetAllApprovisionnement();

        Task<IEnumerable<Approvisionnement>> GetApprovisionnementByRefFournisseur(int RefFournisseur);

        Task AddApprovisionnement(Approvisionnement approvisionnement);

        Task UpdateApprovisionnement(Approvisionnement approvisionnement);

        Task DeleteApprovisionnement(Approvisionnement approvisionnement);
    }
}