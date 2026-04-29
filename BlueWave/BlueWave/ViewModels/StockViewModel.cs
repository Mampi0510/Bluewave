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

        [ObservableProperty] private bool _isLoading;
        [ObservableProperty] private string? _type;  

        [ObservableProperty] private Stock? _selectedStock;

        public StockViewModel(IStockRepository repository)
        {
            _repository = repository;
        }

        [RelayCommand]
        private async Task Add()
        {
            if (string.IsNullOrWhiteSpace(Type))
            {
                MErrorMessage = "Le type est obligatoire.";
                return;
            }

            try
            {
                var stock = new Stock
                {
                    Type = Type,
                    Quantite = 0  // calculé automatiquement via les produits
                };

                await _repository.AddStock(stock);

                Type = string.Empty;
                MErrorMessage = null;

                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                MErrorMessage = $"Erreur : {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task Delete(Stock? stock)
        {
            var cible = stock ?? SelectedStock;
            if (cible == null) return;

            try
            {
                await _repository.DeleteStock(cible);
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
                Stocks.Clear();

                var data = await _repository.GetAllStock();
                foreach (var s in data)
                    Stocks.Add(s);
            }
            catch (Exception ex)
            {
                MErrorMessage = $"Erreur chargement : {ex.Message}";  // ← affichera l'erreur dans l'UI
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}