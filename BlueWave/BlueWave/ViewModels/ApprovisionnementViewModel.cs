using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BlueWave.Core.Interfaces;
using BlueWave.Core.Models;

namespace BlueWave.ViewModels;

public partial class ApprovisionnementViewModel : ViewModelBase
{
    private readonly IApprovisionnementRepository _repository;

    public ObservableCollection<Approvisionnement> Approvisionnements { get; } = new();
    public ObservableCollection<Fournisseur> Fournisseurs { get; } = new();
    public ObservableCollection<Produit> Produits { get; } = new();

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string? _certificat;
    [ObservableProperty] private Fournisseur? _selectedFournisseur;
    [ObservableProperty] private Produit? _selectedProduit;
    [ObservableProperty] private Approvisionnement? _selectedApprovisionnement;

    private readonly IFournisseurRepository _fournisseurRepo;
    private readonly IProduitRepository _produitRepo;

    public ApprovisionnementViewModel(
        IApprovisionnementRepository repository,
        IFournisseurRepository fournisseurRepo,
        IProduitRepository produitRepo)
    {
        _repository = repository;
        _fournisseurRepo = fournisseurRepo;
        _produitRepo = produitRepo;
    }

    [RelayCommand]
    private async Task Add()
    {
        if (SelectedFournisseur == null) { MErrorMessage = "Sélectionnez un fournisseur."; return; }
        if (SelectedProduit == null) { MErrorMessage = "Sélectionnez un produit."; return; }
        if (string.IsNullOrWhiteSpace(Certificat)) { MErrorMessage = "Le certificat est obligatoire."; return; }

        try
        {
            await _repository.AddApprovisionnement(new Approvisionnement
            {
                RefFournisseur = SelectedFournisseur.RefFournisseur,
                CodeProduit = SelectedProduit.CodeProduit,
                Certificat = Certificat
            });

            Certificat = string.Empty;
            SelectedFournisseur = null;
            SelectedProduit = null;
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

            var data = await _repository.GetAllApprovisionnement();
            var fournisseurs = await _fournisseurRepo.GetAllFournisseur();
            var produits = await _produitRepo.GetAllProduit();

            foreach (var item in data) Approvisionnements.Add(item);
            foreach (var f in fournisseurs) Fournisseurs.Add(f);
            foreach (var p in produits) Produits.Add(p);
        }
        catch (Exception ex) { MErrorMessage = $"Erreur : {ex.Message}"; }
        finally { IsLoading = false; }
    }
}