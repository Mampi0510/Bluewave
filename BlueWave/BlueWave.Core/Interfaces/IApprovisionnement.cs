using BlueWave.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueWave.Core.Interfaces
{
    public interface IApprovisionnementRepository
    {
        Task<IEnumerable<Approvisionnement>> GetAllApprovisionnement();
        Task AddApprovisionnement(Approvisionnement approvisionnement);
        Task DeleteApprovisionnement(Approvisionnement approvisionnement);
        Task UpdateApprovisionnement(Approvisionnement approvisionnement); 
        Task<Approvisionnement?> GetLatestByProduit(int codeProduit);
    }
}