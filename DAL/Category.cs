using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("CategoryMst")]
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string CategoryDescription { get; set; }
        public string CaseConversion { get; set; }
        public decimal TotalWeightPerCase { get; set; }
        public DateTime? Created { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }
        public bool isDeleted { get; set; }
        public virtual ICollection<Item> Items { get; set; }        
    }
}
