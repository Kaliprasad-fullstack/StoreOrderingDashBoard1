using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [NotMapped]
    public class Week
    {
        public string first_day_in_week { get; set; }
        public string last_day_in_week { get; set; }
    }
}
