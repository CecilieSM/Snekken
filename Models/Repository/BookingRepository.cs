using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using WPFLib.Services;

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
                    (ResourceId, PersonId, StartTime, EndTime, TotalPrice, RequirementFulfilled, IsPaid)
                VALUES
                    (@ResourceId, @PersonId, @StartTime, @EndTime, @TotalPrice, @RequirementFulfilled, @IsPaid);
                SELECT CAST(SCOPE_IDENTITY() AS int);";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@ResourceId", entity.ResourceId);
            command.Parameters.AddWithValue("@PersonId", entity.PersonId);
            command.Parameters.AddWithValue("@StartTime", entity.StartTime);
            command.Parameters.AddWithValue("@EndTime", entity.EndTime);
            command.Parameters.AddWithValue("@TotalPrice", entity.TotalPrice);
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
                    (int)reader["BookingId"],
                    (int)reader["ResourceId"],
                    (int)reader["PersonId"],
                    (DateTime)reader["StartTime"],
                    (DateTime)reader["EndTime"],
                    (decimal)reader["TotalPrice"],
                    (bool)reader["RequirementFulfilled"],
                    (bool)reader["IsPaid"],
                    reader["HandedOutAt"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["HandedOutAt"],
                    reader["ReturnedAt"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["ReturnedAt"]
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
        string query = @"
            UPDATE Booking
            SET 
                StartTime = @StartTime,
                EndTime = @EndTime,
                TotalPrice = @TotalPrice,
                RequirementFulfilled = @RequirementFulfilled,
                IsPaid = @IsPaid,
                HandedOutAt = @HandedOutAt,
                ReturnedAt = @ReturnedAt
            WHERE BookingId = @BookingId;";
        using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@StartTime", entity.StartTime);
            command.Parameters.AddWithValue("@EndTime", entity.EndTime);
            command.Parameters.AddWithValue("@TotalPrice", entity.TotalPrice);
            command.Parameters.AddWithValue("@RequirementFulfilled", entity.RequirementFulfilled);
            command.Parameters.AddWithValue("@IsPaid", entity.IsPaid);
            command.Parameters.AddWithValue("@HandedOutAt", (object?)entity.HandedOutAt ?? DBNull.Value);
            command.Parameters.AddWithValue("@ReturnedAt", (object?)entity.ReturnedAt ?? DBNull.Value);
            command.Parameters.AddWithValue("@BookingId", entity.BookingId);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    // DELETE - bygges senere
    public void Delete(int id)
    {
        string query = "DELETE FROM Booking WHERE BookingId = @BookingId;";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@BookingId", id);
            connection.Open();
            command.ExecuteNonQuery();

        }
    }

}
