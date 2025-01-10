using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [NotMapped]
    public class DetailReport
    {
        public long Id { get; set; }
        public string SO_Number { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string StoreName { get; set; }
        public string StoreCode { get; set; }
        public string SKUName { get; set; }
        public int DCID { get; set; }
        public string SKUCode { get; set; }
        public int Quantity { get; set; }
        public int QuantityOnHand { get; set; }
        public string Invoice_Number { get; set; }
        public string CustomerZone { get; set; }
        public string CaseConversion { get; set; }
        public string UOMMaster { get; set; }
        public decimal MaximumOrderLimit { get; set; }
        public string LocationName { get; set; }
        public string ApproverName { get; set; }
        public string SOApproverPartner { get; set; }
        public string DcName { get; set; }

    }
}
