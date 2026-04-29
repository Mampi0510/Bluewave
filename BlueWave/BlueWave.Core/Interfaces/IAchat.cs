using BlueWave.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueWave.Core.Interfaces{
    public interface IAchatRepository{
        Task<Achat?> GetAchatById(int Id);

        Task<IEnumerable<Achat>> GetAllAchat();

        Task<IEnumerable<Achat>> GetAchatByNumeroCommande(int numCommande);
        Task AddAchat(Achat achat);

        Task UpdateAchat(Achat achat);

        Task DeleteAchat(Achat achat);
        
    }
}