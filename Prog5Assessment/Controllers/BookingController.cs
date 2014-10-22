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

        private void CheckStep(int step)
        {
            if (Session["bookingStep"] == null)
            {
                Response.Redirect("~/Booking/Step1");
            }
            else if ((int)Session["bookingStep"] < step)
            {
                Session["bookingStep"] = 0;
                Response.Redirect("~/Booking/Step1");
            }

        }

        [HttpGet]
        public ActionResult Step1()
        {
            Session["bookingStep"] = 1;
            return View();
        }

        [HttpPost]
        public ActionResult Step1(Booking bookingInfo)
        {
            TempData["booking"] = bookingInfo;
            Session["bookingStep"] = 2;
            Response.Redirect("~/Booking/Step2");
            return null;
        }

        [HttpGet]
        public ActionResult Step2()
        {
            CheckStep(2);
            List<SelectListItem> li = new List<SelectListItem>();
            if (context.Room.ToList().Count() == 0)
            {
                li.Add(new SelectListItem { Text = "No room available", Value = "-1" });
            }
            else
            {
                li.Add(new SelectListItem { Text = "Select ...", Value = "-1" });
                foreach (var room in context.Room.ToList())
                {
                    li.Add(new SelectListItem { Text = room.Name, Value = "" + room.Id });
                }
            }
            ViewData["rooms"] = li;

            return View();
        }

        [HttpPost]
        public ActionResult Step2(FormCollection collection)
        {
            CheckStep(2);
            try
            {
                var test = collection.GetValue("RoomId");
            }
            catch (Exception ex)
            {
                return View();
            }

            TempData["booking"] = "";
            Response.Redirect("~/Booking/Step3");
            return null;
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
            //dbBooking.BookedRoom = booking.BookedRoom;
            context.SaveChanges();
            return View(dbBooking);
        }

        [HttpGet]
        public ActionResult Create(int id)
        {
            var dbRoom = context.Room.SingleOrDefault(x => x.Id == id);
            ViewData["NumberOfPersons"] = dbRoom.MaxPersons;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Models.Booking booking, int id = -1)
        {
            //return View();
            var dbRoom = context.Room.SingleOrDefault(x => x.Id == id);

            var guest = booking.Guests;

            booking.BookedRoom = dbRoom;
            context.Booking.Add(booking);
            context.SaveChanges();
            Response.Redirect("~/Booking/");
            return null;
        }

        [HttpGet]
        public ActionResult StartBooking()
        {
            return View();
        }
    }
}
