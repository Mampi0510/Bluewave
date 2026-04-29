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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().ToTable("CLIENT");
            // supprime ou simplifie à :
            modelBuilder.Entity<Commande>().ToTable("COMMANDE");
            modelBuilder.Entity<Export>(entity =>
            {
                entity.ToTable("EXPORT");
                entity.Property(e => e.NumeroExport).HasColumnName("NumeroExport");
            }); modelBuilder.Entity<Fournisseur>().ToTable("FOURNISSEUR");
            modelBuilder.Entity<Achat>().ToTable("ACHAT");
            modelBuilder.Entity<Approvisionnement>().ToTable("APPROVISIONNEMENT");

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.ToTable("STOCK");
                entity.Property(s => s.Type).HasColumnName("Type");
                entity.Property(s => s.Quantite).HasColumnName("Quantite");
            });

            modelBuilder.Entity<Produit>(entity =>
            {
                entity.ToTable("PRODUIT");
                entity.Property(p => p.Date_reception).HasColumnName("Date_reception");
            });
        }
    }
}