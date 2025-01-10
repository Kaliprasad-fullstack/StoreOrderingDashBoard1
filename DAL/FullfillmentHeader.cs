using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("FullfillmentHeaderTbl")]
    public class FullfillmentHeader
    {
        [Key]
        public long Id { get; set; }
        public string Internal_Id { get; set; }
        public string CreatedFrom { get; set; }
        public string PostingPeriod { get; set; }
        public string Customer { get; set; }
        public string Date { get; set; }
        public string Division { get; set; }
        public string TotalNOCase { get; set; }
        public string CreateBy { get; set; }
        public string EmployeeDepartment { get; set; }
        public DateTime? CreatedOn { get; set; }

        [ForeignKey("OrderHeaders")]
        public Int64? OrderHeaderId { get; set; }
        public virtual OrderHeader OrderHeaders { get; set; }

        public virtual ICollection<FullFillmentDetail> FullFillmentDetails { get; set; }
    }
}
