using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BlueWave.Data.Context;
using BlueWave.ViewModels;

namespace BlueWave
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            // 🔗 EF Core - SQL Server
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    "Server=.;Database=BlueWaveDb;Trusted_Connection=True;TrustServerCertificate=True;"
                )
            );

            // 🔗 ViewModels
            services.AddTransient<DashboardViewModel>();

            // 🔗 Build DI container
            ServiceProvider = services.BuildServiceProvider();

            base.OnStartup(e);
        }
    }
}