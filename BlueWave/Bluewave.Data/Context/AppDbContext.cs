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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().ToTable("CLIENT");
            modelBuilder.Entity<Fournisseur>().ToTable("FOURNISSEUR");
            modelBuilder.Entity<Achat>().ToTable("ACHAT");
            modelBuilder.Entity<Approvisionnement>().ToTable("APPROVISIONNEMENT");

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.ToTable("STOCK");
                entity.Property(s => s.NomStock).HasColumnName("NomStock");
                entity.Property(s => s.Quantite).HasColumnName("Quantite");
            });

            modelBuilder.Entity<Produit>(entity =>
            {
                entity.ToTable("PRODUIT");
                entity.Property(p => p.Prix).HasColumnName("Prix");
            });

            modelBuilder.Entity<Commande>(entity =>
            {
                entity.ToTable("COMMANDE");
                entity.Property(c => c.Delai).HasColumnName("Delai");
                entity.HasOne(c => c.Client)
                      .WithMany(c => c.Commande)
                      .HasForeignKey(c => c.RefClient);
            });

            modelBuilder.Entity<Approvisionnement>(entity =>
            {
                entity.ToTable("APPROVISIONNEMENT");
                entity.Property(a => a.DateReception).HasColumnName("DateReception");
                entity.Property(a => a.Quantite).HasColumnName("Quantite");
                entity.Property(a => a.NumeroStock).HasColumnName("NumeroStock");
            });
        }
    }
}