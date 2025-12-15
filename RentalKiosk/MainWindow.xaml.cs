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
using RentalKiosk.ViewModels;
using Models.Repository;
using Models;

namespace RentalKiosk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            BookingRepository bookingRepository = new BookingRepository(ConfigHelper.GetConnectionString());
            ResourceTypeRepository resourceTypeRepository = new ResourceTypeRepository(ConfigHelper.GetConnectionString());
            ResourceRepository resourceRepository = new ResourceRepository(ConfigHelper.GetConnectionString());
            PersonRepository personRepository = new PersonRepository(ConfigHelper.GetConnectionString());

            MainViewModel vm = new MainViewModel(bookingRepository, resourceTypeRepository, resourceRepository, personRepository);

            this.DataContext = vm;
        }
    }
}