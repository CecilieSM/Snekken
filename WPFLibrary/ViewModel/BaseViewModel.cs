using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WPFLibrary.Services;

namespace WPFLibrary.ViewModel
{
    //Abstract fordi BaseViewModel skal ikke instantieres direkte
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        //public readonly IMessageService MessageService;

        //protected BaseViewModel(IMessageService messageService) 
        //{
        //    MessageService = messageService;
        //}

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
