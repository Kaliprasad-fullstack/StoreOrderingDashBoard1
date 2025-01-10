using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL
{
    #region "AgeingReportChart by Deepa 18/12/2019"
    public class AgeingReportChart
    {
        public string DefaultCoulmn { get; set; }
        public string Store_Code { get; set; }
        public string StoreName { get; set; }
        public string StatusName { get; set; }
        public string DCName { get; set; }
        public string SubCategoryName { get; set; }
        public string City { get; set; }
        public int A0_5 { get; set; }
        public int A6_10 { get; set; }
        public int A11_15 { get; set; }
        public int Above_16 { get; set; }
        public int CustomRange { get; set; }
        public int PendingDays { get; set; }
        public int Quantity { get; set; }
        public DateTime Store_Order_Date { get; set; }
        public string SKUCode { get; set; }
    }
    public class AgeingAgegroup
    {
        public int A0_5 { get; set; }
        public int A6_10 { get; set; }
        public int A11_15 { get; set; }
        public int Above_16 { get; set; }
    }
    public class NameValuePair
    {
        public string name { get; set; }
        public int value { get; set; }
    }
    #endregion

    #region "FilRate Report Charts 07/1/2020"
    public class FillRateReportChart
    {
        public string Item_Sub_Type { get; set; }
        public string city { get; set; }
        public string DCName { get; set; }
        public string store_code { get; set; }
        public string StoreName { get; set; }
        public decimal Correct_Fulfilment { get; set; }
        public decimal Partial_Fulfilment { get; set; }
        public decimal Pending { get; set; }
        public decimal FinalFulfilment { get; set; }

        public long OYOPlacedOrders { get; set; }
        public long FulfilledOrders { get; set; }
        public long PartialOrders { get; set; }
        public long pending_orders { get; set; }

        public long OrderedQty { get; set; }
        public long DeliveredQty { get; set; }
        public long PartialDeliveredQty { get; set; }
        public long PendingOrderedQty { get; set; }

        public string ReportSubType { get; set; }

    }
    #endregion
    #region "DeliveryStatus Report By Deepa 23/12/2019"
    [NotMapped]
    public class DeliveryStatusReportChart
    {
        public string UniqueReferenceId { get; set; }
        public string InvoiceDate { get; set; }
        public string DeliveryDate { get; set; }
        public string DCName { get; set; }
        public string CityCode { get; set; }
        public string VehicleNo { get; set; }
        public string StoreCode { get; set; }
        public string InvoiceNumber { get; set; }
        public string DeliveryStatus { get; set; }
        public string Reason { get; set; }
        public string City { get; set; }
        public DateTime? Store_Order_Date { get; set; }
        public string Status { get; set; }
        public string SubCategoryName { get; set; }
    }
    #endregion  
}
