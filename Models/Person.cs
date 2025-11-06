using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models;
public class Person
{
    // PK
    public int Id { get; set; }

    // Data
    public string Name { get; set; } = "";
    public string? Mobile { get; set; }          // string, ikke int pga. telefonnummer 
    public string Email { get; set; } = "";

    public Person() { }

    public Person(string name, string email, string? mobile = null)
    {
        Name = name;
        Email = email;
        Mobile = mobile;
    }
}
