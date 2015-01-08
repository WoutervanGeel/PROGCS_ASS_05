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
            // checks

            Session["booking"] = bookingInfo;
            Session["bookingStartDate"] = bookingInfo.StartDate;
            Session["bookingStartDate"] = bookingInfo.EndDate;
            Session["bookingGuestAmount"] = bookingInfo.Guests;
            Session["bookingStep"] = 2;
            Response.Redirect("~/Booking/Step2");
            return null;
        }

        [HttpGet]
        public ActionResult Step2()
        {
            CheckStep(2);

            // generate list
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
        public ActionResult Step2(EventArgs e)
        {
            CheckStep(2);
            String roomId = "";

            // generate list
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

            // error checks

            roomId = Request.Form["RoomId"];

            // success
            Session["bookingRoomId"] = roomId;

            Session["bookingStep"] = 3;
            Response.Redirect("~/Booking/Step3");
            return null;
        }

        [HttpGet]
        public ActionResult Step3()
        {
            ViewData["NumberOfPersons"] = Session["bookingGuestAmount"];
            CheckStep(3);
            return View();
        }

        [HttpPost]
        public ActionResult Step3(EventArgs e)
        {
            ViewData["NumberOfPersons"] = Session["bookingGuestAmount"];
            CheckStep(3);

            Session["GuestGender"] = Request.Form.GetValues("Gender");
            Session["GuestFirstName"] = Request.Form.GetValues("FirstName");
            Session["GuestInfix"] = Request.Form.GetValues("Infix");
            Session["LastName"] = Request.Form.GetValues("LastName");
            Session["GuestGender"] = Request.Form.GetValues("Gender");

            // success
            Session["bookingStep"] = 4;
            Response.Redirect("~/Booking/Step4");
            return null;
        }


        [HttpGet]
        public ActionResult Step4()
        {
            CheckStep(4);
            return View();
        }

        [HttpPost]
        public ActionResult Step4(Booking bookingInfo)
        {
            CheckStep(4);
            // success
            bookingInfo.StartDate = (DateTime)Session["bookingStartDate"];
            bookingInfo.EndDate = (DateTime)Session["bookingEndDate"];
            bookingInfo.Guests = (int)Session["bookingGuestAmount"];
            
            // add room
            bookingInfo.BookedRoom = context.Room.Where(c => c.Id == (int)Session["bookingRoomId"]).ToList()[0];
            context.Booking.Add(bookingInfo);
            context.SaveChanges();
            // add guests
            
            for(int i = 0; i < ((int)Session["bookingGuestAmount"])-1; i++)
            {
                
            }

          

            Session["bookingStep"] = 5;
            Response.Redirect("~/Booking/Step5");
            return null;
        }

        [HttpGet]
        public ActionResult Step5()
        {
            CheckStep(5);
            Session["bookingStep"] = 1;
            return View();
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
        public ActionResult Overview()
        {
            ViewBag.OverviewTable = "";
            return View();
        }

        [HttpPost]
        public ActionResult Overview(Booking bookingFilter)
        {
            DateTime startDate = bookingFilter.StartDate;
            DateTime endDate = bookingFilter.EndDate;

            List<Booking> resultList = new List<Booking>();
            string table = "<table>";
            foreach (var item in context.Booking.ToList())
            {
                if (item.StartDate >= startDate && item.EndDate <= endDate)
                {
                    table += "<tr><td>" + item.StartDate + "</td><td>" + item.EndDate + "</td><td>" + item.Price;
                    //resultList.Add(item);
                }
            }
            table += "</table>";
            ViewBag.OverviewTable = table;
            //ViewBag.OverviewTable = resultList;
            return View();
        }
        
    }
}
