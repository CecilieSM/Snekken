using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Models.Repository
{
    public class PersonRepository : IRepository<Person>
    {
        public ObservableCollection<Person> _persons = new ObservableCollection<Person>();

        private readonly string _connectionString;
        public PersonRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public int Add(Person person)
        {
            string query = @"
                INSERT INTO PERSON (Name, Email, Phone)
                OUTPUT INSERTED.PersonId
                VALUES (@Name, @Email, @Phone);";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", person.Name);
                command.Parameters.AddWithValue("@Email", person.Phone);
                command.Parameters.AddWithValue("@Phone", person.Email);

                connection.Open();
                int newId = (int)command.ExecuteScalar();
                return newId;
            }
        }

        public IEnumerable<Person> GetAll()
        {
            var persons = new List<Person>();
            string query = "SELECT * FROM Person";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        persons.Add(new Person(
                            (int)reader["PersonId"],
                            (string)reader["Name"],
                            (string)reader["Email"],
                            (string)reader["Phone"]
                         ));
                    }
                }
            }
            return persons;
        }

        public Person GetById(int id)
        {
            return _persons.FirstOrDefault(p => p.Id == id);
        }

        public void Update(Person person)
        {
            var existingPerson = GetById(person.Id);
            if (existingPerson != null)
            {
                existingPerson.Name = person.Name;
            }
        }

        public void Delete(int id)
        {
            var person = GetById(id);
            if (person != null)
            {
                _persons.Remove(person);
            }
        }
    }
}
