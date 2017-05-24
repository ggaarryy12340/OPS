using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OPS.Models.OPSContext
{
    public class PickingMakeLog
    {
        [Key]
        [Required]
        [DisplayName("製單記錄ID")]
        public Guid PickingMakeLogId { get; set; }

        [DisplayName("製單紀錄時間")]
        public DateTime? PickingMakeLogTime { get; set; }

        [DisplayName("製單紀錄總輪數")]
        public int RoundQty { get; set; }

        [StringLength(2)]
        [DisplayName("物流方式代碼")]
        public string DeliveryWay { get; set; }

        public virtual ICollection<Picking> Pickings { get; set; }

        public PickingMakeLog()
        {
            PickingMakeLogId = Guid.NewGuid();
            Pickings = new List<Picking>();
        }
    }
}