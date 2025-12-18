using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLib.Services;
using WPFLib.Utility;
using WPFLib.ViewModel;

namespace Models
{
    public class TimeSlot : BaseViewModel
    {
        public int ResourceId { get; set; }

        public DateTime Time { get; set; }

        public bool IsAvailable { get; set; } = true;


    }
}
