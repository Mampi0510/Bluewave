using BlueWave.Core.Models;
using BlueWave.Core.Interfaces;
using BlueWave.Data.Context;
using Microsoft.EntityFrameworkCore;


namespace BlueWave.Data.Repositories{
    public class ExportRepository: IExportRepository{
        private readonly AppDbContext _context;

        public ExportRepository(AppDbContext context){
            _context = context;
        }
        public async Task<Export?> GetExportByNum(int num) => await _context.Export.FindAsync(num);

        public async Task<IEnumerable<Export>> GetAllExport() => await _context.Export.ToListAsync();


        public async Task AddExport(Export export){
            await _context.Export.AddAsync(export);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateExport(Export export){
            _context.Export.Update(export);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteExport(Export export){
            _context.Export.Remove(export);
            await _context.SaveChangesAsync();
        }
    }
}