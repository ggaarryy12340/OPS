using OPS.Models;
using OPS.Models.DAL;
using OPS.Models.OPSContext;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPS.Repostiories
{
    public class OrdersRepository
    {
        private readonly OPSContext _db = new OPSContext();

        public OPSContext db
        {
            get
            {
                return _db;
            }
        }

        public IPagedList<Order> GetOrdersList(OrderSearchParameter searchpara, int Page, int PageSize)
        {
            var ODList = db.Order.Include("OrderDetails").AsQueryable();

            if (searchpara != null)
            {
                if (searchpara.OrderId != Guid.Empty)
                {
                    ODList = ODList.Where(x => x.OrderId == searchpara.OrderId);
                }
                if (!string.IsNullOrEmpty(searchpara.SourceOrderId))
                {
                    ODList = ODList.Where(x => x.SourceOrderId == searchpara.SourceOrderId);
                }
            }

            return ODList.OrderByDescending(x => x.OrderDateTime).ToPagedList(Page, PageSize);
        }
    }
}