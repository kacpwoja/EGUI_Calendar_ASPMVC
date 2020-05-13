using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EGUI_Calendar.Models
{
    public class EventModel
    {
        public Guid ID { get; set; }
        public TimeSpan Time { get; set; }
        public string Name { get; set; }
    }
}
