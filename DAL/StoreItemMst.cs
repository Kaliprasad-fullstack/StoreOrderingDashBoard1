using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAL
{
    [Table("StoreItemMst")]
    public class StoreItemMst
    {
        [Key]
        public long Id { get; set; }
        public int StoreId { get; set; }
        public int CustId { get; set; }
        public int ItemId { get; set; }
        public int IsDeleted { get; set; }
        public DateTime? Created { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }
        [NotMapped]
        public int? TopItemFlag { get; set; }
    }

    public class StoreItemView
    {
        [Key]
        public string SKUCode { get; set; }
        public string SKUName { get; set; }
        public int ItemId { get; set; }
        public int DcId { get; set; }
        public string DcName { get; set; }
        public int StoreCount { get; set; }
        public int ActualStoreCount { get; set; }
        public int Status { get; set; }
        public long Id { get; set; }
        public int StoreId { get; set; }
        public int CustId { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }


        public int Active { get; set; }
        public bool UpdatedRecord { get; set; }

    }
}