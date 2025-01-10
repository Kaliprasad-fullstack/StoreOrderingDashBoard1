using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        public OrderDetail()
        { }
        public OrderDetail(Int64 Id, decimal Quantity ,int ItemId ,  Int64 OrderHeaderId,string Remark )
        {
            this.Id = Id;
            this.Quantity = Quantity;
            this.ItemId = ItemId;
            this.OrderHeaderId=OrderHeaderId;
            this.Remark = Remark;
        }
        [Key]
        public Int64 Id { get; set; }
        public decimal Quantity { get; set; }
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }
        [ForeignKey("OrderHeaders")]
        public Int64 OrderHeaderId { get; set; }
        public virtual OrderHeader OrderHeaders { get; set; }
        public string uniqueReferenceID { get; set; }
        public string Remark { get; set; }
        [NotMapped]
        public int StoreId { get; set; }        
        [ForeignKey("WareHouseDC")]
        public int? DCID { get; set; }
        public virtual WareHouseDC WareHouseDC { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("OrderExcel")]
        public long? OrderExcel_Id { get; set; }
        public virtual OrderExcel OrderExcel { get; set; }

        public string So_Number { get; set; }
        public decimal? So_Quantity { get; set; }
        public string Invoice_Number { get; set; }
        public decimal? Invoice_Quantity { get; set; }
        public string Delivered_Status1 { get; set; }
        public string Dalivered_Date1 { get; set; }
        public string Delivered_Status2 { get; set; }
        public string Dalivered_Date2 { get; set; }
        public decimal? Dalivered_Qty { get; set; }
        public string Return_Reason { get; set; }
        public string Slab { get; set; }
        public int Job_flag { get; set; }

        //public int DaysTaken { get; set; }

        public string Invoice_Date { get; set; }

        public string So_Status { get; set; }

        public int? DaysTaken { get; set; }
        public int? OrderClosed { get; set; }
        public int? OrderDeleted { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public decimal? Original_Ordered_Quantity { get; set; }
    }
}
