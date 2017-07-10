using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OPS.Models.OPSContext
{
    public class Picking
    {
        [Key]
        [Required]
        [DisplayName("揀貨單ID")]
        public Guid PickingId { get; set; }

        [DisplayName("製單時間")]
        public DateTime? PickingDateTime { get; set; }

        [DisplayName("輪數")]
        [Required]
        [StringLength(10)]
        public string Round { get; set; }

        [DisplayName("揀貨完成")]
        [Required]
        [StringLength(2)]
        public string IsComplete { get; set; }

        [ForeignKey("PickingMakeLog")]
        public Guid PickingMakeLogId { get; set; } // 外來鍵

        public virtual PickingMakeLog PickingMakeLog { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public Picking()
        {
            Orders = new List<Order>();
            PickingId = Guid.NewGuid();
            IsComplete = "N";
        }
    }
}