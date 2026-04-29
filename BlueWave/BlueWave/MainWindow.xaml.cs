using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BlueWave.Views;

namespace BlueWave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Dashboard());
        }

        private void Dashboard_click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Dashboard());
        }
        private void Commande_click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Commande());
        }
        private void Client_click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Client());
        }
      private void Fournisseur_click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Fournisseur());
        }
       private void Produit_click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Produit());
        }
       private void Stock_click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Stock());
        }
        private void Export_click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Export());
        }
    }
}