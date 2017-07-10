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

        [DisplayName("物流方式")]
        [Required]
        [StringLength(20)]
        public string DeliveryWay { get; set; }

        [DisplayName("訂單來源")]
        [Required]
        [StringLength(20)]
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
        [StringLength(5)]
        public string RecieveZipCode { get; set; }

        [DisplayName("收件人住址")]
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
        public decimal? Weight { get; set; }

        [DisplayName("物流追蹤號")]
        [StringLength(50)]
        public string TrackingNo { get; set; }

        [DisplayName("門市名稱")]
        [StringLength(20)]
        public string ConvenienceStoreName { get; set; }

        [DisplayName("門市店號")]
        [StringLength(20)]
        public string ConvenienceStoreNo { get; set; }

        [ForeignKey("Picking")]
        public Guid? PickingId { get; set; } // 外來鍵

        public virtual Picking Picking { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual ICollection<PickingMakeLog> PickingMakeLog { get; set; }

        public Order()
        {
            OrderDetails = new List<OrderDetail>();
            PickingMakeLog = new List<PickingMakeLog>();
            OrderId = Guid.NewGuid();
            OrderStatus = "1";
        }
    }
}