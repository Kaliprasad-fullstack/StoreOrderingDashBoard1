using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("RegionMst")]
    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set;}
        public DateTime? CreatedOn { get; set; }
        //public DateTime? Created { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Locations> Locations { get; set; }
    }
}
