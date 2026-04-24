using Microsoft.EntityFrameworkCore;
using BlueWave.Core.Models;

namespace BlueWave.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Client { get; set; }
        public DbSet<Commande> Commande { get; set; }
        public DbSet<Fournisseur> Fournisseur { get; set; }
        public DbSet<Produit> Produit { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<Achat> Achat { get; set; }
        public DbSet<Approvisionnement> Approvisionnement { get; set; }
        public DbSet<Export> Export { get; set; }

    }
}