using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DbLayer.Repository;

namespace DbLayer.Service
{
    public interface IOrderService
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
        List<OrderInboundResponse> AddOrderInbound(List<OrderInbound> order);
        int UpdateQtyInTemp(TempTbl tempTbl);
        List<OrderHeader> MakeaCopy(int storeid);
        List<OrderHeader> GetPlannedOrderNotProcessed(int storeID, int statusFlag);
        List<DetailReport> SoApprove(int storeid);
        List<OrderHeader> FullfillmentCount(int storeid);
        List<OrderHeader> InvoiceCount(int storeid);
        int DeleteOrder(long orderid, int userid);
        string ItemForCustomer(string StoreCode, string SkuCode, int loggedInCustomer, int loggedInUser, string uniqueReferenceID);
        long InsertOrderHeader(string StoreCode, int UserId, string orderReferenceNo, byte? OrderSource, DateTime? OrderDate);
        long InsertOrderDetail(string ItemCode, int Qty, long OrderHeaderId, string uniqueReferenceID);
        int DeleteFinalisedOrder(long Id, string reason, int userid);
        List<string> GetDistinctLocationsFromInvoice(int userid, int custid);
        int UpdateOrder(long Orderheaderid, int Itemid, int Quantity, int userid);
        List<InvoiceHeader> GetInvoicesforDateDC(DateTime invoiceDate, string selectedDCs);
        List<OFRLIFR> MakeCopyWithLFR(int storeid);
        List<CurrentItemInventoryMst> GetCurrentItemInventoryMstForCustomer(int custid, int userid, List<int> storeIds);
        List<OrderHeader> GetDraftOrdersforAdminUser(int userid, int custid, List<int> allowedStores);
        List<OrderDetail> GetDraftOrderDetails(long OrderId, int userid, int custid, List<int> storeIds);
        List<CurrentItemInventoryMst> GetCurrentItemInventoryMstForStoreandItemandCategory(int custid, int userid, List<int> storeIds, List<int> categories, List<int> items);
        List<SoApproveHeader> GetSoForOrder(string finalizedorderno);
        List<InvoiceHeader> GetInvoicesforSos(List<string> sonumbers);
        List<FullfillmentHeader> GetFullfillmentsforSos(List<string> sonumbers);
        InvoiceHeader GetInvoiceInformation(string invoiceNo);
        long savePODResponse(PODHeader PODHeader);
        long savePODDetailResponse(PODDetail pODDetail, int InvoiceDispatchId);
        List<PODDetail> GetPODDetails(long pk_PODHeader);
        void DeletePODHeader(long pk_PODHeader, int userid);
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
        long SaveUploadedFileToTemp(OrderExcel order, int custId, int UserId);
        List<ExcelOrder> GetDataToGenerateCSVSearch(DateTime? fromDate, DateTime? toDate, int custId, int userId, List<int> selectedStores, List<int> selectedSKUs, int SelectedAgeing, int Filtered, List<int> selectedDCs, List<int> selectedCategories, List<int> selectedTypes, List<int> selectedLocations);
        List<ExcelOrder> GetDataToGenerateCSV(DateTime fromDate, DateTime toDate, int custId, int userId);
        long InsertOrderHeaderCSV(string StoreCode, int UserId, string orderReferenceNo, byte? OrderSource, DateTime? OrderDate, int type, int DcId);
        long InsertOrderDetailCSV(string ItemCode, int Qty, long OrderHeaderId, string uniqueReferenceID, long ExcelId, int userId, int dCId);
        List<NetSuitUpload> netSuitUploadsExcel2(DateTime fromDate1, DateTime toDate1, int custId, int userId);
        List<NetSuitUpload> netSuitUploadsExcel_OrderHeaderIds(DateTime fromDate1, DateTime toDate1, int custId, int userId, List<long> OrderHeaderId);
        List<UploadedFilesView> GetUploadedFileDetails(int custId, int userId, List<string> fileType);
        List<File_Error_LogsView> GetFileErrors(int custId, int userId, int fileId, string FileType);
        void DeletedIgnoredRow(DateTime? fromDate, DateTime? toDate, int custId, int userId, List<OrderExcel> excelOrders, List<int> selectedStores, List<int> SelectedSKUs, int SelectedAgeing, int isSearch, List<int> SelectedDCs, List<int> selectedCategory, List<int> selectedTypes, List<int> selectedLocations);
        List<ExcelOrder> GetDataQuantity(int custId, int userId, DateTime? FromDate, DateTime? ToDate, List<string> SelectedStores, List<string> SelectedSKUs, List<int> selectedDCs, int SelectedAgeing = 0, int Filtered = 0);
        List<long> SaveGenerateCSV(DateTime? fromDate, DateTime? toDate, int custId, int userId, List<int> selectedStores, List<int> selectedSKUs, List<int> selectedDCs, int selectedAgeing, int Filtered, List<int> selectedCategories, List<int> selectedTypes, List<int> selectedLocations);
        void disableOrderForNetsuiteUpload(List<NetSuitUpload> customers);
        List<CategoryWiseReasonMaster> GetReasonlist(string categoryid);
        int UpdateStatus(InvoiceDispatch invoiceDispatch);
        List<storeBO> GetStoreName(string invoiceVal);
        long AddUpdatePODH(PODheaderBO pODHeader);
        //long savePODDetailResponse(PODDetail pODDetail);

