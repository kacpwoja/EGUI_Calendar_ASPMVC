using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EGUI_Calendar.Models
{
    public class EventViewModel
    {
        public string Title { get; set; }
        public string Time { get; set; }
        public string Name { get; set; }
        public bool Edit { get; set; }
        public EventModel Event { get; set; }
    }
}
