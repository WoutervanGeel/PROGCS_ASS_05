using Prog5Assessment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prog5Assessment.Controllers
{
    public class MovieController : Controller
    {
        private DatabaseSetup context;

        public MovieController()
        {
            context = new DatabaseSetup();
        }

        // Overview page: displays list of all rooms
        public ActionResult Index()
        {
            ViewBag.rooms = context.Room.ToList();
            return View(context.Movie.ToList());
        }

        public ActionResult Delete(int id = -1)
        {
            var movie = context.Movie.SingleOrDefault(x => x.Id == id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            // delete room
            context.Movie.Remove(movie);
            context.SaveChanges();
            Response.Redirect("~/Movie/");
            return null;
        }

        public ActionResult Guestlist(int id = -1)
        {
            var dbMovie = context.Movie.SingleOrDefault(x => x.Id == id);
            if (dbMovie == null)
            {
                return HttpNotFound();
            }

            List<int> reservationIds = context.Reservation.Where(c => c.MovieId == id).Select(c => c.Id).ToList();
            return View(context.Guest.Where(c => reservationIds.Contains(c.ReservationId)).OrderBy(c => c.LastName).ToList());
        }

        [HttpGet]
        public ActionResult Edit(int id = -1)
        {
            var dbMovie = context.Movie.SingleOrDefault(x => x.Id == id);
            if (dbMovie == null)
            {
                return HttpNotFound();
            }
            ViewBag.roomId = dbMovie.RoomId;
            ViewBag.rooms = context.Room.ToList();
            return View(dbMovie);
        }

        [HttpPost]
        public ActionResult Edit(Movie movie, int id = -1)
        {
            var dbMovie = context.Movie.SingleOrDefault(x => x.Id == id);
            if (dbMovie == null)
            {
                return HttpNotFound();
            }

            // todo: check of zaal beschikbaar is
            int roomId = Convert.ToInt32(Request.Form["roomsdropdown"]);
            var dbRoom = context.Room.SingleOrDefault(x => x.Id == roomId);
            if (dbRoom == null)
            {
                ModelState.AddModelError("RoomId", "Room does not exist!");
            }
            else
            {
                var movieList = context.Movie.Where(c => ((movie.Date < System.Data.Objects.EntityFunctions.AddMinutes(c.Date, c.Duration) && System.Data.Objects.EntityFunctions.AddMinutes(movie.Date, movie.Duration) > c.Date) || (System.Data.Objects.EntityFunctions.AddMinutes(movie.Date, movie.Duration) > c.Date && movie.Date < System.Data.Objects.EntityFunctions.AddMinutes(c.Date, c.Duration))) && c.Id != dbMovie.Id && c.RoomId == roomId).ToList();
                if (movieList.Count() > 0)
                {
                    ModelState.AddModelError("RoomId", "Room already taken!");
                }
                else
                {
                    // no errors
                    dbMovie.RoomId = roomId;
                    dbMovie.Name = movie.Name;
                    dbMovie.Price = movie.Price;
                    dbMovie.Duration = movie.Duration;
                    dbMovie.Date = movie.Date;
                    context.SaveChanges();
                    Response.Redirect("~/Movie/");
                    return null;
                }
            }

            ViewBag.roomId = roomId;
            ViewBag.rooms = context.Room.ToList();
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.rooms = context.Room.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Models.Movie movie)
        {
            // todo: check of zaal beschikbaar is
            int roomId = Convert.ToInt32(Request.Form["roomsdropdown"]);
            var dbRoom = context.Room.SingleOrDefault(x => x.Id == roomId);
            if (dbRoom == null)
            {
                ModelState.AddModelError("RoomId", "Room does not exist!");
            }
            else
            {
                var movieList = context.Movie.Where(c => ((movie.Date < System.Data.Objects.EntityFunctions.AddMinutes(c.Date, c.Duration) && System.Data.Objects.EntityFunctions.AddMinutes(movie.Date, movie.Duration) > c.Date) || (System.Data.Objects.EntityFunctions.AddMinutes(movie.Date, movie.Duration) > c.Date && movie.Date < System.Data.Objects.EntityFunctions.AddMinutes(c.Date, c.Duration))) && c.RoomId == roomId).ToList();
                if (movieList.Count() > 0)
                {
                    ModelState.AddModelError("RoomId", "Room already taken!");
                }
                else
                {
                    // no errors
                    movie.RoomId = roomId;
                    context.Movie.Add(movie);
                    context.SaveChanges();
                    Response.Redirect("~/Movie/");
                    return null;
                }
            }

            ViewBag.rooms = context.Room.ToList();
            return View();

        }

        public string getRoomName()
        {
            return "test";
        }

    }
}
