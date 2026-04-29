using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BlueWave.Core.Interfaces;
using BlueWave.Core.Models;

namespace BlueWave.ViewModels;

public partial class CommandeHubViewModel : ViewModelBase
{
    private readonly ICommandeRepository _commandeRepo;
    private readonly IExportRepository _exportRepo;
    private readonly IAchatRepository _achatRepo;
    private readonly IApprovisionnementRepository _appRepo;
    private readonly IClientRepository _clientRepo;
    private readonly IFournisseurRepository _fournisseurRepo;
    private readonly IProduitRepository _produitRepo;

    public ObservableCollection<Commande> Commandes { get; } = new();
    public ObservableCollection<Export> Exports { get; } = new();
    public ObservableCollection<Achat> Achats { get; } = new();
    public ObservableCollection<Approvisionnement> Approvisionnements { get; } = new();
    public ObservableCollection<Client> Clients { get; } = new();
    public ObservableCollection<Fournisseur> Fournisseurs { get; } = new();
    public ObservableCollection<Produit> Produits { get; } = new();

    [ObservableProperty] private bool _isLoading;

    // Commande
    [ObservableProperty] private Client? _selectedClient;
    [ObservableProperty] private Export? _selectedExport;
    [ObservableProperty] private string? _destination;
    [ObservableProperty] private string? _commandeErrorMessage;

    // Export
    [ObservableProperty] private int _delai;
    [ObservableProperty] private string? _statut;
    [ObservableProperty] private string? _exportErrorMessage;

    // Approvisionnement
    [ObservableProperty] private Fournisseur? _selectedFournisseur;
    [ObservableProperty] private Produit? _selectedProduitApp;
    [ObservableProperty] private string? _certificat;
    [ObservableProperty] private string? _appErrorMessage;

    // Achat
    [ObservableProperty] private Produit? _selectedProduit;
    [ObservableProperty] private Commande? _selectedCommande;
    [ObservableProperty] private int _quantite;
    [ObservableProperty] private string? _achatErrorMessage;

    public CommandeHubViewModel(
        ICommandeRepository commandeRepo,
        IExportRepository exportRepo,
        IAchatRepository achatRepo,
        IApprovisionnementRepository appRepo,
        IClientRepository clientRepo,
        IFournisseurRepository fournisseurRepo,
        IProduitRepository produitRepo)
    {
        _commandeRepo = commandeRepo;
        _exportRepo = exportRepo;
        _achatRepo = achatRepo;
        _appRepo = appRepo;
        _clientRepo = clientRepo;
        _fournisseurRepo = fournisseurRepo;
        _produitRepo = produitRepo;
    }

    [RelayCommand]
    private async Task AddCommande()
    {
        if (SelectedClient == null) { CommandeErrorMessage = "Sélectionnez un client."; return; }
        if (SelectedExport == null) { CommandeErrorMessage = "Sélectionnez un export."; return; }
        if (string.IsNullOrWhiteSpace(Destination)) { CommandeErrorMessage = "La destination est obligatoire."; return; }

        try
        {
            await _commandeRepo.AddCommande(new Commande
            {
                RefClient = SelectedClient.RefClient,
                NumeroExport = SelectedExport.NumeroExport,
                Destination = Destination,
                DateCommande = DateTime.Now
            });

            SelectedClient = null;
            SelectedExport = null;
            Destination = string.Empty;
            CommandeErrorMessage = null;
            await LoadDataAsync();
        }
        catch (Exception ex) { CommandeErrorMessage = ex.Message; }
    }

    [RelayCommand]
    private async Task DeleteCommande(Commande commande)
    {
        if (commande == null) return;
        try { await _commandeRepo.DeleteCommande(commande); await LoadDataAsync(); }
        catch (Exception ex) { CommandeErrorMessage = ex.Message; }
    }

    [RelayCommand]
    private async Task AddExport()
    {
        if (Delai <= 0) { ExportErrorMessage = "Le délai est obligatoire."; return; }
        if (string.IsNullOrWhiteSpace(Statut)) { ExportErrorMessage = "Le statut est obligatoire."; return; }

        try
        {
            await _exportRepo.AddExport(new Export
            {
                Delai = Delai,
                Statut = Statut
            });

            Delai = 0;
            Statut = string.Empty;
            ExportErrorMessage = null;
            await LoadDataAsync();
        }
        catch (Exception ex) { ExportErrorMessage = ex.Message; }
    }

