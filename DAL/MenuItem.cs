using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("MenuItemTbl")]
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }
        public string MenuName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Fa { get; set; }
        public bool IsDeleted { get; set; }
        public int? ParentMenuId { get; set; }
        public virtual Role Role { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsHidden { get; set; }
        public int? Created { get; set; }
        public int? Modified { get; set; }
        public int? MenuOrderBy { get; set; }
        public virtual ICollection<Plan> Plans { get; set; }
    }
}
