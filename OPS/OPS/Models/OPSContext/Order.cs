using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OPS.Models.OPSContext
{
    public class Order
    {
        [Key]
        [Required]
        [DisplayName("訂單ID")]
        public Guid OrderId { get; set; }

        [StringLength(50)]
        [Required]
        [DisplayName("來源訂單編號")]
        public string SourceOrderId { get; set; }

        [DisplayName("訂單日期")]
        public DateTime? OrderDateTime { get; set; }

        [DisplayName("訂單狀態")]
        [Required]
        [StringLength(2)]
        public string OrderStatus { get; set; }

        [DisplayName("物流方式代碼")]
        [Required]
        [StringLength(2)]
        public string DeliveryWay { get; set; }

        [DisplayName("訂單來源代碼")]
        [Required]
        [StringLength(2)]
        public string Distributor { get; set; }

        [DisplayName("收件人姓名")]
        [Required]
        [StringLength(10)]
        public string RecieveName { get; set; }

        [DisplayName("收件人電話")]
        [Required]
        [StringLength(12)]
        public string RecievePhone { get; set; }

        [DisplayName("收件人郵遞區號")]
        [Required]
        [StringLength(5)]
        public string RecieveZipCode { get; set; }

        [DisplayName("收件人住址")]
        [Required]
        public string RecieveAddress { get; set; }

        [DisplayName("訂單金額")]
        [Required]
        public decimal OrderPrice { get; set; }

        [DisplayName("運費金額")]
        [Required]
        public decimal Feight { get; set; }

        [DisplayName("應付金額")]
        [Required]
        public decimal Payment { get; set; }

        [DisplayName("訂單重量")]
        public decimal Weight { get; set; }

        [DisplayName("物流追蹤號")]
        [StringLength(50)]
        public string TrackingNo { get; set; }

        [ForeignKey("Picking")]
        public Guid PickingId { get; set; } // 外來鍵

        public virtual Picking Picking { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual ICollection<PickingMakeLog> OrderStatusLogs { get; set; }

        public Order()
        {
            OrderDetails = new List<OrderDetail>();
            OrderStatusLogs = new List<PickingMakeLog>();
            OrderId = Guid.NewGuid();
        }
    }
}