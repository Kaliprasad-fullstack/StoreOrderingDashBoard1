using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DAL
{
    [Table("EscalationLevelMaster")]
    public class EscalationLevelMaster
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Customer")]
        public int CustId { get; set; }
        public int? TriggerType { get; set; }
        public int? ProcessId { get; set; }
        public int LevelID { get; set; }
        public TimeSpan TriggerTime { get; set; }        
        public int? CreatedBy { get; set; }        
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }   
        [Display(Name ="Disable")]
        public bool IsDeleted { get; set; }
        public virtual Customer Customer { get; set; }

    }
    public class CustomerEmployee
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Customer")]
        public int CustId { get; set; }
        public virtual Customer Customer { get; set; }
        public string EmpName { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<EscalationAction> Escalations { get; set; }
    }
}
