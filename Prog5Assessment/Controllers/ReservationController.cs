﻿using Prog5Assessment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prog5Assessment.Controllers
{
    public class ReservationController : Controller
    {
        private DatabaseSetup context;

        public ReservationController()
        {
            context = new DatabaseSetup();
        }

        public ActionResult Index()
        {
            return View(context.Reservation.ToList());
        }

        private void CheckStep(int step)
        {
            if (Session["reservationStep"] == null)
            {
                Response.Redirect("~/Reservation/Step1");
            }
            else if ((int)Session["reservationStep"] < step)
            {
                Session["reservationStep"] = 0;
                Response.Redirect("~/Reservation/Step1");
            }

        }

        private Boolean isValid(Voucher givenVoucher)
        {
            if (givenVoucher.DateStart > DateTime.Now || givenVoucher.DateEnd < DateTime.Now)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private int GetAvailableSeats(Movie currentMovie)
        {
            List<Reservation> reservationList = context.Reservation.Where(c => c.MovieId == currentMovie.Id).ToList();
            int seatsTaken = 0;
            int availableSeats;
            foreach (Reservation reservation in reservationList)
            {
                seatsTaken += reservation.Guests;
            }

            //availableSeats = currentMovie.Room.Seats;
            Room tempRoom = context.Room.Where(c => c.Id == currentMovie.RoomId).ToList()[0];
            availableSeats = tempRoom.Seats;
            availableSeats -= seatsTaken;
            return availableSeats;
        }

        [HttpGet]
        public ActionResult Step1()
        {
            List<Reservation> reservationList = context.Reservation.ToList();
            if (!(reservationList.Count == 0 && reservationList == null))
            {
                Session["reservationId"] = 1;
            }
            else
            {
                Session["reservationId"] = reservationList[reservationList.Count()];
            }
            
            ViewData["ErrorMessage"] = "";
            Session["reservationStep"] = 1;
            return View();
        }

        [HttpPost]
        public ActionResult Step1(Reservation reservationInfo)
        {
            // checks
            if(reservationInfo.Guests <= 0)
            {
                ViewData["ErrorMessage"] = "Een reservering moet voor minimaal 1 persoon gemaakt worden.";
                return View();
            }

            Session["numberOfGuests"] = reservationInfo.Guests;
            Session["reservation"] = reservationInfo;
            ViewData["ErrorMessage"] = "";
            Session["reservationStep"] = 2;
            Response.Redirect("~/Reservation/Step2");
            return null;
        }

        [HttpGet]
        public ActionResult Step2()
        {
            CheckStep(2);
            ViewData["moviesAvailable"] = true;
            Session["moviesAvailable"] = true;

            // generate list
            List<SelectListItem> li = new List<SelectListItem>();
            if (context.Movie.ToList().Count() == 0)
            {
                li.Add(new SelectListItem { Text = "No Movies available", Value = "-1" });
                ViewData["moviesAvailable"] = false;
                Session["moviesAvailable"] = false;
            }
            else
            {
                li.Add(new SelectListItem { Text = "Select ...", Value = "-1" });
                foreach (var movie in context.Movie.ToList())
                {
                    if (GetAvailableSeats(movie) >= (int)Session["numberOfGuests"])
                    {
                        li.Add(new SelectListItem { Text = movie.Name + " at " +movie.Date, Value = "" + movie.Id });
                    }
                }
                if (li.Count() == 1)
                {
                    ViewData["moviesAvailable"] = false;
                    Session["moviesAvailable"] = false;
                }
            }
            ViewData["movies"] = li;
            Session["movies"] = li;

            return View();
        }

        [HttpPost]
        public ActionResult Step2(Reservation reservationInfo)
        {
            CheckStep(2);

            // checks

            string s = Request.Form["MovieId"];
            if (Convert.ToInt32(s) == -1)
            {
                ViewData["ErrorMessage"] = "Selecteer een film";
                ViewData["moviesAvailable"] = Session["moviesAvailable"];
                ViewData["movies"] = Session["movies"];
                return View();
            }

            Session["reservationMovieId"] = Request.Form["MovieId"];
            Session["reservationStep"] = 3;
            Response.Redirect("~/Reservation/Step3");
            return null;
        }

        [HttpGet]
        public ActionResult Step3()
        {
            ViewData["NumberOfPersons"] = Session["numberOfGuests"];
            CheckStep(3);
            return View();
        }

        [HttpPost]
        public ActionResult Step3(Guest guest)
        {
            ViewData["NumberOfPersons"] = Session["numberOfGuests"];
            CheckStep(3);
            Guest tempGuest;
            for(int i = 0; i < (int)Session["numberOfGuests"]; i++)
            {
                tempGuest = new Guest();
                tempGuest.FirstName = Request.Form.GetValues("FirstName[]")[i];
                tempGuest.LastName = Request.Form.GetValues("LastName[]")[i];
                if (!(Request.Form.GetValues("Insertion[]").Count() == 0 && Request.Form.GetValues("Insertion[]") == null))
                {
                tempGuest.Insertion = Request.Form.GetValues("Insertion[]")[i];
                }
                tempGuest.ReservationId = (int)Session["reservationId"];
                Session["reservationGuest" + i] = tempGuest;
            }

            // success
            Session["reservationStep"] = 4;
            Response.Redirect("~/Reservation/Step4");
            return null;
        }


        [HttpGet]
        public ActionResult Step4()
        {
            CheckStep(4);
            return View();
        }

        [HttpPost]
        public ActionResult Step4(Reservation reservationInfo)
        {
            CheckStep(4);
            
            int tempMovieId = (Convert.ToInt32((String)Session["reservationMovieId"]));
            List<Movie> tempMovieList = context.Movie.Where(c => c.Id == tempMovieId).ToList();
            Movie tempMovie = tempMovieList[0];
            int numberOfPeople = (int)Session["numberOfGuests"];
            int priceMovie = tempMovie.Price;
            int priceTotal = numberOfPeople * priceMovie;

            int i = reservationInfo.BankAccount;
            Session["reservationBankAccount"] = reservationInfo.BankAccount;
            Session["reservationInvoiceAddress"] = reservationInfo.InvoiceAddress;
            Session["reservationInvoiceCity"] = reservationInfo.InvoiceCity;
            Session["reservationInvoicePostal"] = reservationInfo.InvoicePostal;

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            string tempVoucherCode = Request.Form["VoucherCode"];
            List<Voucher> tempVoucherList = context.Voucher.Where(c => c.Code == tempVoucherCode).ToList();

            if (tempVoucherCode.Equals(""))
            {
                //geen voucher ingevuld
                Session["reservationSelectedVoucherId"] = -1;
            }
            else
            {
                if (tempVoucherList == null || tempVoucherList.Count == 0)
                {
                    //geen voucher code gevonden in de database
                    ViewData["ErrorMessage"] = "Deze voucher code is niet geldig.";
                    return View();
                }
                else
                {
                    //voucher gevonden in de database
                    Voucher tempVoucher = tempVoucherList[0];
                    if (isValid(tempVoucher))
                    {
                        //voucher is nog geldig
                        priceTotal = priceTotal - tempVoucher.Discount;
                        Session["reservationSelectedVoucherId"] = tempVoucherList[0].Id;
                    }
                    else
                    {
                        //voucher is niet meer/nog niet geldig
                        ViewData["ErrorMessage"] = "Deze voucher code is niet geldig.";
                        return View();
                    }
                }
            }

            // success
            
            Session["reservationPriceTotal"] = priceTotal;
            Session["reservationStep"] = 5;
            Response.Redirect("~/Reservation/Step5");
            return null;

            //// add room
            //bookingInfo.BookedRoom = context.Room.Where(c => c.Id == (int)Session["bookingRoomId"]).ToList()[0];
            //context.Booking.Add(bookingInfo);
            //context.SaveChanges();
            //// add guests

            //for (int i = 0; i < ((int)Session["bookingGuestAmount"]) - 1; i++)
            //{

            //}



            //Session["bookingStep"] = 5;
            //Response.Redirect("~/Booking/Step5");
            //return null;
        }

        [HttpGet]
        public ActionResult Step5()
        {
            CheckStep(5);
            ViewData["TotalPrice"] = Session["reservationPriceTotal"];
            Session["reservationStep"] = 5;
            return View();
        }

        [HttpPost]
        public ActionResult Step5(Reservation reservationInfo)
        {
            CheckStep(5);

            int tempVoucherId = (int)Session["reservationSelectedVoucherId"];

            if (tempVoucherId != -1)
            {
                reservationInfo.VoucherId = tempVoucherId;
            }
            //List<Voucher> tempVoucherList = context.Voucher.Where(c => c.Code == tempVoucherCode).ToList();
            //Voucher tempVoucher = tempVoucherList[0];
            reservationInfo.Id = (int)Session["reservationId"];
            reservationInfo.MovieId = (Convert.ToInt32((String)Session["reservationMovieId"]));
            reservationInfo.PriceTotal = (int)Session["reservationPriceTotal"];
            reservationInfo.InvoiceAddress = (string)Session["reservationInvoiceAddress"];
            reservationInfo.InvoiceCity = (string)Session["reservationInvoiceCity"];
            reservationInfo.InvoicePostal = (string)Session["reservationInvoicePostal"];
            reservationInfo.BankAccount = (int)Session["reservationBankAccount"];
            
            reservationInfo.Guests = (int)Session["numberOfGuests"];

            for (int i = 0; i < (int)Session["numberOfGuests"]; i++)
            {
                context.Guest.Add((Guest)Session["reservationGuest" + i]);
            }
            context.SaveChanges();
                
            context.Reservation.Add(reservationInfo);
            context.SaveChanges();
            Response.Redirect("~/Home/");
            return null;
        }

//        [HttpGet]
//        public ActionResult Edit(int id = -1)
//        {
//            var booking = context.Booking.SingleOrDefault(x => x.Id == id);

//            if (booking == null)
//            {
//                return HttpNotFound();
//            }
//            return View(booking);
//        }

//        [HttpPost]
//        public ActionResult Edit(Booking booking, int id = -1)
//        {
//            var dbBooking = context.Booking.SingleOrDefault(x => x.Id == id);
//            if (dbBooking == null)
//            {
//                return HttpNotFound();
//            }

//            dbBooking.Price = booking.Price;
//            dbBooking.StartDate = booking.StartDate;
//            dbBooking.EndDate = booking.EndDate;
//            dbBooking.AccountNr = booking.AccountNr;
//            //dbBooking.BookedRoom = booking.BookedRoom;
//            context.SaveChanges();
//            return View(dbBooking);
//        }

//        [HttpGet]
//        public ActionResult Create(int id)
//        {
//            var dbRoom = context.Room.SingleOrDefault(x => x.Id == id);
//            ViewData["NumberOfPersons"] = dbRoom.MaxPersons;
//            return View();
//        }

//        [HttpPost]
//        public ActionResult Create(Models.Booking booking, int id = -1)
//        {
//            //return View();
//            var dbRoom = context.Room.SingleOrDefault(x => x.Id == id);

//            var guest = booking.Guests;

//            booking.BookedRoom = dbRoom;
//            context.Booking.Add(booking);
//            context.SaveChanges();
//            Response.Redirect("~/Booking/");
//            return null;
//        }

//        [HttpGet]
//        public ActionResult Overview()
//        {
//            ViewBag.OverviewTable = "";
//            return View();
//        }

//        [HttpPost]
//        public ActionResult Overview(Booking bookingFilter)
//        {
//            DateTime startDate = bookingFilter.StartDate;
//            DateTime endDate = bookingFilter.EndDate;

//            List<Booking> resultList = new List<Booking>();
//            string table = "<table>";
//            foreach (var item in context.Booking.ToList())
//            {
//                if (item.StartDate >= startDate && item.EndDate <= endDate)
//                {
//                    table += "<tr><td>" + item.StartDate + "</td><td>" + item.EndDate + "</td><td>" + item.Price;
//                    //resultList.Add(item);
//                }
//            }
//            table += "</table>";
//            ViewBag.OverviewTable = table;
//            //ViewBag.OverviewTable = resultList;
//            return View();
//        }

    }
}