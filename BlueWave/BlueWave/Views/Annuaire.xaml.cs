using BlueWave.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace BlueWave.Views
{
    public partial class Annuaire : Page
    {
        public ClientViewModel ClientVm { get; }
        public FournisseurViewModel FournisseurVm { get; }

        public Annuaire()
        {
            InitializeComponent();

            ClientVm = App.ServiceProvider.GetRequiredService<ClientViewModel>();
            FournisseurVm = App.ServiceProvider.GetRequiredService<FournisseurViewModel>();

            DataContext = this;

            Loaded += async (_, _) =>
            {
                await ClientVm.LoadDataAsync();
                await FournisseurVm.LoadDataAsync();
            };
        }
    }
}