using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TrackingBO
    {
        public string uniqueReferenceID { get; set; }
        public string SKUCode { get; set; }
        public string  SKUDesc { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string Invoice_Number { get; set; }
        public decimal? Invoice_Quantity { get; set; }

        public decimal? InvoiceQuantity { get; set; }
        public decimal BookQty { get; set; }
        public string TrackingStatus { get; set; }
        public string Status { get; set; }

        public DateTime? Booked { get; set; }
        public string Dispatch { get; set; }
        public string Delivered { get; set; }
        public string VehicleNo { get; set; }
        public Int32 SKUCnt { get; set; }
        public decimal TotalQty { get; set; }
        public DateTime? DeliveredDate { get; set; }
    }
}
