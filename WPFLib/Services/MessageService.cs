using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace WPFLib.Services;

public static class MessageService
{
    public static void Show(string message)
    {
        MessageBox.Show(message);
    }

    public static void Log(string message) 
    {
        string path = "log.txt";

        if (!File.Exists(path)) 
        {
            File.Create(path).Dispose();
        }

        using (StreamWriter writer = new StreamWriter(path, append: true))
        {
            writer.WriteLine(message);
        }
    }
}

