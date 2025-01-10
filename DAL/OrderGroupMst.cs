using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("OrderGroupMst")]
    public class OrderGroupMst
    {
        [System.ComponentModel.DataAnnotations.Key]
        public long Id { get; set; }
        public int ClientId { get; set; }
        public long MaxGroupID { get; set; }
    }
}
