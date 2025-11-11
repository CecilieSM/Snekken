using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Models.Repository;

public class ResourceRepository : IRepository<Resource>
{
    private readonly string _connectionString;
    public ResourceRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    // CREATE "Opret ressource"
    public int Add(Resource entity)
    {
        string query = @"
                INSERT INTO RESOURCE (Title, Price, IsActive, ResourceTypeId)
                OUTPUT INSERTED.ResourceId
                VALUES (@Title, @Price, @IsActive, @ResourceTypeId);";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Title", entity.Title);
            command.Parameters.AddWithValue("@Price", entity.Price);
            command.Parameters.AddWithValue("@IsActive", entity.IsActive);
            command.Parameters.AddWithValue("@ResourceTypeId", entity.ResourceTypeId);

            connection.Open();
            int newId = (int)command.ExecuteScalar();  // returnerer det nye ID
            return newId;
        }
    }

    // bygges senere
    public IEnumerable<Resource> GetAll()
    {
        throw new NotImplementedException();
    }

    // bygges senere
    public Resource GetById(int id)
    {
        throw new NotImplementedException();
    }

    // bygges senere
    public void Update(Resource entity)
    {
        throw new NotImplementedException();
    }

    // bygges senere
    public void Delete(int id)
    {
        throw new NotImplementedException();
    }
}
