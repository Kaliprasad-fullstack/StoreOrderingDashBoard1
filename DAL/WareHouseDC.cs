using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("WareHouseDCMst")]
    public class WareHouseDC
    {
        public WareHouseDC()
        {
        }
        public WareHouseDC(Int32 Id, string name, int? LocationId, int CompanyId, ICollection<PermitableCls> PermitableCls = null, ICollection<Store> Stores = null)
        {
            this.Id = Id;
            this.Name = name;
            this.LocationmstId = LocationId;
            this.CompanyId = CompanyId;
            this.PermitableCls = PermitableCls;
            this.Stores = Stores;
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public string DCShortName { get; set; }
        [ForeignKey("Locations")]
        public int? LocationId { get; set; }
        public virtual Locations Locations { get; set; }
        public virtual Company Company { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<PermitableCls> PermitableCls { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
        [NotMapped]
        public int? LocationmstId { get; set; }
        [NotMapped]
        public int CompanyId { get; private set; }
        public string DCCode{ get; private set; }
    }
    public class WareHouseDCView
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class WareHouseDCView2
    {
        public int StoreId { get; set; }
        public int DCId { get; set; }
        public string DcName { get; set; }
        public int Category_Id { get; set; }
        public int ItemTypeId { get; set; }
        public int Selected { get; set; }
    }
    //[Table("TicketTypeMST")]
    //public class TicketType2
    //{
    //    [Key]
    //    public int TicketTypeID { get; set; }
    //    public string TicketType { get; set; }
    //    [NotMapped]
    //    public virtual ICollection<Store> Stores { get; set; }
    //}
}
