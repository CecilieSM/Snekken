using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Snekken.Services
{
    public class MessageService
    {
        public void Show(string message)
        {            
            MessageBox.Show(message);
        }
    }
}
