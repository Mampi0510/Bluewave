using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BlueWave.Core.Interfaces;
using BlueWave.Core.Models;

namespace BlueWave.ViewModels;

public partial class ProduitViewModel : ViewModelBase
{
    private readonly IProduitRepository _produitRepository;

    public ObservableCollection<Produit> Produits { get; } = new();

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string? _nomProduit;
    [ObservableProperty] private int _prix;
    [ObservableProperty] private bool _statut;
    [ObservableProperty] private Produit? _selectedProduit;

    public ProduitViewModel(IProduitRepository produitRepository)
    {
        _produitRepository = produitRepository;
    }

    [RelayCommand]
    private async Task Add()
    {
        if (string.IsNullOrWhiteSpace(NomProduit))
        {
            MErrorMessage = "Le nom du produit est obligatoire.";
            return;
        }

        try
        {
            var produit = new Produit
            {
                NomProduit = NomProduit,
                Prix = Prix,
                Statut = Statut
            };

            await _produitRepository.AddProduit(produit);

            NomProduit = string.Empty;
            Prix = 0;
            Statut = false;
            MErrorMessage = null;

            await LoadDataAsync();
        }
        catch (Exception ex)
        {
            MErrorMessage = $"Erreur : {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task Delete(Produit? produit)
    {
        var cible = produit ?? SelectedProduit;
        if (cible == null) return;
        try
        {
            await _produitRepository.DeleteProduit(cible);
            await LoadDataAsync();
        }
        catch (Exception ex) { MErrorMessage = $"Erreur : {ex.Message}"; }
    }

    public async Task LoadDataAsync()
    {
        if (IsLoading) return;
        try
        {
            IsLoading = true;
            Produits.Clear();
            var produits = await _produitRepository.GetAllProduit();
            foreach (var p in produits) Produits.Add(p);
        }
        catch (Exception ex) { MErrorMessage = $"Erreur chargement : {ex.Message}"; }
        finally { IsLoading = false; }
    }
}