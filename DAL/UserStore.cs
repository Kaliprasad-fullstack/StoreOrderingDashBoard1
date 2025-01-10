using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("UserStore")]
    public class UserStore
    {
        [Key]
        public long Id { get; set; }
        public int UserId { get; set; }
        public int StoreId { get; set; }
        public string MkrChkrFlag { get; set; }
    }
}
