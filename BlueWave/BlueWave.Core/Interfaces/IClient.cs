using BlueWave.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueWave.Core.Interfaces{
    public interface IClientRepository{
        Task<Client?> GetClientByRef (int Ref);

        Task<IEnumerable<Client>> GetAllClient();

        Task AddClient(Client client);

        Task UpdateClient(Client client);

        Task DeleteClient(Client client);

        // Autre methode specifique 
    }
}