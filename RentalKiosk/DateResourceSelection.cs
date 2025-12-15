using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace RentalKiosk
{
    public class DateResourceSelection
    {
        public DateTime Day { get; set; }
        public required Resource Resource { get; set; }
    }
}
