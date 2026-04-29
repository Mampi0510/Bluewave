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

        public ObservableCollection<Commande> Commandes { get; set; } = new();

        [ObservableProperty] private int totalCommandes;
        [ObservableProperty] private int totalClients;
        [ObservableProperty] private int totalFournisseurs;
        [ObservableProperty] private int exportEnCours;

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

                ExportEnCours = await _context.Export.CountAsync(e => e.Statut == "En cours");

                var data = await _context.Commande
                    .Include(c => c.Client)
                    .ToListAsync();

                Commandes.Clear();
                foreach (var item in data)
                    Commandes.Add(item);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Dashboard Load Error");
            }
        }
    }
}