using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
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

    [ObservableProperty]
    private string? _nomFournisseur;

    [ObservableProperty]
    private string? _prenomsFournisseur;

    [ObservableProperty]
    private string? _telephoneFournisseur;

    [ObservableProperty]
    private Fournisseur? _selectedFournisseur;

    public FournisseurViewModel(IFournisseurRepository repository)
    {
        _repository = repository;
    }

    [RelayCommand]
    private async Task Add() => await AddFournisseurAsync();

    [RelayCommand]
    private async Task Delete()
    {
        if (SelectedFournisseur == null) return;
        await DeleteFournisseurAsync(SelectedFournisseur);
    }

    public async Task AddFournisseurAsync()
    {
        if (string.IsNullOrWhiteSpace(NomFournisseur))
        {
            MErrorMessage = "Le nom est obligatoire.";
            return;
        }

        if (string.IsNullOrWhiteSpace(TelephoneFournisseur))
        {
            MErrorMessage = "Le téléphone est obligatoire.";
            return;
        }

        var fournisseur = new Fournisseur
        {
            NomFournisseur = NomFournisseur,
            PrenomsFournisseur = PrenomsFournisseur ?? "",
            TelephoneFournisseur = TelephoneFournisseur
        };

        await _repository.AddFournisseur(fournisseur);

        // Vider les champs après ajout
        NomFournisseur = string.Empty;
        PrenomsFournisseur = string.Empty;
        TelephoneFournisseur = string.Empty;
        MErrorMessage = null;

        await LoadDataAsync();
    }

    public async Task DeleteFournisseurAsync(Fournisseur fournisseur)
    {
        await _repository.DeleteFournisseur(fournisseur);
        await LoadDataAsync();
    }

    public async Task UpdateFournisseurAsync(Fournisseur fournisseur)
    {
        await _repository.UpdateFournisseur(fournisseur);
        await LoadDataAsync();
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
                Fournisseurs.Add(item);
        }
        finally
        {
            IsLoading = false;
        }
    }
}