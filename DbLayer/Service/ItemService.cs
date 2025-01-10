using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DbLayer.Repository;

namespace DbLayer.Service
{
    public interface IItemService
    {
        List<Item> GetItems(int storeId);
        Item GetItemById(int Id);
        int AddItem(Item item);
        List<ItemData> GetAllItems(string Opr);
        int UpdateItems(Item item);
        List<Top5SKU> Top5SKUs(DateTime FromDate, DateTime ToDate, int custid, int userid);
        int DeleteItem(int id, bool isDeleted);
        List<UOMmaster> UOMmasters();
        List<Category> Categories();
        List<SubCategory> SubCategories();
        int InsertUOM(UOMmaster uOMmaster);
        int UpdateUOM(UOMmaster uOMmaster);
        int DeleteUOM(int id);
        UOMmaster UOMmaster(int id);
        int InsertCategory(Category category);
        int UpdateCategory(Category category);
        int DeleteCategory(int id);
        Category Categorymaster(int id);
        int InsertSubCategory(SubCategory subCategory);
        int UpdateSubCategory(SubCategory subCategory);
        int DeleteSubCategory(int id);
        SubCategory SubCategorymaster(int id);
        List<Item> CustomerWideItem(int CustId);
        List<int> GetItemsByCustomer(int CustId);
        List<Item> GetItemsForCustomer(int CustId);

        List<DropDown> CustomerWideItem(int CustId, string CategoryName);
        List<ItemView> GetSkuForUserCustomer(int userid, int custid, string OrderReport, int StoreId = 0);
        List<ItemCategoryView> GetItemCategoriesByCustomer(int userId, int custId, int? StoreID = 0);
        List<ItemView> GetSkuForUserCustomerItemCategory(int userId, int custId, List<int> SelectedTypes, List<int> selectedCategory, string OrderReport, int StoreId = 0);
        List<CategoryTypeView> GetItemTypeByCustomer(int userId, int custId, int? StoreID = 0);
        List<ItemCategoryView> GetItemCategoriesByCustomerForItemTypes(int userId, int custId, List<int> selectedItemTypes, int? StoreId = 0);
        //Brand GetBrandById(int id);
        List<ItemDetailView> GetItems(int userid, int custid, int storeId, List<int> SelectedTypes, List<int> SelectedItemCategory, List<int> selectedBrand);
        List<ItemDetailView> GetTopItems(int userid, int custid, int storeId, List<int> SelectedTypes, List<int> SelectedItemCategory, List<int> SelectedTopItems);
        List<ItemSubTypeByCustomer> GetItemSubTypeByCustomer(int userid, int custid);


        // List<ItemProduct> GetItemsAndProducts(int storeId);
        //List<OrderSave> GetItemsForProduct(OrderSave product);
        #region brand
        List<Brand> GetBrands(string Opr);
        int AddBrand(Brand brand);
        int UpdateBrand(Brand cat);
        Brand GetBrandById(int id);
        int DeleteBrand(int id, int UserId, bool isDelete);
        List<Brand> GetBrandsForUserCustomer(int UserId, int CustomerId, string OrderReport, int? StoreId = null);
        List<StoreItemView> GetItemForCustomerStoreActivation(int custId, List<int> dcIds, List<int> itemIds);
        List<CategoryType> ItemCategoryTypes(string v);
        bool UpdateDcStoreItemMapping(StoreItemView data, string opr, int userId);
        bool UpdateAllDcStoreItemMapping(int custId, string opr, List<int> items, int isDeleted, List<int> stores, List<int> dcs, int userId);
        List<StoreItemView> GetItemsForStoreActivation(string v, int custId, List<int> storeIds, List<int> itemIds);
        #endregion
    }
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemrepository;
        public ItemService(IItemRepository itemRepository)
        {
            _itemrepository = itemRepository;
        }

        public List<Item> GetItems(int storeId)
        {
            return _itemrepository.GetItems(storeId);
        }

