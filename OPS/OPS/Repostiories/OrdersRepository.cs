﻿using OPS.Models;
using OPS.Models.DAL;
using OPS.Models.OPSContext;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Transactions;

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
                if (!string.IsNullOrEmpty(searchpara.DeliveryWay))
                {
                    ODList = ODList.Where(x => x.DeliveryWay == searchpara.DeliveryWay);
                }
                if (!string.IsNullOrEmpty(searchpara.Distributor))
                {
                    ODList = ODList.Where(x => x.Distributor == searchpara.Distributor);
                }
                if (!string.IsNullOrEmpty(searchpara.OrderStatus))
                {
                    ODList = ODList.Where(x => x.OrderStatus == searchpara.OrderStatus);
                }
                if (!string.IsNullOrEmpty(searchpara.OrderStatus))
                {
                    ODList = ODList.Where(x => x.OrderStatus == searchpara.OrderStatus);
                }
                //尋找區間
                if (searchpara.OrderDateStartAt != null && searchpara.OrderDateEndAt == null)
                {
                    ODList = ODList.Where(a => a.OrderDateTime >= searchpara.OrderDateStartAt).OrderByDescending(a => a.OrderDateTime);
                }
                if (searchpara.OrderDateStartAt == null && searchpara.OrderDateEndAt != null)
                {
                    ODList = ODList.Where(a => a.OrderDateTime <= searchpara.OrderDateEndAt).OrderByDescending(a => a.OrderDateTime);
                }
                if (searchpara.OrderDateStartAt != null && searchpara.OrderDateEndAt != null)
                {
                    //轉換時間為下午23:59
                    string date = string.Format("{0:yyyy-MM-dd}", searchpara.OrderDateEndAt);
                    string time = "23:59";
                    searchpara.OrderDateEndAt = Convert.ToDateTime(date + " " + time);

                    ODList = ODList.Where(a => a.OrderDateTime >= searchpara.OrderDateStartAt && a.OrderDateTime <= searchpara.OrderDateEndAt).OrderByDescending(a => a.OrderDateTime);
                }

            }

            return ODList.OrderByDescending(x => x.OrderDateTime).ToPagedList(Page, PageSize);
        }

        public Order GetSingleOrder(Guid id)
        {
            return db.Order.Find(id);
        }

        /// <summary>
        /// 新增訂單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CreateOrder(Order model)
        {
            db.Order.Add(model);
            return db.SaveChanges() > 0;
        }

        /// <summary>
        /// 新增訂單紀錄
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CreateOrderStatusLog(Order model)
        {
            OrderStatusLog newlog = new OrderStatusLog();
            newlog.OrderId = model.OrderId;
            db.OrderStatusLog.Add(newlog);
            return db.SaveChanges() > 0;
        }

        public bool OrdersEdit(Order model)
        {
            bool rs = false;
            var ODFromDb = db.Order.Find(model.OrderId);
            try
            {
                //修改訂單主檔
                ODFromDb.SourceOrderId = model.SourceOrderId;
                ODFromDb.OrderDateTime = model.OrderDateTime;
                ODFromDb.OrderStatus = model.OrderStatus;
                ODFromDb.DeliveryWay = model.DeliveryWay;
                ODFromDb.Distributor = model.Distributor;
                ODFromDb.RecieveName = model.RecieveName;
                ODFromDb.RecievePhone = model.RecievePhone;
                ODFromDb.RecieveZipCode = model.RecieveZipCode;
                ODFromDb.RecieveAddress = model.RecieveAddress;
                ODFromDb.OrderPrice = model.OrderPrice;
                ODFromDb.Feight = model.Feight;
                ODFromDb.Payment = model.Payment;
                ODFromDb.Weight = model.Weight;
                ODFromDb.TrackingNo = model.TrackingNo;
                ODFromDb.ConvenienceStoreName = model.ConvenienceStoreName;
                ODFromDb.ConvenienceStoreNo = model.ConvenienceStoreNo;

                //OrderDetails
                if (model.OrderDetails != null)
                {
                    foreach (var item in model.OrderDetails)
                    {
                        var npDetailDB = ODFromDb.OrderDetails.FirstOrDefault(x => x.OrderDetailId == item.OrderDetailId);
                        if (npDetailDB != null) //Modify
                        {
                            //修改訂單明細檔
                            npDetailDB.ProductId = item.ProductId;
                            npDetailDB.ProductName = item.ProductName;
                            npDetailDB.Spec = item.Spec;
                            npDetailDB.Quantity = item.Quantity;
                            npDetailDB.UnitPrice = item.UnitPrice;
                            npDetailDB.TotalPrice = item.TotalPrice;
                        }
                        else //新增訂單明細檔
                        {
                            item.OrderId = model.OrderId;
                            db.OrderDetail.Add(item);
                        }
                    }
                }
                rs = db.SaveChanges() > 0;
            }
            catch (Exception ex) { }

            return rs;
        }

        public Product ReturnPDInfo(string PDNo)
        {
            return db.Product.Find(PDNo);
        }
    }
}