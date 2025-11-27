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

namespace RentalKiosk.View;

    /// <summary>
    /// Interaction logic for BookingView.xaml
    /// </summary>
    public partial class BookingView : Window
{
    public BookingView()
    {
        InitializeComponent();
    }

    // Kaldes når man klikker på en celle i uge-kalenderen
    private void OpenDayView_Click(object sender, RoutedEventArgs e)
    {
        var daySchedule = new DayScheduleView();
        daySchedule.Owner = this;

        // Genbrug samme ViewModel
        daySchedule.DataContext = this.DataContext;

        daySchedule.Show();
    }
}

