using BlueWave.Core.Interfaces;
using BlueWave.Core.Models;
using BlueWave.Data.Repositories;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BlueWave.ViewModels;

public partial class ApprovisionnementViewModel : ViewModelBase
{
    private readonly IApprovisionnementRepository _repository;
    private readonly IFournisseurRepository _fournisseurRepo;
    private readonly IProduitRepository _produitRepo;
    private readonly IStockRepository _stockRepo;

    public ObservableCollection<Approvisionnement> Approvisionnements { get; } = new();
    public ObservableCollection<Fournisseur> Fournisseurs { get; } = new();
    public ObservableCollection<Produit> Produits { get; } = new();
    public ObservableCollection<Stock> Stocks { get; } = new();

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string? _certificat;
    [ObservableProperty] private int _quantite;
    [ObservableProperty] private DateTime _dateReception = DateTime.Now;
    [ObservableProperty] private Fournisseur? _selectedFournisseur;
    [ObservableProperty] private Produit? _selectedProduit;
    [ObservableProperty] private Stock? _selectedStock;
    [ObservableProperty] private Approvisionnement? _selectedApprovisionnement;
    // Sidebar edit
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private string? _editCertificat;
    [ObservableProperty] private int _editQuantite;
    private int _editingIdApp;
    private int _ancienneQuantite; // pour calculer la diff stock

    [RelayCommand]
    private void Edit(Approvisionnement? appro)
    {
        if (appro == null) return;
        _editingIdApp = appro.IdApp;
        _ancienneQuantite = appro.Quantite;
        EditCertificat = appro.Certificat;
        EditQuantite = appro.Quantite;
        IsEditing = true;
        MErrorMessage = null;
    }

    [RelayCommand]
    private void CancelEdit()
    {
        IsEditing = false;
        _editingIdApp = 0;
        EditCertificat = null;
        EditQuantite = 0;
    }

    [RelayCommand]
    private async Task SaveEdit()
    {
        if (string.IsNullOrWhiteSpace(EditCertificat)) { MErrorMessage = "Le certificat est obligatoire."; return; }
        if (EditQuantite <= 0) { MErrorMessage = "La quantité doit être supérieure à 0."; return; }

        try
        {
            var appro = Approvisionnements.FirstOrDefault(a => a.IdApp == _editingIdApp);
            if (appro == null) return;

            // Calculer la différence pour mettre à jour le stock
            int diff = EditQuantite - _ancienneQuantite;

            // Mettre à jour l'appro
            appro.Certificat = EditCertificat;
            appro.Quantite = EditQuantite;
            await _repository.UpdateApprovisionnement(appro);

            // Mettre à jour le stock
            var stock = await _stockRepo.GetStockByNum(appro.NumeroStock);
            if (stock != null)
            {
                stock.Quantite += diff; // diff positif = augmentation, négatif = diminution
                if (stock.Quantite < 0) stock.Quantite = 0;
                await _stockRepo.UpdateStock(stock);
            }

            IsEditing = false;
            _editingIdApp = 0;
            EditCertificat = null;
            EditQuantite = 0;
            MErrorMessage = null;

            await LoadDataAsync();
        }
        catch (Exception ex) { MErrorMessage = $"Erreur : {ex.Message}"; }
    }

    public ApprovisionnementViewModel(
        IApprovisionnementRepository repository,
        IFournisseurRepository fournisseurRepo,
        IProduitRepository produitRepo,
        IStockRepository stockRepo)
    {
        _repository = repository;
        _fournisseurRepo = fournisseurRepo;
        _produitRepo = produitRepo;
        _stockRepo = stockRepo;
    }

    [RelayCommand]
    private async Task Add()
    {
        if (SelectedFournisseur == null) { MErrorMessage = "Sélectionnez un fournisseur."; return; }
        if (SelectedProduit == null) { MErrorMessage = "Sélectionnez un produit."; return; }
        if (SelectedStock == null) { MErrorMessage = "Sélectionnez un stock."; return; }
        if (Quantite <= 0) { MErrorMessage = "La quantité est obligatoire."; return; }
        if (string.IsNullOrWhiteSpace(Certificat)) { MErrorMessage = "Le certificat est obligatoire."; return; }

        try
        {
            await _repository.AddApprovisionnement(new Approvisionnement
            {
                RefFournisseur = SelectedFournisseur.RefFournisseur,
                CodeProduit = SelectedProduit.CodeProduit,
                NumeroStock = SelectedStock.NumeroStock,
                Quantite = Quantite,
                DateReception = DateReception,
                Certificat = Certificat
            });

            Certificat = string.Empty;
            Quantite = 0;
            DateReception = DateTime.Now;
            SelectedFournisseur = null;
            SelectedProduit = null;
            SelectedStock = null;
            MErrorMessage = null;
            await LoadDataAsync();
        }
        catch (Exception ex) { MErrorMessage = $"Erreur : {ex.Message}"; }
    }

    [RelayCommand]
    private async Task Delete(Approvisionnement? appro)
    {
        var cible = appro ?? SelectedApprovisionnement;
        if (cible == null) return;
        try { await _repository.DeleteApprovisionnement(cible); await LoadDataAsync(); }
        catch (Exception ex) { MErrorMessage = $"Erreur : {ex.Message}"; }
    }

    public async Task LoadDataAsync()
    {
        if (IsLoading) return;
        try
        {
            IsLoading = true;
            Approvisionnements.Clear();
            Fournisseurs.Clear();
            Produits.Clear();
            Stocks.Clear();

            var data = await _repository.GetAllApprovisionnement();
            var fournisseurs = await _fournisseurRepo.GetAllFournisseur();
            var produits = await _produitRepo.GetAllProduit();
            var stocks = await _stockRepo.GetAllStock();

            foreach (var a in data) Approvisionnements.Add(a);
            foreach (var f in fournisseurs) Fournisseurs.Add(f);
            foreach (var p in produits) Produits.Add(p);
            foreach (var s in stocks) Stocks.Add(s);
        }
        catch (Exception ex)
        {
            MErrorMessage = $"Erreur : {ex.Message}";
            System.Windows.MessageBox.Show(ex.ToString(), "Erreur Appro Load");
        }
        finally { IsLoading = false; }
    }
}