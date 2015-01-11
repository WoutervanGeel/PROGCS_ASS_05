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
            //kijken of de gebruiker ook echt in de huidige stap hoort
            if (Session["reservationStep"] == null)
            {
                //terug naar stap 1
                Response.Redirect("~/Reservation/Step1");
            }
            else if ((int)Session["reservationStep"] < step)
            {
                //terug naar stap 1
                Session["reservationStep"] = 0;
                Response.Redirect("~/Reservation/Step1");
            }

        }

        private Boolean isValid(Voucher givenVoucher)
        {
            //kijken of de voucher geldig is op de huidige datum
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
            //lijst genereren
            List<Reservation> reservationList = context.Reservation.Where(c => c.MovieId == currentMovie.Id).ToList();
            int seatsTaken = 0;
            int availableSeats;
            //aantal bezette plaatsen berekenen
            foreach (Reservation reservation in reservationList)
            {
                seatsTaken += reservation.Guests;
            }
            //vrije plaatsen berekenen
            Room tempRoom = context.Room.Where(c => c.Id == currentMovie.RoomId).ToList()[0];
            availableSeats = tempRoom.Seats;
            availableSeats -= seatsTaken;
            return availableSeats;
        }

        [HttpGet]
        public ActionResult Step1()
        {
            //het zetten van de reservationId
            List<Reservation> reservationList = context.Reservation.ToList();
            if ((reservationList.Count == 0 || reservationList == null))
            {
                Session["reservationId"] = 1;
            }
            else
            {
                Session["reservationId"] = reservationList.Count() + 1;
            }
            
            ViewData["ErrorMessage"] = "";
            Session["reservationStep"] = 1;
            return View();
        }

        [HttpPost]
        public ActionResult Step1(Reservation reservationInfo)
        {
            if(reservationInfo.Guests <= 0)
            {
                //er wordt een aantal personen kleiner of gelijk aan 0 opgegeven
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
                //er bestaan geen films in de database
                li.Add(new SelectListItem { Text = "No Movies available", Value = "-1" });
                ViewData["moviesAvailable"] = false;
                Session["moviesAvailable"] = false;
            }
            else
            {
                //er bestaan films in de database
                li.Add(new SelectListItem { Text = "Select ...", Value = "-1" });
                foreach (var movie in context.Movie.ToList())
                {
                    if (GetAvailableSeats(movie) >= (int)Session["numberOfGuests"])
                    {
                        //aantal vrije plaatsen is groter dan het aantal gasten
                        li.Add(new SelectListItem { Text = movie.Name + " at " +movie.Date, Value = "" + movie.Id });
                    }
                }
                if (li.Count() == 1)
                {
                    //er is geen film toegevoegd aan de lijst
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

            string s = Request.Form["MovieId"];
            if (Convert.ToInt32(s) == -1)
            {
                //de gebruiker heeft geen film geselecteerd
                ViewData["ErrorMessage"] = "Selecteer een film";
                ViewData["moviesAvailable"] = Session["moviesAvailable"];
                ViewData["movies"] = Session["movies"];
                return View();
            }
            //movieId opslaan in session
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
                //voor elke gast
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
            Session["reservationDiscount"] = 0;
            
            int tempMovieId = (Convert.ToInt32((String)Session["reservationMovieId"]));
            List<Movie> tempMovieList = context.Movie.Where(c => c.Id == tempMovieId).ToList();
            Movie tempMovie = tempMovieList[0];
            int numberOfPeople = (int)Session["numberOfGuests"];
            int priceMovie = tempMovie.Price;
            int priceTotal = numberOfPeople * priceMovie;
            Session["TotalPriceNoDiscount"] = priceTotal;

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
                        if (priceTotal < 0)
                        {
                            priceTotal = 0;
                        }
                        Session["reservationSelectedVoucherId"] = tempVoucherList[0].Id;
                        Session["reservationDiscount"] = tempVoucher.Discount;
                    }
                    else
                    {
                        //voucher is niet meer/nog niet geldig
                        ViewData["ErrorMessage"] = "Deze voucher code is niet geldig.";
                        return View();
                    }
                }
            }
            
            Session["reservationPriceTotal"] = priceTotal;
            Session["reservationStep"] = 5;
            Response.Redirect("~/Reservation/Step5");
            return null;
        }

        [HttpGet]
        public ActionResult Step5()
        {
            CheckStep(5);
            ViewData["TotalPrice"] = Session["reservationPriceTotal"];
            ViewData["TotalPriceNoDiscount"] = Session["TotalPriceNoDiscount"];
            ViewData["reservationDiscount"] = Session["reservationDiscount"];
            Session["reservationStep"] = 5;
            return View();
        }

        [HttpPost]
        public ActionResult Step5(Reservation reservationInfo)
        {
            CheckStep(5);

            Session["reservationStep"] = 6;
            Response.Redirect("~/Reservation/Step6");
            return null;
        }

        [HttpGet]
        public ActionResult Step6()
        {
            CheckStep(6);
            return View();
        }

        [HttpPost]
        public ActionResult Step6(Reservation reservationInfo)
        {
            CheckStep(6);

            int tempVoucherId = (int)Session["reservationSelectedVoucherId"];

            if (tempVoucherId != -1)
            {
                reservationInfo.VoucherId = tempVoucherId;
            }
            //invullen van alle gegevens
            reservationInfo.Id = (int)Session["reservationId"];
            reservationInfo.MovieId = (Convert.ToInt32((String)Session["reservationMovieId"]));
            reservationInfo.PriceTotal = (int)Session["reservationPriceTotal"];
            reservationInfo.InvoiceAddress = reservationInfo.InvoiceAddress;
            reservationInfo.InvoiceCity = reservationInfo.InvoiceCity;
            reservationInfo.InvoicePostal = reservationInfo.InvoicePostal;
            reservationInfo.BankAccount = reservationInfo.BankAccount;

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
    }
}