        List<StoreOrderVB> GetStoreDraftOrder(int storeID, int custId, int userid);
        long UpdateDraftQuantity(int storeID, int itemId, int quantity, string v, int userid);
        OrderSaveResponse SaveStoreOrderToOrderExcel(OrderExcel orderExcel, string issave);
        OrderSaveResponse FinilizeDraft(OrderExcel orderExcel, string issave);
        long DeleteStoreDraft(int userId, int storeId);
        long GetMaxGroupIDByCustomer(int CustId);
        List<TrackingBO> GetTrackingList(string action, string uniqueReferenceID, string invoiceNo, int custid);
        List<TrackingBO> GetStatusList(string uniqueReferenceID, string invoiceNo, int custid);
        List<StoreOrders> GetLastStoreOrders(string storeCode, int StoreId, int CustId, DateTime today);
        List<StoreOrderDetailsVB> GetLastStoreOrdersByDateStoreCode(DateTime storeOrderDate, string storeCode, int StoreId, int CustId, long? GroupId, string PONumber);
        List<OrderDeleteClose> OrderDeleteCloseSearch(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> selectedDC, List<int> selectedStores, List<int> selectedSKUs, string action);
        string OrderDeleteClose(List<OrderDeleteItems> Ids, string Action, int userid);
        long SaveCSVFileDetails(string filename, string path, int custId, int userId, DateTime noW);
        List<CSVFileMasterVM> GetCSVFileDetails(int custId, int userId, DateTime date, DateTime toDate);
        long SavePODImg(PODImages pODImages);
        #region POD Bulk Update
        List<PODBulkUpdate> GetInvoiceForBulkUpdateByVehicleNo(string vehicleNo, int custid);
        //Hadiram 
        long UploadExcelFileData(OrderExcelMapping item);
        long UploadInwardFileData(OrderExcelInward item);
        long ProcessCompleteExcelUploadOutward(string uniqueOrderNo, long fileId, int loggedInCustomer);
        long UploadOrderInventoryFileData(OrderInventory item);
        #endregion
        #region NetsuiteInventory
        List<NetSuiteInventory> GetNetSuiteInventory(int custId, List<int> DCID, List<int> SKUID, int? userId);
        long GetNetSuiteInventoryFromAPI(int UserId, ref string ErrorMessage);
        List<NetSuiteInventory> GetCustomerWiseDCWiseInventory(int custId, List<int> DCIDs, List<int> SKUIds, int? userId);
        List<GBPA> GetGBPLData(int custId, int userId, DateTime now);
        List<TopItemCustomers> GetTopItemsForCustomer(int userid, int custid, int id, List<int> selectedItemType, List<int> selectedItemcategories);
        int UpdateTopItemList(List<TopItemCustomers> items, int id, List<int> selectedItemType, List<int> selectedItemcategories, int userid, int custid);
        List<NetSuitUpload> NetSuitUploadsExcel_OrderHeaderIds(int custId, int userId, List<long> orderHeaderIds, List<ExcelOrder> excelOrders, string v);
        long RemoveFromViewCSVList(List<int> excelOrderIds, int custId, int userId);
        #endregion
        List<OrderDeleteNew> USP_FinlizedOrderDeleteSearch(DateTime startDate, DateTime endDate, int custid, List<int> dCs, List<int> stores, string RFPLID);
        string FinlizedOrderDelete(List<OrderDeleteItems> Ids, int userid);
        string GetPoNumber(int storeId, int? loggedInCustId, int userId);
        string ImportData(DataTable ds, string sp_name, string sysfilename, int userId);
        string OrderEdit(List<OrderEdit> id);
        List<OrderEdit> OrderEditSearch(DateTime? FromDate, DateTime? ToDate, string OrderId);
        OrderEdit OrderEditDetails(int id);
        string UpdateOrders(int Id, string OrderQuantity);
        List<ExcelOrder> GenerateOrder(DateTime? fromDate, DateTime? toDate, int custId, int userId, List<string> selectedStores, List<string> selectedSKUs, List<int> SelectedDCs, int? selectedAgeing, int? isSearch, int? firstCall, List<int> selectedCategory, List<int> selectedTypes, List<int> selectedLocations,string Unique_Reference_Id);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public List<OrderDeleteNew> USP_FinlizedOrderDeleteSearch(DateTime startDate, DateTime endDate, int custid, List<int> dCs, List<int> stores, string RFPLID)
        {
            return _orderRepository.USP_FinlizedOrderDeleteSearch(startDate, endDate, custid, dCs, stores, RFPLID);
        }
        //public int  DeletefinlizedOrder(int orderid, int userid, string Reason)
        //{
        //    return _orderRepository.DeletefinlizedOrder(orderid, userid, Reason);
        //}
        public string FinlizedOrderDelete(List<OrderDeleteItems> Ids, int userid)
        {
            return _orderRepository.FinlizedOrderDelete(Ids, userid);
        }

