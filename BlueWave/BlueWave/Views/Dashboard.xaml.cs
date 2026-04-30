using BlueWave.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace BlueWave.Views
{
    public partial class Dashboard : Page
    {
        private readonly DashboardViewModel _vm;

        public Dashboard()
        {
            InitializeComponent();
            _vm = App.ServiceProvider.GetRequiredService<DashboardViewModel>();
            DataContext = _vm;
            Loaded += async (_, _) => await _vm.LoadDataAsync();
        }
    }
}