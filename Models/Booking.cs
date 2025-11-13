using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models;

public class Booking
{
    private int _id;
    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    private DateTime _startTime;
    public DateTime StartTime
    {
        get { return _startTime; }
        set { _startTime = value; }
    }

    private DateTime _endTime;
    public DateTime EndTime
    {
        get { return _endTime; }
        set { _endTime = value; }
    }

    private bool _requirementFulfilled;
    public bool RequirementFulfilled
    {
        get { return _requirementFulfilled; }
        set { _requirementFulfilled = value; }
    }

    private bool _isPaid;
    public bool IsPaid
    {
        get { return _isPaid; }
        set { _isPaid = value; }
    }

    private int _resourceId;
    public int ResourceId
    {
        get { return _resourceId; }
        set { _resourceId = value; }
    }

    private int _personId;
    public int PersonId
    {
        get { return _personId; }
        set { _personId = value; }
    }

    public Booking(int resourceId, int personId, DateTime startTime, DateTime endTime, bool requirementFulfilled = false, bool isPaid = false)
    {
        this.ResourceId = resourceId;
        this.PersonId = personId;
        this.StartTime = startTime;
        this.EndTime = endTime;
        this.RequirementFulfilled = requirementFulfilled;
        this.IsPaid = isPaid;
    }
}