        public long SaveOrderDetail(OrderDetail orderDetail)
        {
            return _orderRepository.SaveOrderDetail(orderDetail);
        }

        public long SaveOrderHeader(OrderHeader orderHeader)
        {
            return _orderRepository.SaveOrderHeader(orderHeader);
        }

        public OrderHeader GetOrderHeader(long orderid)
        {
            return _orderRepository.GetOrderHeader(orderid);
        }

        public List<OrderHeader> GetSaveAsDraftOrders(int storeid)
        {
            return _orderRepository.GetSaveAsDraftOrders(storeid);
        }

        public List<OrderDetail> GetOrderDetails(long Orderheaderid)
        {
            return _orderRepository.GetOrderDetails(Orderheaderid);
        }

        public int UpdateOrderHeader(OrderHeader orderHeader)
        {
            return _orderRepository.UpdateOrderHeader(orderHeader);
        }

        public int UpdateOrderDetails(OrderDetail orderDetail)
        {
            return _orderRepository.UpdateOrderDetails(orderDetail);
        }

        public OrderDetail GetOrderDetail(long orderid, int itemid)
        {
            return _orderRepository.GetOrderDetail(orderid, itemid);
        }

        public int deleteOrderDetail(OrderDetail orderDetail)
        {
            return _orderRepository.deleteOrderDetail(orderDetail);
        }

        public List<OrderHeader> GetFinilizeOrder(int storeid)
        {
            return _orderRepository.GetFinilizeOrder(storeid);
        }

        public List<OrderHeader> GetProcessedOrder(int storeid)
        {
            return _orderRepository.GetProcessedOrder(storeid);
        }

        public int InsertTemp(TempTbl tempTbl)
        {
            return _orderRepository.InsertTemp(tempTbl);
        }

        public List<TempTbl> tempTbls(long headerid)
        {
            return _orderRepository.tempTbls(headerid);
        }

        public int DeleteTemptbl(long headerid)
        {
            return _orderRepository.DeleteTemptbl(headerid);
        }

        public int DeleteParticularItem(long headerid, int productid)
        {
            return _orderRepository.DeleteParticularItem(headerid, productid);
        }

        public List<ProcessCls> GetFinilisedForProcess()
        {
            return _orderRepository.GetFinilisedForProcess();
        }

        public List<NetSuitUpload> netSuitUploads(DateTime FromDate, DateTime ToDate)
        {
            return _orderRepository.netSuitUploads(FromDate, ToDate);
        }

        public List<NetSuitUpload> netSuitUploadsExcel(DateTime FromDate, DateTime ToDate, int custid, int userid)
        {
            return _orderRepository.netSuitUploadsExcel(FromDate, ToDate, custid, userid);
        }

        public TempTbl TempTbl(long orderheaderid, int itemid)
        {
            return _orderRepository.TempTbl(orderheaderid, itemid);
        }

        public int UpdateQtyInTemp(TempTbl tempTbl)
        {
            return _orderRepository.UpdateQtyInTemp(tempTbl);
        }

        public List<OrderHeader> MakeaCopy(int storeid)
        {
            return _orderRepository.MakeaCopy(storeid);
        }

        public List<DetailReport> SoApprove(int storeid)
        {
            return _orderRepository.SoApprove(storeid);
        }

        public List<OrderHeader> FullfillmentCount(int storeid)
        {
            return _orderRepository.FullfillmentCount(storeid);
        }

        public List<OrderHeader> InvoiceCount(int storeid)
        {
            return _orderRepository.InvoiceCount(storeid);
        }

        public int DeleteOrder(long orderid, int userid)
        {
            return _orderRepository.DeleteOrder(orderid, userid);
        }

        public string ItemForCustomer(string StoreCode, string SkuCode, int LoggedInCustomer, int loggedInUser, string uniqueReferenceID)
        {
            return _orderRepository.ItemForCustomer(StoreCode, SkuCode, LoggedInCustomer, loggedInUser, uniqueReferenceID);
        }

        public long InsertOrderHeader(string StoreCode, int UserId, string orderReferenceNo, byte? OrderSource, DateTime? OrderDate)
        {
            return _orderRepository.InsertOrderHeader(StoreCode, UserId, orderReferenceNo, OrderSource, OrderDate);
        }

        public long InsertOrderDetail(string ItemCode, int Qty, long OrderHeaderId, string uniqueReferenceID)
        {
            return _orderRepository.InsertOrderDetail(ItemCode, Qty, OrderHeaderId, uniqueReferenceID);
        }

        public int DeleteFinalisedOrder(long Id, string reason, int userid)
        {
            return _orderRepository.DeleteFinalisedOrder(Id, reason, userid);
        }

        public int UpdateOrder(long Orderheaderid, int Itemid, int Quantity, int userid)
        {
            return _orderRepository.UpdateOrder(Orderheaderid, Itemid, Quantity, userid);
        }

