using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("PermitableTbl")]
    public class PermitableCls
    {
        public int Id { get; set; }
        public virtual Users Users { get; set; }
        public virtual WareHouseDC WareHouseDC { get; set; }
        public virtual Customer Customer { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public virtual Store Store { get; set; }        
        public virtual Company Company { get; set; }
        public bool IsDeleted { get; set; }
        [NotMapped]
        public int? StoreId { get; set; }
    }
    [NotMapped]
    public class ViewPermitableCls
    {
        public int Id { get; set; }
        public int Customer_Id { get; set; }
        public int? WareHouseDC_Id { get; set; }
        public int? Users_Id { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int? Store_Id { get; set; }
    }
}
