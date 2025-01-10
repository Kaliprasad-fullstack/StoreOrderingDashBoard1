using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class FinalReport
    {
        public string dc_name { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string StoreManager { get; set; }
        public string StoreContactNo { get; set; }
        public string OrderNo { get; set; }
        public DateTime? OrderDate { get; set; }
        public string SKUCode { get; set; }
        public string SKUName { get; set; }
        public int requested_Quantity { get; set; }
        public string Processed_Flag { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string SO_Approved_Flag { get; set; }
        public string SoNumber { get; set; }
        public string Fullfilled_Flag { get; set; }
        public string fullfill_quantity { get; set; }
        public string Invoiced_Flag { get; set; }
        public string Invoice_Number { get; set; }
        public string InvoiceDate { get; set; }
        public string DueDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string TaxCode { get; set; }
        public string TaxRate { get; set; }
        public string Amount { get; set; }
        public string TaxAmount { get; set; }
        public string GrossAmount { get; set; }
        public string TaxLiable { get; set; }
        public string ItemCategory { get; set; }
        public string inv_Quantity { get; set; }
    }

    public class UniqueRefenenceReport
    {
        [Display(Name = "Unique Reference ID")]
        public string UniqueReferenceID { get; set; }
        [Display(Name = "Date of Order")]
        public DateTime? OrderDate { get; set; }
        [Display(Name = "Order Placed Date")]
        public string OrderPlacedDate { get; set; }
        public string OrderType { get; set; }
        public string StoreCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Focus { get; set; }
        public string LastOrderDate { get; set; }
        public string LastDeliveryDate { get; set; }
        public string SKUCode { get; set; }
        public string SKUName { get; set; }
        public string Item_Type { get; set; }
        public string Item_sub_Type { get; set; }
        public string OrderedQty { get; set; }
        public string PackSize { get; set; }
        public string CaseSize { get; set; }
        public string Cases { get; set; }
        public string New_Build { get; set; }
        public string order_status { get; set; }
        public string invoice_status { get; set; }
        public string FromDC { get; set; }
        public string PlanDate { get; set; }
        public string ord_no { get; set; }
        public string SoNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string Invoice_Number { get; set; }
        public string inv_Quantity { get; set; }
        public string DeliveryDate { get; set; }
        public string TakenDays { get; set; }
        public string Slab { get; set; }
        public string VehicleNo { get; set; }
        public string DriverName { get; set; }
        public string DriverContactNo { get; set; }
        public string DocDate { get; set; }
        public string Remarks { get; set; }
        public string OrderReferenceId { get; set; }
        public string Return_Authorization { get; set; }

        public string ord_no_tojoin { get; set; }
        public string dtl_uri { get; set; }
        public string dc_name { get; set; }
        public string StoreName { get; set; }
        public string StoreManager { get; set; }
        public string StoreContactNo { get; set; }
        public string dtl_ord_no_tojoin { get; set; }
        public int requested_Quantity { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string fullfill_quantity { get; set; }
        public string DueDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string TaxCode { get; set; }
        public string TaxRate { get; set; }
        public string Amount { get; set; }
        public string TaxAmount { get; set; }
        public string GrossAmount { get; set; }
        public string TaxLiable { get; set; }
        public string ItemCategory { get; set; }
    }
    public class DailySalesReport
    {
        public string PONumber { get; set; }
        public string Unique_Reference_Id { get; set; }
        public DateTime? Store_Order_Date { get; set; }
        public DateTime? Cust_Order_Date { get; set; }
        public string Order_Type { get; set; }
        public string Store_Code { get; set; }
        public string StoreName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PlaceOfSupply { get; set; }
        public string Priority { get; set; }
        public string Last_Order_Date { get; set; }
        public string Last_Delivery_Date { get; set; }
        public string Item_Code { get; set; }
        public string Item_Desc { get; set; }
        public string UnitofMasureDescription { get; set; }
        public string Item_Type { get; set; }
        public string BrandName { get; set; }
        public string Item_Sub_Type { get; set; }
        public decimal? Revised_Ordered_Qty { get; set; }
        public decimal? Rfpl_Ordered_Qty { get; set; }
        public decimal? Original_Ordered_Qty { get; set; }
        public string Pack_Size { get; set; }
        public string Case_Size { get; set; }
        public string Number_Of_Cases { get; set; }
        public string Item_Category { get; set; }
        public string Invoice_Status { get; set; }
        public string Order_Status { get; set; }
        public string Dc_Name { get; set; }
        public string PlanDate { get; set; }
        public string OrderNo { get; set; }
        public DateTime? PODate { get; set; }
        public string Invoice_Number { get; set; }
        public decimal? So_Quantity { get; set; }
        public decimal? Invoice_Quantity { get; set; }
        public decimal? Invoice_Amount { get; set; }
        public string RANumber { get; set; }
        public decimal? RAQuantity { get; set; }
        public decimal? RAAmount { get; set; }
        public DateTime? RADate { get; set; }
        public decimal? FillRateGap { get; set; }
        public string Invoice_Date { get; set; }
        public string Delivered_Status1 { get; set; }
        public string Delivered_Date1 { get; set; }
        public string Delivered_Status2 { get; set; }
        public string Delivered_Date2 { get; set; }
        public decimal? Delivered_Qty { get; set; }
        public string Return_Reason { get; set; }
        public string Taken_Days { get; set; }
        public string Slab { get; set; }
        public string EnterpriseName { get; set; }
        public string EnterpriseCode { get; set; }
        public string Remarks { get; set; }
        public string Order_Reference_Id { get; set; }
        public string ItemCategoryType { get; set; }
        public DateTime Upload_Date { get; set; }
        public string SODate { get; set; }
        public string SoNumber { get; set; }
        public decimal? SoQuantity { get; set; }
        public string Customer { get; set; }
        public decimal storemst_storeid { get; set; }
        //public string StoreCode { get; set; }
        public string SKUCode { get; set; }
        public string SKUName { get; set; }
        public string Item_Status { get; set; }
        public string MinimumOrder { get; set; }
        public string MaximumOrderLimit { get; set; }
        public string CaseConversion { get; set; }
        public string UOM { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }

    }
    #region MyRegion "Pendency Order Report By Deepa 11/8/2018"
    [NotMapped]
    public class PendencyOrderReport
    {
        public long id { get; set; }
        public DateTime? DateOfOrder { get; set; }
        public DateTime? OrderPlacedDate { get; set; }
        public string OrderType { get; set; }
        public string NewBuild { get; set; }
        public string EnterpriseCode { get; set; }
        public string EnterpriseName { get; set; }
        public string KitchenId { get; set; }
        public string StoreName { get; set; }
        public string DCCode { get; set; }
        public string PONumber { get; set; }
        public string Unique_Reference_Id { get; set; }
        public string ItemName { get; set; }
        public string EXCode { get; set; }
        public string SKUName { get; set; }
        public string PackSize { get; set; }
        public string OrderQuantity { get; set; }
        public string InvoiceQuantity { get; set; }
        public string Status { get; set; }
        public string LogisticPartner { get; set; }
        public string BuildType { get; set; }
        public int PendingDay { get; set; }
        public string Slab { get; set; }
        public string ItemType { get; set; }
        public string ItemSubType { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string Priority { get; set; }
        public int OrderMonth { get; set; }

    }
    public class PendencyOrderReportExportToExcel
    {
        public string PONumber { get; set; }
        public string Unique_Reference_Id { get; set; }
        public string DateOfOrder { get; set; }
        public string EnterpriseCode { get; set; }
        public string EnterpriseName { get; set; }
        public string KitchenId { get; set; }
        public string StoreName { get; set; }
        public string CityName { get; set; }
        public string EXCode { get; set; }
        public string SKUName { get; set; }
        public decimal? OrderQuantity { get; set; }
        public string DCCode { get; set; }
        public string Status { get; set; }
        public int PendingDay { get; set; }
        public string Slab { get; set; }
        public string InvoiceQuantity { get; set; }
    }
    #endregion
    #region "DeliveryStatus Report By Deepa 11/11/2019"
    [NotMapped]
    public class DeliveryStatusReport
    {
        public string UniqueReferenceId { get; set; }
        public string PONumber { get; set; }
        public string InvoiceDate { get; set; }
        public string DeliveryDate { get; set; }
        public string DCName { get; set; }
        public string CityCode { get; set; }
        public string VehicleNo { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceQuantity { get; set; }
        public string DeliveryStatus { get; set; }
        public string Reason { get; set; }
        public string City { get; set; }
        public DateTime? Store_Order_Date { get; set; }
    }
    #endregion

    #region "Ageing Report by Deepa 13/11/2019"
    public class AgeingReport
    {
        public string EnterpriseCode { get; set; }
        public string EnterpriseName { get; set; }
        public string Store_Code { get; set; }
        public string StoreName { get; set; }
        public int? A0_5 { get; set; }        
        public int? A6_10 { get; set; }
        public int? A11_15 { get; set; }
        public int? Above_16 { get; set; }

        public int? A0_3 { get; set; }
        public int? A3_5 { get; set; }
        public int? A5_7 { get; set; }
        public int? A7_10 { get; set; }
        public int? Above_10 { get; set; }

        public string OrderHeaderIds { get; set; }
    }

    public class AgeingDetailReport
    {
        public string Unique_Reference_Id { get; set; }
        public string Item_Code { get; set; }
        public string Item_Desc { get; set; }
        public DateTime? Store_Order_Date { get; set; }
        public string Store_Code { get; set; }
        public int BookedQuantity { get; set; }
        public long orderheaderid { get; set; }
        public int PendingDay { get; set; }
        public string Dalivered_Date1 { get; set; }
        public string Invoice_Date { get; set; }
        public long OrderDetailId { get; set; }
        public string DcCode { get; set; }
    }
    #endregion

    #region "OnTime Report 20/02/2020"
    public class OnTimeReport
    {
        public string CityName { get; set; }
        public string DCName { get; set; }
        public int CityId { get; set; }
        public int DcId { get; set; }
        public decimal OnTimePercentage { get; set; }
        public decimal NotOnTimePercentage { get; set; }
        public int TotalRecords { get; set; }
        public int DeliveryCount { get; set; }
        public int OntimeDeliveryCount { get; set; }
        public int IsOnTimeDelivery { get; set; }
        public long ExcelID { get; set; }
    }

    public class OnTimeDetailReport
    {

        public string Unique_Reference_Id { get; set; }
        public string CityName { get; set; }
        public string DCName { get; set; }
        public string StoreName { get; set; }
        public string ScheduleTime { get; set; }
        public string ScheduledWeekDay { get; set; }
        public string PODTime { get; set; }
        public string PODDateTime { get; set; }
        public int IsOnTimeDelivery { get; set; }

    }
    #endregion


    public class Report2
    {

        public string Correct_Fulfilment { get; set; }
        public string Partial_Fulfilment { get; set; }
        public string Pending { get; set; }

        public string Item_Sub_Type { get; set; }

        public string city { get; set; }
        public decimal oyo_order_qty { get; set; }
        public decimal rfpl_order_qty { get; set; }
        public decimal partial_order_qty { get; set; }
        public decimal pending_order_qty { get; set; }

        public long OYO_Orders { get; set; }
        public long rfpl_correct_fullfilled_orders { get; set; }
        public long partial_orders { get; set; }
        public long pending_orders { get; set; }


    }

    #region "DOR by Deepa 18/11/2019"
    public class DailyOrderReport
    {
        public string Unique_Reference_Id { get; set; }
        public string PONumber { get; set; }
        public DateTime? Store_Order_Date { get; set; }
        public DateTime Upload_Date { get; set; }
        public string EnterpriseName { get; set; }
        public string EnterpriseCode { get; set; }
        public string Store_Code { get; set; }
        public string StoreName { get; set; }
        public string Item_Code { get; set; }
        public string item_desc { get; set; }
        public string ItemCategoryType { get; set; }
        public string Item_Type { get; set; }
        public string Brand { get; set; }
        public string UnitofMasureDescription { get; set; }
        public decimal? Case_Size { get; set; }
        public decimal? Ordered_Qty { get; set; }
        public decimal? Revised_Ordered_Qty { get; set; }
        public decimal? RFPL_Ordered_Qty { get; set; }
        public decimal? Original_Ordered_Qty { get; set; }
        public string Ordered_By { get; set; }
        public string DCName { get; set; }
        public string City { get; set; }
        public string PlaceOfSupply { get; set; }
        public string Item_Sub_Type { get; set; }
        public string Status { get; set; }

    }
    #endregion

    #region ClosingStock
    public class ClosingStock
    {
        public string ORG_ID { get; set; }
        public string StoreName { get; set; }
        public string ITEM_NUMBER { get; set; }
        public string SKUName { get; set; }
        public string Category { get; set; }
        public string UOM { get; set; }
        public decimal STOCK_QUANTITY { get; set; }
        public decimal ISSUE_QUANTITY { get; set; }
        public DateTime REPORT_DATE { get; set; }
        public int CUST_ID { get; set; }
        public int ITEM_ID { get; set; }
        public int STORE_ID { get; set; }
    }
    #endregion

    public class InvoiceView
    {
        public string Internal_Id { get; set; }
        public string Invoice_Number { get; set; }
        public string DueDate { get; set; }
        public string EnterpriseName { get; set; }
        public string EnterpriseCode { get; set; }        
        public string Project_Name { get; set; }
        public string StoreName { get; set; }
        public string CreatedFrom { get; set; }
        public string InvoiceDate { get; set; }
        public string PostingPeriod { get; set; }
        public string CustomerZone { get; set; }
        public string Memo { get; set; }
        public string Department { get; set; }
        public string Class { get; set; }
        public int DC_Location { get; set; }
        public string Division { get; set; }
        public string EmployeeDepartment { get; set; }
        public string SOApproverPartner { get; set; }
        public string CreateBy { get; set; }

        public string Item { get; set; }
        public string Description { get; set; }
        public string HSNSAC { get; set; }
        public string Location { get; set; }
        public string TaxCode { get; set; }
        public string TaxRate { get; set; }
        public string Amount { get; set; }
        public string TaxAmount { get; set; }
        public string GrossAmount { get; set; }
        public string TaxLiable { get; set; }
        public string GoodsAndServices { get; set; }
        public string ItemCategory { get; set; }
        public string Quantity { get; set; }
    }
    #region 
    public class StoreWiseSkuReport
    {
        public string Unique_Reference_Id { get; set; }
        public string PONumber { get; set; }
        public DateTime? Store_Order_Date { get; set; }
        public DateTime Upload_Date { get; set; }
        public string EnterpriseName { get; set; }
        public string EnterpriseCode { get; set; }
        public string Store_Code { get; set; }
        public string StoreName { get; set; }
        public string Item_Code { get; set; }
        public string item_desc { get; set; }
        public string ItemCategoryType { get; set; }
        public string Item_Type { get; set; }
        public string Brand { get; set; }
        public string UnitofMasureDescription { get; set; }
        public decimal? Case_Size { get; set; }
        public decimal? Ordered_Qty { get; set; }
        public decimal? Revised_Ordered_Qty { get; set; }
        public decimal? RFPL_Ordered_Qty { get; set; }
        public decimal? Original_Ordered_Qty { get; set; }
        public string Ordered_By { get; set; }
        public string DCName { get; set; }
        public string City { get; set; }
        public string PlaceOfSupply { get; set; }
        public string Item_Sub_Type { get; set; }
        public string Status { get; set; }
    }
    #endregion
    public class PFAReport
    {

        public int StoreId { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string Customer { get; set; }
        public string DCName { get; set; }
        public int ItemId { get; set; }
        public string ItemStatus { get; set; }
        public int Id { get; set; }
        public string SKUCode { get; set; }
        public string SKUName { get; set; }
        public string MinimumOrder { get; set; }
        public decimal MaximumOrderLimit { get; set; }
        public decimal CaseConversion { get; set; }
        public string UOM { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string BrandName { get; set; }



    }
}