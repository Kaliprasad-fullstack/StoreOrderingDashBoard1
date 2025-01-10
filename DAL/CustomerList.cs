using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [NotMapped]
    public class CustomerList
    {
        public int Id { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set;}
        public string Name { get; set; }
        public string Question { get; set; }
        public int QuestionId { get; set; }
    }
}
