using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("CustomerPlanMst")]
    public class CustomerPlan
    {
        [Key]
        public int Id { get; set; }
        public string PlanName { get; set; }
        public int Stores { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? Created { get; set; }
        public int? Modified { get; set; }
        public virtual ICollection<Plan> Plans { get; set; }
        public virtual ICollection<Users> Users { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        //public virtual ICollection<Store> Store { get; set; }

    }
}
