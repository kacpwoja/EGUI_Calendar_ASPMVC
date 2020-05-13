﻿using System;
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
            if(!year.HasValue || !month.HasValue)
            {
                return DefaultRedirect();
            }

            IndexViewModel vm;

            try
            {
                var date = new DateTime(year.Value, month.Value, 1);
                var isCurrent = DateTime.Now.Year == year.Value && DateTime.Now.Month == month.Value;

                vm = new IndexViewModel
                {
                    Title = date.ToString("MMM - yyyy"),
                    Offset = (int)date.DayOfWeek,
                    Days = DateTime.DaysInMonth(year.Value, month.Value),
                    SelectedDay = isCurrent ? DateTime.Now.Day : 0,
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

        public IActionResult Event()
        {
            return View();
        }

        public IActionResult Day(int? year, int? month, int? day)
        {
            return View();
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
