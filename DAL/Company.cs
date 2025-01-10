using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("CompanyMst")]
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Created { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public int ModifiedBy { get; set; }
        public bool isDeleted { get; set; }
        public ICollection<Customer> customers { get; set; }
        public ICollection<Users> users { get; set; }
        public ICollection<WareHouseDC> wareHouseDCs { get; set; }
    }
}
