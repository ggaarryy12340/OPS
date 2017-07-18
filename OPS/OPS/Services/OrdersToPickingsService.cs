using OPS.Models;
using OPS.Models.OPSContext;
using OPS.Repostiories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPS.Services
{
    public class OrdersToPickingsService
    {
        private readonly OrdersToPickingsRepository _repository = null;

        public OrdersToPickingsRepository Repository
        {
            get { return _repository ?? new OrdersToPickingsRepository(); }
        }

        public List<Order> FindOrders(OrderToPickingParameters para)
        {
            return Repository.FindOrders(para);
        }

        public bool MakePicking(List<Order> Orders, int OrdersPerPicking)
        {
            //decimal x = Math.Ceiling(Orders.Count() / OrdersPerPicking);
            ////揀貨單的數量
            ////int CountOfPickings = Convert.ToInt32(Math.Ceiling(Orders.Count()/OrdersPerPicking));//無條件進位到整數

            ////for (int i = 0; i < CountOfPickings; i++)
            ////{
            ////    var ThisPickingOrders = Orders.Skip(i * OrdersPerPicking).Take(OrdersPerPicking);
            ////}
            return false;
        }
    }
}