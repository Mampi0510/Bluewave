using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BlueWave.Core.Interfaces;
using BlueWave.Core.Models;

namespace BlueWave.ViewModels;

public partial class CommandeViewModel : ViewModelBase
{
    private readonly ICommandeRepository _commandeRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IExportRepository _exportRepository;

    public ObservableCollection<Commande> Commandes { get; } = new();
    public ObservableCollection<Client> Clients { get; } = new();
    public ObservableCollection<Export> Exports { get; } = new();

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private Client? _selectedClient;

    [ObservableProperty]
    private Export? _selectedExport;

    [ObservableProperty]
    private string? _destination;

    [ObservableProperty]
    private Commande? _selectedCommande;

    public CommandeViewModel(ICommandeRepository commandeRepository, IClientRepository clientRepository, IExportRepository exportRepository)
    {
        _commandeRepository = commandeRepository;
        _clientRepository = clientRepository;
        _exportRepository = exportRepository;
    }

    [RelayCommand]
    private async Task Add() => await AddCommandeAsync();

    [RelayCommand]
    private async Task Delete(Commande commande)
    {
        if (commande == null) return;
        await DeleteCommandeAsync(commande);
    }

    public async Task AddCommandeAsync()
    {
        if (SelectedClient == null)
        {
            MErrorMessage = "Veuillez sélectionner un client.";
            return;
        }

        if (SelectedExport == null)
        {
            MErrorMessage = "Veuillez sélectionner un export.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Destination))
        {
            MErrorMessage = "La destination est obligatoire.";
            return;
        }

        try
        {
            var commande = new Commande
            {
                RefClient = SelectedClient.RefClient,
                NumeroExport = SelectedExport.NumeroExport,
                Destination = Destination,
                DateCommande = DateTime.Now
            };

            await _commandeRepository.AddCommande(commande);

            SelectedClient = null;
            SelectedExport = null;
            Destination = string.Empty;
            MErrorMessage = null;

            await LoadDataAsync();
        }
        catch (Exception ex)
        {
            MErrorMessage = $"Erreur : {ex.Message}";
            System.Windows.MessageBox.Show(ex.ToString(), "Erreur AddCommande");
        }
    }

    public async Task DeleteCommandeAsync(Commande commande)
    {
        try
        {
            await _commandeRepository.DeleteCommande(commande);
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
            Commandes.Clear();
            Clients.Clear();
            Exports.Clear();

            var commandes = await _commandeRepository.GetAllCommande();
            var clients = await _clientRepository.GetAllClient();
            var exports = await _exportRepository.GetAllExport();

            foreach (var c in commandes) Commandes.Add(c);
            foreach (var c in clients) Clients.Add(c);
            foreach (var e in exports) Exports.Add(e);
        }
        catch (Exception ex)
        {
            MErrorMessage = $"Erreur chargement : {ex.Message}"; 
            System.Windows.MessageBox.Show(ex.ToString(), "Erreur LoadData");  
        }
        finally
        {
            IsLoading = false;
        }
    }
}