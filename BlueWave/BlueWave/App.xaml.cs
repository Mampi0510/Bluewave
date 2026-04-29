using BlueWave.Core.Interfaces;
using BlueWave.Data.Context;
using BlueWave.Data.Repositories;
using BlueWave.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace BlueWave
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += (s, ex) =>
            {
                MessageBox.Show(ex.ExceptionObject.ToString(), "Unhandled Exception");
            };

            try
            {
                var services = new ServiceCollection();

                services.AddDbContext<AppDbContext>(options =>
                    options.UseMySql(
                        "server=localhost;database=bluewave;user=root;password=",
                        new MySqlServerVersion(new Version(8, 0, 36))
                    )
                );

                // repositories
                services.AddTransient<IClientRepository, ClientRepository>();
                services.AddTransient<ICommandeRepository, CommandeRepository>();
                services.AddTransient<IFournisseurRepository, FournisseurRepository>();
                services.AddTransient<ICommandeRepository, CommandeRepository>();
                services.AddTransient<IExportRepository, ExportRepository>();
                services.AddTransient<IProduitRepository, ProduitRepository>();
                services.AddTransient<IAchatRepository, AchatRepository>();
                services.AddTransient<IApprovisionnementRepository, ApprovisionnementRepository>();
                services.AddTransient<IStockRepository, StockRepository>();

                // viewmodels
                services.AddTransient<StockViewModel>();
                services.AddTransient<DashboardViewModel>();
                services.AddTransient<ClientViewModel>();
                services.AddTransient<CommandeViewModel>();
                services.AddTransient<FournisseurViewModel>();
                services.AddTransient<ProduitViewModel>();
                services.AddTransient<CommandeHubViewModel>();
                services.AddTransient<ExportViewModel>();


                ServiceProvider = services.BuildServiceProvider();

                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Startup Crash");
                Shutdown();
            }
            DispatcherUnhandledException += (s, ex) =>
            {
                MessageBox.Show(ex.Exception.ToString(), "UI Crash");
                ex.Handled = true;
            };
        }
    }
}