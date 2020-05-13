using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EGUI_Calendar.Models
{
    public class DayViewModel
    {
        public string TitleShort { get; set; }
        public string TitleLong { get; set; }
        public EventModel[] Events { get; set; }
    }
}
