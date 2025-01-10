using BAL;
using DAL;
using DbLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbLayer.Service
{
    public interface IReportService
    {
        List<OrderHeader> GetFinilizeOrder(int custid);
        List<OrderHeader> GetProcessedOrder(int custid);
        List<OrderHeader> GetSaveAsDraftOrders();
        List<OrderDetail> GetOrderDetails(long orderheaderid);
        Item GetItemById(int itemId);
        List<DetailReport> DetailReport(DateTime FromDate, DateTime ToDate, bool IsProcessed, bool IsOrderStatus);
        List<DetailReport> DetailReportByStore(DateTime FromDate, DateTime ToDate, bool IsProcessed, bool IsOrderStatus, bool IsSoApproved, bool IsFullFillment, bool IsInvoice, int StoreId);
        List<Top5Store> Top5Stores(DateTime FromDate, DateTime ToDate, int custid, int userid);
        List<DetailReport> DetailFinilizeReport(DateTime FromDate, DateTime ToDate, bool IsProcessed, bool IsOrderStatus, bool IsSoApprove, bool IsFullFillment, bool IsInvoice, int CustId, int UserId);
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
        List<FinalReport> GetFinalReport(DateTime FromDate, DateTime ToDate/*, bool IsProcessed, bool IsOrderStatus, bool IsSoApproved, bool IsFullFillment, bool IsInvoice,*/, int CustId, int UserId);
        List<MTDCls> MTDCls(int UserId, int CustId, List<int> DcId = null);
        List<ReportCount> reportCounts(DateTime FromDate, DateTime ToDate, int CustId, string Region, string State, string Dc, string Store, string Sku, int UserId);
        List<OFRLIFR> OFRReport(DateTime FromDate, DateTime ToDate, int CustId, string Region, string State, string Dc, string Store, string Sku, int UserId);
        List<string> GetYear();
        List<string> GetMonth(string Year);
        List<int> GetWeek(string Year);
        List<int> GetWeekByQuarter(string Year, int Quarter);
        List<int> GetWeekByMonth(string Year, int Month);
        List<Week> GetDays(string Year, int Month);
        List<ReportCount> Revisedreport(DateTime FromDate, DateTime ToDate, int CustId, string Region, string State, string Dc, string Store, string Sku, int userid);
        OFRLIFR OFRLIFR_MTD(int Userid, int Custid, List<int> DCId = null);
        int GetOrdersStoresToBePlaced(int custid, int userid);
        List<UniqueRefenenceReport> UniqueRefenenceReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> DCs, List<int> Stores, List<int> Items);
        List<DailySalesReport> DailySalesReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> DCs, List<int> Stores, List<int> Items, List<int> selectedLocations, List<int> SelectedTypes = null, List<int> SelectedCategories = null);
        List<DailySalesReport> DailySalesBasicReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> DCs, List<int> Stores, List<int> Items, List<int> selectedLocations, List<int> SelectedTypes = null, List<int> SelectedCategories = null, List<int> selectedBrands = null);
        List<Report2> Report2(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> DCs, List<int> Stores, List<int> Items, int ReportType, List<int> SelectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null);
        // Added by Deepa 11/11/2019
        List<PendencyOrderReport> PendencyOrderReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> selectedDC, List<int> selectedStores, List<int> selectedSKUs, string reportType, List<int> SelectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null);
        List<DeliveryStatusReport> DeliveryStatusReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> selectedDC, List<int> selectedStores, List<int> selectedSKUs, int reportType, string InvoiceNo, List<int> SelectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null);
        // Added by Deepa 11/13/2019
        List<AgeingReport> AgeingReport(DateTime startDate, DateTime endDate, int custid, int userid, List<int> dCs, List<int> stores, List<int> items, int ReportType, List<int> SelectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null);
        List<AgeingReport> AgeingReport2(DateTime startDate, DateTime endDate, int custid, int userid, List<int> dCs, List<int> stores, List<int> items, int ReportType, List<int> SelectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null);
        // Added By Deepa 11/14/2019
        List<AgeingDetailReport> AgeingDetailReport(DateTime startDate, DateTime endDate, int custid, int userid, List<string> dCs, string stores, List<string> items, int PDayFrom, int PdayTo, int ReportType);
        List<DailyOrderReport> DailyOrderReport(DateTime startDate, DateTime endDate, int custid, int userid, List<int> selectedStores, List<int> selectedSKUs);
        List<TrackingBO> GetTrackingList(string action, string uniqueReferenceID, string invoiceNo, int custid);
        List<TrackingBO> GetStatusList(string uniqueReferenceID, string invoiceNo, int custid);
        List<Top5SKU> GetTop5SKUs(int custid, int userid, int month, int year);
        List<DAYWISEORDERQTY> GetStoreDayWiseOrderQty(int custid, int userid, int month, int year, int storeid, List<int> sKUs);
        // Added by Deepa 09/12/2019
        List<Report2> FulfillmentReportDrillDown(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> DCs, List<int> Stores, List<int> Items, int ReportType, string CityCategory, List<int> selectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null);
        //Added By Nikhil Kadam 2019/12/13
        List<TOPSKUForYEARMonth> GetStoreTOPSKUS_HalfYear(int UserID, int StoreID, int CustID, int year, int type);
        //Added By Rekha 2020/01/02
        List<PODImageList> GetPODImages(string InvoiceNo, string invoiceNo, int custid);
        // OnTime Report 20/02/2020
        List<OnTimeReport> OnTimeReport(DateTime startDate, DateTime endDate, int custid, int userid, List<int> dCs, List<int> locationIds, int ReportType);
        List<OnTimeDetailReport> OnTimeReportDrillDown(DateTime startDate, DateTime endDate, int custid, int userid, int? dCs, int? locationIds, int ReportType);
        List<OrderDeleteCloseReport> OrderDeleteCloseReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> selectedStores, List<int> selectedSKUs, string action);
        //Store Home Widget MonthWiseOrderDateQTY By Nikhil 26-06-2021
        List<MonthWiseStoreOrderQty> GetStoreMonthWiseOrderDateQty(int custid, int userid, int month, int year, int storeid, List<int> sKUs);
        //
        List<ClosingStock> ClosingStockReport(int custid, int userid, List<int> selectedStores, List<int> selectedSKUs);
        //Dashboards
        List<MonthWiseTotalSalesValue> GetTotalSalesValue(int userId, int custId, List<int> SelectedDc, List<int> storeid, int year);
        List<MonthWiseTotalSalesValue> GetTotalSalesAmount(int userId, int custId, List<int> SelectedDc, List<int> storeid, int year);
        List<TopSKUForCustomer> GetTop10SKUByAmount(int userId, int custId, List<int> selectedStores, int year, List<int> SelectedDc);
        List<TopSKUForCustomer> GetTop10SKUByCase(int userId, int custId, List<int> storeid, int year, List<int> SelectedDc);
        List<MonthWiseFillRate> GetFillRateByMonth(int userId, int custId, List<int> selectedStores, int year, List<int> SelectedDc);
        List<StoreModel> ActiveStoresReport(string Opr, int custId, int userId, List<int> selectedDc, List<int> selectedStores);
        List<InvoiceView> InvoiceReport(DateTime startDate, DateTime endDate, int custid, int userid, List<int> selectedStores, List<int> selectedSKUs);
        List<DailySalesReport> RABasicReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> selectedDC, List<int> selectedStores, List<int> selectedSKUs, List<int> selectedLocations, List<int> selectedTypes, List<int> selectedCategories, List<int> selectedBrands);
        List<DailyOrderReport> DailyOrderReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> selectedDC, List<int> selectedStores, List<int> selectedSKUs, List<int> selectedLocations, List<int> selectedTypes, List<int> selectedCategories, List<int> selectedBrands);
        List<DCStoreWiseItemReport> GetDCStoreWiseItemReport(string reportType, int custid, int userid, List<int> selectedDC, List<int> selectedStores, List<int> selectedSKUs, List<int> selectedTypes, List<int> selectedCategories, List<int> selectedBrands);
        List<ItemReport> GetItemReport(int custid, int userid, List<int> selectedDC, List<int> selectedStores, List<int> selectedSKUs, List<int> selectedTypes, List<int> selectedCategories, List<int> selectedBrands);
        List<StoreModel> StoreWiseItemReport(string reportType, int custid, int userid, List<int> selectedDC, List<int> selectedLocations, List<int> selectedStores);
        List<PFAReport> PFAReport(int custId, List<int> selectedDC, List<int> selectedStores);
        List<DailySalesReport> DailySalesBasicAdminReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> DCs, List<int> Stores, List<int> Items, List<int> selectedLocations, List<int> SelectedTypes = null, List<int> SelectedCategories = null, List<int> selectedBrands = null);

    }
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        public List<OrderHeader> GetFinilizeOrder(int custid)
        {
            try
            {
                var orderheaders = _reportRepository.GetFinilizeOrder(custid);
                //foreach (var orderheader in orderheaders)
                //{
                //    orderheader.Store = _reportRepository.GetStoreDetailsbyId(orderheader.StoreId);
                //}
                return orderheaders;
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return null;
            }
        }

        public Item GetItemById(int itemId)
        {
            return _reportRepository.GetItemById(itemId);
        }

        public List<OrderHeader> GetProcessedOrder(int custid)
        {
            return _reportRepository.GetProcessedOrder(custid);
        }
        public List<OrderHeader> GetSaveAsDraftOrders()
        {
            try
            {
                var orderheaders = _reportRepository.GetSaveAsDraftOrders();
                foreach (var orderheader in orderheaders)
                {
                    orderheader.Store = _reportRepository.GetStoreDetailsbyId(orderheader.StoreId);
                }
                return orderheaders;
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return null;
            }
        }
        public List<OrderDetail> GetOrderDetails(long orderheaderid)
        {
            return _reportRepository.GetOrderDetailsForOrderHeaderId(orderheaderid);
        }

        public List<DetailReport> DetailReport(DateTime FromDate, DateTime ToDate, bool IsProcessed, bool IsOrderStatus)
        {
            return _reportRepository.DetailReport(FromDate, ToDate, IsProcessed, IsOrderStatus);
        }

        public List<DetailReport> DetailReportByStore(DateTime FromDate, DateTime ToDate, bool IsProcessed, bool IsOrderStatus, bool IsSoApproved, bool IsFullFillment, bool IsInvoice, int StoreId)
        {
            return _reportRepository.DetailReportByStore(FromDate, ToDate, IsProcessed, IsOrderStatus, IsSoApproved, IsFullFillment, IsInvoice, StoreId);
        }

        public List<Top5Store> Top5Stores(DateTime FromDate, DateTime ToDate, int custid, int userid)
        {
            return _reportRepository.Top5Stores(FromDate, ToDate, custid, userid);
        }

        public List<DetailReport> DetailFinilizeReport(DateTime FromDate, DateTime ToDate, bool IsProcessed, bool IsOrderStatus, bool IsSoApprove, bool IsFullFillment, bool IsInvoice, int CustId, int UserId)
        {
            return _reportRepository.DetailFinilizeReport(FromDate, ToDate, IsProcessed, IsOrderStatus, IsSoApprove, IsFullFillment, IsInvoice, CustId, UserId);
        }

        public List<DetailReport> SummeriseReportDcAdmin(DateTime FromDate, DateTime ToDate, bool IsProcessed, bool IsOrderStatus, bool IsSoApprove, bool IsFullFillment, bool IsInvoice, int CustId, int UserId)
        {
            return _reportRepository.SummeriseReportDcAdmin(FromDate, ToDate, IsProcessed, IsOrderStatus, IsSoApprove, IsFullFillment, IsInvoice, CustId, UserId);
        }

        public List<DetailReport> ProcessedData(bool IsProcessed, bool IsOrderStatus, bool IsSoApproved, bool IsFullfillment, bool IsInvoice, int CustId, int UserId)
        {
            return _reportRepository.ProcessedData(IsProcessed, IsOrderStatus, IsSoApproved, IsFullfillment, IsInvoice, CustId, UserId);
        }

        public List<DetailReport> SoApprovedReport(DateTime FromDate, DateTime ToDate, int customerid, int userid)
        {
            return _reportRepository.SoApprovedReport(FromDate, ToDate, customerid, userid);
        }

        public List<DetailReport> SoApprovedReportByStore(DateTime FromDate, DateTime ToDate, int StoreId)
        {
            return _reportRepository.SoApprovedReportByStore(FromDate, ToDate, StoreId);
        }

        public List<DetailReport> SoApprovedDetailReport(DateTime FromDate, DateTime ToDate, int customerid, int userid)
        {
            return _reportRepository.SoApprovedDetailReport(FromDate, ToDate, customerid, userid);
        }

        public List<DetailReport> SoApprovedDetailReportByStore(DateTime FromDate, DateTime ToDate, int storeid)
        {
            return _reportRepository.SoApprovedDetailReportByStore(FromDate, ToDate, storeid);
        }

        public List<DetailReport> SoApproved(int customerid, int userid)
        {
            return _reportRepository.SoApproved(customerid, userid);
        }

        public List<DetailReport> NetsuiteReportsByStore(DateTime FromDate, DateTime ToDate, bool IsOrderStatus, bool IsProcessed, bool IsSoApproved, bool IsFullfillment, bool IsInvoice, int StoreId)
        {
            return _reportRepository.NetsuiteReportsByStore(FromDate, ToDate, IsOrderStatus, IsProcessed, IsSoApproved, IsFullfillment, IsInvoice, StoreId);
        }

        public List<DetailReport> NetSuiteItemReport(string SoNumber)
        {
            return _reportRepository.NetSuiteItemReport(SoNumber);
        }

        public List<DetailReport> NetsuiteDetailReportsByStore(DateTime FromDate, DateTime ToDate, bool IsOrderStatus, bool IsProcessed, bool IsSoApproved, bool IsFullfillment, bool IsInvoice, int StoreId)
        {
            return _reportRepository.NetsuiteDetailReportsByStore(FromDate, ToDate, IsOrderStatus, IsProcessed, IsSoApproved, IsFullfillment, IsInvoice, StoreId);
        }

        public List<DetailReport> NetSuiteReportByAdmin(DateTime FromDate, DateTime ToDate, bool IsOrderStatus, bool IsProcessed, bool IsSoApproved, bool IsFullfillment, bool IsInvoice, int CustomerId, int UserId)
        {
            return _reportRepository.NetSuiteReportByAdmin(FromDate, ToDate, IsOrderStatus, IsProcessed, IsSoApproved, IsFullfillment, IsInvoice, CustomerId, UserId);
        }

        public List<DetailReport> NetSuiteDetailReportByAdmin(DateTime FromDate, DateTime ToDate, bool IsOrderStatus, bool IsProcessed, bool IsSoApproved, bool IsFullfillment, bool IsInvoice, int CustomerId, int UserId)
        {
            return _reportRepository.NetSuiteDetailReportByAdmin(FromDate, ToDate, IsOrderStatus, IsProcessed, IsSoApproved, IsFullfillment, IsInvoice, CustomerId, UserId);
        }

        public List<FinalReport> GetFinalReport(DateTime FromDate, DateTime ToDate, /*bool IsProcessed, bool IsOrderStatus, bool IsSoApproved, bool IsFullFillment, bool IsInvoice,*/ int CustId, int UserId)
        {
            return _reportRepository.GetFinalReport(FromDate, ToDate,/* IsProcessed, IsOrderStatus, IsSoApproved, IsFullFillment, IsInvoice,*/ CustId, UserId);
        }

        public List<MTDCls> MTDCls(int UserId, int CustId, List<int> DCId = null)
        {
            return _reportRepository.MTDCls(UserId, CustId, DCId);
        }

        public List<ReportCount> reportCounts(DateTime FromDate, DateTime ToDate, int CustId, string Region, string State, string Dc, string Store, string Sku, int UserId)
        {
            return _reportRepository.reportCounts(FromDate, ToDate, CustId, Region, State, Dc, Store, Sku, UserId);
        }

        public List<OFRLIFR> OFRReport(DateTime FromDate, DateTime ToDate, int CustId, string Region, string State, string Dc, string Store, string Sku, int UserId)
        {
            return _reportRepository.OFRReport(FromDate, ToDate, CustId, Region, State, Dc, Store, Sku, UserId);
        }


        public List<string> GetYear()
        {
            return _reportRepository.GetYear();
        }

        public List<string> GetMonth(string Year)
        {
            return _reportRepository.GetMonth(Year);
        }

        public List<int> GetWeek(string Year)
        {
            return _reportRepository.GetWeek(Year);
        }

        public List<int> GetWeekByQuarter(string Year, int Quarter)
        {
            return _reportRepository.GetWeekByQuarter(Year, Quarter);
        }

        public List<int> GetWeekByMonth(string Year, int Month)
        {
            return _reportRepository.GetWeekByMonth(Year, Month);
        }

        public List<Week> GetDays(string Year, int Month)
        {
            return _reportRepository.GetDays(Year, Month);
        }

        public List<ReportCount> Revisedreport(DateTime FromDate, DateTime ToDate, int CustId, string Region, string State, string Dc, string Store, string Sku, int userid)
        {
            return _reportRepository.Revisedreport(FromDate, ToDate, CustId, Region, State, Dc, Store, Sku, userid);
        }
        public OFRLIFR OFRLIFR_MTD(int Userid, int Custid, List<int> DCId = null)
        {
            return _reportRepository.OFRLIFR_MTD(Userid, Custid, DCId);
        }
        public int GetOrdersStoresToBePlaced(int custid, int userid)
        {
            return _reportRepository.GetOrdersStoresToBePlaced(custid, userid);
        }
        public List<UniqueRefenenceReport> UniqueRefenenceReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> DCs, List<int> Stores, List<int> Items)
        {
            return _reportRepository.UniqueRefenenceReport(startDate, endDate, custid, userid, uniqueReferenceID, DCs, Stores, Items);
        }
        public List<DailySalesReport> DailySalesReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> DCs, List<int> Stores, List<int> Items, List<int> selectedLocations, List<int> SelectedTypes, List<int> SelectedCategories)
        {
            return _reportRepository.DailySalesReport(startDate, endDate, custid, userid, uniqueReferenceID, DCs, Stores, Items, selectedLocations, SelectedTypes, SelectedCategories);
        }
        public List<Report2> Report2(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> selectedDCs, List<int> selectedStores, List<int> selectedSKUs, int ReportType, List<int> SelectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null)
        {
            return _reportRepository.Report2(startDate, endDate, custid, userid, uniqueReferenceID, selectedDCs, selectedStores, selectedSKUs, ReportType, SelectedLocations, SelectedTypes, SelectedCategories);
        }

        #region "Pendency Order Report By Deepa 11/8/2019"
        public List<PendencyOrderReport> PendencyOrderReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> DCs, List<int> Stores, List<int> Items, string ReportType, List<int> SelectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null)
        {
            return _reportRepository.PendencyOrderReport(startDate, endDate, custid, userid, uniqueReferenceID, DCs, Stores, Items, ReportType, SelectedLocations, SelectedTypes, SelectedCategories);
        }
        #endregion

        #region "Delivery Status Report By Deepa 11/11/2019"
        public List<DeliveryStatusReport> DeliveryStatusReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> selectedDCs, List<int> selectedStores, List<int> selectedSKUs, int ReportType, string InvoiceNo, List<int> SelectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null)
        {
            return _reportRepository.DeliveryStatusReport(startDate, endDate, custid, userid, uniqueReferenceID, selectedDCs, selectedStores, selectedSKUs, ReportType, InvoiceNo, SelectedLocations, SelectedTypes, SelectedCategories);
        }
        #endregion

        #region "Ageing Report by Deepa 11/13/2019
        public List<AgeingReport> AgeingReport(DateTime startDate, DateTime endDate, int custid, int userid, List<int> dCs, List<int> stores, List<int> items, int ReportType, List<int> SelectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null)
        {
            return _reportRepository.AgeingReport(startDate, endDate, custid, userid, dCs, stores, items, ReportType, SelectedLocations, SelectedTypes, SelectedCategories);
        }
        public List<AgeingReport> AgeingReport2(DateTime startDate, DateTime endDate, int custid, int userid, List<int> dCs, List<int> stores, List<int> items, int ReportType, List<int> SelectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null)
        {
            return _reportRepository.AgeingReport2(startDate, endDate, custid, userid, dCs, stores, items, ReportType, SelectedLocations, SelectedTypes, SelectedCategories);
        }
        // Added By Deepa 11/14/2019
        public List<AgeingDetailReport> AgeingDetailReport(DateTime startDate, DateTime endDate, int custid, int userid, List<string> dCs, string stores, List<string> items, int PDayFrom, int PdayTo, int ReportType)
        {
            return _reportRepository.AgeingDetailReport(startDate, endDate, custid, userid, dCs, stores, items, PDayFrom, PdayTo, ReportType);
        }
        #endregion

        #region "Daily order Report 18/11/2019"
        //Added by Deepa 18/11/2019
        public List<DailyOrderReport> DailyOrderReport(DateTime startDate, DateTime endDate, int custid, int userid, List<int> stores, List<int> items)
        {
            return _reportRepository.DailyOrderReport(startDate, endDate, custid, userid, stores, items);
        }
        #endregion


        #region Tracking
        public List<TrackingBO> GetTrackingList(string action, string uniqueReferenceID, string InvoiceNo, int custid)
        {
            return _reportRepository.GetTrackingList(action, uniqueReferenceID, InvoiceNo, custid);
        }
        public List<TrackingBO> GetStatusList(string uniqueReferenceID, string InvoiceNo, int custid)
        {
            return _reportRepository.GetStatusList(uniqueReferenceID, InvoiceNo, custid);
        }
        public List<PODImageList> GetPODImages(string uniqueReferenceID, string invoiceNo, int custid)
        {
            return _reportRepository.GetPODImages(uniqueReferenceID, invoiceNo, custid);
        }

        #endregion


        #region StoreTop5SKUs
        public List<Top5SKU> GetTop5SKUs(int custid, int userid, int month, int year)
        {
            return _reportRepository.GetTop5SKUs(custid, userid, month, year);
        }

        #endregion

        #region StoreDayWiseOrderQty
        public List<DAYWISEORDERQTY> GetStoreDayWiseOrderQty(int custid, int userid, int month, int year, int storeid, List<int> sKUs)
        {
            return _reportRepository.GetStoreDayWiseOrderQty(custid, userid, month, year, storeid, sKUs);
        }
        #endregion

        #region "Fulfilment Report Drilldown 06/12/2019"
        public List<Report2> FulfillmentReportDrillDown(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> selectedDCs, List<int> selectedStores, List<int> selectedSKUs, int ReportType, string CityCategory, List<int> selectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null)
        {
            return _reportRepository.FulfillmentReportDrillDown(startDate, endDate, custid, userid, uniqueReferenceID, selectedDCs, selectedStores, selectedSKUs, ReportType, CityCategory, selectedLocations, SelectedTypes, SelectedCategories);
        }
        #endregion

        #region TOPSKUsForHALFYEAR BY Nikhil Kadam 2019/12/13
        public List<TOPSKUForYEARMonth> GetStoreTOPSKUS_HalfYear(int UserID, int StoreID, int CustID, int year, int type)
        {
            return _reportRepository.GetStoreTOPSKUS_HalfYear(UserID, StoreID, CustID, year, type);
        }

        #endregion
        #region "OnTimeReport 20/02/20" deepa
        // Added By Deepa 20/02/2020
        public List<OnTimeReport> OnTimeReport(DateTime startDate, DateTime endDate, int custid, int userid, List<int> dCs, List<int> locationIds, int ReportType)
        {
            return _reportRepository.OnTimeReport(startDate, endDate, custid, userid, dCs, locationIds, ReportType);
        }
        public List<OnTimeDetailReport> OnTimeReportDrillDown(DateTime startDate, DateTime endDate, int custid, int userid, int? dCs, int? locationIds, int ReportType)
        {
            return _reportRepository.OnTimeReportDrillDown(startDate, endDate, custid, userid, dCs, locationIds, ReportType);
        }
        #endregion

        public List<OrderDeleteCloseReport> OrderDeleteCloseReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> selectedStores, List<int> selectedSKUs, string action)
        {
            return _reportRepository.OrderDeleteCloseReport(startDate, endDate, custid, userid, uniqueReferenceID, selectedStores, selectedSKUs, action);
        }

        #region
        public List<MonthWiseStoreOrderQty> GetStoreMonthWiseOrderDateQty(int custid, int userid, int month, int year, int storeid, List<int> sKUs)
        {
            return _reportRepository.GetStoreMonthWiseOrderDateQty(custid, userid, month, year, storeid, sKUs);
        }
        #endregion
        #region Closing Stock
        public List<ClosingStock> ClosingStockReport(int custid, int userid, List<int> selectedStores, List<int> selectedSKUs)
        {
            return _reportRepository.ClosingStockReport(custid, userid, selectedStores, selectedSKUs);
        }
        #endregion
        #region Dashboard
        public List<MonthWiseTotalSalesValue> GetTotalSalesValue(int userId, int custId, List<int> SelectedDc, List<int> storeid, int year)
        {
            return _reportRepository.GetTotalSalesValue(userId, custId, SelectedDc, storeid, year);
        }

        public List<MonthWiseTotalSalesValue> GetTotalSalesAmount(int userId, int custId, List<int> SelectedDc, List<int> storeid, int year)
        {
            return _reportRepository.GetTotalSalesAmount(userId, custId, SelectedDc, storeid, year);
        }
        public List<TopSKUForCustomer> GetTop10SKUByAmount(int userId, int custId, List<int> storeid, int year, List<int> SelectedDc)
        {
            return _reportRepository.GetTop10SKUByAmount(userId, custId, storeid, year, SelectedDc);
        }
        public List<TopSKUForCustomer> GetTop10SKUByCase(int userId, int custId, List<int> storeid, int year, List<int> SelectedDc)
        {
            return _reportRepository.GetTop10SKUByCase(userId, custId, storeid, year, SelectedDc);
        }
        public List<MonthWiseFillRate> GetFillRateByMonth(int userId, int custId, List<int> storeid, int year, List<int> SelectedDc)
        {
            return _reportRepository.GetFillRateByMonth(userId, custId, storeid, year, SelectedDc);
        }
        #endregion

        #region ActiveStoreMasterReports
        public List<StoreModel> ActiveStoresReport(string Opr, int custId, int userId, List<int> selectedDc, List<int> selectedStores)
        {
            return _reportRepository.ActiveStoresReport(Opr, custId, userId, selectedDc, selectedStores);
        }
        #endregion
        #region InvoiceReport
        public List<InvoiceView> InvoiceReport(DateTime startDate, DateTime endDate, int custid, int userid, List<int> selectedStores, List<int> selectedSKUs)
        {
            return _reportRepository.InvoiceReport(startDate, endDate, custid, userid, selectedStores, selectedSKUs);
        }
        #endregion

        #region BasicReport
        public List<DailySalesReport> DailySalesBasicReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> DCs, List<int> Stores, List<int> Items, List<int> selectedLocations, List<int> SelectedTypes, List<int> SelectedCategories, List<int> selectedBrands)
        {
            return _reportRepository.DailySalesBasicReport(startDate, endDate, custid, userid, uniqueReferenceID, DCs, Stores, Items, selectedLocations, SelectedTypes, SelectedCategories, selectedBrands);
        }

        #endregion

        public List<DailySalesReport> RABasicReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> selectedDC, List<int> selectedStores, List<int> selectedSKUs, List<int> selectedLocations, List<int> selectedTypes, List<int> selectedCategories, List<int> selectedBrands)
        {
            return _reportRepository.RABasicReport(startDate, endDate, custid, userid, uniqueReferenceID, selectedDC, selectedLocations, selectedStores, selectedSKUs, selectedTypes, selectedCategories, selectedBrands);
        }
        public List<DailyOrderReport> DailyOrderReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> selectedDC, List<int> selectedStores, List<int> selectedSKUs, List<int> selectedLocations, List<int> selectedTypes, List<int> selectedCategories, List<int> selectedBrands)
        {
            return _reportRepository.DailyOrderReport(startDate, endDate, custid, userid, uniqueReferenceID, selectedDC, selectedLocations, selectedStores, selectedSKUs, selectedTypes, selectedCategories, selectedBrands);
        }
        public List<DCStoreWiseItemReport> GetDCStoreWiseItemReport(string reportType, int custid, int userid, List<int> selectedDC, List<int> selectedStores, List<int> selectedSKUs, List<int> selectedTypes, List<int> selectedCategories, List<int> selectedBrands)
        {
            return _reportRepository.GetDCStoreWiseItemReport(reportType, custid, userid, selectedDC, selectedStores, selectedSKUs, selectedTypes, selectedCategories, selectedBrands);
        }
        public List<ItemReport> GetItemReport(int custid, int userid, List<int> selectedDC, List<int> selectedStores, List<int> selectedSKUs, List<int> selectedTypes, List<int> selectedCategories, List<int> selectedBrands)
        {
            return _reportRepository.GetItemReport(custid, userid, selectedDC, selectedStores, selectedSKUs, selectedTypes, selectedCategories, selectedBrands);
        }
        public List<StoreModel> StoreWiseItemReport(string reportType, int custid, int userid, List<int> selectedDC, List<int> selectedLocations, List<int> selectedStores)
        {
            return _reportRepository.GetStoreReport(custid, userid, selectedDC,selectedLocations, selectedStores);
        }
        public List<DailySalesReport> StoreWiseSkuReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> DCs, List<int> Stores, List<int> Items, List<int> selectedLocations, List<int> SelectedTypes, List<int> SelectedCategories, List<int> selectedBrands)
        {
            return _reportRepository.DailySalesBasicReport(startDate, endDate, custid, userid, uniqueReferenceID, DCs, Stores, Items, selectedLocations, SelectedTypes, SelectedCategories, selectedBrands);
        }
        public List<PFAReport> PFAReport(int custId, List<int> selectedDC,List<int> SelectedStores)
        {
            return _reportRepository.PFAReport(custId,selectedDC,SelectedStores);
        }
        #region BasicReport
        public List<DailySalesReport> DailySalesBasicAdminReport(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> DCs, List<int> Stores, List<int> Items, List<int> selectedLocations, List<int> SelectedTypes, List<int> SelectedCategories, List<int> selectedBrands)
        {
            return _reportRepository.DailySalesBasicAdminReport(startDate, endDate, custid, userid, uniqueReferenceID, DCs, Stores, Items, selectedLocations, SelectedTypes, SelectedCategories, selectedBrands);
        }

        #endregion
    }
}
