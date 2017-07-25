using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OPS.Models.OPSContext
{
    public class PickingProductCheck
    {
        [Key]
        [Required]
        [DisplayName("揀貨單商品ID")]
        public Guid pickingProductCheckId { get; set; }

        [DisplayName("圖片URL")]
        public string ProductImageURL { get; set; }

        [DisplayName("商品數量")]
        public int? Quantity { get; set; }

        [DisplayName("已取商品數量")]
        public int? CheckedQuantity { get; set; }

        [StringLength(2)]
        [DisplayName("取貨完成")]
        public string IsChecked { get; set; }

        [ForeignKey("Picking")]
        [DisplayName("訂單編號")]
        public Guid PickingId { get; set; } // 外來鍵

        [ForeignKey("Product")]
        [DisplayName("商品編號")]// 外來鍵
        public string ProductId { get; set; }

        public virtual Picking Picking { get; set; }

        public virtual Product Product { get; set; }

        public PickingProductCheck()
        {
            pickingProductCheckId = Guid.NewGuid();
            IsChecked = "N";
        }
    }
}