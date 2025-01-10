using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("InvoiceDetailTbl")]
    public class InvoiceDetail
    {
        [Key]
        public long Id { get; set; }
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
        public decimal? Original_Invoice_Quantity { get; set; }
        public virtual InvoiceHeader InvoiceHeader { get; set; }
    }
}
