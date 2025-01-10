using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("SoApproveDetailtbl")]
    public class SoApproveDetail
    {
        [Key]
        public long Id { get; set; }
        public string code { get; set; }
        public string quantity { get; set; }
        public string desc { get; set; }
        public string error { get; set; }
        public decimal? Original_SO_Quantity { get; set; }
        public virtual SoApproveHeader SoApproveHeader { get; set; }
    }
}
