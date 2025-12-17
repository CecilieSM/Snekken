using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models;

public class Booking
{
    private int _bookingId;
    public int BookingId
    {
        get { return _bookingId; }
        set { _bookingId = value; }
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

    private DateTime? _handedOutAt;
    public DateTime? HandedOutAt
    {
        get { return _handedOutAt; }
        set { _handedOutAt = value; }
    }

    private DateTime? _returnedAt;
    public DateTime? ReturnedAt
    {
        get { return _returnedAt; }
        set { _returnedAt = value; }
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

    private decimal _totalPrice;
    public decimal TotalPrice
    {
        get { return _totalPrice; }
        set { _totalPrice = value; }
    }

    //properties til Person, så databasen opdaterer UI'et
    private string _personName = "";
    public string PersonName
    {
        get { return _personName; }
        set { _personName = value; }
    }

    private string _personEmail = "";
    public string PersonEmail
    {
        get { return _personEmail; }
        set { _personEmail = value; }
    }

    private string _personPhone = "";
    public string PersonPhone
    {
        get { return _personPhone; }
        set { _personPhone = value; }
    }


    public Booking(int resourceId, int personId, DateTime startTime, DateTime endTime, decimal totalPrice, bool requirementFulfilled = false, bool isPaid = false)
    {
        this.ResourceId = resourceId;
        this.PersonId = personId;
        this.StartTime = startTime;
        this.EndTime = endTime;
        this.TotalPrice = totalPrice;
        this.RequirementFulfilled = requirementFulfilled;
        this.IsPaid = isPaid;
    }

    public Booking(int bookingId,int resourceId, int personId, DateTime startTime, DateTime endTime, decimal totalPrice, bool requirementFulfilled = false, bool isPaid = false, DateTime? issued = null, DateTime? returned = null)
    {
        this.BookingId = bookingId;
        this.ResourceId = resourceId;
        this.PersonId = personId;
        this.StartTime = startTime;
        this.EndTime = endTime;
        this.TotalPrice = totalPrice;
        this.RequirementFulfilled = requirementFulfilled;
        this.IsPaid = isPaid;
        this.HandedOutAt = issued;
        this.ReturnedAt = returned;
    }
}
