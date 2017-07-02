using OPS.Models;
using OPS.Models.OPSContext;
using OPS.Repostiories;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPS.Services
{
    public class OrdersService
    {
        private readonly OrdersRepository _repository = null;

        public OrdersRepository Repository
        {
            get { return _repository ?? new OrdersRepository(); }
        }

        public IPagedList<Order> GetOrdersList(OrderSearchParameter searchpara, int Page, int PageSize)
        {
            return Repository.GetOrdersList(searchpara, Page, PageSize);
        }
    }
}