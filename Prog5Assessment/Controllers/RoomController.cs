using Prog5Assessment.Models;
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

        public ActionResult PriceList(int id = -1)
        {

           // var room = context.Room.SingleOrDefault(x => x.Id == id);

            return View(context.Room.ToList());
        }

        [HttpGet]
        public ActionResult Delete(int id = -1)
        {
            var room = context.Room.SingleOrDefault(x => x.Id == id);

            if (room == null)
            {
                return HttpNotFound();
            }

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
            if (room.MaxPersons != 2 && room.MaxPersons != 3 && room.MaxPersons != 5)
            {
                // error
                return View();
            }

            dbRoom.MaxPersons = room.MaxPersons;
            dbRoom.MinPrice = room.MinPrice;
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
            if (room.MaxPersons != 2 && room.MaxPersons != 3 && room.MaxPersons != 5)
            {
                // error
                return View();
            }

            // max rooms check
            if(context.Room.Count() > 12)
            {
                // error
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
