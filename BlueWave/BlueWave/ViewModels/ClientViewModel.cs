using BlueWave.Core.Interfaces;
using BlueWave.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace BlueWave.ViewModels;

public partial class ClientViewModel : ViewModelBase
{
    private readonly IClientRepository _repository;

    public ObservableCollection<Client> Clients { get; } = new();

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty] private int _refClient;
    [ObservableProperty] private string? _nomClient;
    [ObservableProperty] private string? _prenomClient;
    [ObservableProperty] private string? _telephone;

    [ObservableProperty]
    private Client? _selectedClient;

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
    private async Task Delete()
    {
        if (SelectedClient == null) return;

        await DeleteClientAsync(SelectedClient);
    }

    public async Task AddClientAsync()
    {
        if (string.IsNullOrWhiteSpace(Telephone))
        {
            MErrorMessage = "Le téléphone est obligatoire.";
            return;
        }

        if (string.IsNullOrWhiteSpace(NomClient))
        {
            MErrorMessage = "Le nom est obligatoire.";
            return;
        }
        var client = new Client
        {
            NomClient = NomClient ?? "",
            PrenomClient = PrenomClient ?? "",
            Telephone = Telephone ?? ""
        };

        await _repository.AddClient(client);

        NomClient = string.Empty;
        PrenomClient = string.Empty;
        Telephone = string.Empty;
        MErrorMessage = null;
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