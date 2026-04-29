using CommunityToolkit.Mvvm.ComponentModel;

namespace BlueWave.ViewModels;

public abstract partial class ViewModelBase : ObservableValidator
{
    [ObservableProperty]
    private string? _mErrorMessage;
}
