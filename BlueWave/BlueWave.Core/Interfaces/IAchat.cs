using BlueWave.Core.Models;

namespace BlueWave.Core.Interfaces
{
    public interface IAchatRepository
    {
        Task<IEnumerable<Achat>> GetAllAchat();
        Task AddAchat(Achat achat);
        Task DeleteAchat(Achat achat);
    }
}