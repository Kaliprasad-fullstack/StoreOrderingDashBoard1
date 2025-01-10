using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("LocationMst")]
    public class Locations
    {
        public Locations()
        {

        }
        public Locations(int LocationId, string Name, string Pincode, Region Region, bool IsDeleted, ICollection<Users> Users, ICollection<WareHouseDC> warehouseDc, ICollection<Store> store)
        {
            this.LocationId = LocationId;
            this.Name = Name;
            this.Region = new Region();
            this.Region.Id = Region.Id;
            this.IsDeleted = IsDeleted;
            if (Users != null)
            {
                this.Users = new List<Users>();
                foreach (Users user in Users)
                {
                    this.Users.Add(new Users() { UserId = user.UserId, Name = user.Name });
                }
            }
            if (warehouseDc != null)
            {
                this.WareHouseDCs = new List<WareHouseDC>();
                foreach (WareHouseDC dc in warehouseDc)
                {
                    this.WareHouseDCs.Add(new WareHouseDC() { Id = dc.Id, Name = dc.Name });
                }
            }
            if (store != null)
            {
                this.Stores = new List<Store>();
                foreach (Store locstore in store)
                {
                    this.Stores.Add(new Store() { Id = locstore.Id, StoreName = locstore.StoreName });
                }
            }
        }
        [Key]
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string Pincode { get; set; }
        //[ForeignKey("Region")]   
        //public int RegionId { get; set; }
        public virtual Region Region { get; set; }
        public DateTime? Created { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string LocationCode { get; set; }

        public virtual ICollection<Users> Users { get; set; }
        public virtual ICollection<WareHouseDC> WareHouseDCs { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
    }
    public class LocationView
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
