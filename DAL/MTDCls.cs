using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MTDCls
    {
        public string Name { get; set; }
        public int Finalized_order_count { get; set; }
        public int Processed_order_count { get; set; }
        public int SO_Approved_count { get; set; }
        public int Fullfilled_count { get; set; }
        public int Invoiced_count { get; set; }
    }
}
