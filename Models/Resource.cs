using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models;

public class Resource
{
    // PK
    public int Id { get; set; }

    // Data
    public string Title { get; set; } = "";
    public decimal Price { get; set; }           // pris pr. Unit fra ResourceType
    public bool IsActive { get; set; } = true;

    // FK - ResourceType
    public int ResourceTypeId { get; set; }

    public Resource() 
    { 
    }

    public Resource(string title, decimal price, int resourceTypeId, bool isActive = true)
    {
        Title = title;
        Price = price;
        ResourceTypeId = resourceTypeId;
        IsActive = isActive;
    }
}
