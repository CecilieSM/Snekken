using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models;

public class DB
{
    private readonly string _connectionString;

    public DB(string connectionString)
    {
        this._connectionString = connectionString;
    }

    public string Migrate()
    {
        return $"Migration completed";
    }

    public string Truncate()
    {
        return $"Rollback completed";
    }

    public string Seed()
    {
        return $"Seeding completed";
    }
}
