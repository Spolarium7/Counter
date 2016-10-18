using GoshenJimenez.Counter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GoshenJimenez.Counter.Controllers
{
    public class HomeController : Controller
    {
        CounterDbContext db = new CounterDbContext();

        //View
        public ActionResult Index()
        {
            var count = GetCount();
            ViewBag.Count = count;
            return View();
        }

        //Ajax calls
        public JsonResult Increase()
        {
            var count = GetCount();
            if(count >= 10)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new JsonResult
                {
                    Data = new
                    {
                        errorMessage = "Up to 10 only"
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else {
                return Json(Increment(), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Reset()
        {
            Models.Counted ctr = db.Counted.FirstOrDefault();
            if (ctr != null)
            {
                ctr.Counter = 0;
                db.SaveChanges();
            }
            else {
                ctr = new Counted();
                ctr.Counter = 0;
                db.Counted.Add(ctr);
                db.SaveChanges();
            }
            return Json(ctr.Counter, JsonRequestBehavior.AllowGet);
        }

        //internal methods
        #region InternalMethods
        private int GetCount()
        {
           Models.Counted ctr = db.Counted.FirstOrDefault();
           if(ctr!= null)
           {
                return ctr.Counter;
           }
           return 0;
        }

        private int Increment()
        {
            Models.Counted ctr = db.Counted.FirstOrDefault();
            if (ctr != null)
            {
                ctr.Counter = ctr.Counter + 1;
                db.SaveChanges();
                return ctr.Counter;
            }else {
                ctr = new Counted();
                ctr.Counter = 1;
                db.Counted.Add(ctr);
                db.SaveChanges();
                return ctr.Counter;
            }
        }
        #endregion
    }
}