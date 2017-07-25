using OPS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OPS.Controllers
{
    public class PickingsAsignController : Controller
    {
        private readonly PickingsAsignService _service = null;

        public PickingsAsignService Service
        {
            get { return _service ?? new PickingsAsignService(); }
        }

        // GET: PickingsAsign
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