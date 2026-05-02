using BlueWave.Core.Interfaces;
using BlueWave.Core.Models;
using BlueWave.Data.Repositories;
using BlueWave.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BlueWave.ViewModels
{
    public partial class CommandeViewModel : ViewModelBase
    {
        private readonly ICommandeRepository _commandeRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IAchatRepository _achatRepository;
        private readonly IProduitRepository _produitRepository;
        private readonly IApprovisionnementRepository _approvisionnementRepository;
        private readonly IStockRepository _stockRepository;

        public ObservableCollection<Commande> Commandes { get; } = new();
        public ObservableCollection<Client> Clients { get; } = new();
        public ObservableCollection<Produit> Produits { get; } = new();

        public ObservableCollection<Achat> AchatsEnCours { get; } = new();

        [ObservableProperty] private bool _isLoading;
        [ObservableProperty] private Client? _selectedClient;
        [ObservableProperty] private string? _destination;
        [ObservableProperty] private int _delai;
        [ObservableProperty] private Produit? _selectedProduit;
        [ObservableProperty] private int _quantiteAchat;

        private Commande? _selectedCommande;
        public Commande? SelectedCommande
        {
            get => _selectedCommande;
            set => SetProperty(ref _selectedCommande, value);
        }

        public CommandeViewModel(
            ICommandeRepository commandeRepository,
            IClientRepository clientRepository,
            IAchatRepository achatRepository,
            IProduitRepository produitRepository,
            IApprovisionnementRepository approvisionnementRepository,
            IStockRepository stockRepository)
        {
            _commandeRepository = commandeRepository;
            _clientRepository = clientRepository;
            _achatRepository = achatRepository;
            _produitRepository = produitRepository;
            _approvisionnementRepository = approvisionnementRepository;
            _stockRepository = stockRepository;
        }

        // Ajouter un produit au panier local
        [RelayCommand]
        private void AddAchatEnCours()
        {
            if (SelectedProduit == null) { MErrorMessage = "Sélectionnez un produit."; return; }
            if (QuantiteAchat <= 0) { MErrorMessage = "La quantité doit être supérieure à 0."; return; }

            var existant = AchatsEnCours.FirstOrDefault(a => a.CodeProduit == SelectedProduit.CodeProduit);
            if (existant != null)
                existant.Quantite += QuantiteAchat;
            else
                AchatsEnCours.Add(new Achat
                {
                    CodeProduit = SelectedProduit.CodeProduit,
                    Produit = SelectedProduit,
                    Quantite = QuantiteAchat
                });

            SelectedProduit = null;
            QuantiteAchat = 0;
            MErrorMessage = null;
        }

        // Retirer un produit du panier
        [RelayCommand]
        private void RemoveAchatEnCours(Achat achat)
        {
            AchatsEnCours.Remove(achat);
        }

        // Créer la commande 
        [RelayCommand]
        private async Task Add()
        {
            if (SelectedClient == null) { MErrorMessage = "Sélectionnez un client."; return; }
            if (string.IsNullOrWhiteSpace(Destination)) { MErrorMessage = "La destination est obligatoire."; return; }
            if (Delai <= 0) { MErrorMessage = "Le délai est obligatoire."; return; }
            if (SelectedProduit == null) { MErrorMessage = "Sélectionnez un produit."; return; }
            if (QuantiteAchat <= 0) { MErrorMessage = "La quantité est obligatoire."; return; }

            try
            {
                var appro = await _approvisionnementRepository.GetLatestByProduit(SelectedProduit.CodeProduit);
                if (appro == null)
                {
                    MErrorMessage = "Aucun approvisionnement trouvé pour ce produit.";
                    return;
                }

                if (appro.Quantite < QuantiteAchat)
                {
                    MErrorMessage = $"Stock insuffisant. Disponible : {appro.Quantite}";
                    return;
                }

                var commande = new Commande
                {
                    RefClient = SelectedClient.RefClient,
                    Destination = Destination,
                    Delai = Delai,
                    DateCommande = DateTime.Now
                };
                await _commandeRepository.AddCommande(commande);

                // AddAchat décrémente déjà le stock dans le repo
                await _achatRepository.AddAchat(new Achat
                {
                    CodeProduit = SelectedProduit.CodeProduit,
                    NumeroCommande = commande.NumeroCommande,
                    Quantite = QuantiteAchat
                });

                // Décrémenter uniquement l'appro
                appro.Quantite -= QuantiteAchat;
                await _approvisionnementRepository.UpdateApprovisionnement(appro);

                SelectedClient = null;
                Destination = string.Empty;
                Delai = 0;
                SelectedProduit = null;
                QuantiteAchat = 0;
                MErrorMessage = null;

                await LoadDataAsync();
            }
            catch (Exception ex) { MErrorMessage = $"Erreur : {ex.Message}"; }
        }

        [RelayCommand]
        private async Task Delete(Commande? commande)
        {
            var cible = commande ?? SelectedCommande;
            if (cible == null) return;
            try
            {
                var details = await _commandeRepository.GetCommandeWithDetails(cible.NumeroCommande);
                if (details?.Achats != null)
                {
                    foreach (var achat in details.Achats)
                    {
                        var appro = await _approvisionnementRepository.GetLatestByProduit(achat.CodeProduit);
                        if (appro != null)
                        {
                            await _approvisionnementRepository.UpdateApprovisionnement(appro);
                        }
                    }
                }

                await _commandeRepository.DeleteCommande(cible);
                await LoadDataAsync();
            }
            catch (Exception ex) { MErrorMessage = $"Erreur : {ex.Message}"; }
        }

        [RelayCommand]
        private async Task GenererFacture(Commande? commande)
        {
            if (commande == null) return;
            try
            {
                var details = await _commandeRepository.GetCommandeWithDetails(commande.NumeroCommande);
                if (details == null) return;
                FacturePdfService.Generer(details);
            }
            catch (Exception ex) { MErrorMessage = $"Erreur facture : {ex.Message}"; }
        }

        public async Task LoadDataAsync()
        {
            if (IsLoading) return;
            try
            {
                IsLoading = true;
                Commandes.Clear(); Clients.Clear(); Produits.Clear();

                var commandes = await _commandeRepository.GetAllCommande();
                var clients = await _clientRepository.GetAllClient();
                var produits = await _produitRepository.GetAllProduit();

                foreach (var c in commandes) Commandes.Add(c);
                foreach (var c in clients) Clients.Add(c);
                foreach (var p in produits) Produits.Add(p);
            }
            catch (Exception ex) { MErrorMessage = $"Erreur : {ex.Message}"; }
            finally { IsLoading = false; }
        }
    }
}