    [RelayCommand]
    private async Task DeleteExport(Export export)
    {
        if (export == null) return;
        try { await _exportRepo.DeleteExport(export); await LoadDataAsync(); }
        catch (Exception ex) { ExportErrorMessage = ex.Message; }
    }

    [RelayCommand]
    private async Task AddAchat()
    {
        if (SelectedProduit == null) { AchatErrorMessage = "Sélectionnez un produit."; return; }
        if (SelectedCommande == null) { AchatErrorMessage = "Sélectionnez une commande."; return; }
        if (Quantite <= 0) { AchatErrorMessage = "La quantité est obligatoire."; return; }

        try
        {
            await _achatRepo.AddAchat(new Achat
            {
                CodeProduit = SelectedProduit.CodeProduit,
                NumeroCommande = SelectedCommande.NumeroCommande,
                Quantite = Quantite
            });

            SelectedProduit = null;
            SelectedCommande = null;
            Quantite = 0;
            AchatErrorMessage = null;
            await LoadDataAsync();
        }
        catch (Exception ex) { AchatErrorMessage = ex.Message; }
    }

    [RelayCommand]
    private async Task DeleteAchat(Achat achat)
    {
        if (achat == null) return;
        try { await _achatRepo.DeleteAchat(achat); await LoadDataAsync(); }
        catch (Exception ex) { AchatErrorMessage = ex.Message; }
    }

    [RelayCommand]
    private async Task AddApprovisionnnement()
    {
        if (SelectedFournisseur == null) { AppErrorMessage = "Sélectionnez un fournisseur."; return; }
        if (SelectedProduitApp == null) { AppErrorMessage = "Sélectionnez un produit."; return; }
        if (string.IsNullOrWhiteSpace(Certificat)) { AppErrorMessage = "Le certificat est obligatoire."; return; }

        try
        {
            await _appRepo.AddApprovisionnement(new Approvisionnement
            {
                RefFournisseur = SelectedFournisseur.RefFournisseur,
                CodeProduit = SelectedProduitApp.CodeProduit,
                Certificat = Certificat
            });

            SelectedFournisseur = null;
            SelectedProduitApp = null;
            Certificat = string.Empty;
            AppErrorMessage = null;
            await LoadDataAsync();
        }
        catch (Exception ex) { AppErrorMessage = ex.Message; }
    }

    [RelayCommand]
    private async Task DeleteApprovisionnnement(Approvisionnement app)
    {
        if (app == null) return;
        try { await _appRepo.DeleteApprovisionnement(app); await LoadDataAsync(); }
        catch (Exception ex) { AppErrorMessage = ex.Message; }
    }

    public async Task LoadDataAsync()
    {
        if (IsLoading) return;
        try
        {
            IsLoading = true;
            Commandes.Clear(); Exports.Clear(); Achats.Clear();
            Approvisionnements.Clear(); Clients.Clear();
            Fournisseurs.Clear(); Produits.Clear();

            var commandes = await _commandeRepo.GetAllCommande();
            var exports = await _exportRepo.GetAllExport();
            var achats = await _achatRepo.GetAllAchat();
            var apps = await _appRepo.GetAllApprovisionnement();
            var clients = await _clientRepo.GetAllClient();
            var fournisseurs = await _fournisseurRepo.GetAllFournisseur();
            var produits = await _produitRepo.GetAllProduit();

            foreach (var c in commandes) Commandes.Add(c);
            foreach (var e in exports) Exports.Add(e);
            foreach (var a in achats) Achats.Add(a);
            foreach (var a in apps) Approvisionnements.Add(a);
            foreach (var c in clients) Clients.Add(c);
            foreach (var f in fournisseurs) Fournisseurs.Add(f);
            foreach (var p in produits) Produits.Add(p);
        }
        catch (Exception ex)
        {
            CommandeErrorMessage = $"Erreur chargement : {ex.Message}";
        }
        finally { IsLoading = false; }
    }
}