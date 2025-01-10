using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("InvoiceDispatch")]
    public class InvoiceDispatch
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public string VehicleNo { get; set; }
        public DateTime DispatchDate { get; set; }


        [ForeignKey("CreatedByUser")]
        public int? CreatedBy { get; set; }
        public virtual Users CreatedByUser { get; set; }


        [ForeignKey("ModifiedByUser")]
        public int? ModifiedBy { get; set; }
        public virtual Users ModifiedByUser { get; set; }


        public int DeliveryStatus { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime SysDate { get; set; }


        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string DA { get; set; }
        [ForeignKey("OrderHeaders")]
        public Int64? OrderHeaderId { get; set; }
        public virtual OrderHeader OrderHeaders { get; set; }

        [ForeignKey("InvoiceHeader")]
        public Int64? InvoiceHeaderId { get; set; }
        public virtual InvoiceHeader InvoiceHeader{ get; set; }

        public int PODStatus { get; set; }
        public int Delivery_Re_Attempt { get; set; }
    }
 }
