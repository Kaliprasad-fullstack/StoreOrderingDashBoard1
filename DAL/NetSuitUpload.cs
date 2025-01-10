using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [NotMapped]
    public class NetSuitUpload
    {
        public Int64 Order { get; set; }
        //public DateTime PO_Date { get; set; }
        public string PO_Date { get; set; }
        public string Department { get; set; }
        public string Class { get; set; }
        public string Location { get; set; }
        public string Division { get; set; }
        public string SOApproverPartner { get; set; }
        public string code { get; set; }
        public string desc { get; set; }
        public int qty { get; set; }
        public string Store_Name { get; set; }
        public string PO { get; set; }
        public string Placeofsupply { get; set; }
        public string Transactiontype { get; set; }
        public int CustomForm { get; set; }
        public string EmployeeDept { get; set; }
        public string PONumber { get; set; }
    }
}
