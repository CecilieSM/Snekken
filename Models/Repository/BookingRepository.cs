using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Models.Repository;

public class BookingRepository : IRepository<Booking>
{
    private readonly string _connectionString;
    public BookingRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

    }

    // CREATE - Opret booking
    public int Add(Booking entity)
    {
        string query = @"
                INSERT INTO Booking
                    (ResourceId, PersonId, StartTime, EndTime, RequirementFulfilled, IsPaid)
                OUTPUT INSERTED.Id
                VALUES
                    (@ResourceId, @PersonId, @StartTime, @EndTime, @RequirementFulfilled, @IsPaid);";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@ResourceId", entity.ResourceId);
            command.Parameters.AddWithValue("@PersonId", entity.PersonId);
            command.Parameters.AddWithValue("@StartTime", entity.StartTime);
            command.Parameters.AddWithValue("@EndTime", entity.EndTime);
            command.Parameters.AddWithValue("@RequirementFulfilled", entity.RequirementFulfilled);
            command.Parameters.AddWithValue("@IsPaid", entity.IsPaid);

            connection.Open();
            int newId = (int)command.ExecuteScalar(); // returnerer det nye booking-ID
            return newId;
        }
    }

    // READ ALL - bygges senere
    public IEnumerable<Booking> GetAll()
    {
        List<Booking> booking = new List<Booking>();
        string query = "SELECT * FROM Booking";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    booking.Add(new Booking(
                        (int)reader["ResourceId"],
                        (int)reader["PersonId"],
                        (DateTime)reader["StartTime"],
                        (DateTime)reader["EndTime"],
                        (bool)reader["RequirementFulfilled"],
                        (bool)reader["IsPaid"]
                     ));
                }
            }
        }
        return booking;
    }

    // READ BY ID - bygges senere
    public Booking GetById(int id)
    {
        throw new NotImplementedException();
    }

    // UPDATE - bygges senere
    public void Update(Booking entity)
    {
        throw new NotImplementedException();
    }

    // DELETE - bygges senere
    public void Delete(int id)
    {
        // query to delete booking by id
        // excecute the query 

        throw new NotImplementedException();
    }


}
