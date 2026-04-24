using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using BlueWave.Core.Interfaces;
using BlueWave.Core.Models;

namespace BlueWave.ViewModels;

public partial class ExportViewModel : ViewModelBase
{
    private readonly IExportRepository _repository;

    public ObservableCollection<Export> Exports { get; } = new();

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty] private int _numeroExport;
    [ObservableProperty] private int _delai;

    public ExportViewModel(IExportRepository repository)
    {
        _repository = repository;
    }

    public async Task LoadDataAsync()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            Exports.Clear();

            var data = await _repository.GetAllExport();
            
            foreach (var item in data)
            {
                Exports.Add(item);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }
}