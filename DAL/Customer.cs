using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DAL
{
    [Table("CustomerMst")]
    public class Customer
    {
        public Customer()
        {

        }
        public Customer(int Id, string Name, bool IsDeleted, DateTime? ModifiedDate, DateTime? Created, int? CreatedBy, int? ModifiedBy, int CompanyId, int CustomerPlanId)
        {
            this.Id = Id;
            this.Name = Name;
            this.IsDeleted = IsDeleted;
            this.ModifiedDate = ModifiedDate;
            this.Created = Created;
            this.CreatedBy = CreatedBy;
            this.ModifiedBy = ModifiedBy;
            this.CompanyId = CompanyId;
            this.CustomerPlanId = CustomerPlanId;
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? Created { get; set; }
        public int? CreatedBy { get; set; }
        //public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }
        public int CustomerFormId { get; set; }
        public string EmployeeDept { get; set; }
        public virtual Company Company { get; set; }
        public virtual CustomerPlan CustomerPlan { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<PermitableCls> PermitableCls { get; set; }
        public virtual ICollection<Plan> plans { get; set; }
        [NotMapped]
        public int CustomerPlanId { get; set; }
        [NotMapped]
        public int CompanyId { get; set; }
        public bool PlannedOrderFlag { get; set; }
        public bool AutoInventoryLogicFlag { get; set; }
        public virtual ICollection<EscalationLevelMaster> EscalationLevelMaster { get; set; }

        public bool HasEscalationMatrix { get; set; }

        [ForeignKey("Customers")]
        public int? ParentId { get; set; }
        public virtual Customer Customers { get; set; }
        public bool ItemTypeDCFlag { get; set; }

        //public int? OrderConfirmationMail { get; set; }        
        [NotMapped]
        public List<int> EmailTypes { get; set; }

        [NotMapped]
        public string StatusRemark { get; set; }
        [NotMapped]
        public int OrderDays { get; set; }
        //public virtual ICollection<TicketGenration> CustomerTickets { get; set; }
    }
    public class CustomerWiseStatusMst
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public int CustId { get; set; }
        public string Status { get; set; }
        public int OrderDays { get; set; }
    }

    public class CustomerEmailMaster
    {
        [Key]
        public int Id { get; set; }
        public int CustId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public int? EmailTypeId { get; set; }
    }

    public class EmailType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ExcelParameters
    {
        [Key]
        public int Id { get; set; }
        public string ParameterName { get; set; }
        public string HeaderName { get; set; }
        [ForeignKey("OrderFormatExcel")]
        public int OrderFormatExcelId { get; set; }
        public virtual OrderFormatExcel OrderFormatExcel { get; set; }
    }
    public class OrderFormatExcel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Customer")]
        public int CustId { get; set; }
        //public int OrderType { get; set; }
        public string OrderType { get; set; }
        public virtual List<ExcelParameters> ExcelHeaders { get; set; }
        public virtual Customer Customer { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? Created { get; set; }
        public int? CreatedBy { get; set; }
        //public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
