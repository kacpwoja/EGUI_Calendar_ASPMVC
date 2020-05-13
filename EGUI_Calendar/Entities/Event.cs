using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EGUI_Calendar.Entities
{
    public class Event
    {
        public Guid ID { get; set; }
        public DateTime Time { get; set; }
        public string Name { get; set; }
    }
}
