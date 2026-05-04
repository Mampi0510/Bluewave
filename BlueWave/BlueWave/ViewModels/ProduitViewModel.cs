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
    [ObservableProperty] private Produit? _selectedProduit;

    // Sidebar edit
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private string? _editNomProduit;
    [ObservableProperty] private int _editPrix;
    private int _editingCodeProduit;

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
            await _produitRepository.AddProduit(new Produit
            {
                NomProduit = NomProduit,
                Prix = Prix
            });

            NomProduit = string.Empty;
            Prix = 0;
            MErrorMessage = null;
            await LoadDataAsync();
        }
        catch (Exception ex) { MErrorMessage = $"Erreur : {ex.Message}"; }
    }

    [RelayCommand]
    private void Edit(Produit? produit)
    {
        if (produit == null) return;
        _editingCodeProduit = produit.CodeProduit;
        EditNomProduit = produit.NomProduit;
        EditPrix = produit.Prix;
        IsEditing = true;
        MErrorMessage = null;
    }

    [RelayCommand]
    private void CancelEdit()
    {
        IsEditing = false;
        _editingCodeProduit = 0;
        EditNomProduit = null;
        EditPrix = 0;
    }

    [RelayCommand]
    private async Task SaveEdit()
    {
        if (string.IsNullOrWhiteSpace(EditNomProduit))
        {
            MErrorMessage = "Le nom est obligatoire.";
            return;
        }
        try
        {
            await _produitRepository.UpdateProduit(new Produit
            {
                CodeProduit = _editingCodeProduit,
                NomProduit = EditNomProduit,
                Prix = EditPrix
            });

            IsEditing = false;
            _editingCodeProduit = 0;
            EditNomProduit = null;
            EditPrix = 0;
            MErrorMessage = null;
            await LoadDataAsync();
        }
        catch (Exception ex) { MErrorMessage = $"Erreur : {ex.Message}"; }
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
        catch (Exception ex) { MErrorMessage = $"Erreur : {ex.Message}"; }
        finally { IsLoading = false; }
    }
}