        public List<OFRLIFR> MakeCopyWithLFR(int storeid)
        {
            return _orderRepository.MakeCopyWithLFR(storeid);
        }
        public List<OrderHeader> GetPlannedOrderNotProcessed(int storeID, int statusFlag)
        {
            return _orderRepository.GetPlannedOrderNotProcessed(storeID, statusFlag);
        }
        public List<CurrentItemInventoryMst> GetCurrentItemInventoryMstForCustomer(int custid, int userid, List<int> storeIds)
        {
            return _orderRepository.GetCurrentItemInventoryMstForCustomer(custid, userid, storeIds);
        }
        public List<OrderHeader> GetDraftOrdersforAdminUser(int userid, int custid, List<int> allowedStores)
        {
            return _orderRepository.GetDraftOrdersforAdminUser(custid, userid, allowedStores);
        }
        public List<OrderDetail> GetDraftOrderDetails(long OrderId, int userid, int custid, List<int> storeIds)
        {
            return _orderRepository.GetDraftOrderDetails(OrderId, custid, userid, storeIds);
        }
        public List<CurrentItemInventoryMst> GetCurrentItemInventoryMstForStoreandItemandCategory(int custid, int userid, List<int> storeIds, List<int> categories, List<int> items)
        {
            return _orderRepository.GetCurrentItemInventoryMstForStoreandItemandCategory(custid, userid, storeIds, categories, items);
        }
        public List<SoApproveHeader> GetSoForOrder(string finalizedorderno)
        {
            return _orderRepository.GetSoForOrder(finalizedorderno);
        }
        public List<InvoiceHeader> GetInvoicesforSos(List<string> sonumbers)
        {
            return _orderRepository.GetInvoicesforSos(sonumbers);
        }
        public List<FullfillmentHeader> GetFullfillmentsforSos(List<string> sonumbers)
        {
            return _orderRepository.GetFullfillmentsforSos(sonumbers);
        }
        public InvoiceHeader GetInvoiceInformation(string invoiceNo)
        {
            return _orderRepository.GetInvoiceInformation(invoiceNo);
        }
        public long savePODResponse(PODHeader PODHeader)
        {
            return _orderRepository.savePODResponse(PODHeader);
        }

        public List<PODDetail> GetPODDetails(long pk_PODHeader)
        {
            return _orderRepository.GetPODDetails(pk_PODHeader);
        }
        public void DeletePODHeader(long pk_PODHeader, int userid)
        {
            _orderRepository.DeletePODHeader(pk_PODHeader, userid);
        }
        public List<InvoiceHeader> GetInvoicesforDateDC(DateTime invoiceDate, string selectedDCs)
        {
            return _orderRepository.GetInvoicesforDateDC(invoiceDate, selectedDCs);
        }
        public List<string> GetDistinctLocationsFromInvoice(int userid, int custid)
        {
            return _orderRepository.GetDistinctLocationsFromInvoice(userid, custid);
        }
        public long AddInvoiceDispatch(InvoiceDispatch invoiceDispatch)
        {
            return _orderRepository.AddInvoiceDispatch(invoiceDispatch);
        }
        public List<InvoiceHeader> GetInvoicesforCustomer(int custid, int userId)
        {
            return _orderRepository.GetInvoicesforCustomer(custid, userId);
        }
        public List<InvoiceHeader> SearchInvoicesforStoreInvoice(int custid, int userId, string storeCode, string invoiceNo)
        {
            return _orderRepository.SearchInvoicesforStoreInvoice(custid, userId, storeCode, invoiceNo);
        }
        public string VehicleDispursementErrors(string from_place, string trandate, string vehicleNo, string docNo)
        {
            return _orderRepository.VehicleDispursementErrors(from_place, trandate, vehicleNo, docNo);
        }
        public OrderFormatExcel GetUploadOrderFormat(int loggedInCustomer, string OrderType)
        {
            return _orderRepository.GetUploadOrderFormat(loggedInCustomer, OrderType);
        }
        public List<Vehicle> GetAutoVehicles(string vehicleNo, int CustId)
        {
            return _orderRepository.GetAutoVehicles(vehicleNo, CustId);
        }
        public List<InvoiceDispatch> GetVehiclesDAforDate(DateTime dispatchDate)
        {
            return _orderRepository.GetVehiclesDAforDate(dispatchDate);
        }
        public long SaveDAForVehicle(InvoiceDispatch invoiceDispatch)
        {
            return _orderRepository.SaveDAForVehicle(invoiceDispatch);
        }
        public long InsertOrderData(int loggedInCustomer, string storeCode, string skuCode, string qty, DateTime? orderDate, string orderReferenceNo, string orderPlacedDate, string orderType, string city, string lastOrderDate, string lateDeliveryDate, string uniqueReferenceID, string packSize, string orderedQty, string caseSize, string cases, string state, string fromDC, string new_Build, string focus, string item_Type, string item_sub_Type, string takenDays, string slab)
        {
            return _orderRepository.InsertOrderData(loggedInCustomer, storeCode, skuCode, qty, orderDate, orderReferenceNo, orderPlacedDate, orderType, city, lastOrderDate, lateDeliveryDate, uniqueReferenceID, packSize, orderedQty, caseSize, cases, state, fromDC, new_Build, focus, item_Type, item_sub_Type, takenDays, slab);
        }
        public long SaveExcelFileData(OrderExcel import)
        {
            return _orderRepository.SaveExcelFileData(import);
        }

