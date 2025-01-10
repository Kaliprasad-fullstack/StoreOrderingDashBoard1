using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("AuditTbl")]
    public class Audit
    {
        [Key]
        public long Id { get; set; }
        public string IpAddress { get; set; }
        public string JsonData { get; set; }
        public string Link { get; set; }
        public DateTime? CreatedOn { get; set; }
        public virtual Users Users { get; set; }
        public virtual Store Store { get; set; }
        [NotMapped]
        public int? UserId { get; set; }
        [NotMapped]
        public int? StoreId { get; set; }
    }
}
