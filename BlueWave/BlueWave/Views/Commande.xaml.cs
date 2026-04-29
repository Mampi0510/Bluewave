using BlueWave.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace BlueWave.Views
{
    public partial class Commande : Page
    {
        private readonly CommandeViewModel _vm;

        public Commande()
        {
            InitializeComponent();

            _vm = App.ServiceProvider.GetRequiredService<CommandeViewModel>();
            DataContext = _vm;

            Loaded += async (_, _) => await _vm.LoadDataAsync();
        }
    }
}