using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ItemTypeDcMaster
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("Customers")]
        public int? Cust_Id { get; set; }
        public virtual Customer Customers { get; set; }

        [ForeignKey("WareHouseDCs")]
        public int? WareHouseDCs_Id { get; set; }
        public virtual WareHouseDC WareHouseDCs { get; set; }

        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }

        public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}
