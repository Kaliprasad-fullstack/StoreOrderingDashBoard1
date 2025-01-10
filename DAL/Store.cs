using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("StoreMst")]
    public class Store
    {
        public Store()
        {
        }
        public Store(int Id, string StoreCode, string StoreName, bool IsDeleted)
        {
            this.Id = Id;
            this.StoreCode = StoreCode;
            this.StoreName = StoreName;
            this.IsDeleted = IsDeleted;
        }
        public Store(int Id, string StoreCode, string StoreName, string Address, string StoreEmailId, string Region, string Route, string StoreManager, string StoreContactNo, DateTime? DeliveryInTime, DateTime? DeliveryOutTime, int CreatedBy, int ModifiedBy, string Password, string PlaceOfSupply, int CustId, bool IsDeleted, DateTime CreatedDate, DateTime ModifiedDate, int LocationId, int NetSuitMapClsId)
        {
            this.Id = Id;
            this.StoreCode = StoreCode;
            this.StoreName = StoreName;
            this.Address = Address;
            this.StoreEmailId = StoreEmailId;
            this.Region = Region;
            this.Route = Route;
            this.StoreManager = StoreManager;
            this.StoreContactNo = StoreContactNo;
            this.DeliveryInTime = DeliveryInTime;
            this.DeliveryOutTime = DeliveryOutTime;
            this.CreatedBy = CreatedBy;
            this.ModifiedBy = ModifiedBy;
            this.Password = Password;
            this.PlaceOfSupply = PlaceOfSupply;
            this.CustId = CustId;
            this.IsDeleted = IsDeleted;
            this.CreatedDate = CreatedDate;
            this.ModifiedDate = ModifiedDate;
            this.LocationId = LocationId;
            this.NetSuitMapClsId = NetSuitMapClsId;
        }
        [Key]
        public int Id { get; set; }
        //  public string Location { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public string StoreEmailId { get; set; }
        public string Region { get; set; }
        public string Route { get; set; }
        public string StoreManager { get; set; }
        public string StoreContactNo { get; set; }
        public DateTime? DeliveryInTime { get; set; }
        public DateTime? DeliveryOutTime { get; set; }
        //  public string PinCode { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public string Password { get; set; }
        public string PlaceOfSupply { get; set; }
        [ForeignKey("Customer")]
        public int CustId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual WareHouseDC WareHouseDC { get; set; }
        //   public virtual CustomerPlan CustomerPlan { get; set; }
        [NotMapped]
        public int NetSuitMapClsId { get; set; }
        public virtual NetSuitMapCls NetSuitMapCls { get; set; }
        public virtual ICollection<OrderHeader> orderHeaders { get; set; }
        public virtual ICollection<Plan> Plans { get; set; }
        public virtual ICollection<DashBoardMapping> DashBoardMappings { get; set; }
        // [ForeignKey("WareHouseDC")]
        //public int DCID { get; set; }
        [ForeignKey("Locations")]
        //[NotMapped]
        public int LocationId { get; set; }
        // public virtual WareHouseDC WareHouseDC { get; set; }
        public virtual Locations Locations { get; set; }
        public ICollection<PermitableCls> PermitableCls { get; set; }
        public virtual ICollection<EscalationAction> EscalationActions { get; set; }
        //public virtual ICollection<TicketGenration> StoreTickets { get; set; }

    }

    public class StoreView
    {
        public int Id { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
    }
    [NotMapped]
    public class JsonStore
    {
        public int? StoreId { get; set; }
        public int CustId { get; set; }
        public int? UserId { get; set; }
    }
    [NotMapped]
    public class StoreSuggesions
    {
        public int data { get; set; }
        public string value { get; set; }
    }
    [NotMapped]
    public class Suggesions
    {
        public string query { get; set; }
        public List<StoreSuggesions> suggestions { get; set; }
    }
    [NotMapped]
    public class ControllerAction
    {
        public string Controller { get; set; }
        public string Action { get; set; }
    }
    [NotMapped]
    public class VehicleSuggesions
    {
        public string query { get; set; }
        public List<VehicleSuggesion> suggestions { get; set; }
    }
    [NotMapped]
    public class VehicleSuggesion
    {
        public string data { get; set; }
        public string value { get; set; }
    }
    [NotMapped]
    public class CityView
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
    }

    [NotMapped]
    public class StoreModel
    {
        public int Id { get; set; }
        public int CustId { get ; set; }
        public string CustomerName { get; set; }
        public string EnterpriseCode { get; set; }
        public string EnterpriseName { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public string StoreEmailId { get; set; }
        public string Region { get; set; }
        public string Route { get; set; }
        public string StoreManager { get; set; }
        public string StoreContactNo { get; set; }
        public DateTime? DeliveryInTime { get; set; }
        public DateTime? DeliveryOutTime { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Password { get; set; }
        public string PlaceOfSupply { get; set; }
        public bool IsDeleted { get; set; }
        public string Active { get; set; }
        public int NetSuitMapClsId { get; set; }
        public int LocationId { get; set; }
        public string Location { get; set; }
        public string WareHouseDCName { get; set; }
        public string UserEmailAddress { get; set; }
    }
    public class StoreMasterUploadViewModel
    {
        public string Customer { get; set; }
        public int Customer_Id { get; set; }

        public string StoreCode { get; set; }
        public int StoreCode_Id { get; set; }

        public string StoreName { get; set; }
        public int StoreName_Id { get; set; }

        public string Address { get; set; }
        public int Address_Id { get; set; }

        public string Location { get; set; }
        public int Location_Id { get; set; }

        public string WareHouse { get; set; }
        public int WareHouse_Id { get; set; }

        public string EmailAddress { get; set; }
        public int EmailAddress_Id { get; set; }

        public string StoreManager { get; set; }
        public int StoreManager_Id { get; set; }

        public string StoreContactNo { get; set; }
        public int StoreContactNo_Id { get; set; }

        public string PlaceOfSupply { get; set; }
        public int PlaceOfSupply_Id { get; set; }

        public string NetSuitClass { get; set; }
        public int NetSuitClass_Id { get; set; }

        public string EnterpriseCode { get; set; }
        public int EnterpriseCode_Id { get; set; }

        public string EnterpriseName { get; set; }
        public int EnterpriseName_Id { get; set; }

        public string Error { get; set; }
        public int Xls_Line_No { get; set; }
        public int Upload_By { get; set; }
    }
    public class DCStoreWiseItemReport
    {
        public string DC { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string MinimumOrder { get; set; }
        public decimal MaximumOrderLimit { get; set; }
        public decimal CaseConversion { get; set; }
        public string UOMMaster { get; set; }
        public string CategoryType { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Brand { get; set; }
        public string Active { get; set; }
    }

    public class ItemReport
    {
        public string Customer { get; set; }
        public string UOMMaster { get; set; }
        public string PackingDescription { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string MinimumOrder { get; set; }
        public decimal MaximumOrderLimit { get; set; }
        public decimal CaseConversion { get; set; }
        public string CategoryType { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Brand { get; set; }
    }
}
