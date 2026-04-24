using BlueWave.Core.Models;
using BlueWave.Core.Interfaces;
using BlueWave.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BlueWave.Data.Repositories{
    public class ClientRepository: IClientRepository{
        
        private readonly AppDbContext _context;

        public ClientRepository(AppDbContext context){
            _context = context;
        }

        public async Task<Client> GetClientByRef (int Ref){
            return await _context.Client.FindAsync(Ref);
        }
        public async Task<IEnumerable<Client>> GetAllClient() => await _context.Client.ToListAsync();


        public async Task AddClient(Client client){
            await _context.Client.AddAsync(client);
            await _context.SaveChangesAsync(); 
        }


        public async Task UpdateClient(Client client){
            _context.Client.Update(client);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteClient(Client client){
            _context.Client.Remove(client);
            await _context.SaveChangesAsync();
        }
    }
}