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
    public interface IReportChartsRepository
    {
        List<AgeingReportChart> AgeingReportChartsSearch(DateTime startDate, DateTime endDate, int custid, int userid, List<int> dCs, List<int> stores, List<int> items, int ReportType, List<int> City, List<int> ItemType, List<int> ItemSubType, string AgeCompareFrom, string AgeCompareTo, int CustomRangeFrom, int CustomRangeTO, string IsCustomRange, int OrderedQuantityFrom, int OrderedQuantityTo, string ByOrderDateOrInvoiceDate);
        List<FillRateReportChart> FillRateReportChartsSearch(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, int ReportType, string ByOrderDateOrInvoiceDate, List<int> City, List<int> State, string Invoice_Number, List<int> ItemType, List<int> item_sub_type, int OrderedQuantityFrom, int orderQuantityTO, string FulfillmentType);


        List<FillRateReportChart> FillRateReportChartsDrillDown(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, int ReportType, string ByOrderDateOrInvoiceDate, List<int> City, List<int> State, string Invoice_Number, List<int> ItemType, List<int> item_sub_type, int OrderedQuantityFrom, int orderQuantityTO, string DcName, string OuterChartData, string FulfillmentType);

        List<DeliveryStatusReportChart> DeliveryStatusReportChartsSearch(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, int ReportType);
    }

    public class ReportChartsRepository: IReportChartsRepository
    {
        private readonly StoreContext _context;
        public ReportChartsRepository(StoreContext context)
        {
            _context = context;
        }

        #region  "Ageing Report for charts By Deepa 18/12/2019"
        public List<AgeingReportChart> AgeingReportChartsSearch(DateTime startDate, DateTime endDate, int custid, int userid, List<int> dCs, List<int> stores, List<int> items, int ReportType,  List<int> City, List<int> ItemType, List<int> ItemSubType,
            string AgeCompareFrom, string AgeCompareTo, int CustomRangeFrom, int CustomRangeTO, string IsCustomRange, int OrderedQuantityFrom, int OrderedQuantityTo, string ByOrderDateOrInvoiceDate)
        {
            try
            {
                string Query = string.Format("exec [USP_Ageing_ReportChart] @StoreOrderFrom='{0}', @StoreOrderToDate='{1}',@UserId='{2}', @CustId='{3}', @DCCode='{4}'," +
                    "@StoreCode='{5}', @SKUCode='{6}',@ReportType={7}, @City='{8}', @ItemType='{9}', @Item_Sub_Type='{10}'" +
                    ",@AgeCompareFrom={11},@AgeCompareTo={12},@CustomRangeFrom={13},@CustomRangeTO={14},@IsCustomRange='{15}',@OrderedQuantityFrom={16},@OrderedQuantityTo={17},@ByOrderDateOrInvoiceDate='{18}'",
                    startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), userid, custid, dCs != null ? string.Join(",", dCs.ToArray()) : "",
                    stores != null ? string.Join(",", stores.ToArray()) : "", items != null ? string.Join(",", items.ToArray()) : "", ReportType,
                    City != null ? string.Join(",", City.ToArray()) : "", ItemType != null ? string.Join(",", ItemType.ToArray()) : "",
                    ItemSubType != null ? string.Join(",", ItemSubType.ToArray()) : "",
                    AgeCompareFrom,AgeCompareTo,CustomRangeFrom, CustomRangeTO,IsCustomRange,OrderedQuantityFrom,OrderedQuantityTo,ByOrderDateOrInvoiceDate
                    );
                return _context.Database.SqlQuery<AgeingReportChart>(Query).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        #endregion
        #region FillRate Report Charts By Deepa 07/01/2020
        public List<FillRateReportChart> FillRateReportChartsSearch(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, int ReportType, string ByOrderDateOrInvoiceDate, List<int> City, List<int> State, string Invoice_Number, List<int> ItemType, List<int> item_sub_type, int OrderedQuantityFrom, int orderQuantityTO, string FulfillmentType)
        {
            try
            {
                string Query = string.Format("exec [USP_FillRateReportChart] @FromDate='{0}', @ToDate='{1}',@UserId='{2}'," +
                                    "@CustId='{3}', @UniqueReferenceID='{4}', @dcid='{5}', @storeid='{6}', @itemid='{7}', " +
                                    "@ReportType='{8}',@ByOrderDateOrInvoiceDate='{9}',@City='{10}',@State='{11}',@Invoice_Number='{12}'," +
                                    "@itemType='{13}',@item_Sub_Type='{14}'," +
                                    "@OrderedQuantityFrom='{15}',@orderQuantityTO='{16}',@FulllfilmentType='{17}'"
                                    , startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), userid,
                                    custid, uniqueReferenceID != null ? uniqueReferenceID : "", dCs != null ? string.Join(",", dCs.ToArray()) : "", stores != null ? string.Join(",", stores.ToArray()) : "", items != null ? string.Join(",", items.ToArray()) : "",
                                    ReportType, ByOrderDateOrInvoiceDate == null ? "" : ByOrderDateOrInvoiceDate, City[0] > 0 ? string.Join(",", City.ToArray()) : "", State[0] > 0 ? "" : string.Join(",", State.ToArray()), Invoice_Number == null ? "" : Invoice_Number,
                                    ItemType[0] > 0 ? string.Join(",", ItemType.ToArray()) : "", item_sub_type[0] > 0 ? string.Join(",", item_sub_type.ToArray()) : "",
                                   OrderedQuantityFrom, orderQuantityTO, FulfillmentType
                                );
                return _context.Database.SqlQuery<FillRateReportChart>(Query).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return null;
            }
        }
        public List<FillRateReportChart> FillRateReportChartsDrillDown(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, int ReportType, string ByOrderDateOrInvoiceDate, List<int> City, List<int> State, string Invoice_Number, List<int> ItemType, List<int> item_sub_type, int OrderedQuantityFrom, int orderQuantityTO, string DcName, string OuterChartData, string FulfillmentType)
        {
            try
            {
                string Query = string.Format("exec [USP_FillRateReportChart_DrillDown] @FromDate='{0}', @ToDate='{1}',@UserId='{2}'," +
                                    "@CustId='{3}', @UniqueReferenceID='{4}', @dcid='{5}', @storeid='{6}', @itemid='{7}', " +
                                    "@ReportType='{8}',@ByOrderDateOrInvoiceDate='{9}',@City='{10}',@State='{11}',@Invoice_Number='{12}'," +
                                    "@itemType='{13}',@item_Sub_Type='{14}'," +
                                    "@OrderedQuantityFrom='{15}',@orderQuantityTO='{16}',@DCName='{17}',@OuterChartData={18}, @FulllfilmentType={19}"
                                    , startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), userid,
                                    custid, uniqueReferenceID != null ? uniqueReferenceID : "", dCs != null ? string.Join(",", dCs.ToArray()) : "", stores != null ? string.Join(",", stores.ToArray()) : "", items != null ? string.Join(",", items.ToArray()) : "",
                                    ReportType, ByOrderDateOrInvoiceDate == null ? "" : ByOrderDateOrInvoiceDate, City[0] > 0 ? string.Join(",", City.ToArray()) : "", State[0] > 0 ? "" : string.Join(",", State.ToArray()), Invoice_Number == null ? "" : Invoice_Number,
                                  ItemType[0] > 0 ? string.Join(",", ItemType.ToArray()) : "", item_sub_type[0] > 0 ? string.Join(",", item_sub_type.ToArray()) : "",
                                   OrderedQuantityFrom, orderQuantityTO, DcName, OuterChartData, FulfillmentType
                                );
                return _context.Database.SqlQuery<FillRateReportChart>(Query).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return null;
            }
        }
  
        #endregion

        #region "Delivery Status Report for Charts bby Deepa 23/12/2019"
        public List<DeliveryStatusReportChart> DeliveryStatusReportChartsSearch(DateTime startDate, DateTime endDate, int custid, int userid, string uniqueReferenceID, List<int> dCs, List<int> stores, List<int> items, int ReportType)
        {
            try
            {
                return _context.Database.SqlQuery<DeliveryStatusReportChart>(
                    string.Format("exec [USP_DeliveryStatusReportChart] @FromDate='{0}', @ToDate='{1}',@UserId='{2}', @CustId='{3}', @UniqueReferenceID='{4}', @dcid='{5}', @storeid='{6}', @itemid='{7}',@ReportType={8}", startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), endDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), userid, custid, uniqueReferenceID != null ? uniqueReferenceID : "", dCs != null ? string.Join(",", dCs.ToArray()) : "", stores != null ? string.Join(",", stores.ToArray()) : "", items != null ? string.Join(",", items.ToArray()) : "", ReportType)).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }


        #endregion
    }
}
