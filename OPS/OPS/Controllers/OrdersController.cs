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
    }
}