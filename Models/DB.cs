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
    private ResourceTypeRepository _resourceTypeRepository;

    private readonly string _connectionString;

    public DB(string connectionString)
    {
        this._connectionString = connectionString;
        _resourceTypeRepository = new ResourceTypeRepository(connectionString);
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
        _resourceTypeRepository.Add(new ResourceType("Båd", TimeUnit.None, null));
        _resourceTypeRepository.Add(new ResourceType("Lokale", TimeUnit.None, null));
        

        return $"Seeding completed";
    }
}
