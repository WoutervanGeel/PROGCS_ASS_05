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

        [HttpGet]
        public ActionResult Edit(int id = -1)
        {
            var booking = context.Booking.SingleOrDefault(x => x.Id == id);

            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        [HttpPost]
        public ActionResult Edit(Booking booking, int id = -1)
        {
            var dbBooking = context.Booking.SingleOrDefault(x => x.Id == id);
            if (dbBooking == null)
            {
                return HttpNotFound();
            }

            dbBooking.Price = booking.Price;
            dbBooking.StartDate = booking.StartDate;
            dbBooking.EndDate = booking.EndDate;
            dbBooking.AccountNr = booking.AccountNr;
            dbBooking.InvoiceAddress = booking.InvoiceAddress;
            dbBooking.BookedRoom = booking.BookedRoom;
            context.SaveChanges();
            return View(dbBooking);
        }
    }
}
