using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repository;

public interface IBookingRepository : IRepository<Booking>
{
    IEnumerable<Booking> GetByResourceId(int resourceId);
    IEnumerable<Booking> GetByPersonId(int personId);
}
