using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Snekken.Utility;
using Snekken.Services;

namespace Snekken.ViewModel;

public class BaseViewModel : INotifyPropertyChanged
{
    public MessageService _messageService = new MessageService();
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}


