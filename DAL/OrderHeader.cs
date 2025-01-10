using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("OrderHeader")]
    public class OrderHeader
    {
        public OrderHeader()
        {
        }
        public OrderHeader(Int64 Id, bool IsProcessed, bool IsOrderStatus, string DraftOrderno, string Finilizeorderno, DateTime? OrderDate, DateTime? ProcessedDate, bool isOrderEmailSent, int StoreId, int? ProcessedUserId, int? DCID, bool Isdeleted, ICollection<OrderDetail> orderDetails, string Remark, int? ModifiedBy, DateTime? ModifiedOn)
        {
            this.Id = Id;
            this.IsProcessed = IsProcessed;
            this.IsOrderStatus = IsOrderStatus;
            this.DraftOrderno = DraftOrderno;
            this.Finilizeorderno = Finilizeorderno;
            this.OrderDate = OrderDate;
            this.ProcessedDate = ProcessedDate;
            this.isOrderEmailSent = isOrderEmailSent;
            this.StoreId = StoreId;
            this.ProcessedUserId = ProcessedUserId;
            this.DCID = DCID;
            this.Isdeleted = Isdeleted;
            this.ModifiedBy = ModifiedBy;
            this.ModifiedOn = ModifiedOn;
            this.Remark = Remark;
            if (orderDetails != null)
            {
                this.orderDetails = new List<OrderDetail>();
                foreach (OrderDetail detail in orderDetails)
                    this.orderDetails.Add(new OrderDetail(detail.Id, detail.Quantity, detail.ItemId, detail.OrderHeaderId, null));
            }
        }
        [Key]
        public Int64 Id { get; set; }
        public bool IsProcessed { get; set; }
        public bool IsOrderStatus { get; set; }
        public bool IsSoApprove { get; set; }
        public string DraftOrderno { get; set; }
        public string Finilizeorderno { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public bool isOrderEmailSent { get; set; }
        [ForeignKey("Store")]
        public int StoreId { get; set; }
        public virtual Store Store { get; set; }
        public int? ProcessedUserId { get; set; }
        public int? DCID { get; set; }
        public bool IsFullFillment { get; set; }
        public bool IsInvoice { get; set; }
        public virtual ICollection<OrderDetail> orderDetails { get; set; }
        public bool Isdeleted { get; set; }
        public string SoNumber { get; set; }
        public string Remark { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        [NotMapped]
        public string order_no { get; set; }
        [NotMapped]
        public decimal OFRPercentage { get; set; }
        [NotMapped]
        public decimal LIFRPercentage { get; set; }

        public byte? OrderSource { get; set; }
        //public string Remark { get; set; }
        //public DateTime? ModifiedOn { get; set; }
        [NotMapped]
        public int? NoOfItems { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string OrderReferenceNo { get; set; }
        public string PONumber { get; set; }
    }

    public class OrderExcelData
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("Customer")]
        public int CustId { get; set; }
        public string OrderPlacedDate { get; set; }
        public string OrderType { get; set; }
        public string City { get; set; }
        public string LastOrderDate { get; set; }
        public string LateDeliveryDate { get; set; }
        public string UniqueReferenceID { get; set; }
        public string PackSize { get; set; }
        public string OrderedQty { get; set; }
        public string CaseSize { get; set; }
        public string Cases { get; set; }
        public string State { get; set; }
        public string FromDC { get; set; }
        public string New_Build { get; set; }
        public string Focus { get; set; }
        public string Item_Type { get; set; }
        public string Item_sub_Type { get; set; }
        public string TakenDays { get; set; }
        public string Slab { get; set; }
        public virtual Customer Customer { get; set; }
    }

    [Table(name: "ExcelData")]
    public class OrderExcel
    {
        [Key]
        public Int64 Id { get; set; }
        [ForeignKey("Customer")]
        public int CustId { get; set; }
        public virtual Customer Customer { get; set; }
        [NotMapped]
        public int Store_Order_Date_Id { get; set; }
        public DateTime? Store_Order_Date { get; set; }
        [NotMapped]
        public int Cust_Order_Date_Id { get; set; }
        public DateTime? Cust_Order_Date { get; set; }
        [NotMapped]
        public int Order_Type_Id { get; set; }
        public string Order_Type { get; set; }
        //
        [NotMapped]
        public int Store_Code_Id { get; set; }
        public string Store_Code { get; set; }
        public int? StoreId { get; set; }
        //
        [NotMapped]
        public int City_Id { get; set; }
        public string City { get; set; }
        public int? CityId { get; set; }
        [NotMapped]
        public int State_Id { get; set; }
        public string State { get; set; }
        [NotMapped]
        public int Last_Order_Date_Id { get; set; }
        public string Last_Order_Date { get; set; }
        [NotMapped]
        public int Last_Delivery_Date_Id { get; set; }
        public string Last_Delivery_Date { get; set; }
        [NotMapped]
        public int Order_Reference_Id_Id { get; set; }
        public string Order_Reference_Id { get; set; }
        [NotMapped]
        public int Unique_Reference_Id_Id { get; set; }
        public string Unique_Reference_Id { get; set; }
        [NotMapped]
        public int Item_Code_Id { get; set; }
        public string Item_Code { get; set; }
        public int? ItemId { get; set; }
        [NotMapped]
        public int Item_Desc_Id { get; set; }
        public string Item_Desc { get; set; }
        [NotMapped]
        public int Item_Type_Id { get; set; }
        public string Item_Type { get; set; }
        public int? ItemTypeId { get; set; }
        [NotMapped]
        public int Item_Sub_Type_Id { get; set; }
        public string Item_Sub_Type { get; set; }
        public int? ItemSubTypeId { get; set; }
        [NotMapped]
        public int Item_Category_Id { get; set; }
        public string Item_Category { get; set; }
        [NotMapped]
        public int Pack_Size_Id { get; set; }
        public string Pack_Size { get; set; }
        [NotMapped]
        public int Case_Size_Id { get; set; }
        public string Case_Size { get; set; }
        [NotMapped]
        public int Ordered_Qty_Id { get; set; }
        public string Ordered_Qty { get; set; }
        public decimal? Original_Ordered_Quantity { get; set; }
        [NotMapped]
        public int Number_Of_Cases_Id { get; set; }
        public string Number_Of_Cases { get; set; }
        [NotMapped]
        public int Source_Dc_Id { get; set; }
        public string Source_Dc { get; set; }
        [NotMapped]
        public int Priority_Id { get; set; }
        public string Priority { get; set; }
        public DateTime? System_Date_Time { get; set; }
        [NotMapped]
        public string Error { get; set; }

        [ForeignKey("Users")]
        public int? Upload_By { get; set; }
        public virtual Users Users { get; set; }
        public DateTime? Upload_Date { get; set; }

        public int Po_Processed_Flag { get; set; }
        [ForeignKey("ProcessedByUser")]
        public int? Po_Processed_By { get; set; }
        public virtual Users ProcessedByUser { get; set; }
        public DateTime? Po_Processed_Date { get; set; }

        public int? Approved_Flag { get; set; }
        [ForeignKey("Approved_ByUser")]
        public int? Approved_By { get; set; }
        public virtual Users Approved_ByUser { get; set; }
        public DateTime? Approved_Date { get; set; }

        public int? Validated_Flag { get; set; }
        [ForeignKey("Validated_ByUser")]
        public int? Validated_By { get; set; }
        public virtual Users Validated_ByUser { get; set; }
        public DateTime? Validated_Date { get; set; }

        public int CSV_Generated { get; set; }

        [ForeignKey("Modified_ByUser")]
        public int? Modified_By { get; set; }
        public virtual Users Modified_ByUser { get; set; }
        public DateTime? Modified_Date { get; set; }

        public int? Error_flag { get; set; }

        public long Xls_Line_No { get; set; }

        [ForeignKey("UploadedFilesMst")]
        public long? Uploaded_File_Id { get; set; }
        public virtual UploadedFilesMst UploadedFilesMst { get; set; }

        public long? Order_Detail_Id { get; set; }

        public int Isdeleted { get; set; }

        public string Remarks { get; set; }

        public bool Return_Authorization { get; set; }

        [NotMapped]
        public int? DCId { get; set; }

        [NotMapped]
        //public int ItemId { get; set; }
        public int? OrderSource { get; set; }
        public string ItemCategoryType { get; set; }
        public int? ItemCategoryTypeId { get; set; }
        public long? GroupId { get; set; }
        //public int? OrderConfirmationMail { get; set; }
        public string Remark { get; set; }
        public string Unique_Order_No { get; set; }
        [NotMapped]
        public int PONumber_Id { get; set; }
        public string PONumber { get; set; }
        public int? UOMMaster_Id { get; set; }
        public string UnitofMasureDescription { get; set; }
        [NotMapped]
        public int EnterpriseId { get; set; }
    }

    [NotMapped]
    public class ExcelOrder
    {
        public long Id { get; set; }
        public DateTime Store_Order_Date { get; set; }
        public string PONumber { get; set; }
        public string Store_Code { get; set; }
        public string Store_Name { get; set; }
        public string Item_Code { get; set; }
        public string Item_Desc { get; set; }
        public string UOM { get; set; }
        public int Threshold_Qty { get; set; }
        public decimal Ordered_Qty { get; set; }
        public string Priority { get; set; }
        public int Checked { get; set; }
        public int DCId { get; set; }
        public string DCName { get; set; }
        public int Category_Id { get; set; }
        public int StoreId { get; set; }
        public int ItemTypeId { get; set; }
        public string Uplaoded_By { get; set; }
        public DateTime? Upload_Date { get; set; }
        //New Items Added by Deepa 21/01/2021
        public int ItemId { get; set; }
        public decimal AutoAllocatedQty { get; set; } // This may change on UI
        public decimal InventoryAvailableQty { get; set; }
        public decimal TotalOrderedQty { get; set; }
        public string InventoryAvailableStatus { get; set; }
        public decimal? ConsumedQty { get; set; }
        public decimal? NetAvailableQty { get; set; }  // InventoryAvailable-TotalOrdered Qty on UI
        public string DCID_ItemID { get; set; }
        public decimal OriginalAutoAllocatedQty { get; set; } // THis Value will not change on UI
        public decimal? Original_Ordered_Quantity { get; set; } // THis Value will not change on UI
        public decimal? MinimumOrderQuantity { get; set; } // THis Value will not change on UI
        public string UnitofMasureDescription { get; set; }
        public decimal? Case_Size { get; set; }
        public string Remark { get; set; }
        public string Unique_Reference_Id { get; set; }
        public string EnterpriseName { get; set; }
        public string EnterpriseId { get; set; }
        public string ItemCategoryType { get; set; }
        public string Item_Type { get; set; }
        public string Brand { get; set; }
        public decimal? Revised_Ordered_Qty { get; set; }
        public decimal? RFPL_Ordered_Qty { get; set; }
        public decimal? Original_Ordered_Qty { get; set; }
        public string Ordered_By { get; set; }
        public string City { get; set; }
        public string PlaceOfSupply { get; set; }
        public string Item_Sub_Type { get; set; }
        public string Status { get; set; }
        public int CustId { get; set; }

    }

    public class TempExcelData
    {
        [Key]
        public Int64 Id { get; set; }

        [ForeignKey("OrderExcel")]
        public long OrderExcel_Id { get; set; }
        public virtual OrderExcel OrderExcel { get; set; }

        [ForeignKey("Customer")]
        public int? CustId { get; set; }
        public virtual Customer Customer { get; set; }

        public DateTime? Store_Order_Date { get; set; }

        public DateTime? Cust_Order_Date { get; set; }

        public string Order_Type { get; set; }

        public string Store_Code { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Last_Order_Date { get; set; }

        public string Last_Delivery_Date { get; set; }

        public string Order_Reference_Id { get; set; }

        public string Unique_Reference_Id { get; set; }

        public string Item_Code { get; set; }

        public string Item_Desc { get; set; }

        public string Item_Type { get; set; }

        public string Item_Sub_Type { get; set; }

        public string Item_Category { get; set; }

        public string Pack_Size { get; set; }

        public string Case_Size { get; set; }

        public string Ordered_Qty { get; set; }

        public string Number_Of_Cases { get; set; }

        public string Source_Dc { get; set; }

        public string Priority { get; set; }

        public DateTime? System_Date_Time { get; set; }

        [ForeignKey("Users")]
        public int? Upload_By { get; set; }
        public virtual Users Users { get; set; }
        public DateTime? Upload_Date { get; set; }

        public int Po_Processed_Flag { get; set; }
        [ForeignKey("ProcessedByUser")]
        public int? Po_Processed_By { get; set; }
        public virtual Users ProcessedByUser { get; set; }
        public DateTime? Po_Processed_Date { get; set; }

        public int? Approved_Flag { get; set; }
        [ForeignKey("Approved_ByUser")]
        public int? Approved_By { get; set; }
        public virtual Users Approved_ByUser { get; set; }
        public DateTime? Approved_Date { get; set; }

        public int? Validated_Flag { get; set; }
        [ForeignKey("Validated_ByUser")]
        public int? Validated_By { get; set; }
        public virtual Users Validated_ByUser { get; set; }
        public DateTime? Validated_Date { get; set; }

        public int CSV_Generated { get; set; }

        [ForeignKey("Modified_ByUser")]
        public int? Modified_By { get; set; }
        public virtual Users Modified_ByUser { get; set; }
        public DateTime? Modified_Date { get; set; }

        public int? Error_flag { get; set; }

        public long Xls_Line_No { get; set; }

        [ForeignKey("UploadedFilesMst")]
        public long? Uploaded_File_Id { get; set; }
        public virtual UploadedFilesMst UploadedFilesMst { get; set; }

        public long? Order_Detail_Id { get; set; }

        public int Isdeleted { get; set; }
        public int DCId { get; set; }

        public string ItemCategoryType { get; set; }
        public string Remark { get; set; }
        //public int? OrderConfirmationMail { get; set; }
        public string PONumber { get; set; }
    }

    [NotMapped]
    public class OrderSaveResponse
    {
        public Int64 Id { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
    }

    [NotMapped]
    public class CodeValuePair
    {
        public string StoreCode { get; set; }
        public long GroupId { get; set; }

    }

    [Table(name: "OrderMailData")]
    public class MailData
    {
        [Key]
        public Int64 Id { get; set; }
        public int CustId { get; set; }
        public string EmailAddress { get; set; }
        [MaxLength(10)]
        public string DocumentType { get; set; }
        public long? DocumentId { get; set; }
        public long? GroupId { get; set; }
        public int EmailTypeId { get; set; }
        public int? IsMailSend { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public class OrderMailContent
    {
        public DateTime Store_Order_Date { get; set; }
        public int StoreId { get; set; }
        public string OrderId { get; set; }
        public string Store_Code { get; set; }
        public string Item_Code { get; set; }
        public string Item_Desc { get; set; }
        public int ItemId { get; set; }
        public string Ordered_Qty { get; set; }
        public long GroupId { get; set; }
        public int CustId { get; set; }
        public string StoreEmailId { get; set; }
        public string Remark { get; set; }
        public long? DocumentId { get; set; }
    }

    //Haldiram Order Upload Format Cs
    public class OrderExcelMapping
    {
        [NotMapped]
        public int Unique_Order_No_Id { get; set; }
        public string Unique_Order_No { get; set; }
        [NotMapped]
        public int Order_Date_Id { get; set; }
        public DateTime? Order_Date { get; set; }
        [NotMapped]
        public int Store_Code_Id { get; set; }
        public string Store_Code { get; set; }
        [NotMapped]
        public int City_Id { get; set; }
        public string City { get; set; }
        public int? CityId { get; set; }
        [NotMapped]
        public int Item_Code_Id { get; set; }
        public string Item_Code { get; set; }
        public int? ItemId { get; set; }
        public int Ordered_Qty_Id { get; set; }
        public string Ordered_Qty { get; set; }
        [NotMapped]
        public int Source_Dc_Id { get; set; }
        public string Source_Dc { get; set; }

        [NotMapped]
        public string Error { get; set; }

        public int? CustId { get; set; }

        public int? Upload_By { get; set; }
        public DateTime? Upload_Date { get; set; }

        public long Xls_Line_No { get; set; }

        public long? Uploaded_File_Id { get; set; }
        [NotMapped]
        public long? GroupId { get; set; }
        public string Remark { get; set; }
        public string Sales_Order_No { get; set; }
        public int Sales_Order_No_Id { get; set; }

        public string SO_Status { get; set; }
        public int SO_Status_Id { get; set; }

        public DateTime? Sales_Order_Date { get; set; }
        public int Sales_Order_Date_Id { get; set; }

        public string Sales_Order_Qty { get; set; }
        public int Sales_Order_Qty_Id { get; set; }

        public string Invoice_No { get; set; }
        public int Invoice_No_Id { get; set; }

        public DateTime? Invoice_Date { get; set; }
        public int Invoice_Date_Id { get; set; }

        public string Invoice_Qty { get; set; }
        public int Invoice_Qty_Id { get; set; }

        public DateTime? Dispatch_Date { get; set; }
        public int Dispatch_Date_Id { get; set; }

        public string Vehicle_No { get; set; }
        public int Vehicle_No_Id { get; set; }

        public string Delivery_Status { get; set; }
        public int Delivery_Status_Id { get; set; }

        public string Delivery_ReAttempt { get; set; }
        public int Delivery_ReAttempt_Id { get; set; }

        public DateTime? POD_Date { get; set; }
        public int POD_Date_Id { get; set; }

        public string Actual_Reporting_Time { get; set; }
        public int Actual_Reporting_Time_Id { get; set; }

        public string Undelivered_Reason { get; set; }
        public int Undelivered_Reason_Id { get; set; }

        public string Delivered_Qty { get; set; }
        public int Delivered_Qty_Id { get; set; }

        public string Is_OnTimeDelivery { get; set; }
        public int Is_OnTimeDelivery_Id { get; set; }

        public string ISR_NO { get; set; }
        public int ISR_NO_Id { get; set; }

    }

    public class OrderExcelInward
    {
        public string PO_Number { get; set; }
        public int PO_Number_Id { get; set; }

        public DateTime? PO_Date { get; set; }
        public int PO_Date_Id { get; set; }

        public string Vendor_Name { get; set; }
        public int Vendor_Name_Id { get; set; }

        public string Status { get; set; }
        public int Status_Id { get; set; }

        public DateTime? GRN_Date { get; set; }
        public int GRN_Date_Id { get; set; }

        public string GRN_No { get; set; }
        public int GRN_No_Id { get; set; }

        public DateTime? SupplierDoc_Date { get; set; }
        public int SupplierDoc_Date_Id { get; set; }

        public string SupplierDoc_Ref { get; set; }
        public int SupplierDoc_Ref_Id { get; set; }

        public string Vehicle_No { get; set; }
        public int Vehicle_No_Id { get; set; }

        public string Remark { get; set; }
        public int Remark_Id { get; set; }

        public string Class { get; set; }
        public int Class_Id { get; set; }

        public string Location { get; set; }
        public int Location_Id { get; set; }

        public string SKU_Code { get; set; }
        public int SKU_Code_Id { get; set; }

        public string SKU_Description { get; set; }
        public int SKU_Description_Id { get; set; }

        public string SKU_Category { get; set; }
        public int SKU_Category_Id { get; set; }

        public string PO_Qty { get; set; }
        public int PO_Qty_Id { get; set; }

        public string GRN_Qty { get; set; }
        public int GRN_Qty_Id { get; set; }

        public string VRA_Qty { get; set; }
        public int VRA_Qty_Id { get; set; }

        public int? CustId { get; set; }

        public int? Upload_By { get; set; }
        public DateTime? Upload_Date { get; set; }
        public long Xls_Line_No { get; set; }
        public long? Uploaded_File_Id { get; set; }
        public long? GroupId { get; set; }
        public string Error { get; set; }
    }

    public class OrderInventory
    {
        public string SKU_Code { get; set; }
        public int SKU_Code_Id { get; set; }

        public string SKU_Description { get; set; }
        public int SKU_Description_Id { get; set; }

        public string SKU_Category { get; set; }
        public int SKU_Category_Id { get; set; }

        public string DC_Code { get; set; }
        public int DC_Code_Id { get; set; }

        public string UOM { get; set; }
        public int UOM_Id { get; set; }

        public string On_Hand { get; set; }
        public int On_Hand_Id { get; set; }

        public string QC { get; set; }
        public int QC_Id { get; set; }

        public int? CustId { get; set; }

        public int? Upload_By { get; set; }
        public DateTime? Upload_Date { get; set; }
        public long Xls_Line_No { get; set; }
        public long? Uploaded_File_Id { get; set; }
        public string Error { get; set; }
    }

    [NotMapped]
    public class NetSuiteInventory
    {
        public DateTime OnHandDate { get; set; }
        public int SKUID { get; set; }
        public string SKUCode { get; set; }
        public string SKUName { get; set; }
        public int DCId { get; set; }
        public string DCName { get; set; }
        public decimal? ConsumedQty { get; set; }
        public decimal AvailableQty { get; set; }
        public decimal OrderedQty { get; set; }
        public string AvailabilityStatus { get; set; }
        public decimal? TempAllocatedQty { get; set; }
        public decimal? NetAvailableQty { get; set; }  // InventoryAvailable-TotalOrdered Qty on UI
        public decimal OriginalAutoAllocatedQty { get; set; } // THis Value will not change on UI

    }

    [NotMapped]
    public class GBPA
    {
        public DateTime TS { get; set; }
        public int SKUID { get; set; }
        public long ID { get; set; }
        public string GBPA_NUMBER { get; set; }
        public string REVISION_NUMBER { get; set; }
        public string ITEM_NUMBER { get; set; }
        public string ITEM_DESCRIPTION { get; set; }
        public string UOM { get; set; }
        public decimal? RATE { get; set; }
        public int CUST_ID { get; set; }
        public int IS_Deleted { get; set; }
    }
    [NotMapped]
    public class UpdateOrderQuantity
    {
        public int id { get; set; }
        public int orderno { get; set; }
        public int OrderQuantity { get; set; }
        public string quantity { get; set; }

    }
}