using Prog5Assessment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prog5Assessment.Controllers
{
    public class BookingController : Controller
    {
        private DatabaseSetup context;

        public BookingController()
        {
            context = new DatabaseSetup();
        }

        public ActionResult Index()
        {
            return View(context.Booking.ToList());
        }

        public ActionResult Edit()
        {
            return View(context.Booking.ToList());
        }
    }
}
