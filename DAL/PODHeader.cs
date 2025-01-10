using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("PODHeaderTbl")]
    public class PODHeader
    {
        public PODHeader(Int64 Id, string Invoice_Number, DateTime? PODDate, int? CreatedBy)
        {
            this.Id = Id;
            this.Invoice_Number = Invoice_Number;
            this.PODDate = PODDate;
            this.CreatedBy = CreatedBy;
        }
        public PODHeader()
        {
        }
        [Key]
        public long Id { get; set; }
        public string Invoice_Number { get; set; }

//        [ForeignKey("Users")]
        public int? CreatedBy { get; set; }
  //      public virtual Users Users { get; set; }
        public DateTime? PODDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string PODSignImageFile { get; set; }
        public string PODTempImageFile { get; set; }
        public int? CompanyId { get; set; }
        public Int32? InvoiceDispatchId { get; set; }
        public int DeliveryStatus { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime SysDate { get; set; }
        public Int64? OrderHeaderId { get; set; }
        public int Reason { get; set; }
        public virtual ICollection<PODDetail> InvoiceDetails { get; set; }
        [ForeignKey("InvoiceHeader")]
        public Int64? InvoiceHeaderId { get; set; }
        public virtual InvoiceHeader InvoiceHeader { get; set; }
        public int IsOnTimeDelivery { get; set; }
    }
    [Table("PODDetailTbl")]
    public class PODDetail
    {
        public PODDetail()
        {
        }
        public PODDetail(long pk_orderDetail, decimal invoiceQuantity, decimal deliveredQuantity, long? pODHeaderId)
        {
            this.Id = pk_orderDetail;
            this.InvoiceQuantity = invoiceQuantity;
            this.DeliveredQuantity = deliveredQuantity;
            this.PODHeaderId = PODHeaderId;
        }

        [Key]
        public long Id { get; set; }
        public string Item { get; set; }
        public int? ItemId { get; set; }
        public string Description { get; set; }
        public decimal InvoiceQuantity { get; set; }
        public decimal DispatchQuantity { get; set; }
        public decimal DeliveredQuantity { get; set; }
        public decimal? Original_Invoice_Quantity { get; set; }
        public decimal? Original_Delivered_Quantity { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int Reason { get; set; }
        public int Status { get; set; }
        [ForeignKey("PODHeader")]
        public long PODHeaderId { get; set; }
        public virtual PODHeader PODHeader { get; set; }        
    }
    [NotMapped]
    public class Vehicle
    {
        public string VehicleNo { get; set; }
    }
}