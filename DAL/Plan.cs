using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("PlanTbl")]
    public class Plan
    {
        public Plan()
        {

        }
        public Plan(int Id,int CustomerPlanID,int MenuItemID,int StoreID,int UserID,int CustomerID,bool IsShow)
        {
            this.Id = Id;
            this.CustomerPlanID = CustomerPlanID;
            this.StoreID = StoreID;
            this.MenuItemID = MenuItemID;
            this.UserID = UserID;
            this.CustomerID = CustomerID;
            this.IsShow = IsShow;
        }
        public int Id { get; set; }
        public virtual CustomerPlan CustomerPlan { get; set; }
        public virtual MenuItem MenuItem { get; set; }
        public virtual Store Store { get; set; }
        public virtual Users Users { get; set; }
        public virtual Customer Customer { get; set; }
        public bool IsShow { get; set; }
        [NotMapped]
        public int CustomerPlanID { get;  set; }
        [NotMapped]
        public int StoreID { get;  set; }
        [NotMapped]
        public int MenuItemID { get;  set; }
        [NotMapped]
        public int UserID { get;  set; }
        [NotMapped]
        public int CustomerID { get;  set; }
    }
}
