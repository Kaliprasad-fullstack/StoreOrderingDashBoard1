using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [NotMapped]
    public class DropDown
    {
        public int Id { get; set; }
        public string SKUName { get; set; }
        public string SKUCode { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
    }
}
