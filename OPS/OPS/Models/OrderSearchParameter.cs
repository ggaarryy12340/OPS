using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPS.Models
{
    public class OrderSearchParameter
    {
        public Guid OrderId { get; set; }

        public string SourceOrderId { get; set; }

        public DateTime? OrderDateStartAt { get; set; }

        public DateTime? OrderDateEndAt { get; set; }

        public string OrderStatus { get; set; }

        public string DeliveryWay { get; set; }

        public string Distributor { get; set; }
    }
}