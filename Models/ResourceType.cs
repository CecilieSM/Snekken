using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models;
public class ResourceType
{
    // PK
    public int Id { get; set; }

    // Data
    public string Type { get; set; } = "";       // fx "Klublokale", "Båd", "Trailer", "Kranplads" osv.
    public string Unit { get; set; } = "Time";   // Hvordan med Enum??
    public bool Status { get; set; } = true;     // aktiv/inaktiv
    public string? Requirement { get; set; }     // fx "Kranfører påkrævet", "Nøgle udleveres"

    public ResourceType() { }

    public ResourceType(string type, string unit, bool status = true, string? requirement = null)
    {
        Type = type;
        Unit = unit;
        Status = status;
        Requirement = requirement;
    }
}
