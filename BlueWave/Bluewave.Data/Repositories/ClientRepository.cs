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

        public async Task<Client?> GetClientByRef (int Ref){
            return await _context.Client.FindAsync(Ref);
        }
        public async Task<IEnumerable<Client>> GetAllClient()
        {
            var clients = await _context.Client
                .OrderByDescending(c => c.RefClient) 
                .ToListAsync();

            foreach (var c in clients)
            {
                c.Telephone = c.Telephone ?? "Non renseigné";
                c.NomClient = c.NomClient ?? "Inconnu";
                c.PrenomClient = c.PrenomClient ?? "Inconnu";
            }

            return clients;
        }
        public async Task AddClient(Client client)
        {
            _context.Client.Add(client);
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