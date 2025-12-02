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
/// Interaction logic for BookingView.xaml
/// </summary>
public partial class BookingView : UserControl
{
    public BookingView()
    {
        InitializeComponent();
    }

    // Lige nu bare en dummy – logik kommer senere
    private void OpenDayView_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        // Her kan vi i fremtiden fortælle ViewModel:
        // "dag X og ressource-kolonne Y er valgt"
    }
}

