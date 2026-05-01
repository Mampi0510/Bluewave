using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BlueWave.Core.Interfaces;
using BlueWave.Core.Models;

namespace BlueWave.ViewModels
{
    public partial class StockViewModel : ViewModelBase
    {
        private readonly IStockRepository _repository;

        public ObservableCollection<Stock> Stocks { get; } = new();
        public ObservableCollection<Approvisionnement> ProduitsDuStock { get; } = new();

        [ObservableProperty] private bool _isLoading;
        [ObservableProperty] private string? _nomStock;
        [ObservableProperty] private bool _isPanelVisible;
        [ObservableProperty] private string? _selectedStockNom;

        private Stock? _selectedStock;
        public Stock? SelectedStock
        {
            get => _selectedStock;
            set
            {
                SetProperty(ref _selectedStock, value);
                if (value != null)
                    _ = LoadProduitsDuStockAsync(value.NumeroStock);
            }
        }

        public StockViewModel(IStockRepository repository)
        {
            _repository = repository;
        }

        private async Task LoadProduitsDuStockAsync(int numeroStock)
        {
            try
            {
                ProduitsDuStock.Clear();
                var appros = await _repository.GetApprovisionnementsByStock(numeroStock);
                foreach (var a in appros)
                    ProduitsDuStock.Add(a);

                SelectedStockNom = Stocks.FirstOrDefault(s => s.NumeroStock == numeroStock)?.NomStock;
                IsPanelVisible = true;
            }
            catch (Exception ex)
            {
                MErrorMessage = $"Erreur : {ex.Message}";
            }
        }

        [RelayCommand]
        private void ClosePanel() => IsPanelVisible = false;

        [RelayCommand]
        private async Task Add()
        {
            if (string.IsNullOrWhiteSpace(NomStock))
            {
                MErrorMessage = "Le Nom est obligatoire.";
                return;
            }
            try
            {
                await _repository.AddStock(new Stock { NomStock = NomStock, Quantite = 0 });
                NomStock = string.Empty;
                MErrorMessage = null;
                await LoadDataAsync();
            }
            catch (Exception ex) { MErrorMessage = $"Erreur : {ex.Message}"; }
        }

        [RelayCommand]
        private async Task Delete(Stock? stock)
        {
            var cible = stock ?? SelectedStock;
            if (cible == null) return;
            try
            {
                await _repository.DeleteStock(cible);
                IsPanelVisible = false;
                await LoadDataAsync();
            }
            catch (Exception ex) { MErrorMessage = $"Erreur : {ex.Message}"; }
        }

        [RelayCommand]
        private void SelectStock(Stock? stock)
        {
            if (stock == null) return;
            SelectedStock = stock;
        }

        public async Task LoadDataAsync()
        {
            if (IsLoading) return;
            try
            {
                IsLoading = true;
                Stocks.Clear();
                var data = await _repository.GetAllStock();
                foreach (var s in data) Stocks.Add(s);
            }
            catch (Exception ex) { MErrorMessage = $"Erreur chargement : {ex.Message}"; }
            finally { IsLoading = false; }
        }
    }
}