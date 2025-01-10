using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("PODHeaderArchive")]
    public class PODHeaderArchive
    {
        [Key]
        public long Id { get; set; }
        public long PODHId { get; set; }
        public string Invoice_Number { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? PODDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string PODSignImageFile { get; set; }
        public string PODTempImageFile { get; set; }
        public int? CompanyId { get; set; }
        public Int32? InvoiceDispatchId { get; set; }
        public int DeliveryStatus { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? SysDate { get; set; }
        public Int64? OrderHeaderId { get; set; }
        public int Reason { get; set; }
      
    }
}
