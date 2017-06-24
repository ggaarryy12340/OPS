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
            }
            else
            {
                TempData["message"] = "增加商品分類失敗";
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var model = Service.GetSingle(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(PDCategory model)
        {
            bool rs = Service.Edit(model);

            if (rs)
            {
                TempData["message"] = "修改商品分類成功";
            }
            else
            {
                TempData["message"] = "修改商品分類失敗";
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            bool rs = Service.Delete(id);

            if (rs)
            {
                TempData["message"] = "刪除商品分類成功";
            }
            else
            {
                TempData["message"] = "刪除商品分類失敗";
            }

            return RedirectToAction("Index");
        }


    }
}