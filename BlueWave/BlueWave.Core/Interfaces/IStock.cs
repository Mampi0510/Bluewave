using BlueWave.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueWave.Core.Interfaces{
    public interface IStockRepository{
        Task<Stock> GetStockByNum (int NumStock);

        Task<IEnumerable<Stock>> GetAllStock();

        Task AddStock(Stock stock);

        Task UpdateStock(Stock stock);

        Task DeleteStock(Stock stock);
    }
}