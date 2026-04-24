using BlueWave.Data.Context;
using BlueWave.ViewModels;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace BlueWave.Views
{
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetService<DashboardViewModel>();
        }
    }
}