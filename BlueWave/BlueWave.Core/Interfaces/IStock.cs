using BlueWave.Core.Models;

namespace BlueWave.Core.Interfaces
{
    public interface IStockRepository
    {
        Task<Stock?> GetStockByNum(int numStock);
        Task<IEnumerable<Stock>> GetAllStock();
        Task AddStock(Stock stock);
        Task UpdateStock(Stock stock);
        Task DeleteStock(Stock stock);
        Task<IEnumerable<Approvisionnement>> GetApprovisionnementsByStock(int numeroStock);
    }
}