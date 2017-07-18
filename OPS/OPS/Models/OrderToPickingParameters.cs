using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OPS.Models
{
    public class OrderToPickingParameters
    {
        [DisplayName("訂單起始日")]
        [Required]
        public DateTime OrderStartAt { get; set; }

        [DisplayName("訂單結束日")]
        [Required]
        public DateTime OrderEndAt { get; set; }

        [DisplayName("訂單來源")]
        public string Distributor { get; set; }

        [DisplayName("物流方式")]
        public string DeliveryWay { get; set; }

        [DisplayName("訂單狀態")]
        public int? OrderStatus { get; set; }

        [DisplayName("訂單數/揀貨單")]
        public int OrdersPerPicking { get; set; }
    }
}