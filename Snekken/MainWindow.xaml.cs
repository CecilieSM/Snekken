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
using Models.Repository;
using Snekken.View;
using Snekken.ViewModel;
using Models;

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

            //create dependencies
            var resourceRepository = new ResourceRepository(ConfigHelper.GetConnectionString());
            var resourceTypeRepository = new ResourceTypeRepository(ConfigHelper.GetConnectionString());
            var personRepository = new PersonRepository(ConfigHelper.GetConnectionString());
            var bookingRepository = new BookingRepository(ConfigHelper.GetConnectionString());

            ResourceViewModel vm = new ResourceViewModel(resourceRepository, resourceTypeRepository);
            BookingViewModel bookingVm = new BookingViewModel(bookingRepository, personRepository, resourceRepository, resourceTypeRepository);

            this.DataContext = vm;

            

        }


        private void RessourcerButton_Click(object sender, RoutedEventArgs e)
        {
            CreateRessource objRessourceWindow = new CreateRessource();
            this.Visibility = Visibility.Hidden;
            objRessourceWindow.DataContext = this.DataContext;

            if (objRessourceWindow.DataContext is ResourceViewModel currentViewModel)
            {
                currentViewModel.RequestClose += (s, e) =>
                {
                    if (e == "AddResource") objRessourceWindow.Close();
                };
            }

            objRessourceWindow.Closed += (s, args) =>
            {
                this.Visibility = Visibility.Visible;
            };

            objRessourceWindow.Show();
        }

        private void BookingButton_Click(object sender, RoutedEventArgs e)
        {

            //var resourceRepository = new ResourceRepository(ConfigHelper.GetConnectionString());
            //var resourceTypeRepository = new ResourceTypeRepository(ConfigHelper.GetConnectionString());
            //var personRepository = new PersonRepository(ConfigHelper.GetConnectionString());
            //var bookingRepository = new BookingRepository(ConfigHelper.GetConnectionString());

            BookingViewAdmin objBookingWindow = new BookingViewAdmin();
            this.Visibility = Visibility.Hidden;
            objBookingWindow.DataContext = this.DataContext;

            if (objBookingWindow.DataContext is BookingViewModel currentViewModel)
            {
                currentViewModel.RequestClose += (s, e) =>
                {
                    if (e == "AddBooking") objBookingWindow.Close();
                };
            }

            objBookingWindow.Closed += (s, args) =>
            {
                this.Visibility = Visibility.Visible;
            };

            objBookingWindow.Show();
        }

    }
}