        public Item GetItemById(int Id)
        {
            return _itemrepository.GetItemById(Id);
        }
        public int AddItem(Item item)
        {
            return _itemrepository.AddItem(item);
        }

        public List<ItemData> GetAllItems(string Opr)
        {
            List<ItemData> Items = _itemrepository.GetAllItems(Opr);
            //List<ItemData> itemdata = Items.Select(x => new ItemData
            //{
            //    CustomerNames = string.Join(", ", x.Customers.Select(y => y.Name).ToArray()),
            //    Created=x.Created,
            //    Modified=x.Modified,
            //    SKUCode=x.SKUCode,
            //    SKUName=x.SKUName,
            //    Id=x.Id,
            //    IsDeleted=x.IsDeleted
            //}).ToList();
            return Items;
        }

        public int UpdateItems(Item item)
        {
            return _itemrepository.UpdateItems(item);
        }

        public List<Top5SKU> Top5SKUs(DateTime FromDate, DateTime ToDate, int custid, int userid)
        {
            return _itemrepository.Top5SKUs(FromDate, ToDate, custid, userid);
        }

        public int DeleteItem(int id, bool isDeleted)
        {
            return _itemrepository.DeleteItem(id, isDeleted);
        }

        public List<UOMmaster> UOMmasters()
        {
            return _itemrepository.UOMmasters();
        }

        public List<Category> Categories()
        {
            return _itemrepository.Categories();
        }

        public List<SubCategory> SubCategories()
        {
            return _itemrepository.SubCategories();
        }

        public int InsertUOM(UOMmaster uOMmaster)
        {
            return _itemrepository.InsertUOM(uOMmaster);
        }

        public int UpdateUOM(UOMmaster uOMmaster)
        {
            return _itemrepository.UpdateUOM(uOMmaster);
        }

        public int DeleteUOM(int id)
        {
            return _itemrepository.DeleteUOM(id);
        }

        public UOMmaster UOMmaster(int id)
        {
            return _itemrepository.UOMmaster(id);
        }

        public int InsertCategory(Category category)
        {
            return _itemrepository.InsertCategory(category);
        }

        public int UpdateCategory(Category category)
        {
            return _itemrepository.UpdateCategory(category);
        }

        public int DeleteCategory(int id)
        {
            return _itemrepository.DeleteCategory(id);
        }

        public Category Categorymaster(int id)
        {
            return _itemrepository.Categorymaster(id);
        }

        public int InsertSubCategory(SubCategory subCategory)
        {
            return _itemrepository.InsertSubCategory(subCategory);
        }

        public int UpdateSubCategory(SubCategory subCategory)
        {
            return _itemrepository.UpdateSubCategory(subCategory);
        }

        public int DeleteSubCategory(int id)
        {
            return _itemrepository.DeleteSubCategory(id);
        }

        public SubCategory SubCategorymaster(int id)
        {
            return _itemrepository.SubCategorymaster(id);
        }

        public List<Item> CustomerWideItem(int CustId)
        {
            return _itemrepository.CustomerWideItem(CustId);
        }

        public List<int> GetItemsByCustomer(int CustId)
        {
            return _itemrepository.GetItemsByCustomer(CustId);
        }

        public List<DropDown> CustomerWideItem(int CustId, string CategoryName)
        {
            return _itemrepository.CustomerWideItem(CustId, CategoryName);
        }

        public List<Item> GetItemsForCustomer(int CustId)
        {
            return _itemrepository.GetItemsForCustomer(CustId);
        }
        //public List<ItemProduct> GetItemsAndProducts(int storeId)
        //{
        //    return _itemrepository.GetItemsAndProducts(storeId);
        //}

