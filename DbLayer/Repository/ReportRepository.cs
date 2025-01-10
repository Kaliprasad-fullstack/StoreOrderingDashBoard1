using BAL;
using DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLayer.Repository
{
    public interface IReportRepository
    {
        List<OrderHeader> GetFinilizeOrder(int custid);
        List<OrderHeader> GetProcessedOrder(int custid);
        List<OrderHeader> GetSaveAsDraftOrders();
        Store GetStoreDetailsbyId(int storeId);
        List<OrderDetail> GetOrderDetailsForOrderHeaderId(long id);
        Item GetItemById(int itemId);
        List<DetailReport> DetailReport(DateTime FromDate, DateTime ToDate, bool IsProcessed, bool IsOrderStatus);
        List<DetailReport> DetailReportByStore(DateTime FromDate, DateTime ToDate, bool IsProcessed, bool IsOrderStatus, bool IsSoApproved, bool IsFullFillment, bool IsInvoice, int StoreId);
        List<DetailReport> DetailFinilizeReport(DateTime FromDate, DateTime ToDate, bool IsProcessed, bool IsOrderStatus, bool IsSoApprove, bool IsFullFillment, bool IsInvoice, int CustId, int UserId);
        List<Top5Store> Top5Stores(DateTime FromDate, DateTime ToDate, int custid, int userid);
        List<DetailReport> SummeriseReportDcAdmin(DateTime FromDate, DateTime ToDate, bool IsProcessed, bool IsOrderStatus, bool IsSoApprove, bool IsFullFillment, bool IsInvoice, int CustId, int UserId);
        List<DetailReport> ProcessedData(bool IsProcessed, bool IsOrderStatus, bool IsSoApproved, bool IsFullfillment, bool IsInvoice, int CustId, int UserId);
        List<DetailReport> SoApprovedReport(DateTime FromDate, DateTime ToDate, int customerid, int userid);
        List<DetailReport> SoApprovedReportByStore(DateTime FromDate, DateTime ToDate, int StoreId);
        List<DetailReport> SoApprovedDetailReport(DateTime FromDate, DateTime ToDate, int customerid, int userid);
        List<DetailReport> SoApprovedDetailReportByStore(DateTime FromDate, DateTime ToDate, int storeid);
        List<DetailReport> SoApproved(int customerid, int userid);
        List<DetailReport> NetsuiteReportsByStore(DateTime FromDate, DateTime ToDate, bool IsOrderStatus, bool IsProcessed, bool IsSoApproved, bool IsFullfillment, bool IsInvoice, int StoreId);
        List<DetailReport> NetSuiteItemReport(string SoNumber);
        List<DetailReport> NetsuiteDetailReportsByStore(DateTime FromDate, DateTime ToDate, bool IsOrderStatus, bool IsProcessed, bool IsSoApproved, bool IsFullfillment, bool IsInvoice, int StoreId);
        List<DetailReport> NetSuiteReportByAdmin(DateTime FromDate, DateTime ToDate, bool IsOrderStatus, bool IsProcessed, bool IsSoApproved, bool IsFullfillment, bool IsInvoice, int CustomerId, int UserId);
        List<DetailReport> NetSuiteDetailReportByAdmin(DateTime FromDate, DateTime ToDate, bool IsOrderStatus, bool IsProcessed, bool IsSoApproved, bool IsFullfillment, bool IsInvoice, int CustomerId, int UserId);
        List<FinalReport> GetFinalReport(DateTime FromDate, DateTime ToDate/*, bool IsProcessed, bool IsOrderStatus, bool IsSoApproved, bool IsFullFillment, bool IsInvoice,*/ , int CustId, int UserId);
        List<MTDCls> MTDCls(int UserId, int CustId, List<int> DCId = null);
        List<ReportCount> reportCounts(DateTime FromDate, DateTime ToDate, int CustId, string Region, string State, string Dc, string Store, string Sku, int UserId);
        List<OFRLIFR> OFRReport(DateTime FromDate, DateTime ToDate, int CustId, string Region, string State, string Dc, string Store, string Sku, int UserId);
        List<string> GetYear();
        List<string> GetMonth(string Year);
        List<int> GetWeek(string Year);
        List<int> GetWeekByQuarter(string Year, int Quarter);
        List<int> GetWeekByMonth(string Year, int Month);
        List<Week> GetDays(string Year, int Month);
        List<ReportCount> Revisedreport(DateTime FromDate, DateTime ToDate, int CustId, string Region, string State, string Dc, string Store, string Sku, int userid);
        // List<ReportCount> Revisedreport(DateTime FromDate, DateTime ToDate, int CustId, string Region, string State, string Dc, string Store, string Sku);

        //List<FinalReport> GetFinalReport(DateTime FromDate, DateTime ToDate, bool IsProcessed, bool IsOrderStatus, bool IsSoApproved, bool IsFullFillment, bool IsInvoice, int CustId, int UserId);
        //public List<FinalReport> GetFinalReport(DateTime FromDate, DateTime ToDate, bool IsProcessed, bool IsOrderStatus, bool IsSoApproved, bool IsFullFillment, bool IsInvoice, int CustId, int UserId, int QueryPicker)
        OFRLIFR OFRLIFR_MTD(int Userid, int Custid, List<int> dCId = null);
        int GetOrdersStoresToBePlaced(int custid, int userid);
        List<UniqueRefenenceReport> UniqueRefenenceReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items);
        List<DailySalesReport> DailySalesReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, List<int> selectedLocations, List<int> SelectedTypes, List<int> SelectedCategories);
        List<DailySalesReport> DailySalesBasicReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, List<int> selectedLocations, List<int> SelectedTypes, List<int> SelectedCategories, List<int> selectedBrands);
        List<Report2> Report2(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, int ReportType, List<int> SelectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null);
        List<PendencyOrderReport> PendencyOrderReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, string reportType, List<int> SelectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null);
        //modified by deepa 2020/01/06
        List<DeliveryStatusReport> DeliveryStatusReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> selectedDCs, List<int> selectedStores, List<int> selectedSKUs, int reportType, string invoiceNo, List<int> SelectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null);
        // Added By Deepa 11/13/2019
        List<AgeingReport> AgeingReport(DateTime startDate, DateTime endDate, int custid, int userid, List<int> dCs, List<int> stores, List<int> items, int ReportType, List<int> SelectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null);
        List<AgeingReport> AgeingReport2(DateTime startDate, DateTime endDate, int custid, int userid, List<int> dCs, List<int> stores, List<int> items, int ReportType, List<int> SelectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null);
        // Added By Deepa 11/14/2019
        List<AgeingDetailReport> AgeingDetailReport(DateTime startDate, DateTime endDate, int custid, int userid, List<string> dCs, string stores, List<string> items, int PDayFrom, int PdayTo, int ReportType);
        List<DailyOrderReport> DailyOrderReport(DateTime startDate, DateTime endDate, int custid, int userid, List<int> stores, List<int> items);
        List<TrackingBO> GetTrackingList(string action, string uniqueReferenceID, string invoiceNo, int custid);
        List<TrackingBO> GetStatusList(string uniqueReferenceID, string invoiceNo, int custid);
        List<Top5SKU> GetTop5SKUs(int custid, int userid, int month, int year);
        List<DAYWISEORDERQTY> GetStoreDayWiseOrderQty(int custid, int userid, int month, int year, int storeid, List<int> sKUs);
        //Added By Deepa 2019-12-09
        List<Report2> FulfillmentReportDrillDown(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, int ReportType, string CityCategory, List<int> selectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null);
        //added by Nikhil Kadam 2019/12/13
        List<TOPSKUForYEARMonth> GetStoreTOPSKUS_HalfYear(int UserID, int StoreID, int CustID, int year, int type);
        //PODImages by Rekha 2020-01-02        
        List<PODImageList> GetPODImages(string invoiceNo, string invoiceNo1, int custid);
        // OnTime Report 20/02/2020
        List<OnTimeReport> OnTimeReport(DateTime startDate, DateTime endDate, int custid, int userid, List<int> dCs, List<int> locationIds, int ReportType);
        List<OnTimeDetailReport> OnTimeReportDrillDown(DateTime startDate, DateTime endDate, int custid, int userid, int? dCs, int? locationIds, int ReportType);
        List<OrderDeleteCloseReport> OrderDeleteCloseReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> selectedStores, List<int> selectedSKUs, string action);

        //Store Home Widget MonthWiseOrderDateQTY By Nikhil 26-06-2021
        List<MonthWiseStoreOrderQty> GetStoreMonthWiseOrderDateQty(int custid, int userid, int month, int year, int storeid, List<int> sKUs);
        //
        List<ClosingStock> ClosingStockReport(int custid, int userid, List<int> selectedStores, List<int> selectedSKUs);
        //Dashboard
        List<MonthWiseTotalSalesValue> GetTotalSalesValue(int userId, int custId, List<int> SelectedDc, List<int> storeid, int year);
        List<MonthWiseTotalSalesValue> GetTotalSalesAmount(int userId, int custId, List<int> SelectedDc, List<int> storeid, int year);
        List<TopSKUForCustomer> GetTop10SKUByAmount(int userId, int custId, List<int> storeid, int year, List<int> SelectedDc);
        List<TopSKUForCustomer> GetTop10SKUByCase(int userId, int custId, List<int> storeid, int year, List<int> SelectedDc);
        List<MonthWiseFillRate> GetFillRateByMonth(int userId, int custId, List<int> storeid, int year, List<int> SelectedDc);
        List<StoreModel> ActiveStoresReport(string Opr, int custId, int userId, List<int> selectedDc, List<int> selectedStores);

        List<InvoiceView> InvoiceReport(DateTime startDate, DateTime endDate, int custid, int userid, List<int> stores, List<int> items);
        List<DailySalesReport> RABasicReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> selectedDC, List<int> selectedLocations, List<int> selectedStores, List<int> selectedSKUs, List<int> selectedTypes, List<int> selectedCategories, List<int> selectedBrands);
        List<DailyOrderReport> DailyOrderReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> selectedDC, List<int> selectedLocations, List<int> selectedStores, List<int> selectedSKUs, List<int> selectedTypes, List<int> selectedCategories, List<int> selectedBrands);
        List<DCStoreWiseItemReport> GetDCStoreWiseItemReport(string reportType, int custid, int userid, List<int> selectedDC, List<int> selectedStores, List<int> selectedSKUs, List<int> selectedTypes, List<int> selectedCategories, List<int> selectedBrands);
        List<ItemReport> GetItemReport(int custid, int userid, List<int> selectedDC, List<int> selectedStores, List<int> selectedSKUs, List<int> selectedTypes, List<int> selectedCategories, List<int> selectedBrands);
        List<StoreModel> GetStoreReport(int custid, int userid, List<int> selectedDC, List<int> selectedLocations, List<int> selectedStores);
        //List<PFAReport> PFAReport(int custId, int userId, List<int> selectedDc, List<int> selectedStores, List<int> SelectedSKUs, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands);
        List<PFAReport> PFAReport(int custId, List<int> selectedDC, List<int> SelectedStores);
        List<DailySalesReport> DailySalesBasicAdminReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> DCs, List<int> Stores, List<int> Items, List<int> selectedLocations, List<int> SelectedTypes = null, List<int> SelectedCategories = null, List<int> selectedBrands = null);


    }

    public class ReportRepository : IReportRepository
    {
        private readonly StoreContext _context;
        public ReportRepository(StoreContext context)
        {
            _context = context;
            _context.Database.CommandTimeout = 0;
        }

        public List<OrderHeader> GetFinilizeOrder(int custid)
        {
            try
            {
                return _context.orderHeaders.Where(x => x.IsOrderStatus == true && x.IsProcessed == false && x.Isdeleted == false && x.Store.Customer.Id == custid).OrderByDescending(x => x.OrderDate).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public Item GetItemById(int itemId)
        {
            try
            {
                return _context.Items.Where(x => x.Id == itemId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<OrderDetail> GetOrderDetailsForOrderHeaderId(long id)
        {
            try
            {
                return _context.orderDetails.Where(x => x.OrderHeaderId == id).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<OrderHeader> GetProcessedOrder(int custid)
        {
            try
            {
                return _context.orderHeaders.Where(x => x.IsProcessed == true && x.Isdeleted == false && x.IsOrderStatus == true && x.Store.Customer.Id == custid).OrderByDescending(x => x.OrderDate).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<OrderHeader> GetSaveAsDraftOrders()
        {
            try
            {
                //var a = _context.orderHeaders.ToList();
                return _context.orderHeaders.Where(x => x.IsOrderStatus == false && x.IsProcessed == false && x.Isdeleted == false).OrderByDescending(x => x.OrderDate).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public Store GetStoreDetailsbyId(int storeId)
        {
            try
            {
                //var a = _context.orderHeaders.ToList();
                return _context.Stores.Where(x => x.Id == storeId && x.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<DetailReport> DetailReport(DateTime FromDate, DateTime ToDate, bool IsProcessed, bool IsOrderStatus)
        {
            try
            {

                return _context.Database.SqlQuery<DetailReport>("exec USP_DetailDraftReport @FromDate,@ToDate,@IsProcessed,@IsOrderStatus",
                    new SqlParameter("@FromDate", FromDate),
                    new SqlParameter("@ToDate", ToDate),
                    new SqlParameter("@IsProcessed", IsProcessed),
                    new SqlParameter("@IsOrderStatus", IsOrderStatus)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<DetailReport> DetailReportByStore(DateTime FromDate, DateTime ToDate, bool IsProcessed, bool IsOrderStatus, bool IsSoApproved, bool IsFullFillment, bool IsInvoice, int StoreId)
        {
            try
            {

                return _context.Database.SqlQuery<DetailReport>("exec USP_GetDatailReportByStore @FromDate,@ToDate,@IsProcessed,@IsOrderStatus,@IsSoApproved,@IsFullFillment,@IsInvoice,@StoreId",
                    new SqlParameter("@FromDate", FromDate),
                    new SqlParameter("@ToDate", ToDate),
                    new SqlParameter("@IsProcessed", IsProcessed),
                    new SqlParameter("@IsOrderStatus", IsOrderStatus),
                    new SqlParameter("@IsSoApproved", IsSoApproved),
                    new SqlParameter("@IsFullFillment", IsFullFillment),
                    new SqlParameter("@IsInvoice", IsInvoice),
                    new SqlParameter("@StoreId", StoreId)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<Top5Store> Top5Stores(DateTime FromDate, DateTime ToDate, int custid, int userid)
        {
            try
            {
                return _context.Database.SqlQuery<Top5Store>("exec USP_Top5Store @FromDate,@ToDate,@CustId,@UserId",
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

        public List<DetailReport> DetailFinilizeReport(DateTime FromDate, DateTime ToDate, bool IsProcessed, bool IsOrderStatus, bool IsSoApprove, bool IsFullFillment, bool IsInvoice, int CustId, int UserId)
        {
            try
            {
                return _context.Database.SqlQuery<DetailReport>("exec USP_DetailReport @FromDate,@ToDate,@IsProcessed,@IsOrderStatus,@IsSoApproved,@IsFullFillment,@IsInvoice,@CustId,@UserId",
                   new SqlParameter("@FromDate", FromDate),
                   new SqlParameter("@ToDate", ToDate),
                   new SqlParameter("@IsProcessed", IsProcessed),
                   new SqlParameter("@IsOrderStatus", IsOrderStatus),
                   new SqlParameter("@IsSoApproved", IsSoApprove),
                   new SqlParameter("@IsFullFillment", IsFullFillment),
                   new SqlParameter("@IsInvoice", IsInvoice),
                   new SqlParameter("@CustId", CustId),
                    new SqlParameter("@UserId", UserId)
                   ).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<DetailReport> SummeriseReportDcAdmin(DateTime FromDate, DateTime ToDate, bool IsProcessed, bool IsOrderStatus, bool IsSoApprove, bool IsFullFillment, bool IsInvoice, int CustId, int UserId)
        {
            try
            {
                return _context.Database.SqlQuery<DetailReport>("exec USP_SummeriseReportForDCAdmin @FromDate,@ToDate,@IsProcessed,@IsOrderStatus,@IsSoApproved,@IsFullFillment,@IsInvoice,@CustId,@UserId",
                   new SqlParameter("@FromDate", FromDate),
                   new SqlParameter("@ToDate", ToDate),
                   new SqlParameter("@IsProcessed", IsProcessed),
                   new SqlParameter("@IsOrderStatus", IsOrderStatus),
                   new SqlParameter("@IsSoApproved", IsSoApprove),
                   new SqlParameter("@IsFullFillment", IsFullFillment),
                   new SqlParameter("@IsInvoice", IsInvoice),
                   new SqlParameter("@CustId", CustId),
                    new SqlParameter("@UserId", UserId)
                   ).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<DetailReport> ProcessedData(bool IsProcessed, bool IsOrderStatus, bool IsSoApproved, bool IsFullfillment, bool IsInvoice, int CustId, int UserId)
        {
            try
            {
                return _context.Database.SqlQuery<DetailReport>("exec USP_FinilisedOrderlistForProcess @IsProcessed,@IsOrderStatus,@IsSoapproved,@IsFullfillment,@IsInvoice,@CustId,@UserId",
                   new SqlParameter("@IsProcessed", IsProcessed),
                   new SqlParameter("@IsOrderStatus", IsOrderStatus),
                   new SqlParameter("@IsSoapproved", IsSoApproved),
                   new SqlParameter("@IsFullfillment", IsFullfillment),
                   new SqlParameter("@IsInvoice", IsInvoice),
                   new SqlParameter("@CustId", CustId),
                    new SqlParameter("@UserId", UserId)
                   ).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<DetailReport> SoApprovedReport(DateTime FromDate, DateTime ToDate, int customerid, int userid)
        {
            try
            {
                return _context.Database.SqlQuery<DetailReport>("exec USP_SoApproveReport @FromDate,@ToDate,@CustomerId,@UserId",
                   new SqlParameter("@FromDate", FromDate),
                   new SqlParameter("@ToDate", ToDate),
                   new SqlParameter("@CustomerId", customerid),
                   new SqlParameter("@UserId", userid)
                   ).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<DetailReport> SoApprovedReportByStore(DateTime FromDate, DateTime ToDate, int StoreId)
        {
            try
            {
                return _context.Database.SqlQuery<DetailReport>("exec USP_SoApproveReportByStore @FromDate,@ToDate,@StoreId",
                   new SqlParameter("@FromDate", FromDate),
                   new SqlParameter("@ToDate", ToDate),
                   new SqlParameter("@StoreId", StoreId)
                   ).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<DetailReport> SoApprovedDetailReport(DateTime FromDate, DateTime ToDate, int customerid, int userid)
        {
            try
            {
                return _context.Database.SqlQuery<DetailReport>("exec USP_SoApproveDetailReport @FromDate,@ToDate,@CustomerId,@UserId",
                   new SqlParameter("@FromDate", FromDate),
                   new SqlParameter("@ToDate", ToDate),
                   new SqlParameter("@CustomerId", customerid),
                   new SqlParameter("@UserId", userid)
                   ).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<DetailReport> SoApprovedDetailReportByStore(DateTime FromDate, DateTime ToDate, int storeid)
        {
            try
            {
                return _context.Database.SqlQuery<DetailReport>("exec USP_SoApproveDetailReportByStore @FromDate,@ToDate,@StoreId",
                   new SqlParameter("@FromDate", FromDate),
                   new SqlParameter("@ToDate", ToDate),
                   new SqlParameter("@StoreId", storeid)
                   ).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<DetailReport> SoApproved(int customerid, int userid)
        {
            try
            {
                return _context.Database.SqlQuery<DetailReport>("exec USP_SoApprovedCount @CustomerId,@UserId",
                   new SqlParameter("@CustomerId", customerid),
                   new SqlParameter("@UserId", userid)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<DetailReport> NetsuiteReportsByStore(DateTime FromDate, DateTime ToDate, bool IsOrderStatus, bool IsProcessed, bool IsSoApproved, bool IsFullfillment, bool IsInvoice, int StoreId)
        {
            try
            {
                return _context.Database.SqlQuery<DetailReport>("exec USP_FullFillmentReportByStore @FromDate,@ToDate,@IsOrderStatus,@IsProcessed,@IsSoApproved,@IsFullfillment,@IsInvoice,@StoreId",
                   new SqlParameter("@FromDate", FromDate),
                   new SqlParameter("@ToDate", ToDate),
                   new SqlParameter("@IsOrderStatus", IsOrderStatus),
                   new SqlParameter("@IsProcessed", IsProcessed),
                   new SqlParameter("@IsSoApproved", IsSoApproved),
                   new SqlParameter("@IsFullfillment", IsFullfillment),
                   new SqlParameter("@IsInvoice", IsInvoice),
                   new SqlParameter("@StoreId", StoreId)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<DetailReport> NetSuiteItemReport(string SoNumber)
        {
            try
            {
                return _context.Database.SqlQuery<DetailReport>("exec USP_FullFillmentReportByItem @CreatedFrom",
                   new SqlParameter("@CreatedFrom", SoNumber)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<DetailReport> NetsuiteDetailReportsByStore(DateTime FromDate, DateTime ToDate, bool IsOrderStatus, bool IsProcessed, bool IsSoApproved, bool IsFullfillment, bool IsInvoice, int StoreId)
        {
            try
            {
                return _context.Database.SqlQuery<DetailReport>("exec USP_FullfillmentDetailReportByStore @FromDate,@ToDate,@IsOrderStatus,@IsProcessed,@IsSoApproved,@IsFullFillment,@IsInvoice,@StoreId",
                   new SqlParameter("@FromDate", FromDate),
                   new SqlParameter("@ToDate", ToDate),
                   new SqlParameter("@IsOrderStatus", IsOrderStatus),
                   new SqlParameter("@IsProcessed", IsProcessed),
                   new SqlParameter("@IsSoApproved", IsSoApproved),
                   new SqlParameter("@IsFullfillment", IsFullfillment),
                   new SqlParameter("@IsInvoice", IsInvoice),
                   new SqlParameter("@StoreId", StoreId)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }


        public List<DetailReport> NetSuiteReportByAdmin(DateTime FromDate, DateTime ToDate, bool IsOrderStatus, bool IsProcessed, bool IsSoApproved, bool IsFullfillment, bool IsInvoice, int CustomerId, int UserId)
        {
            try
            {
                return _context.Database.SqlQuery<DetailReport>("exec USP_SummaryReportAdminReport @FromDate,@ToDate,@IsOrderStatus,@IsProcessed,@IsSoApproved,@IsFullFillment,@IsInvoice,@CustomerId,@UserId",
                  new SqlParameter("@FromDate", FromDate),
                  new SqlParameter("@ToDate", ToDate),
                  new SqlParameter("@IsOrderStatus", IsOrderStatus),
                  new SqlParameter("@IsProcessed", IsProcessed),
                  new SqlParameter("@IsSoApproved", IsSoApproved),
                  new SqlParameter("@IsFullfillment", IsFullfillment),
                  new SqlParameter("@IsInvoice", IsInvoice),
                  new SqlParameter("@CustomerId", CustomerId),
                  new SqlParameter("@UserId", UserId)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<DetailReport> NetSuiteDetailReportByAdmin(DateTime FromDate, DateTime ToDate, bool IsOrderStatus, bool IsProcessed, bool IsSoApproved, bool IsFullfillment, bool IsInvoice, int CustomerId, int UserId)
        {
            try
            {
                return _context.Database.SqlQuery<DetailReport>("exec USP_DetailReportForAdmin @FromDate,@ToDate,@IsOrderStatus,@IsProcessed,@IsSoApproved,@IsFullFillment,@IsInvoice,@CustomerId,@UserId",
                  new SqlParameter("@FromDate", FromDate),
                  new SqlParameter("@ToDate", ToDate),
                  new SqlParameter("@IsOrderStatus", IsOrderStatus),
                  new SqlParameter("@IsProcessed", IsProcessed),
                  new SqlParameter("@IsSoApproved", IsSoApproved),
                  new SqlParameter("@IsFullfillment", IsFullfillment),
                  new SqlParameter("@IsInvoice", IsInvoice),
                  new SqlParameter("@CustomerId", CustomerId),
                  new SqlParameter("@UserId", UserId)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<FinalReport> GetFinalReport(DateTime FromDate, DateTime ToDate,/* bool IsProcessed, bool IsOrderStatus, bool IsSoApproved, bool IsFullFillment, bool IsInvoice,*/ int CustId, int UserId)
        {
            try
            {

                return _context.Database.SqlQuery<FinalReport>("exec USP_FinalReport @FromDate,@ToDate,@UserId,@CustId",
                  new SqlParameter("@FromDate", FromDate),
                      new SqlParameter("@ToDate", ToDate),
                    new SqlParameter("@UserId", UserId),
                new SqlParameter("@CustId", CustId)

                ).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<MTDCls> MTDCls(int UserId, int CustId, List<int> DCId = null)
        {
            try
            {

                return _context.Database.SqlQuery<MTDCls>(
                    string.Format("exec USP_MTDCountReport @UsersId={0}, @CustId={1}, @DcId='{2}'", UserId,
                  CustId, DCId != null && DCId.Count() > 0 && DCId[0] != 0 ? String.Join(",", DCId.ToArray()) : "")
                  ).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<ReportCount> reportCounts(DateTime FromDate, DateTime ToDate, int CustId, string Region, string State, string Dc, string Store, string Sku, int UserId)
        {
            try
            {
                return _context.Database.SqlQuery<ReportCount>("exec USP_OrderCountReport @FromDate,@ToDate,@CustId,@Region,@State,@Dc,@Store,@Sku,@UserId",
                  new SqlParameter("@FromDate", FromDate),
                  new SqlParameter("@ToDate", ToDate),
                  new SqlParameter("@CustId", CustId),
                  new SqlParameter("@Region", Region),
                  new SqlParameter("@State", State),
                 new SqlParameter("@Dc", Dc),
                 new SqlParameter("@Store", Store),
                 new SqlParameter("@Sku", Sku),
                 new SqlParameter("@UserId", UserId)
                  ).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<OFRLIFR> OFRReport(DateTime FromDate, DateTime ToDate, int CustId, string Region, string State, string Dc, string Store, string Sku, int UserId)
        {
            try
            {
                return _context.Database.SqlQuery<OFRLIFR>("exec USP_OFR_LIFRReport @FromDate,@ToDate,@CustId,@Region,@State,@Dc,@Store,@Sku,@UserId",
                  new SqlParameter("@FromDate", FromDate),
                  new SqlParameter("@ToDate", ToDate),
                  new SqlParameter("@CustId", CustId),
                  new SqlParameter("@Region", Region),
                  new SqlParameter("@State", State),
                 new SqlParameter("@Dc", Dc),
                 new SqlParameter("@Store", Store),
                 new SqlParameter("@Sku", Sku),
                 new SqlParameter("@UserId", UserId)
                  ).ToList();
                // return _context.Database.SqlQuery<OFRLIFR>("exec USP_OFR_LIFRReport").ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<string> GetYear()
        {
            try
            {
                return _context.Database.SqlQuery<string>("exec USP_GetYear").ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<string> GetMonth(string Year)
        {
            try
            {
                return _context.Database.SqlQuery<string>("exec USP_GetMonth @Year",
                    new SqlParameter("@Year", Year)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<int> GetWeek(string Year)
        {
            try
            {
                return _context.Database.SqlQuery<int>("exec USP_GetWeek @Year",
                    new SqlParameter("@Year", Year)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<int> GetWeekByQuarter(string Year, int Quarter)
        {
            try
            {
                return _context.Database.SqlQuery<int>("exec USP_GetWeekByQuarter @Year,@Quarter",
                    new SqlParameter("@Year", Year),
                    new SqlParameter("@Quarter", Quarter)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<int> GetWeekByMonth(string Year, int Month)
        {
            try
            {
                return _context.Database.SqlQuery<int>("exec [USP_GetWeekByMonth] @Year,@Month",
                    new SqlParameter("@Year", Year),
                    new SqlParameter("@Month", Month)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<Week> GetDays(string Year, int Month)
        {
            try
            {
                return _context.Database.SqlQuery<Week>("exec [USP_WeekFirstDayLastDay] @Year,@Month",
                    new SqlParameter("@Year", Year),
                    new SqlParameter("@Month", Month)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<ReportCount> Revisedreport(DateTime FromDate, DateTime ToDate, int CustId, string Region, string State, string Dc, string Store, string Sku, int userid)
        {
            try
            {
                return _context.Database.SqlQuery<ReportCount>("exec USP_RevisedOrder @FromDate,@ToDate,@CustId,@Region,@State,@Dc,@Store,@Sku,@UserId",//
                  new SqlParameter("@FromDate", FromDate),
                  new SqlParameter("@ToDate", ToDate),
                  new SqlParameter("@CustId", CustId),
                  new SqlParameter("@Region", Region),
                  new SqlParameter("@State", State),
                  new SqlParameter("@Dc", Dc),
                 new SqlParameter("@Store", Store),
                 new SqlParameter("@Sku", Sku),
                 new SqlParameter("@UserId", userid)
                  ).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public OFRLIFR OFRLIFR_MTD(int Userid, int Custid, List<int> DCId = null)
        {
            try
            {
                return _context.Database.SqlQuery<OFRLIFR>(string.Format("exec USP_MTD_LIFR_OFR @UserId={0},@CustId={1},@DCId='{2}'",
                    Userid, Custid, DCId != null && DCId.Count() > 0 && DCId[0] != 0 ? String.Join(",", DCId.ToArray()) : "")
                    ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public int GetOrdersStoresToBePlaced(int custid, int userid)
        {
            try
            {
                return _context.Database.SqlQuery<int>("exec USP_GetStoresOrdersToBePlaced @UserId,@CustId",
               new SqlParameter("@UserId", userid),
               new SqlParameter("@CustId", custid)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<UniqueRefenenceReport> UniqueRefenenceReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items)
        {
            try
            {
                _context.Database.CommandTimeout = 0;
                return _context.Database.SqlQuery<UniqueRefenenceReport>(
                    string.Format("exec USP_UniqueRefenenceReport @FromDate='{0}', @ToDate='{1}',@UserId='{2}', @custId='{3}', @UniqueReferenceID='{4}', @dcid='{5}', @storeid='{6}', @itemid='{7}'", startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), userid, custid, uniqueReferenceID != null ? uniqueReferenceID : "", dCs != null ? string.Join(",", dCs.ToArray()) : "", stores != null ? string.Join(",", stores.ToArray()) : "", items != null ? string.Join(",", items.ToArray()) : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
                //return new List<UniqueRefenenceReport>();
            }
        }
        public List<DailySalesReport> DailySalesReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, List<int> selectedLocations, List<int> SelectedTypes, List<int> SelectedCategories)
        {
            try
            {
                return _context.Database.SqlQuery<DailySalesReport>(
                    string.Format(@"exec USP_Daily_Sales_Report @FromDate='{0}', @ToDate='{1}',@UserId='{2}', @CustId='{3}', @UniqueReferenceID='{4}', @dcid='{5}', @storeid='{6}', @itemid='{7}', @LocationId='{8}',@ItemCategory='{9}',@ItemType='{10}'",
                    startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    userid,
                    custid,
                    uniqueReferenceID != null ? uniqueReferenceID : "",
                    dCs != null ? string.Join(",", dCs.ToArray()) : "",
                    stores != null ? string.Join(",", stores.ToArray()) : "",
                    items != null ? string.Join(",", items.ToArray()) : "",
                    selectedLocations != null ? string.Join(",", selectedLocations.ToArray()) : "",
                    SelectedCategories != null ? string.Join(",", SelectedCategories.ToArray()) : "",
                    SelectedTypes != null ? string.Join(",", SelectedTypes.ToArray()) : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<Report2> Report2(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, int ReportType, List<int> SelectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null)
        {
            try
            {
                _context.Database.CommandTimeout = 0;
                return _context.Database.SqlQuery<Report2>(
                    string.Format("exec USP_City_Wise_Fullfillment_Report @FromDate='{0}', @ToDate='{1}',@UserId='{2}', @CustId='{3}', @UniqueReferenceID='{4}', @dcid='{5}', @storeid='{6}', @itemid='{7}',@ReportType={8}, @LocationId='{9}',@ItemCategory='{10}',@ItemType='{11}'",
                    startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    userid,
                    custid,
                    uniqueReferenceID != null ? uniqueReferenceID : "",
                    dCs != null ? string.Join(",", dCs.ToArray()) : "",
                    stores != null ? string.Join(",", stores.ToArray()) : "",
                    items != null ? string.Join(",", items.ToArray()) : "", ReportType,
                    SelectedLocations != null ? string.Join(",", SelectedLocations.ToArray()) : "",
                    SelectedTypes != null ? string.Join(",", SelectedTypes.ToArray()) : "",
                    SelectedCategories != null ? string.Join(",", SelectedCategories.ToArray()) : "")
                    ).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        #region "Pendency Order Report By Deepa 11/8/2018"   
        public List<PendencyOrderReport> PendencyOrderReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, string ReportType, List<int> SelectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null)
        {
            try
            {

                string Str = string.Format("exec USP_OrderPendency_Report @StoreOrderFrom='{0}', @StoreOrderToDate='{1}',@StoreCode='{2}', @DCCode='{3}', @SKUCode='{4}', @UniqueRefNo='{5}', @ReportType='{6}', @LocationId='{7}',@ItemCategory='{8}',@ItemType='{9}'",
                    startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    stores != null ? string.Join(",", stores.ToArray()) : "",
                    dCs != null ? string.Join(",", dCs.ToArray()) : "",
                    items != null ? string.Join(",", items.ToArray()) : "",
                    uniqueReferenceID != null ? uniqueReferenceID : "",
                    ReportType,
                    SelectedLocations != null ? string.Join(",", SelectedLocations.ToArray()) : "",
                    SelectedCategories != null ? string.Join(",", SelectedCategories.ToArray()) : "",
                    SelectedTypes != null ? string.Join(",", SelectedTypes.ToArray()) : "");
                return _context.Database.SqlQuery<PendencyOrderReport>(Str).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        #endregion
        #region  "DeliveryStatus Report By Deepa 11/11/2019"
        public List<DeliveryStatusReport> DeliveryStatusReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, int ReportType, string InvoiceNo, List<int> SelectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null)
        {
            try
            {
                return _context.Database.SqlQuery<DeliveryStatusReport>(
                    string.Format("exec [USP_DeliveryStatusReport] @FromDate='{0}', @ToDate='{1}',@UserId='{2}', @CustId='{3}', @UniqueReferenceID='{4}', @dcid='{5}', @storeid='{6}', @itemid='{7}',@ReportType={8},@InvoiceNo='{9}', @LocationId='{10}',@ItemCategory='{11}',@ItemType='{12}'",
                    startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    userid,
                    custid,
                    uniqueReferenceID != null ? uniqueReferenceID : "",
                    dCs != null ? string.Join(",", dCs.ToArray()) : "",
                    stores != null ? string.Join(",", stores.ToArray()) : "",
                    items != null ? string.Join(",", items.ToArray()) : "",
                    ReportType,
                    InvoiceNo != null ? InvoiceNo : "",
                    SelectedLocations != null ? string.Join(",", SelectedLocations.ToArray()) : "",
                    SelectedCategories != null ? string.Join(",", SelectedCategories.ToArray()) : "",
                    SelectedTypes != null ? string.Join(",", SelectedTypes.ToArray()) : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        #endregion

        #region  "Ageing Report By Deepa 13/11/2019"
        public List<AgeingReport> AgeingReport(DateTime startDate, DateTime endDate, int custid, int userid, List<int> dCs, List<int> stores, List<int> items, int ReportType, List<int> SelectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null)
        {
            try
            {
                return _context.Database.SqlQuery<AgeingReport>(
                    string.Format("exec [USP_Ageing_Report] @StoreOrderFrom='{0}', @StoreOrderToDate='{1}',@UserId='{2}', @CustId='{3}', @DCCode='{4}', @StoreCode='{5}', @SKUCode='{6}',@ReportType={7},@LocationId='{8}',@ItemCategory='{9}',@ItemCategoryType='{10}'",
                    startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    userid,
                    custid,
                    dCs != null ?
                    string.Join(",", dCs.ToArray()) : "",
                    stores != null ? string.Join(",", stores.ToArray()) : "",
                    items != null ? string.Join(",", items.ToArray()) : "",
                    ReportType,
                    SelectedLocations != null ? string.Join(",", SelectedLocations.ToArray()) : "",
                    SelectedCategories != null ? string.Join(",", SelectedCategories.ToArray()) : "",
                    SelectedTypes != null ? string.Join(",", SelectedTypes.ToArray()) : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<AgeingDetailReport> AgeingDetailReport(DateTime startDate, DateTime endDate, int custid, int userid, List<string> dCs, string stores, List<string> items, int PDayFrom, int PdayTo, int ReportType)
        {
            try
            {
                return _context.Database.SqlQuery<AgeingDetailReport>(
                    string.Format("exec [USP_AgeingDetail_Report] @StoreOrderFrom='{0}', @StoreOrderToDate='{1}',@UserId='{2}', @CustId='{3}', @DCCode='{4}', @StoreCode='{5}', @SKUCode='{6}',@PDayFrom='{7}', @PDayTo='{8}', @ReportType='{9}'", startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), userid, custid, dCs != null ? string.Join(",", dCs.ToArray()) : "", stores, items != null ? string.Join(",", items.ToArray()) : "", PDayFrom, PdayTo, ReportType)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        #endregion

        #region "Daily Order Report 18/11/2019"
        //Added by Deepa 18/11/2019
        public List<DailyOrderReport> DailyOrderReport(DateTime startDate, DateTime endDate, int custid, int userid, List<int> stores, List<int> items)
        {
            try
            {
                return _context.Database.SqlQuery<DailyOrderReport>(
                    string.Format("exec [USP_Daily_Order_Data_Report2] @FromDate='{0}', @ToDate='{1}',@UserId='{2}', @CustId='{3}', @storeid='{4}', @itemid='{5}'", startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), userid, custid, stores != null ? string.Join(",", stores.ToArray()) : "", items != null ? string.Join(",", items.ToArray()) : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
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
                  new SqlParameter("@uniqueReferenceID", uniqueReferenceID != null ? uniqueReferenceID : ""),
                  new SqlParameter("@InvoiceNo", InvoiceNo != null ? InvoiceNo : ""),
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
                  new SqlParameter("@uniqueReferenceID", uniqueReferenceID != null ? uniqueReferenceID : ""),
                  new SqlParameter("@InvoiceNo", InvoiceNo != null ? InvoiceNo : ""),
                  new SqlParameter("@Custid", custid)
                    ).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<PODImageList> GetPODImages(string uniqueReferenceID, string InvoiceNo, int custid)
        {
            try
            {
                return _context.Database.SqlQuery<PODImageList>("exec USP_GetPODImages  @uniqueReferenceID,@InvoiceNo,@Custid",
                  new SqlParameter("@uniqueReferenceID", uniqueReferenceID != null ? uniqueReferenceID : ""),
                  new SqlParameter("@InvoiceNo", InvoiceNo != null ? InvoiceNo : ""),
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

        #region StoreTop5SKUs
        public List<Top5SKU> GetTop5SKUs(int custid, int userid, int month, int year)
        {
            try
            {
                return _context.Database.SqlQuery<Top5SKU>(
                    string.Format("exec USP_GetTop5SKUs @UserId={0}, @CustId={1}, @Month='{2}', @Year={3}", userid, custid, month, year)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        #endregion

        #region StoreDayWiseOrderQty
        public List<DAYWISEORDERQTY> GetStoreDayWiseOrderQty(int custid, int userid, int month, int year, int storeid, List<int> sKUs)
        {
            try
            {
                return _context.Database.SqlQuery<DAYWISEORDERQTY>(
                    string.Format("exec USP_Store_SKU_Wise_Perfomance @UserId={0}, @CustId={1}, @Month={2}, @Year={3}, @StoreId={4}, @ItemId='{5}'", userid, custid, month, year, storeid, sKUs != null ? string.Join(",", sKUs.ToArray()) : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        #endregion

        #region FullfillmentReportDrillDown By Deepa 2019-12-09
        public List<Report2> FulfillmentReportDrillDown(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, int ReportType, string CityCategory, List<int> selectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null)
        {
            try
            {
                return _context.Database.SqlQuery<Report2>(
                    string.Format("exec [USP_City_Wise_fullfillment_DrillDown_report] @FromDate='{0}', @ToDate='{1}',@UserId='{2}', @CustId='{3}', @UniqueReferenceID='{4}', @dcid='{5}', @storeid='{6}', @itemid='{7}',@ReportType={8},@CityCategory='{9}', @LocationId='{10}',@ItemCategory='{11}',@ItemType='{12}'",
                    startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    userid,
                    custid,
                    uniqueReferenceID != null ? uniqueReferenceID : "",
                    dCs != null ? string.Join(",", dCs.ToArray()) : "",
                    stores != null ? string.Join(",", stores.ToArray()) : "",
                    items != null ? string.Join(",", items.ToArray()) : "",
                    ReportType,
                    CityCategory,
                    selectedLocations != null ? string.Join(",", selectedLocations.ToArray()) : "",
                    SelectedTypes != null ? string.Join(",", SelectedTypes.ToArray()) : "",
                    SelectedCategories != null ? string.Join(",", SelectedCategories.ToArray()) : ""
                    )).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        #endregion

        #region TOPSKUsForHALFYEAR BY Nikhil Kadam 2019/12/13
        public List<TOPSKUForYEARMonth> GetStoreTOPSKUS_HalfYear(int UserID, int StoreID, int CustID, int year, int type)
        {
            try
            {
                return _context.Database.SqlQuery<TOPSKUForYEARMonth>(string.Format("EXEC USP_Store_TOP_SKUS_HalfYear @StoreId='{0}',@UserId='{1}',@CustId={2}, @year={3},@half_year_type={4}", StoreID, UserID, CustID, year, type)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return null;
            }
        }
        #endregion

        #region OnTimeReport 20/02/2020 deepa
        public List<OnTimeReport> OnTimeReport(DateTime startDate, DateTime endDate, int custid, int userid, List<int> dCs, List<int> locationIds, int ReportType)
        {
            try
            {
                return _context.Database.SqlQuery<OnTimeReport>(
                    string.Format("exec [USP_OnTimeReport] @StoreOrderFrom='{0}', @StoreOrderToDate='{1}',@UserId='{2}', @CustId='{3}', @DCCode='{4}', @LocationIds='{5}', @ReportType={6}", startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), userid, custid, dCs != null ? string.Join(",", dCs.ToArray()) : "", locationIds != null ? string.Join(",", locationIds.ToArray()) : "", ReportType)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }

        }
        public List<OnTimeDetailReport> OnTimeReportDrillDown(DateTime startDate, DateTime endDate, int custid, int userid, int? dCs, int? locationIds, int ReportType)
        {
            try
            {
                return _context.Database.SqlQuery<OnTimeDetailReport>(
                    string.Format("exec [USP_OnTimeReportDrillDown] @StoreOrderFrom='{0}', @StoreOrderToDate='{1}',@UserId='{2}', @CustId='{3}', @DCCode='{4}', @LocationIds='{5}', @ReportType={6}", startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), userid, custid, dCs != null ? dCs : 0, locationIds != null ? locationIds : 0, ReportType)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }

        }
        #endregion

        #region OrderDeleteCloseReport 2020/05/26
        public List<OrderDeleteCloseReport> OrderDeleteCloseReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> selectedStores, List<int> selectedSKUs, string action)
        {
            try
            {
                return _context.Database.SqlQuery<OrderDeleteCloseReport>(
                    string.Format("exec USP_OrderDeleteCloseReport @StoreOrderFrom='{0}', @StoreOrderToDate='{1}',@UserId='{2}', @CustId='{3}', @storeid='{4}', @itemid='{5}',@UniqueRefNo='{6}', @Action='{7}'",
                    startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    userid,
                    custid,
                    selectedStores != null ? string.Join(",", selectedStores.ToArray()) : "",
                    selectedSKUs != null ? string.Join(",", selectedSKUs.ToArray()) : "",
                    uniqueReferenceID,
                    action)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        #endregion

        #region StoreMonthWise OrderDate OrderQty
        public List<MonthWiseStoreOrderQty> GetStoreMonthWiseOrderDateQty(int custid, int userid, int month, int year, int storeid, List<int> sKUs)
        {
            try
            {
                return _context.Database.SqlQuery<MonthWiseStoreOrderQty>(
                    string.Format("exec USP_Store_Date_Wise_OrderQty @UserId={0}, @CustId={1}, @Month={2}, @Year={3}, @StoreId={4}, @ItemId='{5}'", userid, custid, month, year, storeid, sKUs != null ? string.Join(",", sKUs.ToArray()) : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        #endregion

        #region Closing Stock 17-09-2021
        public List<ClosingStock> ClosingStockReport(int custid, int userid, List<int> selectedStores, List<int> selectedSKUs)
        {
            try
            {
                return _context.Database.SqlQuery<ClosingStock>(
                    string.Format("exec USP_ClosingStock @UserId={0}, @CustId={1}, @StoreId='{2}', @ItemId='{3}'", userid, custid, selectedStores != null ? string.Join(",", selectedStores.ToArray()) : "", selectedSKUs != null ? string.Join(",", selectedSKUs.ToArray()) : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        #endregion
        #region Dashboard
        public List<MonthWiseTotalSalesValue> GetTotalSalesValue(int userId, int custId, List<int> SelectedDc, List<int> storeid, int year)
        {
            try
            {
                return _context.Database.SqlQuery<MonthWiseTotalSalesValue>(
                    string.Format("exec USP_GetTotalSalesValue @ReportType='{0}', @UserId={1}, @CustId={2}, @Year={3}, @StoreId='{4}', @DcId='{5}'", "CASES", userId, custId, year, storeid != null ? string.Join(",", storeid.ToArray()) : "", SelectedDc != null && SelectedDc.Count() > 0 && SelectedDc[0] != 0 ? String.Join(",", SelectedDc.ToArray()) : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }

        }
        public List<MonthWiseTotalSalesValue> GetTotalSalesAmount(int userId, int custId, List<int> SelectedDc, List<int> storeid, int year)
        {
            try
            {
                return _context.Database.SqlQuery<MonthWiseTotalSalesValue>(
                    string.Format("exec USP_GetTotalSalesValue @ReportType='{0}', @UserId={1}, @CustId={2}, @Year={3}, @StoreId='{4}', @DcId='{5}'", "Amount", userId, custId, year, storeid != null ? string.Join(",", storeid.ToArray()) : "", SelectedDc != null && SelectedDc.Count() > 0 && SelectedDc[0] != 0 ? String.Join(",", SelectedDc.ToArray()) : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }

        }
        public List<TopSKUForCustomer> GetTop10SKUByAmount(int userId, int custId, List<int> storeid, int year, List<int> SelectedDc)
        {
            try
            {
                return _context.Database.SqlQuery<TopSKUForCustomer>(
                    string.Format("exec USP_GetTopSKUForCustomers @ReportType='{0}', @UserId={1}, @CustId={2}, @Year={3}, @StoreId='{4}', @DcId='{5}'", "TOP10SKUAMT", userId, custId, year, storeid != null ? string.Join(",", storeid.ToArray()) : "", SelectedDc != null && SelectedDc.Count() > 0 && SelectedDc[0] != 0 ? String.Join(",", SelectedDc.ToArray()) : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }

        }
        public List<TopSKUForCustomer> GetTop10SKUByCase(int userId, int custId, List<int> storeid, int year, List<int> SelectedDc)
        {
            try
            {
                return _context.Database.SqlQuery<TopSKUForCustomer>(
                    string.Format("exec USP_GetTopSKUForCustomers @ReportType='{0}', @UserId={1}, @CustId={2}, @Year={3}, @StoreId='{4}', @DcId='{5}'", "TOP10SKUCASE", userId, custId, year, storeid != null ? string.Join(",", storeid.ToArray()) : "", SelectedDc != null && SelectedDc.Count() > 0 && SelectedDc[0] != 0 ? String.Join(",", SelectedDc.ToArray()) : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }

        }
        public List<MonthWiseFillRate> GetFillRateByMonth(int userId, int custId, List<int> storeid, int year, List<int> SelectedDc)
        {
            try
            {
                return _context.Database.SqlQuery<MonthWiseFillRate>(
                    string.Format("exec USP_GetFillRateByMonth @ReportType='{0}', @UserId={1}, @CustId={2}, @Year={3}, @StoreId='{4}', @DcId='{5}'", "FILLRATE", userId, custId, year, storeid != null ? string.Join(",", storeid.ToArray()) : "", SelectedDc != null && SelectedDc.Count() > 0 && SelectedDc[0] != 0 ? String.Join(",", SelectedDc.ToArray()) : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        #endregion

        #region ActiveStoreMasterReports
        public List<StoreModel> ActiveStoresReport(string Opr, int custId, int userId, List<int> selectedDc, List<int> selectedStores)
        {
            try
            {
                return _context.Database.SqlQuery<StoreModel>(
                    string.Format("exec USP_ActiveStoreMasterReport @Opr='{0}', @UserId={1}, @CustId={2}, @Dcs='{3}', @Stores='{4}'", Opr, userId, custId, selectedDc != null ? string.Join(",", selectedDc.ToArray()) : "", selectedStores != null ? string.Join(",", selectedStores.ToArray()) : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return null;
            }
        }
        #endregion
        #region InvoiceReport
        public List<InvoiceView> InvoiceReport(DateTime startDate, DateTime endDate, int custid, int userid, List<int> stores, List<int> items)
        {
            try
            {
                return _context.Database.SqlQuery<InvoiceView>(
                    string.Format("exec [USP_InvoiceReport] @FromDate='{0}', @ToDate='{1}',@UserId='{2}', @CustId='{3}', @storeid='{4}', @itemid='{5}'", startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), userid, custid, stores != null ? string.Join(",", stores.ToArray()) : "", items != null ? string.Join(",", items.ToArray()) : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }

        }
        #endregion
        #region BasicReport
        public List<DailySalesReport> DailySalesBasicReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, List<int> selectedLocations, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            try
            {
                return _context.Database.SqlQuery<DailySalesReport>(
                    string.Format(@"exec USP_Daily_Sales_Basic_Report @FromDate='{0}', @ToDate='{1}',@UserId='{2}', @CustId='{3}', @UniqueReferenceID='{4}', @dcid='{5}', @storeid='{6}', @itemid='{7}', @LocationId='{8}',@ItemCategory='{9}',@ItemType='{10}', @SelectedBrands='{11}'",
                    startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    userid,
                    custid,
                    uniqueReferenceID != null ? uniqueReferenceID : "",
                    dCs != null ? string.Join(",", dCs.ToArray()) : "",
                    stores != null ? string.Join(",", stores.ToArray()) : "",
                    items != null ? string.Join(",", items.ToArray()) : "",
                    selectedLocations != null ? string.Join(",", selectedLocations.ToArray()) : "",
                    SelectedCategories != null ? string.Join(",", SelectedCategories.ToArray()) : "",
                    SelectedTypes != null ? string.Join(",", SelectedTypes.ToArray()) : "",
                    SelectedBrands != null ? string.Join(",", SelectedBrands.ToArray()) : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<AgeingReport> AgeingReport2(DateTime startDate, DateTime endDate, int custid, int userid, List<int> dCs, List<int> stores, List<int> items, int ReportType, List<int> SelectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null)
        {
            try
            {
                return _context.Database.SqlQuery<AgeingReport>(
                    string.Format("exec [USP_Ageing_Report2] @StoreOrderFrom='{0}', @StoreOrderToDate='{1}',@UserId='{2}', @CustId='{3}', @DCCode='{4}', @StoreCode='{5}', @SKUCode='{6}',@ReportType={7},@LocationId='{8}',@ItemCategory='{9}',@ItemCategoryType='{10}'",
                    startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    userid,
                    custid,
                    dCs != null ?
                    string.Join(",", dCs.ToArray()) : "",
                    stores != null ? string.Join(",", stores.ToArray()) : "",
                    items != null ? string.Join(",", items.ToArray()) : "",
                    ReportType,
                    SelectedLocations != null ? string.Join(",", SelectedLocations.ToArray()) : "",
                    SelectedCategories != null ? string.Join(",", SelectedCategories.ToArray()) : "",
                    SelectedTypes != null ? string.Join(",", SelectedTypes.ToArray()) : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<DailySalesReport> RABasicReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> selectedDC, List<int> selectedLocations, List<int> selectedStores, List<int> selectedSKUs, List<int> selectedTypes, List<int> selectedCategories, List<int> selectedBrands)
        {
            try
            {
                return _context.Database.SqlQuery<DailySalesReport>(
                    string.Format(@"exec USP_RABasicReport @FromDate='{0}', @ToDate='{1}',@UserId='{2}', @CustId='{3}', @UniqueReferenceID='{4}', @dcid='{5}', @storeid='{6}', @itemid='{7}', @LocationId='{8}',@ItemCategory='{9}',@ItemType='{10}', @SelectedBrands='{11}'",
                    startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    userid,
                    custid,
                    uniqueReferenceID != null ? uniqueReferenceID : "",
                    selectedDC != null ? string.Join(",", selectedDC.ToArray()) : "",
                    selectedStores != null ? string.Join(",", selectedStores.ToArray()) : "",
                    selectedSKUs != null ? string.Join(",", selectedSKUs.ToArray()) : "",
                    selectedLocations != null ? string.Join(",", selectedLocations.ToArray()) : "",
                    selectedCategories != null ? string.Join(",", selectedCategories.ToArray()) : "",
                    selectedTypes != null ? string.Join(",", selectedTypes.ToArray()) : "",
                    selectedBrands != null ? string.Join(",", selectedBrands.ToArray()) : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return null;
            }
        }

        public List<DailyOrderReport> DailyOrderReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> selectedDC, List<int> selectedLocations, List<int> selectedStores, List<int> selectedSKUs, List<int> selectedTypes, List<int> selectedCategories, List<int> selectedBrands)
        {
            try
            {
                return _context.Database.SqlQuery<DailyOrderReport>(
                    string.Format(@"exec USP_Daily_Order_Data_Report @FromDate='{0}', @ToDate='{1}',@UserId='{2}', @CustId='{3}', @UniqueReferenceID='{4}', @dcid='{5}', @storeid='{6}', @itemid='{7}', @LocationId='{8}',@ItemCategory='{9}',@ItemType='{10}', @SelectedBrands='{11}'",
                    startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    userid,
                    custid,
                    uniqueReferenceID != null ? uniqueReferenceID : "",
                    selectedDC != null ? string.Join(",", selectedDC.ToArray()) : "",
                    selectedStores != null ? string.Join(",", selectedStores.ToArray()) : "",
                    selectedSKUs != null ? string.Join(",", selectedSKUs.ToArray()) : "",
                    selectedLocations != null ? string.Join(",", selectedLocations.ToArray()) : "",
                    selectedCategories != null ? string.Join(",", selectedCategories.ToArray()) : "",
                    selectedTypes != null ? string.Join(",", selectedTypes.ToArray()) : "",
                    selectedBrands != null ? string.Join(",", selectedBrands.ToArray()) : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return null;
            }
        }

        #endregion

        #region DCStoreWiseItemReport
        public List<DCStoreWiseItemReport> GetDCStoreWiseItemReport(string reportType, int custid, int userid, List<int> selectedDC, List<int> selectedStores, List<int> selectedSKUs, List<int> selectedTypes, List<int> selectedCategories, List<int> selectedBrands)
        {
            try
            {
                string query = string.Format(@"exec USP_DC_Store_Wise_Item_Report @UserId='{0}', @CustId='{1}',@ReportType='{2}', @DCId='{3}', @StoreId='{4}', @ItemId='{5}', @ItemCategory='{6}',@ItemType='{7}',@BrandId='{8}' ",
                          userid,
                          custid, reportType,
                          selectedDC != null ? string.Join(",", selectedDC.ToArray()) : "",
                          selectedStores != null ? string.Join(",", selectedStores.ToArray()) : "",
                          selectedSKUs != null ? string.Join(",", selectedSKUs.ToArray()) : "",
                          selectedCategories != null ? string.Join(",", selectedCategories.ToArray()) : "",
                          selectedTypes != null ? string.Join(",", selectedTypes.ToArray()) : "",
                          selectedBrands != null ? string.Join(",", selectedBrands.ToArray()) : "");
                return _context.Database.SqlQuery<DCStoreWiseItemReport>(query).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }
        #endregion
        #region ItemReport
        public List<ItemReport> GetItemReport(int custid, int userid, List<int> selectedDC, List<int> selectedStores, List<int> selectedSKUs, List<int> selectedTypes, List<int> selectedCategories, List<int> selectedBrands)
        {
            string query = string.Format(@"exec USP_Item_Report @UserId='{0}', @CustId='{1}', @DCId='{2}', @StoreId='{3}', @ItemId='{4}', @ItemType='{5}',@ItemCategory='{6}',@BrandId='{7}' ",
                      userid,
                      custid,
                      selectedDC != null ? string.Join(",", selectedDC.ToArray()) : "",
                      selectedStores != null ? string.Join(",", selectedStores.ToArray()) : "",
                      selectedSKUs != null ? string.Join(",", selectedSKUs.ToArray()) : "",
                      selectedTypes != null ? string.Join(",", selectedTypes.ToArray()) : "",
                      selectedCategories != null ? string.Join(",", selectedCategories.ToArray()) : "",
                      selectedBrands != null ? string.Join(",", selectedBrands.ToArray()) : "");
            return _context.Database.SqlQuery<ItemReport>(query).ToList();
        }
        public List<StoreModel> GetStoreReport(int custid, int userid, List<int> selectedDC, List<int> SelectedLocations, List<int> selectedStores)
        {
            string query = string.Format(@"exec USP_Store_Report @UserId='{0}', @CustId='{1}', @DCs='{2}',@Locations='{3}', @Stores='{4}'",
                      userid,
                      custid,
                      selectedDC != null ? string.Join(",", selectedDC.ToArray()) : "",
                      SelectedLocations != null ? string.Join(",", SelectedLocations.ToArray()) : "",
                      selectedStores != null ? string.Join(",", selectedStores.ToArray()) : "");
            return _context.Database.SqlQuery<StoreModel>(query).ToList();
        }
        #endregion
        public List<PFAReport> PFAReport(int custId, List<int> selectedDC, List<int> SelectedStores)
        {
            //try
            //{
            //    // Convert lists to comma-separated strings for SQL
            //    var dcs = selectedDc != null ? string.Join(",", selectedDc) : "";
            //    var stores = selectedStores != null ? string.Join(",", selectedStores) : "";
            //    var skus = SelectedSKUs != null ? string.Join(",", SelectedSKUs) : "";
            //    var types = SelectedTypes != null ? string.Join(",", SelectedTypes) : "";
            //    var categories = SelectedCategories != null ? string.Join(",", SelectedCategories) : "";
            //    var brands = SelectedBrands != null ? string.Join(",", SelectedBrands) : "";

            //    // Define parameters for the stored procedure
            //    var userIdParam = new SqlParameter("@UserId", userId);
            //    var custIdParam = new SqlParameter("@CustId", custId);
            //    var dcsParam = new SqlParameter("@Dcs", dcs);
            //    var storesParam = new SqlParameter("@Stores", stores);
            //    var skusParam = new SqlParameter("@SKUs", skus);
            //    var typesParam = new SqlParameter("@Types", types);
            //    var categoriesParam = new SqlParameter("@Categories", categories);
            //    var brandsParam = new SqlParameter("@Brands", brands);

            //    // Execute the stored procedure and get results
            //    var results = _context.Database.SqlQuery<PFAReport>(
            //        "exec USP_GetPFADetails @UserId, @CustId, @Dcs, @Stores, @SKUs, @Types, @Categories, @Brands",
            //        userIdParam, custIdParam, dcsParam, storesParam, skusParam, typesParam, categoriesParam, brandsParam
            //    ).ToList();

            //    return results;
            //}
            //catch (Exception ex)
            //{
            //    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
            //    return null;
            //}
            try
            {
                string sqlQuery = string.Format(@"exec USP_GetPFADetails @CustId='{0}', @selectedDc='{1}',@selectedStores='{2}'", custId,
                    selectedDC != null ? string.Join(",", selectedDC.ToArray()) : "",
                    SelectedStores != null ? string.Join(",", SelectedStores.ToArray()) : "");

                return _context.Database.SqlQuery<PFAReport>(sqlQuery).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return null;
            }

        }
        public List<DailySalesReport> DailySalesBasicAdminReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, List<int> selectedLocations, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            try
            {
                return _context.Database.SqlQuery<DailySalesReport>(
                    string.Format(@"exec USP_Daily_Sales_Basic_Report1 @FromDate='{0}', @ToDate='{1}',@UserId='{2}', @CustId='{3}', @UniqueReferenceID='{4}', @dcid='{5}', @storeid='{6}', @itemid='{7}', @LocationId='{8}',@ItemCategory='{9}',@ItemType='{10}', @SelectedBrands='{11}'",
                    startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    userid,
                    custid,
                    uniqueReferenceID != null ? uniqueReferenceID : "",
                    dCs != null ? string.Join(",", dCs.ToArray()) : "",
                    stores != null ? string.Join(",", stores.ToArray()) : "",
                    items != null ? string.Join(",", items.ToArray()) : "",
                    selectedLocations != null ? string.Join(",", selectedLocations.ToArray()) : "",
                    SelectedCategories != null ? string.Join(",", SelectedCategories.ToArray()) : "",
                    SelectedTypes != null ? string.Join(",", SelectedTypes.ToArray()) : "",
                    SelectedBrands != null ? string.Join(",", SelectedBrands.ToArray()) : "")).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
    }
}
