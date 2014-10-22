using Prog5Assessment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prog5Assessment.Controllers
{
    public class CustomPriceController : Controller
    {
        private DatabaseSetup context;

        public CustomPriceController()
        {
            context = new DatabaseSetup();
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Models.CustomPrices customPrice, int id = -1)
        {
            //return View();
            var dbRoom = context.Room.SingleOrDefault(x => x.Id == id);

            customPrice.Room = dbRoom;
            context.CustomPrices.Add(customPrice);
            context.SaveChanges();
            Response.Redirect("~/Room/");
            return null;
        }
    }
}
