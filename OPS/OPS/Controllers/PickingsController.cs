using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OPS.Controllers
{
    public class PickingsController : Controller
    {
        // GET: Pickings
        public ActionResult Index()
        {
            return View();
        }
    }
}