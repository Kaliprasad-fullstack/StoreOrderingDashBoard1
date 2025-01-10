using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [NotMapped]
    public class Menu
    {
        public int Id { get; set; }
        public string MenuName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Fa { get; set; }
        public bool IsDeleted { get; set; }
        public int? ParentMenuId { get; set; }
        public bool IsShow { get; set; }
        public int? PlanId { get; set; }
        public int custplanid { get; set; }
    }
}
