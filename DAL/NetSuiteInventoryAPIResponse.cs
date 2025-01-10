using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class NetSuiteInventoryAPIResponse
    {
        public string asOnDate { get; set; }
        public string dcLocation { get; set; }
        public string SKUCode { get; set; }
        public decimal? onhandQty { get; set; }
        public decimal? availableQty { get; set; }
        public decimal? commitedQty { get; set; }
    }
}

