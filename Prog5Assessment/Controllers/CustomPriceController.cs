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

        public ActionResult Index(int id = -1)
        {
            var room = context.Room.SingleOrDefault(x => x.Id == id);
            if (room == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoomId = id;
            return View(context.CustomPrices.Where(c => c.Room_Id == id));
        }

        public ActionResult Delete(int id = -1)
        {
            var price = context.CustomPrices.SingleOrDefault(x => x.Id == id);

            if (price == null)
            {
                return HttpNotFound();
            }

            int roomId = price.Room_Id;
            context.CustomPrices.Remove(price);
            context.SaveChanges();
            Response.Redirect("~/CustomPrice/Index/" + roomId);
            return null;
        }

        [HttpGet]
        public ActionResult Create(int id = -1)
        {
            var dbRoom = context.Room.SingleOrDefault(x => x.Id == id);
            if (dbRoom == null)
            {
                return HttpNotFound();
            }
     
            return View();
        }

        [HttpPost]
        public ActionResult Create(Models.CustomPrices customPrice, int id = -1)
        {
            var dbRoom = context.Room.SingleOrDefault(x => x.Id == id);
            if (dbRoom == null)
            {
                return HttpNotFound();
            }

            customPrice.Room_Id = id;
            context.CustomPrices.Add(customPrice);
            context.SaveChanges();
            Response.Redirect("~/CustomPrice/Index/"+id);
            return null;
        }
    }
}
