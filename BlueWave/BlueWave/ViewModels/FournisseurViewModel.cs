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

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string? _nomFournisseur;
    [ObservableProperty] private string? _prenomsFournisseur;
    [ObservableProperty] private string? _telephoneFournisseur;
    [ObservableProperty] private Fournisseur? _selectedFournisseur;

    // Sidebar edit
    [ObservableProperty] private bool _isEditing;
    [ObservableProperty] private string? _editNomFournisseur;
    [ObservableProperty] private string? _editPrenomsFournisseur;
    [ObservableProperty] private string? _editTelephoneFournisseur;
    private int _editingRefFournisseur;

    public FournisseurViewModel(IFournisseurRepository repository)
    {
        _repository = repository;
    }

    [RelayCommand]
    private async Task Add()
    {
        if (string.IsNullOrWhiteSpace(NomFournisseur)) { MErrorMessage = "Le nom est obligatoire."; return; }
        if (string.IsNullOrWhiteSpace(TelephoneFournisseur)) { MErrorMessage = "Le téléphone est obligatoire."; return; }

        try
        {
            await _repository.AddFournisseur(new Fournisseur
            {
                NomFournisseur = NomFournisseur,
                PrenomsFournisseur = PrenomsFournisseur ?? "",
                TelephoneFournisseur = TelephoneFournisseur
            });

            NomFournisseur = string.Empty;
            PrenomsFournisseur = string.Empty;
            TelephoneFournisseur = string.Empty;
            MErrorMessage = null;
            await LoadDataAsync();
        }
        catch (Exception ex) { MErrorMessage = $"Erreur : {ex.Message}"; }
    }

    [RelayCommand]
    private void Edit(Fournisseur? fournisseur)
    {
        if (fournisseur == null) return;
        _editingRefFournisseur = fournisseur.RefFournisseur;
        EditNomFournisseur = fournisseur.NomFournisseur;
        EditPrenomsFournisseur = fournisseur.PrenomsFournisseur;
        EditTelephoneFournisseur = fournisseur.TelephoneFournisseur;
        IsEditing = true;
        MErrorMessage = null;
    }

    [RelayCommand]
    private void CancelEdit()
    {
        IsEditing = false;
        _editingRefFournisseur = 0;
        EditNomFournisseur = null;
        EditPrenomsFournisseur = null;
        EditTelephoneFournisseur = null;
    }

    [RelayCommand]
    private async Task SaveEdit()
    {
        if (string.IsNullOrWhiteSpace(EditNomFournisseur)) { MErrorMessage = "Le nom est obligatoire."; return; }
        if (string.IsNullOrWhiteSpace(EditTelephoneFournisseur)) { MErrorMessage = "Le téléphone est obligatoire."; return; }

        try
        {
            await _repository.UpdateFournisseur(new Fournisseur
            {
                RefFournisseur = _editingRefFournisseur,
                NomFournisseur = EditNomFournisseur,
                PrenomsFournisseur = EditPrenomsFournisseur ?? "",
                TelephoneFournisseur = EditTelephoneFournisseur
            });

            IsEditing = false;
            _editingRefFournisseur = 0;
            EditNomFournisseur = null;
            EditPrenomsFournisseur = null;
            EditTelephoneFournisseur = null;
            MErrorMessage = null;
            await LoadDataAsync();
        }
        catch (Exception ex) { MErrorMessage = $"Erreur : {ex.Message}"; }
    }

    [RelayCommand]
    private async Task Delete(Fournisseur? fournisseur)
    {
        var cible = fournisseur ?? SelectedFournisseur;
        if (cible == null) return;
        try
        {
            await _repository.DeleteFournisseur(cible);
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
            Fournisseurs.Clear();
            var data = await _repository.GetAllFournisseur();
            foreach (var item in data) Fournisseurs.Add(item);
        }
        finally { IsLoading = false; }
    }
}