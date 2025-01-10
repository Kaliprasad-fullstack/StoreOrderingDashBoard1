using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL;
using DAL;
using DbLayer.Service;

namespace DbLayer.Repository
{
    public interface IItemRepository
    {
        List<Item> GetItems(int storeId);
        Item GetItemById(int Id);
        int AddItem(Item item);
        List<Item> GetAllItems();
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
        List<DropDown> CustomerWideItem(int CustId, string CategoryName);
        List<Item> GetItemsForCustomer(int custId);
        List<ItemView> GetSkuForUserCustomer(int userid, int custid, string OrderReport, int StoreId = 0);
        List<ItemCategoryView> GetItemCategoriesByCustomer(int userid, int custid, int? StoreID = 0);
        List<ItemView> GetSkuForUserCustomerItemCategory(int userId, int custId, List<int> SelectedTypes, List<int> selectedCategory, string OrderReport, int StoreId = 0);
        List<CategoryTypeView> GetItemTypeByCustomer(int userId, int custId, int? StoreID = 0);
        List<ItemCategoryView> GetItemCategoriesByCustomerForItemTypes(int userId, int custId, List<int> selectedItemTypes, int? StoreId = 0);
        List<ItemDetailView> GetItems(int userid, int custid, int storeId, List<int> selectedTypes, List<int> selectedItemCategory, List<int> selectedBrand);
        List<ItemDetailView> GetTopItems(int userid, int custid, int storeId, List<int> SelectedTypes, List<int> SelectedItemCategory, List<int> SelectedTopItems);
        List<ItemSubTypeByCustomer> GetItemSubTypeByCustomer(int userid, int custid);
        #region brands
        List<Brand> GetBrands(string Opr);
        int AddBrand(Brand brand);
        int UpdateBrand(Brand cat);
        Brand GetBrandById(int brandId);
        int DeleteBrand(int id, int userid, bool isDelete);
        List<Brand> GetBrandsForUserCustomer(int userId, int customerId, string orderReport, int? StoreId);
        List<StoreItemView> GetItemForCustomerStoreActivation(int custId, List<int> dcIds, List<int> itemIds);
        List<CategoryType> ItemCategoryTypes(string opr);
        bool UpdateDcStoreItemMapping(StoreItemView data, string opr, int userId);
        bool UpdateAllDcStoreItemMapping(int custId, string opr, List<int> items, int isDeleted, List<int> stores, List<int> dcs, int userId);
        List<StoreItemView> GetItemsForStoreActivation(string opr, int custId, List<int> storeIds, List<int> itemIds);
        #endregion
        //List<ItemProduct> GetItemsAndProducts(int storeId);
        //List<OrderSave> GetItemsForProduct(OrderSave product);
    }

