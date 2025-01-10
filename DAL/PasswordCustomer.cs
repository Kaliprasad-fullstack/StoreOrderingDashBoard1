using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("PasswordCustomerTbl")]
    public class PasswordCustomer
    {
        [Key]
        public int Id { get; set; }
       public virtual Customer Customer { get; set; }
        public virtual Users Users { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Password { get; set; }
        //public string Answer { get; set; }
       // public virtual Question Question { get; set; }
        public bool IsDeleted { get; set; }
    }
}
