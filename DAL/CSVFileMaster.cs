using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table(name: "CSVFileMaster")]
    public class CSVFileMaster
    {
        [Key]
        public Int64 Id { get; set; }

        [ForeignKey("Customer")]
        public int CustId { get; set; }
        public virtual Customer Customer { get; set; }

        public string Filepath { get; set; }
        public string FileName { get; set; }

        [ForeignKey("Users")]
        public int? Created_By { get; set; }
        public virtual Users Users { get; set; }
        public DateTime? Created_Date { get; set; }

        public int Isdeleted { get; set; }
    }

    public class CSVFileMasterVM
    {

        public Int64 Id { get; set; }
        public string Filepath { get; set; }
        public string FileName { get; set; }
        public string Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public int Isdeleted { get; set; }
    }
}
