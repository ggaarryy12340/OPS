using OPS.Models;
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
    public class ProductController : Controller
    {
        private int pageSize = 15;
        private readonly ProductService _service = null;

        public ProductService Service
        {
            get { return _service ?? new ProductService(); }
        }

        // GET: Product
        public ActionResult Index(int? page, ProductIndextView model)
        {
            var pageNumeber = page ?? 1;
            model.Products = Service.GetAllProduct(pageNumeber, pageSize);
            return View(model);
        }

        public ActionResult CreateProduct()
        {
            ViewBag.CategoryDropdownList = new SelectList(Service.GetCategoryDropdownList(), "PDCategoryId", "PDCategoryName");
            return View();
        }

        [HttpPost]
        public ActionResult CreateProduct(Product model)
        {
            bool rs = false;

            //表單驗證
            if (ModelState.IsValid)
            {
                rs = Service.CreateProduct(model);
                if (rs)
                {
                    TempData["message"] = "增加商品成功";
                }
                else
                {
                    TempData["message"] = "增加商品失敗";
                }

                return RedirectToAction("Index");
            }
            ViewBag.CategoryDropdownList = new SelectList(Service.GetCategoryDropdownList(), "PDCategoryId", "PDCategoryName");
            return View(model);
        }

        public ActionResult ImportProduct()
        {
            return View();
        }

        public ActionResult ProductDetail(string id)
        {
            return View(Service.GetProductDetail(id));
        }

        public ActionResult ProductEdit(string id)
        {
            ViewBag.CategoryDropdownList = new SelectList(Service.GetCategoryDropdownList(), "PDCategoryId", "PDCategoryName");
            return View(Service.GetProductDetail(id));
        }

        [HttpPost]
        public ActionResult ProductEdit(Product model)
        {
            bool rs = false;

            //表單驗證
            if (ModelState.IsValid)
            {
                rs = Service.ProductEdit(model);
                if (rs)
                {
                    TempData["message"] = "修改商品成功";
                }
                else
                {
                    TempData["message"] = "修改商品失敗";
                }

                return RedirectToAction("Index");
            }
            ViewBag.CategoryDropdownList = new SelectList(Service.GetCategoryDropdownList(), "PDCategoryId", "PDCategoryName");
            return View(model);
        }

        public ActionResult ProductDelete(string id)
        {
            var rs = Service.ProductDelete(id);

            if (rs)
            {
                TempData["message"] = "刪除商品成功";
            }
            else
            {
                TempData["message"] = "刪除商品失敗";
            }

            return RedirectToAction("Index");
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