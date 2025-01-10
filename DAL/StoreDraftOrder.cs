using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("StoreDraft")]
    public class StoreDraftOrder
    {
        [Key]
        public Int64 Id { get; set; }
        [ForeignKey("Customer")]
        public int CustId { get; set; }
        public virtual Customer Customer { get; set; }
        public DateTime? Store_Order_Date { get; set; }     
        public string Store_Code { get; set; }
        public string Item_Code { get; set; }
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }
        public string Item_Desc { get; set; }
        public string Unique_Reference_Id { get; set; }
        public string Ordered_Qty { get; set; }
        [ForeignKey("Users")]
        public int? Upload_By { get; set; }
        public virtual Users Users { get; set; }
        public DateTime? Upload_Date { get; set; }
        public int IsDeleted { get; set; }
        [ForeignKey("Modified_ByUser")]
        public int? Modified_By { get; set; }
        public virtual Users Modified_ByUser { get; set; }
        public DateTime? Modified_Date { get; set; }
    }

    public class StoreOrderVB
    {
        public Int64 Id { get; set; }
        public int CustId { get; set; }
        public DateTime? Store_Order_Date { get; set; }
        public string Store_Code { get; set; }
        public string Item_Code { get; set; }
        public string CaseConversion { get; set; }
        public string UnitofMasureDescription { get; set; }
        public decimal MaximumOrderLimit { get; set; }
        public int ItemId { get; set; }
        public string Item_Desc { get; set; }
        public string Unique_Reference_Id { get; set; }
        public string Ordered_Qty { get; set; }
        public int? Upload_By { get; set; }
        public DateTime? Upload_Date { get; set; }
        public int IsDeleted { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public decimal MinimumOrderQuantity { get; set; }
        public decimal CaseSize { get; set; }
        public string PONumber { get; set; }
    }
}