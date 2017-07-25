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

        public void MakePicking(List<Order> Orders, decimal OrdersPerPicking, string DeliveryWay)
        {
            bool rs = false;
            Guid PickingMakeLogId;

            //揀貨單的數量
            int CountOfPickings = (int)(Math.Ceiling(Orders.Count() / OrdersPerPicking)); ;//無條件進位到整數

            //產生製單紀錄PickingMakeLog
            rs = Repository.CreatePickingMakeLog(CountOfPickings, DeliveryWay, out PickingMakeLogId);

            //產生揀貨單Picking
            if (rs)
            {
                for (var i = 1; i <= CountOfPickings; i++)
                {
                    var ThisPickingOrders = Orders.Skip((i-1) * (int)OrdersPerPicking).Take((int)OrdersPerPicking).ToList();//此揀貨單包含的訂單
                    Repository.MakeSinglePicking(i, PickingMakeLogId, ThisPickingOrders);
                }
            }
        }

        public void Dispose()
        {
            Repository.Dispose();
        }

    }
}