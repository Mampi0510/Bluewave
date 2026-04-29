using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BlueWave.Core.Interfaces;
using BlueWave.Core.Models;

namespace BlueWave.ViewModels;

public partial class ProduitViewModel : ViewModelBase
{
    private readonly IProduitRepository _produitRepository;
    private readonly IStockRepository _stockRepository;

    public ObservableCollection<Produit> Produits { get; } = new();
    public ObservableCollection<Stock> Stocks { get; } = new();

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string? _nomProduit;
    [ObservableProperty] private int _quantite;
    [ObservableProperty] private DateTime _dateReception = DateTime.Now;
    [ObservableProperty] private bool _statut;
    [ObservableProperty] private Stock? _selectedStock;
    [ObservableProperty] private Produit? _selectedProduit;

    public ProduitViewModel(IProduitRepository produitRepository, IStockRepository stockRepository)
    {
        _produitRepository = produitRepository;
        _stockRepository = stockRepository;
    }

    [RelayCommand]
    private async Task Add()
    {
        if (string.IsNullOrWhiteSpace(NomProduit))
        {
            MErrorMessage = "Le nom du produit est obligatoire.";
            return;
        }

        if (SelectedStock == null)
        {
            MErrorMessage = "Veuillez sélectionner un stock.";
            return;
        }

        try
        {
            var produit = new Produit
            {
                NomProduit = NomProduit,
                Quantite = Quantite,
                Date_reception = DateReception,
                Statut = Statut,
                NumeroStock = SelectedStock.NumeroStock
            };

            await _produitRepository.AddProduit(produit);

            NomProduit = string.Empty;
            Quantite = 0;
            DateReception = DateTime.Now;
            Statut = false;
            SelectedStock = null;
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
        catch (Exception ex)
        {
            MErrorMessage = $"Erreur : {ex.Message}";
        }
    }

    public async Task LoadDataAsync()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            Produits.Clear();
            Stocks.Clear();

            var produits = await _produitRepository.GetAllProduit();
            var stocks = await _stockRepository.GetAllStock();

            foreach (var p in produits) Produits.Add(p);
            foreach (var s in stocks) Stocks.Add(s);
        }
        catch (Exception ex)
        {
            MErrorMessage = $"Erreur chargement : {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}