using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("FullFillmentDetailTbl")]
    public class FullFillmentDetail
    {
        [Key]
        public long Id { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string QuantityOnHand { get; set; }
        public string Quantity { get; set; }
        public virtual FullfillmentHeader FullfillmentHeader { get; set; }
    }
}
