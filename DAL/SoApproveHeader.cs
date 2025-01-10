using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("SoApproveHeadertbl")]
    public class SoApproveHeader
    {
        [Key]
        public long Id { get; set; }
        public string SO_Number { get; set; }
        public int SO_Status { get; set; }
        public string PO_Date { get; set; }
        public string Department { get; set; }
        public string Class { get; set; }
        public string Location { get; set; }
        public string SOApproverPartner { get; set; }
        public string Store_Name { get; set; }
        public string PO { get; set; }
        public string Placeofsupply { get; set; }
        public string Transactiontype { get; set; }
        public string CustomForm { get; set; }
        public string EmployeeDept { get; set; }
        public string Memo { get; set; }
        public string msg { get; set; }
        public bool IsSOApproved { get; set; }
        public string Errormsg { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ForcedLock { get; set; }
        public DateTime? ForcedLock_Date { get; set; }

        public virtual ICollection<SoApproveDetail> SoApproveDetails { get; set; }
        [ForeignKey("OrderHeaders")]
        public Int64? OrderHeaderId { get; set; }
        public virtual OrderHeader OrderHeaders { get; set; }

    }

    public class SoDetails
    {
        public int Id { get; set; }
        public string DC_NAME { get; set; }
        public string STORE_CODE { get; set; }
        public string ORDER_NO { get; set; }
        public string PO_DATE { get; set; }
        public string STATUS { get; set; }
        public string SO_DATE { get; set; }
        public string SO_NUMBER { get; set; }
        public string ITEM { get; set; }
        public string ITEM_CATEGORY { get; set; }
        public string MAXIMUM_OF_SO_QUANTITY { get; set; }
        public string MAXIMUM_OF_QUANTITY_COMMITTED { get; set; }
        public string FULFILLMENT_DATE { get; set; }
        public string FULFILLMENT_NUMBER { get; set; }
        public string MAXIMUM_OF_QUANTITY_FULFILLED { get; set; }
        public string INVOICE_DATE { get; set; }
        public string INVOICE_NUMBER { get; set; }
        public string MAXIMUM_OF_QUANTITY_INVOICED { get; set; }
        public string MAXIMUM_OF_BALANCE_QUANTITY { get; set; }
        public string MAXIMUM_OF_ITEM_RATE { get; set; }
        public string TAX_ITEM { get; set; }
        public string MAXIMUM_OF_AMOUNT_TAX { get; set; }
        public string MAXIMUM_OF_AMOUNT { get; set; }
        public string MAXIMUM_OF_TOTAL_AMOUNT { get; set; }
        public string COMMIT { get; set; }
        public string CREATED_BY { get; set; }
    }
}