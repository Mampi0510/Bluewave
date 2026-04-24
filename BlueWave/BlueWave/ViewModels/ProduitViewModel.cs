using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using BlueWave.Core.Interfaces;
using BlueWave.Core.Models;

namespace BlueWave.ViewModels;

public partial class ProduitViewModel : ViewModelBase
{
    private readonly IProduitRepository _repository;

    public ObservableCollection<Produit> Produits { get; } = new();

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty] private int _codeProduit;
    [ObservableProperty] private int _numeroStock;
    [ObservableProperty] private string? _nomProduit;
    [ObservableProperty] private int _quantite;
    [ObservableProperty] private DateTime _dateReception;
    [ObservableProperty] private bool _statut;

    public ProduitViewModel(IProduitRepository repository)
    {
        _repository = repository;
    }

    public async Task LoadDataAsync()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            Produits.Clear();

            var data = await _repository.GetAllProduit();
            
            foreach (var item in data)
            {
                Produits.Add(item);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }
}