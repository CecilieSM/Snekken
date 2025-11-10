using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models;

public class Resource
{
    private int _id;              // PK
    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    private string _title = "";
    public string Title
    {
        get { return _title; }
        set { _title = value; }
    }

    private double _price;
    public double Price
    {
        get { return _price; }
        set { _price = value; }
    }

    private bool _isActive = true;
    public bool IsActive
    {
        get { return _isActive; }
        set { _isActive = value; }
    }

    private int _resourceTypeId;
    public int ResourceTypeId
    {
        get { return _resourceTypeId; }
        set { _resourceTypeId = value; }
    }

    public Resource(string title, double price, int resourceTypeId, bool isActive = true)
    {
        this.Title = title;
        this.Price = price;
        this.ResourceTypeId = resourceTypeId;
        this.IsActive = isActive;
    }
}
