using OPS.Models;
using OPS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OPS.Controllers
{
    public class OrdersToPickingsController : Controller
    {
        private readonly OrdersToPickingsService _service = null;

        public OrdersToPickingsService Service
        {
            get { return _service ?? new OrdersToPickingsService(); }
        }

        // GET: OrdersToPickings
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Index(OrderToPickingParameters para)
        {
            bool rs = false;

            //表單驗證
            if (ModelState.IsValid)
            {
                var Orders = Service.FindOrders(para);
                Service.MakePicking(Orders, para.OrdersPerPicking, para.DeliveryWay);
            }
            return View(para);
            
        }
    }
}