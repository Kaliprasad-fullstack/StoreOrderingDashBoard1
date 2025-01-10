using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [NotMapped]
    public class ReportCount
    {
       // public int Id { get; set; }
        public string Name { get; set; }
        public int Total_Store_Count { get; set; }
        public int Total_order_count { get; set; }
        public int Orders_with_in_time_frame { get; set; }
        public int Orders_with_out_time_frame { get; set; }
    }
}
