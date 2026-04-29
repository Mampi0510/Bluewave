using BlueWave.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace BlueWave.Views
{
    public partial class Fournisseur : Page
    {
        private readonly FournisseurViewModel _vm;

        public Fournisseur()
        {
            InitializeComponent();
            _vm = App.ServiceProvider.GetRequiredService<FournisseurViewModel>();
            DataContext = _vm;

            Loaded += async (_, _) => await _vm.LoadDataAsync();
        }
    }
}