using Prog5Assessment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prog5Assessment.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        private DatabaseSetup context;

        public HomeController()
        {
            context = new  DatabaseSetup();
        }

        public ActionResult Index()
        {
            var testing = context.Room.ToList();
            return View();
        }

    }
}
