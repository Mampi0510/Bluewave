using BlueWave.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace BlueWave.Views
{
    public partial class Produit : Page
    {
        private readonly ProduitViewModel _vm;

        public Produit()
        {
            InitializeComponent();

            _vm = App.ServiceProvider.GetRequiredService<ProduitViewModel>();
            DataContext = _vm;

            Loaded += async (_, _) => await _vm.LoadDataAsync();
        }
    }
}