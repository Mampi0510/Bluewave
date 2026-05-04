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

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string? _nomClient;
    [ObservableProperty] private string? _prenomClient;
    [ObservableProperty] private string? _telephone;
    [ObservableProperty] private Client? _selectedClient;

    // Sidebar edit
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private string? _editNomClient;
    [ObservableProperty] private string? _editPrenomClient;
    [ObservableProperty] private string? _editTelephone;
    private int _editingRefClient;

    public ClientViewModel(IClientRepository repository)
    {
        _repository = repository;
    }

    [RelayCommand]
    private async Task Add()
    {
        if (string.IsNullOrWhiteSpace(NomClient)) { MErrorMessage = "Le nom est obligatoire."; return; }
        if (string.IsNullOrWhiteSpace(Telephone)) { MErrorMessage = "Le téléphone est obligatoire."; return; }

        try
        {
            await _repository.AddClient(new Client
            {
                NomClient = NomClient,
                PrenomClient = PrenomClient ?? "",
                Telephone = Telephone
            });

            NomClient = string.Empty;
            PrenomClient = string.Empty;
            Telephone = string.Empty;
            MErrorMessage = null;
            await LoadDataAsync();
        }
        catch (Exception ex) { MErrorMessage = $"Erreur : {ex.Message}"; }
    }

    [RelayCommand]
    private void Edit(Client? client)
    {
        if (client == null) return;
        _editingRefClient = client.RefClient;
        EditNomClient = client.NomClient;
        EditPrenomClient = client.PrenomClient;
        EditTelephone = client.Telephone;
        IsEditing = true;
        MErrorMessage = null;
    }

    [RelayCommand]
    private void CancelEdit()
    {
        IsEditing = false;
        _editingRefClient = 0;
        EditNomClient = null;
        EditPrenomClient = null;
        EditTelephone = null;
    }

    [RelayCommand]
    private async Task SaveEdit()
    {
        if (string.IsNullOrWhiteSpace(EditNomClient)) { MErrorMessage = "Le nom est obligatoire."; return; }
        if (string.IsNullOrWhiteSpace(EditTelephone)) { MErrorMessage = "Le téléphone est obligatoire."; return; }

        try
        {
            await _repository.UpdateClient(new Client
            {
                RefClient = _editingRefClient,
                NomClient = EditNomClient,
                PrenomClient = EditPrenomClient ?? "",
                Telephone = EditTelephone
            });

            IsEditing = false;
            _editingRefClient = 0;
            EditNomClient = null;
            EditPrenomClient = null;
            EditTelephone = null;
            MErrorMessage = null;
            await LoadDataAsync();
        }
        catch (Exception ex) { MErrorMessage = $"Erreur : {ex.Message}"; }
    }

    [RelayCommand]
    private async Task Delete(Client? client)
    {
        var cible = client ?? SelectedClient;
        if (cible == null) return;
        try
        {
            await _repository.DeleteClient(cible);
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
            Clients.Clear();
            var data = await _repository.GetAllClient();
            foreach (var item in data) Clients.Add(item);
        }
        finally { IsLoading = false; }
    }
}