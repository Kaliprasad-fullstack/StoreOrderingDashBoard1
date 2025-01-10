using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class OFRLIFR
    {
        public string dc_name { get; set; }
        public int? store_count { get; set; }
        public int? order_qty { get; set; }
        public int? order_placed_Quantity { get; set; }
        public int? order_delivered_Quantity { get; set; }
        public decimal? OFRPercentage { get; set; }
        public int? order_filled_lines { get; set; }
        public int? order_not_fill_lines { get; set; }
        public int? total_lines { get; set; }
        public decimal? LIFRPercentage { get; set; }
        public long Id { get; set; }
        public string order_no { get; set; }
        public DateTime? OrderDate { get; set; }
    }
    public class StoreOrders
    {
        public DateTime? Store_Order_Date { get; set; }
        public string Po_Number { get; set; }
        public int? Store_ordered_number_of_line_items { get; set; }
        public decimal? Total_Store_ordered_qty { get; set; }
        public decimal? Total_Original_ordered_qty { get; set; }
        public decimal? Case_Ordered_qty { get; set; }
        public decimal? Rfpl_Ordered_Qty { get; set; }
        public int? Rfpl_Processed_number_of_line_items { get; set; }
        public int? Rfpl_Invoiced_number_of_line_items { get; set; }
        public decimal? Invoiced_Qty { get; set; }
        public string TakenDays { get; set; }
        public string Order_status { get; set; }
        public long? GroupId { get; set; }
    }
    public class StoreOrderDetailsVB
    {
        public string Unique_Reference_Id { get; set; }
        public string Po_Number { get; set; }
        public string Store_Code { get; set; }
        public DateTime? Store_Order_Date { get; set; }
        public string Item_Code { get; set; }
        public string Item_Desc { get; set; }
        public string UnitofMasureDescription { get; set; }
        public decimal? Case_Size { get; set; }
        public decimal? Revised_Ordered_Qty { get; set; }//OrderQty
        public decimal? Original_Ordered_Qty { get; set; }//Original_Ordered_qty
        public decimal? Rfpl_Ordered_Qty { get; set; }
        public int? Rfpl_Invoiced_number_of_line_items { get; set; }
        public int? Rfpl_Invoiced_Qty { get; set; }
        public string Order_Status { get; set; }
        public decimal? Inv_Quantity { get; set; }
        public string dc_name { get; set; }
        public string TakenDays { get; set; }

    }

    public class DAYWISEORDERQTY
    {
        public string day_name { get; set; }
        public int Rfpl_Ordered_Qty { get; set; }
    }
    public class MonthWiseStoreOrderQty
    {
        public DateTime? calendar_date { get; set; }
        public string w_day_name { get; set; }
        public DateTime? Store_Order_Date { get; set; }
        public decimal? Rfpl_Ordered_Qty { get; set; }
        public decimal? Rfpl_FulFilledQty { get; set; }
        public decimal? InvoiceQty { get; set; }         
    }
    public class TOPSKUForYEARMonth
    {
        public string Item_Desc { get; set; }
        public string Year_Month { get; set; }
        public int sku_ordered_times { get; set; }
    }
    public class MonthWiseTotalSalesValue
    {
        public int Month { get; set; }
        public string MonthName { get; set; }
        public int Year { get; set; }
        public decimal TotalSalesValue { get; set; }
        public decimal Amount { get; set; }
    }
    public class TopSKUForCustomer
    {
        public string SKUCode { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalSalesValue { get; set; }
    }
    public class MonthWiseFillRate
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }        
        public decimal TotalSalesValue { get; set; }
    }
}
