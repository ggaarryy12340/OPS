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

        public Order GetSingleOrder(Guid id)
        {
            return Repository.GetSingleOrder(id);
        }

        public bool CreateOrder(Order model)
        {
            var rs1 = false;
            var rs2 = false;

            rs1 = Repository.CreateOrder(model);
            if (rs1)
            {
                rs2 = Repository.CreateOrderStatusLog(model);
            }

            return rs2;
        }

        public bool OrdersEdit(Order model)
        {
            return Repository.OrdersEdit(model);
        }

        public Product ReturnPDInfo(string PDNo)
        {
            return Repository.ReturnPDInfo(PDNo);
        }
    }
}