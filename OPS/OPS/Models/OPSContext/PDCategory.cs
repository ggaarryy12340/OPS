using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OPS.Models.OPSContext
{
    public class PDCategory
    {
        [Key]
        [Required]
        [DisplayName("商品類別ID")]
        public int PDCategoryId { get; set; }

        [StringLength(50)]
        [Required]
        [DisplayName("商品類別名稱")]
        public string PDCategoryName { get; set; }
    }
}