using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Models;

public static class ConfigHelper
{

    public static string GetConnectionString()
    {
        IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
        string? ConnectionString = config.GetConnectionString("DefaultConnection");
        return "ConnectionString";
    }
}
