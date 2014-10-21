using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prog5Assessment.Controllers
{
    public class RoomController : Controller
    {
        public ActionResult Index()
        {
            return View();
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
