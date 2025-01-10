using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("UOMMst")]
    public class UOMmaster
    {
        [Key]
        public int Id { get; set; }
        public string UOMMaster { get; set; }
        public string UnitofMasureDescription { get; set; }
        public DateTime? Created { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        //public virtual ICollection<Product> Products { get; set; }
    }
}
