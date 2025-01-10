using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    [Table("ExcelData")]
    public class Exceldata
    {
        public string Store_Order_Date { get; set; }
        public string Unique_Reference_Id { get; set; }
        public string Source_Dc { get; set; }
        public string OrderQuantity { get; set; }
        
    }
}

