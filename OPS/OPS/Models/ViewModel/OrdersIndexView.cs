using OPS.Models.OPSContext;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPS.Models.ViewModel
{
    public class OrdersIndexView
    {
        public IPagedList<Order> Orders { get; set; }

        public OrderSearchParameter OrderSearchParameter { get; set; }
    }
}