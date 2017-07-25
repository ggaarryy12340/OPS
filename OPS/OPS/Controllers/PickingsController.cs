using OPS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OPS.Controllers
{
    public class PickingsController : Controller
    {
        private readonly PickingsService _service = null;

        public PickingsService Service
        {
            get { return _service ?? new PickingsService(); }
        }

        // GET: Pickings
        public ActionResult Index()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Service.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}