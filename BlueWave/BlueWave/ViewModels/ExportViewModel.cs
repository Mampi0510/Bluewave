using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BlueWave.Core.Interfaces;
using BlueWave.Core.Models;

namespace BlueWave.ViewModels
{
    public partial class ExportViewModel : ViewModelBase
    {
        private readonly IExportRepository _repository;

        public ObservableCollection<Export> Exports { get; } = new();

        [ObservableProperty] private bool _isLoading;
        [ObservableProperty] private string _delai;
        [ObservableProperty] private string? _statut;
        [ObservableProperty] private Export? _selectedExport;

        public ExportViewModel(IExportRepository repository)
        {
            _repository = repository;
        }

        [RelayCommand]
        private async Task Add()
        {
            if (!int.TryParse(Delai, out int delaiInt) || delaiInt <= 0)
            {
                MErrorMessage = "Le délai doit être un nombre supérieur à 0.";
                return;
            }
            if (string.IsNullOrWhiteSpace(Statut)) { MErrorMessage = "Le statut est obligatoire."; return; }

            try
            {
                await _repository.AddExport(new Export { Delai = delaiInt, Statut = Statut });
                Delai = string.Empty;
                Statut = string.Empty;
                MErrorMessage = null;
                await LoadDataAsync();
            }
            catch (Exception ex) { MErrorMessage = $"Erreur : {ex.Message}"; }
        }

        [RelayCommand]
        private async Task Delete(Export? export)
        {
            var cible = export ?? SelectedExport;
            if (cible == null) return;
            try { await _repository.DeleteExport(cible); await LoadDataAsync(); }
            catch (Exception ex) { MErrorMessage = $"Erreur : {ex.Message}"; }
        }

        public async Task LoadDataAsync()
        {
            if (IsLoading) return;
            try
            {
                IsLoading = true;
                Exports.Clear();
                var data = await _repository.GetAllExport();
                foreach (var e in data) Exports.Add(e);
            }
            catch (Exception ex) { MErrorMessage = $"Erreur : {ex.Message}"; }
            finally { IsLoading = false; }
        }
        public List<string> Statuts { get; } = new()
        {
           "En cours", "Expédié"
        };
    }
}