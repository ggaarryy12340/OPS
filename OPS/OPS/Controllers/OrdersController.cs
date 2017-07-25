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
    public class OrdersController : Controller
    {
        private int pageSize = 15;
        private readonly OrdersService _service = null;

        public OrdersService Service
        {
            get { return _service ?? new OrdersService(); }
        }

        // GET: Orders
        public ActionResult Index(int? page, OrdersIndexView model)
        {
            var pageNumeber = page ?? 1;
            model.Orders = Service.GetOrdersList(model.OrderSearchParameter, pageNumeber, pageSize);
            return View(model);
        }

        public ActionResult CreateOrders()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateOrders(Order model)
        {
            bool rs = false;

            //表單驗證
            if (ModelState.IsValid)
            {
                rs = Service.CreateOrder(model);
                if (rs)
                {
                    TempData["message"] = "增加訂單成功";
                }
                else
                {
                    TempData["message"] = "增加訂單失敗";
                }

                return RedirectToAction("Index");
            }
            return View(model);
        }

        //動態新增明細列
        public ActionResult CreateDetail()
        {
            return PartialView("_CreateDetail");
        }

        public ActionResult OrdersDetail(Guid id)
        {
            var Order = Service.GetSingleOrder(id);
            return View(Order);
        }

        public ActionResult OrdersEdit(Guid id)
        {
            var Order = Service.GetSingleOrder(id);
            return View(Order);
        }

        [HttpPost]
        public ActionResult OrdersEdit(Order model)
        {
            bool rs = false;

            //表單驗證
            if (ModelState.IsValid)
            {
                rs = Service.OrdersEdit(model);
                if (rs)
                {
                    TempData["message"] = "編輯訂單成功";
                }
                else
                {
                    TempData["message"] = "編輯訂單失敗";
                }

                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult OrdersLog(Guid id)
        {
            var Log = Service.GetSingleOrderLog(id);
            return View(Log);
        }

        //輸入商品編號回傳商品資訊
        public ActionResult ReturnPDInfo(string PDNewNo)
        {
            var PDInfo = Service.ReturnPDInfo(PDNewNo);
            return Json(PDInfo, JsonRequestBehavior.AllowGet);
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