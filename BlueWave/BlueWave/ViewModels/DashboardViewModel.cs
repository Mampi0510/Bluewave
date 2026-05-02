using CommunityToolkit.Mvvm.ComponentModel;
using BlueWave.Data.Context;
using BlueWave.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace BlueWave.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly AppDbContext _context;

        public ObservableCollection<Commande> Commandes { get; } = new();

        [ObservableProperty] private int _totalCommandes;
        [ObservableProperty] private int _totalClients;
        [ObservableProperty] private int _totalFournisseurs;
        [ObservableProperty] private int _totalProduits;
        [ObservableProperty] private int _totalStocks;

        public DashboardViewModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task LoadDataAsync()
        {
            try
            {
                TotalCommandes = await _context.Commande.CountAsync();
                TotalClients = await _context.Client.CountAsync();
                TotalFournisseurs = await _context.Fournisseur.CountAsync();
                TotalProduits = await _context.Produit.CountAsync();
                TotalStocks = await _context.Stock.CountAsync();

                var commandes = await _context.Commande
                    .Include(c => c.Client)
                    .OrderByDescending(c => c.DateCommande)
                    .Take(20)
                    .ToListAsync();

                Commandes.Clear();
                foreach (var c in commandes) Commandes.Add(c);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Dashboard Error");
            }
        }
    }
}