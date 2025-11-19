using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Models.Repository
{
    public class MessageService
    {
        public void Show(string message)
        {
            // Real UI dialog (WPF / WinForms)
            MessageBox.Show(message);
        }
    }
}