        //public List<OrderSave> GetItemsForProduct(OrderSave product)
        //{
        //    return _itemrepository.GetItemsForProduct(product);
        //}
        public List<ItemView> GetSkuForUserCustomer(int userid, int custid, string OrderReport, int StoreId = 0)
        {
            return _itemrepository.GetSkuForUserCustomer(userid, custid, OrderReport, StoreId);
        }
        public List<ItemCategoryView> GetItemCategoriesByCustomer(int userId, int custId, int? StoreID = 0)
        {
            return _itemrepository.GetItemCategoriesByCustomer(userId, custId, StoreID);
        }
        public List<ItemView> GetSkuForUserCustomerItemCategory(int userId, int custId, List<int> SelectedTypes, List<int> selectedCategory, string OrderReport, int StoreId = 0)
        {
            return _itemrepository.GetSkuForUserCustomerItemCategory(userId, custId, SelectedTypes, selectedCategory, OrderReport, StoreId);
        }
        public List<CategoryTypeView> GetItemTypeByCustomer(int userId, int custId, int? StoreID = 0)
        {
            return _itemrepository.GetItemTypeByCustomer(userId, custId, StoreID);
        }
        public List<ItemCategoryView> GetItemCategoriesByCustomerForItemTypes(int userId, int custId, List<int> selectedItemTypes, int? StoreId = 0)
        {
            return _itemrepository.GetItemCategoriesByCustomerForItemTypes(userId, custId, selectedItemTypes, StoreId);
        }

        public List<ItemDetailView> GetItems(int userid, int custid, int storeId, List<int> SelectedTypes, List<int> SelectedItemCategory, List<int> selectedBrand)
        {
            return _itemrepository.GetItems(userid, custid, storeId, SelectedTypes, SelectedItemCategory, selectedBrand);
        }
        public List<ItemDetailView> GetTopItems(int userid, int custid, int storeId, List<int> SelectedTypes, List<int> SelectedItemCategory, List<int> SelectedTopItems)
        {
            return _itemrepository.GetTopItems(userid, custid, storeId, SelectedTypes, SelectedItemCategory, SelectedTopItems);
        }
        public List<ItemSubTypeByCustomer> GetItemSubTypeByCustomer(int userid, int custid)
        {
            return _itemrepository.GetItemSubTypeByCustomer(userid, custid);
        }

        #region brand
        public List<Brand> GetBrands(string Opr)
        {
            return _itemrepository.GetBrands(Opr);
        }
        public int AddBrand(Brand brand)
        {
            return _itemrepository.AddBrand(brand);
        }
        public int UpdateBrand(Brand brand)
        {
            return _itemrepository.UpdateBrand(brand);
        }
        public Brand GetBrandById(int BrandId)
        {
            return _itemrepository.GetBrandById(BrandId);
        }
        public int DeleteBrand(int id, int userid, bool isDelete)
        {
            return _itemrepository.DeleteBrand(id, userid, isDelete);
        }
        public List<Brand> GetBrandsForUserCustomer(int UserId, int CustomerId, string OrderReport, int? StoreId = null)
        {
            return _itemrepository.GetBrandsForUserCustomer(UserId, CustomerId, OrderReport, StoreId);
        }
        #endregion
        public List<StoreItemView> GetItemForCustomerStoreActivation(int custId, List<int> dcIds, List<int> itemIds)
        {
            return _itemrepository.GetItemForCustomerStoreActivation(custId, dcIds, itemIds);
        }
        public List<CategoryType> ItemCategoryTypes(string Opr)
        {
            return _itemrepository.ItemCategoryTypes(Opr);
        }
        public bool UpdateDcStoreItemMapping(StoreItemView data, string opr, int userId)
        {
            return _itemrepository.UpdateDcStoreItemMapping(data, opr, userId);
        }
        public bool UpdateAllDcStoreItemMapping(int custId, string opr, List<int> items, int isDeleted, List<int> stores, List<int> dcs, int userId)
        {
            return _itemrepository.UpdateAllDcStoreItemMapping(custId, opr, items, isDeleted, stores, dcs, userId);
        }
        public List<StoreItemView> GetItemsForStoreActivation(string opr, int custId, List<int> storeIds, List<int> itemIds)
        {
            return _itemrepository.GetItemsForStoreActivation(opr, custId, storeIds, itemIds);
        }
    }
}
