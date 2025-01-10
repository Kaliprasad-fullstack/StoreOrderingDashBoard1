using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("SoStatusMaster")]
    public class SoStatusMaster
    {
        [Key]
        public int Id { get; set; }
        public int NetsuiteStatusId { get; set; }
        public string StatusName { get; set; }
    }
    [Table("SoPOMappingTbl")]
    public class SoPoMapping
    {
        [Key]
        public int Id { get; set; }
        public string PurchaseOrder { get; set; }
        public string SalesOrder { get; set; }
        public int Status { get; set; }
        public string SoDate { get; set; }
        public string Location { get; set; }
    }
    [Table("SoStatusDetailTbl")]
    public class SoStatusDetail
    {
        [Key]
        public int Id { get; set; }
        public string SoNumber { get; set; }
        public int SO_Status { get; set; }
        public string EventDate { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? TS{get;set;}
    }
}
