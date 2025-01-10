using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class NetSuitExcelUpload
    {
        public Int64 Order { get; set; }
        public string ExpectedDeliveryDate { get; set; }
        public string Department { get; set; }
        public string Class { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public string Division { get; set; }
        public string SOApproverPartner { get; set; }
        public string ITEMCODE { get; set; }
        public string ITEMNAME { get; set; }
        public int ORDERQUANTITY { get; set; }
        public string StoreCode { get; set; }
        public string IndentNo { get; set; }
        public int Amount { get; set; }
        public string IndentDate { get; set; }
        public string TRANSACTIONTYPE { get; set; }
        public string PLACEOFSUPPLY { get; set; }

    }
}
