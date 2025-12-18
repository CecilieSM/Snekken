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
        //public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        //public TimeSpan EndTime { get; set; }
        public bool IsAvailable { get; set; } = true;

        private bool _isSelected;
        //public bool IsSelected 
        //{
        //    get => _isSelected;
        //    set
        //    {
        //        _isSelected = value;
        //        OnPropertyChanged();
        //    }
        //}
    }
}
