using BAL;
using DAL;
using DbLayer.Service;
using OfficeOpenXml;
using StoreOrderingDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace StoreOrderingDashBoard.Controllers
{
    public class OrderController : Controller
    {
        private readonly IItemService _itemService;
        private readonly IOrderService _orderService;
        private readonly IStoreService _storeService;
        private readonly IAuditService _auditService;
        private readonly IReportService _reportService;
        private readonly ICustomerService _customerService;
        private readonly IWareHouseService _wareHouseService;

        public OrderController(IItemService itemService, IOrderService orderService, IStoreService storeService, IAuditService auditService, IReportService reportService, ICustomerService customerService, IWareHouseService wareHouseService)
        {
            _itemService = itemService;
            _orderService = orderService;
            _storeService = storeService;
            _auditService = auditService;
            _reportService = reportService;
            _customerService = customerService;
            _wareHouseService = wareHouseService;
        }
        // GET: Order

        #region StoreOrder
        [CustomAuthorization]
        public ActionResult OrderNew()
        {
            int StoreID = SessionValues.StoreId;
            int userid = SessionValues.UserId;
            int custid = SessionValues.LoggedInCustId.Value;
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid, StoreID);
            List<Brand> Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Order", StoreID);
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid, StoreID);
            var store = _storeService.GetStoreById(StoreID);
            //List<StoreOrderVB> draftOrders = _orderService.GetStoreDraftOrder(StoreID, store.CustId, userid);
            //var ItemList = _itemService.GetItems(userid, custid, SessionValues.StoreId, ItemType.Select(x => x.Id).ToList(), Itemcategories.Select(x => x.Id).ToList(), Brands.Select(x=>x.Id).ToList());
            //if (draftOrders != null)
            //    ItemList.RemoveAll(x => draftOrders.Select(y => y.ItemId).Contains(x.Id));
            //ViewBag.List = ItemList;
            List<Master> ToplistItems = new List<Master>
            {
                new Master
                {
                    Text = "Top 10 Items",
                    Id = 1
                }
            };
            ViewBag.ToplistItems = new MultiSelectList(ToplistItems.ToList(), "Id", "Text", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", null);
            return View();
        }

        [HttpPost]
        [CustomAuthorization]
        public ActionResult OrderNew(List<int> SelectedTypes, List<int> SelectedCategories, string Errors, List<int> SelectedTopItems, List<int> SelectedBrand)
        {
            int StoreID = SessionValues.StoreId;
            var store = _storeService.GetStoreById(StoreID);
            int userid = SessionValues.UserId;
            int custid = SessionValues.LoggedInCustId.Value;
            var Req = Request;
            //ItemType
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid, StoreID);
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid, StoreID);
            List<Brand> Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Order", StoreID);
            if (SelectedTypes != null && SelectedTypes.Count() > 0 && SelectedTypes[0] != 0)
            {
                Itemcategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes, StoreID);
            }
            else
            {
                SelectedTypes = ItemType.Select(X => X.Id).ToList();
            }
            if (SelectedCategories == null || SelectedCategories.Count() <= 0 && SelectedCategories[0] == 0)
            {
                SelectedCategories = Itemcategories.Select(x => x.Id).ToList();
            }
            SelectedBrand = SelectedBrand == null ? Brands.Select(x => x.Id).ToList() : SelectedBrand;
            SelectedBrand = SelectedBrand != null && SelectedBrand.Count() > 0 && SelectedBrand[0] == 0 ? Brands.Select(x => x.Id).ToList() : SelectedBrand;
            try
            {
                if (Errors != null && Errors != "")
                {
                    var JavaScriptSerializer = new JavaScriptSerializer();
                    List<OrderSaveResponse> ErrorList = JavaScriptSerializer.Deserialize<List<OrderSaveResponse>>(Errors);
                    foreach (OrderSaveResponse response in ErrorList)
                    {
                        ModelState.AddModelError("", response.Message);
                    }
                    ViewBag.ErrorCount = ErrorList.Count();
                }
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
            List<StoreOrderVB> draftOrders = _orderService.GetStoreDraftOrder(StoreID, store.CustId, userid);
            var ItemList = _itemService.GetItems(userid, custid, StoreID, SelectedTypes, SelectedCategories, SelectedBrand);
            if (draftOrders != null)
            {
                ItemList.RemoveAll(x => draftOrders.Select(y => y.ItemId).Contains(x.Id));
            }

            ViewBag.List = ItemList;
            List<Master> ToplistItems = new List<Master>
            {
                new Master
                {
                    Text = "Top 10 Items",
                    Id = 1
                }
            };
            ViewBag.ToplistItems = new MultiSelectList(ToplistItems.ToList(), "Id", "Text", SelectedTopItems);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", SelectedBrand);
            return View();
        }

        public ActionResult GetItemsForOrder(List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedTopItems, List<int> SelectedBrand)
        {
            int StoreID = SessionValues.StoreId;
            var store = _storeService.GetStoreById(StoreID);
            int userid = SessionValues.UserId;
            int custid = SessionValues.LoggedInCustId.Value;
            var Req = Request;
            List<ItemDetailView> TopItemList = null;
            //ItemType
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid, StoreID);
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid, StoreID);
            List<Brand> Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Order", StoreID);
            if (SelectedTypes != null && SelectedTypes.Count() > 0 && SelectedTypes[0] != 0)
            {
                Itemcategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes, StoreID);
            }
            else
            {
                SelectedTypes = ItemType.Select(X => X.Id).ToList();
            }
            if (SelectedCategories == null || SelectedCategories.Count() <= 0 || SelectedCategories[0] == 0)
            {
                SelectedCategories = Itemcategories.Select(x => x.Id).ToList();
            }
            if (SelectedTopItems != null && SelectedTopItems.Count() > 0 && SelectedTopItems[0] != 0)
            {
                TopItemList = _itemService.GetTopItems(userid, custid, StoreID, SelectedTypes, SelectedCategories, SelectedTopItems);
            }
            SelectedBrand = SelectedBrand == null ? Brands.Select(x => x.Id).ToList() : SelectedBrand;
            SelectedBrand = SelectedBrand != null && SelectedBrand.Count() > 0 && SelectedBrand[0] == 0 ? Brands.Select(x => x.Id).ToList() : SelectedBrand;

            List<StoreOrderVB> draftOrders = _orderService.GetStoreDraftOrder(StoreID, store.CustId, userid);

            var ItemList = _itemService.GetItems(userid, custid, StoreID, SelectedTypes, SelectedCategories, SelectedBrand);
            if (TopItemList != null)
            {
                ItemList = ItemList.Where(x => TopItemList.Select(y => y.Id).Contains(x.Id)).ToList();
            }
            if (draftOrders != null)
                ItemList.RemoveAll(x => draftOrders.Select(y => y.ItemId).Contains(x.Id));
            return Json(new { data = ItemList }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [CustomApiAuthorization]
        public JsonResult SaveOrFinaliseOrder(List<OrderSave> order, string issave)
        {
            int StoreID = SessionValues.StoreId;
            int userid = SessionValues.UserId;
            var store = _storeService.GetStoreById(StoreID);
            DateTime StoreOrderDate = DateTime.Now;
            List<OrderSaveResponse> responses = new List<OrderSaveResponse>();
            foreach (OrderSave save in order)
            {
                OrderExcel orderExcel = new OrderExcel();
                orderExcel.Isdeleted = 0;
                orderExcel.CustId = store.CustId;
                orderExcel.Upload_By = userid;
                orderExcel.Store_Order_Date = StoreOrderDate;
                orderExcel.Store_Code = store.StoreCode;
                orderExcel.Item_Code = save.ItemCode;
                orderExcel.Item_Desc = save.ItemDesc;
                orderExcel.Ordered_Qty = save.Qty;
                orderExcel.Upload_Date = DateTime.Now;
                orderExcel.Validated_Flag = 1;
                orderExcel.ItemId = save.ProductId;
                OrderSaveResponse pk_OrderSave = _orderService.SaveStoreOrderToOrderExcel(orderExcel, issave);
                pk_OrderSave.Type = issave;
                responses.Add(pk_OrderSave);
                try
                {
                    var auditJavascript = new JavaScriptSerializer();
                    var OrderAudit = auditJavascript.Serialize(save);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), OrderAudit, Request.Url.OriginalString.ToString(), null, SessionValues.StoreId);
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    throw ex;
                }
            }

            //bool cond = (1 > 0 ? true : false);
            return Json(responses, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StoreDraftOrder()
        {
            int StoreID = Convert.ToInt32(SessionValues.StoreId);
            int userid = Convert.ToInt32(SessionValues.UserId);
            var store = _storeService.GetStoreById(StoreID);
            List<StoreOrderVB> draftOrders = _orderService.GetStoreDraftOrder(StoreID, store.CustId, userid);
            ViewBag.Orderdetaillist = draftOrders;
            return View();
        }

        [CustomApiAuthorization]
        public ActionResult UpdateDraftQuantity(int ItemId, int Quantity)
        {
            int StoreID = SessionValues.StoreId;
            int userid = SessionValues.UserId;
            var store = _storeService.GetStoreById(StoreID);
            bool cond = _orderService.UpdateDraftQuantity(StoreID, ItemId, Quantity, "Update", userid) > 0 ? true : false;

            return Json(cond, JsonRequestBehavior.AllowGet);
        }
        [CustomApiAuthorization]
        public ActionResult DeleteDraftItem(int ItemId)
        {
            int StoreID = SessionValues.StoreId;
            int userid = SessionValues.UserId;
            var store = _storeService.GetStoreById(StoreID);
            bool cond = _orderService.UpdateDraftQuantity(StoreID, ItemId, 0, "Delete", userid) > 0 ? true : false;
            return Json(cond, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorization]
        public ActionResult ItemlistPartial()
        {
            int StoreID = Convert.ToInt32(SessionValues.StoreId);
            int userid = Convert.ToInt32(SessionValues.UserId);
            var store = _storeService.GetStoreById(StoreID);
            int custid = 0;
            if (SessionValues.LoggedInCustId != null)
            {
                custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            }
            else if (SessionValues.StoreId != 0)
            {
                int storeid = SessionValues.StoreId;
                custid = _storeService.GetStoreById(storeid).CustId;

            }
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid, StoreID);
            //ItemType
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid, StoreID);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);

            return PartialView("_ItemlistPartial");
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GetItemslistPartial(List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrand)
        {
            int StoreID = SessionValues.StoreId;
            int userid = SessionValues.UserId;
            var store = _storeService.GetStoreById(StoreID);
            int custid = 0;

            custid = Convert.ToInt32(SessionValues.LoggedInCustId);

            int storeid = SessionValues.StoreId;
            custid = store.CustId;


            List<StoreOrderVB> draftOrders = _orderService.GetStoreDraftOrder(StoreID, store.CustId, userid);
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid, StoreID);
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid, StoreID);
            List<Brand> Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Order", StoreID);
            if (SelectedTypes != null && SelectedTypes.Count() > 0 && SelectedTypes[0] != 0)
            {
                Itemcategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes, StoreID);
            }
            else
            {
                SelectedTypes = ItemType.Select(X => X.Id).ToList();
            }
            if (SelectedCategories != null && SelectedCategories.Count() > 0 && SelectedCategories[0] == 0)
            {
                SelectedCategories = Itemcategories.Select(x => x.Id).ToList();
            }
            SelectedBrand = SelectedBrand == null ? Brands.Select(x => x.Id).ToList() : SelectedBrand;
            SelectedBrand = SelectedBrand != null && SelectedBrand.Count() > 0 && SelectedBrand[0] == 0 ? Brands.Select(x => x.Id).ToList() : SelectedBrand;

            var itemlist = _itemService.GetItems(userid, custid, StoreID, SelectedTypes, SelectedCategories, SelectedBrand);
            itemlist.RemoveAll(x => draftOrders.Select(y => y.ItemId).Contains(x.Id));
            return Json(new { data = itemlist }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomApiAuthorization]
        public JsonResult AddItem(List<OrderSave> order)
        {
            long pk_orderDetail = 0;
            foreach (OrderSave save in order)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.Quantity = Convert.ToInt32(save.Qty);
                orderDetail.ItemId = save.ProductId;
                orderDetail.OrderHeaderId = Convert.ToInt64(save.OrderheaderId);
                orderDetail.StoreId = SessionValues.StoreId;
                orderDetail.CreatedDate = DateTime.Now;
                pk_orderDetail = _orderService.SaveOrderDetail(orderDetail);
                try
                {
                    var OrderDetailAudit = new OrderDetail(orderDetail.Id, orderDetail.Quantity, orderDetail.ItemId, orderDetail.OrderHeaderId, null);
                    var auditJavascript = new JavaScriptSerializer();
                    var orderDetailAudit = auditJavascript.Serialize(OrderDetailAudit);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), orderDetailAudit, Request.Url.OriginalString.ToString(), null, SessionValues.StoreId);
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    throw ex;
                }
            }
            bool cond = pk_orderDetail > 0 ? true : false;
            return Json(cond, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomApiAuthorization]
        public JsonResult FinilizeDraft(List<OrderSave> order, string issave)
        {
            int StoreId = SessionValues.StoreId;
            int userId = SessionValues.UserId;
            var store = _storeService.GetStoreById(StoreId);
            DateTime StoreOrderDate = DateTime.Now;
            List<OrderSaveResponse> responses = new List<OrderSaveResponse>();
            long vGroupId = _orderService.GetMaxGroupIDByCustomer(store.CustId);
            foreach (OrderSave save in order)
            {
                OrderExcel orderExcel = new OrderExcel();
                orderExcel.Isdeleted = 0;
                orderExcel.CustId = store.CustId;
                orderExcel.Upload_By = userId;
                orderExcel.Store_Order_Date = StoreOrderDate;
                orderExcel.Store_Code = store.StoreCode;
                orderExcel.Item_Code = save.ItemCode;
                orderExcel.Item_Desc = save.ItemDesc;
                orderExcel.Ordered_Qty = save.Qty;
                orderExcel.Upload_Date = DateTime.Now;
                orderExcel.Validated_Flag = 1;
                orderExcel.ItemId = save.ProductId;
                orderExcel.PONumber = save.PONumber;
                orderExcel.GroupId = vGroupId;
                OrderSaveResponse pk_OrderSave = _orderService.FinilizeDraft(orderExcel, issave);
                pk_OrderSave.Type = issave;
                responses.Add(pk_OrderSave);
                try
                {
                    var auditJavascript = new JavaScriptSerializer();
                    var OrderAudit = auditJavascript.Serialize(save);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), OrderAudit, Request.Url.OriginalString.ToString(), null, SessionValues.StoreId);
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    throw ex;
                }
            }
            //bool cond = (1 > 0 ? true : false);
            return Json(responses, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteDraft(long UserId)
        {
            int StoreId = SessionValues.StoreId;
            int userId = SessionValues.UserId;
            var store = _storeService.GetStoreById(StoreId);
            DateTime StoreOrderDate = DateTime.Now;
            long affectedrow = _orderService.DeleteStoreDraft(userId, StoreId);
            bool cond = affectedrow > 0 ? true : false;
            return Json(cond, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [CustomAuthorization]
        public ActionResult SaveAsDraftList()
        {
            int storid = SessionValues.StoreId;
            var savedraft = _orderService.GetSaveAsDraftOrders(storid);
            ViewBag.SaveList = savedraft;
            return View();
        }

        [CustomAuthorization]
        public ActionResult OrderDetailList(long orderheaderid)
        {
            var OrderDetaillist = _orderService.GetOrderDetails(orderheaderid);
            ViewBag.Orderdetaillist = OrderDetaillist;
            return View();
        }

        [HttpPost]
        [CustomApiAuthorization]
        public JsonResult Finilize(List<OrderSave> order)
        {
            long pk_headerId = order.First().OrderheaderId;
            var orderheader = _orderService.GetOrderHeader(pk_headerId);
            orderheader.IsOrderStatus = true;
            orderheader.Finilizeorderno = "rfpl/finalised/";
            orderheader.OrderDate = DateTime.Now;
            orderheader.ModifiedOn = DateTime.Now;
            long affectedrow = _orderService.UpdateOrderHeader(orderheader);
            try
            {
                var orderHeaderAudit = new OrderHeader(orderheader.Id, orderheader.IsProcessed, orderheader.IsOrderStatus, orderheader.DraftOrderno, orderheader.Finilizeorderno, orderheader.OrderDate, orderheader.ProcessedDate, orderheader.isOrderEmailSent, orderheader.StoreId, orderheader.ProcessedUserId, orderheader.DCID, orderheader.Isdeleted, orderheader.orderDetails, null, null, null);
                var auditJavascript = new JavaScriptSerializer();
                var OrderAudit = auditJavascript.Serialize(orderHeaderAudit);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), OrderAudit, Request.Url.OriginalString.ToString(), null, SessionValues.StoreId);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            foreach (OrderSave save in order)
            {
                var orderdtail = _orderService.GetOrderDetail(pk_headerId, save.ProductId);
                orderdtail.Quantity = Convert.ToInt32(save.Qty);
                long affectedrow1 = _orderService.UpdateOrderDetails(orderdtail);
                try
                {
                    var OrderDetailAudit = new OrderDetail(orderdtail.Id, orderdtail.Quantity, orderdtail.ItemId, orderdtail.OrderHeaderId, null);
                    var auditJavascript = new JavaScriptSerializer();
                    var orderDetailAudit = auditJavascript.Serialize(OrderDetailAudit);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), orderDetailAudit, Request.Url.OriginalString.ToString(), null, SessionValues.StoreId);
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    throw ex;
                }
            }
            bool cond = affectedrow > 0 ? true : false;
            if (cond)
            {
                int id = SessionValues.StoreId != 0 ? SessionValues.StoreId : 1;
                var store = _storeService.GetStoreById(id);
                HelperCls.SendEmail(store.StoreEmailId, "Order Finalize", HelperCls.PopulateBodyFinalizeorder(store.StoreName, pk_headerId.ToString()));
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomApiAuthorization]
        public JsonResult DeleteItem(long orderheaderid, int itemid)
        {
            var orderdetail = _orderService.GetOrderDetail(orderheaderid, itemid);
            int affectedrow = _orderService.deleteOrderDetail(orderdetail);
            bool cond = affectedrow > 0 ? true : false;
            try
            {
                var OrderDetailAudit = new OrderDetail(orderdetail.Id, orderdetail.Quantity, orderdetail.ItemId, orderdetail.OrderHeaderId, null);
                var auditJavascript = new JavaScriptSerializer();
                var orderDetailAudit = auditJavascript.Serialize(OrderDetailAudit);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), orderDetailAudit, Request.Url.OriginalString.ToString(), null, SessionValues.StoreId);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            return Json(cond, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomApiAuthorization]
        public JsonResult UpdateFinilize(long orderheaderid)
        {
            var orderheader = _orderService.GetOrderHeader(orderheaderid);
            orderheader.IsOrderStatus = true;
            orderheader.ModifiedOn = DateTime.Now;
            int affectedrow = _orderService.UpdateOrderHeader(orderheader);
            bool cond = affectedrow > 0 ? true : false;
            try
            {
                var orderHeaderAudit = new OrderHeader(orderheader.Id, orderheader.IsProcessed, orderheader.IsOrderStatus, orderheader.DraftOrderno, orderheader.Finilizeorderno, orderheader.OrderDate, orderheader.ProcessedDate, orderheader.isOrderEmailSent, orderheader.StoreId, orderheader.ProcessedUserId, orderheader.DCID, orderheader.Isdeleted, orderheader.orderDetails, null, null, null);
                var auditJavascript = new JavaScriptSerializer();
                var OrderAudit = auditJavascript.Serialize(orderHeaderAudit);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), OrderAudit, Request.Url.OriginalString.ToString(), null, SessionValues.StoreId);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            return Json(cond, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomApiAuthorization]
        public JsonResult DeleteFinilise(long orderheaderid)
        {
            var orderheader = _orderService.GetOrderHeader(orderheaderid);
            orderheader.Isdeleted = true;
            int affectedrow = _orderService.UpdateOrderHeader(orderheader);
            bool cond = affectedrow > 0 ? true : false;
            try
            {
                var orderHeaderAudit = new OrderHeader(orderheader.Id, orderheader.IsProcessed, orderheader.IsOrderStatus, orderheader.DraftOrderno, orderheader.Finilizeorderno, orderheader.OrderDate, orderheader.ProcessedDate, orderheader.isOrderEmailSent, orderheader.StoreId, orderheader.ProcessedUserId, orderheader.DCID, orderheader.Isdeleted, orderheader.orderDetails, null, null, null);
                var auditJavascript = new JavaScriptSerializer();
                var OrderAudit = auditJavascript.Serialize(orderHeaderAudit);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), OrderAudit, Request.Url.OriginalString.ToString(), null, SessionValues.StoreId);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            return Json(cond, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomApiAuthorization]
        public JsonResult UpdateQuantity(long OrderHeaderId, int ItemId, int Quantity)
        {
            var detail = _orderService.GetOrderDetail(OrderHeaderId, ItemId);
            detail.Quantity = Quantity;
            bool cond = _orderService.UpdateOrderDetails(detail) > 0 ? true : false;
            try
            {
                OrderDetail AuditOrderDetail = new OrderDetail(detail.Id, Quantity, ItemId, OrderHeaderId, null);
                var auditJavascript = new JavaScriptSerializer();
                var orderDetailAudit = auditJavascript.Serialize(AuditOrderDetail);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), orderDetailAudit, Request.Url.OriginalString.ToString(), null, SessionValues.StoreId);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            return Json(cond, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorization]
        public ActionResult SummeriseReport(long orderheaderid)
        {
            var orderdetail = _orderService.tempTbls(orderheaderid);
            int id = SessionValues.StoreId != 0 ? SessionValues.StoreId : 1;
            var itemlist = _itemService.GetItems(id);
            foreach (TempTbl order in orderdetail)
            {
                var item = _itemService.GetItemById(order.ProductId);
                itemlist.Remove(item);
            }
            ViewBag.Itemlist = itemlist;
            return PartialView("_SummeriseReportPartial");
        }

        [CustomAuthorization]
        public ActionResult MakeACopy(long headerid)
        {
            var orderdetails = _orderService.GetOrderDetails(headerid);
            var temptbl = _orderService.tempTbls(headerid);
            if (temptbl.Count() == 0)
            {
                foreach (OrderDetail re in orderdetails)
                {
                    TempTbl temp = new TempTbl();
                    temp.headerid = re.OrderHeaderId;
                    temp.ProductId = re.Item.Id;
                    temp.ItemCode = re.Item.SKUCode;
                    temp.Skudescription = re.Item.PackingDescription;
                    temp.Category = re.Item.Category.CaseConversion;
                    temp.UOM = re.Item.UOMmaster.UnitofMasureDescription;
                    temp.MaxorderLimit = re.Item.MaximumOrderLimit;
                    temp.Quantity = re.Quantity;
                    _orderService.InsertTemp(temp);
                    try
                    {
                        var auditJavascript = new JavaScriptSerializer();
                        var orderDetailAudit = auditJavascript.Serialize(temp);
                        _auditService.InsertAudit(HelperCls.GetIPAddress(), orderDetailAudit, Request.Url.OriginalString.ToString(), null, SessionValues.StoreId);
                    }
                    catch (Exception ex)
                    {
                        HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                        throw ex;
                    }
                }
            }
            ViewBag.Itemlist = _orderService.tempTbls(headerid);
            return View();
        }

        [HttpPost]
        [CustomApiAuthorization]
        public JsonResult InsertInTemp(List<TempTbl> tempTbls)
        {
            int affectedrow = 0;
            foreach (TempTbl tbl in tempTbls)
            {
                try
                {
                    var auditJavascript = new JavaScriptSerializer();
                    var orderDetailAudit = auditJavascript.Serialize("OrderHeaderId: " + tbl.headerid + ", ItemId" + tbl.ProductId + ", Quantity" + tbl.Quantity);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), orderDetailAudit, Request.Url.OriginalString.ToString(), null, SessionValues.StoreId);
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    throw ex;
                }
                affectedrow = _orderService.InsertTemp(tbl);
            }
            bool cond = affectedrow > 0 ? true : false;
            return Json(cond, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomApiAuthorization]
        public JsonResult DeleteFromTemp(long headerid, int productid)
        {
            bool cond = _orderService.DeleteParticularItem(headerid, productid) > 0 ? true : false;
            try
            {
                var auditJavascript = new JavaScriptSerializer();
                var orderDetailAudit = auditJavascript.Serialize("HeaderId: " + headerid + ", ProductId" + productid);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), orderDetailAudit, Request.Url.OriginalString.ToString(), null, SessionValues.StoreId);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            return Json(cond, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomApiAuthorization]
        public JsonResult UpdateFromTemp(long OrderHeaderId, int ItemId, int Quantity)
        {
            var temp = _orderService.TempTbl(OrderHeaderId, ItemId);
            temp.Quantity = Quantity;
            bool cond = _orderService.UpdateQtyInTemp(temp) > 0 ? true : false;
            try
            {
                var auditJavascript = new JavaScriptSerializer();
                var orderDetailAudit = auditJavascript.Serialize("OrderHeaderId: " + temp.headerid + ", ItemId" + temp.ProductId + ", Quantity" + temp.Quantity);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), orderDetailAudit, Request.Url.OriginalString.ToString(), null, SessionValues.StoreId);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            return Json(cond, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomApiAuthorization]
        public JsonResult SavaAsDraftOrFinilize(List<OrderSave> order, string issave)
        {
            int id = SessionValues.StoreId != 0 ? SessionValues.StoreId : 1;
            var store = _storeService.GetStoreById(id);
            OrderHeader orderHeader = new OrderHeader();
            orderHeader.DraftOrderno = (issave == "Save" ? "rfpl/draft/" : null);
            orderHeader.Finilizeorderno = (issave != "Save" ? "rfpl/finalised/" : null);
            orderHeader.Isdeleted = false;
            orderHeader.IsOrderStatus = (issave == "Save" ? false : true);
            orderHeader.OrderDate = DateTime.Now;
            orderHeader.ProcessedDate = ((issave == "Save") ? (DateTime?)null : DateTime.Now);
            orderHeader.StoreId = id;
            orderHeader.IsProcessed = false;
            orderHeader.DCID = store.WareHouseDC.Id;
            orderHeader.CreatedDate = DateTime.Now;
            long pk_orderHeader = _orderService.SaveOrderHeader(orderHeader);
            try
            {
                var orderHeaderAudit = new OrderHeader(orderHeader.Id, orderHeader.IsProcessed, orderHeader.IsOrderStatus, orderHeader.DraftOrderno, orderHeader.Finilizeorderno, orderHeader.OrderDate, orderHeader.ProcessedDate, orderHeader.isOrderEmailSent, orderHeader.StoreId, orderHeader.ProcessedUserId, orderHeader.DCID, orderHeader.Isdeleted, orderHeader.orderDetails, null, null, null);
                var auditJavascript = new JavaScriptSerializer();
                var OrderAudit = auditJavascript.Serialize(orderHeaderAudit);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), OrderAudit, Request.Url.OriginalString.ToString(), null, SessionValues.StoreId);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            foreach (OrderSave save in order)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.Quantity = Convert.ToInt32(save.Qty);
                orderDetail.ItemId = save.ProductId;
                orderDetail.OrderHeaderId = pk_orderHeader;
                orderDetail.CreatedDate = DateTime.Now;
                long pk_orderDetail = _orderService.SaveOrderDetail(orderDetail);
                try
                {
                    var OrderDetailAudit = new OrderDetail(orderDetail.Id, orderDetail.Quantity, orderDetail.ItemId, orderDetail.OrderHeaderId, null);
                    var auditJavascript = new JavaScriptSerializer();
                    var orderDetailAudit = auditJavascript.Serialize(OrderDetailAudit);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), orderDetailAudit, Request.Url.OriginalString.ToString(), null, SessionValues.StoreId);
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    throw ex;
                }
            }
            bool cond = (pk_orderHeader > 0 ? true : false);
            if (cond && orderHeader.IsOrderStatus)
                HelperCls.SendEmail(store.StoreEmailId, "Order Finalised", HelperCls.PopulateBodyFinalizeorder(store.StoreName, pk_orderHeader.ToString()));

            _orderService.DeleteTemptbl(order.ToList().FirstOrDefault().OrderheaderId);

            return Json(cond, JsonRequestBehavior.AllowGet);

        }

        // [CustomAuthorization]
        public ActionResult ItemList(long headerid)
        {
            var orderdetail = _orderService.GetOrderDetails(headerid);
            ViewBag.Itemlist = orderdetail;
            Session["StoreName1"] = "store";
            return View();
        }

        [AdminAuthorization]
        public ActionResult OrderDelete()
        {
            try
            {
                int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
                int userid = SessionValues.UserId;
                var finilizeOrder = _reportService.ProcessedData(false, true, false, false, false, custid, userid);
                List<ProcessedCls> processeds = new List<ProcessedCls>();
                foreach (DetailReport header in finilizeOrder)
                {
                    ProcessedCls processedCls = new ProcessedCls();
                    processedCls.Id = header.Id;
                    processedCls.OrderDate = header.OrderDate;
                    processedCls.StoreName = header.StoreName;
                    processedCls.StoreCode = header.StoreCode;
                    processedCls.DCID = header.DCID;
                    processedCls.FinilizedOrderno = header.OrderNo;
                    //  processedCls.IsThreshold = (_orderService.GetOrderHeader(header.Id).orderDetails.Where(x => x.Item.MaximumOrderLimit < x.Quantity).Count() > 0 ? true : false);
                    processeds.Add(processedCls);
                }
                ViewBag.FinilizeOrder = processeds;
                return View();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        [HttpPost]
        [CustomApiAuthorization]
        public JsonResult DeleteOrders(List<OrderSave> order)
        {
            try
            {
                int userid = SessionValues.UserId;
                var affected = 0;
                foreach (OrderSave orders in order)
                {
                    affected = _orderService.DeleteOrder(orders.OrderheaderId, userid);
                    try
                    {
                        var ordersave = new OrderSave();
                        ordersave.OrderheaderId = orders.OrderheaderId;
                        ordersave.UserId = userid;
                        var auditJavascript = new JavaScriptSerializer();
                        var orderAudit = auditJavascript.Serialize(ordersave);
                        _auditService.InsertAudit(HelperCls.GetIPAddress(), orderAudit, Request.Url.OriginalString.ToString(), userid, null);
                    }
                    catch (Exception ex)
                    {
                        HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                        throw ex;
                    }
                }
                bool cond = affected > 0 ? true : false;
                return Json(cond, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        [AdminAuthorization]
        public ActionResult ImportOrder()
        {
            var LoggedInCustomer = Convert.ToInt32(SessionValues.LoggedInCustId);
            var PreOrderedCustomer = _customerService.GetCustomerById(LoggedInCustomer);
            ViewBag.PreOrder = PreOrderedCustomer.PlannedOrderFlag;
            return View(PreOrderedCustomer);
        }

        [HttpPost]
        [AdminAuthorization]
        public ActionResult Importexcel(string PreOrder = "off")
        {
            var LoggedInCustomer = Convert.ToInt32(SessionValues.LoggedInCustId);
            List<string> ErrorList = new List<string>();
            var LoggedInUser = SessionValues.UserId;
            var PreOrderedCustomer = _customerService.GetCustomerById(LoggedInCustomer);
            if (Request.Files["FileUpload1"].ContentLength > 0)
            {
                try
                {
                    HttpPostedFileBase file = Request.Files["FileUpload1"];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;

                        string _FileName = Path.GetFileName(file.FileName);
                        _FileName = LoggedInUser + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + _FileName;
                        string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), "" + _FileName);
                        if (!Directory.Exists(Server.MapPath("~/UploadedFiles")))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/UploadedFiles"));
                        }
                        file.SaveAs(_path);

                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        var imports = new List<ImportToExcel>();
                        if (PreOrderedCustomer.PlannedOrderFlag && PreOrder.ToLower() == "on" && PreOrder != null)
                        {
                            using (var package = new ExcelPackage(file.InputStream))
                            {
                                var currentSheet = package.Workbook.Worksheets;
                                var workSheet = currentSheet.First();
                                var noOfCol = workSheet.Dimension.End.Column;
                                var noOfRow = workSheet.Dimension.End.Row;
                                OrderFormatExcel excelmapping = _orderService.GetUploadOrderFormat(LoggedInCustomer, "ORDER");
                                if (excelmapping != null)
                                {
                                    ImportToExcel format = new ImportToExcel();
                                    for (int i = 1; i <= workSheet.Dimension.End.Column; i++)
                                    {
                                        var firstRowCell = workSheet.Cells[1, i];
                                        if (excelmapping.ExcelHeaders.Exists(x => x.HeaderName == firstRowCell.Value.ToString()))
                                        {
                                            string HeaderName = excelmapping.ExcelHeaders.Where(x => x.HeaderName == firstRowCell.Value.ToString()).FirstOrDefault().ParameterName;
                                            if (HeaderName == nameof(format.StoreCode))
                                            {
                                                format.StoreColumnId = i;
                                            }
                                            else if (HeaderName == nameof(format.SkuCode))
                                            {
                                                format.SKUColumnId = i;
                                            }
                                            else if (HeaderName == nameof(format.Qty))
                                            {
                                                format.QtyColumnId = i;
                                            }
                                            else if (HeaderName == nameof(format.OrderDate))
                                            {
                                                format.OrderDateColumnId = i;
                                            }
                                            else if (HeaderName == nameof(format.OrderReferenceNo))
                                            {
                                                format.OrderReferenceID = i;
                                            }
                                        }
                                    }
                                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                                    {
                                        if (format.OrderDateColumnId != 0 && format.StoreColumnId != 0 && format.SKUColumnId != 0 && format.SKUColumnId != 0)
                                        {
                                            if (workSheet.Cells[rowIterator, format.StoreColumnId].Value != null && workSheet.Cells[rowIterator, format.SKUColumnId].Value != null && workSheet.Cells[rowIterator, format.QtyColumnId].Value != null)
                                            {
                                                var user = new Users();
                                                var import = new ImportToExcel();
                                                import.StoreCode = workSheet.Cells[rowIterator, format.StoreColumnId].Value.ToString();
                                                import.SkuCode = workSheet.Cells[rowIterator, format.SKUColumnId].Value.ToString();
                                                import.Qty = workSheet.Cells[rowIterator, format.QtyColumnId].Value.ToString();
                                                if (format.OrderReferenceID != 0)
                                                    import.OrderReferenceNo = workSheet.Cells[rowIterator, format.OrderReferenceID].Value.ToString();
                                                DateTime? date = new DateTime?();
                                                try
                                                {
                                                    date = workSheet.Cells[rowIterator, format.OrderDateColumnId].Value != null ? Convert.ToDateTime(workSheet.Cells[rowIterator, format.OrderDateColumnId].Value.ToString()) : (Nullable<DateTime>)null;
                                                }
                                                catch (Exception ex)
                                                {
                                                    import.Error += ex.Message;
                                                }
                                                import.OrderDate = date;
                                                import.Error = _orderService.ItemForCustomer(import.StoreCode, import.SkuCode, LoggedInCustomer, LoggedInUser, null);
                                                imports.Add(import);
                                            }
                                        }
                                        else
                                        {
                                            if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null && workSheet.Cells[rowIterator, 3].Value != null)
                                            {
                                                var user = new Users();
                                                var import = new ImportToExcel();
                                                import.StoreCode = workSheet.Cells[rowIterator, 1].Value.ToString();
                                                import.SkuCode = workSheet.Cells[rowIterator, 2].Value.ToString();
                                                import.Qty = workSheet.Cells[rowIterator, 3].Value.ToString();
                                                DateTime? date = new DateTime?();
                                                try
                                                {
                                                    date = workSheet.Cells[rowIterator, 4].Value != null ? Convert.ToDateTime(workSheet.Cells[rowIterator, 4].Value.ToString()) : (Nullable<DateTime>)null;
                                                }
                                                catch (Exception ex)
                                                {
                                                    import.Error += ex.Message;
                                                }
                                                import.OrderDate = date;
                                                import.Error += _orderService.ItemForCustomer(import.StoreCode, import.SkuCode, LoggedInCustomer, LoggedInUser, null);
                                                imports.Add(import);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                                    {
                                        if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null && workSheet.Cells[rowIterator, 3].Value != null)
                                        {
                                            var user = new Users();
                                            var import = new ImportToExcel();
                                            import.StoreCode = workSheet.Cells[rowIterator, 1].Value.ToString();
                                            import.SkuCode = workSheet.Cells[rowIterator, 2].Value.ToString();
                                            import.Qty = workSheet.Cells[rowIterator, 3].Value.ToString();
                                            DateTime? date = new DateTime?();
                                            try
                                            {
                                                date = workSheet.Cells[rowIterator, 4].Value != null ? Convert.ToDateTime(workSheet.Cells[rowIterator, 4].Value.ToString()) : (Nullable<DateTime>)null;
                                            }
                                            catch (Exception ex)
                                            {
                                                import.Error += ex.Message;
                                            }
                                            import.OrderDate = date;
                                            import.Error += _orderService.ItemForCustomer(import.StoreCode, import.SkuCode, LoggedInCustomer, LoggedInUser, null);
                                            imports.Add(import);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            using (var package = new ExcelPackage(file.InputStream))
                            {
                                var currentSheet = package.Workbook.Worksheets;
                                var workSheet = currentSheet.First();
                                var noOfCol = workSheet.Dimension.End.Column;
                                OrderFormatExcel excelmapping = _orderService.GetUploadOrderFormat(LoggedInCustomer, "ORDER");
                                if (excelmapping != null)
                                {
                                    ImportToExcel format = new ImportToExcel();

                                    for (int i = 1; i <= workSheet.Dimension.End.Column; i++)
                                    {
                                        var firstRowCell = workSheet.Cells[1, i];
                                        if (excelmapping.ExcelHeaders.Exists(x => x.HeaderName == firstRowCell.Value.ToString()))
                                        {
                                            string HeaderName = excelmapping.ExcelHeaders.Where(x => x.HeaderName == firstRowCell.Value.ToString()).FirstOrDefault().ParameterName;
                                            if (HeaderName.Trim().ToUpper() == nameof(format.StoreCode).Trim().ToUpper()) { format.StoreColumnId = i; }
                                            else if (HeaderName.Trim().ToUpper() == nameof(format.SkuCode).Trim().ToUpper()) { format.SKUColumnId = i; }
                                            else if (HeaderName.Trim().ToUpper() == nameof(format.Qty).Trim().ToUpper()) { format.QtyColumnId = i; }
                                            else if (HeaderName.Trim().ToUpper() == nameof(format.OrderDate).Trim().ToUpper()) { format.OrderDateColumnId = i; }
                                            else if (HeaderName.Trim().ToUpper() == nameof(format.OrderReferenceNo).Trim().ToUpper()) { format.OrderReferenceID = i; }
                                            else if (HeaderName.Trim().ToUpper() == nameof(format.OrderPlacedDate).Trim().ToUpper()) { format.OrderPlacedDateID = i; }
                                            else if (HeaderName.Trim().ToUpper() == nameof(format.OrderType).Trim().ToUpper()) { format.OrderTypeID = i; }
                                            else if (HeaderName.Trim().ToUpper() == nameof(format.City).Trim().ToUpper()) { format.CityID = i; }
                                            else if (HeaderName.Trim().ToUpper() == nameof(format.LastOrderDate).Trim().ToUpper()) { format.LastOrderDateID = i; }
                                            else if (HeaderName.Trim().ToUpper() == nameof(format.LateDeliveryDate).Trim().ToUpper()) { format.LateDeliveryDateID = i; }
                                            else if (HeaderName.Trim().ToUpper() == nameof(format.UniqueReferenceID).Trim().ToUpper()) { format.UniqueReferenceIDID = i; }
                                            else if (HeaderName.Trim().ToUpper() == nameof(format.PackSize).Trim().ToUpper()) { format.PackSizeID = i; }
                                            else if (HeaderName.Trim().ToUpper() == nameof(format.OrderedQty).Trim().ToUpper()) { format.OrderedQtyID = i; }
                                            else if (HeaderName.Trim().ToUpper() == nameof(format.CaseSize).Trim().ToUpper()) { format.CaseSizeID = i; }
                                            else if (HeaderName.Trim().ToUpper() == nameof(format.Cases).Trim().ToUpper()) { format.CasesID = i; }
                                            else if (HeaderName.Trim().ToUpper() == nameof(format.State).Trim().ToUpper()) { format.StateID = i; }
                                            else if (HeaderName.Trim().ToUpper() == nameof(format.FromDC).Trim().ToUpper()) { format.FromDCID = i; }
                                            else if (HeaderName.Trim().ToUpper() == nameof(format.New_Build).Trim().ToUpper()) { format.New_BuildID = i; }
                                            else if (HeaderName.Trim().ToUpper() == nameof(format.Focus).Trim().ToUpper()) { format.FocusID = i; }
                                            else if (HeaderName.Trim().ToUpper() == nameof(format.Item_Type).Trim().ToUpper()) { format.Item_TypeID = i; }
                                            else if (HeaderName.Trim().ToUpper() == nameof(format.Item_sub_Type).Trim().ToUpper()) { format.Item_sub_TypeID = i; }
                                            else if (HeaderName.Trim().ToUpper() == nameof(format.TakenDays).Trim().ToUpper()) { format.TakenDaysID = i; }
                                            else if (HeaderName.Trim().ToUpper() == nameof(format.Slab).Trim().ToUpper()) { format.SlabID = i; }
                                        }
                                    }
                                    var noOfRow = workSheet.Dimension.End.Row;
                                    if (format.OrderDateColumnId == 0)
                                    {
                                        string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(format.OrderDate)).FirstOrDefault().HeaderName;
                                        ErrorList.Add(HeaderName + " column does not match");
                                    }
                                    if (format.OrderPlacedDateID == 0)
                                    {
                                        string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(format.OrderPlacedDate)).FirstOrDefault().HeaderName;
                                        ErrorList.Add(HeaderName + " column does not match");
                                    }
                                    if (format.OrderTypeID == 0)
                                    {
                                        string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(format.OrderType)).FirstOrDefault().HeaderName;
                                        ErrorList.Add(HeaderName + " column does not match");
                                    }
                                    if (format.StoreColumnId == 0)
                                    {
                                        string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(format.StoreCode)).FirstOrDefault().HeaderName;
                                        ErrorList.Add(HeaderName + " column does not match");
                                    }
                                    if (format.CityID == 0)
                                    {
                                        string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(format.City)).FirstOrDefault().HeaderName;
                                        ErrorList.Add(HeaderName + " column does not match");
                                    }
                                    if (format.LastOrderDateID == 0)
                                    {
                                        string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(format.LastOrderDate)).FirstOrDefault().HeaderName;
                                        ErrorList.Add(HeaderName + " column does not match");
                                    }
                                    if (format.LateDeliveryDateID == 0)
                                    {
                                        string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(format.LateDeliveryDate)).FirstOrDefault().HeaderName;
                                        ErrorList.Add(HeaderName + " column does not match");
                                    }
                                    if (format.OrderReferenceID == 0)
                                    {
                                        string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(format.OrderReferenceNo)).FirstOrDefault().HeaderName;
                                        ErrorList.Add(HeaderName + " column does not match");
                                    }
                                    if (format.SKUColumnId == 0)
                                    {
                                        string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(format.SkuCode)).FirstOrDefault().HeaderName;
                                        ErrorList.Add(HeaderName + " column does not match");
                                    }
                                    if (format.UniqueReferenceIDID == 0)
                                    {
                                        string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(format.UniqueReferenceID)).FirstOrDefault().HeaderName;
                                        ErrorList.Add(HeaderName + " column does not match");
                                    }
                                    if (format.OrderedQtyID == 0)
                                    {
                                        string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(format.OrderedQty)).FirstOrDefault().HeaderName;
                                        ErrorList.Add(HeaderName + " column does not match");
                                    }
                                    if (format.CaseSizeID == 0)
                                    {
                                        string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(format.CaseSize)).FirstOrDefault().HeaderName;
                                        ErrorList.Add(HeaderName + " column does not match");
                                    }
                                    if (format.CasesID == 0)
                                    {
                                        string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(format.Cases)).FirstOrDefault().HeaderName;
                                        ErrorList.Add(HeaderName + " column does not match");
                                    }
                                    if (format.FromDCID == 0)
                                    {
                                        string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(format.FromDC)).FirstOrDefault().HeaderName;
                                        ErrorList.Add(HeaderName + " column does not match");
                                    }
                                    if (format.New_BuildID == 0)
                                    {
                                        string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(format.New_Build)).FirstOrDefault().HeaderName;
                                        ErrorList.Add(HeaderName + " column does not match");
                                    }
                                    if (format.QtyColumnId == 0)
                                    {
                                        string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(format.Qty)).FirstOrDefault().HeaderName;
                                        ErrorList.Add(HeaderName + " column does not match");
                                    }
                                    if (format.FocusID == 0)
                                    {
                                        string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(format.Focus)).FirstOrDefault().HeaderName;
                                        ErrorList.Add(HeaderName + " column does not match");
                                    }
                                    if (format.Item_TypeID == 0)
                                    {
                                        string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(format.Item_Type)).FirstOrDefault().HeaderName;
                                        ErrorList.Add(HeaderName + " column does not match");
                                    }
                                    if (format.Item_sub_TypeID == 0)
                                    {
                                        string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(format.Item_sub_Type)).FirstOrDefault().HeaderName;
                                        ErrorList.Add(HeaderName + " column does not match");
                                    }
                                    if (format.TakenDaysID == 0)
                                    {
                                        string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(format.TakenDays)).FirstOrDefault().HeaderName;
                                        ErrorList.Add(HeaderName + " column does not match");
                                    }
                                    if (format.SlabID == 0)
                                    {
                                        string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(format.Slab)).FirstOrDefault().HeaderName;
                                        ErrorList.Add(HeaderName + " column does not match");
                                    }
                                    if (format.StoreColumnId != 0 && format.SKUColumnId != 0 && format.QtyColumnId != 0)
                                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                                        {
                                            if (workSheet.Cells[rowIterator, format.StoreColumnId].Value != null && workSheet.Cells[rowIterator, format.SKUColumnId].Value != null && workSheet.Cells[rowIterator, format.QtyColumnId].Value != null)
                                            {
                                                var user = new Users();
                                                var import = new ImportToExcel();
                                                import.Error = "";
                                                if (workSheet.Cells[rowIterator, format.StoreColumnId].Value != null)
                                                    import.StoreCode = workSheet.Cells[rowIterator, format.StoreColumnId].Value.ToString();
                                                else
                                                {
                                                    import.Error += "Error at Row: " + rowIterator + ", column: " + format.StoreColumnId + "No StoreCode.";
                                                    import.Qty = 0.ToString();
                                                }
                                                if (workSheet.Cells[rowIterator, format.SKUColumnId].Value != null)
                                                    import.SkuCode = workSheet.Cells[rowIterator, format.SKUColumnId].Value.ToString();
                                                else
                                                {
                                                    import.Error += "Error at Row: " + rowIterator + ", column: " + format.SKUColumnId + "No Kitchen ID.";
                                                    import.Qty = 0.ToString();
                                                }
                                                try
                                                {
                                                    import.Qty = Convert.ToInt32(workSheet.Cells[rowIterator, format.QtyColumnId].Value.ToString()).ToString();
                                                }
                                                catch (Exception ex)
                                                {
                                                    import.Error += "Error at Row: " + rowIterator + ", column: " + format.QtyColumnId + "Unable to Convert the Quantity.";
                                                    import.Qty = 0.ToString();
                                                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                                                }
                                                DateTime? date = new DateTime?();
                                                if (format.OrderDateColumnId != 0)
                                                    try
                                                    {
                                                        if (workSheet.Cells[rowIterator, format.OrderDateColumnId].Value != null)
                                                        {
                                                            date = workSheet.Cells[rowIterator, format.OrderDateColumnId].Value != null ? HelperCls.FromExcelStringDate(workSheet.Cells[rowIterator, format.OrderDateColumnId].Value.ToString()) : (Nullable<DateTime>)null;
                                                        }
                                                        else
                                                        {
                                                            import.Error += "Error at Row: " + rowIterator + ", column: " + format.OrderDateColumnId + "Unable to Convert the Date.";
                                                            date = null;
                                                        }
                                                        import.OrderDate = date;
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        import.Error += "Error at Row: " + rowIterator + ", column: " + format.OrderDateColumnId + "Unable to Convert the Date.";
                                                        HelperCls.DebugLog("Error" + workSheet.Cells[rowIterator, format.OrderDateColumnId].Value + ex.Message + System.Environment.NewLine + ex.StackTrace);
                                                        import.Error += ex.Message;
                                                    }
                                                if (format.OrderReferenceID != 0 && workSheet.Cells[rowIterator, format.OrderReferenceID].Value != null)
                                                    import.OrderReferenceNo = workSheet.Cells[rowIterator, format.OrderReferenceID].Value != null ? workSheet.Cells[rowIterator, format.OrderReferenceID].Value.ToString() : "";
                                                else
                                                {
                                                    import.Error += "Error at Row: " + rowIterator + ", column: " + format.OrderReferenceID + "OrderReferenceNo is Blank";
                                                    import.OrderReferenceNo = "";
                                                }
                                                if (format.OrderPlacedDateID != 0)
                                                    import.OrderPlacedDate = workSheet.Cells[rowIterator, format.OrderPlacedDateID].Value != null ? workSheet.Cells[rowIterator, format.OrderPlacedDateID].Value.ToString() : "";
                                                if (format.OrderTypeID != 0)
                                                    import.OrderType = workSheet.Cells[rowIterator, format.OrderTypeID].Value != null ? workSheet.Cells[rowIterator, format.OrderTypeID].Value.ToString() : "";
                                                if (format.CityID != 0)
                                                    import.City = workSheet.Cells[rowIterator, format.CityID].Value != null ? workSheet.Cells[rowIterator, format.CityID].Value.ToString() : "";
                                                if (format.LastOrderDateID != 0)
                                                    import.LastOrderDate = workSheet.Cells[rowIterator, format.LastOrderDateID].Value != null ? workSheet.Cells[rowIterator, format.LastOrderDateID].Value.ToString() : "";
                                                if (format.LateDeliveryDateID != 0)
                                                    import.LateDeliveryDate = workSheet.Cells[rowIterator, format.LateDeliveryDateID].Value != null ? workSheet.Cells[rowIterator, format.LateDeliveryDateID].Value.ToString() : "";
                                                if (format.UniqueReferenceIDID != 0 && workSheet.Cells[rowIterator, format.UniqueReferenceIDID].Value != null)
                                                    import.UniqueReferenceID = workSheet.Cells[rowIterator, format.UniqueReferenceIDID].Value != null ? workSheet.Cells[rowIterator, format.UniqueReferenceIDID].Value.ToString() : "";
                                                else
                                                {
                                                    import.Error += "Error at Row: " + rowIterator + ", column: " + format.UniqueReferenceIDID + "UniqueReferenceID is Blank";
                                                    import.UniqueReferenceID = 0.ToString();
                                                }
                                                if (format.PackSizeID != 0)
                                                    import.PackSize = workSheet.Cells[rowIterator, format.PackSizeID].Value != null ? workSheet.Cells[rowIterator, format.PackSizeID].Value.ToString() : "";
                                                if (format.OrderedQtyID != 0)
                                                    import.OrderedQty = workSheet.Cells[rowIterator, format.OrderedQtyID].Value != null ? workSheet.Cells[rowIterator, format.OrderedQtyID].Value.ToString() : "";
                                                if (format.CaseSizeID != 0)
                                                    import.CaseSize = workSheet.Cells[rowIterator, format.CaseSizeID].Value != null ? workSheet.Cells[rowIterator, format.CaseSizeID].Value.ToString() : "";
                                                if (format.CasesID != 0)
                                                    import.Cases = workSheet.Cells[rowIterator, format.CasesID].Value != null ? workSheet.Cells[rowIterator, format.CasesID].Value.ToString() : "";
                                                if (format.StateID != 0)
                                                    import.State = workSheet.Cells[rowIterator, format.StateID].Value != null ? workSheet.Cells[rowIterator, format.StateID].Value.ToString() : "";
                                                if (format.FromDCID != 0)
                                                    import.FromDC = workSheet.Cells[rowIterator, format.FromDCID].Value != null ? workSheet.Cells[rowIterator, format.FromDCID].Value.ToString() : "";
                                                if (format.New_BuildID != 0)
                                                    import.New_Build = workSheet.Cells[rowIterator, format.New_BuildID].Value != null ? workSheet.Cells[rowIterator, format.New_BuildID].Value.ToString() : "";
                                                if (format.FocusID != 0)
                                                    import.Focus = workSheet.Cells[rowIterator, format.FocusID].Value != null ? workSheet.Cells[rowIterator, format.FocusID].Value.ToString() : "";
                                                if (format.Item_TypeID != 0)
                                                    import.Item_Type = workSheet.Cells[rowIterator, format.Item_TypeID].Value != null ? workSheet.Cells[rowIterator, format.Item_TypeID].Value.ToString() : "";
                                                if (format.Item_sub_TypeID != 0)
                                                    import.Item_sub_Type = workSheet.Cells[rowIterator, format.Item_sub_TypeID].Value != null ? workSheet.Cells[rowIterator, format.Item_sub_TypeID].Value.ToString() : "";
                                                if (format.TakenDaysID != 0)
                                                    import.TakenDays = workSheet.Cells[rowIterator, format.TakenDaysID].Value != null ? workSheet.Cells[rowIterator, format.TakenDaysID].Value.ToString() : "";
                                                if (format.SlabID != 0)
                                                    import.Slab = workSheet.Cells[rowIterator, format.SlabID].Value != null ? workSheet.Cells[rowIterator, format.SlabID].Value.ToString() : "";
                                                import.Error += _orderService.ItemForCustomer(import.StoreCode, import.SkuCode, LoggedInCustomer, LoggedInUser, import.UniqueReferenceID);
                                                imports.Add(import);
                                            }
                                        }
                                }
                                else
                                {
                                    var noOfRow = workSheet.Dimension.End.Row;
                                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                                    {
                                        if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null && workSheet.Cells[rowIterator, 3].Value != null)
                                        {
                                            var user = new Users();
                                            var import = new ImportToExcel();
                                            import.StoreCode = workSheet.Cells[rowIterator, 1].Value.ToString();
                                            import.SkuCode = workSheet.Cells[rowIterator, 2].Value.ToString();
                                            import.Qty = workSheet.Cells[rowIterator, 3].Value.ToString();
                                            import.Error = _orderService.ItemForCustomer(import.StoreCode, import.SkuCode, LoggedInCustomer, LoggedInUser, null);
                                            imports.Add(import);
                                        }
                                    }
                                }
                            }
                        }
                        var im = imports
                                .GroupBy(l => new { l.SkuCode, l.StoreCode, l.OrderDate, l.Error })
                                .Select(cl => new ImportToExcel
                                {
                                    SkuCode = cl.First().SkuCode,
                                    StoreCode = cl.First().StoreCode,
                                    OrderDate = cl.First().OrderDate,
                                    Error = string.Join(",", cl.Where(o => o.Error != "").Select(o => o.Error).ToArray()),
                                    OrderReferenceNo = cl.First().OrderReferenceNo,
                                    OrderPlacedDate = cl.First().OrderPlacedDate,
                                    OrderType = cl.First().OrderType,
                                    City = cl.First().City,
                                    LastOrderDate = cl.First().LastOrderDate,
                                    LateDeliveryDate = cl.First().LateDeliveryDate,
                                    UniqueReferenceID = cl.First().UniqueReferenceID,
                                    PackSize = cl.First().PackSize,
                                    OrderedQty = cl.First().OrderedQty,
                                    CaseSize = cl.First().CaseSize,
                                    Cases = cl.First().Cases,
                                    State = cl.First().State,
                                    FromDC = cl.First().FromDC,
                                    New_Build = cl.First().New_Build,
                                    Focus = cl.First().Focus,
                                    Item_Type = cl.First().Item_Type,
                                    Item_sub_Type = cl.First().Item_sub_Type,
                                    TakenDays = cl.First().TakenDays,
                                    Slab = cl.First().Slab,
                                    Qty = cl.Sum(c => Convert.ToInt32(c.Qty)).ToString()
                                }).OrderBy(x => x.OrderDate).ToList();
                        ViewBag.FinilizeOrder = im;
                        ViewBag.PreOrder = PreOrder;
                        ViewBag.ErrorList = ErrorList;
                        if (ErrorList != null && ErrorList.Count() > 0)
                        {
                            ViewBag.FinilizeOrder = null;
                            ViewBag.PreOrder = null;
                            ViewBag.Error = string.Join(", ", ErrorList.ToArray());
                            foreach (string err in ErrorList)
                                ModelState.AddModelError("", err);
                        }
                        return View("ImportOrder", PreOrderedCustomer);
                    }
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    ViewBag.Error = "Invalid File Format";
                    ViewBag.FinilizeOrder = null;
                }
            }
            ViewBag.Preorder = PreOrder;
            return View("ImportOrder", PreOrderedCustomer);
        }

        [HttpPost]
        [AdminApiAuthorization]
        public JsonResult SaveUploadOrder(List<ImportToExcel> order, string PreOrder = "off")
        {
            //
            var LoggedInCustomer = Convert.ToInt32(SessionValues.LoggedInCustId);
            var PreOrderedCustomer = _customerService.GetCustomerById(LoggedInCustomer);
            if (PreOrderedCustomer.PlannedOrderFlag && PreOrder.ToLower() == "on" && PreOrder != null)
            {
                if (order.Count > 0)
                {
                    var data = from a in order
                               where a.Error != null
                               select a.StoreCode;
                    var data1 = data.ToList();
                    var Importlist = new List<ImportToExcel>();
                    foreach (string storecode in data1)
                    {
                        var remove = from b in order
                                     where b.StoreCode == storecode
                                     select b;
                        var remove1 = remove.ToList();
                        foreach (ImportToExcel importToExcel in remove1)
                        {
                            order.Remove(importToExcel);
                        }
                    }
                    var storecode1 = (from p in order
                                      select p).GroupBy(x => new { x.StoreCode, x.OrderDate })
                                    .Select(g => g.FirstOrDefault()).ToList();
                    int userid = SessionValues.UserId;
                    foreach (ImportToExcel excel in storecode1)
                    {
                        var header = from or in order
                                     where or.StoreCode == excel.StoreCode && excel.OrderDate == or.OrderDate
                                     select or;
                        var store = header.FirstOrDefault().StoreCode;
                        var warehouse = header.FirstOrDefault().Location;
                        var OrderDate = header.FirstOrDefault().OrderDate;
                        var OrderReferenceNo = header.FirstOrDefault().OrderReferenceNo;
                        var headerid = _orderService.InsertOrderHeader(store, userid, OrderReferenceNo, 1, OrderDate);
                        foreach (ImportToExcel importTo in header)
                        {
                            var detailid = _orderService.InsertOrderDetail(importTo.SkuCode, Convert.ToInt32(importTo.Qty), headerid, null);
                        }
                    }
                    var auditJavascript = new JavaScriptSerializer();
                    var orderAudit = auditJavascript.Serialize(order);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), orderAudit, Request.Url.OriginalString.ToString(), userid, null);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (order.Count > 0)
                {
                    var data = from a in order
                               where a.Error != null
                               select a.StoreCode;
                    var data1 = data.ToList();
                    var Importlist = new List<ImportToExcel>();
                    foreach (string storecode in data1)
                    {
                        var remove = from b in order
                                     where b.StoreCode == storecode
                                     select b;
                        var remove1 = remove.ToList();
                        foreach (ImportToExcel importToExcel in remove1)
                        {
                            order.Remove(importToExcel);
                        }
                    }
                    var storecode1 = (from p in order
                                      select p).GroupBy(x => new { x.StoreCode, x.OrderDate })
                                    .Select(g => g.FirstOrDefault()).ToList();
                    int userid = SessionValues.UserId;
                    foreach (ImportToExcel excel in storecode1)
                    {
                        var header = from or in order
                                     where or.StoreCode == excel.StoreCode && or.OrderDate == excel.OrderDate
                                     select or;
                        var store = header.FirstOrDefault().StoreCode;
                        var warehouse = header.FirstOrDefault().Location;
                        var OrderReferenceNo = header.FirstOrDefault().OrderReferenceNo;
                        var OrderDate = header.FirstOrDefault().OrderDate;
                        long headerid;
                        if (OrderDate == null)
                            headerid = _orderService.InsertOrderHeader(store, userid, OrderReferenceNo, 2, DateTime.Now);
                        else
                            headerid = _orderService.InsertOrderHeader(store, userid, OrderReferenceNo, 2, OrderDate);
                        foreach (ImportToExcel importTo in header)
                        {
                            var detailid = _orderService.InsertOrderDetail(importTo.SkuCode, Convert.ToInt32(importTo.Qty), headerid, importTo.UniqueReferenceID);
                        }

                    }
                    foreach (ImportToExcel excelorder in order)
                    {
                        try
                        {
                            var detailId = _orderService.InsertOrderData(LoggedInCustomer, excelorder.StoreCode, excelorder.SkuCode, excelorder.Qty, excelorder.OrderDate, excelorder.OrderReferenceNo, excelorder.OrderPlacedDate, excelorder.OrderType, excelorder.City, excelorder.LastOrderDate, excelorder.LateDeliveryDate, excelorder.UniqueReferenceID, excelorder.PackSize, excelorder.OrderedQty, excelorder.CaseSize, excelorder.Cases, excelorder.State, excelorder.FromDC, excelorder.New_Build, excelorder.Focus, excelorder.Item_Type, excelorder.Item_sub_Type, excelorder.TakenDays, excelorder.Slab);
                        }
                        catch (Exception ex)
                        {
                            HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                        }
                    }

                    var auditJavascript = new JavaScriptSerializer();
                    var orderAudit = auditJavascript.Serialize(order);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), orderAudit, Request.Url.OriginalString.ToString(), userid, null);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }

        }

        [HttpGet]
        [AdminAuthorization]
        public ActionResult CreateOrder()
        {
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            int userid = Convert.ToInt32(SessionValues.UserId);
            List<Store> allowedStores = _storeService.GetAvailableStoresforUserId(custid, userid);
            ViewBag.allowedStores = allowedStores;
            List<int> StoreIds = allowedStores.Select(x => x.Id).Distinct().ToList();
            List<CurrentItemInventoryMst> currentItemList = _orderService.GetCurrentItemInventoryMstForCustomer(custid, userid, StoreIds);
            ViewBag.allowedCategory = currentItemList.Select(x => x.Item.Category).Distinct().ToList();
            ViewBag.Items = currentItemList.Select(x => x.Item).Distinct().ToList();
            return View(currentItemList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminApiAuthorization]
        public ActionResult SaveOrFinaliseOrderForStores(List<OrderSave> order, string issave, List<int> selectedStores)
        {
            bool cond = false;

            if (order.Count() > 0 && issave != "" && issave != null)
            {
                var groupedorder = order.GroupBy(x => x.storeId);
                foreach (var itemorder in groupedorder)
                {
                    var store = _storeService.GetStoreById(itemorder.Key);
                    OrderHeader orderHeader = new OrderHeader();
                    orderHeader.DraftOrderno = (issave == "Save" ? "rfpl/draft/" : null);
                    orderHeader.Finilizeorderno = (issave != "Save" ? "rfpl/finalised/" : null);
                    orderHeader.Isdeleted = false;
                    orderHeader.IsOrderStatus = (issave == "Save" ? false : true);
                    orderHeader.OrderDate = DateTime.Now;
                    orderHeader.ProcessedDate = ((issave == "Save") ? (DateTime?)null : DateTime.Now);
                    orderHeader.StoreId = itemorder.Key;
                    orderHeader.IsProcessed = false;
                    orderHeader.DCID = store.WareHouseDC.Id;
                    long pk_orderHeader = _orderService.SaveOrderHeader(orderHeader);
                    try
                    {
                        var orderHeaderAudit = new OrderHeader(orderHeader.Id, orderHeader.IsProcessed, orderHeader.IsOrderStatus, orderHeader.DraftOrderno, orderHeader.Finilizeorderno, orderHeader.OrderDate, orderHeader.ProcessedDate, orderHeader.isOrderEmailSent, orderHeader.StoreId, orderHeader.ProcessedUserId, orderHeader.DCID, orderHeader.Isdeleted, orderHeader.orderDetails, null, null, null);
                        var auditJavascript = new JavaScriptSerializer();
                        var OrderAudit = auditJavascript.Serialize(orderHeaderAudit);
                        _auditService.InsertAudit(HelperCls.GetIPAddress(), OrderAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, itemorder.Key);
                    }
                    catch (Exception ex)
                    {
                        HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                        throw ex;
                    }
                    foreach (OrderSave save in itemorder.ToList())
                    {
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.Quantity = Convert.ToInt32(save.Qty);
                        orderDetail.ItemId = save.ProductId;
                        orderDetail.OrderHeaderId = pk_orderHeader;
                        orderDetail.StoreId = save.storeId;
                        long pk_orderDetail = _orderService.SaveOrderDetail(orderDetail);
                        try
                        {
                            var OrderDetailAudit = new OrderDetail(orderDetail.Id, orderDetail.Quantity, orderDetail.ItemId, orderDetail.OrderHeaderId, null);
                            var auditJavascript = new JavaScriptSerializer();
                            var OrderAudit = auditJavascript.Serialize(OrderDetailAudit);
                            _auditService.InsertAudit(HelperCls.GetIPAddress(), OrderAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, itemorder.Key);
                        }
                        catch (Exception ex)
                        {
                            HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                            throw ex;
                        }
                    }
                    int userid = SessionValues.UserId;
                    cond = (pk_orderHeader > 0 ? true : false);
                    if (cond)
                    {
                        var orderDetails = _orderService.GetOrderDetails(pk_orderHeader);
                        cond = (orderDetails.Count() > 0 ? true : false);
                        if (!cond)
                            _orderService.DeleteOrder(pk_orderHeader, userid);
                    }
                    if (cond && orderHeader.IsOrderStatus)
                    {
                        HelperCls.SendEmail(store.StoreEmailId, "Order Finalize", HelperCls.PopulateBodyFinalizeorder(store.StoreName, pk_orderHeader.ToString()));
                    }
                }
            }
            return Json(cond, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AdminAuthorization]
        public ActionResult DraftOrders()
        {
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            int userid = SessionValues.UserId;
            List<Store> allowedStores = _storeService.GetAvailableStoresforUserId(custid, userid);
            List<int> StoreIds = allowedStores.Select(x => x.Id).Distinct().ToList();
            List<OrderHeader> orders = _orderService.GetDraftOrdersforAdminUser(userid, custid, StoreIds);
            return View(orders);
        }

        [AdminAuthorization]
        public ActionResult DraftOrder(long OrderId)
        {
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            int userid = SessionValues.UserId;
            List<Store> allowedStores = _storeService.GetAvailableStoresforUserId(custid, userid);
            List<int> StoreIds = allowedStores.Select(x => x.Id).Distinct().ToList();
            List<OrderHeader> orders = _orderService.GetDraftOrdersforAdminUser(userid, custid, StoreIds);
            var OrderDetaillist = _orderService.GetDraftOrderDetails(OrderId, userid, custid, StoreIds);
            return View(orders);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [AdminApiAuthorization]
        public ActionResult CurrentItemInventory(List<int> SelectedstoreIds, List<int> Categories, List<int> Items)
        {
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            int userid = SessionValues.UserId;
            List<CurrentItemInventoryMst> currentItemList = _orderService.GetCurrentItemInventoryMstForCustomer(custid, userid, SelectedstoreIds);
            List<Store> allowedStores = _storeService.GetAvailableStoresforUserId(custid, userid);
            ViewBag.allowedStores = allowedStores;
            ViewBag.allowedCategory = currentItemList.Select(x => x.Item.Category).Distinct().ToList();
            ViewBag.Items = currentItemList.Select(x => x.Item).Distinct().ToList();
            currentItemList = _orderService.GetCurrentItemInventoryMstForStoreandItemandCategory(custid, userid, SelectedstoreIds, Categories, Items);
            return View("CreateOrder", currentItemList);
        }

        [AdminApiAuthorization]
        public ActionResult OrderInformation(long orderheaderid)
        {
            OrderHeader orderHeader = _orderService.GetOrderHeader(orderheaderid);
            List<SoApproveHeader> sos = new List<SoApproveHeader>();
            List<InvoiceHeader> invoices = new List<InvoiceHeader>();
            List<FullfillmentHeader> Fullfillments = new List<FullfillmentHeader>();
            if (orderHeader != null)
            {
                sos = _orderService.GetSoForOrder(orderHeader.Finilizeorderno + orderHeader.Id);
            }
            if (sos != null && sos.Count() > 0)
            {
                List<string> sonumbers = sos.Select(x => x.SO_Number).Distinct().ToList();
                invoices = _orderService.GetInvoicesforSos(sonumbers);
                Fullfillments = _orderService.GetFullfillmentsforSos(sonumbers);
            }
            ViewBag.Orders = orderHeader;
            ViewBag.sos = sos;
            ViewBag.invoices = invoices;
            ViewBag.Fullfillments = Fullfillments;
            return View("_OrderInformation");
        }

        //[AdminAuthorization]
        public ActionResult POD(string InvoiceNo, string VehicleNo, int invoiceDId, int PODHeaderId)
        {
            InvoiceHeader invoice = _orderService.GetInvoiceInformation(InvoiceNo);
            var Reasons = _orderService.GetReasonlist("BO");
            ViewBag.Resons = new SelectList(Reasons.ToList(), "Id", "Reason", null);
            ViewBag.InvoiceDispatchId = invoiceDId;
            ViewBag.PODHeaderId = PODHeaderId;
            ViewBag.InvoiceNo = InvoiceNo;
            ViewBag.VehicleNo = VehicleNo;
            return View(invoice);
        }


        public ActionResult PODList(string VehicleNo)
        {
            ViewBag.VehicleNo = VehicleNo;
            return View();
        }

        public ActionResult DownloadFormat(string filename)
        {
            string filepath = Server.MapPath("~/Uploads/" + filename);
            try
            {
                if (System.IO.File.Exists(filepath))
                {
                    return File(filepath, "application/vnd.ms-excel");
                }
                else
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }
        }

        [AdminAuthorization]
        public ActionResult UploadVehicleDisbursement()
        {
            var LoggedInCustomer = SessionValues.LoggedInCustId;
            return View();
        }

        [HttpPost]
        [AdminApiAuthorization]
        public ActionResult ImportVehicleDisbursement()
        {
            try
            {
                if (Request.Files["FileUpload1"].ContentLength > 0)
                {
                    HttpPostedFileBase file = Request.Files["FileUpload1"];
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        var imports = new List<VehicleDispatchExcel>();
                        //
                        var LoggedInCustomer = Convert.ToInt32(SessionValues.LoggedInCustId);

                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.FirstOrDefault();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;
                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                if (workSheet.Cells[rowIterator, 1].Value != null && workSheet.Cells[rowIterator, 2].Value != null)
                                {
                                    var user = new Users();
                                    var import = new VehicleDispatchExcel();
                                    import.DocNo = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    import.VehicleNo = workSheet.Cells[rowIterator, 2].Value != null ? workSheet.Cells[rowIterator, 2].Value.ToString() : "";
                                    imports.Add(import);
                                }
                            }

                        }

                        var im = imports
                                .GroupBy(l => new { l.DocNo, l.VehicleNo })
                                .Select(cl => new VehicleDispatchExcel
                                {
                                    DocNo = cl.First().DocNo,
                                    VehicleNo = cl.First().VehicleNo,
                                    Error = cl.First().Error

                                }).OrderBy(x => x.DocNo).ToList();
                        ViewBag.FinilizeOrder = im;
                        return View("UploadVehicleDisbursement");
                    }

                }
            }
            catch (Exception Ex)
            {
                HelperCls.DebugLog("StackTrace: " + Ex.Message + " \n StackTrace: " + Ex.StackTrace);
                throw (Ex);
            }

            return View("UploadVehicleDisbursement");
        }

        [HttpPost]
        [AdminApiAuthorization]
        public ActionResult SaveVehicleDisbursementList(List<VehicleDispatchExcel> vehicleDisbursementList)
        {
            try
            {
                if (vehicleDisbursementList.Count > 0)
                {
                    var data = from a in vehicleDisbursementList
                               where a.Error != null && a.Error != ""
                               select a;
                    var data1 = data.ToList();
                    var Importlist = new List<VehicleDispatchExcel>();
                    foreach (var docNo in data1)
                    {
                        var remove = from b in vehicleDisbursementList
                                     where b.DocNo == docNo.DocNo && b.VehicleNo == docNo.VehicleNo
                                     select b;
                        var remove1 = remove.ToList();
                        foreach (VehicleDispatchExcel importToExcel in remove1)
                        {
                            vehicleDisbursementList.Remove(importToExcel);
                        }
                    }
                    int userid = SessionValues.UserId;
                    foreach (VehicleDispatchExcel invoice in vehicleDisbursementList)
                    {
                        InvoiceDispatch invoiceDispatch = new InvoiceDispatch();
                        invoiceDispatch.CreatedBy = userid;
                        invoiceDispatch.InvoiceNo = invoice.DocNo;
                        invoiceDispatch.VehicleNo = invoice.VehicleNo;
                        invoiceDispatch.CreatedDate = DateTime.Now;

                        long SheetId = _orderService.AddInvoiceDispatch(invoiceDispatch);
                        if (SheetId == 0)
                            return Json(false, JsonRequestBehavior.AllowGet);
                    }

                    var auditJavascript = new JavaScriptSerializer();
                    var orderAudit = auditJavascript.Serialize(vehicleDisbursementList);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), orderAudit, Request.Url.OriginalString.ToString(), userid, null);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception Ex)
            {
                HelperCls.DebugLog("StackTrace: " + Ex.Message + " \n StackTrace: " + Ex.StackTrace);
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AutoVehicleNo(string search)
        {
            List<Vehicle> vehicles = _orderService.GetAutoVehicles(search, Convert.ToInt32(SessionValues.LoggedInCustId));
            List<VehicleSuggesion> vehicleSuggesions;
            if (search == null || search == "")
                vehicleSuggesions = (from veh in vehicles
                                     select new VehicleSuggesion { data = veh.VehicleNo, value = veh.VehicleNo }).ToList();
            else
            {
                vehicleSuggesions = (from veh in vehicles
                                     where veh.VehicleNo.Contains(search)
                                     select new VehicleSuggesion { data = veh.VehicleNo, value = veh.VehicleNo }).ToList();
            }
            var suggesions = new VehicleSuggesions() { query = search, suggestions = vehicleSuggesions };
            return Json(vehicleSuggesions, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorization]
        public ActionResult UploadOrderExcel()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UploadOrderExcel(HttpPostedFileBase file)
        {
            var LoggedInCustomer = Convert.ToInt32(SessionValues.LoggedInCustId);
            List<string> ErrorList = new List<string>();
            var LoggedInUser = SessionValues.UserId;
            if (Request.Files["file"].ContentLength > 0)
            {
                try
                {
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        var fileSize = file.ContentLength;
                        int RowsInserted = 0;
                        if (fileSize > GlobalValues.MegaBytes)
                        {
                            string err = "GENERAL EXCEPTION - LARGE FILE | DATA UPLOAD FAILED";
                            ModelState.AddModelError("", err);
                            return View();
                        }
                        else
                        {
                            string _FileName = Path.GetFileName(file.FileName);
                            _FileName = LoggedInUser + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + "_" + _FileName;
                            string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), "" + _FileName);

                            byte[] fileBytes = new byte[file.ContentLength];
                            var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                            using (var package = new ExcelPackage(file.InputStream))
                            {
                                var currentSheet = package.Workbook.Worksheets;
                                var workSheet = currentSheet["Sheet1"] != null ? currentSheet["Sheet1"] : currentSheet.First();
                                var noOfCol = workSheet.Dimension.End.Column;
                                OrderExcel orderExcel = new OrderExcel();
                                OrderFormatExcel excelmapping = _orderService.GetUploadOrderFormat(LoggedInCustomer, "ORDER");
                                if (excelmapping != null)
                                {
                                    for (int i = 1; i <= workSheet.Dimension.End.Column; i++)
                                    {
                                        var firstRowCell = workSheet.Cells[1, i];
                                        var HeaderName = firstRowCell.Value != null ? firstRowCell.Value.ToString() : "";
                                        if (excelmapping.ExcelHeaders.Exists(x => x.HeaderName == HeaderName))
                                        {
                                            string ParameterName = excelmapping.ExcelHeaders.Where(x => x.HeaderName == firstRowCell.Value.ToString()).FirstOrDefault().ParameterName;
                                            if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Store_Order_Date).Trim().ToUpper())
                                            {
                                                orderExcel.Store_Order_Date_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Cust_Order_Date).Trim().ToUpper())
                                            {
                                                orderExcel.Cust_Order_Date_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Order_Type).Trim().ToUpper())
                                            {
                                                orderExcel.Order_Type_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Store_Code).Trim().ToUpper())
                                            {
                                                orderExcel.Store_Code_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.City).Trim().ToUpper())
                                            {
                                                orderExcel.City_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.State).Trim().ToUpper())
                                            {
                                                orderExcel.State_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Last_Order_Date).Trim().ToUpper())
                                            {
                                                orderExcel.Last_Order_Date_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Last_Delivery_Date).Trim().ToUpper())
                                            {
                                                orderExcel.Last_Delivery_Date_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Order_Reference_Id).Trim().ToUpper())
                                            {
                                                orderExcel.Order_Reference_Id_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Unique_Reference_Id).Trim().ToUpper())
                                            {
                                                orderExcel.Unique_Reference_Id_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Item_Code).Trim().ToUpper())
                                            {
                                                orderExcel.Item_Code_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Item_Desc).Trim().ToUpper())
                                            {
                                                orderExcel.Item_Desc_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Item_Type).Trim().ToUpper())
                                            {
                                                orderExcel.Item_Type_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Item_Sub_Type).Trim().ToUpper())
                                            {
                                                orderExcel.Item_Sub_Type_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Item_Category).Trim().ToUpper())
                                            {
                                                orderExcel.Item_Category_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Pack_Size).Trim().ToUpper())
                                            {
                                                orderExcel.Pack_Size_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Case_Size).Trim().ToUpper())
                                            {
                                                orderExcel.Case_Size_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Ordered_Qty).Trim().ToUpper())
                                            {
                                                orderExcel.Ordered_Qty_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Number_Of_Cases).Trim().ToUpper())
                                            {
                                                orderExcel.Number_Of_Cases_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Source_Dc).Trim().ToUpper())
                                            {
                                                orderExcel.Source_Dc_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Priority).Trim().ToUpper())
                                            {
                                                orderExcel.Priority_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.PONumber).Trim().ToUpper())
                                            {
                                                orderExcel.PONumber_Id = i;
                                            }
                                        }
                                    }
                                }

                                var noOfRow = workSheet.Dimension.End.Row;
                                if (orderExcel.Store_Order_Date_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Store_Order_Date)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Cust_Order_Date_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Cust_Order_Date)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Order_Type_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Order_Type)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Store_Code_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Store_Code)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.City_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.City)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.State_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.State)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Last_Order_Date_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Last_Order_Date)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Last_Delivery_Date_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Last_Delivery_Date)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Order_Reference_Id_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Order_Reference_Id)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Unique_Reference_Id_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Unique_Reference_Id)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Item_Code_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Item_Code)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Item_Desc_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Item_Desc)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Item_Type_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Item_Type)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Item_Sub_Type_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Item_Sub_Type)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Item_Category_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Item_Category)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Pack_Size_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Pack_Size)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Case_Size_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Case_Size)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Ordered_Qty_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Ordered_Qty)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Number_Of_Cases_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Number_Of_Cases)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Source_Dc_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Source_Dc)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Priority_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Priority)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.PONumber_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.PONumber)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (ErrorList.Count == 0)
                                {
                                    if (!Directory.Exists(Server.MapPath("~/UploadedFiles")))
                                    {
                                        Directory.CreateDirectory(Server.MapPath("~/UploadedFiles"));
                                    }
                                    file.SaveAs(_path);
                                    UploadedFilesMst mst = new UploadedFilesMst();
                                    mst.CustId = LoggedInCustomer;
                                    mst.Upload_By = LoggedInUser;
                                    mst.User_FileName = fileName;
                                    mst.Filepath = _path;
                                    mst.System_FileName = _FileName;
                                    mst.FileType = "ORDER";
                                    long FileId = _orderService.SaveUploadedFile(mst);
                                    List<OrderExcel> lstOrderExcel = new List<OrderExcel>();
                                    for (int rowIterator = 2; rowIterator <= workSheet.Dimension.End.Row; rowIterator++)
                                    {
                                        var user = new Users();
                                        var import = new OrderExcel();
                                        import.Error = "";
                                        DateTime? date;
                                        try
                                        {
                                            if (workSheet.Cells[rowIterator, orderExcel.Store_Order_Date_Id].Value != null)
                                            {
                                                date = workSheet.Cells[rowIterator, orderExcel.Store_Order_Date_Id].Value != null ? HelperCls.FromExcelStringDate(workSheet.Cells[rowIterator, orderExcel.Store_Order_Date_Id].Value.ToString()) : (Nullable<DateTime>)null;
                                            }
                                            else
                                            {
                                                import.Error += "Error at Row: " + rowIterator + ", Column: " + orderExcel.Store_Order_Date_Id + "Unable to Convert the Date.";
                                                date = null;
                                            }
                                            import.Store_Order_Date = date;
                                        }
                                        catch (Exception ex)
                                        {
                                            import.Error += "Error at Row: " + rowIterator + ", column: " + orderExcel.Store_Order_Date_Id + "Unable to Convert the Date.";
                                            HelperCls.DebugLog("Error Date: " + workSheet.Cells[rowIterator, orderExcel.Store_Order_Date_Id].Value + ex.Message + System.Environment.NewLine + ex.StackTrace);
                                            import.Error += ex.Message;
                                        }
                                        DateTime? date2;
                                        try
                                        {
                                            if (workSheet.Cells[rowIterator, orderExcel.Cust_Order_Date_Id].Value != null)
                                            {
                                                date2 = workSheet.Cells[rowIterator, orderExcel.Cust_Order_Date_Id].Value != null ? HelperCls.FromExcelStringDate(workSheet.Cells[rowIterator, orderExcel.Cust_Order_Date_Id].Value.ToString()) : (Nullable<DateTime>)null;
                                            }
                                            else
                                            {
                                                import.Error += "Error at Row: " + rowIterator + ", Column: " + orderExcel.Cust_Order_Date_Id + "Unable to Convert the Date.";
                                                date2 = null;
                                            }
                                            import.Cust_Order_Date = date2;
                                        }
                                        catch (Exception ex)
                                        {
                                            import.Error += "Error at Row: " + rowIterator + ", column: " + orderExcel.Cust_Order_Date_Id + "Unable to Convert the Date.";
                                            HelperCls.DebugLog("Error Date: " + workSheet.Cells[rowIterator, orderExcel.Cust_Order_Date_Id].Value + ex.Message + System.Environment.NewLine + ex.StackTrace);
                                            import.Error += ex.Message;
                                        }
                                        import.Order_Type = workSheet.Cells[rowIterator, orderExcel.Order_Type_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Order_Type_Id].Value.ToString() : "";
                                        import.Store_Code = workSheet.Cells[rowIterator, orderExcel.Store_Code_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Store_Code_Id].Value.ToString() : "";
                                        import.State = workSheet.Cells[rowIterator, orderExcel.State_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.State_Id].Value.ToString() : "";
                                        import.Last_Order_Date = workSheet.Cells[rowIterator, orderExcel.Last_Order_Date_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Last_Order_Date_Id].Value.ToString() : "";
                                        import.Last_Delivery_Date = workSheet.Cells[rowIterator, orderExcel.Last_Delivery_Date_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Last_Delivery_Date_Id].Value.ToString() : "";
                                        import.City = workSheet.Cells[rowIterator, orderExcel.City_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.City_Id].Value.ToString() : "";
                                        import.Order_Reference_Id = workSheet.Cells[rowIterator, orderExcel.Order_Reference_Id_Id].Text != null ? workSheet.Cells[rowIterator, orderExcel.Order_Reference_Id_Id].Text.ToString() : "";
                                        import.Unique_Reference_Id = workSheet.Cells[rowIterator, orderExcel.Unique_Reference_Id_Id].Text != null ? workSheet.Cells[rowIterator, orderExcel.Unique_Reference_Id_Id].Text.ToString() : "";
                                        import.Item_Code = workSheet.Cells[rowIterator, orderExcel.Item_Code_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Item_Code_Id].Value.ToString() : "";
                                        import.Item_Desc = workSheet.Cells[rowIterator, orderExcel.Item_Desc_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Item_Desc_Id].Value.ToString() : "";
                                        import.Item_Type = workSheet.Cells[rowIterator, orderExcel.Item_Type_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Item_Type_Id].Value.ToString() : "";
                                        import.Item_Sub_Type = workSheet.Cells[rowIterator, orderExcel.Item_Sub_Type_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Item_Sub_Type_Id].Value.ToString() : "";
                                        import.Item_Category = workSheet.Cells[rowIterator, orderExcel.Item_Category_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Item_Category_Id].Value.ToString() : "";
                                        import.Pack_Size = workSheet.Cells[rowIterator, orderExcel.Pack_Size_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Pack_Size_Id].Value.ToString() : "";
                                        import.Case_Size = workSheet.Cells[rowIterator, orderExcel.Case_Size_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Case_Size_Id].Value.ToString() : "";
                                        import.Ordered_Qty = workSheet.Cells[rowIterator, orderExcel.Ordered_Qty_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Ordered_Qty_Id].Value.ToString() : "";
                                        import.Number_Of_Cases = workSheet.Cells[rowIterator, orderExcel.Number_Of_Cases_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Number_Of_Cases_Id].Value.ToString() : "";
                                        import.Source_Dc = workSheet.Cells[rowIterator, orderExcel.Source_Dc_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Source_Dc_Id].Value.ToString() : "";
                                        import.Priority = workSheet.Cells[rowIterator, orderExcel.Priority_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Priority_Id].Value.ToString() : "";
                                        import.PONumber = workSheet.Cells[rowIterator, orderExcel.PONumber_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.PONumber_Id].Value.ToString() : "";
                                        import.Xls_Line_No = rowIterator;
                                        import.Uploaded_File_Id = FileId;
                                        import.Upload_By = LoggedInUser;
                                        import.CustId = LoggedInCustomer;
                                        lstOrderExcel.Add(import);
                                    }
                                    //------ Add storewise GroupID in all records--
                                    string[] uniqueStores = lstOrderExcel.Select(p => p.Store_Code).Distinct().ToArray();
                                    List<CodeValuePair> lstCodeValuePair = new List<CodeValuePair>();

                                    for (int i = 0; i < uniqueStores.Count(); i++)
                                    {
                                        CodeValuePair obj = new CodeValuePair();
                                        obj.StoreCode = uniqueStores[i];
                                        obj.GroupId = _orderService.GetMaxGroupIDByCustomer(mst.CustId);
                                        lstCodeValuePair.Add(obj);
                                    }


                                    //---------------------------------------------
                                    foreach (var item in lstOrderExcel)
                                    {
                                        item.GroupId = lstCodeValuePair.Where(a => a.StoreCode == item.Store_Code).Select(a => a.GroupId).FirstOrDefault();
                                        long ReturnValue = _orderService.SaveExcelFileData(item);
                                        if (ReturnValue != 0)
                                        {
                                            RowsInserted++;
                                        }
                                    }
                                    ViewBag.RowsCount = noOfRow - 1;
                                    ViewBag.RowsInserted = RowsInserted;
                                    return View();
                                }
                            }
                            //ViewBag.PreOrder = PreOrder;
                            ViewBag.ErrorList = ErrorList;
                            if (ErrorList != null && ErrorList.Count() > 0)
                            {
                                ViewBag.FinilizeOrder = null;
                                ViewBag.PreOrder = null;
                                ViewBag.Error = string.Join(", ", ErrorList.ToArray());
                                foreach (string err in ErrorList)
                                    ModelState.AddModelError("", err);
                            }
                            return View();
                        }
                    }
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    ViewBag.Error = "Invalid File Format";
                    ModelState.AddModelError("", "Invalid File Format");
                    ViewBag.FinilizeOrder = null;
                }
            }
            return View();
        }

        [AdminAuthorization]
        public ActionResult GenerateOrder(int Redirect = 0)
        {
            int CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            int UserId = Convert.ToInt32(SessionValues.UserId);
            var Stores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Order");
            var Items = _itemService.GetSkuForUserCustomer(UserId, CustId, "Order");
            var Brands = _itemService.GetBrandsForUserCustomer(UserId, CustId, "Order");

            var Locations = _storeService.GetCitiesforUserCustomer(UserId, CustId);

            var DCs = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Order");
            //ItemCategories
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(UserId, CustId);
            //ItemType
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(UserId, CustId);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", null);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", null);

            ViewBag.Redirect = Redirect;
            return View();
        }
        
        public ActionResult GenerateOrderExportExcel(DateTime? FromDate, DateTime? ToDate, List<int> SelectedLocations, List<int> SelectedDCs, List<string> SelectedStores, List<int> SelectedTypes, List<int> SelectedCategories, List<string> SelectedSKUs, int? selectedAgeing, int? isSearch, int? FirstCall, string Unique_Reference_Id)
        {
            try
            {
                int CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
                int UserId = Convert.ToInt32(SessionValues.UserId);
                var Brands = _itemService.GetBrandsForUserCustomer(UserId, CustId, "Order");
                var Locations = _storeService.GetCitiesforUserCustomer(UserId, CustId);
                var DCs = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Order");
                var Stores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Order");
                List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(UserId, CustId);
                List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(UserId, CustId);
                var Items = _itemService.GetSkuForUserCustomer(UserId, CustId, "Order");

                // Parameter Defaults
                SelectedTypes = SelectedTypes?.Any() == true && SelectedTypes[0] != 0 ? SelectedTypes : ItemType.Select(x => x.Id).ToList();
                SelectedCategories = SelectedCategories?.Any() == true && SelectedCategories[0] != 0 ? SelectedCategories : itemCategories.Select(x => x.Id).ToList();
                SelectedSKUs = SelectedSKUs?.Any() == true && !string.IsNullOrEmpty(SelectedSKUs[0]) ? SelectedSKUs : new List<string>() { "-1" };
                SelectedLocations = SelectedLocations?.Any() == true && SelectedLocations[0] != 0 ? SelectedLocations : Locations.Select(x => x.LocationId).ToList();
                SelectedDCs = SelectedDCs?.Any() == true && SelectedDCs[0] != 0 ? SelectedDCs : DCs.Select(x => x.Id).ToList();
                SelectedStores = SelectedStores?.Any() == true && !string.IsNullOrEmpty(SelectedStores[0]) ? SelectedStores : new List<string> { "-1" };

                List<ExcelOrder> finilised = _orderService.GenerateOrder(FromDate, ToDate, CustId, UserId, SelectedStores, SelectedSKUs, SelectedDCs, selectedAgeing, isSearch, FirstCall, SelectedCategories, SelectedTypes, SelectedLocations, Unique_Reference_Id);

                if (finilised == null || !finilised.Any())
                {
                    return Content("No data available for the given criteria.");
                }

                string Title = "Name: Generate Order Report";

                var finilisedList = finilised.Select(head => new
                {
                    head.Id,
                    Store_Order_Date = Convert.ToDateTime(head.Store_Order_Date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    head.PONumber,
                    head.Store_Code,
                    head.Store_Name,
                    head.Item_Code,
                    head.Item_Desc,
                    head.UOM,
                    head.Threshold_Qty,
                    head.Ordered_Qty,
                    head.Priority,
                    head.Checked,
                    head.DCId,
                    head.DCName,
                    head.Category_Id,
                    head.StoreId,
                    head.Uplaoded_By,
                    head.ItemId,
                    head.AutoAllocatedQty,
                    head.InventoryAvailableQty,
                    head.TotalOrderedQty,
                    head.InventoryAvailableStatus,
                    head.ConsumedQty,
                    head.Remark,
                    head.Original_Ordered_Quantity,
                    head.UnitofMasureDescription,
                    head.Case_Size,
                    head.City,
                    head.PlaceOfSupply,
                    head.ItemCategoryType,
                    head.Brand,
                    head.Revised_Ordered_Qty
                });

                using (var excel = new ExcelPackage())
                {
                    var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells[1, 1].Value = Title;
                    workSheet.Cells[1, 1].Style.Font.Bold = true;
                    workSheet.Cells[2, 1].Style.Font.Bold = true;

                    workSheet.Cells[4, 1].Value = "Id";
                    workSheet.Cells[4, 2].Value = "Store Order Date";
                    workSheet.Cells[4, 3].Value = "PONumber";

                    workSheet.Cells[4, 4].Value = "StoreCode";
                    workSheet.Cells[4, 5].Value = "StoreName";
                    workSheet.Cells[4, 6].Value = "Item_Code";
                    workSheet.Cells[4, 7].Value = "Item_Desc";
                    workSheet.Cells[4, 8].Value = "UOM";
                    workSheet.Cells[4, 9].Value = "Threshold_Qty";
                    workSheet.Cells[4, 10].Value = "Ordered_Qty";
                    workSheet.Cells[4, 11].Value = "Priority";
                    workSheet.Cells[4, 12].Value = "Checked";
                    workSheet.Cells[4, 13].Value = "DCID";
                    workSheet.Cells[4, 14].Value = "DCName";
                    workSheet.Cells[4, 15].Value = "Category_Id";
                    workSheet.Cells[4, 16].Value = "StoreId";
                    workSheet.Cells[4, 17].Value = "Uplaoded_By";
                    workSheet.Cells[4, 18].Value = "ItemId";
                    workSheet.Cells[4, 19].Value = "AutoAllocatedQty";
                    workSheet.Cells[4, 20].Value = "InventoryAvailableQty";
                    workSheet.Cells[4, 21].Value = "MinimumOrderQuantity";
                    workSheet.Cells[4, 22].Value = "TotalOrderedQty";
                    workSheet.Cells[4, 23].Value = "Status";
                    workSheet.Cells[4, 24].Value = "ConsumedQty";
                    workSheet.Cells[4, 25].Value = "Remark";
                    workSheet.Cells[4, 26].Value = "Original_Ordered_Quantity";
                    workSheet.Cells[4, 27].Value = "UnitofMasureDescription";
                    workSheet.Cells[4, 28].Value = "Case_Size";
                    workSheet.Cells[4, 28].Value = "City";
                    workSheet.Cells[4, 29].Value = "Place of Supply";
                    workSheet.Cells[4, 30].Value = "ItemCategoryType";
                    workSheet.Cells[4, 31].Value = "Brand";
                    workSheet.Cells[4, 32].Value = "Revised_Ordered_Qty";


                    using (ExcelRange Rng = workSheet.Cells["A4:AQ4"])
                    {
                        Rng.Style.Font.Bold = true;
                    }

                    workSheet.Cells[5, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                    workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();

                    return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "GenerateOrder.xlsx");
                }
            }
            catch (Exception ex)
            {
                // Log exception
                return Content("An error occurred: " + ex.Message);
            }
        }
        [ValidateAntiForgeryToken]
        [AdminApiAuthorization]
        [HttpPost]
        public ActionResult GetUploadedStoreItems(DateTime? FromDate, DateTime? ToDate, List<int> SelectedLocations, List<int> SelectedDCs, List<string> SelectedStores, List<int> SelectedTypes, List<int> SelectedCategories, List<string> SelectedSKUs, int SelectedAgeing, int isSearch, int FirstCall)
        {
            int CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            int UserId = Convert.ToInt32(SessionValues.UserId);
            var Locations = _storeService.GetCitiesforUserCustomer(UserId, CustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Order");
            var Stores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Order");
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(UserId, CustId);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(UserId, CustId);
            var Items = _itemService.GetSkuForUserCustomer(UserId, CustId, "Order");

            SelectedTypes = SelectedTypes != null && SelectedTypes.Count() > 0 && SelectedTypes[0] != 0 ? SelectedTypes : ItemType.Select(x => x.Id).ToList();
            SelectedCategories = SelectedCategories != null && SelectedCategories.Count() > 0 && SelectedCategories[0] != 0 ? SelectedCategories : itemCategories.Select(x => x.Id).ToList();
            SelectedSKUs = SelectedSKUs != null && SelectedSKUs.Count() > 0 && SelectedSKUs[0] != "" ? SelectedSKUs : new List<string>() { "-1" }; //Items.Select(x => x.Id.ToString()).ToList();
            SelectedLocations = SelectedLocations != null && SelectedLocations.Count() > 0 && SelectedLocations[0] != 0 ? SelectedLocations : Locations.Select(x => x.LocationId).ToList();
            SelectedDCs = SelectedDCs != null && SelectedDCs.Count() > 0 && SelectedDCs[0] != 0 ? SelectedDCs : DCs.Select(x => x.Id).ToList();
            SelectedStores = SelectedStores != null && SelectedStores.Count() > 0 && SelectedStores[0] != "" ? SelectedStores : new List<string> { "-1" };///Stores.Select(x => x.Id.ToString()).ToList();
            List<ExcelOrder> ExcelOrders = _orderService.GetUploadedStoreItems(FromDate, ToDate, CustId, UserId, SelectedStores, SelectedSKUs, SelectedDCs, SelectedAgeing, isSearch, FirstCall, SelectedCategories, SelectedTypes, SelectedLocations);
            JsonResult result = Json(new { data = ExcelOrders }, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
            return result;
            //return Json(new { data = ExcelOrders }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminApiAuthorization]
        public ActionResult SaveUploadedFile(List<OrderExcel> excelOrders, DateTime? FromDate, DateTime? ToDate, List<int> SelectedLocations, List<int> SelectedDCs, List<int> SelectedStores, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedSKUs, int SelectedAgeing, int isSearch)
        {
            bool Loaded = true;
            try
            {
                int CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
                int UserId = Convert.ToInt32(SessionValues.UserId);

                var Locations = _storeService.GetCitiesforUserCustomer(UserId, CustId);
                var DCs = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Order");
                var Stores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Order");

                List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(UserId, CustId);
                List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(UserId, CustId);
                var Items = _itemService.GetSkuForUserCustomer(UserId, CustId, "Order");

                SelectedLocations = SelectedLocations != null && SelectedLocations.Count() > 0 && SelectedLocations[0] != 0 ? SelectedLocations : Locations.Select(x => x.LocationId).ToList();
                SelectedDCs = SelectedDCs != null && SelectedDCs.Count() > 0 && SelectedDCs[0] != 0 ? SelectedDCs : DCs.Select(x => x.Id).ToList();
                SelectedStores = SelectedStores != null && SelectedStores.Count() > 0 && SelectedStores[0] != 0 ? SelectedStores : Stores.Select(x => x.Id).ToList();

                SelectedTypes = SelectedTypes != null && SelectedTypes.Count() > 0 && SelectedTypes[0] != 0 ? SelectedTypes : ItemType.Select(x => x.Id).ToList();
                SelectedCategories = SelectedCategories != null && SelectedCategories.Count() > 0 && SelectedCategories[0] != 0 ? SelectedCategories : itemCategories.Select(x => x.Id).ToList();
                SelectedSKUs = SelectedSKUs != null && SelectedSKUs.Count() > 0 && SelectedSKUs[0] != 0 ? SelectedSKUs : Items.Select(x => x.Id).ToList();
                _orderService.DeletedIgnoredRow(FromDate, ToDate, CustId, UserId, excelOrders, SelectedStores, SelectedSKUs, SelectedAgeing, isSearch, SelectedDCs, SelectedCategories, SelectedTypes, SelectedLocations);
                foreach (OrderExcel order in excelOrders)
                {
                    long ReturnValue = _orderService.SaveUploadedFileToTemp(order, CustId, UserId);
                    if (ReturnValue == 0)
                    {
                        Loaded = false;
                        break;
                    }
                }
                try
                {
                    var auditJavascript = new JavaScriptSerializer();
                    var orderAudit = auditJavascript.Serialize(excelOrders);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), orderAudit, Request.Url.OriginalString.ToString(), UserId, null);
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                ViewBag.Error = ex.Message;
            }
            return Json(new { data = Loaded }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerateCSV(DateTime? FromDate, DateTime? ToDate, int IsRedirected = 0, string URL = "", int Filtered = 0)
        {
            string sd = FromDate.HasValue ? FromDate.Value.Date.ToString("yyyy-MM-dd") : "";
            string ed = ToDate.HasValue ? ToDate.Value.Date.ToString("yyyy-MM-dd") : "";

            int CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            int UserId = Convert.ToInt32(SessionValues.UserId);

            var Locations = _storeService.GetCitiesforUserCustomer(UserId, CustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Order");
            var Stores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Order");

            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(UserId, CustId);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(UserId, CustId);
            var Items = _itemService.GetSkuForUserCustomer(UserId, CustId, "Order");

            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", null);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["IsRedirected"] = IsRedirected;
            ViewData["Filtered"] = Filtered;
            ViewData["URL"] = URL;
            return View();
        }
        [HttpPost]
        public ActionResult GenerateCSV(DateTime? FromDate, DateTime? ToDate, List<string> SelectedStores = null, List<string> SelectedSKUs = null, List<int> SelectedDCs = null, List<int> SelectedCategories = null, int SelectedAgeing = 0, int IsRedirected = 0, string URL = "", int Filtered = 0, List<int> SelectedLocations = null, List<int> SelectedTypes = null)
        {
            string sd = FromDate.HasValue ? FromDate.Value.Date.ToString("yyyy-MM-dd") : "";
            string ed = ToDate.HasValue ? ToDate.Value.Date.ToString("yyyy-MM-dd") : "";

            int CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            int UserId = Convert.ToInt32(SessionValues.UserId);

            List<CityView> Locations = _storeService.GetCitiesforUserCustomer(UserId, CustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Order");
            var Stores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Order");
            if (SelectedLocations != null && SelectedLocations.Count() > 0 && SelectedLocations[0] != 0)
            {
                DCs = _wareHouseService.GetWareHouseForCustomerIdLocations(UserId, CustId, SelectedLocations);
            }
            if (SelectedDCs != null && SelectedDCs.Count() > 0 && SelectedDCs[0] != 0)
            {
                Stores = _storeService.GetStoresForWareHouse(UserId, CustId, SelectedDCs, "Order", SelectedLocations);
            }
            List<CategoryTypeView> CategoryTypeView = _itemService.GetItemTypeByCustomer(UserId, CustId);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(UserId, CustId);
            var Items = _itemService.GetSkuForUserCustomer(UserId, CustId, "Order");
            if (SelectedTypes != null && SelectedTypes.Count() > 0 && SelectedTypes[0] != 0)
            {
                itemCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(UserId, CustId, SelectedTypes);
            }
            if (SelectedCategories != null && SelectedCategories.Count() > 0 && SelectedCategories[0] != 0)
            {
                Items = _itemService.GetSkuForUserCustomerItemCategory(UserId, CustId, SelectedTypes, SelectedCategories, "Order");
            }
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDCs);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.ItemTypes = new MultiSelectList(CategoryTypeView.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["IsRedirected"] = IsRedirected;
            ViewData["Filtered"] = 1;
            ViewData["URL"] = URL;
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GetDataToGenerateCSV(DateTime? FromDate, DateTime? ToDate, List<int> SelectedLocations, List<int> SelectedDCs, List<int> SelectedStores, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedSKUs, int SelectedAgeing = 0, int Filtered = 0)
        {
            int CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            int UserId = Convert.ToInt32(SessionValues.UserId);
            var Locations = _storeService.GetCitiesforUserCustomer(UserId, CustId);
            var Dcs = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Order");
            var Stores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Order");

            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(UserId, CustId);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(UserId, CustId);
            var Items = _itemService.GetSkuForUserCustomer(UserId, CustId, "Order");

            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDCs == null || SelectedDCs.Count() == 0 || SelectedDCs[0] == 0)
                SelectedDCs = SelectedDCs = _wareHouseService.GetWareHouseForCustomerIdLocations(UserId, CustId, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(UserId, CustId, SelectedDCs, "Order", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(UserId, CustId, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(UserId, CustId, SelectedTypes, SelectedCategories, "Order").Select(x => x.Id).ToList();
            List<ExcelOrder> ExcelOrders = _orderService.GetDataToGenerateCSVSearch(FromDate, ToDate, CustId, UserId, SelectedStores, SelectedSKUs, SelectedAgeing, Filtered, SelectedDCs, SelectedCategories, SelectedTypes, SelectedLocations);
            return Json(new { data = ExcelOrders }, JsonRequestBehavior.AllowGet);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [AdminApiAuthorization]
        public void SaveDataToGenerateCSV(DateTime? FromDate, DateTime? ToDate, List<int> SelectedLocations = null, List<int> SelectedDCs = null, List<int> SelectedStores = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null, List<int> SelectedSKUs = null, int SelectedAgeing = 0, int Filtered = 0)
        {
            int CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            int UserId = Convert.ToInt32(SessionValues.UserId);
            var Locations = _storeService.GetCitiesforUserCustomer(UserId, CustId);
            var Dcs = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Order");
            var Stores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Order");

            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(UserId, CustId);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(UserId, CustId);
            var Items = _itemService.GetSkuForUserCustomer(UserId, CustId, "Order");

            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDCs == null || SelectedDCs.Count() == 0 || SelectedDCs[0] == 0)
                SelectedDCs = SelectedDCs = _wareHouseService.GetWareHouseForCustomerIdLocations(UserId, CustId, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(UserId, CustId, SelectedDCs, "Order", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(UserId, CustId, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(UserId, CustId, SelectedTypes, SelectedCategories, "Order").Select(x => x.Id).ToList();
            List<ExcelOrder> ExcelOrders = _orderService.GetDataToGenerateCSVSearch(FromDate, ToDate, CustId, UserId, SelectedStores, SelectedSKUs, SelectedAgeing, Filtered, SelectedDCs, SelectedCategories, SelectedTypes, SelectedLocations);
            if (ExcelOrders != null && ExcelOrders.Count() > 0)
            {
                List<long> OrderHeaderIds = _orderService.SaveGenerateCSV(FromDate, ToDate, CustId, UserId, SelectedStores, SelectedSKUs, SelectedDCs, SelectedAgeing, Filtered, SelectedCategories, SelectedTypes, SelectedLocations);
                StringWriter sw = new StringWriter();
                sw.WriteLine("Order #,Expected Delivery Date,Department,Class,Status,Location,Division,SO Approver Partner,ITEM CODE,ITEM NAME,ORDER QUANTITY,eBizDownloadStatus,Store Code,Indent No,Amount,Indent Date,Location,TRANSACTION TYPE,PLACE OF SUPPLY,CUSTOMER PO NO");
                // You can write sql query according your need  
                var NoW = DateTime.Now;
                if (ExcelOrders != null && ExcelOrders.Count() > 0)
                {
                    //DateTime FromDate1 = ExcelOrders.Select(x => x.Store_Order_Date).Min();
                    //DateTime ToDate1 = ExcelOrders.Select(x => x.Store_Order_Date).Max();
                    //var customers = _orderService.netSuitUploadsExcel2(FromDate1, ToDate1, CustId, UserId);
                    try
                    {
                        var customers = _orderService.NetSuitUploadsExcel_OrderHeaderIds(CustId, UserId, OrderHeaderIds, ExcelOrders, "ORDER");
                        if (customers == null || customers.Count() == 0)
                        {
                            customers = _orderService.NetSuitUploadsExcel_OrderHeaderIds(CustId, UserId, OrderHeaderIds, ExcelOrders, "EXCEL");
                        }
                        _orderService.disableOrderForNetsuiteUpload(customers);
                        foreach (var item in customers)
                        {
                            sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",\"{14}\",\"{15}\",\"{16}\",\"{17}\",\"{18}\",\"{19}\"",
                                 item.Order,
                                 DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                 item.Department,
                                 item.Class,
                                 "Pending Approval",
                                 item.Location,
                                 item.Division,
                                 item.SOApproverPartner,
                                 item.code,
                                 item.desc,
                                 item.qty,
                                 "Edit",
                                 item.Store_Name,
                                 item.PO,
                                 1,
                                 item.PO_Date,
                                 item.Location,
                                 item.Transactiontype,
                                 item.Placeofsupply,
                                 "" + item.PONumber + ""
                            ));
                        }

                    }
                    catch (Exception ex)
                    {
                        HelperCls.DebugLog(ex.Message);
                    }
                }
                var filename = "SalesOrderUpload_" + UserId.ToString() + "_" + NoW.ToString("dd_MM_yyyy HH_mm") + ".csv";
                var csv = new StringBuilder();
                //Suggestion made by KyleMit
                var newLine = string.Format("{0}", sw.ToString());
                csv.AppendLine(newLine);
                string _path = Path.Combine(Server.MapPath("~/CSVFiles/"), "" + filename);
                if (!Directory.Exists(Server.MapPath("~/CSVFiles/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/CSVFiles/"));
                }
                //2019-12-06 Start Added CSVfilesaveto DB
                long key = _orderService.SaveCSVFileDetails(filename, _path, CustId, UserId, NoW);
                //2019-12-06 End
                //after your loop
                try
                {
                    System.IO.File.WriteAllText(_path, csv.ToString());
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                }
                var response = System.Web.HttpContext.Current.Response;
                response.Clear();
                response.ClearHeaders();
                response.BufferOutput = true;
                response.ContentEncoding = Encoding.Unicode;
                response.AddHeader("content-disposition", "attachment;filename=" + filename);
                response.AddHeader("content-disposition", "attachment;filename=" + filename);
                response.ContentType = "text/csv";
                response.Write(sw.ToString());
                response.End();
                //return RedirectToAction("GenerateCSV", "Order");
            }
            //else
            //{
            //    return Content("No Orders Selected.");
            //}
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GenerateCSVExportToExcel(DateTime? FromDate, DateTime? ToDate, List<int> SelectedLocations, List<int> SelectedDCs, List<int> SelectedStores, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedSKUs, int SelectedAgeing = 0, int Filtered = 0)
        {
            int CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            int UserId = Convert.ToInt32(SessionValues.UserId);
            var Locations = _storeService.GetCitiesforUserCustomer(UserId, CustId);
            var Dcs = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Order");
            var Stores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Order");

            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(UserId, CustId);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(UserId, CustId);
            var Items = _itemService.GetSkuForUserCustomer(UserId, CustId, "Order");

            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDCs == null || SelectedDCs.Count() == 0 || SelectedDCs[0] == 0)
                SelectedDCs = SelectedDCs = _wareHouseService.GetWareHouseForCustomerIdLocations(UserId, CustId, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(UserId, CustId, SelectedDCs, "Order", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(UserId, CustId, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(UserId, CustId, SelectedTypes, SelectedCategories, "Order").Select(x => x.Id).ToList();
            List<ExcelOrder> ExcelOrders = _orderService.GetDataToGenerateCSVSearch(FromDate, ToDate, CustId, UserId, SelectedStores, SelectedSKUs, SelectedAgeing, Filtered, SelectedDCs, SelectedCategories, SelectedTypes, SelectedLocations);
            //return Json(new { data = ExcelOrders }, JsonRequestBehavior.AllowGet);
            string Title = "Name: View Summary Report";
            //string Message = "Store Order Date: From: " + startDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " To: " + endDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "";

            var finilisedList = ExcelOrders.Select(head => new
            {
                head.Id,
              //head.CustId,
                Store_Order_Date = Convert.ToDateTime(head.Store_Order_Date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                //head.CustId,
                head.Store_Code,
                head.Store_Name,
                head.Item_Code,
                head.Item_Desc,
                head.UOM,
                head.Threshold_Qty,
                head.Ordered_Qty,

                head.Priority,
                head.Checked,
                head.DCId,
                head.DCName,
                head.ItemTypeId,
                // head.ToDate,
                head.Uplaoded_By,
                head.ItemId,

                head.Case_Size,
                head.UnitofMasureDescription,
                head.MinimumOrderQuantity,
                head.PONumber,
                //head.Unique_Reference_Id
            });
            //ExportToExcel(finilised);
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].Value = Title;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                //workSheet.Cells[2, 1].Value = Message;
                workSheet.Cells[2, 1].Style.Font.Bold = true;
                string DateCellFormat = "dd/mm/yyyy";

                workSheet.Cells[4, 1].Value = "Id";
                workSheet.Cells[4, 2].Value = "Store Order Date";

                workSheet.Cells[4, 3].Value = "StoreCode";
                workSheet.Cells[4, 4].Value = "StoreName";
                workSheet.Cells[4, 5].Value = "Item_Code";
                workSheet.Cells[4, 6].Value = "Item_Desc";
                workSheet.Cells[4, 7].Value = "UOM";
                workSheet.Cells[4, 8].Value = "Threshold_Qty";
                workSheet.Cells[4, 9].Value = "Ordered_Qty";

                workSheet.Cells[4, 10].Value = "Priority";
                workSheet.Cells[4, 11].Value = "Checked";
                workSheet.Cells[4, 12].Value = "DCID";
                workSheet.Cells[4, 13].Value = "DCName";

                workSheet.Cells[4, 14].Value = "ItemTypeId";

                workSheet.Cells[4, 15].Value = "Uplaoded_By";

                workSheet.Cells[4, 16].Value = "ItemId";
                workSheet.Cells[4, 17].Value = "Case_Size";
                //workSheet.Cells[4, 18].Value = "UnitofMasureDescription";
                //workSheet.Cells[4, 19].Value = "MinimumOrderQuantity";
                //workSheet.Cells[4, 20].Value = "PONumber";

                using (ExcelRange Rng = workSheet.Cells["A4:AQ4"])
                {
                    Rng.Style.Font.Bold = true;
                }
                workSheet.Cells[5, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                using (ExcelRange Rng = workSheet.Cells["C:C"])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }
                using (ExcelRange Rng = workSheet.Cells["Q:R"])
                {
                    Rng.Style.Numberformat.Format = "#";
                }
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ViewSummaryReport.xlsx");
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        [AdminApiAuthorization]
        public ActionResult RemoveFromViewCSVList(List<int> ExcelOrderIds)
        {
            int CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            int UserId = Convert.ToInt32(SessionValues.UserId);
            long ReturnValue = _orderService.RemoveFromViewCSVList(ExcelOrderIds, CustId, UserId);
            return Json(new { data = ReturnValue }, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorization]
        public ActionResult UploadedFiles()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetUploadedFileDetails(List<string> FileType)
        {
            int CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            int UserId = Convert.ToInt32(SessionValues.UserId);
            List<UploadedFilesView> ExcelOrders = _orderService.GetUploadedFileDetails(CustId, UserId, FileType);
            return Json(new { data = ExcelOrders }, JsonRequestBehavior.AllowGet);
        }
        [AdminApiAuthorization]
        public ActionResult ErrorLog(int FileId, string FileType)
        {
            int CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            int UserId = Convert.ToInt32(SessionValues.UserId);
            List<File_Error_LogsView> error_Logs = _orderService.GetFileErrors(CustId, UserId, FileId, FileType);
            MemoryStream memoryStream = new MemoryStream();
            TextWriter tw = new StreamWriter(memoryStream);
            foreach (File_Error_LogsView err in error_Logs)
            {
                tw.WriteLine(string.Format("{0}", err.Error_Desc));
            }
            tw.Flush();
            tw.Close();
            return File(memoryStream.GetBuffer(), "text/plain", "ERRORS.txt");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminApiAuthorization]
        public ActionResult GetDataQuantity(DateTime? FromDate, DateTime? ToDate, List<string> SelectedStores, List<string> SelectedSKUs, List<int> SelectedDCs, int SelectedAgeing = 0, int Filtered = 0)
        {
            int CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            int UserId = Convert.ToInt32(SessionValues.UserId);
            var Stores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Order");
            var Items = _itemService.GetSkuForUserCustomer(UserId, CustId, "Order");
            var Dcs = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Order");
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == "")
                SelectedStores = Stores.Select(x => x.StoreCode).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == "")
                SelectedSKUs = Items.Select(x => x.SKUCode).ToList();
            if (SelectedDCs == null || SelectedDCs.Count() == 0 || SelectedDCs[0] == 0)
                SelectedDCs = Dcs.Select(x => x.Id).ToList();
            List<ExcelOrder> ExcelOrders = _orderService.GetDataQuantity(CustId, UserId, FromDate, ToDate, SelectedStores, SelectedSKUs, SelectedDCs, SelectedAgeing, Filtered);
            return Json(new { data = ExcelOrders }, JsonRequestBehavior.AllowGet);
        }

        #region POD
        [HttpPost]
        public ActionResult GetInvoiceList(string vehicleNo, int? custid)
        {
            List<PODListData> invoices = _orderService.GetInvoicesforVehicle(vehicleNo, custid.HasValue ? custid.Value : 0);
            return Json(new { data = invoices }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPODImages(string InvoiceNo)
        {
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            string uniqueReferenceID = "";
            List<PODImageList> obj = new List<PODImageList>();
            obj = _reportService.GetPODImages(uniqueReferenceID, InvoiceNo, custid);
            string InvoicePath = GetInvoiceMonthYear(InvoiceNo);
            InvoicePath = InvoicePath + '/' + InvoiceNo;
            string ImageName = "";
            for (int i = 0; i < obj.Count(); i++)
            {
                if (obj[i].ImgName.Contains("Sign"))
                {
                    ImageName = obj[i].ImgName;
                    break;
                }
            }
            string FinalPath = Server.MapPath("~/PODImages/" + InvoicePath + "/" + ImageName);

            string base64String = null;
            try
            {
                using (System.Drawing.Image image = System.Drawing.Image.FromFile(FinalPath))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();
                        base64String = Convert.ToBase64String(imageBytes);
                    }
                }
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
            return Json(new { data = base64String }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetStoreName(string invoiceVal)
        {
            List<storeBO> SBO = _orderService.GetStoreName(invoiceVal);
            return Json(new { data = SBO }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddUpdatePODHeader(PODheaderBO pODheaderBO)
        {
            int userid;
            var id = SessionValues.UserId;
            if (id == 0)
            {
                userid = 0;
            }
            else
            {
                userid = Convert.ToInt32(SessionValues.UserId);
            }
            pODheaderBO.CreatedBy = userid;
            long i = _orderService.AddUpdatePODH(pODheaderBO);
            return Json(new { data = i }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetReasonList(string categoryid)
        {
            List<CategoryWiseReasonMaster> reasonlist = _orderService.GetReasonlist(categoryid);
            return Json(new { data = reasonlist }, JsonRequestBehavior.AllowGet);
        }
        #region  POD Image Name
        public ActionResult PODDetails(string InvoiceNo, List<PODResponse> InvoiceDetails, int PODHeaderId, int InvoiceDispatchId)
        {
            bool cond = false;
            try
            {

                if (InvoiceDetails.Count() > 0 && InvoiceNo != "" && InvoiceNo != null)
                {

                    int str = 0;
                    foreach (PODResponse save in InvoiceDetails.ToList())
                    {
                        if (save.DispatchQty == save.DeliveredQty)
                        {
                            str = 1;
                        }
                        else if ((save.DispatchQty > save.DeliveredQty) && (save.DeliveredQty > 0))
                        {
                            str = 3;
                        }
                        else if ((save.DispatchQty > save.DeliveredQty) && (save.DeliveredQty == 0))
                        {
                            str = 2;
                        }
                        PODDetail PODDetail = new PODDetail();
                        PODDetail.Item = save.ItemCode;
                        PODDetail.InvoiceQuantity = save.DispatchQty;
                        PODDetail.DeliveredQuantity = save.DeliveredQty;
                        PODDetail.PODHeaderId = PODHeaderId;
                        PODDetail.Reason = save.Reason;
                        PODDetail.Status = str;
                        long pk_orderDetail = _orderService.savePODDetailResponse(PODDetail, InvoiceDispatchId);

                    }
                }
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
            return Json(cond, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SavePODImages(string InvoiceNo, int PODHeaderId, List<PODImageList> PODImageLists, string SignatureDataUrl)
        {
            bool cond = false;
            try
            {
                // Get invoice No
                string InvoiceMonthYear = GetInvoiceMonthYear(InvoiceNo);
                string pathMonthFolder = Server.MapPath("~/PODImages/" + InvoiceMonthYear + "/");

                if (!Directory.Exists(pathMonthFolder))
                    Directory.CreateDirectory(pathMonthFolder);

                string dirUrl = InvoiceNo;
                if (SignatureDataUrl != null)
                {

                    string path = Server.MapPath("~/PODImages/" + InvoiceMonthYear + "/" + dirUrl + "/");
                    byte[] buffer = Convert.FromBase64String(SignatureDataUrl.Split(',')[1]);
                    MemoryStream memoryStream = new MemoryStream(buffer, 0, buffer.Length);
                    memoryStream.Write(buffer, 0, buffer.Length);
                    Image image = Image.FromStream((Stream)memoryStream, true);
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "Sign" + ".jpg";
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    image.Save(path + fileName);
                    PODImages PODImages = new PODImages();
                    PODImages.PODHeaderId = PODHeaderId;
                    PODImages.FileName = (fileName);
                    PODImages.Type = "Signature";
                    PODImages.InvoiceNo = InvoiceNo;
                    long pk_PODImg = _orderService.SavePODImg(PODImages);
                }
                if (PODImageLists.Count() > 0)
                {

                    for (int i = 0; i < PODImageLists.Count; i++)
                    {
                        PODImageList item = PODImageLists.ElementAt(i);
                        string path = Server.MapPath("~/PODImages/" + InvoiceMonthYear + "/");
                        path = path + "/" + dirUrl + "/";
                        byte[] buffer = Convert.FromBase64String(item.ImgName.Split(',')[1]);
                        MemoryStream memoryStream = new MemoryStream(buffer, 0, buffer.Length);
                        memoryStream.Write(buffer, 0, buffer.Length);
                        Image image = Image.FromStream((Stream)memoryStream, true);
                        string tempstr;
                        if (i == 0)
                        {
                            tempstr = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                        }
                        else
                        {
                            tempstr = DateTime.Now.ToString("yyyyMMddHHmmss") + "(" + i + ")" + ".jpg";
                        }
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        image.Save(path + tempstr);
                        PODImages PODImages = new PODImages();
                        PODImages.PODHeaderId = PODHeaderId;
                        PODImages.FileName = (tempstr);
                        PODImages.Type = "Invoice";
                        PODImages.InvoiceNo = InvoiceNo;
                        long pk_PODImg = _orderService.SavePODImg(PODImages);
                    }

                }


            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
            return Json(cond, JsonRequestBehavior.AllowGet);
        }

        public string GetInvoiceMonthYear(string InvoiceNo)
        {
            InvoiceHeader objInvoiceHeader = _orderService.GetInvoiceInformation(InvoiceNo);
            DateTime InvoiceDate = DateTime.ParseExact(objInvoiceHeader.InvoiceDate, "dd/MM/yyyy", null);
            string InvoiceMonth = InvoiceDate.ToString("MMM", CultureInfo.InvariantCulture).ToUpper();
            string InvoiceYear = InvoiceDate.Year.ToString();
            return InvoiceMonth + InvoiceYear;
        }

        #endregion
        #endregion

        #region Tracking
        public ActionResult TrackingStatusList()
        {

            return View();
        }
        [HttpPost]
        public ActionResult GetTrackingList(string action, string uniqueReferenceID, string InvoiceNo, int custid)
        {
            List<TrackingBO> list1 = new List<TrackingBO>();
            if (action == "URID")
            {
                list1 = _orderService.GetTrackingList(action, uniqueReferenceID, InvoiceNo, custid);
                return Json(new { data = list1 }, JsonRequestBehavior.AllowGet);

            }
            if (action == "InvoiceNo")
            {
                list1 = _orderService.GetTrackingList(action, uniqueReferenceID, InvoiceNo, custid);
                return Json(new { data = list1 }, JsonRequestBehavior.AllowGet);
            }
            if (action == "URIDInvoiceNo")
            {
                list1 = _orderService.GetTrackingList(action, uniqueReferenceID, InvoiceNo, custid);
                return Json(new { data = list1 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStatusList(string uniqueReferenceID, string InvoiceNo, int custid)
        {
            List<TrackingBO> obj = new List<TrackingBO>();
            obj = _orderService.GetStatusList(uniqueReferenceID, InvoiceNo, custid);
            return Json(new { data = obj }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region StoreOrderList Widget
        public ActionResult GetLastOrders()
        {
            int StoreID = SessionValues.StoreId;
            int userid = SessionValues.UserId;
            var store = _storeService.GetStoreById(StoreID);
            DateTime Today = DateTime.Now.Date;
            List<StoreOrders> StoreOrders = _orderService.GetLastStoreOrders(store.StoreCode, store.Id, store.CustId, Today);
            return Json(new { data = StoreOrders }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OrderDetails(DateTime StoreOrderDate, long? GroupId, string PONumber)
        {
            int StoreID = SessionValues.StoreId;
            int userid = SessionValues.UserId;
            var store = _storeService.GetStoreById(StoreID);
            List<StoreOrderDetailsVB> storeOrderDetails = _orderService.GetLastStoreOrdersByDateStoreCode(StoreOrderDate, store.StoreCode, store.Id, store.CustId, GroupId, PONumber);
            ViewBag.storeOrderDetailsList = storeOrderDetails;
            return View();
        }
        #endregion


        #region "Order Delete/ Close By Deepa 28/11/2019"
        [AdminAuthorization]
        public ActionResult OrderDeleteClose()
        {
            int userid = Convert.ToInt32(SessionValues.UserId);
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Order");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Order");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Order");
            var ReasonList = _orderService.GetReasonlist("2");
            ViewBag.Resons = new SelectList(ReasonList.ToList(), "Id", "Reason", null);
            DateTime now = DateTime.Now;
            var startDate = now.AddDays(-1).Date;
            var endDate = startDate.AddDays(1).Date;
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = null;
            var selectedDCs = DCs.Select(x => x.Id).ToList();
            var selectedStores = Stores.Select(x => x.Id).ToList();
            var SelectedSKUs = Items.Select(x => x.Id).ToList();
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminApiAuthorization]
        public ActionResult OrderDeleteClose(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, String Action, string hdnIds)
        {
            int userid = Convert.ToInt32(SessionValues.UserId);
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Order");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Order");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Order");
            var ReasonList = _orderService.GetReasonlist(Action);
            ViewBag.Resons = new SelectList(ReasonList.ToList(), "Id", "Reason", null);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = UniqueReferenceID;
            SelectedDC = SelectedDC != null && SelectedDC.Count() > 0 ? SelectedDC : DCs.Select(x => x.Id).ToList();
            SelectedStores = SelectedStores != null && SelectedStores.Count() > 0 ? SelectedStores : Stores.Select(x => x.Id).ToList();
            SelectedSKUs = SelectedSKUs != null && SelectedSKUs.Count() > 0 ? SelectedSKUs : Items.Select(x => x.Id).ToList();
            List<OrderDeleteClose> finilised = _orderService.OrderDeleteCloseSearch(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, Action);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.FinalizedReports = finilised.ToList();
            return View();
        }

        [HttpPost]
        [CustomApiAuthorization]
        public JsonResult OrderDeleteCloseItems(List<OrderDeleteItems> Items, string Action)
        {
            try
            {
                int userid = Convert.ToInt32(SessionValues.UserId);
                var affected = _orderService.OrderDeleteClose(Items, Action, userid);
                bool cond = affected != "" ? true : false;
                return Json(cond, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        #endregion
        #region CSVList
        public ActionResult CSVLists()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetCSVFileDetails(DateTime FromDate, DateTime ToDate)
        {
            int CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            int UserId = Convert.ToInt32(SessionValues.UserId);
            List<CSVFileMasterVM> ExcelOrders = _orderService.GetCSVFileDetails(CustId, UserId, FromDate, ToDate);
            return Json(new { data = ExcelOrders }, JsonRequestBehavior.AllowGet);
        }
        [AdminApiAuthorization]
        public ActionResult DownloadCSV(string filename)
        {
            string filepath = Server.MapPath("~/CSVFiles/" + filename);
            try
            {
                if (System.IO.File.Exists(filepath))
                {
                    return File(filepath, "application/csv", filename);
                }
                else
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }
        }

        #endregion
        #region GetItemsForSKUCategory
        public ActionResult GetItemsForSKUCategory(List<int> SelectedCategories, List<int> SelectedTypes, string OrderReport)
        {
            int CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            int UserId = SessionValues.UserId;
            int StoreId = 0;

            if (SessionValues.LoggedInCustId != null)
            {
                CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            }
            if (SessionValues.StoreId != 0)
            {
                StoreId = Convert.ToInt32(SessionValues.StoreId.ToString());
                CustId = _storeService.GetStoreById(StoreId).CustId;
            }
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(UserId, CustId, StoreId);
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(UserId, CustId, StoreId);
            if (SelectedTypes != null && SelectedTypes.Count() > 0 && SelectedTypes[0] != 0)
            {
                Itemcategories = _itemService.GetItemCategoriesByCustomerForItemTypes(UserId, CustId, SelectedTypes, StoreId);
            }
            else
            {
                SelectedTypes = ItemType.Select(X => X.Id).ToList();
            }
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(UserId, CustId, StoreId);
            SelectedCategories = SelectedCategories != null && SelectedCategories.Count() > 0 && SelectedCategories[0] != 0 ? SelectedCategories : itemCategories.Select(x => x.Id).ToList();
            List<ItemView> items = _itemService.GetSkuForUserCustomerItemCategory(UserId, CustId, SelectedTypes, SelectedCategories, OrderReport, StoreId);
            return Json(new { data = items.OrderBy(X => X.SKUCode) }, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region GetCatgoriesForItemTypes
        public ActionResult GetCatgoriesForItemTypes(List<int> SelectedItemTypes)
        {
            int CustId = SessionValues.LoggedInCustId.Value;
            int UserId = SessionValues.UserId;
            int StoreId = 0;
            if (SessionValues.StoreId != 0)
            {
                StoreId = SessionValues.StoreId;
                CustId = SessionValues.LoggedInCustId.Value;
            }
            List<CategoryTypeView> itemCategories = _itemService.GetItemTypeByCustomer(UserId, CustId, StoreId);
            SelectedItemTypes = SelectedItemTypes != null && SelectedItemTypes.Count() > 0 && SelectedItemTypes[0] != 0 ? SelectedItemTypes : itemCategories.Select(x => x.Id).ToList();
            List<ItemCategoryView> items = _itemService.GetItemCategoriesByCustomerForItemTypes(UserId, CustId, SelectedItemTypes, StoreId);
            return Json(new { data = items.OrderBy(X => X.Id) }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetCatgoriesForItemTypes
        public ActionResult GetWareHouseDCForLocations(List<int> SelectedLocations = null)
        {
            int CustId = Convert.ToInt32(SessionValues.LoggedInCustId);
            int UserId = SessionValues.UserId;
            int StoreId = 0;
            if (SessionValues.LoggedInCustId != null)
            {
                CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            }
            if (SessionValues.StoreId != 0)
            {
                StoreId = Convert.ToInt32(SessionValues.StoreId.ToString());
                CustId = _storeService.GetStoreById(StoreId).CustId;
            }
            List<CityView> Locations = _storeService.GetCitiesforUserCustomer(UserId, CustId);
            SelectedLocations = SelectedLocations != null && SelectedLocations.Count() > 0 && SelectedLocations[0] != 0 ? SelectedLocations : Locations.Select(x => x.LocationId).ToList();
            List<WareHouseDCView> wareHouseDC = _wareHouseService.GetWareHouseForCustomerIdLocations(UserId, CustId, SelectedLocations, StoreId);
            return Json(new { data = wareHouseDC.OrderBy(X => X.Id) }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region GetCatgoriesForItemTypes
        public ActionResult GetStoresForWareHouse(List<int> SelectedWareHouseDCs, string OrderReport, List<int> SelectedLocations = null)
        {
            int CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            int UserId = SessionValues.UserId;
            int StoreId = 0;//Convert.ToInt32(SessionValues.StoreId.ToString());
            if (SessionValues.LoggedInCustId != null)
            {
                CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            }
            if (SessionValues.StoreId != 0)
            {
                StoreId = Convert.ToInt32(SessionValues.StoreId.ToString());
                CustId = _storeService.GetStoreById(StoreId).CustId;
            }
            List<WareHouseDCView> warehouse = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Order");
            List<CityView> Locations = _storeService.GetCitiesforUserCustomer(UserId, CustId);
            SelectedWareHouseDCs = SelectedWareHouseDCs != null && SelectedWareHouseDCs.Count() > 0 && SelectedWareHouseDCs[0] != 0 ? SelectedWareHouseDCs : warehouse.Select(x => x.Id).ToList();
            SelectedLocations = SelectedLocations != null && SelectedLocations.Count() > 0 && SelectedLocations[0] != 0 ? SelectedLocations : Locations.Select(x => x.LocationId).ToList();
            List<StoreView> wareHouseDC = _storeService.GetStoresForWareHouse(UserId, CustId, SelectedWareHouseDCs, OrderReport, SelectedLocations);
            return Json(new { data = wareHouseDC.OrderBy(X => X.Id) }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public ActionResult GetDraftOrder()
        {
            int StoreID = SessionValues.StoreId;
            int userid = SessionValues.UserId;
            int custid = SessionValues.LoggedInCustId.Value;
            var store = _storeService.GetStoreById(StoreID);
            List<StoreOrderVB> draftOrders = _orderService.GetStoreDraftOrder(StoreID, store.CustId, userid);
            return Json(new { data = draftOrders }, JsonRequestBehavior.AllowGet);
        }

        #region POD Bulk Update
        [AdminAuthorization]
        public ActionResult PODBulkUpdate()
        {
            ViewData["VehicleNo"] = "";
            ViewBag.RecordCount = 1;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            if (Session["InvoiceList"] != null)
                Session.Remove("InvoiceList");

            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult PODBulkUpdate(string vehicleNo, int? custid)
        {
            ViewData["VehicleNo"] = vehicleNo;
            custid = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            List<CategoryWiseReasonMaster> reasonlist = _orderService.GetReasonlist("BO");

            List<PODBulkUpdate> invoices = _orderService.GetInvoiceForBulkUpdateByVehicleNo(vehicleNo, custid.HasValue ? custid.Value : 0);
            for (int i = 0; i < invoices.Count(); i++)
            {
                InvoiceHeader invoice = _orderService.GetInvoiceInformation(invoices[i].Invoice_Number);
                // invoices[i].InvoiceDetails = invoice.InvoiceDetails;
                List<BulkPODResponse> lst = new List<BulkPODResponse>();
                List<InvoiceDetail> dtl = invoice.InvoiceDetails.ToList();
                for (int j = 0; j < dtl.Count(); j++)
                {
                    BulkPODResponse obj = new BulkPODResponse();
                    obj.InvoiceQty = Convert.ToDecimal(dtl[j].Quantity);
                    obj.ItemId = Convert.ToInt32(dtl[j].Id);
                    obj.ItemCode = dtl[j].Item;
                    obj.Description = dtl[j].Description;
                    lst.Add(obj);
                }
                invoices[i].BulkPODResponse = lst;
            }

            Session["InvoiceList"] = invoices;
            ViewBag.InvoiceList = invoices;
            ViewBag.RecordCount = invoices.Count();
            ViewBag.Reasons = new SelectList(reasonlist, "Id", "Reason");
            return View("PODBulkUpdate");
        }

        public ActionResult PODBulkUpdateSKUDetails(string InvoiceDispatchId, string InvoiceNumber, string DeliveryStatus)
        {
            List<PODBulkUpdate> invoices = (List<PODBulkUpdate>)Session["InvoiceList"];
            invoices = invoices.Where(a => a.InvoiceDispatchId == Convert.ToInt32(InvoiceDispatchId) && (a.Invoice_Number == InvoiceNumber)).ToList();
            List<CategoryWiseReasonMaster> reasonlist = _orderService.GetReasonlist("BO");
            ViewBag.Reasons = new SelectList(reasonlist, "Id", "Reason");
            ViewBag.BulkPODResponse = invoices.FirstOrDefault().BulkPODResponse;
            ViewBag.DeliveryStatus = DeliveryStatus;
            ViewBag.InvoiceNumber = InvoiceNumber;
            ViewBag.InvoiceDispatchId = InvoiceDispatchId;
            return PartialView("_PODBulkupdateDetailPartial");
        }
        public JsonResult SavePODBulkUpdate_SKUDetails(List<BulkPODResponse> BulkPODResponse)
        {
            // POPUP Save

            List<PODBulkUpdate> invoices = (List<PODBulkUpdate>)Session["InvoiceList"];
            string InvoiceNo = BulkPODResponse[0].Invoice_number;
            for (int j = 0; j < invoices.Count(); j++)
            {
                if (invoices[j].Invoice_Number == InvoiceNo)
                {
                    invoices[j].BulkPODResponse.Clear();
                    for (int i = 0; i < BulkPODResponse.Count(); i++)
                    {
                        BulkPODResponse[i].IsDataSaved = true;
                        invoices[j].BulkPODResponse.Add(BulkPODResponse[i]);
                    }
                    break;
                }
            }
            Session["InvoiceList"] = invoices;
            return Json(new { data = 1 }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SavePODBulkUpdate(List<PODBulkUpdate> PODBulkUpdate)
        {
            int userid;
            var id = SessionValues.UserId;
            if (id == 0)
            {
                userid = 0;
            }
            else
            {
                userid = SessionValues.UserId;
            }
            long Id = 0;
            for (int i = 0; i < PODBulkUpdate.Count(); i++)
            {
                PODheaderBO pODheaderBO = new PODheaderBO();
                pODheaderBO.CreatedBy = userid;
                pODheaderBO.InvoiceDispatchId = PODBulkUpdate[i].InvoiceDispatchId;
                pODheaderBO.Invoice_Number = PODBulkUpdate[i].Invoice_Number;
                pODheaderBO.DeliveryStatus = PODBulkUpdate[i].DeliveryStatus;
                pODheaderBO.DeliveryDate = PODBulkUpdate[i].DeliveryDate;
                pODheaderBO.Reason = PODBulkUpdate[i].Reason == null ? 0 : Convert.ToInt32(PODBulkUpdate[i].Reason);
                pODheaderBO.Reattempte = 0;
                Id = _orderService.AddUpdatePODH(pODheaderBO);
                //------------------------POD Details for SKU update------------
                if (pODheaderBO.DeliveryStatus == 1 || pODheaderBO.DeliveryStatus == 2)
                {
                    SavePODBulkUpdateForSKUDetails(pODheaderBO, Id);
                }
                else
                {
                    List<PODBulkUpdate> invoices = (List<PODBulkUpdate>)Session["InvoiceList"];
                    invoices = invoices.Where(a => a.Invoice_Number == PODBulkUpdate[i].Invoice_Number).ToList();
                    SavePODBulkUpdateForSKUDetails(invoices.FirstOrDefault().BulkPODResponse.ToList(), Id, Convert.ToInt32(pODheaderBO.InvoiceDispatchId), pODheaderBO.Reason);

                }
                //--------------------------------------------------------------
            }
            return Json(new { data = Id }, JsonRequestBehavior.AllowGet);
        }
        public void SavePODBulkUpdateForSKUDetails(PODheaderBO objPODheaderBO, long PODHeaderId)
        {
            // Auto insert the Details into PODDetailTable for status (Delivered and Not Delivered)
            InvoiceHeader invoice = _orderService.GetInvoiceInformation(objPODheaderBO.Invoice_Number);

            if (invoice.InvoiceDetails.Count() > 0)
            {
                foreach (var item in invoice.InvoiceDetails)
                {
                    PODDetail PODDetail = new PODDetail();
                    PODDetail.Item = item.Item;
                    PODDetail.InvoiceQuantity = Convert.ToDecimal(item.Quantity);
                    if (objPODheaderBO.DeliveryStatus == 1)
                    {
                        PODDetail.DeliveredQuantity = Convert.ToDecimal(item.Quantity);
                        PODDetail.Status = objPODheaderBO.DeliveryStatus;
                        PODDetail.Reason = 0; // No reason
                    }
                    else if (objPODheaderBO.DeliveryStatus == 2)
                    {
                        PODDetail.DeliveredQuantity = 0;
                        PODDetail.Status = objPODheaderBO.DeliveryStatus;
                        PODDetail.Reason = objPODheaderBO.Reason;
                    }

                    PODDetail.PODHeaderId = PODHeaderId;

                    long pk_orderDetail = _orderService.savePODDetailResponse(PODDetail, Convert.ToInt32(objPODheaderBO.InvoiceDispatchId));
                }
            }

        }
        public void SavePODBulkUpdateForSKUDetails(List<BulkPODResponse> BulkPODResponse, long PODHeaderId, int InvoiceDispatchId, int InvoiceReason)
        {
            // For Status=3 Partially delivered
            foreach (var item in BulkPODResponse)
            {
                PODDetail PODDetail = new PODDetail();
                PODDetail.Item = item.ItemCode;
                PODDetail.InvoiceQuantity = Convert.ToDecimal(item.InvoiceQty);
                if (item.IsDataSaved == false)
                {
                    PODDetail.DeliveredQuantity = item.InvoiceQty;
                    PODDetail.Reason = InvoiceReason;
                }
                else
                {
                    PODDetail.DeliveredQuantity = item.DeliveredQty;
                    if (PODDetail.DeliveredQuantity < PODDetail.InvoiceQuantity && item.Reason == null)
                    {
                        PODDetail.Reason = InvoiceReason;
                    }
                    PODDetail.Reason = item.Reason == null ? 0 : Convert.ToInt32(item.Reason);
                }
                PODDetail.PODHeaderId = PODHeaderId;
                PODDetail.Status = 3;
                long pk_orderDetail = _orderService.savePODDetailResponse(PODDetail, InvoiceDispatchId);
            }
        }
        public JsonResult GetInoiceDetails_POD(string InvoiceNo)
        {
            InvoiceHeader invoice = _orderService.GetInvoiceInformation(InvoiceNo);
            return Json(new { data = invoice }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region HaldiRamOrderUploads
        [AdminAuthorization]
        public ActionResult UploadOrder()
        {
            return View();
        }
        [HttpPost]
        [AdminApiAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult UploadOrder(HttpPostedFileBase file)
        {
            var LoggedInCustomer = Convert.ToInt32(SessionValues.LoggedInCustId);
            List<string> ErrorList = new List<string>();
            var LoggedInUser = SessionValues.UserId;
            if (Request.Files["file"].ContentLength > 0)
            {
                try
                {
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        var fileSize = file.ContentLength;
                        int RowsInserted = 0;
                        if (fileSize > GlobalValues.MegaBytes)
                        {
                            string err = "GENERAL EXCEPTION - LARGE FILE | DATA UPLOAD FAILED";
                            ModelState.AddModelError("", err);
                            return View();
                        }
                        else
                        {
                            string _FileName = Path.GetFileName(file.FileName);
                            _FileName = LoggedInUser + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + "_" + _FileName;
                            string ExtentedFilePath = "/Client-" + LoggedInCustomer + "/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MMM") + "/OUTWARD";
                            string _path = Path.Combine(Server.MapPath("~/UploadedFiles" + ExtentedFilePath), "" + _FileName);

                            byte[] fileBytes = new byte[file.ContentLength];
                            var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                            using (var package = new ExcelPackage(file.InputStream))
                            {
                                var currentSheet = package.Workbook.Worksheets;
                                var workSheet = currentSheet["Sheet1"] != null ? currentSheet["Sheet1"] : currentSheet.First();
                                var noOfCol = workSheet.Dimension.End.Column;
                                OrderExcelMapping orderExcel = new OrderExcelMapping();
                                OrderFormatExcel excelmapping = _orderService.GetUploadOrderFormat(LoggedInCustomer, "OUTWARD");
                                if (excelmapping != null)
                                {
                                    for (int i = 1; i <= workSheet.Dimension.End.Column; i++)
                                    {
                                        var firstRowCell = workSheet.Cells[1, i];
                                        var HeaderName = firstRowCell.Value != null ? firstRowCell.Value.ToString().Trim().ToUpper() : "";
                                        if (excelmapping.ExcelHeaders.Exists(x => x.HeaderName.ToUpper() == HeaderName))
                                        {
                                            string ParameterName = excelmapping.ExcelHeaders.Where(x => x.HeaderName.Trim().ToUpper() == firstRowCell.Value.ToString().Trim().ToUpper()).FirstOrDefault().ParameterName;
                                            if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Unique_Order_No).Trim().ToUpper())
                                            {
                                                orderExcel.Unique_Order_No_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Order_Date).Trim().ToUpper())
                                            {
                                                orderExcel.Order_Date_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Store_Code).Trim().ToUpper())
                                            {
                                                orderExcel.Store_Code_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.City).Trim().ToUpper())
                                            {
                                                orderExcel.City_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Source_Dc).Trim().ToUpper())
                                            {
                                                orderExcel.Source_Dc_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Item_Code).Trim().ToUpper())
                                            {
                                                orderExcel.Item_Code_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Ordered_Qty).Trim().ToUpper())
                                            {
                                                orderExcel.Ordered_Qty_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Sales_Order_No).Trim().ToUpper())
                                            {
                                                orderExcel.Sales_Order_No_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.SO_Status).Trim().ToUpper())
                                            {
                                                orderExcel.SO_Status_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Sales_Order_Date).Trim().ToUpper())
                                            {
                                                orderExcel.Sales_Order_Date_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Sales_Order_Qty).Trim().ToUpper())
                                            {
                                                orderExcel.Sales_Order_Qty_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Invoice_No).Trim().ToUpper())
                                            {
                                                orderExcel.Invoice_No_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Invoice_Date).Trim().ToUpper())
                                            {
                                                orderExcel.Invoice_Date_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Invoice_Qty).Trim().ToUpper())
                                            {
                                                orderExcel.Invoice_Qty_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Dispatch_Date).Trim().ToUpper())
                                            {
                                                orderExcel.Dispatch_Date_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Vehicle_No).Trim().ToUpper())
                                            {
                                                orderExcel.Vehicle_No_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Delivery_Status).Trim().ToUpper())
                                            {
                                                orderExcel.Delivery_Status_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Delivery_ReAttempt).Trim().ToUpper())
                                            {
                                                orderExcel.Delivery_ReAttempt_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.POD_Date).Trim().ToUpper())
                                            {
                                                orderExcel.POD_Date_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Undelivered_Reason).Trim().ToUpper())
                                            {
                                                orderExcel.Undelivered_Reason_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Delivered_Qty).Trim().ToUpper())
                                            {
                                                orderExcel.Delivered_Qty_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Is_OnTimeDelivery).Trim().ToUpper())
                                            {
                                                orderExcel.Is_OnTimeDelivery_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.ISR_NO).Trim().ToUpper())
                                            {
                                                orderExcel.ISR_NO_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Actual_Reporting_Time).Trim().ToUpper())
                                            {
                                                orderExcel.Actual_Reporting_Time_Id = i;
                                            }
                                        }
                                    }
                                }

                                var noOfRow = workSheet.Dimension.End.Row;
                                if (orderExcel.Unique_Order_No_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Unique_Order_No)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Order_Date_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Order_Date)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Store_Code_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Store_Code)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.City_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.City)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Source_Dc_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Source_Dc)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Item_Code_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Item_Code)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Ordered_Qty_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Ordered_Qty)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Sales_Order_No_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Sales_Order_No)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.SO_Status_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.SO_Status)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Sales_Order_Date_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Sales_Order_Date)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Sales_Order_Qty_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Sales_Order_Qty)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Invoice_No_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Invoice_No)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Invoice_Date_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Invoice_Date)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Invoice_Qty_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Invoice_Qty)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Dispatch_Date_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Dispatch_Date)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Vehicle_No_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Vehicle_No)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Delivery_Status_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Delivery_Status)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Delivery_ReAttempt_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Delivery_ReAttempt)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.POD_Date_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.POD_Date)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Actual_Reporting_Time_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Actual_Reporting_Time)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Undelivered_Reason_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Undelivered_Reason)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Delivered_Qty_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Delivered_Qty)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Is_OnTimeDelivery_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Is_OnTimeDelivery)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                //if (orderExcel.ISR_NO_Id == 0)
                                //{
                                //    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.ISR_NO)).FirstOrDefault().HeaderName;
                                //    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                //}
                                if (ErrorList.Count == 0)
                                {
                                    if (!Directory.Exists(Server.MapPath("~/UploadedFiles" + ExtentedFilePath)))
                                    {
                                        Directory.CreateDirectory(Server.MapPath("~/UploadedFiles" + ExtentedFilePath));
                                    }
                                    file.SaveAs(_path);
                                    UploadedFilesMst mst = new UploadedFilesMst();
                                    mst.CustId = LoggedInCustomer;
                                    mst.Upload_By = LoggedInUser;
                                    mst.User_FileName = fileName;
                                    mst.Filepath = _path;
                                    mst.System_FileName = _FileName;
                                    mst.FileType = "OUTWARD";
                                    long FileId = _orderService.SaveUploadedFile(mst);
                                    List<OrderExcelMapping> lstOrderExcel = new List<OrderExcelMapping>();
                                    for (int rowIterator = 2; rowIterator <= workSheet.Dimension.End.Row; rowIterator++)
                                    {
                                        var user = new Users();
                                        var import = new OrderExcelMapping();
                                        import.Error = "";
                                        import.Unique_Order_No = workSheet.Cells[rowIterator, orderExcel.Unique_Order_No_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Unique_Order_No_Id].Value.ToString() : "";
                                        DateTime? date;
                                        try
                                        {
                                            if (workSheet.Cells[rowIterator, orderExcel.Order_Date_Id].Value != null)
                                            {
                                                date = workSheet.Cells[rowIterator, orderExcel.Order_Date_Id].Value != null ? HelperCls.FromExcelStringDate(workSheet.Cells[rowIterator, orderExcel.Order_Date_Id].Value.ToString()) : (Nullable<DateTime>)null;
                                            }
                                            else
                                            {
                                                import.Error += "Error at Row: " + rowIterator + ", Column: " + orderExcel.Order_Date_Id + "Unable to Convert the Date.";
                                                date = null;
                                            }
                                            import.Order_Date = date;
                                        }
                                        catch (Exception ex)
                                        {
                                            import.Error += "Error at Row: " + rowIterator + ", column: " + orderExcel.Order_Date_Id + "Unable to Convert the Date.";
                                            HelperCls.DebugLog("Error Date: " + workSheet.Cells[rowIterator, orderExcel.Order_Date_Id].Value + ex.Message + System.Environment.NewLine + ex.StackTrace);
                                            import.Error += ex.Message;
                                        }
                                        import.Store_Code = workSheet.Cells[rowIterator, orderExcel.Store_Code_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Store_Code_Id].Value.ToString() : "";
                                        import.City = workSheet.Cells[rowIterator, orderExcel.City_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.City_Id].Value.ToString() : "";
                                        import.Source_Dc = workSheet.Cells[rowIterator, orderExcel.Source_Dc_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Source_Dc_Id].Value.ToString() : "";
                                        import.Item_Code = workSheet.Cells[rowIterator, orderExcel.Item_Code_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Item_Code_Id].Value.ToString() : "";
                                        import.Ordered_Qty = workSheet.Cells[rowIterator, orderExcel.Ordered_Qty_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Ordered_Qty_Id].Value.ToString() : "";
                                        import.Sales_Order_No = workSheet.Cells[rowIterator, orderExcel.Sales_Order_No_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Sales_Order_No_Id].Value.ToString() : "";
                                        import.SO_Status = workSheet.Cells[rowIterator, orderExcel.SO_Status_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.SO_Status_Id].Value.ToString() : "";
                                        DateTime? date2;
                                        try
                                        {
                                            if (workSheet.Cells[rowIterator, orderExcel.Sales_Order_Date_Id].Value != null)
                                            {
                                                date2 = workSheet.Cells[rowIterator, orderExcel.Sales_Order_Date_Id].Value != null ? HelperCls.FromExcelStringDateOnly(workSheet.Cells[rowIterator, orderExcel.Sales_Order_Date_Id].Value.ToString()) : (Nullable<DateTime>)null;
                                            }
                                            else
                                            {
                                                import.Error += "Error at Row: " + rowIterator + ", Column: " + orderExcel.Sales_Order_Date_Id + "Unable to Convert the Date.";
                                                date2 = null;
                                            }
                                            import.Sales_Order_Date = date2;
                                        }
                                        catch (Exception ex)
                                        {
                                            import.Error += "Error at Row: " + rowIterator + ", column: " + orderExcel.Sales_Order_Date_Id + "Unable to Convert the Date.";
                                            HelperCls.DebugLog("Error Date: " + workSheet.Cells[rowIterator, orderExcel.Sales_Order_Date_Id].Value + ex.Message + System.Environment.NewLine + ex.StackTrace);
                                            import.Error += ex.Message;
                                        }
                                        import.Sales_Order_Qty = workSheet.Cells[rowIterator, orderExcel.Sales_Order_Qty_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Sales_Order_Qty_Id].Value.ToString() : "";
                                        import.Invoice_No = workSheet.Cells[rowIterator, orderExcel.Invoice_No_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Invoice_No_Id].Value.ToString() : "";
                                        DateTime? date3;
                                        try
                                        {
                                            if (workSheet.Cells[rowIterator, orderExcel.Invoice_Date_Id].Value != null)
                                            {
                                                date3 = workSheet.Cells[rowIterator, orderExcel.Invoice_Date_Id].Value != null ? HelperCls.FromExcelStringDateOnly(workSheet.Cells[rowIterator, orderExcel.Invoice_Date_Id].Value.ToString()) : (Nullable<DateTime>)null;
                                            }
                                            else
                                            {
                                                import.Error += "Error at Row: " + rowIterator + ", Column: " + orderExcel.Invoice_Date_Id + "Unable to Convert the Date.";
                                                date3 = null;
                                            }
                                            import.Invoice_Date = date3;
                                        }
                                        catch (Exception ex)
                                        {
                                            import.Error += "Error at Row: " + rowIterator + ", column: " + orderExcel.Invoice_Date_Id + "Unable to Convert the Date.";
                                            HelperCls.DebugLog("Error Date: " + workSheet.Cells[rowIterator, orderExcel.Invoice_Date_Id].Value + ex.Message + System.Environment.NewLine + ex.StackTrace);
                                            import.Error += ex.Message;
                                        }
                                        import.Invoice_Qty = workSheet.Cells[rowIterator, orderExcel.Invoice_Qty_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Invoice_Qty_Id].Value.ToString() : "";
                                        DateTime? date4;
                                        try
                                        {
                                            if (workSheet.Cells[rowIterator, orderExcel.Dispatch_Date_Id].Value != null)
                                            {
                                                date4 = workSheet.Cells[rowIterator, orderExcel.Dispatch_Date_Id].Value != null ? HelperCls.FromExcelStringDateOnly(workSheet.Cells[rowIterator, orderExcel.Dispatch_Date_Id].Value.ToString()) : (Nullable<DateTime>)null;
                                            }
                                            else
                                            {
                                                import.Error += "Error at Row: " + rowIterator + ", Column: " + orderExcel.Dispatch_Date_Id + "Unable to Convert the Date.";
                                                date4 = null;
                                            }
                                            import.Dispatch_Date = date4;
                                        }
                                        catch (Exception ex)
                                        {
                                            import.Error += "Error at Row: " + rowIterator + ", column: " + orderExcel.Dispatch_Date_Id + "Unable to Convert the Date.";
                                            HelperCls.DebugLog("Error Date: " + workSheet.Cells[rowIterator, orderExcel.Dispatch_Date_Id].Value + ex.Message + System.Environment.NewLine + ex.StackTrace);
                                            import.Error += ex.Message;
                                        }

                                        import.Vehicle_No = workSheet.Cells[rowIterator, orderExcel.Vehicle_No_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Vehicle_No_Id].Value.ToString() : "";
                                        import.Delivery_Status = workSheet.Cells[rowIterator, orderExcel.Delivery_Status_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Delivery_Status_Id].Value.ToString() : "";
                                        import.Delivery_ReAttempt = workSheet.Cells[rowIterator, orderExcel.Delivery_ReAttempt_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Delivery_ReAttempt_Id].Value.ToString() : "";

                                        DateTime? date5;
                                        try
                                        {
                                            if (workSheet.Cells[rowIterator, orderExcel.POD_Date_Id].Value != null)
                                            {
                                                date5 = workSheet.Cells[rowIterator, orderExcel.POD_Date_Id].Value != null ? HelperCls.FromExcelStringDateOnly(workSheet.Cells[rowIterator, orderExcel.POD_Date_Id].Value.ToString()) : (Nullable<DateTime>)null;
                                            }
                                            else
                                            {
                                                import.Error += "Error at Row: " + rowIterator + ", Column: " + orderExcel.POD_Date_Id + "Unable to Convert the Date.";
                                                date5 = null;
                                            }
                                            import.POD_Date = date5;
                                        }
                                        catch (Exception ex)
                                        {
                                            import.Error += "Error at Row: " + rowIterator + ", column: " + orderExcel.POD_Date_Id + "Unable to Convert the Date.";
                                            HelperCls.DebugLog("Error Date: " + workSheet.Cells[rowIterator, orderExcel.POD_Date_Id].Value + ex.Message + System.Environment.NewLine + ex.StackTrace);
                                            import.Error += ex.Message;
                                        }
                                        import.Actual_Reporting_Time = workSheet.Cells[rowIterator, orderExcel.Actual_Reporting_Time_Id].Text != null ? workSheet.Cells[rowIterator, orderExcel.Actual_Reporting_Time_Id].Text.ToString() : "";
                                        import.Undelivered_Reason = workSheet.Cells[rowIterator, orderExcel.Undelivered_Reason_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Undelivered_Reason_Id].Value.ToString() : "";
                                        import.Delivered_Qty = workSheet.Cells[rowIterator, orderExcel.Delivered_Qty_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Delivered_Qty_Id].Value.ToString() : "";
                                        import.Is_OnTimeDelivery = workSheet.Cells[rowIterator, orderExcel.Is_OnTimeDelivery_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Is_OnTimeDelivery_Id].Value.ToString() : "";
                                        if (orderExcel.ISR_NO_Id != 0)
                                            import.ISR_NO = workSheet.Cells[rowIterator, orderExcel.ISR_NO_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.ISR_NO_Id].Value.ToString() : "";
                                        import.Xls_Line_No = rowIterator;
                                        import.Uploaded_File_Id = FileId;
                                        import.Upload_By = LoggedInUser;
                                        import.CustId = LoggedInCustomer;
                                        lstOrderExcel.Add(import);
                                    }
                                    //------ Add storewise GroupID in all records--
                                    string[] uniqueStores = lstOrderExcel.Select(p => p.Store_Code).Distinct().ToArray();
                                    List<CodeValuePair> lstCodeValuePair = new List<CodeValuePair>();

                                    for (int i = 0; i < uniqueStores.Count(); i++)
                                    {
                                        CodeValuePair obj = new CodeValuePair();
                                        obj.StoreCode = uniqueStores[i];
                                        obj.GroupId = _orderService.GetMaxGroupIDByCustomer(mst.CustId);
                                        lstCodeValuePair.Add(obj);
                                    }


                                    //---------------------------------------------
                                    foreach (var item in lstOrderExcel)
                                    {
                                        item.GroupId = lstCodeValuePair.Where(a => a.StoreCode == item.Store_Code).Select(a => a.GroupId).FirstOrDefault();
                                        long ReturnValue = _orderService.UploadExcelFileData(item);
                                        if (ReturnValue != 0)
                                        {
                                            RowsInserted++;
                                        }
                                    }
                                    ViewBag.RowsCount = noOfRow - 1;
                                    ViewBag.RowsInserted = RowsInserted;
                                    List<string> UniqueOrderNoList = lstOrderExcel.Select(x => x.Unique_Order_No).Distinct().ToList();
                                    foreach (string UniqueOrderNo in UniqueOrderNoList)
                                    {
                                        //long ReturnValue = _orderService.ProcessCompleteExcelUploadOutward(UniqueOrderNo, FileId, LoggedInCustomer);
                                    }
                                    return View();
                                }
                            }
                            //ViewBag.PreOrder = PreOrder;
                            ViewBag.ErrorList = ErrorList;
                            if (ErrorList != null && ErrorList.Count() > 0)
                            {
                                ViewBag.FinilizeOrder = null;
                                ViewBag.PreOrder = null;
                                ViewBag.Error = string.Join(", ", ErrorList.ToArray());
                                foreach (string err in ErrorList)
                                    ModelState.AddModelError("", err);
                            }
                            return View();
                        }
                    }
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    ViewBag.Error = "Invalid File Format";
                    ModelState.AddModelError("", "Invalid File Format");
                    ViewBag.FinilizeOrder = null;
                }
            }
            return View();
        }

        [AdminAuthorization]
        public ActionResult UploadInward()
        {
            return View();
        }
        [HttpPost]
        [AdminApiAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult UploadInward(HttpPostedFileBase file)
        {
            var LoggedInCustomer = Convert.ToInt32(SessionValues.LoggedInCustId);
            List<string> ErrorList = new List<string>();
            var ErrorLists = new List<string>();
            var LoggedInUser = SessionValues.UserId;
            if (Request.Files["file"].ContentLength > 0)
            {
                try
                {
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        var fileSize = file.ContentLength;
                        int RowsInserted = 0;
                        if (fileSize > GlobalValues.MegaBytes)
                        {
                            string err = "GENERAL EXCEPTION - LARGE FILE | DATA UPLOAD FAILED";
                            ModelState.AddModelError("", err);
                            return View();
                        }
                        else
                        {
                            string _FileName = Path.GetFileName(file.FileName);
                            string ExtentedFilePath = "/Client-" + LoggedInCustomer + "/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MMM") + "/INWARD";
                            _FileName = LoggedInUser + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + "_" + _FileName;
                            string _path = Path.Combine(Server.MapPath("~/UploadedFiles" + ExtentedFilePath), "" + _FileName);

                            byte[] fileBytes = new byte[file.ContentLength];
                            var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                            using (var package = new ExcelPackage(file.InputStream))
                            {
                                var currentSheet = package.Workbook.Worksheets;
                                var workSheet = currentSheet["Sheet1"] != null ? currentSheet["Sheet1"] : currentSheet.First();
                                var noOfCol = workSheet.Dimension.End.Column;
                                OrderExcelInward orderExcel = new OrderExcelInward();
                                OrderFormatExcel excelmapping = _orderService.GetUploadOrderFormat(LoggedInCustomer, "INWARD");
                                if (excelmapping != null)
                                {
                                    for (int i = 1; i <= workSheet.Dimension.End.Column; i++)
                                    {
                                        var firstRowCell = workSheet.Cells[1, i];
                                        var HeaderName = firstRowCell.Value != null ? firstRowCell.Value.ToString().Trim() : "";
                                        if (excelmapping.ExcelHeaders.Exists(x => x.HeaderName == HeaderName))
                                        {
                                            string ParameterName = excelmapping.ExcelHeaders.Where(x => x.HeaderName == firstRowCell.Value.ToString().Trim()).FirstOrDefault().ParameterName;
                                            if (ParameterName.Trim().ToUpper() == nameof(orderExcel.PO_Number).Trim().ToUpper())
                                            {
                                                orderExcel.PO_Number_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.PO_Date).Trim().ToUpper())
                                            {
                                                orderExcel.PO_Date_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Vendor_Name).Trim().ToUpper())
                                            {
                                                orderExcel.Vendor_Name_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Status).Trim().ToUpper())
                                            {
                                                orderExcel.Status_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.GRN_Date).Trim().ToUpper())
                                            {
                                                orderExcel.GRN_Date_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.GRN_No).Trim().ToUpper())
                                            {
                                                orderExcel.GRN_No_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.SupplierDoc_Date).Trim().ToUpper())
                                            {
                                                orderExcel.SupplierDoc_Date_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.SupplierDoc_Ref).Trim().ToUpper())
                                            {
                                                orderExcel.SupplierDoc_Ref_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Vehicle_No).Trim().ToUpper())
                                            {
                                                orderExcel.Vehicle_No_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Remark).Trim().ToUpper())
                                            {
                                                orderExcel.Remark_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Class).Trim().ToUpper())
                                            {
                                                orderExcel.Class_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.Location).Trim().ToUpper())
                                            {
                                                orderExcel.Location_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.SKU_Code).Trim().ToUpper())
                                            {
                                                orderExcel.SKU_Code_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.SKU_Description).Trim().ToUpper())
                                            {
                                                orderExcel.SKU_Description_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.SKU_Category).Trim().ToUpper())
                                            {
                                                orderExcel.SKU_Category_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.PO_Qty).Trim().ToUpper())
                                            {
                                                orderExcel.PO_Qty_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.GRN_Qty).Trim().ToUpper())
                                            {
                                                orderExcel.GRN_Qty_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderExcel.VRA_Qty).Trim().ToUpper())
                                            {
                                                orderExcel.VRA_Qty_Id = i;
                                            }
                                        }
                                    }
                                }

                                var noOfRow = workSheet.Dimension.End.Row;
                                if (orderExcel.PO_Number_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.PO_Number)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.PO_Date_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.PO_Date)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Vendor_Name_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Vendor_Name)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Status_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Status)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.GRN_Date_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.GRN_Date)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.GRN_No_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.GRN_No)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.SupplierDoc_Date_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.SupplierDoc_Date)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.SupplierDoc_Ref_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.SupplierDoc_Ref)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Vehicle_No_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Vehicle_No)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Remark_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Remark)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Class_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Class)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.Location_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.Location)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.SKU_Code_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.SKU_Code)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.SKU_Description_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.SKU_Description)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.SKU_Category_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.SKU_Category)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.PO_Qty_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.PO_Qty)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.GRN_Qty_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.GRN_Qty)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderExcel.VRA_Qty_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderExcel.VRA_Qty)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (ErrorList.Count == 0)
                                {
                                    if (!Directory.Exists(Server.MapPath("~/UploadedFiles" + ExtentedFilePath)))
                                    {
                                        Directory.CreateDirectory(Server.MapPath("~/UploadedFiles" + ExtentedFilePath));
                                    }
                                    file.SaveAs(_path);
                                    UploadedFilesMst mst = new UploadedFilesMst();
                                    mst.CustId = LoggedInCustomer;
                                    mst.Upload_By = LoggedInUser;
                                    mst.User_FileName = fileName;
                                    mst.Filepath = _path;
                                    mst.System_FileName = _FileName;
                                    mst.FileType = "INWARD";
                                    long FileId = _orderService.SaveUploadedFile(mst);
                                    List<OrderExcelInward> lstOrderExcel = new List<OrderExcelInward>();
                                    for (int rowIterator = 2; rowIterator <= workSheet.Dimension.End.Row; rowIterator++)
                                    {
                                        var user = new Users();
                                        var import = new OrderExcelInward();
                                        import.Error = "";
                                        import.PO_Number = workSheet.Cells[rowIterator, orderExcel.PO_Number_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.PO_Number_Id].Value.ToString() : "";
                                        DateTime? date;
                                        try
                                        {
                                            if (workSheet.Cells[rowIterator, orderExcel.PO_Date_Id].Value != null)
                                            {
                                                date = workSheet.Cells[rowIterator, orderExcel.PO_Date_Id].Value != null ? HelperCls.FromExcelStringDateOnly(workSheet.Cells[rowIterator, orderExcel.PO_Date_Id].Value.ToString()) : (Nullable<DateTime>)null;
                                            }
                                            else
                                            {
                                                //if (import.Error != "" || import.Error != null)
                                                //    import.Error += "\n";
                                                //import.Error += "Error at Row: " + rowIterator + ", Column: " + orderExcel.PO_Date_Id + ". Unable to Convert the Date. The Format should be dd/mm/yyyy. ";
                                                date = null;
                                            }
                                            import.PO_Date = date;
                                        }
                                        catch (Exception ex)
                                        {
                                            if (import.Error != "" || import.Error != null)
                                                import.Error += "\n";
                                            import.Error += "Error at Row: " + rowIterator + ", column: " + orderExcel.PO_Date_Id + ". Unable to Convert the Date. The Format should be dd/mm/yyyy. ";
                                            HelperCls.DebugLog("Error Date: " + workSheet.Cells[rowIterator, orderExcel.PO_Date_Id].Value + ex.Message + System.Environment.NewLine + ex.StackTrace);
                                            import.Error += ex.Message;
                                        }
                                        import.Vendor_Name = workSheet.Cells[rowIterator, orderExcel.Vendor_Name_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Vendor_Name_Id].Value.ToString() : "";
                                        import.Status = workSheet.Cells[rowIterator, orderExcel.Status_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Status_Id].Value.ToString() : "";
                                        DateTime? date2;
                                        try
                                        {
                                            if (workSheet.Cells[rowIterator, orderExcel.GRN_Date_Id].Value != null)
                                            {
                                                date2 = workSheet.Cells[rowIterator, orderExcel.GRN_Date_Id].Value != null ? HelperCls.FromExcelStringDateOnly(workSheet.Cells[rowIterator, orderExcel.GRN_Date_Id].Value.ToString()) : (Nullable<DateTime>)null;
                                            }
                                            else
                                            {
                                                //if (import.Error != "" || import.Error != null)
                                                //    import.Error += "\n";
                                                //import.Error += "Error at Row: " + rowIterator + ", Column: " + orderExcel.GRN_Date_Id + ". Unable to Convert the Date. The Format should be dd/mm/yyyy. ";
                                                date2 = null;
                                            }
                                            import.GRN_Date = date2;
                                        }
                                        catch (Exception ex)
                                        {
                                            if (import.Error != "" || import.Error != null)
                                                import.Error += "\n";
                                            import.Error += "Error at Row: " + rowIterator + ", column: " + orderExcel.GRN_Date_Id + ". Unable to Convert the Date. The Format should be dd/mm/yyyy. ";
                                            HelperCls.DebugLog("Error Date: " + workSheet.Cells[rowIterator, orderExcel.GRN_Date_Id].Value + ex.Message + System.Environment.NewLine + ex.StackTrace);
                                            import.Error += ex.Message;
                                        }
                                        import.GRN_No = workSheet.Cells[rowIterator, orderExcel.GRN_No_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.GRN_No_Id].Value.ToString() : "";
                                        DateTime? date3;
                                        try
                                        {
                                            if (workSheet.Cells[rowIterator, orderExcel.SupplierDoc_Date_Id].Value != null)
                                            {
                                                date3 = workSheet.Cells[rowIterator, orderExcel.SupplierDoc_Date_Id].Value != null ? HelperCls.FromExcelStringDateOnly(workSheet.Cells[rowIterator, orderExcel.SupplierDoc_Date_Id].Value.ToString()) : (Nullable<DateTime>)null;
                                            }
                                            else
                                            {
                                                //import.Error += "Error at Row: " + rowIterator + ", Column: " + orderExcel.SupplierDoc_Date_Id + ". Unable to Convert the Date. The Format should be dd/mm/yyyy. ";
                                                //if (import.Error != "" || import.Error != null)
                                                //    import.Error += "\n";
                                                date3 = null;
                                            }
                                            import.SupplierDoc_Date = date3;
                                        }
                                        catch (Exception ex)
                                        {
                                            if (import.Error != "" || import.Error != null)
                                                import.Error += "\n";
                                            import.Error += "Error at Row: " + rowIterator + ", column: " + orderExcel.SupplierDoc_Date_Id + ". Unable to Convert the Date. The Format should be dd/mm/yyyy. ";
                                            HelperCls.DebugLog("Error Date: " + workSheet.Cells[rowIterator, orderExcel.SupplierDoc_Date_Id].Value + ex.Message + System.Environment.NewLine + ex.StackTrace);
                                            import.Error += ex.Message;
                                        }
                                        import.SupplierDoc_Ref = workSheet.Cells[rowIterator, orderExcel.SupplierDoc_Ref_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.SupplierDoc_Ref_Id].Value.ToString() : "";
                                        import.Vehicle_No = workSheet.Cells[rowIterator, orderExcel.Vehicle_No_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Vehicle_No_Id].Value.ToString() : "";
                                        import.Remark = workSheet.Cells[rowIterator, orderExcel.Remark_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Remark_Id].Value.ToString() : "";
                                        import.Class = workSheet.Cells[rowIterator, orderExcel.Class_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Class_Id].Value.ToString() : "";
                                        import.Location = workSheet.Cells[rowIterator, orderExcel.Location_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.Location_Id].Value.ToString() : "";

                                        import.SKU_Code = workSheet.Cells[rowIterator, orderExcel.SKU_Code_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.SKU_Code_Id].Value.ToString() : "";
                                        import.SKU_Description = workSheet.Cells[rowIterator, orderExcel.SKU_Description_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.SKU_Description_Id].Value.ToString() : "";
                                        import.SKU_Category = workSheet.Cells[rowIterator, orderExcel.SKU_Category_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.SKU_Category_Id].Value.ToString() : "";

                                        import.PO_Qty = workSheet.Cells[rowIterator, orderExcel.PO_Qty_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.PO_Qty_Id].Value.ToString() : "";
                                        import.GRN_Qty = workSheet.Cells[rowIterator, orderExcel.GRN_Qty_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.GRN_Qty_Id].Value.ToString() : "";
                                        import.VRA_Qty = workSheet.Cells[rowIterator, orderExcel.VRA_Qty_Id].Value != null ? workSheet.Cells[rowIterator, orderExcel.VRA_Qty_Id].Value.ToString() : "";

                                        import.Xls_Line_No = rowIterator;
                                        import.Uploaded_File_Id = FileId;
                                        import.Upload_By = LoggedInUser;
                                        import.CustId = LoggedInCustomer;
                                        lstOrderExcel.Add(import);
                                    }
                                    //------ Add storewise GroupID in all records--
                                    //string[] uniqueStores = lstOrderExcel.Select(p => p.Store_Code).Distinct().ToArray();
                                    //List<CodeValuePair> lstCodeValuePair = new List<CodeValuePair>();

                                    //for (int i = 0; i < uniqueStores.Count(); i++)
                                    //{
                                    //    CodeValuePair obj = new CodeValuePair();
                                    //    obj.StoreCode = uniqueStores[i];
                                    //    obj.GroupId = _orderService.GetMaxGroupIDByCustomer(mst.CustId);
                                    //    lstCodeValuePair.Add(obj);
                                    //}


                                    //---------------------------------------------
                                    ErrorLists = lstOrderExcel.Where(x => x.Error != "").Select(x => x.Error).ToList();
                                    if (ErrorLists.Count() <= 0)
                                    {
                                        foreach (var item in lstOrderExcel)
                                        {
                                            // item.GroupId = lstCodeValuePair.Where(a => a.StoreCode == item.Store_Code).Select(a => a.GroupId).FirstOrDefault();
                                            long ReturnValue = _orderService.UploadInwardFileData(item);
                                            if (ReturnValue != 0)
                                            {
                                                RowsInserted++;
                                            }
                                        }
                                    }
                                    if ((ErrorLists != null && ErrorLists.Count > 0))
                                    {
                                        ViewBag.FinilizeOrder = null;
                                        ViewBag.PreOrder = null;
                                        ViewBag.Error = string.Join(", ", ErrorList.ToArray());
                                        foreach (string err in ErrorList)
                                            ModelState.AddModelError("", err);
                                        foreach (string err in ErrorLists)
                                            ModelState.AddModelError("", err);
                                    }
                                    ViewBag.RowsCount = noOfRow - 1;
                                    ViewBag.RowsInserted = RowsInserted;
                                    return View();
                                }
                            }
                            //ViewBag.PreOrder = PreOrder;
                            ViewBag.ErrorList = ErrorList;
                            if ((ErrorList != null && ErrorList.Count() > 0) || (ErrorLists != null && ErrorLists.Count > 0))
                            {
                                ViewBag.FinilizeOrder = null;
                                ViewBag.PreOrder = null;
                                ViewBag.Error = string.Join(", ", ErrorList.ToArray());
                                foreach (string err in ErrorList)
                                    ModelState.AddModelError("", err);
                                foreach (string err in ErrorLists)
                                    ModelState.AddModelError("", err);
                            }
                            return View();
                        }
                    }
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    ViewBag.Error = "Invalid File Format";
                    ModelState.AddModelError("", "Invalid File Format");
                    ViewBag.FinilizeOrder = null;
                }
            }
            return View();
        }

        [AdminAuthorization]
        public ActionResult UploadInventory()
        {
            return View();
        }
        [HttpPost]
        [AdminApiAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult UploadInventory(HttpPostedFileBase file)
        {
            var LoggedInCustomer = Convert.ToInt32(SessionValues.LoggedInCustId);
            List<string> ErrorList = new List<string>();
            var LoggedInUser = SessionValues.UserId;
            if (Request.Files["file"].ContentLength > 0)
            {
                try
                {
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        var fileSize = file.ContentLength;
                        int RowsInserted = 0;
                        if (fileSize > GlobalValues.MegaBytes)
                        {
                            string err = "GENERAL EXCEPTION - LARGE FILE | DATA UPLOAD FAILED";
                            ModelState.AddModelError("", err);
                            return View();
                        }
                        else
                        {
                            string _FileName = Path.GetFileName(file.FileName);
                            _FileName = LoggedInUser + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + "_" + _FileName;
                            string ExtentedFilePath = "/Client-" + LoggedInCustomer + "/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MMM") + "/ORDERINVENTORY";
                            string _path = Path.Combine(Server.MapPath("~/UploadedFiles" + ExtentedFilePath), "" + _FileName);

                            byte[] fileBytes = new byte[file.ContentLength];
                            var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                            using (var package = new ExcelPackage(file.InputStream))
                            {
                                var currentSheet = package.Workbook.Worksheets;
                                var workSheet = currentSheet["Sheet1"] != null ? currentSheet["Sheet1"] : currentSheet.First();
                                var noOfCol = workSheet.Dimension.End.Column;
                                OrderInventory orderInv = new OrderInventory();
                                OrderFormatExcel excelmapping = _orderService.GetUploadOrderFormat(LoggedInCustomer, "ORDERINVENTORY");
                                if (excelmapping != null)
                                {
                                    for (int i = 1; i <= workSheet.Dimension.End.Column; i++)
                                    {
                                        var firstRowCell = workSheet.Cells[1, i];
                                        var HeaderName = firstRowCell.Value != null ? firstRowCell.Value.ToString().Trim() : "";
                                        if (excelmapping.ExcelHeaders.Exists(x => x.HeaderName == HeaderName))
                                        {
                                            string ParameterName = excelmapping.ExcelHeaders.Where(x => x.HeaderName == firstRowCell.Value.ToString().Trim()).FirstOrDefault().ParameterName;
                                            if (ParameterName.Trim().ToUpper() == nameof(orderInv.SKU_Code).Trim().ToUpper())
                                            {
                                                orderInv.SKU_Code_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderInv.SKU_Description).Trim().ToUpper())
                                            {
                                                orderInv.SKU_Description_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderInv.SKU_Category).Trim().ToUpper())
                                            {
                                                orderInv.SKU_Category_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderInv.DC_Code).Trim().ToUpper())
                                            {
                                                orderInv.DC_Code_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderInv.UOM).Trim().ToUpper())
                                            {
                                                orderInv.UOM_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderInv.On_Hand).Trim().ToUpper())
                                            {
                                                orderInv.On_Hand_Id = i;
                                            }
                                            else if (ParameterName.Trim().ToUpper() == nameof(orderInv.QC).Trim().ToUpper())
                                            {
                                                orderInv.QC_Id = i;
                                            }
                                        }
                                    }
                                }

                                var noOfRow = workSheet.Dimension.End.Row;
                                if (orderInv.SKU_Code_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderInv.SKU_Code)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderInv.SKU_Description_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderInv.SKU_Description)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderInv.SKU_Category_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderInv.SKU_Category)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderInv.DC_Code_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderInv.DC_Code)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderInv.UOM_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderInv.UOM)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderInv.On_Hand_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderInv.On_Hand)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (orderInv.QC_Id == 0)
                                {
                                    string HeaderName = excelmapping.ExcelHeaders.Where(x => x.ParameterName == nameof(orderInv.QC)).FirstOrDefault().HeaderName;
                                    ErrorList.Add("COLUMN_NAME ERROR: " + HeaderName + " | DATA UPLOAD FAILED");
                                }
                                if (ErrorList.Count == 0)
                                {
                                    if (!Directory.Exists(Server.MapPath("~/UploadedFiles" + ExtentedFilePath)))
                                    {
                                        Directory.CreateDirectory(Server.MapPath("~/UploadedFiles" + ExtentedFilePath));
                                    }
                                    file.SaveAs(_path);
                                    UploadedFilesMst mst = new UploadedFilesMst();
                                    mst.CustId = LoggedInCustomer;
                                    mst.Upload_By = LoggedInUser;
                                    mst.User_FileName = fileName;
                                    mst.Filepath = _path;
                                    mst.System_FileName = _FileName;
                                    mst.FileType = "ORDERINVENTORY";
                                    long FileId = _orderService.SaveUploadedFile(mst);
                                    List<OrderInventory> lstOrderinventory = new List<OrderInventory>();
                                    for (int rowIterator = 2; rowIterator <= workSheet.Dimension.End.Row; rowIterator++)
                                    {
                                        var user = new Users();
                                        var import = new OrderInventory();
                                        import.Error = "";
                                        import.SKU_Code = workSheet.Cells[rowIterator, orderInv.SKU_Code_Id].Value != null ? workSheet.Cells[rowIterator, orderInv.SKU_Code_Id].Value.ToString() : "";
                                        import.SKU_Description = workSheet.Cells[rowIterator, orderInv.SKU_Description_Id].Value != null ? workSheet.Cells[rowIterator, orderInv.SKU_Description_Id].Value.ToString() : "";
                                        import.SKU_Category = workSheet.Cells[rowIterator, orderInv.SKU_Category_Id].Value != null ? workSheet.Cells[rowIterator, orderInv.SKU_Category_Id].Value.ToString() : "";
                                        import.DC_Code = workSheet.Cells[rowIterator, orderInv.DC_Code_Id].Value != null ? workSheet.Cells[rowIterator, orderInv.DC_Code_Id].Value.ToString() : "";
                                        import.UOM = workSheet.Cells[rowIterator, orderInv.UOM_Id].Value != null ? workSheet.Cells[rowIterator, orderInv.UOM_Id].Value.ToString() : "";
                                        import.On_Hand = workSheet.Cells[rowIterator, orderInv.On_Hand_Id].Value != null ? workSheet.Cells[rowIterator, orderInv.On_Hand_Id].Value.ToString() : "";
                                        import.QC = workSheet.Cells[rowIterator, orderInv.QC_Id].Value != null ? workSheet.Cells[rowIterator, orderInv.QC_Id].Value.ToString() : "";

                                        import.Xls_Line_No = rowIterator;
                                        import.Uploaded_File_Id = FileId;
                                        import.Upload_By = LoggedInUser;
                                        import.CustId = LoggedInCustomer;
                                        lstOrderinventory.Add(import);
                                    }
                                    //---------------------------------------------
                                    foreach (var item in lstOrderinventory)
                                    {

                                        long ReturnValue = _orderService.UploadOrderInventoryFileData(item);
                                        if (ReturnValue != 0)
                                        {
                                            RowsInserted++;
                                        }
                                    }
                                    ViewBag.RowsCount = noOfRow - 1;
                                    ViewBag.RowsInserted = RowsInserted;
                                    return View();
                                }
                            }
                            //ViewBag.PreOrder = PreOrder;
                            ViewBag.ErrorList = ErrorList;
                            if (ErrorList != null && ErrorList.Count() > 0)
                            {
                                ViewBag.FinilizeOrder = null;
                                ViewBag.PreOrder = null;
                                ViewBag.Error = string.Join(", ", ErrorList.ToArray());
                                foreach (string err in ErrorList)
                                    ModelState.AddModelError("", err);
                            }
                            return View();
                        }
                    }
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    ViewBag.Error = "Invalid File Format";
                    ModelState.AddModelError("", "Invalid File Format");
                    ViewBag.FinilizeOrder = null;
                }
            }
            return View();
        }


        #endregion

        #region Inventory 25 Jan 2021, 27 Jan 2021 -Deepa 
        [AdminAuthorization]
        public ActionResult GetNetSuiteInventory()
        {
            int UserId = SessionValues.UserId;
            int CustId = Convert.ToInt32(SessionValues.LoggedInCustId);
            DateTime now = DateTime.Now;
            var Items = _itemService.GetSkuForUserCustomer(UserId, CustId, "Order");

            var DCs = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Order");
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);

            List<int> SelectedDC = null;
            List<int> SelectedSKUs = null;
            SelectedDC = DCs.Select(x => x.Id).ToList();
            SelectedSKUs = Items.Select(x => x.Id).ToList();

            List<NetSuiteInventory> finilised = _orderService.GetNetSuiteInventory(CustId, SelectedDC, SelectedSKUs, UserId);
            ViewBag.NetSuiteInventoryList = finilised.ToList();

            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GetNetSuiteInventory(List<int> SelectedDC, List<int> SelectedSKUs)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);

            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Order");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Order");

            SelectedDC = SelectedDC != null && SelectedDC.Count() > 0 && SelectedDC[0] != 0 ? SelectedDC : DCs.Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomer(userid, custid, "Order").Select(x => x.Id).ToList();

            List<NetSuiteInventory> finilised = _orderService.GetNetSuiteInventory(custid, SelectedDC, SelectedSKUs, userid);

            ViewBag.NetSuiteInventoryList = finilised.ToList();
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);

            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult NetSuiteInventoryExportExcel(List<int> SelectedDC, List<int> SelectedSKUs)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Order");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Order");
            SelectedDC = SelectedDC != null && SelectedDC.Count() > 0 ? SelectedDC : DCs.Select(x => x.Id).ToList();
            SelectedSKUs = SelectedSKUs != null && SelectedSKUs.Count() > 0 ? SelectedSKUs : Items.Select(x => x.Id).ToList();

            List<NetSuiteInventory> finilised = _orderService.GetNetSuiteInventory(custid, SelectedDC, SelectedSKUs, userid);
            string Title = "NetSuite Inventory";
            string Message = "Inventroy Date:" + finilised.FirstOrDefault().OnHandDate;

            var finilisedList = finilised.Select(head => new
            {
                head.SKUCode,
                head.SKUName,
                head.DCName,
                head.AvailableQty,
                head.OrderedQty,
                OnHandDate = head.OnHandDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
            });
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].Value = Title;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Cells[2, 1].Value = Message;
                workSheet.Cells[2, 1].Style.Font.Bold = true;

                workSheet.Cells[4, 1].Value = "SKU Code";
                workSheet.Cells[4, 2].Value = "SKU Name";
                workSheet.Cells[4, 3].Value = "DC Name";
                workSheet.Cells[4, 4].Value = "Available Stock";
                workSheet.Cells[4, 5].Value = "Actual Ordered Qty";
                workSheet.Cells[4, 6].Value = "Inventory Date";


                workSheet.Cells[5, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "NetSuiteInventory.xlsx");

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RefreshNetsuiteInventory()
        {
            int UserId = SessionValues.UserId;
            int CustId = Convert.ToInt32(SessionValues.LoggedInCustId);

            var Items = _itemService.GetSkuForUserCustomer(UserId, CustId, "Order");
            var DCs = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Order");


            List<int> SelectedDC = null;
            List<int> SelectedSKUs = null;
            SelectedDC = DCs.Select(x => x.Id).ToList();
            SelectedSKUs = Items.Select(x => x.Id).ToList();
            string Error = "";
            //API Call
            try
            {
                long a = _orderService.GetNetSuiteInventoryFromAPI(UserId, ref Error);
                if (a != 1) { ViewBag.Error = Error; }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message + System.Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
            }
            List<NetSuiteInventory> finilised = _orderService.GetNetSuiteInventory(CustId, SelectedDC, SelectedSKUs, UserId);
            ViewBag.NetSuiteInventoryList = finilised.ToList();
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);

            return View("GetNetSuiteInventory");
        }
        //-Deepa
        public ActionResult GetNetSuiteInventoryDrillDown(string SelectedDCId, string SelectedSKU)
        {
            int CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            int UserId = Convert.ToInt32(SessionValues.UserId);
            var Locations = _storeService.GetCitiesforUserCustomer(UserId, CustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Order");
            var Stores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Order");
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(UserId, CustId);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(UserId, CustId);
            var Items = _itemService.GetSkuForUserCustomer(UserId, CustId, "Order");
            DateTime? FromDate = null;
            DateTime? ToDate = null;

            List<string> SelectedSKUs = new List<string>();
            List<int> SelectedDCs = new List<int>();

            List<int> SelectedTypes = ItemType.Select(x => x.Id).ToList();
            List<int> SelectedCategories = itemCategories.Select(x => x.Id).ToList();

            SelectedSKUs.Add(SelectedSKU);
            List<int> SelectedLocations = Locations.Select(x => x.LocationId).ToList();

            SelectedDCs.Add(Convert.ToInt32(SelectedDCId));
            List<string> SelectedStores = Stores.Select(x => x.Id.ToString()).ToList();

            List<ExcelOrder> ExcelOrders = _orderService.GetUploadedStoreItems(FromDate, ToDate, CustId, UserId, SelectedStores, SelectedSKUs, SelectedDCs, 1, 1, 0, SelectedCategories, SelectedTypes, SelectedLocations);

            ViewBag.InventoryDrillDown = ExcelOrders.ToList();

            return PartialView("_InventoryDrillDownPartial"); // Create  View WIP
        }

        public JsonResult GetNetSuiteInventoryForSelectedDC(string DCId, string SKUId)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            int iSKUId = Convert.ToInt32(SKUId);
            List<int> SelectedDC = new List<int>();
            List<int> SelectedSKUs = new List<int>();

            SelectedDC.Add(Convert.ToInt32(DCId));
            SelectedSKUs.Add(Convert.ToInt32(SKUId));

            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Order");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Order");

            SelectedDC = SelectedDC != null && SelectedDC.Count() > 0 && SelectedDC[0] != 0 ? SelectedDC : DCs.Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomer(userid, custid, "Order").Select(x => x.Id).ToList();

            List<NetSuiteInventory> finilised = _orderService.GetCustomerWiseDCWiseInventory(custid, SelectedDC, SelectedSKUs, userid);


            return Json(new { data = finilised }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region GBPLData 03 Aug 2021
        public ActionResult GBPLData()
        {
            int UserId = SessionValues.UserId;
            int CustId = Convert.ToInt32(SessionValues.LoggedInCustId);
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GetGBPLData()
        {
            int UserId = SessionValues.UserId;
            int CustId = Convert.ToInt32(SessionValues.LoggedInCustId);
            List<GBPA> Data = _orderService.GetGBPLData(CustId, UserId, DateTime.Now);
            ViewBag.GBPAData = Data.ToList();
            return View("GBPLData");
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GetGBPLDataExportExcel()
        {
            int UserId = SessionValues.UserId;
            int CustId = Convert.ToInt32(SessionValues.LoggedInCustId);
            List<GBPA> Data = _orderService.GetGBPLData(CustId, UserId, DateTime.Now);
            string Title = "GBPA Inventory";

            var finilisedList = Data.Select(head => new
            {
                head.GBPA_NUMBER,
                head.REVISION_NUMBER,
                head.ITEM_NUMBER,
                head.ITEM_DESCRIPTION,
                head.UOM,
                head.RATE,
                TS = head.TS.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
            });
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].Value = Title;
                workSheet.Cells[1, 1].Style.Font.Bold = true;

                workSheet.Cells[3, 1].Value = "GBPA_NUMBER";
                workSheet.Cells[3, 2].Value = "REVISION_NUMBER";
                workSheet.Cells[3, 3].Value = "ITEM_NUMBER";
                workSheet.Cells[3, 4].Value = "ITEM_DESCRIPTION";
                workSheet.Cells[3, 5].Value = "UOM";
                workSheet.Cells[3, 6].Value = "RATE";
                workSheet.Cells[3, 7].Value = "TS";

                workSheet.Cells[4, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "GBPAInventory.xlsx");
            }
        }

        #endregion

        #region TopItemListCustomerWise
        public ActionResult TopItemList(int? Id = 1)
        {
            int userid = SessionValues.UserId;
            int custid = SessionValues.LoggedInCustId.Value;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.SelectedTopItems = Id;
            return View();
        }
        [HttpPost]
        public ActionResult GetTopItemList(int Id, List<int> SelectedItemType, List<int> SelectedItemcategories)
        {
            int userid = SessionValues.UserId;
            int custid = SessionValues.LoggedInCustId.Value;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            if (SelectedItemType != null && SelectedItemType.Count() > 0 && SelectedItemType[0] != 0)
            {
                Itemcategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedItemType);
            }
            else
            {
                SelectedItemType = ItemType.Select(X => X.Id).ToList();
            }
            if (SelectedItemcategories == null || SelectedItemcategories.Count() <= 0 || SelectedItemcategories[0] == 0)
            {
                SelectedItemcategories = Itemcategories.Select(x => x.Id).ToList();
            }
            List<TopItemCustomers> TopItemLists = _orderService.GetTopItemsForCustomer(userid, custid, Id, SelectedItemType, SelectedItemcategories);
            JsonResult result = Json(new { data = TopItemLists }, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
            return result;
        }
        [HttpPost]
        public ActionResult UpdateTopItemList(List<TopItemCustomers> Items, int Id, List<int> SelectedItemType, List<int> SelectedItemcategories)
        {
            int userid = SessionValues.UserId;
            int custid = SessionValues.LoggedInCustId.Value;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            if (SelectedItemType != null && SelectedItemType.Count() > 0 && SelectedItemType[0] != 0)
            {
                Itemcategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedItemType);
            }
            else
            {
                SelectedItemType = ItemType.Select(X => X.Id).ToList();
            }
            if (SelectedItemcategories == null || SelectedItemcategories.Count() <= 0 || SelectedItemcategories[0] == 0)
            {
                SelectedItemcategories = Itemcategories.Select(x => x.Id).ToList();
            }
            int id = _orderService.UpdateTopItemList(Items, Id, SelectedItemType, SelectedItemcategories, userid, custid);
            JsonResult result = Json(new { data = Items }, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
            return result;
        }
        #endregion

        public JsonResult GetDataForCustomer(int CustId, string data)
        {
            int userid = SessionValues.UserId;
            if (data == "DC")
            {
                List<WareHouseDCView> dcs = _wareHouseService.GetWareHouseForCustomerId(userid, CustId, "Order");
                return Json(new { data = dcs }, JsonRequestBehavior.AllowGet);
            }
            if (data == "DC")
            {
                List<ItemView> items = _itemService.GetSkuForUserCustomer(userid, CustId, "Order");
                return Json(new { data = items }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MissingCSV()
        {
            return View();
        }
        #region mrunali_finalizedOrderDelete
        [AdminAuthorization]
        public ActionResult OrderDeleteNew()
        {
            int userid = Convert.ToInt32(SessionValues.UserId);
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Order");


            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");

            var ReasonList = _orderService.GetReasonlist("2");
            ViewBag.Resons = new SelectList(ReasonList.ToList(), "Id", "Reason", null);
            DateTime now = DateTime.Now;
            var startDate = now.AddDays(-1).Date;
            var endDate = startDate.AddDays(1).Date;
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = null;
            var selectedDCs = DCs.Select(x => x.Id).ToList();
            var selectedStores = Stores.Select(x => x.Id).ToList();

            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminApiAuthorization]
        public ActionResult OrderDeleteNew(DateTime? FromDate, DateTime? ToDate, string RFPLID, List<int> SelectedDC, List<int> SelectedStores, string hdnIds)
        {
            String Action = "2";
            int userid = Convert.ToInt32(SessionValues.UserId);
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Order");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Order");
            var ReasonList = _orderService.GetReasonlist(Action);

            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["RFPLID"] = RFPLID;
            SelectedDC = SelectedDC != null && SelectedDC.Count() > 0 ? SelectedDC : DCs.Select(x => x.Id).ToList();
            SelectedStores = SelectedStores != null && SelectedStores.Count() > 0 ? SelectedStores : Stores.Select(x => x.Id).ToList();
            //  SelectedSKUs = SelectedSKUs != null && SelectedSKUs.Count() > 0 ? SelectedSKUs : Items.Select(x => x.Id).ToList();
            List<OrderDeleteNew> finilised = _orderService.USP_FinlizedOrderDeleteSearch(startDate, endDate, custid, SelectedDC, SelectedStores, RFPLID);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.Resons = new SelectList(ReasonList.ToList(), "Id", "Reason", null);
            ViewBag.FinalizedReports = finilised.ToList();
            return View();
        }

        [HttpPost]
        [CustomApiAuthorization]
        public JsonResult OrderDeleteNewItems(List<OrderDeleteItems> Items)
        {
            try
            {
                int userid = Convert.ToInt32(SessionValues.UserId);
                string affected = _orderService.FinlizedOrderDelete(Items, userid);
                bool cond = affected != "" ? true : false;
                return Json(cond, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }


        #endregion

        #region PONumber
        [HttpGet]
        public ActionResult GetPONumber()
        {

            try
            {
                int StoreId = Convert.ToInt32(SessionValues.StoreId);
                string PoNumber = _orderService.GetPoNumber(StoreId, SessionValues.LoggedInCustId, SessionValues.UserId);
                return Json(new { PoNumber = PoNumber }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        #endregion
        #region 

        [AdminAuthorization]
        public ActionResult OrderEdit()
        {
            int userid = Convert.ToInt32(SessionValues.UserId);
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            //var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Order");
            //var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Order");
            //var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Order");
            //ViewBag.FinalizedReports = _orderService.OrderEditSearch();
            DateTime now = DateTime.Now;
            var startDate = now.AddDays(-1).Date;
            var endDate = startDate.AddDays(1).Date;
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["FromDate"] = sd;
            ViewData["ToDate"] = ed;
            ViewData["OrderId"] = null;
            //var selectedDCs = DCs.Select(x => x.Id).ToList();
            //var selectedStores = Stores.Select(x => x.Id).ToList();
            //var SelectedSKUs = Items.Select(x => x.Id).ToList();
            //ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            //ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            //ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            return View();
        }
        [AdminAuthorization]
        [HttpPost]
        public ActionResult OrderEdit(DateTime? FromDate, DateTime? ToDate, string OrderId)
        {
            int userid = Convert.ToInt32(SessionValues.UserId);
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            
            ViewBag.FinalizedReports = _orderService.OrderEditSearch(FromDate, ToDate, OrderId);
            DateTime now = DateTime.Now;
            var startDate = now.AddDays(-1).Date;
            var endDate = startDate.AddDays(1).Date;
            string sd = FromDate.HasValue ? FromDate.Value.Date.ToString("yyyy-MM-dd") : "";
            string ed = ToDate.HasValue ? ToDate.Value.Date.ToString("yyyy-MM-dd") : "";
            ViewData["FromDate"] = sd;
            ViewData["ToDate"] = ed;
            ViewData["OrderId"] = OrderId;
           
            return View();
        }


        //[AdminAuthorization]
        //public ActionResult OrderEdit()
        //{
        //    ViewBag.FinalizedReports = _orderService.OrderEditSearch();
        //    return View();
        //}


        public ActionResult EditOrderdetails(int Id)
        {
            var FinalizedReports = _orderService.OrderEditDetails(Id);
            if (FinalizedReports == null)
            {
                return HttpNotFound();
            }
            return View(FinalizedReports);
        }
        //[HttpPost]
        //public ActionResult EditOrderdetails(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID)
        //{
        //    var orders = _orderService.OrderEditDetails(FromDate, ToDate, UniqueReferenceID); 


        //    ViewBag.FinalizedReports = orders;


        //    return View("OrderEdit"); 
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOrderdetails(int Id, string OrderQuantity)
        {
            var updatedOrder = _orderService.OrderEditDetails(Id);

            if (updatedOrder == null)
            {
                return HttpNotFound();
            }
            updatedOrder.Id = Id;
            updatedOrder.OrderQuantity = OrderQuantity;
            var affected = _orderService.UpdateOrders(Id, OrderQuantity);
            return View(updatedOrder);
        }



        #endregion


    }
}