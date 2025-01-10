using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("CategoryWiseReasonMaster")]
    public class CategoryWiseReasonMaster
    {
        [Key]
        public int Id { get; set; }
        [StringLength(10)]
        public string categoryName { get; set; }
        public string Reason { get; set; }
      
    }
}
