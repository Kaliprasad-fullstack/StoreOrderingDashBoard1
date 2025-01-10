using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [NotMapped]
    public class ProcessCls
    {
        public long ID { get; set; }
        public string StoreName { get; set; }
        public string order_no { get; set; }
        public DateTime OrderDate { get; set; }
        public int order_to_be_chk { get; set; }
    }
}
