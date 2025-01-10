using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("InvoiceHeaderTbl")]
    public class InvoiceHeader
    {
        [Key]
        public long Id { get; set; }
        public string Internal_Id { get; set; }
        public string Invoice_Number { get; set; }
        public string DueDate { get; set; }
        public string Project_Name { get; set; }
        public string CreatedFrom { get; set; }
        public string InvoiceDate { get; set; }
        public string PostingPeriod { get; set; }
        public string CustomerZone { get; set; }
        public string Memo { get; set; }
        public string Department { get; set; }
        public string Class { get; set; }
        public string Location { get; set; }
        public int DC_Location { get; set; }
        public string Division { get; set; }
        public string EmployeeDepartment { get; set; }
        public string SOApproverPartner { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
        [NotMapped]
        public DateTime? DispatchDate { get; set; }
        [NotMapped]
        public string VehicleNo { get; set; }
        [ForeignKey("OrderHeaders")]
        public Int64? OrderHeaderId { get; set; }
        public virtual OrderHeader OrderHeaders { get; set; }
        [ForeignKey("SoApproveHeaders")]
        public Int64? SoApproveHeaderId { get; set; }
        public virtual SoApproveHeader SoApproveHeaders { get; set; }
        
    }
}
