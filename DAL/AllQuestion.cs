using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [NotMapped]
    public class AllQuestion
    {
        public int Id { get; set; }
        public string ques { get; set; }
        public string answer { get; set; }
        public int UserId { get; set; }
    }
}
