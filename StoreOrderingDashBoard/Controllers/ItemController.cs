using BAL;
using DAL;
using DbLayer.Service;
using StoreOrderingDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace StoreOrderingDashBoard.Controllers
{
    public class ItemController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IItemService _itemService;
        //private readonly IOrderService _orderService;
        //private readonly ILocationService _locationService;
        private readonly IAuditService _auditService;
        private readonly IWareHouseService _wareHouseService;
        private readonly IStoreService _storeService;
        public ItemController(ICustomerService customerService, IItemService itemService, IAuditService auditService, IWareHouseService wareHouseService, IStoreService storeService)
        {
            _customerService = customerService;
            _itemService = itemService;
            _auditService = auditService;
            _wareHouseService = wareHouseService;
            _storeService = storeService;
        }

        // GET: Item
        [AdminAuthorization]
        public ActionResult Index()
        {
            //var item = _itemService.GetAllItems();
            //ViewBag.Items = item;
            return View();
        }

        [AdminApiAuthorization]
        [HttpPost]
        public JsonResult GetAllItems(string Items)
        {
            var ItemList = _itemService.GetAllItems("All");
            return Json(new { data = ItemList }, JsonRequestBehavior.AllowGet);
        }
        [AdminApiAuthorization]
        [HttpPost]
        public JsonResult DeleteItem(int Id, bool IsDeleted)
        {
            bool cond = _itemService.DeleteItem(Id, IsDeleted) > 0 ? true : false;
            try
            {
                var storeJavascript = new JavaScriptSerializer();
                var storeser = storeJavascript.Serialize("Item Id: " + Id + ", IsDeleted:" + cond);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), storeser, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
            return Json(cond, JsonRequestBehavior.AllowGet);
        }


        [AdminAuthorization]
        public ActionResult AddItem()
        {
            var CustomerList = _customerService.GetAllCustomers();
            var UOMList = _itemService.UOMmasters();
            var Categories = _itemService.Categories();
            var Subcategories = _itemService.SubCategories();
            var Brands = _itemService.GetBrands("Active");
            var ItemCategoryTypes = _itemService.ItemCategoryTypes("Active");
            Item item = new Item();
            item.UOMmaster = new UOMmaster();
            item.Category = new Category();
            item.SubCategory = new SubCategory();
            ViewBag.CustomerList = CustomerList;
            ViewBag.UOMList = UOMList;
            ViewBag.ItemCategoryType = ItemCategoryTypes;
            ViewBag.Categories = Categories;
            ViewBag.SubCategories = Subcategories;
            ViewBag.Brands = Brands;
            ViewBag.PageName = "Add Item";
            return View(item);
        }

        [AdminAuthorization]
        [HttpPost]
        public ActionResult AddItem(Item item)
        {
            if (ModelState.IsValid)
            {
                var CustomerList = _customerService.GetAllCustomers();
                ViewBag.CustomerList = CustomerList;
                var SKUItemCheck = _itemService.GetAllItems("All").Where(x => x.SKUCode == item.SKUCode);
                if (SKUItemCheck.Count() <= 0)
                {
                    if (item.Id > 0)
                    {
                        item.ModifiedBy = SessionValues.UserId;
                        int affectedrow = _itemService.UpdateItems(item);
                        TempData["Messege"] = "Item Updated.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        item.CreatedBy = SessionValues.UserId;
                        item.ModifiedBy = SessionValues.UserId;
                        int affectedrow = _itemService.AddItem(item);
                        TempData["Messege"] = "Item Added.";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("SkuCode", "Item with same SkuCode already exists.");
                }
            }
            var CustomerLists = _customerService.GetAllCustomers();
            var UOMList = _itemService.UOMmasters();
            var ItemCategoryTypes = _itemService.ItemCategoryTypes("Active");
            var Categories = _itemService.Categories();
            var Subcategories = _itemService.SubCategories();
            var Brands = _itemService.GetBrands("Active");
            ViewBag.CustomerList = CustomerLists;
            ViewBag.UOMList = UOMList;
            ViewBag.ItemCategoryType = ItemCategoryTypes;
            ViewBag.Categories = Categories;
            ViewBag.SubCategories = Subcategories;
            ViewBag.Brands = Brands;
            if (item.Id != 0)
            {
                ViewBag.PageName = "Edit Item";
            }
            else
            {
                ViewBag.PageName = "Add Item";
            }
            return View(item);
        }

        [AdminAuthorization]
        public ActionResult EditItem(int Id)
        {
            var item = _itemService.GetItemById(Id);
            if (item == null)
            {
                TempData["ErrorMsg"] = "Item @Id=" + Id + " is not accessible please select valid record.";
                return RedirectToAction("Index", "Item");
            }
            item.SelectedCustomers = item.Customers != null ? item.Customers.Select(x => x.Id) : null;
            var CustomerList = _customerService.GetAllCustomers();
            var UOMList = _itemService.UOMmasters();
            var ItemCategoryTypes = _itemService.ItemCategoryTypes("Active");
            var Categories = _itemService.Categories();
            var Subcategories = _itemService.SubCategories();
            var Brands = _itemService.GetBrands("Active");
            ViewBag.CustomerList = CustomerList;
            ViewBag.UOMList = UOMList;
            ViewBag.ItemCategoryType = ItemCategoryTypes;
            ViewBag.Categories = Categories;
            ViewBag.SubCategories = Subcategories;
            ViewBag.Brands = Brands;
            ViewBag.PageName = "Edit Item";
            return View("AddItem", item);
        }
        [HttpGet]
        public ActionResult EditItemMapping(int CustId, int? ItemId = null, int? DcId = null)
        {
            var CustomerList = _customerService.GetAllCustomers();
            var warehouse = _wareHouseService.GetWareHouseForCustomerId(SessionValues.UserId, CustId, "Order");
            var Items = _itemService.GetSkuForUserCustomer(SessionValues.UserId, CustId, "Order");
            List<int> ItemIds = null;
            List<int> CustIds = null;
            List<int> DcIds = null;
            ItemIds = ItemId.HasValue ? new List<int> { ItemId.Value } : null;
            CustIds = new List<int> { CustId };
            DcIds = DcId.HasValue ? new List<int> { DcId.Value } : null;
            ViewBag.Customers = new SelectList(CustomerList.Where(x => x.IsDeleted == false), "Id", "Name", CustId);
            ViewBag.DCs = new MultiSelectList(warehouse.ToList(), "Id", "Name", DcIds);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", ItemIds);
            return View();
        }
        [HttpPost]
        public ActionResult EditItemMapping(List<int> ItemIds, int CustId, List<int> DcIds)
        {
            var CustomerList = _customerService.GetAllCustomers();
            var warehouse = _wareHouseService.GetWareHouseForCustomerId(SessionValues.UserId, CustId, "Order");
            var Items = _itemService.GetSkuForUserCustomer(SessionValues.UserId, CustId, "Order");
            ViewBag.Customers = new SelectList(CustomerList.Where(x => x.IsDeleted == false), "Id", "Name", CustId);
            ViewBag.DCs = new MultiSelectList(warehouse.ToList(), "Id", "Name", DcIds);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", ItemIds);
            return View();
        }
        [AdminApiAuthorization]
        [HttpPost]
        public ActionResult GetItemForCustomerStoreActivation(List<int> ItemIds, int CustId, List<int> DcIds)
        {
            var warehouse = _wareHouseService.GetWareHouseForCustomerId(SessionValues.UserId, CustId, "Order");
            var Items = _itemService.GetSkuForUserCustomer(SessionValues.UserId, CustId, "Order");
            ItemIds = ItemIds == null ? Items.Select(x => x.Id).ToList() : ItemIds;
            ItemIds = ItemIds != null && ItemIds.Count() > 0 && ItemIds[0] == 0 ? Items.Select(x => x.Id).ToList() : ItemIds;

            DcIds = DcIds == null ? warehouse.Select(x => x.Id).ToList() : DcIds;
            DcIds = DcIds != null && DcIds.Count() > 0 && DcIds[0] == 0 ? warehouse.Select(x => x.Id).ToList() : DcIds;
            List<StoreItemView> Itemsdata = _itemService.GetItemForCustomerStoreActivation(CustId, DcIds, ItemIds);
            JsonResult result = Json(new { data = Itemsdata }, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
            return result;
        }
        [AdminApiAuthorization]
        [HttpPost]
        public JsonResult UpdateDcStoreItemMapping(StoreItemView Data, string Opr)
        {
            bool data = _itemService.UpdateDcStoreItemMapping(Data, Opr, SessionValues.UserId);
            JsonResult result = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
            return result;
        }
        [AdminApiAuthorization]
        [HttpPost]
        public JsonResult UpdateAllDcStoreItemMapping(int CustId, List<int> Dcs, List<int> Items, List<int> Stores, string Opr, int IsDeleted)
        {
            var Allwarehouse = _wareHouseService.GetWareHouseForCustomerId(SessionValues.UserId, CustId, "Order");
            var AllItems = _itemService.GetSkuForUserCustomer(SessionValues.UserId, CustId, "Order");

            Items = Items == null ? AllItems.Select(x => x.Id).ToList() : Items;
            Items = Items != null && Items.Count() > 0 && Items[0] == 0 ? AllItems.Select(x => x.Id).ToList() : Items;

            Dcs = Dcs == null ? Allwarehouse.Select(x => x.Id).ToList() : Dcs;
            Dcs = Dcs != null && Dcs.Count() > 0 && Dcs[0] == 0 ? Allwarehouse.Select(x => x.Id).ToList() : Dcs;

            bool data = _itemService.UpdateAllDcStoreItemMapping(CustId, Opr, Items, IsDeleted, Stores, Dcs, SessionValues.UserId);
            JsonResult result = Json(new { data = data }, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
            return result;
        }
        public ActionResult EditItemStoreMapping(int CustId, int StoreId = 0, int DcId = 0, int ItemId = 0, string MappingType="DC")
        {
            List<Store> stores = _storeService.GetStoresByCustomerId(CustId) ;
            List<Item> items=_itemService.GetItemsForCustomer(CustId);
            List<int> SelectedStores=null;
            List<int> SelectedItems=null;
            if (StoreId != 0)
            {
                SelectedStores = stores.Where(x => x.Id == StoreId).Select(y=>y.Id).ToList();
            }
            else if (DcId != 0)
            {
                SelectedStores = stores.Where(x => x.WareHouseDC.Id== DcId).Select(y => y.Id).ToList();
            }
            if (ItemId != 0)
            {
                SelectedItems= items.Where(x => x.Id == ItemId).Select(y => y.Id).ToList();
            }
            ViewBag.Stores = new MultiSelectList(stores.ToList(), "Id", "StoreName", SelectedStores);
            ViewBag.Items = new MultiSelectList(items.ToList(), "Id", "SKUCode", SelectedItems);
            ViewBag.CustId = CustId;
            ViewBag.MappingType = MappingType;
            return View();
        }
        [HttpPost]
        public ActionResult GetItemsForStoreActivation(List<int> ItemIds, int CustId, List<int> StoreIds)
        {
            //var warehouse = _wareHouseService.GetWareHouseForCustomerId(SessionValues.UserId, CustId, "Order");
            var Items = _itemService.GetSkuForUserCustomer(SessionValues.UserId, CustId, "Order");
            ItemIds = ItemIds == null ? Items.Select(x => x.Id).ToList() : ItemIds;
            ItemIds = ItemIds != null && ItemIds.Count() > 0 && ItemIds[0] == 0 ? Items.Select(x => x.Id).ToList() : ItemIds;
            
            List<StoreItemView> Itemsdata = _itemService.GetItemsForStoreActivation("Store",CustId, StoreIds, ItemIds);
            JsonResult result = Json(new { data = Itemsdata }, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
            return result;
        }
    }
}