    public class ItemRepository : IItemRepository
    {
        private readonly StoreContext _context;
        private readonly ICustomerService _customerService;
        public ItemRepository(StoreContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }
        public List<Item> GetItems(int storeId)
        {
            try
            {
                var customer = _context.Stores.Where(x => x.Id == storeId && x.IsDeleted == false).FirstOrDefault();
                var query = from item in _context.Items
                            where item.Customers.Any(x => x.Id == customer.Customer.Id) && item.IsDeleted == false
                            select item;
                return query.ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            //try
            //{
            //    return _context.Database.SqlQuery<ItemProduct>("exec USP_GetProducts_Items @StoreId",
            //       new SqlParameter("@StoreId", storeId)).ToList();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }
        //public List<ItemProduct> GetItemsAndProducts(int storeId)
        //{
        //    try
        //    {
        //        return _context.Database.SqlQuery<ItemProduct>("exec USP_GetProducts_Items @StoreId",
        //           new SqlParameter("@StoreId", storeId)).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public Item GetItemById(int Id)
        {
            try
            {
                return _context.Items.Where(x => x.IsDeleted == false && x.Id == Id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public int AddItem(Item item)
        {
            try
            {
                //var Customers = _customerService.GetSelectedCustomers(item.SelectedCustomers);
                //item.Customers = new List<Customer>();
                //item.UOMmaster = _context.uOMmasters.Find(item.UOMmaster.Id);
                //item.Category = _context.categories.Find(item.Category.Id);
                //item.SubCategory = _context.subCategories.Find(item.SubCategory.Id);
                //foreach (Customer cust in Customers)
                //{
                //    var customer = new Customer { Id = cust.Id };
                //    Customer cust1 = new Customer();
                //    cust1 = customer;
                //    _context.Customers.Attach(cust1);
                //    //_context.Items..Add(customer); 

                //    item.Customers.Add(customer);
                //}
                //_context.Items.Add(item);
                //// _context.Entry(item).State = EntityState.Modified;
                ////_context.Entry(item).State == EntityState.Detached;
                ///

                string consString = System.Configuration.ConfigurationManager.ConnectionStrings["StoreConnectionString"].ConnectionString;// _context.Database.Connection.ConnectionString;
                using (SqlConnection con = new SqlConnection(consString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_AddUpdateItems"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@OPR", "ADD");
                        cmd.Parameters.AddWithValue("@Id", item.Id);
                        cmd.Parameters.AddWithValue("@SKUCode", item.SKUCode);
                        cmd.Parameters.AddWithValue("@SKUName", item.SKUName);
                        cmd.Parameters.AddWithValue("@PackingDescription", item.PackingDescription);
                        cmd.Parameters.AddWithValue("@MinimumOrder", item.MinimumOrder);
                        cmd.Parameters.AddWithValue("@MaximumOrderLimit", item.MaximumOrderLimit);
                        cmd.Parameters.AddWithValue("@CaseConversion", item.CaseConversion);
                        cmd.Parameters.AddWithValue("@TotalWeightPerCase", item.TotalWeightPerCase);
                        cmd.Parameters.AddWithValue("@UOMmasterId", item.UOMmaster.Id);
                        cmd.Parameters.AddWithValue("@ItemCategoryType", item.ItemCategoryType.Id);
                        cmd.Parameters.AddWithValue("@CategoryId", item.Category.Id);
                        cmd.Parameters.AddWithValue("@SubCategoryId", item.SubCategory.Id);
                        cmd.Parameters.AddWithValue("@BrandId", item.BrandId);
                        cmd.Parameters.AddWithValue("@Customers", string.Join(",", item.SelectedCustomers.ToArray()));
                        cmd.Parameters.AddWithValue("@CreatedBy", item.CreatedBy);
                        cmd.Parameters.AddWithValue("@ModifiedBy", item.ModifiedBy);
                        cmd.Parameters.AddWithValue("@IsDeleted", item.IsDeleted);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                int rowsaffected = _context.SaveChanges();
                return rowsaffected;
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<Item> GetAllItems()
        {
            try
            {
                //return _context.Items.Where(x => x.IsDeleted == false).ToList();
                //_context.Configuration.ProxyCreationEnabled = false;
                return _context.Items.ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<ItemData> GetAllItems(string Opr)
        {

            try
            {
                return _context.Database.SqlQuery<ItemData>(
                    string.Format("exec USP_GetAllItems @Opr='{0}'", Opr)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        private IEnumerable<Customer> GetStopsToAdd(IEnumerable<int> originalStopsId, IEnumerable<int> editedStopsId)
        {
            var stopToAdd = new List<Customer>();
            // stops in editedStopsId and not in originalStopsId will be added
            var stopIdsToAdd = editedStopsId.Except(originalStopsId);
            foreach (var id in stopIdsToAdd)
            {
                var customer = _context.Customers.Find(id);
                if (!stopToAdd.Contains(customer))
                    stopToAdd.Add(customer);
            }
            return stopToAdd;
        }

        private IEnumerable<Customer> GetStopsToRemove(IEnumerable<Customer> originalRouteStops, IEnumerable<int> stopToRemove)
        {
            var stopsToRemove = new List<Customer>();
            foreach (var id in stopToRemove)
            {
                var stop = originalRouteStops.FirstOrDefault(x => x.Id == id);
                if (!stopsToRemove.Contains(stop))
                    stopsToRemove.Add(stop);
            }
            return stopsToRemove;
        }

        public int UpdateItems(Item item)
        {
            //try
            //{
            //    //get original item
            //    var originalitem = _context.Items.Find(item.Id);
            //    originalitem.SKUName = item.SKUName;
            //    originalitem.SKUCode = item.SKUCode;
            //    originalitem.PackingDescription = item.PackingDescription;
            //    originalitem.MinimumOrder = item.MinimumOrder;
            //    originalitem.MaximumOrderLimit = item.MaximumOrderLimit;
            //    originalitem.CaseConversion = item.CaseConversion;
            //    originalitem.TotalWeightPerCase = item.TotalWeightPerCase;
            //    originalitem.UOMmaster = _context.uOMmasters.Find(item.UOMmaster.Id);
            //    originalitem.Category = _context.categories.Find(item.Category.Id);
            //    originalitem.SubCategory = _context.subCategories.Find(item.SubCategory.Id);
            //    originalitem.ItemCategoryType = _context.ItemType.Find(item.ItemCategoryType.Id);
            //    originalitem.Modified = DateTime.Now;
            //    originalitem.ModifiedBy = item.ModifiedBy;
            //    originalitem.BrandId = item.BrandId;
            //    //get original customer id using lazy loading
            //    var originalcustomer = originalitem.Customers.Select(x => x.Id);
            //    var editedcustomer = item.SelectedCustomers;
            //    // customer in originalcustomer and not in editedcustomer will be added   
            //    var customerToAdd = GetStopsToAdd(originalcustomer, editedcustomer);
            //    customerToAdd.ToList().ForEach(x => originalitem.Customers.Add(x));
            //    //customer in originalcustomer and not in editedcustomer will be removed
            //    var customertoremove = originalcustomer.Except(editedcustomer);
            //    var CustomerToRemove = GetStopsToRemove(originalitem.Customers, customertoremove);
            //    CustomerToRemove.ToList().ForEach(x => originalitem.Customers.Remove(x));
            //    _context.Entry(originalitem).State = EntityState.Modified;
            //    return _context.SaveChanges();
            //}
            //catch (Exception ex)
            //{
            //    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
            //    throw ex;
            //}

            string consString = System.Configuration.ConfigurationManager.ConnectionStrings["StoreConnectionString"].ConnectionString;// _context.Database.Connection.ConnectionString;
            using (SqlConnection con = new SqlConnection(consString))
            {
                using (SqlCommand cmd = new SqlCommand("USP_AddUpdateItems"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@OPR", "UPDATE");
                    cmd.Parameters.AddWithValue("@Id", item.Id);
                    cmd.Parameters.AddWithValue("@SKUCode", item.SKUCode);
                    cmd.Parameters.AddWithValue("@SKUName", item.SKUName);
                    cmd.Parameters.AddWithValue("@PackingDescription", item.PackingDescription);
                    cmd.Parameters.AddWithValue("@MinimumOrder", item.MinimumOrder);
                    cmd.Parameters.AddWithValue("@MaximumOrderLimit", item.MaximumOrderLimit);
                    cmd.Parameters.AddWithValue("@CaseConversion", item.CaseConversion);
                    cmd.Parameters.AddWithValue("@TotalWeightPerCase", item.TotalWeightPerCase);
                    cmd.Parameters.AddWithValue("@UOMmasterId", item.UOMmaster.Id);
                    cmd.Parameters.AddWithValue("@ItemCategoryType", item.ItemCategoryType.Id);
                    cmd.Parameters.AddWithValue("@CategoryId", item.Category.Id);
                    cmd.Parameters.AddWithValue("@SubCategoryId", item.SubCategory.Id);
                    cmd.Parameters.AddWithValue("@BrandId", item.BrandId);
                    cmd.Parameters.AddWithValue("@Customers", string.Join(",", item.SelectedCustomers.ToArray()));
                    cmd.Parameters.AddWithValue("@CreatedBy", item.CreatedBy);
                    cmd.Parameters.AddWithValue("@ModifiedBy", item.ModifiedBy);
                    cmd.Parameters.AddWithValue("@IsDeleted", item.IsDeleted);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            int rowsaffected = _context.SaveChanges();
            return rowsaffected;
        }

        public List<Top5SKU> Top5SKUs(DateTime FromDate, DateTime ToDate, int custid, int userid)
        {
            try
            {
                return _context.Database.SqlQuery<Top5SKU>("exec USP_Top5Sku @FromDate,@ToDate,@CustId,@UserId",
                   new SqlParameter("@FromDate", FromDate),
                   new SqlParameter("@ToDate", ToDate),
                   new SqlParameter("@CustId", custid),
                   new SqlParameter("@UserId", userid)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteItem(int id, bool isDeleted)
        {
            try
            {
                var item = _context.Items.Find(id);
                item.IsDeleted = isDeleted;
                _context.Entry(item).State = EntityState.Modified;
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<UOMmaster> UOMmasters()
        {
            try
            {
                return _context.uOMmasters.Where(x => x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<Category> Categories()
        {
            try
            {
                return _context.categories.Where(x => x.isDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<SubCategory> SubCategories()
        {
            try
            {
                return _context.subCategories.Where(x => x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int InsertUOM(UOMmaster uOMmaster)
        {
            try
            {
                _context.uOMmasters.Add(uOMmaster);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int UpdateUOM(UOMmaster uOMmaster)
        {
            try
            {
                _context.Entry(uOMmaster).State = EntityState.Modified;
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }

        }

        public int DeleteUOM(int id)
        {
            try
            {
                var UOM = _context.uOMmasters.Find(id);
                UOM.IsDeleted = true;
                _context.Entry(UOM).State = EntityState.Modified;
                return _context.SaveChanges();

            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public UOMmaster UOMmaster(int id)
        {
            try
            {
                return _context.uOMmasters.Find(id);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int InsertCategory(Category category)
        {
            try
            {
                _context.categories.Add(category);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int UpdateCategory(Category category)
        {
            try
            {
                _context.Entry(category).State = EntityState.Modified;
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int DeleteCategory(int id)
        {
            try
            {
                var category = _context.categories.Find(id);
                category.isDeleted = true;
                _context.Entry(category).State = EntityState.Modified;
                return _context.SaveChanges();

            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public Category Categorymaster(int id)
        {
            try
            {
                return _context.categories.Find(id);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int InsertSubCategory(SubCategory subCategory)
        {
            try
            {
                _context.subCategories.Add(subCategory);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int UpdateSubCategory(SubCategory subCategory)
        {
            try
            {
                _context.Entry(subCategory).State = EntityState.Modified;
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int DeleteSubCategory(int id)
        {
            try
            {
                var subcategory = _context.subCategories.Find(id);
                subcategory.IsDeleted = true;
                _context.Entry(subcategory).State = EntityState.Modified;
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public SubCategory SubCategorymaster(int id)
        {
            try
            {
                return _context.subCategories.Find(id);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<Item> CustomerWideItem(int CustId)
        {
            try
            {
                var query = from item in _context.Items
                            where item.Customers.Any(x => x.Id == CustId) && item.IsDeleted == false
                            select item;
                return query.ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }

        }

        public List<int> GetItemsByCustomer(int CustId)
        {
            try
            {
                var query = from item in _context.Items
                            where item.Customers.Any(x => x.Id == CustId) && item.IsDeleted == false
                            select item.Id;
                return query.ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<DropDown> CustomerWideItem(int CustId, string CategoryName)
        {
            try
            {

                return _context.Database.SqlQuery<DropDown>("exec USP_GetItemByCategory @CustId,@CategoryName",
                  new SqlParameter("@CustId", CustId),
                  new SqlParameter("@CategoryName", CategoryName)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<Item> GetItemsForCustomer(int custId)
        {
            try
            {
                _context.Configuration.ProxyCreationEnabled = false;
                var query = from item in _context.Items
                            where item.Customers.Any(x => x.Id == custId) && item.IsDeleted == false
                            select item;
                return query.ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<ItemView> GetSkuForUserCustomer(int userid, int custid, string OrderReport, int StoreId = 0)
        {
            try
            {
                return _context.Database.SqlQuery<ItemView>(
                    string.Format("exec USP_GetWareHouseStoreForCustomer @UserId={0}, @CustId={1}, @CallType={2},@OrderReport='{3}', @StoreId={4}", userid, custid, 3, OrderReport, StoreId)).ToList();
                //return _context.Database.SqlQuery<ItemView>(
                //    string.Format("exec USP_GetWareHouseStoreForCustomer @UserId={0}, @CustId={1}, @CallType={2},@StoreId={3}", userid, custid, 3, StoreId)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return null;
            }
        }
        //public List<OrderSave> GetItemsForProduct(OrderSave product)
        //{
        //    try
        //    {
        //        return _context.Database.SqlQuery<OrderSave>("exec USP_GetItemsForProduct @ProductId, @Qty, @StoreId",
        //           new SqlParameter("@ProductId", product.ProductId),
        //           new SqlParameter("@Qty", product.Qty),
        //           new SqlParameter("@StoreId", product.storeId)
        //           ).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public List<ItemCategoryView> GetItemCategoriesByCustomer(int userid, int custid, int? StoreID = 0)
        {
            try
            {
                return _context.Database.SqlQuery<ItemCategoryView>(
                    string.Format("exec USP_GetWareHouseStoreForCustomer @UserId={0}, @CustId={1}, @CallType={2}, @StoreId={3}",
                    userid, custid, 4, StoreID.HasValue ? StoreID : 0)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<ItemView> GetSkuForUserCustomerItemCategory(int userId, int custId, List<int> SelectedTypes, List<int> selectedCategory, string OrderReport, int StoreId = 0)
        {
            try
            {
                return _context.Database.SqlQuery<ItemView>(
                    string.Format("exec USP_GetWareHouseStoreForCustomer @UserId={0}, @CustId={1}, @CallType={2},@Categories='{3}', @StoreId={4}," +
                    "@ItemTypes='{5}', @OrderReport='{6}'", userId, custId, 5, string.Join(",", selectedCategory.ToArray()), StoreId, string.Join(",", SelectedTypes.ToArray()), OrderReport)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<CategoryTypeView> GetItemTypeByCustomer(int userId, int custId, int? StoreID = 0)
        {
            try
            {
                return _context.Database.SqlQuery<CategoryTypeView>(
                    string.Format("exec USP_GetWareHouseStoreForCustomer @UserId={0}, @CustId={1}, @CallType={2}, @StoreId={3}", userId, custId, 8, StoreID.HasValue ? StoreID : 0)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<ItemCategoryView> GetItemCategoriesByCustomerForItemTypes(int userId, int custId, List<int> selectedItemTypes, int? StoreId = 0)
        {
            try
            {
                return _context.Database.SqlQuery<ItemCategoryView>(
                    string.Format("exec USP_GetWareHouseStoreForCustomer @UserId={0}, @CustId={1}, @CallType={2}, @ItemTypes='{3}', @StoreId={4}", userId, custId, 9, string.Join(",", selectedItemTypes.ToArray()), StoreId.HasValue ? StoreId : 0)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<ItemDetailView> GetItems(int userid, int custid, int storeId, List<int> selectedTypes, List<int> selectedItemCategory, List<int> selectedBrand)
        {
            try
            {
                return _context.Database.SqlQuery<ItemDetailView>(
                    string.Format("exec USP_GetWareHouseStoreForCustomer @UserId={0}, @CustId={1}, @CallType={2}, @StoreId={3}, @ItemTypes='{4}',@Categories='{5}', @BrandId='{6}'",
                    userid, custid, 11, storeId,
                    string.Join(",", selectedTypes.ToArray()),
                    string.Join(",", selectedItemCategory.ToArray()),
                    string.Join(",", selectedBrand.ToArray()))).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<ItemDetailView> GetTopItems(int userid, int custid, int storeId, List<int> selectedTypes, List<int> selectedItemCategory, List<int> SelectedTopItems)
        {
            try
            {
                return _context.Database.SqlQuery<ItemDetailView>(
                    string.Format("exec USP_GetWareHouseStoreForCustomer @UserId={0}, @CustId={1}, @CallType={2}, @StoreId={3}, @ItemTypes='{4}',@Categories='{5}', @SelectedTopItems='{6}'",
                    userid, custid, 12, storeId,
                    string.Join(",", selectedTypes.ToArray()),
                    string.Join(",", selectedItemCategory.ToArray()),
                    string.Join(",", SelectedTopItems.ToArray()))).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<ItemSubTypeByCustomer> GetItemSubTypeByCustomer(int userId, int custId)
        {
            try
            {
                return _context.Database.SqlQuery<ItemSubTypeByCustomer>(
                    string.Format("exec [GetItemSubTypeByCustomerId] @UserId={0}, @CustId={1}", userId, custId)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        #region brand
        public List<Brand> GetBrands(string Opr)
        {
            try
            {
                return _context.Database.SqlQuery<Brand>(
                    string.Format("exec USP_GetBrands @OPR='{0}'", Opr)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public int AddBrand(Brand brand)
        {
            try
            {
                return _context.Database.SqlQuery<int>("exec USP_AddUpdateBrand @Opr,@BrandName,@CreatedBy,@Id, @ModifiedBy, @IsDeleted",
                  new SqlParameter("@Opr", "ADD-BRAND"),
                  new SqlParameter("@BrandName", brand.BrandName),
                  new SqlParameter("@CreatedBy", brand.CreatedBy),
                  new SqlParameter("@Id", brand.Id),
                  new SqlParameter("@ModifiedBy", brand.ModifiedBy == null ? 0 : brand.ModifiedBy),
                  new SqlParameter("@IsDeleted", brand.IsDeleted)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public int UpdateBrand(Brand brand)
        {
            try
            {
                return _context.Database.SqlQuery<int>("exec USP_AddUpdateBrand @Opr,@BrandName,@CreatedBy,@Id, @ModifiedBy, @IsDeleted",
                  new SqlParameter("@Opr", "EDIT-BRAND"),
                  new SqlParameter("@BrandName", brand.BrandName),
                  new SqlParameter("@CreatedBy", brand.CreatedBy),
                  new SqlParameter("@Id", brand.Id),
                  new SqlParameter("@ModifiedBy", brand.ModifiedBy),
                  new SqlParameter("@IsDeleted", brand.IsDeleted)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public Brand GetBrandById(int brandId)
        {
            try
            {
                return _context.Database.SqlQuery<Brand>(
                    string.Format("exec USP_GetBrands @OPR='ID', @Id= {0}", brandId)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public int DeleteBrand(int id, int userid, bool isDelete)
        {
            try
            {
                return _context.Database.SqlQuery<int>("exec USP_AddUpdateBrand @Opr,@BrandName,@CreatedBy,@Id, @ModifiedBy, @IsDeleted",
                  new SqlParameter("@Opr", "Brand-Delete"),
                  new SqlParameter("@BrandName", ""),
                  new SqlParameter("@CreatedBy", userid),
                  new SqlParameter("@Id", id),
                  new SqlParameter("@ModifiedBy", userid),
                  new SqlParameter("@IsDeleted", isDelete)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<Brand> GetBrandsForUserCustomer(int userid, int custid, string OrderReport, int? StoreId = 0)
        {
            try
            {
                return _context.Database.SqlQuery<Brand>(
                    string.Format("exec USP_GetWareHouseStoreForCustomer @UserId={0}, @CustId={1}, @CallType={2},@OrderReport='{3}', @StoreId={4}",
                    userid, custid, 14, OrderReport, StoreId != null ? StoreId : 0)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return null;
            }
        }
        #endregion

        public List<StoreItemView> GetItemForCustomerStoreActivation(int custId, List<int> dcIds, List<int> itemIds)
        {
            try
            {
                return _context.Database.SqlQuery<StoreItemView>(
                    string.Format("exec USP_GetItemForCustomerStoreActivation @Opr='{0}', @CustId={1}, @DcId='{2}', @ItemIds='{3}'", "DC", custId, string.Join(",", dcIds.ToArray()), string.Join(",", itemIds.ToArray()))).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<CategoryType> ItemCategoryTypes(string opr)
        {
            if (opr == "All")
            {
                return _context.ItemType.ToList();
            }
            else if (opr == "Active")
            {
                return _context.ItemType.Where(x => x.isDeleted == false).ToList();
            }
            else
            {
                return null;
            }
        }

        public bool UpdateDcStoreItemMapping(StoreItemView data, string opr, int userId)
        {
            try
            {
                return _context.Database.SqlQuery<bool>(
                    string.Format("exec USP_UpdateDcStoreItemMapping @Opr='{0}', @CustId={1}, @ItemId={2}, @DcId={3}, @StoreId={4}, @IsDeleted={5}, @UserId={6}", opr, data.CustId, data.ItemId, data.DcId, data.StoreId, data.Status, userId)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public bool UpdateAllDcStoreItemMapping(int custId, string opr, List<int> items, int isDeleted, List<int> stores, List<int> dcs, int userId)
        {
            try
            {
                return _context.Database.SqlQuery<bool>(
                    string.Format("exec USP_UpdateAllDcStoreItemMapping @Opr='{0}', @CustId={1}, @ItemIds='{2}', @DcIds='{3}', @StoreIds='{4}', @IsDeleted={5}, @UserId={6}",
                    opr, custId, string.Join(",", items.ToArray()),
                    dcs != null ? string.Join(",", dcs.ToArray()) : "",
                    stores != null ? string.Join(",", stores.ToArray()) : "",
                    isDeleted, userId)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<StoreItemView> GetItemsForStoreActivation(string opr, int custId, List<int> storeIds, List<int> itemIds)
        {
            try
            {
                return _context.Database.SqlQuery<StoreItemView>(
                   string.Format("exec USP_GetItemForCustomerStoreActivation @Opr='{0}', @CustId={1}, @storeIds='{2}', @ItemIds='{3}'",
                   opr, custId, string.Join(",", storeIds.ToArray()), string.Join(",", itemIds.ToArray()))).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

    }
}
