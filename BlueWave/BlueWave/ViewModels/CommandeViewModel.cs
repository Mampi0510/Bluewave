using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BlueWave.Core.Interfaces;
using BlueWave.Core.Models;
using BlueWave.Services;

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
        public ObservableCollection<PanierItem> Panier { get; } = new();

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

        // Ajouter au panier
        [RelayCommand]
        private async Task AddAuPanier()
        {
            if (SelectedProduit == null) { MErrorMessage = "Sélectionnez un produit."; return; }
            if (QuantiteAchat <= 0) { MErrorMessage = "La quantité est obligatoire."; return; }

            // Vérifier stock disponible
            var appro = await _approvisionnementRepository.GetLatestByProduit(SelectedProduit.CodeProduit);
            if (appro == null) { MErrorMessage = "Aucun approvisionnement pour ce produit."; return; }

            // Quantité déjà dans le panier pour ce produit
            var dejaDansPanier = Panier
                .Where(p => p.CodeProduit == SelectedProduit.CodeProduit)
                .Sum(p => p.Quantite);

            if (appro.Quantite - dejaDansPanier < QuantiteAchat)
            {
                MErrorMessage = $"Stock insuffisant. Disponible : {appro.Quantite - dejaDansPanier}";
                return;
            }

            // Ajouter ou fusionner dans le panier
            var existant = Panier.FirstOrDefault(p => p.CodeProduit == SelectedProduit.CodeProduit);
            if (existant != null)
                existant.Quantite += QuantiteAchat;
            else
                Panier.Add(new PanierItem
                {
                    CodeProduit = SelectedProduit.CodeProduit,
                    NomProduit = SelectedProduit.NomProduit ?? "",
                    Quantite = QuantiteAchat
                });

            SelectedProduit = null;
            QuantiteAchat = 0;
            MErrorMessage = null;
        }

        // Retirer du panier
        [RelayCommand]
        private void RetirerDuPanier(PanierItem item)
        {
            Panier.Remove(item);
        }

        // Créer la commande
        [RelayCommand]
        private async Task Add()
        {
            if (SelectedClient == null) { MErrorMessage = "Sélectionnez un client."; return; }
            if (string.IsNullOrWhiteSpace(Destination)) { MErrorMessage = "La destination est obligatoire."; return; }
            if (Delai <= 0) { MErrorMessage = "Le délai est obligatoire."; return; }
            if (Panier.Count == 0) { MErrorMessage = "Ajoutez au moins un produit."; return; }

            try
            {
                var commande = new Commande
                {
                    RefClient = SelectedClient.RefClient,
                    Destination = Destination,
                    Delai = Delai,
                    DateCommande = DateTime.Now
                };
                await _commandeRepository.AddCommande(commande);

                foreach (var item in Panier)
                {
                    // Créer l'achat (décrémente le stock via AchatRepository)
                    await _achatRepository.AddAchat(new Achat
                    {
                        CodeProduit = item.CodeProduit,
                        NumeroCommande = commande.NumeroCommande,
                        Quantite = item.Quantite
                    });

                    // Décrémenter l'appro
                    var appro = await _approvisionnementRepository.GetLatestByProduit(item.CodeProduit);
                    if (appro != null)
                    {
                        appro.Quantite -= item.Quantite;
                        await _approvisionnementRepository.UpdateApprovisionnement(appro);
                    }
                }

                SelectedClient = null;
                Destination = string.Empty;
                Delai = 0;
                Panier.Clear();
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
                            appro.Quantite += achat.Quantite;
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

    // Classe panier locale
    public partial class PanierItem : ObservableObject
    {
        public int CodeProduit { get; set; }
        public string NomProduit { get; set; } = "";
        [ObservableProperty] private int _quantite;
    }
}