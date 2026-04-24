using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using BlueWave.Core.Interfaces;
using BlueWave.Core.Models;

namespace BlueWave.ViewModels;

public partial class FournisseurViewModel : ViewModelBase
{
    private readonly IFournisseurRepository _repository;

    public ObservableCollection<Fournisseur> Fournisseurs { get; } = new();

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty] private int _refFournisseur;
    [ObservableProperty] private string? _nomFournisseur;
    [ObservableProperty] private string? _prenomsFournisseur;
    [ObservableProperty] private string? _telephoneFournisseur;

    public FournisseurViewModel(IFournisseurRepository repository)
    {
        _repository = repository;
    }

    public async Task LoadDataAsync()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            Fournisseurs.Clear();

            var data = await _repository.GetAllFournisseur();
            
            foreach (var item in data)
            {
                Fournisseurs.Add(item);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }
}