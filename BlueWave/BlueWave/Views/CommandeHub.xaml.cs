using BlueWave.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace BlueWave.Views
{
    public partial class CommandeHub : Page
    {
        private readonly CommandeHubViewModel _vm;

        public CommandeHub()
        {
            InitializeComponent();

            _vm = App.ServiceProvider.GetRequiredService<CommandeHubViewModel>();
            DataContext = _vm;

            Loaded += async (_, _) => await _vm.LoadDataAsync();
        }
    }
}