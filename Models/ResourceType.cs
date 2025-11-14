using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models;
public class ResourceType
{
    private int _id;                     // PK
    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    private string _title = "";          // fx "båd" "klubhus" osv.
    public string Title
    {
        get { return _title; }
        set { _title = value; }
    }
    private TimeUnit _unit = TimeUnit.None;       // referer til Enum fil "TimeUnit"
    public TimeUnit Unit
    {
        get { return _unit; }
        set { _unit = value; }
    }
    private string _requirement;          // fx "Kranfører påkrævet", "Nøgle udleveres"
    public string Requirement
    {
        get { return _requirement; }
        set { _requirement = value; }
    }

    public ResourceType(string title, TimeUnit unit, string requirement = null)
    {
        this.Title = title;
        this.Unit = unit;
        this.Requirement = requirement;
    }
    public ResourceType(string title, TimeUnit unit, int id, string requirement = null)
    {
        this.Id = id;
        this.Title = title;
        this.Unit = unit;
        this.Requirement = requirement;
    }
}
