using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WPFLib.Services;

namespace WPFLib.ViewModel
{
    //Abstract fordi BaseViewModel skal ikke instantieres direkte


    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        // EVENTS
        public event EventHandler<string>? RequestClose;
        protected void CloseWindowRequested(string message)
        {
            RequestClose?.Invoke(this, message);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


    }
}
