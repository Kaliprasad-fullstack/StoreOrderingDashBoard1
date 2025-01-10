using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DAL
{
    public class CategoryType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Created { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }
        public bool isDeleted { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
    [NotMapped]
    public class CategoryTypeView
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    [NotMapped]
    public class ItemSubTypeByCustomer
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
    }
}