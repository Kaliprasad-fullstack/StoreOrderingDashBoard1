using BAL;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DbLayer.Repository
{
    public interface IOrderRepository
    {
        long SaveOrderHeader(OrderHeader orderHeader);
        long SaveOrderDetail(OrderDetail orderDetail);
        OrderHeader GetOrderHeader(long orderid);
        List<OrderHeader> GetSaveAsDraftOrders(int storeid);
        List<OrderDetail> GetOrderDetails(long Orderheaderid);
        int UpdateOrderHeader(OrderHeader orderHeader);
        int UpdateOrderDetails(OrderDetail orderDetail);
        OrderDetail GetOrderDetail(long orderid, int itemid);
        int deleteOrderDetail(OrderDetail orderDetail);
        List<OrderHeader> GetFinilizeOrder(int storeid);
        List<OrderHeader> GetProcessedOrder(int storeid);
        int InsertTemp(TempTbl tempTbl);
        List<TempTbl> tempTbls(long headerid);
        int DeleteTemptbl(long headerid);
        int DeleteParticularItem(long headerid, int productid);
        List<ProcessCls> GetFinilisedForProcess();
        List<NetSuitUpload> netSuitUploads(DateTime FromDate, DateTime ToDate);
        List<NetSuitUpload> netSuitUploadsExcel(DateTime FromDate, DateTime ToDate, int custid, int userid);
        TempTbl TempTbl(long orderheaderid, int itemid);
        int UpdateQtyInTemp(TempTbl tempTbl);
        List<OrderHeader> MakeaCopy(int storeid);
        List<DetailReport> SoApprove(int storeid);
        List<OrderHeader> FullfillmentCount(int storeid);
        List<OrderHeader> InvoiceCount(int storeid);
        int DeleteOrder(long orderid, int userid);
        string ItemForCustomer(string StoreCode, string SkuCode, int loggedInCustomer, int loggedInUser, string uniqueReferenceID);
        long InsertOrderHeader(string StoreCode, int UserId, string orderReferenceNo, byte? OrderSource, DateTime? OrderDate);
        long InsertOrderDetail(string ItemCode, int Qty, long OrderHeaderId, string uniqueReferenceID);
        int DeleteFinalisedOrder(long Id, string reason, int userid);
        int UpdateOrder(long Orderheaderid, int Itemid, int Quantity, int userid);
        List<OFRLIFR> MakeCopyWithLFR(int storeid);
        List<OrderHeader> GetPlannedOrderNotProcessed(int storeID, int statusFlag);
        List<CurrentItemInventoryMst> GetCurrentItemInventoryMstForCustomer(int custid, int userid, List<int> storeIds);
        List<OrderHeader> GetDraftOrdersforAdminUser(int custid, int userid, List<int> allowedStores);
        List<OrderDetail> GetDraftOrderDetails(long orderId, int custid, int userid, List<int> storeIds);
        List<CurrentItemInventoryMst> GetCurrentItemInventoryMstForStoreandItemandCategory(int custid, int userid, List<int> storeIds, List<int> categories, List<int> items);
        List<SoApproveHeader> GetSoForOrder(string finalizedorderno);
        List<InvoiceHeader> GetInvoicesforSos(List<string> sonumbers);
        List<FullfillmentHeader> GetFullfillmentsforSos(List<string> sonumbers);
        InvoiceHeader GetInvoiceInformation(string invoiceNo);
        long savePODResponse(PODHeader pODHeader);
        long savePODDetailResponse(PODDetail pODDetail, int InvoiceDispatchId);
        List<PODDetail> GetPODDetails(long pk_PODHeader);
        void DeletePODHeader(long pk_PODHeader, int userid);
        List<InvoiceHeader> GetInvoicesforDateDC(DateTime invoiceDate, string selectedDCs);
        List<string> GetDistinctLocationsFromInvoice(int userid, int custid);
        long AddInvoiceDispatch(InvoiceDispatch invoiceDispatch);
        List<InvoiceHeader> GetInvoicesforCustomer(int custid, int userId);
        List<InvoiceHeader> SearchInvoicesforStoreInvoice(int custid, int userId, string storeCode, string invoiceNo);
        string VehicleDispursementErrors(string to_Address1, string product, string vehicleNo, string docNo);
        OrderFormatExcel GetUploadOrderFormat(int loggedInCustomer, string OrderType);
        List<PODListData> GetInvoicesforVehicle(string vehicleNo, int custid);
        List<Vehicle> GetAutoVehicles(string vehicleNo, int CustId);
        List<InvoiceDispatch> GetVehiclesDAforDate(DateTime dispatchDate);
        long SaveDAForVehicle(InvoiceDispatch invoiceDispatch);
        long InsertOrderData(int loggedInCustomer, string storeCode, string skuCode, string qty, DateTime? orderDate, string orderReferenceNo, string orderPlacedDate, string orderType, string city, string lastOrderDate, string lateDeliveryDate, string uniqueReferenceID, string packSize, string orderedQty, string caseSize, string cases, string state, string fromDC, string new_Build, string focus, string item_Type, string item_sub_Type, string takenDays, string slab);
        long SaveExcelFileData(OrderExcel import);
        List<ExcelOrder> GetUploadedStoreItems(DateTime? fromDate, DateTime? toDate, int custId, int userId, List<string> selectedStores, List<string> selectedSKUs, List<int> SelectedDCs, int selectedAgeing, int isSearch, int firstCall, List<int> selectedCategory, List<int> selectedTypes, List<int> selectedLocations);
        long SaveUploadedFile(UploadedFilesMst mst);
        long SaveUploadedFileToTemp(OrderExcel order, int CustId, int UserId);
        List<ExcelOrder> GetDataToGenerateCSV(DateTime fromDate, DateTime toDate, int custId, int userId);
        long InsertOrderHeaderCSV(string StoreCode, int UserId, string orderReferenceNo, byte? OrderSource, DateTime? OrderDate, int type, int DcId);
        long InsertOrderDetailCSV(string ItemCode, int Qty, long OrderHeaderId, string uniqueReferenceID, long ExcelId, int userId, int dCId);
        List<NetSuitUpload> netSuitUploadsExcel2(DateTime fromDate, DateTime toDate, int custid, int userid);
        List<NetSuitUpload> netSuitUploadsExcel_OrderHeaderIds(DateTime fromDate, DateTime toDate, int custid, int userid, List<long> OrderHeaderIds);
        List<UploadedFilesView> GetUploadedFileDetails(int custid, int userid, List<string> fileType);
        List<File_Error_LogsView> GetFileErrors(int custId, int userId, int fileId, string FileType);
        void DeletedIgnoredRow(DateTime? fromDate, DateTime? toDate, int custId, int userId, List<OrderExcel> excelOrders, List<int> selectedStores, List<int> SelectedSKUs, int SelectedAgeing, int isSearch, List<int> SelectedDCs, List<int> selectedCategory, List<int> selectedTypes, List<int> selectedLocations);
        List<ExcelOrder> GetDataQuantity(int custId, int userId, DateTime? FromDate, DateTime? ToDate, List<string> SelectedStores, List<int> selectedDCs, List<string> SelectedSKUs, int SelectedAgeing = 0, int Filtered = 0);
        List<ExcelOrder> GetDataToGenerateCSVSearch(DateTime? fromDate, DateTime? toDate, int custId, int userId, List<int> selectedStores, List<int> selectedSKUs, int selectedAgeing, int Filtered, List<int> selectedDCs, List<int> selectedCategories, List<int> selectedTypes, List<int> selectedLocations);
        List<long> SaveGenerateCSV(DateTime? fromDate, DateTime? toDate, int custId, int userId, List<int> selectedStores, List<int> selectedSKUs, List<int> selectedDCs, int selectedAgeing, int Filtered, List<int> selectedCategories, List<int> selectedTypes, List<int> selectedLocations);
        void disableOrderForNetsuiteUpload(List<string> POList);
        List<CategoryWiseReasonMaster> GetReasonlist(string categoryid);
        int UpdateStatus(InvoiceDispatch invoiceDispatch);
        List<storeBO> GetStoreName(string invoiceVal);
        long AddUpdatePODH(PODheaderBO invoiceDispatch);
        //14 Nov 2019
        OrderSaveResponse SaveStoreOrderToOrderExcel(OrderExcel orderExcel, string issave);
        List<StoreOrderVB> GetStoreDraftOrder(int storeID, int custId, int userId);
        long UpdateDraftQuantity(int storeId, int itemId, int quantity, string method, int userid);
        OrderSaveResponse FinilizeDraft(OrderExcel orderExcel, string issave);
        long DeleteStoreDraft(int userId, int storeId);
        long GetMaxGroupIDByCustomer(int CustId);
        List<TrackingBO> GetTrackingList(string action, string uniqueReferenceID, string invoiceNo, int custid);
        List<TrackingBO> GetStatusList(string uniqueReferenceID, string invoiceNo, int custid);
        List<StoreOrders> GetLastStoreOrders(string storeCode, int StoreId, int CustId, DateTime today);
        List<StoreOrderDetailsVB> GetLastStoreOrdersByDateStoreCode(DateTime storeOrderDate, string storeCode, int StoreId, int CustId, long? GroupId, string PONumber);
        List<OrderDeleteClose> OrderDeleteCloseSearch(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, string Action);
        string OrderDeleteClose(List<OrderDeleteItems> Ids, string Action, int userid);
        #region SaveCVSFile By nikhil 2019/12/06
        long SaveCSVFileDetails(string filename, string path, int custId, int userId, DateTime noW);
        List<CSVFileMasterVM> GetCSVFileDetails(int custId, int userId, DateTime date, DateTime toDate);
        long SavePODImg(PODImages pODImages);
        OrderInboundResponse AddOrderInbound(OrderInbound orders);
        #endregion
        #region POD Bulk Update
        List<PODBulkUpdate> GetInvoiceForBulkUpdateByVehicleNo(string vehicleNo, int custid);
        #endregion

        long UploadExcelFileData(OrderExcelMapping import);
        long UploadInwardFileData(OrderExcelInward import);
        long ProcessCompleteExcelUploadOutward(string uniqueOrderNo, long fileId, int loggedInCustomer);
        long UploadOrderInventoryFileData(OrderInventory item);
        #region NetsuiteInventory
        List<NetSuiteInventory> GetNetSuiteInventory(int custId, List<int> DCID, List<int> SKUID, int? userId);
        void InsertNetSuiteInventoryBulkUpload(List<NetSuiteInventoryAPIResponse> lstAPIResponse, int userId);
        List<NetSuiteInventory> GetCustomerWiseDCWiseInventory(int custId, List<int> DCIDs, List<int> SKUIds, int? userId);
        List<GBPA> GetGBPLData(int custId, int userId, DateTime date);
        List<TopItemCustomers> GetTopItemsForCustomer(int userid, int custid, int SelectedTopItems, List<int> selectedItemType, List<int> selectedItemcategories);
        int UpdateTopItemList(List<TopItemCustomers> items, int id, List<int> selectedItemType, List<int> selectedItemcategories, int userid, int custid);
        List<NetSuitUpload> NetSuitUploadsExcel_OrderHeaderIds(int custId, int userId, List<long> orderHeaderIds, List<ExcelOrder> excelOrders, string orderExcel);
        long RemoveFromViewCSVList(List<int> excelOrderIds, int custId, int userId);
        #endregion

        List<OrderDeleteNew> USP_FinlizedOrderDeleteSearch(DateTime startDate, DateTime endDate, int custid, List<int> dCs, List<int> stores, string RFPLID);
        string FinlizedOrderDelete(List<OrderDeleteItems> Ids, int userid);
        string GetPoNumber(int storeId, int? loggedInCustId, int loggedinuserid);
        string ImportData(DataTable ds, string sp_name, string sysfilename1, int userId);
        string OrderEdit(List<OrderEdit> id);
        List<OrderEdit> OrderEditSearch(DateTime? FromDate, DateTime? ToDate, string OrderId);
        OrderEdit OrderEditdetails(int id);
        string UpdateOrders(int Id, string OrderQuantity);
        List<ExcelOrder> GenerateOrder(DateTime? fromDate, DateTime? toDate, int custId, int userId, List<string> selectedStores, List<string> selectedSKUs, List<int> SelectedDCs, int? selectedAgeing, int? isSearch, int? firstCall, List<int> selectedCategory, List<int> selectedTypes, List<int> selectedLocations,string Unique_Reference_Id);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly StoreContext _context;

        public OrderRepository()
        {

        }

        public OrderRepository(StoreContext context)
        {
            _context = context;
        }
        #region DeleteFinlizedOrder
        public List<OrderDeleteNew> USP_FinlizedOrderDeleteSearch(DateTime startDate, DateTime endDate, int custid, List<int> dCs, List<int> stores, string RFPLID)
        {
            try
            {
                _context.Database.CommandTimeout = 460;
                string Str = string.Format("exec USP_FinlizedOrderDeleteSearch @StoreOrderFrom='{0}', @StoreOrderToDate='{1}',@StoreCode='{2}', @DCCode='{3}', @RFPLID='{4}', @cust_id='{5}'",
                    startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), stores != null ? string.Join(",", stores.ToArray()) : "", dCs != null ? string.Join(",", dCs.ToArray()) : "", RFPLID != null ? RFPLID : "", custid);
                return _context.Database.SqlQuery<OrderDeleteNew>(Str).ToList();

                //string Str = string.Format("exec USP_OrderDeleteCloseSearch @StoreOrderFrom='{0}', @StoreOrderToDate='{1}',@StoreCode='{2}', @DCCode='{3}', @SKUCode='{4}', @UniqueRefNo='{5}', @Action='{6}'", 
                //    startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), stores != null ? string.Join(",", stores.ToArray()) : "", dCs != null ? string.Join(",", dCs.ToArray()) : "", items != null ? string.Join(",", items.ToArray()) : "", uniqueReferenceID != null ? uniqueReferenceID : "", Action);
                //return _context.Database.SqlQuery<OrderDeleteClose>(Str).ToList();

            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public string FinlizedOrderDelete(List<OrderDeleteItems> Ids, int userid)
        {
            try
            {
                foreach (OrderDeleteItems item in Ids)
                {
                    // Set a longer timeout (e.g., 60 seconds)
                    _context.Database.CommandTimeout = 360;
                    string result = _context.Database.SqlQuery<string>(string.Format("exec USP_DeleteFinlizedOrder @OrderHeaderId={0}, @UserId='{1}',@Reason='{2}'",
                       item.ProductId,
                       userid,
                      item.Reason
                      )).ToList().FirstOrDefault();
                }
                return "Result";
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }


        //public int DeletefinlizedOrder(int  orderid, int userid,string Reason)
        //{
        //    try
        //    {
        //        return _context.Database.SqlQuery<int>("exec USP_DeleteFinlizedOrder @OrderHeaderId,@UserId,@Reason",
        //          new SqlParameter("@OrderHeaderId", orderid),
        //          new SqlParameter("@UserId", userid),
        //          new SqlParameter("@Reason", Reason)).ToList().FirstOrDefault();
        //    }
        //    catch (Exception ex)
        //    {
        //        HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
        //        throw ex;
        //    }
        //}
        #endregion
        //public long SaveOrderDetail(OrderDetail orderDetail)
        //{
        //    try
        //    {
        //        var item = _context.Items.Find(orderDetail.ItemId).IsDeleted;
        //       if(!item)
        //        {
        //            var entity = _context.orderDetails.Add(orderDetail);
        //            _context.SaveChanges();
        //            return entity.Id;
        //        }
        //        return 0;
        //    }
        //    catch(Exception ex)
        //    {
        //        HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
        //        throw ex;
        //    }
        //}
        public long SaveOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                var customer = _context.Stores.Where(x => x.IsDeleted == false && x.Id == orderDetail.StoreId).FirstOrDefault().Customer;
                if (customer != null && customer.IsDeleted == false)
                {
                    if (customer.Items.Where(x => x.Id == orderDetail.ItemId && x.IsDeleted == false).FirstOrDefault() != null)
                    {
                        var item = customer.Items.Where(x => x.Id == orderDetail.ItemId && x.IsDeleted == false).FirstOrDefault().IsDeleted;
                        if (!item)
                        {
                            var entity = _context.orderDetails.Add(orderDetail);
                            _context.SaveChanges();
                            return entity.Id;
                        }
                        return 0;
                    }
                    return 0;
                }
                return 0;
                //var item = _context.Items.Find(orderDetail.ItemId).IsDeleted;
                ////var item = _context.Stores.Where(x => x.Id == orderDetail.StoreId).Join(_context.Customers, store => store.Id, customer => customer.Id,
                ////    (store, customer) => new { store, customer }).Join(_context.Items, items => items.customer.Id, itemselected => itemselected.Id, (Customer, Item) => new { Customer, Item }).
                ////    Where(items => items.Item.IsDeleted == false && items.Customer.store.IsDeleted == false && items.Customer.customer.IsDeleted == false && items.Item.Id==orderDetail.ItemId).Select(m => m.Item.IsDeleted).FirstOrDefault();
                //if (!item)
                //{
                //    var entity = _context.orderDetails.Add(orderDetail);
                //    _context.SaveChanges();
                //    return entity.Id;
                //}
                //return 0;
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public long SaveOrderHeader(OrderHeader orderHeader)
        {
            try
            {

                var entity = _context.orderHeaders.Add(orderHeader);
                _context.SaveChanges();
                return entity.Id;

            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public OrderHeader GetOrderHeader(long orderid)
        {
            try
            {
                return _context.orderHeaders.Where(x => x.Isdeleted == false && x.Id == orderid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<OrderHeader> GetSaveAsDraftOrders(int storeid)
        {
            //try
            //{
            //    var a = _context.orderHeaders.ToList();
            //    return _context.orderHeaders.Where(x => x.IsOrderStatus == false && x.StoreId == storeid && x.IsProcessed == false && x.IsSoApprove == false && x.Isdeleted == false && x.IsFullFillment == false && x.IsInvoice == false).OrderByDescending(x => x.OrderDate).ToList();
            //}
            //catch (Exception ex)
            //{
            //    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
            //    throw ex;
            //}

            try
            {
                return _context.Database.SqlQuery<OrderHeader>("exec USP_GetDraftAndPlannedOrders @StoreId",
                  new SqlParameter("@StoreId", storeid)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<OrderDetail> GetOrderDetails(long Orderheaderid)
        {
            try
            {
                return _context.orderDetails.Where(x => x.OrderHeaderId == Orderheaderid).ToList();
                //var OrderDetails= from OrderDetail in _context.orderDetails
                //                  join OrderHeader in _context.orderHeaders 
                //                  on OrderHeader.
                //                  where OrderDetail.OrderHeaderId=Orderheaderid
                //return OrderDetails;
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int UpdateOrderHeader(OrderHeader orderHeader)
        {
            try
            {
                _context.Entry(orderHeader).State = EntityState.Modified;
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int UpdateOrderDetails(OrderDetail orderDetail)
        {
            try
            {
                _context.Entry(orderDetail).State = EntityState.Modified;
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public OrderDetail GetOrderDetail(long orderid, int itemid)
        {
            try
            {
                return _context.orderDetails.Where(x => x.OrderHeaderId == orderid && x.ItemId == itemid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int deleteOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                _context.Entry(orderDetail).State = EntityState.Deleted;
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<OrderHeader> GetFinilizeOrder(int storeid)
        {
            try
            {
                return _context.orderHeaders.Where(x => x.IsOrderStatus == true && x.IsProcessed == false && x.IsSoApprove == false && x.StoreId == storeid && x.Isdeleted == false && x.IsFullFillment == false && x.IsInvoice == false).OrderByDescending(x => x.OrderDate).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<OrderHeader> GetProcessedOrder(int storeid)
        {
            try
            {
                return _context.orderHeaders.Where(x => x.IsProcessed == true && x.IsOrderStatus == true && x.StoreId == storeid && x.Isdeleted == false && x.IsSoApprove == false && x.IsFullFillment == false && x.IsInvoice == false).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int InsertTemp(TempTbl tempTbl)
        {
            try
            {
                _context.tempTbls.Add(tempTbl);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<TempTbl> tempTbls(long headerid)
        {
            try
            {
                return _context.tempTbls.Where(x => x.headerid == headerid).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int DeleteTemptbl(long headerid)
        {
            try
            {
                int affectedrow = 0;
                var temptbl = _context.tempTbls.Where(x => x.headerid == headerid).ToList();
                foreach (TempTbl tem in temptbl)
                {
                    _context.Entry(tem).State = EntityState.Deleted;
                    affectedrow = _context.SaveChanges();
                }
                return affectedrow;
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int DeleteParticularItem(long headerid, int productid)
        {
            try
            {
                var tempbl = _context.tempTbls.Where(x => x.headerid == headerid && x.ProductId == productid).FirstOrDefault();
                _context.Entry(tempbl).State = EntityState.Deleted;
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<ProcessCls> GetFinilisedForProcess()
        {
            try
            {
                //var process = _context.GetFinilsedOrder();
                //List<ProcessCls> processlist = new List<ProcessCls>();
                //var processlist = process.ToList();
                //var context = _context.DataContext();
                //{
                //return _context.Database.SqlQuery<MenuItem>(string.Format("exec spManageMenu @Opr={0},@RoleId={1}", "GET", RoleId)).ToList();
                return _context.Database.SqlQuery<List<ProcessCls>>("exec USP_GetFiniliseOrder").FirstOrDefault();
                //}

                //return process;
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<NetSuitUpload> netSuitUploads(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                StoreContext context = new StoreContext();
                return context.Database.SqlQuery<NetSuitUpload>("exec USP_UploadNetSuitData @FromDate,@ToDate",
                  new SqlParameter("@FromDate", FromDate),
                  new SqlParameter("@ToDate", ToDate)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<NetSuitUpload> netSuitUploadsExcel(DateTime FromDate, DateTime ToDate, int custid, int userid)
        {
            try
            {

                return _context.Database.SqlQuery<NetSuitUpload>("exec USP_UploadNetSuitExcelData @FromDate,@ToDate,@CustId,@UserId",
                  new SqlParameter("@FromDate", FromDate),
                  new SqlParameter("@ToDate", ToDate),
                  new SqlParameter("@CustId", custid),
                  new SqlParameter("@UserId", userid)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public TempTbl TempTbl(long orderheaderid, int itemid)
        {
            try
            {
                return _context.tempTbls.Where(x => x.headerid == orderheaderid && x.ProductId == itemid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateQtyInTemp(TempTbl tempTbl)
        {
            try
            {
                _context.Entry(tempTbl).State = EntityState.Modified;
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<OrderHeader> MakeaCopy(int storeid)
        {
            try
            {
                //return _context.Database.SqlQuery<OrderHeader>("exec [USP_Top5Order] @StoreId",
                // new SqlParameter("@StoreId", storeid)).ToList();
                return _context.orderHeaders.Where(x => x.IsOrderStatus == true && x.StoreId == storeid && x.Isdeleted == false).OrderByDescending(x => x.OrderDate).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<DetailReport> SoApprove(int storeid)
        {
            try
            {

                return _context.Database.SqlQuery<DetailReport>("exec USP_SoApproveByStore @StoreId",
                  new SqlParameter("@StoreId", storeid)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<OrderHeader> FullfillmentCount(int storeid)
        {
            try
            {
                return _context.orderHeaders.Where(x => x.IsProcessed == true && x.IsOrderStatus == true && x.StoreId == storeid && x.Isdeleted == false && x.IsSoApprove == true && x.IsFullFillment == true && x.IsInvoice == false).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<OrderHeader> InvoiceCount(int storeid)
        {
            try
            {
                return _context.orderHeaders.Where(x => x.IsProcessed == true && x.IsOrderStatus == true && x.StoreId == storeid && x.Isdeleted == false && x.IsSoApprove == true && x.IsFullFillment == true && x.IsInvoice == true).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int DeleteOrder(long orderid, int userid)
        {
            try
            {
                return _context.Database.SqlQuery<int>("exec USP_DeleteOrder @OrderHeaderId,@UserId",
                  new SqlParameter("@OrderHeaderId", orderid),
                  new SqlParameter("@UserId", userid)).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public string ItemForCustomer(string StoreCode, string SkuCode, int loggedInCustomer, int LoggedInUser, string uniqueReferenceID)
        {
            try
            {
                return _context.Database.SqlQuery<string>("exec USP_IsCustomerItem @StoreCode, @SkuCode, @LoggedInCustomer, @LoggedInUser, @uniqueReferenceID",
                  new SqlParameter("@StoreCode", StoreCode),
                  new SqlParameter("@SkuCode", SkuCode),
                  new SqlParameter("@LoggedInCustomer", loggedInCustomer),
                  new SqlParameter("@LoggedInUser", LoggedInUser),
                  new SqlParameter("@uniqueReferenceID", uniqueReferenceID != null ? uniqueReferenceID : "")
                  ).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public long InsertOrderHeader(string StoreCode, int UserId, string orderReferenceNo, byte? OrderSource, DateTime? OrderDate)
        {
            try
            {
                //return _context.Database.SqlQuery<long>("exec USP_InsertOrderHeader @StoreCode,@UserId",
                //  new SqlParameter("@StoreCode", StoreCode),
                //  new SqlParameter("@UserId", UserId)).ToList().FirstOrDefault();
                return _context.Database.SqlQuery<long>("exec USP_InsertOrderHeader @StoreCode, @UserId, @orderReferenceNo, @orderDate, @OrderSource",
                  new SqlParameter("@StoreCode", StoreCode),
                  new SqlParameter("@UserId", UserId),
                  new SqlParameter("@OrderSource", OrderSource),
                  new SqlParameter("@orderReferenceNo", orderReferenceNo != null ? orderReferenceNo : ""),
                  new SqlParameter("@OrderDate", OrderDate)).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public long InsertOrderDetail(string ItemCode, int Qty, long OrderHeaderId, string uniqueReferenceID)
        {
            try
            {
                return _context.Database.SqlQuery<long>("exec USP_InsertOrderDetail @ItemCode, @Qty, @OrderHeaderId, @uniqueReferenceID",
                  new SqlParameter("@ItemCode", ItemCode),
                  new SqlParameter("@Qty", Qty),
                  new SqlParameter("@OrderHeaderId", OrderHeaderId),
                  new SqlParameter("@uniqueReferenceID", uniqueReferenceID != null ? uniqueReferenceID : "")).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int DeleteFinalisedOrder(long Id, string reason, int userid)
        {
            try
            {
                var order = _context.orderHeaders.Where(x => x.Id == Id && x.Isdeleted == false).FirstOrDefault();
                order.Isdeleted = true;
                order.ModifiedBy = userid;
                order.Remark = reason;
                _context.Entry(order).State = EntityState.Modified;
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int UpdateOrder(long Orderheaderid, int Itemid, int Quantity, int userid)
        {
            try
            {
                return _context.Database.SqlQuery<int>("exec USP_UpdateItem @OrderHeaderId,@ItemId,@Quantity,@UserId",
                  new SqlParameter("@OrderHeaderId", Orderheaderid),
                  new SqlParameter("@ItemId", Itemid),
                  new SqlParameter("@Quantity", Quantity),
                  new SqlParameter("@UserId", userid)).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<OFRLIFR> MakeCopyWithLFR(int storeid)
        {
            try
            {
                return _context.Database.SqlQuery<OFRLIFR>("exec [USP_Top5Order] @StoreId",
               new SqlParameter("@StoreId", storeid)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }

        }
        public List<OrderHeader> GetPlannedOrderNotProcessed(int storeID, int statusFlag)
        {
            try
            {
                return _context.Database.SqlQuery<OrderHeader>("exec USP_GetItemDetails @StoreId, @StatusFlag",
               new SqlParameter("@StoreId", storeID),
               new SqlParameter("@StatusFlag", statusFlag)
               ).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<CurrentItemInventoryMst> GetCurrentItemInventoryMstForCustomer(int custid, int userid, List<int> storeIds)
        {
            try
            {
                // return _context.Database.SqlQuery<CurrentItemInventoryMst>("exec USP_GetItemDetails @StoreId, @StatusFlag",
                //new SqlParameter("@StoreId", storeID),
                //new SqlParameter("@StatusFlag", statusFlag)
                //).ToList();
                return _context.currentItemInventoryMsts.Where(x => x.CustomerId == custid && x.Customer.IsDeleted == false && x.Item.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<OrderHeader> GetDraftOrdersforAdminUser(int custid, int userid, List<int> allowedStores)
        {
            try
            {
                return _context.orderHeaders.Where(x => allowedStores.Contains(x.StoreId) && x.IsOrderStatus == false && x.Store.IsDeleted == false && x.Isdeleted == false).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<OrderDetail> GetDraftOrderDetails(long orderId, int custid, int userid, List<int> storeIds)
        {
            try
            {
                List<OrderDetail> OrderDetail = null;
                var order = _context.orderHeaders.Where(x => storeIds.Contains(x.StoreId) && x.Store.IsDeleted == false && x.Isdeleted == false && x.Id == orderId && x.IsOrderStatus == false).FirstOrDefault();
                if (order != null)
                {
                    OrderDetail = _context.orderDetails.Where(x => x.OrderHeaderId == order.Id).ToList();
                }
                return OrderDetail;
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<CurrentItemInventoryMst> GetCurrentItemInventoryMstForStoreandItemandCategory(int custid, int userid, List<int> storeIds, List<int> categories, List<int> items)
        {
            try
            {
                if (categories != null && items != null)
                    return _context.currentItemInventoryMsts.Where(x => x.CustomerId == custid && x.Customer.IsDeleted == false
                    && storeIds.Contains(x.Store.Id) && x.Store.IsDeleted == false
                    && categories.Contains(x.Item.Category.Id) && x.Item.Category.isDeleted == false
                    && items.Contains(x.Item.Id) && x.Item.IsDeleted == false
                    ).ToList();
                else if (categories != null && items == null)
                    return _context.currentItemInventoryMsts.Where(x => x.CustomerId == custid && x.Customer.IsDeleted == false
                   && storeIds.Contains(x.Store.Id) && x.Store.IsDeleted == false
                   && categories.Contains(x.Item.Category.Id) && x.Item.Category.isDeleted == false
                   && x.Item.IsDeleted == false
                   ).ToList();
                else if (categories == null && items != null)
                    return _context.currentItemInventoryMsts.Where(x => x.CustomerId == custid && x.Customer.IsDeleted == false
                   && storeIds.Contains(x.Store.Id) && x.Store.IsDeleted == false
                   && x.Item.Category.isDeleted == false
                   && items.Contains(x.Item.Id) && x.Item.IsDeleted == false
                   ).ToList();
                else
                    return _context.currentItemInventoryMsts.Where(x => x.CustomerId == custid && x.Customer.IsDeleted == false
                   && storeIds.Contains(x.Store.Id) && x.Store.IsDeleted == false
                   && x.Item.Category.isDeleted == false
                   && x.Item.IsDeleted == false
                   ).ToList();

            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<SoApproveHeader> GetSoForOrder(string finalizedorderno)
        {
            try
            {
                return _context.soApproveHeaders.Where(x => x.PO == finalizedorderno).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<InvoiceHeader> GetInvoicesforSos(List<string> sonumbers)
        {
            try
            {
                return _context.invoiceHeaders.Where(x => sonumbers.Contains(x.Invoice_Number)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<FullfillmentHeader> GetFullfillmentsforSos(List<string> sonumbers)
        {
            try
            {
                return _context.fullfillmentHeaders.Where(x => sonumbers.Contains(x.CreatedFrom)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public InvoiceHeader GetInvoiceInformation(string invoiceNo)
        {
            try
            {
                var invoiceheader = _context.invoiceHeaders.Where(x => x.Invoice_Number == invoiceNo).FirstOrDefault();
                //var PODHeaders = _context.PODHeaders.Where(x => x.Invoice_Number == invoiceNo && x.IsDeleted == false).ToList();
                //if (PODHeaders != null && PODHeaders.Count() > 0)
                //    return null;
                //else
                return invoiceheader;
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public long savePODResponse(PODHeader pODHeader)
        {
            try
            {
                return _context.Database.SqlQuery<long>("exec USP_InsertPODHeader @Invoice_Number,@CreatedBy,@PODSignImageFile,@PODTempImageFile,@PODDate",
                  new SqlParameter("@Invoice_Number", pODHeader.Invoice_Number),
                  new SqlParameter("@CreatedBy", pODHeader.CreatedBy != null ? pODHeader.CreatedBy : 1),
                  new SqlParameter("@PODSignImageFile", pODHeader.PODSignImageFile != null ? pODHeader.PODSignImageFile : ""),
                  new SqlParameter("@PODTempImageFile", pODHeader.PODTempImageFile != null ? pODHeader.PODTempImageFile : ""),
                  new SqlParameter("@CompanyId", pODHeader.CompanyId != null ? pODHeader.CompanyId : 0),
                  new SqlParameter("@PODDate", pODHeader.PODDate)).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<PODDetail> GetPODDetails(long pk_PODHeader)
        {
            try
            {
                return _context.PODDetails.Where(x => x.PODHeaderId == pk_PODHeader).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public void DeletePODHeader(long pk_PODHeader, int userid)
        {
            try
            {
                _context.Database.SqlQuery<int>("exec USP_DeletePOD @PODHeaderId,@UserId",
                 new SqlParameter("@PODHeaderId", pk_PODHeader),
                 new SqlParameter("@UserId", userid)).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<InvoiceHeader> GetInvoicesforDateDC(DateTime invoiceDate, string selectedDCs)
        {
            try
            {
                string datestring = invoiceDate.ToString("dd/MM/yyyy");
                _context.Configuration.ProxyCreationEnabled = false;
                var invoices = _context.invoiceHeaders.Where(x => x.InvoiceDate == datestring && x.CustomerZone == selectedDCs).Select(x => x.Invoice_Number).ToList();
                var invoicesdispatched = _context.InvoiceDispatchList.Where(x => invoices.Contains(x.InvoiceNo)).Select(x => x.InvoiceNo);
                return _context.invoiceHeaders.Where(x => x.InvoiceDate == datestring && x.CustomerZone == selectedDCs && !invoicesdispatched.Contains(x.Invoice_Number)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<string> GetDistinctLocationsFromInvoice(int userid, int custid)
        {
            try
            {
                return _context.invoiceHeaders.Select(x => x.CustomerZone).Distinct().ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public long AddInvoiceDispatch(InvoiceDispatch invoiceDispatch)
        {
            try
            {
                return _context.Database.SqlQuery<long>("exec USP_AddUpdateinvoiceDispatch @Invoice_Number, @CreatedBy, @VehicleNo",
              new SqlParameter("@Invoice_Number", invoiceDispatch.InvoiceNo),
              new SqlParameter("@CreatedBy", invoiceDispatch.CreatedBy),
              new SqlParameter("@VehicleNo", invoiceDispatch.VehicleNo)
              ).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<InvoiceHeader> GetInvoicesforCustomer(int custid, int userId)
        {
            try
            {
                return _context.Database.SqlQuery<InvoiceHeader>("exec USP_GetInvoicesforCustomer @CustId, @UserId",
              new SqlParameter("@CustId", custid),
              new SqlParameter("@UserId", userId)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<InvoiceHeader> SearchInvoicesforStoreInvoice(int custid, int userId, string storeCode, string invoiceNo)
        {
            try
            {
                return _context.Database.SqlQuery<InvoiceHeader>("exec USP_SearchInvoicesforStoreInvoice @CustId, @UserId, @StoreCode, @InvoiceNo",
              new SqlParameter("@CustId", custid),
              new SqlParameter("@UserId", userId),
              new SqlParameter("@StoreCode", storeCode),
              new SqlParameter("@InvoiceNo", invoiceNo != null ? invoiceNo : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public string VehicleDispursementErrors(string fromPlace, string trandate, string vehicleNo, string docNo)
        {
            try
            {
                return _context.Database.SqlQuery<string>("exec USP_VehicleDispursementErrors @fromPlace, @tranDate, @vehicleNo, @docNo",
              new SqlParameter("@fromPlace", fromPlace),
              new SqlParameter("@tranDate", trandate),
              new SqlParameter("@vehicleNo", vehicleNo),
              new SqlParameter("@docNo", docNo != null ? docNo : "")).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public OrderFormatExcel GetUploadOrderFormat(int loggedInCustomer, string OrderType)
        {
            try
            {
                return _context.OrderFormatExcel.Where(x => x.IsDeleted == false && x.CustId == loggedInCustomer && x.OrderType == OrderType).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<Vehicle> GetAutoVehicles(string vehicleNo, int CustId)
        {
            try
            {
                return _context.Database.SqlQuery<Vehicle>("exec USP_SearchVehicle @vehicleNo, @CustId",
              new SqlParameter("@vehicleNo", vehicleNo),
              new SqlParameter("@CustId", CustId)
              ).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<InvoiceDispatch> GetVehiclesDAforDate(DateTime dispatchDate)
        {
            try
            {
                return _context.Database.SqlQuery<InvoiceDispatch>("exec USP_SearchVehiclesDAforDate @dispatchDate",
              new SqlParameter("@dispatchDate", dispatchDate)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public long SaveDAForVehicle(InvoiceDispatch invoiceDispatch)
        {
            try
            {
                return _context.Database.SqlQuery<long>("exec USP_UpdateDAForVehicle @VehicleNo, @ModifiedDate, @ModifiedBy, @DA",
                    new SqlParameter("@VehicleNo", invoiceDispatch.VehicleNo),
                    new SqlParameter("@InvoiceNo", invoiceDispatch.InvoiceNo),
                    //new SqlParameter("@DispatchDate", invoiceDispatch.DispatchDate),
                    new SqlParameter("@ModifiedDate", invoiceDispatch.ModifiedDate),
                    new SqlParameter("@ModifiedBy", invoiceDispatch.ModifiedBy),
                    new SqlParameter("@DA", invoiceDispatch.DA != null ? invoiceDispatch.DA : "")
                    ).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public long InsertOrderData(int loggedInCustomer, string storeCode, string skuCode, string qty, DateTime? orderDate, string orderReferenceNo, string orderPlacedDate, string orderType, string city, string lastOrderDate, string lateDeliveryDate, string uniqueReferenceID, string packSize, string orderedQty, string caseSize, string cases, string state, string fromDC, string new_Build, string focus, string item_Type, string item_sub_Type, string takenDays, string slab)
        {
            try
            {
                return _context.Database.SqlQuery<long>("exec USP_AddOrderData @loggedInCustomer, @storeCode, @skuCode, @qty, @orderReferenceNo, @orderPlacedDate, @orderType, @city, @lastOrderDate, @lateDeliveryDate, @uniqueReferenceID, @packSize, @orderedQty, @caseSize, @cases, @state, @fromDC, @new_Build, @focus, @item_Type, @item_sub_Type, @takenDays, @slab",
                    new SqlParameter("@loggedInCustomer", loggedInCustomer),
                    new SqlParameter("@storeCode", storeCode),
                    new SqlParameter("@skuCode", skuCode),
                    new SqlParameter("@qty", qty),
                    new SqlParameter("@orderReferenceNo", orderReferenceNo),
                    new SqlParameter("@orderPlacedDate", orderPlacedDate == null ? "" : orderPlacedDate),
                    new SqlParameter("@orderType", orderType == null ? "" : orderType),
                    new SqlParameter("@city", city == null ? "" : city),
                    new SqlParameter("@lastOrderDate", lastOrderDate == null ? "" : lastOrderDate),
                    new SqlParameter("@lateDeliveryDate", lateDeliveryDate == null ? "" : lateDeliveryDate),
                    new SqlParameter("@uniqueReferenceID", uniqueReferenceID == null ? "" : uniqueReferenceID),
                    new SqlParameter("@packSize", packSize == null ? "" : packSize),
                    new SqlParameter("@orderedQty", orderedQty == null ? "" : orderedQty),
                    new SqlParameter("@caseSize", caseSize == null ? "" : caseSize),
                    new SqlParameter("@cases", cases == null ? "" : cases),
                    new SqlParameter("@state", state == null ? "" : state),
                    new SqlParameter("@fromDC", fromDC == null ? "" : fromDC),
                    new SqlParameter("@new_Build", new_Build == null ? "" : new_Build),
                    new SqlParameter("@focus", focus == null ? "" : focus),
                    new SqlParameter("@item_Type", item_Type == null ? "" : item_Type),
                    new SqlParameter("@item_sub_Type", item_sub_Type == null ? "" : item_sub_Type),
                    new SqlParameter("@takenDays", takenDays == null ? "" : takenDays),
                    new SqlParameter("@slab", slab == null ? "" : slab)
                    ).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<ExcelOrder> GetUploadedStoreItems(DateTime? fromDate, DateTime? toDate, int custId, int userId, List<string> selectedStores, List<string> selectedSKUs, List<int> SelectedDCs, int selectedAgeing, int isSearch, int firstCall, List<int> selectedCategory, List<int> selectedTypes, List<int> selectedLocations)
        {
            try
            {
                return _context.Database.SqlQuery<ExcelOrder>(
                        string.Format("EXEC USP_GetUploded_Store_Items @FromDate = '{0}',@ToDate = '{1}', @UserId = {2},@CustId = {3}, @Stores='{4}',@SKUs='{5}',@Ageing={6},@isSearch={7},@FirstCall={8},@DCs='{9}',@ItemCategories='{10}',@ItemTypes='{11}',@Locations='{12}'",
                       fromDate.HasValue ? fromDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "",
                        toDate.HasValue ? toDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "",
                        userId, custId,
                        string.Join(",", selectedStores.ToArray()),
                        string.Join(",", selectedSKUs.ToArray()),
                        selectedAgeing, isSearch, firstCall, string.Join(",", SelectedDCs.ToArray()),
                        string.Join(",", selectedCategory.ToArray()),
                        string.Join(",", selectedTypes.ToArray()),
                        string.Join(",", selectedLocations.ToArray()))).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return null;
            }
        }
        public long SaveExcelFileData(OrderExcel import)
        {
            try
            {
                return _context.Database.SqlQuery<long>(string.Format("exec USP_SaveExcelFileData @Store_Order_Date='{0}', @Cust_Order_Date='{1}',@Order_Type='{2}'," +
                        " @Store_Code='{3}', @City='{4}', @State='{5}', @Last_Order_Date='{6}', @Last_Delivery_Date='{7}'," +
                        "@Order_Reference_Id='{8}', @Unique_Reference_Id='{9}', @Item_Code='{10}'," +
                        "@Item_Desc='{11}',@Item_Type='{12}',@Item_Sub_Type='{13}',@Item_Category='{14}',@Pack_Size='{15}'," +
                        "@Case_Size='{16}',@Ordered_Qty='{17}',@Number_Of_Cases='{18}',@Source_Dc='{19}',@Priority='{20}'," +
                        "@Upload_By='{21}',@CustId='{22}',@Uploaded_File_Id='{23}',@Xls_Line_No='{24}',@GroupId='{25}', @PONumber='{26}'"
                        , import.Store_Order_Date.HasValue ? import.Store_Order_Date.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "",
 import.Cust_Order_Date.HasValue ? import.Cust_Order_Date.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "",
import.Order_Type,
import.Store_Code,
import.City,
import.State,
import.Last_Order_Date,
import.Last_Delivery_Date,
import.Order_Reference_Id,
import.Unique_Reference_Id,
import.Item_Code,
import.Item_Desc,
import.Item_Type,
 import.Item_Sub_Type,
 import.Item_Category,
import.Pack_Size,
 import.Case_Size,
 import.Ordered_Qty,
import.Number_Of_Cases,
 import.Source_Dc,
import.Priority,
import.Upload_By,
import.CustId,
import.Uploaded_File_Id,
import.Xls_Line_No,
import.GroupId,
import.PONumber)
).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return 0;
            }
        }

        public long SaveUploadedFile(UploadedFilesMst mst)
        {
            try
            {
                return _context.Database.SqlQuery<long>(string.Format("EXEC	USP_SaveUploadedFile @CustId = {0},@Upload_By = {1}, @User_FileName = '{2}',@Filepath = '{3}',@System_FileName ='{4}',@FileType='{5}'", mst.CustId, mst.Upload_By, mst.User_FileName, mst.Filepath, mst.System_FileName, mst.FileType)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return 0;
            }
        }
        public long SaveUploadedFileToTemp(OrderExcel order, int CustId, int UserId)
        {
            try
            {
                return _context.Database.SqlQuery<long>(
                        string.Format("exec USP_SaveExcelFileDataToTemp @Store_Code='{0}', @Item_Code='{1}'," +
                        "@Item_Desc='{2}', @Ordered_Qty='{3}', @Processed_By={4}, @CustId={5}, @OrderXlsId={6}, @DCID={7}, @Remark='{8}'",
                        order.Store_Code, order.Item_Code,
                        order.Item_Desc, order.Ordered_Qty, UserId, CustId, order.Id, order.DCId, order.Remark)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return 0;
            }
        }
        public List<ExcelOrder> GetDataToGenerateCSV(DateTime fromDate, DateTime toDate, int custId, int userId)
        {
            try
            {
                return _context.Database.SqlQuery<ExcelOrder>(
                        string.Format("EXEC	USP_GetDataToGenerateCSV @FromDate = '{0}', @ToDate = '{1}', @UserId = {2}, @CustId = {3}, @isSearch={4}", fromDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), toDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), userId, custId, 0)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return null;
            }
        }
        public List<ExcelOrder> GetDataToGenerateCSVSearch(DateTime? fromDate, DateTime? toDate, int custId, int userId, List<int> selectedStores, List<int> selectedSKUs, int selectedAgeing, int Filtered, List<int> selectedDCs, List<int> selectedCategories, List<int> selectedTypes, List<int> selectedLocations)
        {
            try
            {
                return _context.Database.SqlQuery<ExcelOrder>(
                        string.Format("EXEC USP_GetDataToGenerateCSV @FromDate = '{0}',@ToDate = '{1}', @UserId = {2},@CustId = {3}, @Stores='{4}', @SKUs='{5}', @Ageing={6},@isSearch={7}, @DCs='{8}',@selectedCategories='{9}',@selectedTypes='{10}',@selectedLocations='{11}'",
                        fromDate.HasValue ? fromDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "",
                        toDate.HasValue ? toDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "",
                        userId, custId,
                        selectedStores != null ? string.Join(",", selectedStores.ToArray()) : "",
                        selectedSKUs != null ? string.Join(",", selectedSKUs.ToArray()) : "",
                        selectedAgeing,
                        Filtered == 1 && selectedStores != null && selectedStores.Count() > 0 && selectedStores[0] != 0 ? 1 : 0,
                        selectedDCs != null ? string.Join(",", selectedDCs.ToArray()) : "",
                        selectedCategories != null ? string.Join(",", selectedCategories.ToArray()) : "",
                        selectedTypes != null ? string.Join(",", selectedTypes.ToArray()) : "",
                        selectedLocations != null ? string.Join(",", selectedLocations.ToArray()) : ""
                        )).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return null;
            }
        }

        public long InsertOrderHeaderCSV(string StoreCode, int UserId, string orderReferenceNo, byte? OrderSource, DateTime? OrderDate, int type, int DcId)
        {
            try
            {
                return _context.Database.SqlQuery<long>(
                        string.Format("EXEC	USP_InsertOrderHeader_CSV @StoreCode = '{0}', @UserId = '{1}', @OrderSource = {2}, @OrderDate = '{3}', @DCId={4}, @Type={5}",
                        StoreCode, UserId, OrderSource, OrderDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), DcId, type)).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public long InsertOrderDetailCSV(string ItemCode, int Qty, long OrderHeaderId, string uniqueReferenceID, long ExcelId, int userId, int dCId)
        {
            try
            {
                return _context.Database.SqlQuery<long>(string.Format("exec USP_InsertOrderDetail_CSV @ItemCode='{0}', @Qty={1}, @OrderHeaderId={2}, @uniqueReferenceID='{3}', @ExcelId={4}, @UserId={5},@DCId={6}",
                        ItemCode, Qty, OrderHeaderId, uniqueReferenceID != null ? uniqueReferenceID : "", ExcelId, userId, dCId)).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<NetSuitUpload> netSuitUploadsExcel2(DateTime fromDate, DateTime toDate, int custid, int userid)
        {
            try
            {

                return _context.Database.SqlQuery<NetSuitUpload>("exec USP_UploadNetSuitExcelData_CSV @FromDate,@ToDate,@CustId,@UserId",
                  new SqlParameter("@FromDate", fromDate),
                  new SqlParameter("@ToDate", toDate),
                  new SqlParameter("@CustId", custid),
                  new SqlParameter("@UserId", userid)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<NetSuitUpload> netSuitUploadsExcel_OrderHeaderIds(DateTime fromDate, DateTime toDate, int custid, int userid, List<long> OrderHeaderIds)
        {
            try
            {
                return _context.Database.SqlQuery<NetSuitUpload>(string.Format("exec USP_UploadNetSuitExcelData_CSV2 @FromDate='{0}',@ToDate='{1}',@CustId={2},@UserId='{3}',@OrderHeaderIds='{4}'", fromDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), toDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), custid, userid, OrderHeaderIds != null ? String.Join(",", OrderHeaderIds.ToArray()) : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<UploadedFilesView> GetUploadedFileDetails(int custid, int userid, List<string> fileType)
        {
            try
            {

                return _context.Database.SqlQuery<UploadedFilesView>(
                         string.Format("EXEC USP_Get_Uploded_File_Status @custid = {0},@userid = {1}, @FileTypes='{2}'", custid, userid, fileType != null ? string.Join(",", fileType) : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<File_Error_LogsView> GetFileErrors(int custId, int userId, int fileId, string FileType)
        {
            try
            {

                return _context.Database.SqlQuery<File_Error_LogsView>(
                         string.Format("EXEC USP_Get_Error_Dtl @cust_id = {0},@user_id = {1}, @file_id={2},@FileType='{3}'", custId, userId, fileId, FileType)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }





        public void DeletedIgnoredRow(DateTime? fromDate, DateTime? toDate, int custId, int userId, List<OrderExcel> excelOrders, List<int> selectedStores, List<int> selectedSKUs, int selectedAgeing, int isSearch, List<int> SelectedDCs, List<int> selectedCategory, List<int> selectedTypes, List<int> selectedLocations)
        {
            try
            {
                string ExcelIds = string.Join(",", excelOrders.Select(x => x.Id).ToArray());
                _context.Database.SqlQuery<long>(
                        string.Format("EXEC USP_Delete_Temp_xls_Data @CustId = {0},@UserId = {1}, @FromDate='{2}',@ToDate='{3}', @Stores='{4}',@SKUs='{5}',@Ageing={6},@isSearch={7}, @Dcs='{8}',@selectedCategories='{9}',@ItemType='{10}',@Locations='{11}'",
                        custId, userId,
                        fromDate.HasValue ? fromDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "",
                        toDate.HasValue ? toDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "",
                        string.Join(",", selectedStores.ToArray()),
                        string.Join(",", selectedSKUs.ToArray()),
                        selectedAgeing, isSearch,
                        string.Join(",", SelectedDCs.ToArray()),
                        string.Join(",", selectedCategory.ToArray()),
                        string.Join(",", selectedTypes.ToArray()),
                        string.Join(",", selectedLocations.ToArray()))).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
        }
        public List<ExcelOrder> GetDataQuantity(int custId, int userId, DateTime? FromDate, DateTime? ToDate, List<string> SelectedStores, List<int> selectedDCs, List<string> SelectedSKUs, int SelectedAgeing = 0, int Filtered = 0)
        {
            try
            {
                return _context.Database.SqlQuery<ExcelOrder>(
                        string.Format("EXEC	USP_GetDataQuantity @CustId = {0},@UserId = {1}, @FromDate='{2}',@ToDate='{3}', @Stores='{4}',@SKUs='{5}',@Ageing={6},@isSearch={7},@Dcs='{8}'",
                        custId, userId,
                        FromDate.HasValue ? FromDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "",
                        ToDate.HasValue ? ToDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "",
                        SelectedStores != null ? string.Join(",", SelectedStores.ToArray()) : "",
                        SelectedSKUs != null ? string.Join(",", SelectedSKUs.ToArray()) : "",
                        SelectedAgeing,
                        Filtered != 0 && SelectedStores != null && SelectedStores.Count() > 0 && SelectedStores[0] != "" ? 1 : 0,
                        selectedDCs != null ? string.Join(",", selectedDCs.ToArray()) : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return null;
            }
        }

        public List<long> SaveGenerateCSV(DateTime? fromDate, DateTime? toDate, int custId, int userId, List<int> selectedStores, List<int> selectedSKUs, List<int> selectedDCs, int selectedAgeing, int Filtered, List<int> selectedCategories, List<int> selectedTypes, List<int> selectedLocations)
        {
            try
            {
                return _context.Database.SqlQuery<long>(
                        string.Format("EXEC USP_CreateOrderFromTempTable_2 @FromDate = '{0}',@ToDate = '{1}', @UserId = {2},@CustId = {3}, @Stores='{4}', @SKUs='{5}', @Ageing={6},@isSearch={7}, @DCs='{8}',@selectedCategories='{9}',@selectedTypes='{10}', @selectedLocations='{11}'",
                        fromDate.HasValue ? fromDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "",
                        toDate.HasValue ? toDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "",
                        userId, custId,
                        selectedStores != null ? string.Join(",", selectedStores.ToArray()) : "",
                        selectedSKUs != null ? string.Join(",", selectedSKUs.ToArray()) : "",
                        selectedAgeing,
                        Filtered != 0 && selectedStores != null && selectedStores.Count() > 0 && selectedStores[0] != 0 ? 1 : 0,
                        selectedDCs != null ? string.Join(",", selectedDCs.ToArray()) : "",
                        selectedCategories != null ? string.Join(",", selectedCategories.ToArray()) : "",
                        selectedTypes != null ? string.Join(",", selectedTypes.ToArray()) : "",
                        selectedLocations != null ? string.Join(",", selectedLocations.ToArray()) : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return null;
            }
        }

        public void disableOrderForNetsuiteUpload(List<string> POList)
        {
            try
            {
                _context.Database.SqlQuery<long>(
                       string.Format("EXEC USP_disableOrderForUpload @POs='{0}'",
                       POList != null ? string.Join(",", POList.ToArray()) : "")).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                //return 0;
            }
        }

        public int UpdateStatus(InvoiceDispatch invoiceDispatch)
        {
            try
            {
                return _context.Database.SqlQuery<int>("exec USP_UpdateStatus @InvoiceNo,@DeliveryStatus,@DeliveryDate",
                  new SqlParameter("@InvoiceNo", invoiceDispatch.InvoiceNo),
                  new SqlParameter("@DeliveryStatus", invoiceDispatch.DeliveryStatus),
                  new SqlParameter("@DeliveryDate", invoiceDispatch.DeliveryDate)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public long AddUpdatePODH(PODHeader pODHeader)
        {
            try
            {
                long i = _context.Database.SqlQuery<int>("exec USP_AddUpdatePODHeader @Action,@InvoiceDispatchId,@Invoice_Number,@DeliveryStatus,@DeliveryDate,@OrderHeaderId,@Reason,@CreatedBy",
                  new SqlParameter("@Action", "Add"),
                  new SqlParameter("@InvoiceDispatchId", pODHeader.InvoiceDispatchId),
                  new SqlParameter("@Invoice_Number", pODHeader.Invoice_Number),
                  new SqlParameter("@DeliveryStatus", pODHeader.DeliveryStatus),
                  new SqlParameter("@DeliveryDate", pODHeader.DeliveryDate),
                  new SqlParameter("@OrderHeaderId", ""),
                  new SqlParameter("@Reason", pODHeader.Reason),
                  new SqlParameter("@CreatedBy", pODHeader.CreatedBy)
                  ).ToList().FirstOrDefault();
                return i;
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }


        #region POD
        public List<PODListData> GetInvoicesforVehicle(string vehicleNo, int custid)
        {
            try
            {
                return _context.Database.SqlQuery<PODListData>("exec USP_GetInvoiceNoList @vehicleNo,@custid",
                new SqlParameter("@vehicleNo", vehicleNo),
               new SqlParameter("@custid", custid)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<storeBO> GetStoreName(string invoiceVal)
        {
            try
            {
                return _context.Database.SqlQuery<storeBO>("exec USP_GetInvoiceStoreName @invoiceVal",
              new SqlParameter("@invoiceVal", invoiceVal)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public long AddUpdatePODH(PODheaderBO pODheaderBO)
        {
            try
            {
                long i = _context.Database.SqlQuery<int>("exec USP_AddUpdatePODHeader @Action,@InvoiceDispatchId,@Invoice_Number,@DeliveryStatus,@DeliveryDate,@Reason,@CreatedBy,@Reattempte",
                  new SqlParameter("@Action", "Add"),
                  new SqlParameter("@InvoiceDispatchId", pODheaderBO.InvoiceDispatchId),
                  new SqlParameter("@Invoice_Number", pODheaderBO.Invoice_Number),
                  new SqlParameter("@DeliveryStatus", pODheaderBO.DeliveryStatus),
                  new SqlParameter("@DeliveryDate", DateTime.ParseExact(pODheaderBO.DeliveryDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)),
                  new SqlParameter("@Reason", pODheaderBO.Reason),
                  new SqlParameter("@CreatedBy", pODheaderBO.CreatedBy),
                  new SqlParameter("@Reattempte", pODheaderBO.Reattempte)
                  ).ToList().FirstOrDefault();
                return i;
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<CategoryWiseReasonMaster> GetReasonlist(string categoryid)
        {
            try
            {
                return _context.Database.SqlQuery<CategoryWiseReasonMaster>("exec USP_CategoryWiseReason @categoryid",
              new SqlParameter("@categoryid", categoryid)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public long savePODDetailResponse(PODDetail pODDetail, int InvoiceDispatchId)
        {
            try
            {
                return _context.Database.SqlQuery<long>("exec USP_AddPODDetails @PODHeaderId,@Item,@InvoiceQuantity,@DeliveredQuantity,@ItemId,@Reason,@Status,@InvoiceDespatchId",
                     new SqlParameter("@PODHeaderId", pODDetail.PODHeaderId),
                     new SqlParameter("@Item", pODDetail.Item != null ? pODDetail.Item : (object)DBNull.Value),
                     new SqlParameter("@InvoiceQuantity", pODDetail.InvoiceQuantity),
                     new SqlParameter("@DeliveredQuantity", pODDetail.DeliveredQuantity),
                     new SqlParameter("@ItemId", pODDetail.ItemId.HasValue ? pODDetail.ItemId : (object)DBNull.Value),
                     new SqlParameter("@Status", pODDetail.Status),
                     new SqlParameter("@Reason", pODDetail.Reason),
                     new SqlParameter("@InvoiceDespatchId", InvoiceDispatchId)
                  ).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        #endregion
        #region  StoreOrderToExcelOrder Nikhil Kadam
        public OrderSaveResponse SaveStoreOrderToOrderExcel(OrderExcel orderExcel, string issave)
        {
            try
            {
                return _context.Database.SqlQuery<OrderSaveResponse>(string.Format("exec USP_SaveStoreOrderToOrderExcel  @CustId={0}, @UserId={1}, @Method='{2}',@IsSave='{3}', @Store_Order_Date='{4}', @Store_Code='{5}',@Item_Code='{6}', @ItemId={7},@Item_Desc='{8}', @Ordered_Qty={9}",
                        orderExcel.CustId, orderExcel.Upload_By, "ADD", issave, orderExcel.Store_Order_Date.Value.ToString("yyyy-MM-dd HH:mm:ss"), orderExcel.Store_Code, orderExcel.Item_Code, orderExcel.ItemId, orderExcel.Item_Desc, orderExcel.Ordered_Qty)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return new OrderSaveResponse() { Id = 0, Message = ex.Message, Status = 2, Type = issave };
            }
        }
        public List<StoreOrderVB> GetStoreDraftOrder(int storeID, int custId, int UserId)
        {
            try
            {
                return _context.Database.SqlQuery<StoreOrderVB>(string.Format("exec USP_StoreDraftOrder @CustId={0}, @UserId={1}, @StoreId='{2}'", custId, UserId, storeID)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return null;
            }
        }
        public long UpdateDraftQuantity(int storeId, int itemId, int quantity, string method, int userid)
        {
            try
            {
                return _context.Database.SqlQuery<long>(string.Format("exec USP_UpdateStoreDraft @StoreId={0}, @ItemId={1}, @Quantity={2},@Method='{3}',@UserId ={4}", storeId, itemId, quantity, method, userid)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return 0;
            }
        }
        public OrderSaveResponse FinilizeDraft(OrderExcel orderExcel, string issave)
        {
            try
            {
                //long Groupid = GetMaxGroupIDByCustomer(orderExcel.CustId);
                return _context.Database.SqlQuery<OrderSaveResponse>(string.Format("exec USP_FinilizeDraft @Store_Code='{0}',@UserId ={1},@Item_Code='{2}',@MaxGroupId ={3},@PONumber='{4}' ", orderExcel.Store_Code, orderExcel.Upload_By, orderExcel.Item_Code, orderExcel.GroupId, orderExcel.PONumber)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return new OrderSaveResponse() { Id = 0, Message = ex.Message, Status = 2, Type = issave };
            }
        }

        public long GetMaxGroupIDByCustomer(int CustId)
        {
            try
            {
                return _context.Database.SqlQuery<long>(string.Format("exec USP_GetMaxGroupIdByCustomer @CustId={0}", CustId)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return 0;
            }
        }

        public long DeleteStoreDraft(int userId, int storeId)
        {
            try
            {
                return _context.Database.SqlQuery<long>(string.Format("exec USP_UpdateStoreDraft @StoreId={0}, @ItemId={1}, @Quantity={2},@Method='{3}',@UserId ={4}", storeId, 1, 1, "DeleteDraft", userId)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return 0;
            }
        }
        #endregion
        #region Tracking
        public List<TrackingBO> GetTrackingList(string action, string uniqueReferenceID, string InvoiceNo, int custid)
        {
            try
            {
                return _context.Database.SqlQuery<TrackingBO>("exec USP_GetDataUniqueRefID @Action,@uniqueReferenceID,@InvoiceNo,@Custid",
                  new SqlParameter("@Action", action),
                  new SqlParameter("@uniqueReferenceID", uniqueReferenceID),
                  new SqlParameter("@InvoiceNo", InvoiceNo),
                  new SqlParameter("@Custid", custid)
              ).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<TrackingBO> GetStatusList(string uniqueReferenceID, string InvoiceNo, int custid)
        {
            try
            {
                return _context.Database.SqlQuery<TrackingBO>("exec USP_GetStatusList @uniqueReferenceID,@InvoiceNo,@Custid",
                  new SqlParameter("@uniqueReferenceID", uniqueReferenceID),
                  new SqlParameter("@InvoiceNo", InvoiceNo),
                  new SqlParameter("@Custid", custid)
                    ).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        #endregion

        public List<StoreOrders> GetLastStoreOrders(string storeCode, int StoreId, int CustId, DateTime today)
        {
            try
            {
                return _context.Database.SqlQuery<StoreOrders>(string.Format("EXEC USP_Store_last_5_orders @StoreDate='{0}',@StoreCode='{1}',@ReportType={2},@StoreId={3},@CustId={4}", today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), storeCode, 1, StoreId, CustId)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return null;
            }
        }
        public List<StoreOrderDetailsVB> GetLastStoreOrdersByDateStoreCode(DateTime storeOrderDate, string storeCode, int StoreId, int CustId, long? GroupId, string PoNumber)
        {
            try
            {
                return _context.Database.SqlQuery<StoreOrderDetailsVB>(string.Format("EXEC USP_Store_last_5_orders @StoreDate='{0}',@StoreCode='{1}',@ReportType={2},@StoreId={3},@CustId={4},@GroupId='{5}', @PONumber='{6}'", storeOrderDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), storeCode, 2, StoreId, CustId, GroupId.HasValue ? GroupId.ToString() : "", PoNumber != null ? PoNumber : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return null;
            }
        }

        #region "Order Delete Close By Deepa 28/11/2019"
        public List<OrderDeleteClose> OrderDeleteCloseSearch(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, string Action)
        {
            try
            {
                string Str = string.Format("exec USP_OrderDeleteCloseSearch @StoreOrderFrom='{0}', @StoreOrderToDate='{1}',@StoreCode='{2}', @DCCode='{3}', @SKUCode='{4}', @UniqueRefNo='{5}', @Action='{6}'", startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), stores != null ? string.Join(",", stores.ToArray()) : "", dCs != null ? string.Join(",", dCs.ToArray()) : "", items != null ? string.Join(",", items.ToArray()) : "", uniqueReferenceID != null ? uniqueReferenceID : "", Action);
                return _context.Database.SqlQuery<OrderDeleteClose>(Str).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public string OrderDeleteClose(List<OrderDeleteItems> Ids, string Action, int userid)
        {
            try
            {
                foreach (OrderDeleteItems item in Ids)
                {
                    string result = _context.Database.SqlQuery<string>(string.Format("exec USP_OrderDeleteClose @Ids={0}, @Action='{1}',@Reason='{2}',@CustomReason='{3}',@UserId={4}",
                       item.ProductId,
                       Action,
                      item.Reason,
                      item.CustomReason,
                      userid)).ToList().FirstOrDefault();
                }
                return "Result";
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        #endregion

        #region SaveCVSFile By nikhil 2019/12/06
        public long SaveCSVFileDetails(string filename, string path, int custId, int userId, DateTime noW)
        {
            try
            {
                string Str = string.Format("exec USP_SaveCSVFileDetails @filename='{0}', @filePath='{1}',@CustID='{2}', @UserID='{3}', @CreatedDate='{4}'", filename, path, custId, userId, noW.ToString("yyyy-MM-dd HH:mm:ss"));
                return _context.Database.SqlQuery<long>(Str).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return 0;
            }
        }

        public List<CSVFileMasterVM> GetCSVFileDetails(int custId, int userId, DateTime date, DateTime toDate)
        {
            try
            {
                string Str = string.Format("exec USP_GetCSVFileDetails @CustID={0}, @UserID={1}, @CreatedFromDate='{2}',@CreatedToDate='{3}'", custId, userId, date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), toDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                return _context.Database.SqlQuery<CSVFileMasterVM>(Str).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return null;
            }
        }
        #endregion

        #region SavePODImage by Rekha 2019/12/24 updateddate 2020/01/03
        public long SavePODImg(PODImages pODImages)
        {
            try
            {
                return _context.Database.SqlQuery<int>("exec USP_AddPODImages @PODHeaderId,@FileName,@Type,@InvoiceNo",
                     new SqlParameter("@PODHeaderId", Convert.ToInt64(pODImages.PODHeaderId)),
                     new SqlParameter("@FileName", pODImages.FileName),
                     new SqlParameter("@Type", pODImages.Type),
                     new SqlParameter("@InvoiceNo", pODImages.InvoiceNo)
                  ).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        #endregion

        #region API 28/01/2020
        public OrderInboundResponse AddOrderInbound(OrderInbound order)
        {
            //try
            //{
            //    return _context.Database.SqlQuery<OrderSaveResponse>(string.Format("exec USP_AddOrderInbound @UserId={1}, @Store_Order_Date='{1}', @Store_Code='{2}',@Item_Code='{3}',@Item_Desc='{4}', @Ordered_Qty={5}",
            //            orderExcel.CustId, orderExcel.Upload_By, "ADD", issave, orderExcel.Store_Order_Date.Value.ToString("yyyy-MM-dd HH:mm:ss"), orderExcel.Store_Code, orderExcel.Item_Code, orderExcel.ItemId, orderExcel.Item_Desc, orderExcel.Ordered_Qty)).FirstOrDefault();
            //}
            //catch (Exception ex)
            //{
            //    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
            return new OrderInboundResponse() { ClientPONo = order.ClientPONo, Date = DateTime.Now, Status = "Order Not Received" };
            //}
        }
        #endregion

        #region "POD Bulk Update" 26/02/2020 deepa
        public List<PODBulkUpdate> GetInvoiceForBulkUpdateByVehicleNo(string vehicleNo, int custid)
        {
            try
            {
                return _context.Database.SqlQuery<PODBulkUpdate>("exec USP_GetInvoiceForBulkUpdate @vehicleNo,@custid",
                new SqlParameter("@vehicleNo", vehicleNo),
               new SqlParameter("@custid", custid)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        #endregion

        public long UploadExcelFileData(OrderExcelMapping import)
        {
            try
            {
                return _context.Database.SqlQuery<long>(@"exec USP_UploadExcelFileData_Outward @Unique_Order_No, @Order_Date, @Store_Code, " +
                        "@City, @Source_Dc, @Item_Code, @Ordered_Qty, " +
                        "@Sales_Order_No, @SO_Status, @Sales_Order_Date, @Sales_Order_Qty," +
                        " @Invoice_No, @Invoice_Date, @Invoice_Qty," +
                        " @Dispatch_Date, @Vehicle_No, @Delivery_Status, @Delivery_ReAttempt, @POD_Date, @Undelivered_Reason, " +
                        "@Delivered_Qty, @Is_OnTimeDelivery, @Upload_By, @CustId, @Uploaded_File_Id, @Xls_Line_No, @GroupId, @ISR_NO, @Actual_Reporting_Time",
                        new SqlParameter("@Unique_Order_No", import.Unique_Order_No),
                        new SqlParameter("@Order_Date", import.Order_Date.HasValue ? import.Order_Date.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : ""),
                        new SqlParameter("@Store_Code", import.Store_Code),
                        new SqlParameter("@City", import.City),
                        new SqlParameter("@Source_Dc", import.Source_Dc),
                        new SqlParameter("@Item_Code", import.Item_Code),
                        new SqlParameter("@Ordered_Qty", import.Ordered_Qty),
                        new SqlParameter("@Sales_Order_No", import.Sales_Order_No),
                        new SqlParameter("@SO_Status", import.SO_Status),
                        new SqlParameter("@Sales_Order_Date", import.Sales_Order_Date.HasValue ? import.Sales_Order_Date.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : ""),
                        new SqlParameter("@Sales_Order_Qty", import.Sales_Order_Qty),
                        new SqlParameter("@Invoice_No", import.Invoice_No),
                        new SqlParameter("@Invoice_Date", import.Invoice_Date.HasValue ? import.Invoice_Date.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : ""),
                        new SqlParameter("@Invoice_Qty", import.Invoice_Qty),
                        new SqlParameter("@Dispatch_Date", import.Dispatch_Date.HasValue ? import.Dispatch_Date.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : ""),
                        new SqlParameter("@Vehicle_No", import.Vehicle_No),
                        new SqlParameter("@Delivery_Status", import.Delivery_Status),
                        new SqlParameter("@Delivery_ReAttempt", import.Delivery_ReAttempt),
                        new SqlParameter("@POD_Date", import.POD_Date.HasValue ? import.POD_Date.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : ""),
                        new SqlParameter("@Undelivered_Reason", import.Undelivered_Reason),
                        new SqlParameter("@Delivered_Qty", import.Delivered_Qty),
                        new SqlParameter("@Is_OnTimeDelivery", import.Is_OnTimeDelivery),
                        new SqlParameter("@Upload_By", import.Upload_By),
                        new SqlParameter("@CustId", import.CustId),
                        new SqlParameter("@Uploaded_File_Id", import.Uploaded_File_Id),
                        new SqlParameter("@Xls_Line_No", import.Xls_Line_No),
                        new SqlParameter("@GroupId", import.GroupId),
                        new SqlParameter("@ISR_NO", import.ISR_NO != null ? import.ISR_NO : ""),
                        new SqlParameter("@Actual_Reporting_Time", import.Actual_Reporting_Time != null ? import.Actual_Reporting_Time : "")
                        ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return 0;
            }
        }

        public long UploadInwardFileData(OrderExcelInward import)
        {
            try
            {
                return _context.Database.SqlQuery<long>(@"exec USP_UploadInwardFileData @PO_Number, @PO_Date, @Vendor_Name, " +
                        "@Status, @GRN_Date, @GRN_No, @SupplierDoc_Date, " +
                        "@SupplierDoc_Ref, @Vehicle_No, @Remark, @Class, " +
                        " @Location, @SKU_Code, @SKU_Description, @SKU_Category, " +
                        " @PO_Qty, @GRN_Qty, @VRA_Qty," +
                        "@Upload_By, @CustId, @Uploaded_File_Id, @Xls_Line_No",
                        new SqlParameter("@PO_Number", import.PO_Number),
                        new SqlParameter("@PO_Date", import.PO_Date.HasValue ? import.PO_Date.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : ""),
                        new SqlParameter("@Vendor_Name", import.Vendor_Name),
                        new SqlParameter("@Status", import.Status),
                        new SqlParameter("@GRN_Date", import.GRN_Date.HasValue ? import.GRN_Date.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : ""),
                        new SqlParameter("@GRN_No", import.GRN_No),
                        new SqlParameter("@SupplierDoc_Date", import.SupplierDoc_Date.HasValue ? import.SupplierDoc_Date.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : ""),
                        new SqlParameter("@SupplierDoc_Ref", import.SupplierDoc_Ref),
                        new SqlParameter("@Vehicle_No", import.Vehicle_No),
                        new SqlParameter("@Remark", import.Remark),
                        new SqlParameter("@Class", import.Class),
                        new SqlParameter("@Location", import.Location),
                        new SqlParameter("@SKU_Code", import.SKU_Code),
                        new SqlParameter("@SKU_Description", import.SKU_Description),
                        new SqlParameter("@SKU_Category", import.SKU_Category),
                        new SqlParameter("@PO_Qty", import.PO_Qty),
                        new SqlParameter("@GRN_Qty", import.GRN_Qty),
                        new SqlParameter("@VRA_Qty", import.VRA_Qty),
                        new SqlParameter("@Upload_By", import.Upload_By),
                        new SqlParameter("@CustId", import.CustId),
                        new SqlParameter("@Uploaded_File_Id", import.Uploaded_File_Id),
                        new SqlParameter("@Xls_Line_No", import.Xls_Line_No)
                        ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return 0;
            }
        }

        public long ProcessCompleteExcelUploadOutward(string uniqueOrderNo, long fileId, int loggedInCustomer)
        {
            try
            {
                return _context.Database.ExecuteSqlCommand(string.Format("exec USP_Process_CompleteExcelUpload_outward @custid ={0}, @unique_order_no='{1}',@Uploaded_File_Id={2}",
                       loggedInCustomer, uniqueOrderNo, fileId));
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return 0;
            }
        }

        public long UploadOrderInventoryFileData(OrderInventory import)
        {
            try
            {
                return _context.Database.SqlQuery<long>(@"exec USP_UploadOrderInventoryFileData @SKU_Code, @SKU_Description, @SKU_Category, " +
                        "@DC_Code, @UOM, @On_Hand, @QC, " +
                        "@Upload_By, @CustId, @Uploaded_File_Id, @Xls_Line_No",
                        new SqlParameter("@SKU_Code", import.SKU_Code),
                        new SqlParameter("@SKU_Description", import.SKU_Description),
                        new SqlParameter("@SKU_Category", import.SKU_Category),
                        new SqlParameter("@DC_Code", import.DC_Code),
                        new SqlParameter("@UOM", import.UOM),
                        new SqlParameter("@On_Hand", import.On_Hand),
                        new SqlParameter("@QC", import.QC),
                        new SqlParameter("@Upload_By", import.Upload_By),
                        new SqlParameter("@CustId", import.CustId),
                        new SqlParameter("@Uploaded_File_Id", import.Uploaded_File_Id),
                        new SqlParameter("@Xls_Line_No", import.Xls_Line_No)
                        ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return 0;
            }
        }

        #region NetSuiteInventory
        public List<NetSuiteInventory> GetNetSuiteInventory(int custId, List<int> DCIDs, List<int> SKUIds, int? userId)
        {
            try
            {
                return _context.Database.SqlQuery<NetSuiteInventory>(
                        string.Format("EXEC	USP_GetCustomerWiseInventory @CustId = {0},@DCIDs = '{1}',@SkUIDs = '{2}',@UserId = {3} ",
                         custId,
                         DCIDs != null ? string.Join(",", DCIDs.ToArray()) : "",
                         SKUIds != null ? string.Join(",", SKUIds.ToArray()) : "",
                         userId == null ? 0 : userId)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return null;
            }

        }

        public void InsertNetSuiteInventoryBulkUpload(List<NetSuiteInventoryAPIResponse> alldata, int UserId)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[7]
            {
                new DataColumn("SKU_Code", typeof(string)),
                new DataColumn("DC_Code", typeof(string)),
                new DataColumn("On_HandQty",typeof(decimal)),
                new DataColumn("AvailableQty",typeof(decimal)),
                new DataColumn("CommitedQty",typeof(decimal)),
                new DataColumn("userId",typeof(int)),
                new DataColumn("asOnDate",typeof(DateTime))
            });
            List<NetSuiteInventoryAPIResponse> Lstinv = alldata.Where(a => a.dcLocation.Contains("OTRD")).ToList();

            foreach (NetSuiteInventoryAPIResponse data in Lstinv)
            {
                dt.Rows.Add(
                    data.SKUCode,
                    data.dcLocation,
                    data.onhandQty,
                    data.availableQty,
                    data.commitedQty,
                    UserId,
                    data.asOnDate == null ? (DateTime?)null : DateTime.ParseExact(data.asOnDate.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture)
                    );
            }
            if (dt.Rows.Count > 0)
            {
                string consString = _context.Database.Connection.ConnectionString;
                using (SqlConnection con = new SqlConnection(consString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_BulkInsert_NetSuiteInventoryData"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@NetSuiteInventoryAPIResponse", dt);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
        }

        public List<NetSuiteInventory> GetCustomerWiseDCWiseInventory(int custId, List<int> DCIDs, List<int> SKUIds, int? userId)
        {
            try
            {
                return _context.Database.SqlQuery<NetSuiteInventory>(
                        string.Format("EXEC	USP_GetCustomerWiseDCWiseInventory @CustId = {0},@DCIDs = '{1}',@SkUIDs = '{2}',@UserId = {3} ",
                         custId,
                         DCIDs != null ? string.Join(",", DCIDs.ToArray()) : "",
                         SKUIds != null ? string.Join(",", SKUIds.ToArray()) : "",
                         userId == null ? 0 : userId)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return null;
            }
        }
        #endregion

        #region GBPA 03 Aug 2021
        public List<GBPA> GetGBPLData(int custId, int userId, DateTime date)
        {
            try
            {
                return _context.Database.SqlQuery<GBPA>(
                         string.Format("EXEC USP_GetGBPLData @CustId = {0},@Date='{1}',@Type= '{2}',@UserId = {3} ",
                          custId,
                          date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                          "LATEST",
                          userId)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return null;
            }
        }
        #endregion

        public List<TopItemCustomers> GetTopItemsForCustomer(int userid, int custid, int SelectedTopItems, List<int> selectedItemType, List<int> selectedItemcategories)
        {
            try
            {
                return _context.Database.SqlQuery<TopItemCustomers>(
                    string.Format("exec USP_GetWareHouseStoreForCustomer @UserId={0}, @CustId={1}, @CallType={2}, @ItemTypes='{3}',@Categories='{4}', @SelectedTopItems='{5}'",
                    userid, custid, 13,
                    string.Join(",", selectedItemType.ToArray()),
                    string.Join(",", selectedItemcategories.ToArray()),
                    SelectedTopItems)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw null;
            }
        }
        public int UpdateTopItemList(List<TopItemCustomers> items, int id, List<int> selectedItemType, List<int> selectedItemCategories, int userid, int custid)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[4]
                {
                new DataColumn("ItemId", typeof(int)),
                new DataColumn("CustId", typeof(int)),
                new DataColumn("IsDeleted",typeof(int)),
                new DataColumn("UpdatedBy",typeof(int))
                });

                foreach (TopItemCustomers data in items)
                {
                    dt.Rows.Add(
                        data.Item_Id,
                       custid,
                        data.Checked ? 0 : 1,
                        userid
                        );
                }
                if (dt.Rows.Count > 0)
                {
                    string consString = _context.Database.Connection.ConnectionString;
                    using (SqlConnection con = new SqlConnection(consString))
                    {
                        using (SqlCommand cmd = new SqlCommand("USP_AddUpdateBulkTopItemCustomers"))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = con;
                            cmd.Parameters.AddWithValue("@TopItemCustomersResponse", dt);
                            cmd.Parameters.AddWithValue("@Id", id);
                            cmd.Parameters.AddWithValue("@OPR", "ADD");
                            cmd.Parameters.AddWithValue("@selectedItemType", string.Join(",", selectedItemType.ToArray()));
                            cmd.Parameters.AddWithValue("@selectedItemCategories", string.Join(",", selectedItemCategories.ToArray()));
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return 0;
            }
            finally
            {
            }
            return 1;
        }

        public List<NetSuitUpload> NetSuitUploadsExcel_OrderHeaderIds(int custId, int userId, List<long> orderHeaderIds, List<ExcelOrder> excelOrders, string orderExcel)
        {
            return _context.Database.SqlQuery<NetSuitUpload>(
                string.Format("exec USP_UploadNetSuitExcelData_CSV3 @CustId={0},@UserId='{1}',@OrderHeaderIds='{2}', @ExcelIds='{3}', @Type='{4}'",
                custId, userId,
                orderHeaderIds != null ? String.Join(",", orderHeaderIds.ToArray()) : "",
                excelOrders != null ? String.Join(",", excelOrders.Select(x => x.Id).ToArray()) : "",
                orderExcel
                )).ToList();
        }

        public long RemoveFromViewCSVList(List<int> excelOrderIds, int custId, int userId)
        {
            return _context.Database.SqlQuery<long>(
                string.Format("exec USP_RemoveFromViewCSVList @CustId={0},@UserId='{1}',@OrderExcelIds='{2}'",
                custId, userId,
                excelOrderIds != null ? String.Join(",", excelOrderIds.ToArray()) : "")).FirstOrDefault();
        }
        public string GetPoNumber(int storeId, int? loggedInCustId, int loggedinuserid)
        {
            return _context.Database.SqlQuery<string>(
               string.Format("exec USP_getPoNumber @UserId={0},@StoreId={1}, @CustId={2}", loggedinuserid, storeId, loggedInCustId)).FirstOrDefault();
        }
        public string ImportData(DataTable ds, string sp_name, string sysfilename, int userId)
        {
            List<SqlParameter> c = new List<SqlParameter>();
            c.Add(new SqlParameter("@DATA", ds));
            c.Add(new SqlParameter("@FILENAME", sysfilename));
            return this.SqlCommand(sp_name, c);
        }
        public string SqlCommand(string query, List<SqlParameter> collection)
        {

            string consString = System.Configuration.ConfigurationManager.ConnectionStrings["StoreConnectionString"].ConnectionString;// _context.Database.Connection.ConnectionString;
            using (SqlConnection con = new SqlConnection(consString))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach(SqlParameter sqlParameter in collection)
                    {
                        cmd.Parameters.Add(sqlParameter);
                    }
                    cmd.Connection = con;
                    con.Open();
                    string sc = (string)cmd.ExecuteScalar();
                    con.Close();
                    return sc;
                }
            }
        }
        public List<OrderEdit> OrderEditSearch(DateTime? FromDate, DateTime? ToDate, string OrderId)
        {
            try
            {
                string Str = string.Format("exec USP_OrderEditSearch");

// string Str = string.Format("exec USP_OrderEditSearch @StoreOrderFrom='{0}', @StoreOrderToDate='{1}',@StoreCode='{2}', @DCCode='{3}', @SKUCode='{4}', @UniqueRefNo='{5}', @Action='{6}'", 
                    //startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), 
                    //endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    //stores != null ? string.Join(",", stores.ToArray()) : "", 
                    //dCs != null ? string.Join(",", dCs.ToArray()) : "", 
                    //items != null ? string.Join(",", items.ToArray()) : "", 
                    //uniqueReferenceID != null ? uniqueReferenceID : "", Action);
                return _context.Database.SqlQuery<OrderEdit>(Str).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public OrderEdit OrderEditdetails(int id)
        {
            try
            {
                string query = $"exec USP_orderEditDetails @Id = {id}";
                return _context.Database.SqlQuery<OrderEdit>(query).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw;
            }
        }
        public string OrderEdit(List<OrderEdit> id)
        {
            try
            {
                foreach (OrderEdit item in id)
                {
                    string result = _context.Database.SqlQuery<string>(string.Format("exec USP_UpdateOrderDetails @Ids={0}",
                      item.Id
                     // Action,
                     //item.custid,
                     //item.ItemName
                     )).ToList().FirstOrDefault();
                }
                return "Result";
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }


        }
        public string UpdateOrders(int Id, string OrderQuantity)
        {
            try
            {
                var result = _context.Database.SqlQuery<string>(
                    "EXEC USP_UpdateOrderDetails @Id, @OrderQuantity",
                    new SqlParameter("@Id", Id),
                    new SqlParameter("@OrderQuantity", OrderQuantity)
                ).FirstOrDefault();

                if (result =="Success")
                {
                    return "Orders updated successfully";
                }

                return "Update failed for Order ID: " + Id; 
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return "An error occurred while updating the orders.";
            }
        }
        public List<ExcelOrder> GenerateOrder(DateTime? fromDate, DateTime? toDate, int custId, int userId, List<string> selectedStores, List<string> selectedSKUs, List<int> SelectedDCs, int? selectedAgeing, int? isSearch, int? firstCall, List<int> selectedCategory, List<int> selectedTypes, List<int> selectedLocations,string Unique_Reference_Id)
        {
            try
            {
                // Prepare SQL command with parameters
                var sql = "EXEC USP_GetUploded_Store_Items " +
                          "@FromDate, @ToDate, @UserId, @CustId, " +
                          "@Stores, @SKUs, @Ageing, @isSearch, @FirstCall, " +
                          "@DCs, @ItemCategories, @ItemTypes, @Locations";

                var parameters = new List<SqlParameter>
        {
            new SqlParameter("@FromDate", SqlDbType.Date) { Value = (object)fromDate ?? DBNull.Value },
            new SqlParameter("@ToDate", SqlDbType.Date) { Value = (object)toDate ?? DBNull.Value },
            new SqlParameter("@UserId", SqlDbType.Int) { Value = userId },
            new SqlParameter("@CustId", SqlDbType.Int) { Value = custId },
            new SqlParameter("@Stores", SqlDbType.NVarChar) { Value = string.Join(",", selectedStores) },
            new SqlParameter("@SKUs", SqlDbType.NVarChar) { Value = string.Join(",", selectedSKUs) },
            new SqlParameter("@Ageing", SqlDbType.Int) { Value = (object)selectedAgeing ?? DBNull.Value },
            new SqlParameter("@isSearch", SqlDbType.Int) { Value = (object)isSearch ?? DBNull.Value },
            new SqlParameter("@FirstCall", SqlDbType.Int) { Value = (object)firstCall ?? DBNull.Value },
            new SqlParameter("@DCs", SqlDbType.NVarChar) { Value = string.Join(",", SelectedDCs) },
            new SqlParameter("@ItemCategories", SqlDbType.NVarChar) { Value = string.Join(",", selectedCategory) },
            new SqlParameter("@ItemTypes", SqlDbType.NVarChar) { Value = string.Join(",", selectedTypes) },
            new SqlParameter("@Locations", SqlDbType.NVarChar) { Value = string.Join(",", selectedLocations) }
        };

                return _context.Database.SqlQuery<ExcelOrder>(sql, parameters.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw;
            }
        }
    }
}