        public List<ExcelOrder> GetUploadedStoreItems(DateTime? fromDate, DateTime? toDate, int custId, int userId, List<string> selectedStores, List<string> selectedSKUs, List<int> SelectedDCs, int selectedAgeing, int isSearch, int firstCall, List<int> selectedCategory, List<int> selectedTypes, List<int> selectedLocations)
        {
            return _orderRepository.GetUploadedStoreItems(fromDate, toDate, custId, userId, selectedStores, selectedSKUs, SelectedDCs, selectedAgeing, isSearch, firstCall, selectedCategory, selectedTypes, selectedLocations);
        }
        public long SaveUploadedFile(UploadedFilesMst mst)
        {
            return _orderRepository.SaveUploadedFile(mst);
        }
        public long SaveUploadedFileToTemp(OrderExcel order, int custId, int UserId)
        {
            return _orderRepository.SaveUploadedFileToTemp(order, custId, UserId);
        }
        public List<ExcelOrder> GetDataToGenerateCSV(DateTime fromDate, DateTime toDate, int custId, int userId)
        {
            return _orderRepository.GetDataToGenerateCSV(fromDate, toDate, custId, userId);
        }
        public List<ExcelOrder> GetDataToGenerateCSVSearch(DateTime? fromDate, DateTime? toDate, int custId, int userId, List<int> selectedStores, List<int> selectedSKUs, int SelectedAgeing, int Filtered, List<int> selectedDCs, List<int> selectedCategories, List<int> selectedTypes, List<int> selectedLocations)
        {
            return _orderRepository.GetDataToGenerateCSVSearch(fromDate, toDate, custId, userId, selectedStores, selectedSKUs, SelectedAgeing, Filtered, selectedDCs, selectedCategories, selectedTypes, selectedLocations);
        }

        public long InsertOrderHeaderCSV(string StoreCode, int UserId, string orderReferenceNo, byte? OrderSource, DateTime? OrderDate, int type, int DcId)
        {
            return _orderRepository.InsertOrderHeaderCSV(StoreCode, UserId, orderReferenceNo, OrderSource, OrderDate, type, DcId);
        }
        public long InsertOrderDetailCSV(string ItemCode, int Qty, long OrderHeaderId, string uniqueReferenceID, long ExcelId, int userId, int dCId)
        {
            return _orderRepository.InsertOrderDetailCSV(ItemCode, Qty, OrderHeaderId, uniqueReferenceID, ExcelId, userId, dCId);
        }
        public List<NetSuitUpload> netSuitUploadsExcel2(DateTime FromDate, DateTime ToDate, int custid, int userid)
        {
            return _orderRepository.netSuitUploadsExcel2(FromDate, ToDate, custid, userid);
        }
        public List<NetSuitUpload> netSuitUploadsExcel_OrderHeaderIds(DateTime FromDate, DateTime ToDate, int custid, int userid, List<long> OrderHeaderIds)
        {
            return _orderRepository.netSuitUploadsExcel_OrderHeaderIds(FromDate, ToDate, custid, userid, OrderHeaderIds);
        }
        public List<UploadedFilesView> GetUploadedFileDetails(int custId, int userId, List<string> fileType)
        {
            return _orderRepository.GetUploadedFileDetails(custId, userId, fileType);
        }
        public List<File_Error_LogsView> GetFileErrors(int custId, int userId, int fileId, string FileType)
        {
            return _orderRepository.GetFileErrors(custId, userId, fileId, FileType);
        }
        public void DeletedIgnoredRow(DateTime? fromDate, DateTime? toDate, int custId, int userId, List<OrderExcel> excelOrders, List<int> selectedStores, List<int> SelectedSKUs, int SelectedAgeing, int isSearch, List<int> SelectedDCs, List<int> selectedCategory, List<int> selectedTypes, List<int> selectedLocations)
        {
            _orderRepository.DeletedIgnoredRow(fromDate, toDate, custId, userId, excelOrders, selectedStores, SelectedSKUs, SelectedAgeing, isSearch, SelectedDCs, selectedCategory, selectedTypes, selectedLocations);
        }

        public List<ExcelOrder> GetDataQuantity(int custId, int userId, DateTime? FromDate, DateTime? ToDate, List<string> SelectedStores, List<string> SelectedSKUs, List<int> selectedDCs, int SelectedAgeing = 0, int Filtered = 0)
        {
            return _orderRepository.GetDataQuantity(custId, userId, FromDate, ToDate, SelectedStores, selectedDCs, SelectedSKUs, SelectedAgeing, Filtered);
        }
        public List<long> SaveGenerateCSV(DateTime? fromDate, DateTime? toDate, int custId, int userId, List<int> selectedStores, List<int> selectedSKUs, List<int> selectedDCs, int selectedAgeing, int Filtered, List<int> selectedCategories, List<int> selectedTypes, List<int> selectedLocations)
        {
            return _orderRepository.SaveGenerateCSV(fromDate, toDate, custId, userId, selectedStores, selectedSKUs, selectedDCs, selectedAgeing, Filtered, selectedCategories, selectedTypes, selectedLocations);
        }
        public void disableOrderForNetsuiteUpload(List<NetSuitUpload> customers)
        {
            if (customers != null && customers.Count() > 0)
                _orderRepository.disableOrderForNetsuiteUpload(customers.Select(x => x.PO).Distinct().ToList());
        }

