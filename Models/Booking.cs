using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models;

public class Booking
{
    // PK
    public int Id { get; set; }

    // Tidsrum for booking
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    // Statusser 
    public bool RequirementStatus { get; set; }  // opfyldt/ikke opfyldt
    public bool PaymentStatus { get; set; }      // depositum betalt/ikke betalt

    // FK’er
    public int ResourceId { get; set; }         
    public int PersonId { get; set; }           

    public Booking() 
    { 
    }

    public Booking(int resourceId, int personId, DateTime startTime, DateTime endTime, bool requirementStatus = false, bool paymentStatus = false)
    {
        ResourceId = resourceId;
        PersonId = personId;
        StartTime = startTime;
        EndTime = endTime;
        RequirementStatus = requirementStatus;
        PaymentStatus = paymentStatus;
    }
}
