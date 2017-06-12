using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OPS.Models.OPSContext
{
    public class OrderDetail
    {
        [Key]
        [Required]
        [DisplayName("訂單明細ID")]
        public Guid OrderDetailId { get; set; }

        [StringLength(100)]
        [DisplayName("商品名稱")]
        public string ProductName { get; set; }

        [StringLength(20)]
        [DisplayName("商品規格")]
        public string Spec { get; set; }

        [DisplayName("商品數量")]
        public int Quantity { get; set; }

        [DisplayName("商品單價")]
        public decimal UnitPrice { get; set; }

        [DisplayName("商品總價")]
        public decimal TotalPrice { get; set; }

        [ForeignKey("Order")]
        [DisplayName("訂單編號")]
        public Guid OrderId { get; set; } // 外來鍵

        [ForeignKey("Product")]
        [DisplayName("商品編號")]// 外來鍵
        public string ProductId { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }

        public OrderDetail()
        {
            OrderDetailId = Guid.NewGuid();
        }
    }
}