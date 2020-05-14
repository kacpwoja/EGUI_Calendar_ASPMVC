using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EGUI_Calendar.Models;
using EGUI_Calendar.Entities;

namespace EGUI_Calendar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EventsContext events;

        public HomeController(ILogger<HomeController> logger, EventsContext events)
        {
            _logger = logger;
            this.events = events;
        }

        public IActionResult Index(int? year, int? month)
        {
            if (!year.HasValue || !month.HasValue) return DefaultRedirect();

            IndexViewModel vm;

            try
            {
                var date = new DateTime(year.Value, month.Value, 1);
                var isCurrent = DateTime.Now.Year == year.Value && DateTime.Now.Month == month.Value;

                vm = new IndexViewModel
                {
                    Title = date.ToString("MMMM yyyy"),
                    Offset = (int)date.DayOfWeek - 1,
                    Days = DateTime.DaysInMonth(year.Value, month.Value),
                    Today = isCurrent ? DateTime.Now.Day : 0,
                    BusyDays = events.GetBusyDays(year.Value, month.Value)
                };
            }
            catch
            {
                return DefaultRedirect();
            }
            return View(vm);
        }

        public IActionResult Info()
        {
            return View();
        }

        public IActionResult Event(int? year, int? month, int? day, Guid? id)
        {
            if (!year.HasValue || !month.HasValue || !day.HasValue) return DefaultRedirect();
            var edit = id.HasValue;

            EventViewModel vm;

            try
            {
                var date = new DateTime(year.Value, month.Value, day.Value);
                var ev = edit ? events.GetEvent(year.Value, month.Value, day.Value, id.Value) : null;

                vm = new EventViewModel
                {
                    Edit = edit,
                    Title = edit ? ev.Name : "New Event",
                    Time = edit ? ev.Time.ToString(@"HH\:mm") : "00:00",
                    Name = edit ? ev.Name : ""
                };
            }
            catch
            {
                return DefaultRedirect();
            }
            return View(vm);
        }

        public IActionResult Day(int? year, int? month, int? day)
        {
            if (!year.HasValue || !month.HasValue || !day.HasValue) return DefaultRedirect();

            DayViewModel vm;

            try
            {
                var date = new DateTime(year.Value, month.Value, day.Value);
                var ev = events.GetEvents(year.Value, month.Value, day.Value);

                vm = new DayViewModel
                {
                    TitleLong = date.ToString("dddd, d MMMM yyyy"),
                    TitleShort = date.ToString("d MMM yyyy"),
                    Events = ev.Select(o => new EventModel
                    {
                        ID = o.ID,
                        Time = o.Time.TimeOfDay,
                        Name = o.Name
                    }).OrderBy(o => o.Time).ToArray()
                };
            }
            catch
            {
                return DefaultRedirect();
            }

            return View(vm);
        }

        [HttpPost]
        public IActionResult New(TimeSpan? time, string name, int? year, int? month, int? day)
        {
            if (!year.HasValue || !month.HasValue || !day.HasValue) return DefaultRedirect();

            if (time == null) time = new TimeSpan(0, 0, 0);
            if (name == null) name = string.Empty;

            try
            {
                var date = new DateTime(year.Value, month.Value, day.Value, time.Value.Hours, time.Value.Minutes, time.Value.Seconds);
                events.AddEvent(date, name);
            }
            catch
            {
                return DefaultRedirect();
            }

            return RedirectToAction(nameof(Day), new { year, month, day });
        }

        [HttpPost]
        public IActionResult Edit(TimeSpan? time, string name, int? year, int? month, int? day, Guid? id)
        {
            if (!year.HasValue || !month.HasValue || !day.HasValue) return DefaultRedirect();

            if (time == null) time = new TimeSpan(0, 0, 0);
            if (name == null) name = string.Empty;

            try
            {
                var date = new DateTime(year.Value, month.Value, day.Value, time.Value.Hours, time.Value.Minutes, time.Value.Seconds);
                events.Update(id.Value, date, name);
            }
            catch
            {
                return DefaultRedirect();
            }

            return RedirectToAction(nameof(Day), new { year, month, day });
        }

        [HttpPost]
        public IActionResult Delete(int? year, int? month, int? day, Guid? id)
        {
            if (!year.HasValue || !month.HasValue || !day.HasValue) return DefaultRedirect();

            try
            {
                events.Remove(year.Value, month.Value, day.Value, id.Value);
            }
            catch
            {
                return DefaultRedirect();
            }

            return RedirectToAction(nameof(Day), new { year, month, day });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private IActionResult DefaultRedirect()
        {
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;

            return RedirectToAction(nameof(Index), new { year, month });
        }
    }
}
