using Prog5Assessment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prog5Assessment.Controllers
{
    public class VoucherController : Controller
    {
        private DatabaseSetup context;

        public VoucherController()
        {
            context = new DatabaseSetup();
        }

        // Overview page: displays list of all rooms
        public ActionResult Index()
        {

            return View(context.Voucher.ToList());
        }

        public ActionResult Delete(int id = -1)
        {
            var voucher = context.Voucher.SingleOrDefault(x => x.Id == id);

            if (voucher == null)
            {
                return HttpNotFound();
            }

            // delete voucher
            context.Voucher.Remove(voucher);

            context.SaveChanges();
            Response.Redirect("~/Voucher/");
            return null;
        }

        [HttpGet]
        public ActionResult Edit(int id = -1)
        {
            var voucher = context.Voucher.SingleOrDefault(x => x.Id == id);

            if(voucher == null)
            {
                return HttpNotFound();
            }
            return View(voucher);
        }

        [HttpPost]
        public ActionResult Edit(Voucher voucher, int id = -1)
        {
            var dbVoucher = context.Voucher.SingleOrDefault(x => x.Id == id);
            if (dbVoucher == null)
            {
                return HttpNotFound();
            }

            dbVoucher.Code = voucher.Code;
            dbVoucher.DateEnd = voucher.DateEnd;
            dbVoucher.DateStart = voucher.DateStart;
            dbVoucher.Discount = voucher.Discount;
            context.SaveChanges();
            Response.Redirect("~/Voucher/");
            return null;
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Models.Voucher voucher)
        {
            // limit persons
            //if (room.MaxPersons != 2 && room.MaxPersons != 3 && room.MaxPersons != 5)
            //{
            //    // error
            //    return View();
            //}

            // no errors
            context.Voucher.Add(voucher);
            context.SaveChanges();
            Response.Redirect("~/Voucher/");
            return null;
        }

    }
}
