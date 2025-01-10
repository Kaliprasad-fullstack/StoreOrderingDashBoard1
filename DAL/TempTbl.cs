using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("TempTbl")]
    public class TempTbl
    {
        [Key]
        public long Id { get; set; }
        public int ProductId { get; set; }
        public string ItemCode { get; set; }
        public string Skudescription { get; set; }
        public string Category { get; set; }
        public string UOM { get; set; }
        public decimal? MaxorderLimit { get; set; }
        public decimal Quantity { get; set; }
        public long headerid { get; set; }
    }
}
