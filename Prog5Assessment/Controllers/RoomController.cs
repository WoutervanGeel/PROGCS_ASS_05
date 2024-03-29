﻿using Prog5Assessment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prog5Assessment.Controllers
{
    public class RoomController : Controller
    {
        private DatabaseSetup context;

        public RoomController()
        {
            context = new DatabaseSetup();
        }

        // Overview page: displays list of all rooms
        public ActionResult Index()
        {

            return View(context.Room.ToList());
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

            if(room == null)
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
            return View();
        }

        [HttpPost]
        public ActionResult Create(Models.Room room)
        {
            // limit persons
            //if (room.MaxPersons != 2 && room.MaxPersons != 3 && room.MaxPersons != 5)
            //{
            //    // error
            //    return View();
            //}

            // max rooms check
            if(context.Room.Count() >= 4)
            {
                // error
                ModelState.AddModelError("", "There are already 4 rooms!");
                return View();
            }

            // no errors
            context.Room.Add(room);
            context.SaveChanges();
            Response.Redirect("~/Room/");
            return null;
        }

    }
}
