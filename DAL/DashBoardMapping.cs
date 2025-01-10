using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("DashBoardMappingTbl")]
    public class DashBoardMapping
    {
        public DashBoardMapping()
        {

        }
        public DashBoardMapping(int Id, int? CustomerPlanID, int? StoreID, int? UserID, int? DashBoardPerID, bool IsShow)
        {
            this.Id = Id;
            this.CustomerPlanID = CustomerPlanID;
            this.StoreID = StoreID;
            this.UserID = UserID;
            this.DashBoardPerID = DashBoardPerID;
            this.IsShow = IsShow;
        }
        [Key]
        public int Id { get; set; }
        public virtual CustomerPlan CustomerPlan { get; set; }
        public virtual DashBoardPer DashBoardPer { get; set; }
        public virtual Store Store { get; set; }
        public virtual Users Users { get; set; }
        public bool IsShow { get; set; }

        [NotMapped]
        public int? CustomerPlanID { get; private set; }
        [NotMapped]
        public int? StoreID { get; private set; }
        //[NotMapped]
        //public int MenuItemID { get; private set; }
        [NotMapped]
        public int? UserID { get; private set; }
        [NotMapped]
        public int? DashBoardPerID { get; private set; }
    }
}
