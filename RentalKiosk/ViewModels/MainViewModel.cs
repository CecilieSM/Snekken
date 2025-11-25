using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLib.Services;
using WPFLib.Utility;
using WPFLib.ViewModel;
using System.Windows.Input;
using System.Windows;

namespace RentalKiosk.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand AddSomething { get; }

        public MainViewModel() 
        {
            MessageService.Show("Hello from MainViewModel!");
            MessageService.Log("MainViewModel initialized." + DateTime.Now);

            AddSomething = new RelayCommand(ExecuteAddSomething, CanAddSomething);
        }

        public void ExecuteAddSomething(object parameter) 
        {
            MessageService.Show("AddSomething command executed!");
            MessageService.Log("AddSomething command executed at " + DateTime.Now);
        }

        public bool CanAddSomething() 
        {
            // For demonstration, always return true
            return true;
        }

    }
}
