using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [NotMapped]
    public class StoreUserExcelImport
    {
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public string StoreEmailId { get; set; }
        public string Region { get; set; }
        public string Route { get; set; }
        public string StoreManager { get; set; }
        public string StoreContactNo { get; set; }
        public string CustomerName { get; set; }
        public string Location { get; set; }
        public string WarehouseDc { get; set; }
        public string UserEmailAddress { get; set; }
        public string PlaceOfSupply { get; set; }
        public string PlanName { get; set; }
        public string MkrChkrFlag { get; set; }
        public List<string> Error { get; set; }
        public long StoreId { get; set; }
        public int? NetSuitMapCls_Id { get; set; }
        //public string NetSuitMapCls { get; set; }
    }
}
