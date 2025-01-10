using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("ItemMst")]
    public class Item
    {
        private ICollection<Customer> customers;
        public Item()
        {

        }
        public Item(int id, string sKUName, string sKUCode, string packingDescription, string minimumOrder, decimal maximumOrderLimit, decimal caseConversion, decimal totalWeightPerCase, int UOMmasterId, int CategoryId, int SubCategoryId, ICollection<Customer> customers, IEnumerable<int> SelectedCustomers, bool isDeleted)
        {
            this.Id = id;
            SKUName = sKUName;
            SKUCode = sKUCode;
            PackingDescription = packingDescription;
            MinimumOrder = minimumOrder;
            MaximumOrderLimit = maximumOrderLimit;
            CaseConversion = caseConversion;
            TotalWeightPerCase = totalWeightPerCase;
            this.UOMmasterId = UOMmasterId;
            this.CategoryId = CategoryId;
            this.SubCategoryId = SubCategoryId;
            this.customers = customers;
            this.SelectedCustomers = SelectedCustomers;
            IsDeleted = isDeleted;
        }

        [Key]
        public int Id { get; set; }
        public string SKUCode { get; set; }
        public string SKUName { get; set; }
        public string PackingDescription { get; set; }
        //public string UOM { get; set; }
        //public string UOMDescription { get; set; }
        public string MinimumOrder { get; set; }
        //  public string Category { get; set; }
        public string Customer { get; set; }
        public string Active { get; set; }
        public decimal MaximumOrderLimit { get; set; }
        public decimal CaseConversion { get; set; }
        public decimal TotalWeightPerCase { get; set; }
        public decimal? PackingSize { get; set; }
        [NotMapped]
        public IEnumerable<int> SelectedCustomers { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        //[ForeignKey("UOMmaster")]
        //public int UOMId { get; set; }
        public virtual UOMmaster UOMmaster { get; set; }
        public virtual CategoryType ItemCategoryType { get; set; }
        public virtual Category Category { get; set; }
        public virtual SubCategory SubCategory { get; set; }

        [ForeignKey("ItemCategoryType")]
        public int? ItemCategoryTypeId { get; set; }


        public DateTime? Created { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        //[NotMapped]
        public int? BrandId { get; set; }
        [NotMapped]
        public int UOMmasterId { get; set; }
        [NotMapped]
        public int CategoryId { get; set; }
        [NotMapped]
        public int SubCategoryId { get; set; }
        [NotMapped]
        public string CustomerNames { get; set; }
    }

    #region _CurrentInventoryMst
    [Table(name: "CurrentItemInventoryMst")]
    public class CurrentItemInventoryMst
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [ForeignKey("Store")]
        public int? StoreId { get; set; }
        public virtual Store Store { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        [ForeignKey("OrderHeader")]
        public Int64? LastOrderId { get; set; }
        public virtual OrderHeader OrderHeader { get; set; }

        public DateTime? LastOrderDate { get; set; }
        public int? OrderQuantity { get; set; }

        public DateTime? POSSaleDate { get; set; }
        public int? POSSalesQuantity { get; set; }

        [ForeignKey("DispatchedOrderHeader")]
        public Int64? DispatchedOrderId { get; set; }
        public virtual OrderHeader DispatchedOrderHeader { get; set; }

        public DateTime? DispatchDate { get; set; }
        public int? DispatchQuantity { get; set; }

        public DateTime? PODDate { get; set; }
        public int? PODQuantity { get; set; }


        public DateTime? SuggessionUpdatedDate { get; set; }
        public int? SuggestedQuantity { get; set; }

        [DataType(DataType.Text)]
        public string LastModifiedByAPI { get; set; }
    }
    #endregion

    public class ItemView
    {
        public int Id { get; set; }
        public string SKUCode { get; set; }
    }

    public class ItemCategoryView
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
    }

    public class ItemDetailView
    {
        public int Id { get; set; }
        public string SKUCode { get; set; }
        public string SKUName { get; set; }
        public string CaseConversion { get; set; }
        public string BrandName { get; set; }
        public int BrandId { get; set; }
        public string UnitofMasureDescription { get; set; }
        public decimal MaximumOrderLimit { get; set; }
        public decimal MinimumOrderQuantity { get; set; }
        public decimal CaseSize { get; set; }
    }

    public class TopItemCustomers
    {
        public long? Id { get; set; }
        public int Item_Id { get; set; }
        public string SKUCode { get; set; }
        public string SKUName { get; set; }
        public string ItemCategoryType { get; set; }
        public int ItemCategoryTypeId { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public int Customer_Id { get; set; }
        public int Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public bool Checked { get; set; }
    }
    public class TopItemList
    {
        public int TopItemListId { get; set; }
        public List<CategoryTypeView> ItemType { get; set; }
        public List<ItemCategoryView> ItemCategories { get; set; }
        public List<TopItemCustomers> TopItemCustomers { get; set; }
    }

    public class ItemData
    {
        public string CustomerNames { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public string SKUCode { get; set; }
        public string SKUName { get; set; }
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class ItemMasterUploadViewModel
    {
        public string Customer { get; set; }
        public int Customer_Id { get; set; }

        public string SKUCode { get; set; }
        public int SKUCode_Id { get; set; }

        public string SKUName { get; set; }
        public int SKUName_Id { get; set; }

        public string UOM { get; set; }
        public int UOM_Id { get; set; }

        public string Category { get; set; }
        public int Category_Id { get; set; }

        public string SubCategory { get; set; }
        public int SubCategory_Id { get; set; }

        public string ItemCategoryType { get; set; }
        public int ItemCategoryType_Id { get; set; }

        public string Brand { get; set; }
        public int Brand_Id { get; set; }

        public string TotalWeightPerCase { get; set; }
        public int TotalWeightPerCase_Id { get; set; }

        public string MinimumOrder { get; set; }
        public int MinimumOrder_Id { get; set; }

        public string MaximumOrderLimit { get; set; }
        public int MaximumOrderLimit_Id { get; set; }

        public string CaseConversion { get; set; }
        public int CaseConversion_Id { get; set; }
        
        public string Error { get; set; }
        public int Xls_Line_No { get; set; }
        public int Upload_By { get; set; }
    }
    public class ItemMasterUploadViewModelMapping
    {

    }



}
