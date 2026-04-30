using BlueWave.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace BlueWave.Views
{
    public partial class GestionCommerciale : Page
    {
        public ApprovisionnementViewModel ApproVm { get; }
        public CommandeViewModel CommandeVm { get; }

        public GestionCommerciale()
        {
            InitializeComponent();

            ApproVm = App.ServiceProvider.GetRequiredService<ApprovisionnementViewModel>();
            CommandeVm = App.ServiceProvider.GetRequiredService<CommandeViewModel>();

            DataContext = this;

            Loaded += async (_, _) =>
            {
                await ApproVm.LoadDataAsync();
                await CommandeVm.LoadDataAsync();
            };
        }
    }
}