        public int UpdateStatus(InvoiceDispatch invoiceDispatch)
        {
            return _orderRepository.UpdateStatus(invoiceDispatch);
        }

        //public long AddUpdatePODH(PODHeader pODHeader)
        //{
        //    return _orderRepository.AddUpdatePODH(pODHeader);
        //}     

        #region POD
        public List<PODListData> GetInvoicesforVehicle(string vehicleNo, int custid)
        {
            return _orderRepository.GetInvoicesforVehicle(vehicleNo, custid);
        }
        public List<storeBO> GetStoreName(string invoiceVal)
        {
            return _orderRepository.GetStoreName(invoiceVal);
        }
        public long AddUpdatePODH(PODheaderBO pODheaderBO)
        {
            return _orderRepository.AddUpdatePODH(pODheaderBO);
        }
        public List<CategoryWiseReasonMaster> GetReasonlist(string categoryid)
        {
            return _orderRepository.GetReasonlist(categoryid);
        }
        public long savePODDetailResponse(PODDetail pODDetail, int InvoiceDispatchId)
        {
            return _orderRepository.savePODDetailResponse(pODDetail, InvoiceDispatchId);
        }
        #endregion

        #region StoreOrderToExcelOrder Nikhil Kadam
        public OrderSaveResponse SaveStoreOrderToOrderExcel(OrderExcel orderExcel, string issave)
        {
            return _orderRepository.SaveStoreOrderToOrderExcel(orderExcel, issave);
        }

        public List<StoreOrderVB> GetStoreDraftOrder(int storeID, int custId, int UserId)
        {
            return _orderRepository.GetStoreDraftOrder(storeID, custId, UserId);
        }

        public long UpdateDraftQuantity(int StoreId, int itemId, int quantity, string Method, int userid)
        {
            return _orderRepository.UpdateDraftQuantity(StoreId, itemId, quantity, Method, userid);
        }

        public OrderSaveResponse FinilizeDraft(OrderExcel orderExcel, string issave)
        {
            return _orderRepository.FinilizeDraft(orderExcel, issave);
        }
        public long DeleteStoreDraft(int userId, int storeId)
        {
            return _orderRepository.DeleteStoreDraft(userId, storeId);
        }
        public long GetMaxGroupIDByCustomer(int CustId)
        {
            return _orderRepository.GetMaxGroupIDByCustomer(CustId);
        }
        #endregion

        #region Tracking
        public List<TrackingBO> GetTrackingList(string action, string uniqueReferenceID, string InvoiceNo, int custid)
        {
            return _orderRepository.GetTrackingList(action, uniqueReferenceID, InvoiceNo, custid);
        }
        public List<TrackingBO> GetStatusList(string uniqueReferenceID, string InvoiceNo, int custid)
        {
            return _orderRepository.GetStatusList(uniqueReferenceID, InvoiceNo, custid);
        }
        #endregion

        #region OrderDeleteClose By Deepa 28/11/2019"

        public List<OrderDeleteClose> OrderDeleteCloseSearch(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, string Action)
        {
            return _orderRepository.OrderDeleteCloseSearch(startDate, endDate, custid, userid, uniqueReferenceID, dCs, stores, items, Action);
        }
        public string OrderDeleteClose(List<OrderDeleteItems> Ids, string Action, int userid)
        {
            return _orderRepository.OrderDeleteClose(Ids, Action, userid);
        }
        #endregion

        #region StoreOrderList Widget
        public List<StoreOrders> GetLastStoreOrders(string storeCode, int StoreId, int CustId, DateTime today)
        {
            return _orderRepository.GetLastStoreOrders(storeCode, StoreId, CustId, today);
        }
        public List<StoreOrderDetailsVB> GetLastStoreOrdersByDateStoreCode(DateTime storeOrderDate, string storeCode, int StoreId, int CustId, long? GroupId, string PONumber)
        {
            return _orderRepository.GetLastStoreOrdersByDateStoreCode(storeOrderDate, storeCode, StoreId, CustId, GroupId, PONumber);
        }
        #endregion

        #region SaveCVSFile By nikhil 2019/12/06
        public long SaveCSVFileDetails(string filename, string path, int custId, int userId, DateTime noW)
        {
            return _orderRepository.SaveCSVFileDetails(filename, path, custId, userId, noW);
        }

        public List<CSVFileMasterVM> GetCSVFileDetails(int custId, int userId, DateTime fromDate, DateTime toDate)
        {
            return _orderRepository.GetCSVFileDetails(custId, userId, fromDate, toDate);
        }
        #endregion

        #region SavePODImage by Rekha 2019/12/24
        public long SavePODImg(PODImages pODImages)
        {
            return _orderRepository.SavePODImg(pODImages);
        }
        #endregion


