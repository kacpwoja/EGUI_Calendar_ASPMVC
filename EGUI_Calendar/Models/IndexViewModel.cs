using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EGUI_Calendar.Controllers
{
    public class IndexViewModel
    {
        public string Title { get; set; }
        public int Days { get; set; }
        public int SelectedDay { get; set; }
        public int[] BusyDays { get; set; }
        public int Offset { get; set; }
    }
}
