using BlueWave.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace BlueWave.Views
{
    public partial class Client : Page
    {
        private readonly ClientViewModel _vm;

        public Client()
        {
            InitializeComponent();

            _vm = App.ServiceProvider.GetRequiredService<ClientViewModel>();
            DataContext = _vm;

            Loaded += async (_, _) => await _vm.LoadDataAsync();
        }
    }
}