using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Snekken.View;

namespace Snekken.View
{
    /// <summary>
    /// Interaction logic for CreateRessource.xaml
    /// </summary>
    public partial class CreateRessource : Window
    {
        public CreateRessource()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateRessourceType createRessourceType = new CreateRessourceType();
            this.Visibility = Visibility.Hidden;
            createRessourceType.DataContext = this.DataContext;

            createRessourceType.Closed += (s, args) =>
            {
                this.Visibility = Visibility.Visible;
            };

            createRessourceType.Show();
        }
    }
}
