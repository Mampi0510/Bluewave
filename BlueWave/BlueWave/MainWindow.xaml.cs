using System.Windows;
using BlueWave.Views;

namespace BlueWave
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Dashboard());
        }

        private void Dashboard_click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(new Dashboard());

        private void GestionCommerciale_click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(new GestionCommerciale());

        private void Produit_click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(new Produit());

        private void Stock_click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(new Stock());

        private void Annuaire_click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(new Annuaire());
    }
}