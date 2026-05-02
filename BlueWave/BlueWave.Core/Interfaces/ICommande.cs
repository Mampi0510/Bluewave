using BlueWave.Core.Models;

namespace BlueWave.Core.Interfaces
{
    public interface ICommandeRepository
    {
        Task<IEnumerable<Commande>> GetAllCommande();
        Task<Commande?> GetCommandeByNum(int num);
        Task AddCommande(Commande commande);
        Task DeleteCommande(Commande commande);
        Task<Commande?> GetCommandeWithDetails(int numeroCommande);
    }
}