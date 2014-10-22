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
        private DatabaseSetup context;

        public HomeController()
        {
            context = new  DatabaseSetup();
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
