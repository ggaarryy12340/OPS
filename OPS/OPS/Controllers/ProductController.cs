using OPS.Models;
using OPS.Models.OPSContext;
using OPS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OPS.Controllers
{
    public class ProductController : Controller
    {

        // GET: Product
        public ActionResult Index(ProductIndextView model)
        {
            return View(model);
        }

        public ActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateProduct(Product model)
        {
            return View(model);
        }

        public ActionResult ImportProduct()
        {
            return View();
        }
    }
}