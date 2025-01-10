using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("NetSuitMapMst")]
    public class NetSuitMapCls
    {
        [Key]
        public int Id { get; set; }
        public string Customer { get; set; }
        public string Department { get; set; }
        public string Class { get; set; }
        public string Division { get; set; }
        public string TransactionType { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
    }
}
