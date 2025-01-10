using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("ReturnAuthorizations")]
    public class ReturnAuthorizations
    {
        [Key]
        public int ID { get; set; }
        public DateTime INVOICE_DATE { get; set; }
        public string INVOICE_NUMBER { get; set; }
        public DateTime RA_DATE { get; set; }
        public string RA_NUMBER { get; set; }
        public string STATUS { get; set; }
        public string STORE_CODE { get; set; }
        public string STORE_NAME { get; set; }
        public string ITEM { get; set; }
        public float INVOICE_QUANTITY { get; set; }
        public float RA_QUANTITY { get; set; }
        public string CREDIT_QUANTITY_IR { get; set; }
        public string UOM { get; set; }
        public string LOCATION { get; set; }
        public string REASON { get; set; }
        public string SALES_RETURN_DATE { get; set; }
        public string CREDIT_MEMO_REASON { get; set; }
        public string CREDIT_MEMO_CATEGORY { get; set; }
        public string CLASS { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}