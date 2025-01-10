using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class PODheaderBO
    {
        public Int32? InvoiceDispatchId { get; set; }
        public int DeliveryStatus { get; set; }
        public string DeliveryDate { get; set; }
        public string Invoice_Number { get; set; }
        public int Reason { get; set; }
        public int? CreatedBy { get; set; }
        public int Reattempte { get; set; }
    }

    public class PODBulkUpdate
    {
        public Int32? InvoiceDispatchId { get; set; }
        public int DeliveryStatus { get; set; }
        public string DeliveryDate { get; set; }
        public string Invoice_Number { get; set; }
        public int? Reason { get; set; }
        public int? StoreId { get; set; }
        public string StoreCode { get; set; }
        public String StoreName { get; set; }
        public long OrderHeaderId { get; set; }
        public int? CreatedBy { get; set; }
        public int? Reattempte { get; set; }
        public ICollection<InvoiceDetail> InvoiceDetails { get; set; }
        public ICollection<BulkPODResponse> BulkPODResponse { get; set; }
        
    }
    public class BulkPODResponse
    {
        public string Item { get; set; }
        public string Description { get; set; }
        public int ItemId { get; set; }
        public decimal InvoiceQty { get; set; }
        public decimal DeliveredQty { get; set; }
        public int? Reason { get; set; }
        public decimal DispatchQty { get; set; }
        public string ItemCode { get; set; }
        public string Invoice_number { get; set; }
        public long? PODHeaderId { get; set; }
        public bool IsDataSaved { get; set; }

    }
    public class ViewPODBulkUpdate
    {
        public virtual ICollection<PODBulkUpdate> PODBulkUpdates { get; set; }
    }
}
