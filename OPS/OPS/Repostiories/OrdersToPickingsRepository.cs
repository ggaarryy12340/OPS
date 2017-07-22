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

        /// <summary>
        /// 產生製單紀錄PickingMakeLog
        /// </summary>
        /// <param name="CountOfPickings"></param>
        /// <param name="DeliveryWay"></param>
        /// <returns></returns>
        public bool CreatePickingMakeLog(int CountOfPickings, string DeliveryWay, out Guid PickingMakeLogId)
        {

            PickingMakeLog pickingMakeLog = new PickingMakeLog();
            PickingMakeLogId = pickingMakeLog.PickingMakeLogId;//回傳PickingMakeLogId參數

            pickingMakeLog.DeliveryWay = DeliveryWay;
            pickingMakeLog.PickingMakeLogTime = DateTime.Now;
            pickingMakeLog.RoundQty = CountOfPickings;
            db.PickingMakeLog.Add(pickingMakeLog);

            return db.SaveChanges() > 0;
        }

        /// <summary>
        /// 產生揀貨單Picking
        /// </summary>
        /// <param name="round"></param>
        /// <param name="PickingMakeLogId"></param>
        /// <param name="ThisPickingOrders"></param>
        public void MakeSinglePicking(int round, Guid PickingMakeLogId, List<Order> ThisPickingOrders)
        {
            Picking picking = new Picking();

            picking.PickingDateTime = DateTime.Now;
            picking.Round = round;
            picking.PickingMakeLogId = PickingMakeLogId;
            db.Picking.Add(picking);

            //定單狀態改為揀貨中，加入OrderLog揀貨中時間
            foreach (var item in ThisPickingOrders)
            {
                var Order = db.Order.FirstOrDefault(x => x.OrderId == item.OrderId);
                var OrderLog = db.OrderStatusLog.FirstOrDefault(x => x.OrderId == item.OrderId);

                OrderLog.OrderPickingTime = DateTime.Now;//紀錄訂單揀貨中時間

                Order.PickingId = picking.PickingId;//訂單加入揀貨單編號
                Order.OrderStatus = "2";//狀態:揀貨中
            }

            db.SaveChanges();
        }
    }
}