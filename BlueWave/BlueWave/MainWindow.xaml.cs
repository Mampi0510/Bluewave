using System.Windows;
using BlueWave.Views;

namespace BlueWave
{
    public partial class MainWindow : Window
    {
        private Dashboard? _dashboard;
        private GestionCommerciale? _gestionCommerciale;
        private Produit? _produit;
        private Stock? _stock;
        private Annuaire? _annuaire;

        public MainWindow()
        {
            InitializeComponent();
            _dashboard = new Dashboard();
            MainFrame.Navigate(_dashboard);
        }

        private void Dashboard_click(object sender, RoutedEventArgs e)
        {
            _dashboard ??= new Dashboard();
            MainFrame.Navigate(_dashboard);
        }

        private void GestionCommerciale_click(object sender, RoutedEventArgs e)
        {
            _gestionCommerciale ??= new GestionCommerciale();
            MainFrame.Navigate(_gestionCommerciale);
        }

        private void Produit_click(object sender, RoutedEventArgs e)
        {
            _produit ??= new Produit();
            MainFrame.Navigate(_produit);
        }

        private void Stock_click(object sender, RoutedEventArgs e)
        {
            _stock ??= new Stock();
            MainFrame.Navigate(_stock);
        }

        private void Annuaire_click(object sender, RoutedEventArgs e)
        {
            _annuaire ??= new Annuaire();
            MainFrame.Navigate(_annuaire);
        }
    }
}