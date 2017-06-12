using OPS.Models.OPSContext;
using OPS.Models.ViewModel;
using OPS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OPS.Controllers
{
    public class ProductCategoryController : Controller
    {
        private readonly ProductCategoryService _service = null;

        public ProductCategoryService Service
        {
            get { return _service ?? new ProductCategoryService(); }
        }

        // GET: ProductCategory
        public ActionResult Index()
        {
            ProductCategoryView vm = new ProductCategoryView()
            {
                PDCategories = Service.GetPDCategories()
            };

            return View(vm);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(PDCategory model)
        {
            bool rs = Service.CreatePDCategory(model);

            if (rs)
            {
                TempData["message"] = "增加商品分類成功";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "增加商品分類失敗";
            }

            return View();
        }
    }
}