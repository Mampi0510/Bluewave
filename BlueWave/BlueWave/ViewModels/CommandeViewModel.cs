using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using BlueWave.Core.Interfaces;
using BlueWave.Core.Models;

namespace BlueWave.ViewModels;

public partial class CommandeViewModel : ViewModelBase
{
    private readonly ICommandeRepository _repository;

    public ObservableCollection<Commande> Commandes { get; } = new();

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty] private int _numeroCommande;
    [ObservableProperty] private int _refClient;
    [ObservableProperty] private int _codeExport;
    [ObservableProperty] private DateTime _dateCommande;
    [ObservableProperty] private string? _destination;

    public CommandeViewModel(ICommandeRepository repository)
    {
        _repository = repository;
    }

    public async Task LoadDataAsync()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            Commandes.Clear();

            var data = await _repository.GetAllCommande();
            
            foreach (var item in data)
            {
                Commandes.Add(item);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }
}