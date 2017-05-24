using OPS.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OPS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            OPSContext db = new OPSContext();
            var x = db.Order.Count();

            ViewBag.OrderCount = x;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}