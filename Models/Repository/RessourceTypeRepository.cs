using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Models.Repository;

public class ResourceTypeRepository : IRepository<ResourceType>
{
    private readonly string _connectionString;

    public ResourceTypeRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    // CREATE - Opret ResourceType)
    public int Add(ResourceType entity)
    {
        string query = @"
                INSERT INTO RESOURCETYPE (Title, Unit, Requirement)
                OUTPUT INSERTED.ResourceTypeId
                VALUES (@Title, @Unit, @Requirement);";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Title", entity.Title);
            command.Parameters.AddWithValue("@Unit", ((int)entity.Unit));   
            command.Parameters.AddWithValue("@Requirement",
                (object?)entity.Requirement ?? DBNull.Value);

            connection.Open();
            int newId = (int)command.ExecuteScalar();  // returnerer det nye ResourceTypeId
            return newId;
        }
    }

    // READ ALL - bygges senere
    public IEnumerable<ResourceType> GetAll()
    {
        var resourceTypes = new List<ResourceType>();
        string query = "SELECT * FROM ResourceType";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    resourceTypes.Add(new ResourceType(
                        (string)reader["Title"],
                        (TimeUnit)reader["Unit"],
                        (string)reader["Requirement"]
                        ));

                }
           
            }

        }
     
        return resourceTypes;
    }

    // READ BY ID - bygges senere
    public ResourceType GetById(int id)
    {
        throw new NotImplementedException();
    }

    // UPDATE - bygges senere
    public void Update(ResourceType entity)
    {
        throw new NotImplementedException();
    }

    // DELETE - bygges senere
    public void Delete(int id)
    {
        throw new NotImplementedException();
    }
}

