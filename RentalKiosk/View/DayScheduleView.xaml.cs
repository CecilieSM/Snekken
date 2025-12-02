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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RentalKiosk.View;

/// <summary>
/// Interaction logic for DayScheduleView.xaml
/// </summary>
public partial class DayScheduleView : UserControl
{
    public DayScheduleView()
    {
        InitializeComponent();
    }

    private void TimeSlot_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Senere – sæt SelectedTimeRange / SelectedStartTime i ViewModel
        // og aktiver formularen i BookingFormView.
    }
}
