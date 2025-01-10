using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("Store_ItemType_DC_Master")]
    public class Store_ItemType_DC_Master
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Store")]
        public int StoreId { get; set; }
        public virtual Store Store { get; set; }

        [ForeignKey("WareHouseDC")]
        public int? DCID { get; set; }
        public virtual WareHouseDC WareHouseDC { get; set; }

        [ForeignKey("ItemCategoryType")]
        public int? ItemTypeId { get; set; }
        public virtual CategoryType ItemCategoryType { get; set; }

        public DateTime? Created { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
