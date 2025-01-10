using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DAL
{
    [Table("EscalationAction")]
    public class EscalationAction
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Store")]
        public int StoreId { get; set; }
        public int Level { get; set; }
        [NotMapped]
        public int CustEmpId { get; set; }
        public virtual CustomerEmployee CustEmp { get; set; }
        public byte EmailTo_CC_BCCFlag { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }                
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public virtual Store Store { get; set; }
        [Display(Name = "Disable")]
        public bool IsDeleted { get; set; }
    }
}
