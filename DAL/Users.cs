using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Users
    {
        public Users()
        {

        }
        public Users(int UserId, string EmailAddress, string OTP)
        {
            this.UserId = UserId;
            this.EmailAddress = EmailAddress;
            this.OTP = OTP;
        }
        public Users(int id,int UserId,string Name,string Department,string EmailAddress,string Password,int RoleId)
        {
            this.Id = id;
            this.UserId = UserId;
            this.Name = Name;
            this.Department = Department;
            this.EmailAddress = EmailAddress;
            this.Password = Password;
            this.RoleId = RoleId;
        }
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        public int ReportToId { get; set; }
        [ForeignKey("Locations")]
        public int LocationId { get; set; }
        public int RoleId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? LogOutTime { get; set; }
        public bool IsLogin { get; set; }
        public virtual Locations Locations { get; set; }
        public virtual WareHouseDC WareHouseDC { get; set; }
        public virtual CustomerPlan CustomerPlan { get; set; }
        public virtual Company Company { get; set; }
        public ICollection<PermitableCls> PermitableCls { get; set; }
        public virtual ICollection<Plan> Plans { get; set; }
        public virtual ICollection<DashBoardMapping> DashBoardMappings { get; set; }

        //  public virtual WareHouseDC WareHouseDC { get; set; }
        [NotMapped]
        public List<AllQuestion> AllQuestions { get; set; }
        [NotMapped]
        public List<Customer> Customers { get; set; }
        [NotMapped]
        public string Stored { get; set; }
        [NotMapped]
        public int[] Customer { get; set; }
        [NotMapped]
        public int CustomerPlanId { get; set; }
        [NotMapped]
        public int PlanId { get; set; }
        [NotMapped]
        public int StoreId { get; set; }
        [NotMapped]
        public string StoreName { get; set; }
        [NotMapped]
        public string ChrMkrFlag { get; set; }
        [NotMapped]
        public string NewPassword { get; set; }
        [NotMapped]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
        [NotMapped]
        public string OTP { get; set; }

        //public virtual ICollection<EscalationAction> Escalations { get; set; }
    }
    [NotMapped]
    public class StoreUser
    {
        public string MkrChkrFlag { get; set; }
        public int StoreId { get; set; }
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        public int ReportToId { get; set; }
        public int LocationId { get; set; }
        public int RoleId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }
        public int CustomerPlanId { get; set; }
        public int CustId { get; set; }
        public string StoreName { get; set; }
        public string StoreCode { get; set; }
        public StoreUser(string MkrChkrFlag, int StoreId, int Id, string Name, string Department, string EmailAddress, string Password, bool IsDeleted, int ReportToId, int LocationId, int RoleId, DateTime? CreatedOn, int? CreatedBy, DateTime? Modified, int? ModifiedBy, int PlanId)
        {
            this.Id = Id;
            this.Name = Name;
            this.Department = Department;
            this.EmailAddress = EmailAddress;
            this.Password = Password;
            this.IsDeleted = IsDeleted;
            this.ReportToId = ReportToId;
            this.LocationId = LocationId;
            this.RoleId = RoleId;
            this.CreatedOn = CreatedOn;
            this.CreatedBy = CreatedBy;
            this.Modified = Modified;
            this.ModifiedBy = ModifiedBy;
            this.CustomerPlanId = PlanId;
            this.MkrChkrFlag = MkrChkrFlag;
            this.StoreId = StoreId;
        }
        public StoreUser()
        {

        }
    }    
}
