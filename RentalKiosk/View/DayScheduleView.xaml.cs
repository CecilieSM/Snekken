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
/// Interaction logic for DayScheduleView.xaml
/// </summary>

public partial class DayScheduleView : Window
{
    public DayScheduleView()
    {
        InitializeComponent();
    }

    private void TimeSlot_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as System.Windows.Controls.Button;
        string tag = button?.Tag as string ?? "";   // fx "17:00;1"

        // TODO (valgfrit): parse tag'en hvis du vil opdatere SelectedBookingHeader i ViewModel:
        // var parts = tag.Split(';');  // parts[0] = tid, parts[1] = kolonneNr

        var details = new DayBookingView();
        details.Owner = this;

        // Så vi kan Genbruge samme ViewModel
        details.DataContext = this.DataContext;

        details.ShowDialog();
    }
}
