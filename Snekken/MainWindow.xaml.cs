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
using Snekken.View;

namespace Snekken
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void RessourcerButton_Click(object sender, RoutedEventArgs e)
        {
            CreateRessource objRessourceWindow = new CreateRessource();
            this.Visibility = Visibility.Hidden;
            objRessourceWindow.Show();
        }
    }
}