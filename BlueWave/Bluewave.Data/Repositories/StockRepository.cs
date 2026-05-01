using BlueWave.Core.Models;
using BlueWave.Core.Interfaces;
using BlueWave.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BlueWave.Data.Repositories{
    public class StockRepository: IStockRepository{
        private readonly AppDbContext _context;

        public StockRepository(AppDbContext context){
            _context = context;
        }

        public async Task<Stock?> GetStockByNum(int numStock) => await _context.Stock.FindAsync(numStock);

        public async Task<IEnumerable<Stock>> GetAllStock() =>
            await _context.Stock.ToListAsync();
        public async Task AddStock(Stock stock){
            await _context.Stock.AddAsync(stock);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStock(Stock stock){
            _context.Stock.Update(stock);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStock(Stock stock){
            _context.Stock.Remove(stock);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Approvisionnement>> GetApprovisionnementsByStock(int numeroStock) =>
            await _context.Approvisionnement
            .Include(a => a.Produit)
            .Where(a => a.NumeroStock == numeroStock)
            .ToListAsync();
    }
}