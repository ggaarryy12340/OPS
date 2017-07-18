using OPS.Models.DAL;
using OPS.Models.OPSContext;
using OPS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPS.Repostiories
{
    public class OrdersToPickingsRepository
    {
        private readonly OPSContext _db = new OPSContext();

        public OPSContext db
        {
            get
            {
                return _db;
            }
        }

        public List<Order> FindOrders(OrderToPickingParameters para)
        {
            //只選訂單狀態:訂單成立
            var ODList = db.Order.Include("OrderDetails").Where(x => x.OrderStatus == "1").AsQueryable();

            if (para != null)
            {
                if (!string.IsNullOrEmpty(para.DeliveryWay))
                {
                    ODList = ODList.Where(x => x.DeliveryWay == para.DeliveryWay);
                }
                if (!string.IsNullOrEmpty(para.Distributor))
                {
                    ODList = ODList.Where(x => x.Distributor == para.Distributor);
                }
                //尋找區間
                if (para.OrderStartAt != null && para.OrderEndAt != null)
                {
                    //轉換時間為下午23:59
                    string date = string.Format("{0:yyyy-MM-dd}", para.OrderEndAt);
                    string time = "23:59";
                    para.OrderEndAt = Convert.ToDateTime(date + " " + time);

                    ODList = ODList.Where(a => a.OrderDateTime >= para.OrderStartAt && a.OrderDateTime <= para.OrderEndAt).OrderBy(a => a.OrderDateTime);
                } 
            }

            return ODList.ToList();
        }
    }
}