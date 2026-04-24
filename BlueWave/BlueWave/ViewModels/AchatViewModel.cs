using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using BlueWave.Core.Interfaces;
using BlueWave.Core.Models; 

namespace BlueWave.ViewModels;

public partial class AchatViewModel : ViewModelBase
{
    private readonly IAchatRepository _achatRepository;

    
    public ObservableCollection<Achat> Achats { get; } = new();

    [ObservableProperty]
    private bool _isLoading;

    
    [ObservableProperty] private int _idAchat;
    [ObservableProperty] private int _codeProduit;
    [ObservableProperty] private int _numeroCommande;
    [ObservableProperty] private int _quantite;

    public AchatViewModel(IAchatRepository achatRepository)
    {
        _achatRepository = achatRepository;
    }

    
    public async Task LoadDataAsync()
    {
        if (IsLoading) return;

        try
        {
            IsLoading = true;
            Achats.Clear();

            var data = await _achatRepository.GetAllAchat();
            
            foreach (var item in data)
            {
                Achats.Add(item);
            }
        }
        finally
        {
            IsLoading = false;
        }
    }
}