        #region API 28/01/2020
        public List<OrderInboundResponse> AddOrderInbound(List<OrderInbound> orders)
        {
            List<OrderInboundResponse> OrderResponses = new List<OrderInboundResponse>();
            foreach (OrderInbound order in orders)
            {
                OrderInboundResponse response = _orderRepository.AddOrderInbound(order);
            }
            return OrderResponses;
        }
        #endregion

        #region POD Bulk Update 26/02/2020 deepa
        public List<PODBulkUpdate> GetInvoiceForBulkUpdateByVehicleNo(string vehicleNo, int custid)
        {
            return _orderRepository.GetInvoiceForBulkUpdateByVehicleNo(vehicleNo, custid);
        }
        #endregion

        #region HaldiRamOrderUploads
        public long UploadExcelFileData(OrderExcelMapping import)
        {
            return _orderRepository.UploadExcelFileData(import);
        }
        public long UploadInwardFileData(OrderExcelInward import)
        {
            return _orderRepository.UploadInwardFileData(import);
        }
        public long ProcessCompleteExcelUploadOutward(string uniqueOrderNo, long fileId, int loggedInCustomer)
        {
            return _orderRepository.ProcessCompleteExcelUploadOutward(uniqueOrderNo, fileId, loggedInCustomer);
        }

        public long UploadOrderInventoryFileData(OrderInventory item)
        {
            return _orderRepository.UploadOrderInventoryFileData(item);
        }
        #endregion

