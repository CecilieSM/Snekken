using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Models.Repository;

namespace Models;

public class DB
{
    private ResourceRepository _resourceRepository;
    private ResourceTypeRepository _resourceTypeRepository;
    private PersonRepository _personRepository;
    private BookingRepository _bookingRepository;
    private readonly string _connectionString;

    public DB(string connectionString)
    {
        this._connectionString = connectionString;
        _resourceRepository = new ResourceRepository(connectionString);
        _resourceTypeRepository = new ResourceTypeRepository(connectionString);
        _bookingRepository = new BookingRepository(connectionString);
        _personRepository = new PersonRepository(connectionString);
    }

    public string Migrate()
    {
        int numbRows = 0;
        // Henter sql filen til en string
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Schema", "Schema.sql");
        string query = File.ReadAllText(path);

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            numbRows = command.ExecuteNonQuery();
        }

        return $"Migration completed: Number of affected rows: {numbRows}";
    }

    public string Truncate()
    {
        int numbRows = 0;
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Schema", "Truncate.sql");
        string query = File.ReadAllText(path);
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            numbRows = command.ExecuteNonQuery();
        }
        return $"Rollback completed: Number of affected rows: {numbRows}";
    }

    public string Seed()
    {
        _resourceTypeRepository.Add(new ResourceType("Båd", TimeUnit.None, "Har bådcertifikat"));
        _resourceTypeRepository.Add(new ResourceType("Lokale", TimeUnit.None));
        _resourceRepository.Add(new Resource("Storlokale", 550.00, 2, "20 personer"));
        _resourceRepository.Add(new Resource("Mødelokale", 350.00, 2, "10 personer"));
        _resourceRepository.Add(new Resource("Jolle", 400.00, 1, "SS1 16 fod"));
        _personRepository.Add(new Person("Claus Hansen", "claus@hotmil.com"));
        _personRepository.Add(new Person("Dorte Jensen", "dorte@gmail.com"));
        _bookingRepository.Add(new Booking(1, 1, DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddHours(2)));
        _bookingRepository.Add(new Booking(2, 2, DateTime.Now.AddDays(3), DateTime.Now.AddDays(3).AddHours(5)));
        _bookingRepository.Add(new Booking(3, 2, DateTime.Now.AddDays(6), DateTime.Now.AddDays(6).AddHours(1)));

        return $"Seeding completed";
    }
}
