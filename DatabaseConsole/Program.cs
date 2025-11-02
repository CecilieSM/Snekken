using System;
using System.Security.Cryptography.X509Certificates;

namespace DatabaseConsole;

internal class Program
{
    static void Main(string[] args)
    {
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
        Console.WriteLine("Running migration...");
        // TODO: Call migration logic from model library
        Pause();
    }

    static void RollbackMigration()
    {
        Console.WriteLine("Rolling back database...");
        // TODO: Call rollback logic
        Pause();
    }

    static void SeedDatabase()
    {
        Console.WriteLine("Seeding database...");
        // TODO: Call seeding logic
        Pause();
    }

    static void Pause()
    {
        Console.WriteLine("\nOperation complete. Press any key to return to menu.");
        Console.ReadKey();
    }


}
