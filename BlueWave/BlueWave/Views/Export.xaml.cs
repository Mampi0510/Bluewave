using BlueWave.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace BlueWave.Views
{
    public partial class Export : Page
    {
        private readonly ExportViewModel _vm;

        public Export()
        {
            InitializeComponent();
            _vm = App.ServiceProvider.GetRequiredService<ExportViewModel>();
            DataContext = _vm;
            Loaded += async (_, _) => await _vm.LoadDataAsync();
        }
    }
}