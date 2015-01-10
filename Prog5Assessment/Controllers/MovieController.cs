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

            return View(context.Movie.ToList());
        }

        public ActionResult Delete(int id = -1)
        {
            var room = context.Room.SingleOrDefault(x => x.Id == id);

            if (room == null)
            {
                return HttpNotFound();
            }

            //verwijderen van referenties
            var movies = context.Movie.Where(c => c.Id == id).ToList();
            foreach (var movie in movies)
            {
                context.Movie.Remove(movie);
            }

            // delete room
            context.Room.Remove(room);
            context.SaveChanges();
            Response.Redirect("~/Room/");
            return null;
        }

        [HttpGet]
        public ActionResult Edit(int id = -1)
        {
            var room = context.Room.SingleOrDefault(x => x.Id == id);

            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        [HttpPost]
        public ActionResult Edit(Room room, int id = -1)
        {
            var dbRoom = context.Room.SingleOrDefault(x => x.Id == id);
            if (dbRoom == null)
            {
                return HttpNotFound();
            }

            // limit persons
            //if (room.MaxPersons != 2 && room.MaxPersons != 3 && room.MaxPersons != 5)
            //{
            //    // error
            //    return View();
            //}

            dbRoom.Seats = room.Seats;
            dbRoom.Name = room.Name;
            context.SaveChanges();
            Response.Redirect("~/Room/");
            return null;
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
                ModelState.AddModelError("Room", "Room does not exist!");
            }
            else
            {
                var movieList = context.Movie.Where(c => (movie.Date < System.Data.Objects.EntityFunctions.AddMinutes(c.Date, c.Duration) && System.Data.Objects.EntityFunctions.AddMinutes(movie.Date, movie.Duration) > c.Date) || (System.Data.Objects.EntityFunctions.AddMinutes(movie.Date, movie.Duration) > c.Date && movie.Date < System.Data.Objects.EntityFunctions.AddMinutes(c.Date, c.Duration))).ToList();
                if (movieList.Count() > 0)
                {
                    ModelState.AddModelError("Room", "Room already taken!");
                }
                else
                {
                    // no errors
                    movie.Room = dbRoom;
                    context.Movie.Add(movie);
                    context.SaveChanges();
                    Response.Redirect("~/Movie/");
                    return null;
                }
            }

            ViewBag.rooms = context.Room.ToList();
            return View();

        }

    }
}