        #region NetSuite Inventory while Processing
        //25/01/2021 Deepa
        public List<NetSuiteInventory> GetNetSuiteInventory(int custId, List<int> DCID, List<int> SKUId, int? userId)
        {
            return _orderRepository.GetNetSuiteInventory(custId, DCID, SKUId, userId);
        }
        public long GetNetSuiteInventoryFromAPI(int UserId, ref string ErrorMessage)
        {
            List<OrderInventory> lst = new List<OrderInventory>();

            //consume API
            // lst= NetSuiteInventoryAPI("ALL");
            List<NetSuiteInventoryAPIResponse> lstAPIResponse = NetSuiteInventoryAPI(ref ErrorMessage);
            //Insert/update into database
            //return _orderRepository.InsertNetSuiteInventory(lstAPIResponse, UserId);
            if (lstAPIResponse != null)
            {
                _orderRepository.InsertNetSuiteInventoryBulkUpload(lstAPIResponse, UserId);
                return 1;
            }
            else { return 0; }
        }
        public static List<NetSuiteInventoryAPIResponse> NetSuiteInventoryAPI(ref string ErrorMessage)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate (
                 Object obj, X509Certificate certificate, X509Chain chain,
                 SslPolicyErrors errors)
            {
                return (true);
            };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            string timeout = System.Configuration.ConfigurationManager.AppSettings["Ns-APITimeout"].ToString();
            int Seconds = Convert.ToInt32(timeout);
            //---------------------------------------
            string NS_realm = "3424475";// 1234567_SB1 for sandbox
            string url = "https://3424475.restlets.api.netsuite.com/app/site/hosting/restlet.nl?script=778&deploy=1";
            //string url = "https://3424475.restlets..netsuite.com/app/site/hosting/restlet.nl?script=778&deploy=1";
            HttpWebRequest request = null;
            String header = "Authorization: OAuth ";
            try
            {
                string consumer_id = "3fdba25b8d40d9d37b517d0d7d9ffa7e073ad4e38738231ac52fc18061cf2cb3"; // (Also called Consumer Key)
                string consumer_secret = "9117ec9593ff0c830bbcfaf29dfee5464739b75fb7057705a52b5fa432552f8f";
                string token_id = "49fa93092070dc43beae4faf3df958c9e6d1f9f1f43ce8a03afa0e84430ece1f";
                string token_secret = "75a484c90e452cf7314a63b5ce89da947ac845fbf62677bb9aee42993676808e";

                Uri uri = new Uri(url);

                BAL.OAuthBase req = new BAL.OAuthBase();

                string normalized_url;
                string normalized_params;

                // string nonce = "8GLUyzLuPdC";//req.GenerateNonce();
                string nonce = req.GenerateNonce();
                // string time = "1619441554";
                string time = req.GenerateTimeStamp();
                //string AgentName = "integration4@rkfoodland.com";
                //string signature = "QGm%2Bc6401D%2Byyi6XIlbLohzgMM4Tnd4xO15zL55CfBY%3D";
                string signature = req.GenerateSignature(uri, consumer_id, consumer_secret, token_id, token_secret, "POST",
                                     time, nonce, out normalized_url, out normalized_params);

                // URL encode any + characters generated in the signature
                if (signature.Contains("+"))
                {
                    signature = signature.Replace("+", "%2B");
                }

                // Construct the OAuth header		
                header += "oauth_signature=\"" + signature + "\",";
                header += "oauth_version=\"1.0\",";
                header += "oauth_nonce=\"" + nonce + "\",";
                header += "oauth_signature_method=\"HMAC-SHA256\",";
                header += "oauth_consumer_key=\"" + consumer_id + "\",";
                header += "oauth_token=\"" + token_id + "\",";
                header += "oauth_timestamp=\"" + time + "\",";
                // header += "User-Agent=\"" + AgentName + "\",";

                header += "realm=\"" + NS_realm + "\"";

            }
            catch (Exception q)
            {
                BAL.HelperCls.DebugLog(q.Message);
                ErrorMessage += q.Message;
                //Console.WriteLine("Configuration error. Check tokens and NBN.");
            }
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "POST";
                request.Headers.Add(header);
                byte[] DCNames = Encoding.ASCII.GetBytes("\"ALL\"");
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(DCNames, 0, DCNames.Length);
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine("Couldn't generate request. Check RESTlet URL and NBN.");
                BAL.HelperCls.DebugLog(e.Message + System.Environment.NewLine + e.StackTrace);
                ErrorMessage += e.Message;
            }
            try
            {
                request.Timeout = Seconds * 1000;
                List<NetSuiteInventoryAPIResponse> ResponseResult = null;
                using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream webStream = webResponse.GetResponseStream())
                    {
                        using (StreamReader responseReader = new StreamReader(webStream))
                        {
                            string response = responseReader.ReadLine();
                            ResponseResult = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NetSuiteInventoryAPIResponse>>(response);
                        }
                    }
                }
                return ResponseResult;
            }
            catch (UriFormatException e)
            {
                BAL.HelperCls.DebugLog(e.Message + System.Environment.NewLine + e.StackTrace);
                //Console.WriteLine("Error forming request. Check tokens, URL, and NBN details.");
                ErrorMessage += e.Message;
                return null;
            }
            catch (Exception e)
            {
                BAL.HelperCls.DebugLog(e.Message + System.Environment.NewLine + e.StackTrace);
                //Console.WriteLine("Server or connection error:\n" + e.ToString());
                ErrorMessage += e.Message;
                return null;
            }
        }
        public List<NetSuiteInventory> GetCustomerWiseDCWiseInventory(int custId, List<int> DCID, List<int> SKUId, int? userId)
        {
            return _orderRepository.GetCustomerWiseDCWiseInventory(custId, DCID, SKUId, userId);
        }

        #endregion
        #region GBPA
        public List<GBPA> GetGBPLData(int custId, int userId, DateTime date)
        {
            return _orderRepository.GetGBPLData(custId, userId, date);
        }
        #endregion
        #region TopItemListCustomerWise
        public List<TopItemCustomers> GetTopItemsForCustomer(int userid, int custid, int SelectedTopItems, List<int> selectedItemType, List<int> selectedItemcategories)
        {
            return _orderRepository.GetTopItemsForCustomer(userid, custid, SelectedTopItems, selectedItemType, selectedItemcategories);
        }
        public int UpdateTopItemList(List<TopItemCustomers> items, int id, List<int> selectedItemType, List<int> selectedItemcategories, int userid, int custid)
        {
            return _orderRepository.UpdateTopItemList(items, id, selectedItemType, selectedItemcategories, userid, custid);
        }
        #endregion

        public List<NetSuitUpload> NetSuitUploadsExcel_OrderHeaderIds(int custId, int userId, List<long> orderHeaderIds, List<ExcelOrder> excelOrders, string OrderExcel)
        {
            return _orderRepository.NetSuitUploadsExcel_OrderHeaderIds(custId, userId, orderHeaderIds, excelOrders, OrderExcel);
        }
        public long RemoveFromViewCSVList(List<int> excelOrderIds, int custId, int userId)
        {
            return _orderRepository.RemoveFromViewCSVList(excelOrderIds, custId, userId);
        }
        public string GetPoNumber(int storeId, int? loggedInCustId, int loggedinuserid)
        {
            return _orderRepository.GetPoNumber(storeId, loggedInCustId, loggedinuserid);
        }
        public string ImportData(DataTable ds, string sp_name, string sysfilename, int userId)
        {
            return _orderRepository.ImportData(ds, sp_name, sysfilename, userId);
        }
        #region OrderEdit

        public List<OrderEdit> OrderEditSearch(DateTime? FromDate, DateTime? ToDate, string OrderId)
        {
            return _orderRepository.OrderEditSearch(FromDate, ToDate, OrderId);
        }

        public OrderEdit OrderEditDetails(int id)
        {
            return _orderRepository.OrderEditdetails(id);
        }
        public string OrderEdit(List<OrderEdit> id)
        {
            return _orderRepository.OrderEdit(id);
        }
        #endregion
        public string UpdateOrders(int Id, string OrderQuantity)
        {
            return _orderRepository.UpdateOrders(Id, OrderQuantity);
        }
        public List<ExcelOrder> GenerateOrder(DateTime? fromDate, DateTime? toDate, int custId, int userId, List<string> selectedStores, List<string> selectedSKUs, List<int> SelectedDCs, int? selectedAgeing, int? isSearch, int? firstCall, List<int> selectedCategory, List<int> selectedTypes, List<int> selectedLocations,string Unique_Reference_Id)
        {
            return _orderRepository.GenerateOrder(fromDate, toDate, custId, userId, selectedStores, selectedSKUs, SelectedDCs, selectedAgeing, isSearch, firstCall, selectedCategory, selectedTypes, selectedLocations, Unique_Reference_Id);
        }
    }
}
