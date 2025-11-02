using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using Models;


namespace DatabaseConsole;

internal class Program
{

    private static DB db;
    static void Main(string[] args)
    {
        string connectionString = ConfigHelper.GetConnectionString();
        db = new DB(connectionString);
    
        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("=== Database Migration Tool ===");
            Console.WriteLine("1. Run Migration");
            Console.WriteLine("2. Rollback Migration");
            Console.WriteLine("Q. Quit");
            Console.Write("\nSelect an option: ");

            char pressedKey = Console.ReadKey().KeyChar;
            Console.Clear();

            switch (pressedKey)
            {
                case '1': RunMigration(); break;
                case '2': RollbackMigration(); break;
                case '3': SeedDatabase(); break;
                case 'q': running = false; break;
                default:
                    Console.Clear();
                    Console.WriteLine("Action not recognized.");
                    Pause();
                    break;
            }
        }
    }

    static void RunMigration()
    {
        string response = db.Migrate();
        Console.WriteLine(response);
        Pause();
    }

    static void RollbackMigration()
    {
        string response = db.Truncate();
        Console.WriteLine(response);
        Pause();
    }

    static void SeedDatabase()
    {
        string response = db.Seed();
        Console.WriteLine(response);
        Pause();
    }

    static void Pause()
    {
        Console.WriteLine("\nOperation complete. Press any key to return to menu.");
        Console.ReadKey();
    }


}
