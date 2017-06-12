using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OPS.Models.OPSContext
{
    public class Product
    {
        [Key]
        [Required]
        [DisplayName("商品ID")]
        public string ProductId { get; set; }

        [StringLength(60)]
        [Required]
        [DisplayName("商品名稱")]
        public string ProductName { get; set; }

        [Required]
        [DisplayName("商品定價")]
        public decimal RegularPrice { get; set; }

        [DisplayName("圖片URL")]
        public string ImageURL { get; set; }

        [ForeignKey("PDCategory")]
        [DisplayName("商品類別ID")]// 外來鍵
        public int PDCategoryId { get; set; }

        public virtual PDCategory PDCategory { get; set; }

        //public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}