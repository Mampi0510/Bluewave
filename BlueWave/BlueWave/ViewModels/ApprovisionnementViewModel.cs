using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using BlueWave.Core.Interfaces;
using BlueWave.Core.Models;

namespace BlueWave.ViewModels;

public partial class ApprovisionnementViewModel : ViewModelBase
{
    private readonly IApprovisionnementRepository _repository;

    public ObservableCollection<Approvisionnement> Approvisionnements { get; } = new();

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty] private int _idApp;
    [ObservableProperty] private int _refFournisseur;
    [ObservableProperty] private int _codeProduit;
    [ObservableProperty] private string? _certificat;

    public ApprovisionnementViewModel(IApprovisionnementRepository repository)
    {
        _repository = repository;
    }

    public async Task LoadDataAsync()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            Approvisionnements.Clear();

            var data = await _repository.GetAllApprovisionnement();
            
            foreach (var item in data)
            {
                Approvisionnements.Add(item);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }
}