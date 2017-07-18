using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OPS.Models.OPSContext
{
    public class OrderStatusLog
    {
        [Key]
        [Required]
        [DisplayName("訂單狀態紀錄Id")]
        public Guid OrderStatusLogId { get; set; }

        [DisplayName("訂單成立時間")]
        public DateTime? OrderCreateTime { get; set; }

        [DisplayName("揀貨中時間")]
        public DateTime? OrderPickingTime { get; set; }

        [DisplayName("包裝中時間")]
        public DateTime? OrderPackingTime { get; set; }

        [DisplayName("包裝完成時間")]
        public DateTime? OrderAlreadyTime { get; set; }

        [DisplayName("已出貨時間")]
        public DateTime? OrderDeliveryTime { get; set; }

        [ForeignKey("Order")]
        public Guid OrderId { get; set; } // 外來鍵

        public virtual Order Order { get; set; }

        public OrderStatusLog()
        {
            OrderStatusLogId = Guid.NewGuid();
            OrderCreateTime = DateTime.Now;
        }
    }
}