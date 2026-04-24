using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using BlueWave.Core.Interfaces;
using BlueWave.Core.Models;
using CommunityToolkit.Mvvm.Input;

namespace BlueWave.ViewModels;

public partial class ClientViewModel : ViewModelBase
{
    private readonly IClientRepository _repository;

    public ObservableCollection<Client> Clients { get; } = new();

    [ObservableProperty]
    private bool _isLoading;
    private Client? selectedClient;

    [ObservableProperty] private int _refClient;
    [ObservableProperty] private string? _nomClient;
    [ObservableProperty] private string? _prenomClient;
    [ObservableProperty] private string? _telephone;

    public ClientViewModel(IClientRepository repository)
    {
        _repository = repository;
    }

    [RelayCommand]
    private async Task Add()
    {
        await AddClientAsync();
    }

    [RelayCommand]
    private async Task Delete(Client client)
    {
        await DeleteClientAsync(client);
    }

    public async Task AddClientAsync()
    {
        var client = new Client
        {
            NomClient = NomClient,
            PrenomClient = PrenomClient,
            Telephone = Telephone
        };

        await _repository.AddClient(client);
        await LoadDataAsync();
    }

    public async Task DeleteClientAsync(Client client)
    {
        await _repository.DeleteClient(client);
        await LoadDataAsync();
    }

    public async Task UpdateClientAsync(Client client)
    {
        await _repository.UpdateClient(client);
        await LoadDataAsync();
    }

    public async Task LoadDataAsync()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            Clients.Clear();

            var data = await _repository.GetAllClient();
            
            foreach (var item in data)
            {
                Clients.Add(item);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }
}