using Prog5Assessment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prog5Assessment.Controllers
{
    public class GuestController : Controller
    {
        private DatabaseSetup context;

        public GuestController()
        {
            context = new DatabaseSetup();
        }
        //
        // GET: /Guest/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Guest/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Guest/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Guest/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Guest/Edit/5

        [HttpGet]
        public ActionResult Edit(int id = -1)
        {
            var dbGuest = context.Guest.SingleOrDefault(x => x.Id == id);
            if (dbGuest == null)
            {
                return HttpNotFound();
            }
            //ViewBag.guestId = dbGuest.Id;
            return View(dbGuest);
        }

        //
        // POST: /Guest/Edit/5

        [HttpPost]
        public ActionResult Edit(Guest guest, int id = -1)
        {
            var dbGuest = context.Guest.SingleOrDefault(x => x.Id == id);
            if (dbGuest == null)
            {
                return HttpNotFound();
            }

            // no errors
            dbGuest.FirstName = guest.FirstName;
            dbGuest.Insertion = guest.Insertion;
            dbGuest.LastName = guest.LastName;
            context.SaveChanges();
            Response.Redirect("~/Movie/");

            return View();
        }

        //
        // GET: /Guest/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Guest/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
