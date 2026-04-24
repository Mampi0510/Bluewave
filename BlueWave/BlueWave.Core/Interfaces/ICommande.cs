using BlueWave.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueWave.Core.Interfaces{
    public interface ICommandeRepository{
        Task<Commande> GetCommandeByNum(int NumCommande);

        Task<IEnumerable<Commande> > GetAllCommande();

        Task<IEnumerable<Commande> > GetCommandeByRefClient(int refClient);

        Task<IEnumerable<Commande> > GetCommandeByCodeExport(int codeExport);

        Task AddCommande(Commande commande);

        Task UpdateCommande(Commande commande);

        Task DeleteCommande(Commande commande);
    }
}