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
    public interface IReportChartsServices
    {
        // Added by Deepa 11/13/2019
        List<AgeingReportChart> AgeingReportChartsSearch(DateTime startDate, DateTime endDate, int custid, int userid, List<int> dCs, List<int> stores, List<int> items, int ReportType, List<int> City, List<int> ItemType, List<int> ItemSubType, string AgeCompareFrom, string AgeCompareTo, int CustomRangeFrom, int CustomRangeTO, string IsCustomRange, int OrderedQuantityFrom, int OrderedQuantityTo, string ByOrderDateOrInvoiceDate);
        List<DeliveryStatusReportChart> DeliveryStatusReportChartsSearch(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> selectedDCs, List<int> selectedStores, List<int> selectedSKUs, int ReportType);
        List<FillRateReportChart> FillRateReportCharts(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, int ReportType, string ByOrderDateOrInvoiceDate, List<int> City, List<int> State, string Invoice_Number, List<int> ItemType, List<int> item_sub_type, int OrderedQuantityFrom, int orderQuantityTO, string FulfillmentType);
        List<FillRateReportChart> FillRateReportChartsDrillDown(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, int ReportType, string ByOrderDateOrInvoiceDate, List<int> City, List<int> State, string Invoice_Number, List<int> ItemType, List<int> item_sub_type, int OrderedQuantityFrom, int orderQuantityTO, string DcName, string OuterChartData, string FulfillmentType);
    }
    public class ReportChartsServices:IReportChartsServices
    {
        private readonly IReportChartsRepository _reportChartsRepository;
        public ReportChartsServices(IReportChartsRepository reportChartsRepository)
        {
            _reportChartsRepository = reportChartsRepository;
        }

        #region "Ageing Report for chart by Deepa 18/12/2019
        public List<AgeingReportChart> AgeingReportChartsSearch(DateTime startDate, DateTime endDate, int custid, int userid, List<int> dCs, List<int> stores, List<int> items, int ReportType, List<int> City, List<int> ItemType, List<int> ItemSubType, string AgeCompareFrom, string AgeCompareTo, int CustomRangeFrom, int CustomRangeTO, string IsCustomRange, int OrderedQuantityFrom, int OrderedQuantityTo, string ByOrderDateOrInvoiceDate)
        {
            return _reportChartsRepository.AgeingReportChartsSearch(startDate, endDate, custid, userid, dCs, stores, items, ReportType,City,ItemType,ItemSubType, AgeCompareFrom, AgeCompareTo, CustomRangeFrom, CustomRangeTO, IsCustomRange, OrderedQuantityFrom, OrderedQuantityTo, ByOrderDateOrInvoiceDate);
        }
        #endregion

        #region FillRate Report Charts
        public List<FillRateReportChart> FillRateReportCharts(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, int ReportType, string ByOrderDateOrInvoiceDate, List<int> City, List<int> State, string Invoice_Number, List<int> ItemType, List<int> item_sub_type, int OrderedQuantityFrom, int orderQuantityTO, string FulfillmentType)
        {
            return _reportChartsRepository.FillRateReportChartsSearch(startDate, endDate, custid, userid, uniqueReferenceID, dCs, stores, items, ReportType, ByOrderDateOrInvoiceDate,City,State, Invoice_Number,ItemType,item_sub_type,OrderedQuantityFrom, orderQuantityTO,FulfillmentType);
        }
        public List<FillRateReportChart> FillRateReportChartsDrillDown(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, int ReportType, string ByOrderDateOrInvoiceDate, List<int> City, List<int> State, string Invoice_Number, List<int> ItemType, List<int> item_sub_type, int OrderedQuantityFrom, int orderQuantityTO, string DcName, string OuterChartData, string FulfillmentType)
        {
            return _reportChartsRepository.FillRateReportChartsDrillDown(startDate, endDate, custid, userid, uniqueReferenceID, dCs, stores, items, ReportType, ByOrderDateOrInvoiceDate, City, State, Invoice_Number, ItemType, item_sub_type, OrderedQuantityFrom, orderQuantityTO,DcName, OuterChartData,FulfillmentType);
        }
        #endregion


        #region "Delivery Status Report By Deepa 23/12/2019"
        public List<DeliveryStatusReportChart> DeliveryStatusReportChartsSearch(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> selectedDCs, List<int> selectedStores, List<int> selectedSKUs, int ReportType)
        {
            return _reportChartsRepository.DeliveryStatusReportChartsSearch(startDate, endDate, custid, userid, uniqueReferenceID, selectedDCs, selectedStores, selectedSKUs, ReportType);
        }
        #endregion

    }
}
