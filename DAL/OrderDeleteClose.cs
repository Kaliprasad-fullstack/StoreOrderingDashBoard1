using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{

    [NotMapped]
    public class OrderDeleteNew
    {
        public Int64 Id { get; set; }
       
        public DateTime? DateOfOrder { get; set; }
        public string StoreCode { get; set; }
        public string DCCode { get; set; }
        public string DcName { get; set; }
        public string RFPLID { get; set; }
       
  
       
       
        public int custid { get; set; }
       
    }

    [NotMapped]
    public class OrderDeleteClose
    {
            public DateTime? DateOfOrder { get; set; }
            public string StoreCode { get; set; }
            public string DCCode { get; set; }
            public string Unique_Reference_Id { get; set; }
        public string RFPLID { get; set; }
        public string ItemName { get; set; }
            public string OrderQuantity { get; set; }
            public string StatusName { get; set; }
            public long Id { get; set; }
        public int custid { get; set; }
        public int orderno { get; set; }
    }
    [NotMapped]
    public class OrderDeleteItems
    {
        public int ProductId { get; set; }
        public string Reason { get; set; }
        public string CustomReason { get; set; }
    }

    [NotMapped]
    public class OrderDeleteFinlized
    {
        public int OrderId { get; set; }
        public string Reason { get; set; }
        public string CustomReason { get; set; }
    }
    [NotMapped]
    public class OrderDeleteCloseReport
    {
        public string Unique_Reference_Id { get; set; }
        public DateTime DateOfOrder { get; set; }
        public string DateOfOrderStr { get; set; }
        public string StoreCode { get; set; }       
        public string SKUCode { get; set; }
        public string OrderQuantity { get; set; }
        public string Reason { get; set; }
    }
    [NotMapped]
    public class OrderEdit
    {
        public DateTime? DateOfOrder { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string DCCode { get; set; }
        public string Unique_Reference_Id { get; set; }
        public string RFPLID { get; set; }
        public string ItemName { get; set; }
        public string OrderQuantity { get; set; }
        public string StatusName { get; set; }
        public long Id { get; set; }
        public int custid { get; set; }
        public long orderno { get; set; }
        public long orders { get; set; }

    }
    [NotMapped]
    public class GenerateOrder
    {
        public int UserId { get; set; }
        public DateTime FromDate { get; set; }
        public int CustId { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string Item_Code { get; set; }
        public string Item_Desc { get; set; }
        public string UOM { get; set; }
        public string Threshold_Qty { get; set; }
        public string Ordered_Qty { get; set; }
        public string Priority { get; set; }
        public string Checked { get; set; }
        public string DCID { get; set; }
        public string ItemTypeId { get; set; }
        public DateTime ToDate { get; set; }
        public string Uplaoded_By { get; set; }
        public string ItemId { get; set; }
        public string Case_Size { get; set; }
        public string UnitofMasureDescription { get; set; }
        public string MinimumOrderQuantity { get; set; }
        public string PONumber { get; set; }
        public string Unique_Reference_Id { get; set; }

    
}
}