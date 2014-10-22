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

        [HttpGet]
        public ActionResult Index(int id = -1)
        {
            var room = context.Room.SingleOrDefault(x => x.Id == id);
            if (room == null)
            {
                return HttpNotFound();
            }

            return View(context.CustomPrices.Where(c => c.Room_Id == id));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Models.CustomPrices customPrice, int id = -1)
        {
            var dbRoom = context.Room.SingleOrDefault(x => x.Id == id);
            context.CustomPrices.Add(customPrice);
            context.SaveChanges();
            Response.Redirect("~/CustomPrices/"+id);
            return null;
        }
    }
}
