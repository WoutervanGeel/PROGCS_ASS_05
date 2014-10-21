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

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Models.Room room)
        {
            // limit persons
            if (room.MaxPersons != 2 || room.MaxPersons != 3 || room.MaxPersons != 5)
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
            // go back to list
            return Index();
        }

        [HttpGet]
        public ActionResult NewRoom()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewRoom(Models.Room room)
        {
            //In deze methode kom je op het moment dat je op Submit klikt op de room pagina newRoom
            //room.Id = 5;
            //Models.DatabaseSetup
            return View();
        }
    }
}
