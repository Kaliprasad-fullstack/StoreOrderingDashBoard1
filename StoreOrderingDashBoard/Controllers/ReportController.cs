using BAL;
using DAL;
using DbLayer.Service;
using OfficeOpenXml;
using StoreOrderingDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
//using StoreOrderingDashBoard.Reports.DataSet;
//using Microsoft.Reporting.WebForms;
//using System.Data.SqlClient;

namespace StoreOrderingDashBoard.Controllers
{
    //[AdminAuthorization] Test
    public class ReportController : Controller
    {
        // GET: Report test
        private readonly IReportService _reportService;
        private readonly IOrderService _orderService;
        private readonly IWareHouseService _wareHouseService;
        private readonly IStoreService _storeService;
        private readonly IItemService _itemService;
        public ReportController(IReportService reportService, IOrderService orderService, IWareHouseService wareHouseService, IStoreService storeService, IItemService itemService)
        {
            _reportService = reportService;
            _orderService = orderService;
            _wareHouseService = wareHouseService;
            _storeService = storeService;
            _itemService = itemService;
        }
        public ActionResult Index()
        {
            return View();
        }

        [AdminAuthorization]
        public ActionResult DraftReports(DateTime? FromDate, DateTime? ToDate)
        {
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;

            var saveasdraft = from save in _reportService.GetSaveAsDraftOrders()
                              where save.OrderDate >= startDate && save.OrderDate <= endDate
                              select save;

            ViewBag.SaveasDraftOrderList = saveasdraft.ToList();

            return View();
        }

        [AdminAuthorization]
        public ActionResult DraftReportsDetails(DateTime? FromDate, DateTime? ToDate)
        {
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            var saveasdraft = _reportService.DetailReport(startDate, endDate, false, false);
            ViewBag.SaveasDraftOrderList = saveasdraft;
            return View();
        }

        [AdminAuthorization]
        public ActionResult FinalizedReports(DateTime? FromDate, DateTime? ToDate)
        {
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            int userid = SessionValues.UserId;
            var finilised = _reportService.SummeriseReportDcAdmin(startDate, endDate, false, true, false, false, false, custid, userid);
            ViewBag.FinalizedReports = finilised.ToList();
            return View("FinalizedReports");
        }

        [AdminAuthorization]
        public ActionResult FinalizedReportsDetails(DateTime? FromDate, DateTime? ToDate)
        {
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            int userid = SessionValues.UserId;
            var finilized = _reportService.DetailFinilizeReport(startDate, endDate, false, true, false, false, false, custid, userid);
            ViewBag.SaveasDraftOrderList = finilized;
            return View();
        }

        public ActionResult DraftItemsPartial(long orderheaderid)
        {
            List<OrderDetail> orderdetails = _reportService.GetOrderDetails(orderheaderid);
            foreach (OrderDetail order in orderdetails)
            {
                order.Item = _reportService.GetItemById(order.ItemId);
            }
            ViewBag.OrderList = orderdetails;
            return PartialView("_ItemReportListPartial");
        }

        public ActionResult NetsuitItemPartial(string SoNumber)
        {
            var itemlist = _reportService.NetSuiteItemReport(SoNumber);
            ViewBag.ItemList = itemlist;
            return PartialView("_NetsuitItemPartial");
        }

        [AdminAuthorization]
        public ActionResult ProcessedReport(DateTime? FromDate, DateTime? ToDate)
        {
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            int userid = SessionValues.UserId;
            var Proccessed = _reportService.SummeriseReportDcAdmin(startDate, endDate, true, true, false, false, false, custid, userid);
            ViewBag.ProcessedReport = Proccessed.ToList();
            return View();
        }

        [AdminAuthorization]
        public ActionResult ProcessedReportDetails(DateTime? FromDate, DateTime? ToDate)
        {
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            int userid = SessionValues.UserId;
            var processed = _reportService.DetailFinilizeReport(startDate, endDate, true, true, false, false, false, custid, userid);
            ViewBag.SaveasDraftOrderList = processed;
            return View();
        }

        [CustomAuthorization]
        public ActionResult SaveAsDraftbystore(DateTime? FromDate, DateTime? ToDate)
        {
            int StoreId = SessionValues.StoreId;
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;

            var saveasdraft = from save in _orderService.GetSaveAsDraftOrders(StoreId)
                              where save.OrderDate >= startDate && save.OrderDate <= endDate
                              select save;
            ViewBag.SaveasDraftOrderList = saveasdraft.ToList();
            return View();
        }

        [CustomAuthorization]
        public ActionResult SaveAsDraftbystoreDetail(DateTime? FromDate, DateTime? ToDate)
        {
            int StoreId = SessionValues.StoreId;
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;

            var saveasdraft = _reportService.DetailReportByStore(startDate, endDate, false, false, false, false, false, StoreId);
            ViewBag.SaveasDraftOrderList = saveasdraft;
            return View();
        }

        [CustomAuthorization]
        public ActionResult FinalizedReportsbystore(DateTime? FromDate, DateTime? ToDate)
        {
            int StoreId = SessionValues.StoreId;
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;

            var finilised = from final in _orderService.GetFinilizeOrder(StoreId)
                            where final.OrderDate >= startDate && final.OrderDate <= endDate
                            select final;

            ViewBag.FinalizedReports = finilised.ToList();
            return View();
        }

        [CustomAuthorization]
        public ActionResult FinalizedReportsbystoreDetail(DateTime? FromDate, DateTime? ToDate)
        {
            int StoreId = SessionValues.StoreId;
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;

            var saveasdraft = _reportService.DetailReportByStore(startDate, endDate, false, true, false, false, false, StoreId);
            ViewBag.SaveasDraftOrderList = saveasdraft;
            return View();
        }

        [CustomAuthorization]
        public ActionResult ProcessedReportbystore(DateTime? FromDate, DateTime? ToDate)
        {
            int StoreId = SessionValues.StoreId;
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;

            var Proccessed = from process in _orderService.GetProcessedOrder(StoreId)
                             where process.OrderDate >= startDate && process.OrderDate <= endDate
                             select process;

            ViewBag.ProcessedReport = Proccessed.ToList();
            return View();
        }

        [CustomAuthorization]
        public ActionResult ProcessedReportbystoreDetail(DateTime? FromDate, DateTime? ToDate)
        {

            int StoreId = SessionValues.StoreId;
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;

            var saveasdraft = _reportService.DetailReportByStore(startDate, endDate, true, true, false, false, false, StoreId);
            ViewBag.SaveasDraftOrderList = saveasdraft;
            return View();
        }

        [CustomAuthorization]
        public ActionResult SoApprovedbyStore(DateTime? FromDate, DateTime? ToDate)
        {
            int StoreId = SessionValues.StoreId;
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;

            var soapprove = _reportService.SoApprovedReportByStore(startDate, endDate, StoreId);
            ViewBag.SoApproveList = soapprove;
            return View();
        }

        [CustomAuthorization]
        public ActionResult SoApprovedDetailReportByStore(DateTime? FromDate, DateTime? ToDate)
        {
            int StoreId = SessionValues.StoreId;
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;

            var soapprovedetail = _reportService.SoApprovedDetailReportByStore(startDate, endDate, StoreId);
            ViewBag.SoApproveDetail = soapprovedetail;
            return View();
        }

        [AdminAuthorization]
        public ActionResult SoApprovedDetail(DateTime? FromDate, DateTime? ToDate)
        {

            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            int userid = SessionValues.UserId;
            var soapprovedetail = _reportService.SoApprovedDetailReport(startDate, endDate, custid, userid);
            ViewBag.SoApproveDetail = soapprovedetail;
            return View();
        }

        [AdminAuthorization]
        public ActionResult SoApproved(DateTime? FromDate, DateTime? ToDate)
        {
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            int userid = SessionValues.UserId;
            var soapprove = _reportService.SoApprovedReport(startDate, endDate, custid, userid);
            ViewBag.SoApproveList = soapprove;
            return View();
        }

        [CustomAuthorization]
        public ActionResult FullFillmentReportbyStore(DateTime? FromDate, DateTime? ToDate)
        {
            int StoreId = SessionValues.StoreId;
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;

            var fullfillment = _reportService.NetsuiteReportsByStore(startDate, endDate, true, true, true, true, false, StoreId);
            ViewBag.FullFillment = fullfillment;
            return View();
        }

        [CustomAuthorization]
        public ActionResult FullFillmentDetailReportByStore(DateTime? FromDate, DateTime? ToDate)
        {
            int StoreId = SessionValues.StoreId;
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;

            var fullfillmentdetail = _reportService.NetsuiteDetailReportsByStore(startDate, endDate, true, true, true, true, false, StoreId);
            ViewBag.fullfillmentdetail = fullfillmentdetail;
            return View();
        }

        [AdminAuthorization]
        public ActionResult FullFillmentSummaryReport(DateTime? FromDate, DateTime? ToDate)
        {
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            int userid = SessionValues.UserId;
            var FullFillment = _reportService.NetSuiteReportByAdmin(startDate, endDate, true, true, true, true, false, custid, userid);
            ViewBag.FullFillment = FullFillment.ToList();
            return View();
        }

        [AdminAuthorization]
        public ActionResult FullFillmentDetailReport(DateTime? FromDate, DateTime? ToDate)
        {
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            int userid = SessionValues.UserId;
            var fullfillmentdetail = _reportService.NetSuiteDetailReportByAdmin(startDate, endDate, true, true, true, true, false, custid, userid);
            ViewBag.fullfillmentdetail = fullfillmentdetail.ToList();
            return View();
        }

        [CustomAuthorization]
        public ActionResult InvoiceReportByStore(DateTime? FromDate, DateTime? ToDate)
        {
            int StoreId = SessionValues.StoreId;
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;

            var invoice = _reportService.NetsuiteReportsByStore(startDate, endDate, true, true, true, true, true, StoreId);
            ViewBag.invoice = invoice;
            return View();
        }

        [CustomAuthorization]
        public ActionResult InvoiceDetailReportByStore(DateTime? FromDate, DateTime? ToDate)
        {
            int StoreId = SessionValues.StoreId;
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;

            var InvoiceDetail = _reportService.NetsuiteDetailReportsByStore(startDate, endDate, true, true, true, true, true, StoreId);
            ViewBag.InvoiceDetail = InvoiceDetail;
            return View();
        }

        [AdminAuthorization]
        public ActionResult InvoiceSummaryReport(DateTime? FromDate, DateTime? ToDate)
        {
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            int userid = SessionValues.UserId;
            var invoice = _reportService.NetSuiteReportByAdmin(startDate, endDate, true, true, true, true, true, custid, userid);
            ViewBag.invoice = invoice.ToList();
            return View();
        }

        [AdminAuthorization]
        public ActionResult InvoiceDetailReport(DateTime? FromDate, DateTime? ToDate)
        {
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            int userid = SessionValues.UserId;
            var InvoiceDetail = _reportService.NetSuiteDetailReportByAdmin(startDate, endDate, true, true, true, true, true, custid, userid);
            ViewBag.InvoiceDetail = InvoiceDetail.ToList();
            return View();
        }

        [AdminAuthorization]
        public ActionResult OrderCount(DateTime? FromDate, DateTime? ToDate, string Region, string State, string Dc, string Store, string Category, List<int> Sku)
        {
            ViewBag.Region = _wareHouseService.RegionMst();
            ViewBag.Year = _reportService.GetYear();
            var a = _wareHouseService.RegionMst();
            ViewBag.State = _wareHouseService.stateMst();
            ViewBag.WareHouse = _wareHouseService.GetWareHouse();
            int Id = Convert.ToInt32(SessionValues.LoggedInCustId);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["Regions"] = (Region != null ? Region : "");
            ViewData["States"] = (State != null ? State : "");
            ViewData["Dcs"] = (Dc != null ? Dc : "");
            ViewData["Stores"] = (Store != null ? Store : "");
            ViewData["Categorys"] = (Category != null ? Category : "");
            ViewBag.Store = _storeService.GetStoresByCustomerId(Id);

            int userid = SessionValues.UserId;
            string Item = (Sku != null ? String.Join(",", Sku) : String.Join(",", _itemService.GetItemsByCustomer(Id)));
            ViewBag.Category = _itemService.Categories();
            var data = _reportService.reportCounts(startDate, endDate, Id, (Region != null ? Region : ""), (State != null ? State : ""), (Dc != null ? Dc : ""), (Store != null ? Store : ""), Item, userid);
            ViewBag.OrderCount = data;
            return View();
        }

        [AdminAuthorization]
        public ActionResult RevisedOrderReport(DateTime? FromDate, DateTime? ToDate, string Region, string State, string Dc, string Store, string Category, List<int> Sku)
        {
            ViewBag.Region = _wareHouseService.RegionMst();
            ViewBag.Year = _reportService.GetYear();
            var a = _wareHouseService.RegionMst();
            ViewBag.State = _wareHouseService.stateMst();
            ViewBag.WareHouse = _wareHouseService.GetWareHouse();
            int Id = Convert.ToInt32(SessionValues.LoggedInCustId);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["Regions"] = (Region != null ? Region : "");
            ViewData["States"] = (State != null ? State : "");
            ViewData["Dcs"] = (Dc != null ? Dc : "");
            ViewData["Stores"] = (Store != null ? Store : "");
            ViewData["Categorys"] = (Category != null ? Category : "");
            ViewBag.Store = _storeService.GetStoresByCustomerId(Id);

            string Item = (Sku != null ? String.Join(",", Sku) : String.Join(",", _itemService.GetItemsByCustomer(Id)));
            int userid = SessionValues.UserId;
            ViewBag.Category = _itemService.Categories();
            var data = _reportService.Revisedreport(startDate, endDate, Id, (Region != null ? Region : ""), (State != null ? State : ""), (Dc != null ? Dc : ""), (Store != null ? Store : ""), Item, userid);
            ViewBag.OrderCount = data;
            return View();
        }

        [AdminAuthorization]
        public ActionResult OfrReport(DateTime? FromDate, DateTime? ToDate, string Region, string State, string Dc, string Store, string Category, List<int> Sku)
        {

            ViewBag.Region = _wareHouseService.RegionMst();
            ViewBag.Year = _reportService.GetYear();
            var a = _wareHouseService.RegionMst();
            ViewBag.State = _wareHouseService.stateMst();
            ViewBag.WareHouse = _wareHouseService.GetWareHouse();
            int Id = Convert.ToInt32(SessionValues.LoggedInCustId);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["Regions"] = (Region != null ? Region : "");
            ViewData["States"] = (State != null ? State : "");
            ViewData["Dcs"] = (Dc != null ? Dc : "");
            ViewData["Stores"] = (Store != null ? Store : "");
            ViewData["Categorys"] = (Category != null ? Category : "");
            ViewBag.Store = _storeService.GetStoresByCustomerId(Id);

            int userid = SessionValues.UserId;
            string Item = (Sku != null ? String.Join(",", Sku) : String.Join(",", _itemService.GetItemsByCustomer(Id)));
            ViewBag.Category = _itemService.Categories();
            var data = _reportService.OFRReport(startDate, endDate, Id, (Region != null ? Region : ""), (State != null ? State : ""), (Dc != null ? Dc : ""), (Store != null ? Store : ""), Item, userid);
            ViewBag.OFRCount = data;
            return View();
        }


        [AdminAuthorization]
        public ActionResult FinalReport(DateTime? FromDate, DateTime? ToDate)
        {
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? new DateTime(now.Year, now.Month, 1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddMonths(1).AddDays(-1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            int userid = SessionValues.UserId;
            var finilised = _reportService.GetFinalReport(startDate, endDate, /*true, true, true, true, true,*/ custid, userid);
            ViewBag.FinalizedReports = finilised.ToList();
            return View();
        }

        [AdminAuthorization]
        public ActionResult DcManagerReport(DateTime? FromDate, DateTime? ToDate)
        {
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? now.Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            int userid = SessionValues.UserId;
            var finilised = _reportService.GetFinalReport(startDate, endDate, /*true, true, true, true, true,*/ custid, userid);
            ViewBag.FinalizedReports = finilised.ToList();
            return View();
        }


        [AdminApiAuthorization]
        public JsonResult GetItem(string Item)
        {
            int Id = Convert.ToInt32(SessionValues.LoggedInCustId);
            var Items = _itemService.CustomerWideItem(Id, Item);
            return Json(Items, JsonRequestBehavior.AllowGet);
        }

        [AdminApiAuthorization]
        public JsonResult GetWeek(string Year)
        {
            var week = _reportService.GetWeek(Year);
            return Json(week, JsonRequestBehavior.AllowGet);
        }

        [AdminApiAuthorization]
        public JsonResult GetWeekByQuarter(string Year, int Quarter)
        {
            var week = _reportService.GetWeekByQuarter(Year, Quarter);
            return Json(week, JsonRequestBehavior.AllowGet);
        }

        [AdminApiAuthorization]
        public JsonResult GetWeekByMonth(string Year, int Month)
        {
            var week = _reportService.GetWeekByMonth(Year, Month);
            return Json(week, JsonRequestBehavior.AllowGet);
        }

        [AdminApiAuthorization]
        public JsonResult GetWeekFirstLastDay(string Year, int Week)
        {
            var week = _reportService.GetDays(Year, Week);
            return Json(week, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PendingPlannedOrders()
        {
            return View();
        }
        [AdminAuthorization]
        public ActionResult QlikViewReport()
        {
            return View();
        }
        [AdminAuthorization]
        public ActionResult UniqueRefenenceReport()
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
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
            var finilised = _reportService.UniqueRefenenceReport(startDate, endDate, /*true, true, true, true, true,*/ custid, userid, null, selectedDCs, selectedStores, SelectedSKUs);
            ViewBag.FinalizedReports = finilised.ToList();
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            return View();
        }
        [AdminAuthorization]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UniqueRefenenceReport(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
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
            var finilised = _reportService.UniqueRefenenceReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs);
            ViewBag.FinalizedReports = finilised.ToList();
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            return View();
        }
        //[AdminAuthorization]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UniqueRefenenceExportExcel(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
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
            var finilised = _reportService.UniqueRefenenceReport(startDate, endDate, /*true, true, true, true, true,*/ custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs);
            string Title = "Name: Daily Sales Report";
            string Message = "Store Order Date: From: " + startDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " To: " + endDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "";

            foreach (var head in finilised)
            {
                head.OrderPlacedDate = head.OrderPlacedDate != "" && head.OrderPlacedDate != null ? HelperCls.FromExcelStringDate(head.OrderPlacedDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null;
                head.LastOrderDate = head.LastOrderDate != "" && head.LastOrderDate != null ? HelperCls.FromExcelStringDate(head.LastOrderDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "";
                head.LastDeliveryDate = head.LastDeliveryDate != "" && head.LastDeliveryDate != null ? HelperCls.FromExcelStringDate(head.LastDeliveryDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "";
                head.PlanDate = head.PlanDate != null ? head.PlanDate : "";
                head.InvoiceDate = head.InvoiceDate != null ? head.InvoiceDate : "";
                head.DeliveryDate = head.DeliveryDate != null ? head.DeliveryDate : "";
                head.DocDate = head.DocDate != null ? head.DocDate : "";
            }
            var finilisedList = finilised.Select(head => new
            {
                head.UniqueReferenceID,
                head.OrderDate,
                head.OrderPlacedDate,
                head.OrderType,
                head.StoreCode,
                head.City,
                head.State,
                head.Focus,
                head.LastOrderDate,
                head.LastDeliveryDate,
                head.SKUCode,
                head.SKUName,
                head.Item_Type,
                head.Item_sub_Type,
                head.requested_Quantity,
                head.PackSize,
                head.CaseSize,
                head.Cases,
                head.New_Build,
                head.order_status,
                head.invoice_status,
                head.FromDC,
                head.PlanDate,
                head.ord_no,
                head.SoNumber,
                head.InvoiceDate,
                head.Invoice_Number,
                head.inv_Quantity,
                head.DeliveryDate,
                head.TakenDays,
                head.Slab,
                head.VehicleNo,
                head.DriverName,
                head.DriverContactNo,
                head.DocDate,
                head.Remarks,
                head.OrderReferenceId,
                head.Return_Authorization
            });
            //ExportToExcel(finilised);
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].Value = Title;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Cells[2, 1].Value = Message;
                workSheet.Cells[2, 1].Style.Font.Bold = true;
                string DateCellFormat = "dd-mm-yyyy";
                workSheet.Cells[4, 1].Value = "Unique Reference ID";
                workSheet.Cells[4, 2].Value = "Date of Order";
                workSheet.Cells[4, 3].Value = "Order Placed Date";
                workSheet.Cells[4, 4].Value = "Order Type";
                workSheet.Cells[4, 5].Value = "Kitchen ID";
                workSheet.Cells[4, 6].Value = "City";
                workSheet.Cells[4, 7].Value = "State";
                workSheet.Cells[4, 8].Value = "Store Priority";
                workSheet.Cells[4, 9].Value = "Last Order Date";
                workSheet.Cells[4, 10].Value = "Last Delivery Date";
                workSheet.Cells[4, 11].Value = "SKU Code";
                workSheet.Cells[4, 12].Value = "Item Name";
                workSheet.Cells[4, 13].Value = "Item Type";
                workSheet.Cells[4, 14].Value = "Item Sub Type";
                workSheet.Cells[4, 15].Value = "Order Quantity";
                workSheet.Cells[4, 16].Value = "Pack Size";
                workSheet.Cells[4, 17].Value = "Case Size";
                workSheet.Cells[4, 18].Value = "# of Cases";
                workSheet.Cells[4, 19].Value = "New_Build";
                workSheet.Cells[4, 20].Value = "Order Status";
                workSheet.Cells[4, 21].Value = "Invoice Status";
                workSheet.Cells[4, 22].Value = "From DC";
                workSheet.Cells[4, 23].Value = "Plan Date";
                workSheet.Cells[4, 24].Value = "PO Number";
                workSheet.Cells[4, 25].Value = "SO Number";
                workSheet.Cells[4, 26].Value = "Invoiced Date";
                workSheet.Cells[4, 27].Value = "Invoice No";
                workSheet.Cells[4, 28].Value = "Invoiced Quantity";
                workSheet.Cells[4, 29].Value = "Delivery Date";
                workSheet.Cells[4, 30].Value = "Taken Days";
                workSheet.Cells[4, 31].Value = "Slab";
                workSheet.Cells[4, 32].Value = "Vehicle No.";
                workSheet.Cells[4, 33].Value = "Driver Name";
                workSheet.Cells[4, 34].Value = "Driver Contact No.";
                workSheet.Cells[4, 35].Value = "DocDate";
                workSheet.Cells[4, 36].Value = "Remarks";
                workSheet.Cells[4, 37].Value = "Order Reference ID";
                workSheet.Cells[4, 38].Value = "Return Authorization";
                workSheet.Cells[1, 1, 1, 38].Style.Font.Bold = true;

                using (ExcelRange Rng = workSheet.Cells["A4:AL4"])
                {
                    Rng.Style.Font.Bold = true;
                }
                workSheet.Cells[5, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                using (ExcelRange Rng = workSheet.Cells["B:B"])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }
                using (ExcelRange Rng = workSheet.Cells["P:P"])
                {
                    Rng.Style.Numberformat.Format = "#";
                }
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Daily Sales Report.xlsx");
            }
        }
        [AdminAuthorization]
        public ActionResult DSRAdvanced()
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            //ItemCategories
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            //ItemType
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
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
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", null);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DSRAdvanced(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();

            List<DailySalesReport> finilised = _reportService.DailySalesReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, SelectedLocations, SelectedTypes, SelectedCategories);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.FinalizedReports = finilised.ToList();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DSRAdvancedExportExcel(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();

            List<DailySalesReport> finilised = _reportService.DailySalesReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, SelectedLocations, SelectedTypes, SelectedCategories);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.FinalizedReports = finilised.ToList();

            string Title = "Name: Daily Sales Report";
            string Message = "Store Order Date: From: " + startDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " To: " + endDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "";
            var finilisedList = finilised.Select(head => new
            {
                head.PONumber,
                head.Unique_Reference_Id,

                Store_Order_Date = Convert.ToDateTime(head.Store_Order_Date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Cust_Order_Date = head.Cust_Order_Date != null ? Convert.ToDateTime(head.Cust_Order_Date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                UploadedDate = Convert.ToDateTime(head.Upload_Date).ToString("dd/MM/yyyy hh:mm", CultureInfo.InvariantCulture),
                head.Order_Type,

                head.EnterpriseCode,
                head.EnterpriseName,
                head.Store_Code,
                head.StoreName,
                head.City,
                head.PlaceOfSupply,

                head.Item_Code,
                head.Item_Desc,
                head.ItemCategoryType,
                head.Item_Type,
                head.UnitofMasureDescription,
                head.Case_Size,

                head.Original_Ordered_Qty,
                head.Revised_Ordered_Qty,
                head.Rfpl_Ordered_Qty,

                head.Number_Of_Cases,
                head.Order_Status,
                head.Invoice_Status,
                head.Dc_Name,
                head.PlanDate,
                head.OrderNo,

                head.Invoice_Number,
                head.Invoice_Quantity,
                head.Invoice_Date,

                head.Delivered_Status1,
                head.Delivered_Date1,

                head.Delivered_Status2,
                head.Delivered_Date2,

                head.Delivered_Qty,
                head.Return_Reason,
                head.Taken_Days,
                head.Slab,
                head.Remarks,
                head.Order_Reference_Id,
                head.Last_Order_Date,
                head.Last_Delivery_Date
            });
            //ExportToExcel(finilised);
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].Value = Title;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Cells[2, 1].Value = Message;
                workSheet.Cells[2, 1].Style.Font.Bold = true;
                string DateCellFormat = "dd/mm/yyyy";
                workSheet.Cells[4, 1].Value = "PO Number";
                workSheet.Cells[4, 2].Value = "Unique Reference ID";

                workSheet.Cells[4, 3].Value = "Store Order Date";
                workSheet.Cells[4, 4].Value = "Order Placed Date";
                workSheet.Cells[4, 5].Value = "System DateTime";
                workSheet.Cells[4, 6].Value = "Order Type";

                workSheet.Cells[4, 7].Value = "Enterprise Code";
                workSheet.Cells[4, 8].Value = "Enterprise Name";
                workSheet.Cells[4, 9].Value = "Store Code";
                workSheet.Cells[4, 10].Value = "Store Name";
                workSheet.Cells[4, 11].Value = "City";
                workSheet.Cells[4, 12].Value = "Place of Supply";

                workSheet.Cells[4, 13].Value = "SKU Code";
                workSheet.Cells[4, 14].Value = "SKU Description";
                workSheet.Cells[4, 15].Value = "SKU Category Type";
                workSheet.Cells[4, 16].Value = "SKU Category";
                workSheet.Cells[4, 17].Value = "UOM";
                workSheet.Cells[4, 18].Value = "Case Size";

                workSheet.Cells[4, 19].Value = "Original Order Quantity";
                workSheet.Cells[4, 20].Value = "Revised Order Quantity";
                workSheet.Cells[4, 21].Value = "RFPL Order Quantity";

                workSheet.Cells[4, 22].Value = "# of Cases";
                workSheet.Cells[4, 23].Value = "Order Status";
                workSheet.Cells[4, 24].Value = "Invoice Status";
                workSheet.Cells[4, 25].Value = "DC";
                workSheet.Cells[4, 26].Value = "Plan Date";
                workSheet.Cells[4, 27].Value = "PO Number";

                workSheet.Cells[4, 28].Value = "Invoice Number";
                workSheet.Cells[4, 29].Value = "Invoiced Quantity";
                workSheet.Cells[4, 30].Value = "Invoiced Date";

                workSheet.Cells[4, 31].Value = "Delivered Status1";
                workSheet.Cells[4, 32].Value = "Delivered Date1";

                workSheet.Cells[4, 33].Value = "Delivered Status2";
                workSheet.Cells[4, 34].Value = "Delivered Date2";

                workSheet.Cells[4, 35].Value = "Delivered Quantity";
                workSheet.Cells[4, 36].Value = "Return Reason";
                workSheet.Cells[4, 37].Value = "Taken Days";
                workSheet.Cells[4, 38].Value = "Slab";
                workSheet.Cells[4, 39].Value = "Remarks";
                workSheet.Cells[4, 40].Value = "Order Reference ID";
                workSheet.Cells[4, 41].Value = "Last Order Date";
                workSheet.Cells[4, 42].Value = "Last Delivery Date";
                workSheet.Cells[1, 1, 1, 42].Style.Font.Bold = true;
                using (ExcelRange Rng = workSheet.Cells["A4:AN4"])
                {
                    Rng.Style.Font.Bold = true;
                }
                workSheet.Cells[5, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                using (ExcelRange Rng = workSheet.Cells["C:C"])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }
                using (ExcelRange Rng = workSheet.Cells["E:E"])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Daily Sales Report.xlsx");
            }
        }

        [AdminAuthorization]
        public ActionResult Report2()
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            //ItemCategories
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            //ItemType
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
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

            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", null);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Report2(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, int ReportType, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = UniqueReferenceID;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();

            List<Report2> finilised = _reportService.Report2(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, ReportType, SelectedLocations, SelectedTypes, SelectedCategories);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.FinalizedReports = finilised.ToList();
            ViewBag.ReportType = ReportType;
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Report2ExportExcel(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, int ReportType, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = UniqueReferenceID;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();

            List<Report2> finilised = _reportService.Report2(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, ReportType, SelectedLocations, SelectedTypes, SelectedCategories);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.FinalizedReports = finilised.ToList();
            ViewBag.ReportType = ReportType;
            if (ReportType == 1 || ReportType == 5)
            {
                string Title = "Name: City - Order - Wise Report";
                if (ReportType == 5)
                    Title = "Name: City - Order - Wise Report- New";
                string Message = "Store Order Date: From: " + startDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " To: " + endDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "";
                var finilisedList = finilised.Select(head => new
                {
                    head.city,
                    head.OYO_Orders,
                    head.rfpl_correct_fullfilled_orders,
                    head.Correct_Fulfilment,
                    head.partial_orders,
                    head.Partial_Fulfilment,
                    head.pending_orders,
                    head.Pending
                });
                using (var excel = new ExcelPackage())
                {
                    var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells[1, 1].Value = Title;
                    workSheet.Cells[1, 1].Style.Font.Bold = true;
                    workSheet.Cells[2, 1].Value = Message;
                    workSheet.Cells[2, 1].Style.Font.Bold = true;
                    //string DateCellFormat = "dd-mm-yyyy";
                    if (ReportType == 1)
                    {
                        workSheet.Cells[4, 1].Value = "City";
                        workSheet.Cells[4, 2].Value = "No Of Lines";
                        workSheet.Cells[4, 3].Value = "Correct Fullfilment Lines";
                        workSheet.Cells[4, 4].Value = "Correct Fulfilment%";
                        workSheet.Cells[4, 5].Value = "Partial Fulfilment Lines";
                        workSheet.Cells[4, 6].Value = "Partial Fulfilment%";
                        workSheet.Cells[4, 7].Value = "Pending Fulfilment Lines";
                        workSheet.Cells[4, 8].Value = "Pending Fulfilment%";

                    }
                    else
                    {
                        workSheet.Cells[4, 1].Value = "City";
                        workSheet.Cells[4, 2].Value = "No Of Orders";
                        workSheet.Cells[4, 3].Value = "Correct Fullfilment Orders";
                        workSheet.Cells[4, 4].Value = "Correct Fulfilment%";
                        workSheet.Cells[4, 5].Value = "Partial Fulfilment Orders";
                        workSheet.Cells[4, 6].Value = "Partial Fulfilment%";
                        workSheet.Cells[4, 7].Value = "Pending Fulfilment Orders";
                        workSheet.Cells[4, 8].Value = "Pending Fulfilment%";
                    }

                    using (ExcelRange Rng = workSheet.Cells["A4:AL4"])
                    {
                        Rng.Style.Font.Bold = true;
                    }
                    workSheet.Cells[5, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                    workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                    return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FillRateReport.xlsx");
                }
            }
            else if (ReportType == 2 || ReportType == 6)
            {
                string Title = "Name: City - Order - Quantity Report";
                if (ReportType == 6)
                    Title = "Name: City - Order - Quantity Report New";
                string Message = "Store Order Date: From: " + startDate.ToString("dd-MM-yyyy") + " To: " + endDate.ToString("dd-MM-yyyy") + "";
                var finilisedList = finilised.Select(head => new
                {
                    head.city,
                    head.oyo_order_qty,
                    head.rfpl_order_qty,
                    head.Correct_Fulfilment,
                    head.partial_order_qty,
                    head.Partial_Fulfilment,
                    head.pending_order_qty,
                    head.Pending
                });
                using (var excel = new ExcelPackage())
                {
                    var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells[1, 1].Value = Title;
                    workSheet.Cells[1, 1].Style.Font.Bold = true;
                    workSheet.Cells[2, 1].Value = Message;
                    workSheet.Cells[2, 1].Style.Font.Bold = true;
                    //string DateCellFormat = "dd-mm-yyyy";
                    workSheet.Cells[4, 1].Value = "City";
                    workSheet.Cells[4, 2].Value = "Total Quantity";
                    workSheet.Cells[4, 3].Value = "Correct Fulfilment Quantity";
                    workSheet.Cells[4, 4].Value = "Correct Fulfilment%";
                    workSheet.Cells[4, 5].Value = "Partial Fulfilment Quantity";
                    workSheet.Cells[4, 6].Value = "Partial Fulfilment%";

                    workSheet.Cells[4, 7].Value = "Pending fulfilment Quantity";
                    workSheet.Cells[4, 8].Value = "Pending Fulfilment%";
                    using (ExcelRange Rng = workSheet.Cells["A4:AL4"])
                    {
                        Rng.Style.Font.Bold = true;
                    }
                    workSheet.Cells[5, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                    workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                    return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FillRateReport.xlsx");
                }
            }
            else if (ReportType == 3 || ReportType == 7)
            {
                string Title = "Name: Item - SubType - Order";
                if (ReportType == 7)
                    Title = "Name: Item - SubType - Order New";
                string Message = "Store Order Date: From: " + startDate.ToString("dd-MM-yyyy") + " To: " + endDate.ToString("dd-MM-yyyy") + "";
                var finilisedList = finilised.Select(head => new
                {
                    head.Item_Sub_Type,
                    head.OYO_Orders,
                    head.rfpl_correct_fullfilled_orders,
                    head.Correct_Fulfilment,
                    head.partial_orders,
                    head.Partial_Fulfilment,
                    head.pending_orders,
                    head.Pending
                });
                using (var excel = new ExcelPackage())
                {
                    var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells[1, 1].Value = Title;
                    workSheet.Cells[1, 1].Style.Font.Bold = true;
                    workSheet.Cells[2, 1].Value = Message;
                    workSheet.Cells[2, 1].Style.Font.Bold = true;
                    //string DateCellFormat = "dd-mm-yyyy";
                    if (ReportType == 3)
                    {
                        workSheet.Cells[4, 1].Value = "Item Sub Type";
                        workSheet.Cells[4, 2].Value = "No of Lines";
                        workSheet.Cells[4, 3].Value = "Correct Fulfilment Lines";
                        workSheet.Cells[4, 4].Value = "Correct Fulfilment%";
                        workSheet.Cells[4, 5].Value = "Partial Fulfilment Lines";
                        workSheet.Cells[4, 6].Value = "Partial Fulfilment%";
                        workSheet.Cells[4, 7].Value = "Pending Fulfilment Lines";
                        workSheet.Cells[4, 8].Value = "Pending Fulfilment%";
                    }
                    else
                    {
                        workSheet.Cells[4, 1].Value = "Item Sub Type";
                        workSheet.Cells[4, 2].Value = "No of Orders";
                        workSheet.Cells[4, 3].Value = "Correct Fulfilment Orders";
                        workSheet.Cells[4, 4].Value = "Correct Fulfilment%";
                        workSheet.Cells[4, 5].Value = "Partial Fulfilment Orders";
                        workSheet.Cells[4, 6].Value = "Partial Fulfilment%";
                        workSheet.Cells[4, 7].Value = "Pending Fulfilment Orders";
                        workSheet.Cells[4, 8].Value = "Pending Fulfilment%";
                    }

                    using (ExcelRange Rng = workSheet.Cells["A4:AL4"])
                    {
                        Rng.Style.Font.Bold = true;
                    }
                    workSheet.Cells[5, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                    workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                    return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FillRateReport.xlsx");
                }
            }
            else
            {
                string Title = "Name: Item - SubType - Quantity";
                if (ReportType == 8)
                    Title = "Name: Item - SubType - Quantity New";
                string Message = "Store Order Date: From: " + startDate.ToString("dd-MM-yyyy") + " To: " + endDate.ToString("dd-MM-yyyy") + "";
                var finilisedList = finilised.Select(head => new
                {
                    head.Item_Sub_Type,
                    head.oyo_order_qty,
                    head.rfpl_order_qty,
                    head.Correct_Fulfilment,
                    head.partial_order_qty,
                    head.Partial_Fulfilment,
                    head.pending_order_qty,
                    head.Pending
                });
                using (var excel = new ExcelPackage())
                {
                    var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells[1, 1].Value = Title;
                    workSheet.Cells[1, 1].Style.Font.Bold = true;
                    workSheet.Cells[2, 1].Value = Message;
                    workSheet.Cells[2, 1].Style.Font.Bold = true;
                    //string DateCellFormat = "dd-mm-yyyy";
                    workSheet.Cells[4, 1].Value = "Item Sub Type";
                    workSheet.Cells[4, 2].Value = "Total Quantity";
                    workSheet.Cells[4, 3].Value = "Total Fulfilment Quantity%";
                    workSheet.Cells[4, 4].Value = "Correct Fulfilment";
                    workSheet.Cells[4, 5].Value = "Partial Fulfilment Quantity";
                    workSheet.Cells[4, 6].Value = "Partial Fulfilment%";
                    workSheet.Cells[4, 7].Value = "Pending Fulfilment Quantity";
                    workSheet.Cells[4, 8].Value = "Pending Fulfilment";
                    using (ExcelRange Rng = workSheet.Cells["A4:AL4"])
                    {
                        Rng.Style.Font.Bold = true;
                    }
                    workSheet.Cells[5, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);

                    workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                    return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FillRateReport.xlsx");
                }
            }
        }

        #region "Pendency Order Report By Deepa 11/8/2019"
        [AdminAuthorization]
        public ActionResult PendencyOrderReport()
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            //ItemCategories
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            //ItemType
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
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

            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", null);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult PendencyOrderReport(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, String ReportType, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = UniqueReferenceID;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();

            List<PendencyOrderReport> finilised = _reportService.PendencyOrderReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, ReportType, SelectedLocations, SelectedTypes, SelectedCategories);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.FinalizedReports = finilised.ToList();
            ViewBag.ReportType = ReportType;
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult PendencyOrderReportExportExcel(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, String ReportType, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = UniqueReferenceID;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();

            var finilised = _reportService.PendencyOrderReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, ReportType, SelectedLocations, SelectedTypes, SelectedCategories);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.FinalizedReports = finilised.ToList();
            ViewBag.ReportType = ReportType;

            string Title = "Name: Pendency Order Report";
            string Message = "Order Date: From: " + startDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " To: " + endDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "";
            List<PendencyOrderReportExportToExcel> finilisedList = finilised.Select(head => new PendencyOrderReportExportToExcel
            {
                PONumber = head.PONumber,
                Unique_Reference_Id = head.Unique_Reference_Id,
                DateOfOrder = Convert.ToDateTime(head.DateOfOrder).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                EnterpriseCode = head.EnterpriseCode,
                EnterpriseName = head.EnterpriseName,
                KitchenId = head.KitchenId,
                StoreName = head.StoreName,
                CityName = head.CityName,
                EXCode = head.EXCode,
                SKUName = head.ItemName,
                OrderQuantity = Convert.ToDecimal(head.OrderQuantity),
                DCCode = head.DCCode,
                Status = head.Status,
                PendingDay = head.PendingDay,
                Slab = head.Slab,
                InvoiceQuantity = head.InvoiceQuantity
            }).ToList();
            //ExportToExcel(finilised);
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].Value = Title;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Cells[2, 1].Value = Message;
                workSheet.Cells[2, 1].Style.Font.Bold = true;
                string DateCellFormat = "dd-mm-yyyy";
                workSheet.Cells[4, 1].Value = "PO Number";
                workSheet.Cells[4, 2].Value = "Unique Reference ID";
                workSheet.Cells[4, 3].Value = "Store Order Date";
                workSheet.Cells[4, 4].Value = "Enterprise Code";
                workSheet.Cells[4, 5].Value = "Enterprise Name";
                workSheet.Cells[4, 6].Value = "Store Code";
                workSheet.Cells[4, 7].Value = "Store Name";
                workSheet.Cells[4, 8].Value = "City";
                workSheet.Cells[4, 9].Value = "SKU Code";
                workSheet.Cells[4, 10].Value = "SKU Name";
                workSheet.Cells[4, 11].Value = "Ordered Quantity";
                workSheet.Cells[4, 12].Value = "From DC";
                workSheet.Cells[4, 13].Value = "Status";
                workSheet.Cells[4, 14].Value = "Pending Days";
                workSheet.Cells[4, 15].Value = "Slab";
                workSheet.Cells[4, 16].Value = "Invoice Quantity";
                using (ExcelRange Rng = workSheet.Cells["A4:AL4"])
                {
                    Rng.Style.Font.Bold = true;
                }
                workSheet.Cells[5, 1].LoadFromCollection(finilisedList.ToList(), PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                using (ExcelRange Rng = workSheet.Cells["B:B"])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }
                using (ExcelRange Rng = workSheet.Cells["K:K"])
                {
                    Rng.Style.Numberformat.Format = "#";
                }
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PendencyOrderReport.xlsx");
            }
        }


        #endregion

        #region "Delivery Status Report By Deepa 11/11/2019 updated date 2020/01/06"
        [AdminAuthorization]
        public ActionResult DeliveryStatusReport()
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");

            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            //ItemCategories
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            //ItemType
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
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
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", null);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DeliveryStatusReport(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, int ReportType, string InvoiceNo, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            DateTime now = DateTime.Now;
            string DateCellFormat = "dd-mm-yyyy";
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = UniqueReferenceID;
            ViewData["InvoiceNO"] = InvoiceNo;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();
            List<DeliveryStatusReport> finilised = _reportService.DeliveryStatusReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, ReportType, InvoiceNo, SelectedLocations, SelectedTypes, SelectedCategories);
            if (InvoiceNo != "")
            {

                finilised = finilised.Where(a => a.InvoiceNumber.Length > 0).ToList();
                finilised = finilised.Where(a => a.InvoiceNumber.Contains(InvoiceNo)).ToList();
            }
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.FinalizedReports = finilised.ToList();
            ViewBag.ReportType = ReportType;
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DeliveryStatusReportExportExcel(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, int ReportType, string InvoiceNo, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = UniqueReferenceID;
            ViewData["InvoiceNO"] = InvoiceNo;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();
            var finilised = _reportService.DeliveryStatusReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, ReportType, InvoiceNo, SelectedLocations, SelectedTypes, SelectedCategories);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            if (InvoiceNo != "")
            {

                finilised = finilised.Where(a => a.InvoiceNumber.Length > 0).ToList();
                finilised = finilised.Where(a => a.InvoiceNumber.Contains(InvoiceNo)).ToList();
            }
            string Title = "";
            if (ReportType == 1)
            {
                Title = "Name: Invoice Level Delivery Report";
            }
            else
            {
                Title = "Name: Order Level Delivery Report";
            }
            string Message = "Store Order Date: From: " + startDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " To: " + endDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "";
            var finilisedList = finilised.Select(head => new
            {
                head.UniqueReferenceId,
                Store_Order_Date = Convert.ToDateTime(head.Store_Order_Date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                head.InvoiceDate,
                head.DeliveryDate,
                head.DCName,
                head.StoreCode,
                head.City,
                head.VehicleNo,
                head.InvoiceNumber,
                head.DeliveryStatus,
                head.Reason
            });
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].Value = Title;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Cells[2, 1].Value = Message;
                workSheet.Cells[2, 1].Style.Font.Bold = true;
                string DateCellFormat = "dd-mm-yyyy";
                if (ReportType == 2)
                {
                    workSheet.Cells[4, 1].Value = "Unique Reference Id";
                }
                workSheet.Cells[4, 2].Value = "Store Order Date";
                workSheet.Cells[4, 3].Value = "Dispatch Date";
                workSheet.Cells[4, 4].Value = "Delivery Date";
                workSheet.Cells[4, 5].Value = "From DC";
                workSheet.Cells[4, 6].Value = "Store Code";
                workSheet.Cells[4, 7].Value = "City";
                workSheet.Cells[4, 8].Value = "Vehicle No";

                workSheet.Cells[4, 9].Value = "Invocie Number";
                workSheet.Cells[4, 10].Value = "Delivery Status";
                workSheet.Cells[4, 11].Value = "Remark";
                using (ExcelRange Rng = workSheet.Cells["A4:AL4"])
                {
                    Rng.Style.Font.Bold = true;
                }
                using (ExcelRange Rng = workSheet.Cells["B:B"])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }
                using (ExcelRange Rng = workSheet.Cells["C:C"])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }

                workSheet.Cells[5, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DeliveryStatusReport.xlsx");

            }

        }
        #endregion

        #region "Ageing Report By Deepa 11/13/2019"
        [AdminAuthorization]
        public ActionResult AgeingReport()
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            DateTime now = DateTime.Now;

            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            //ItemCategories
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            //ItemType
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);

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

            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", null);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AgeingReport(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, int ReportType, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories)
        {

            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            DateTime now = DateTime.Now;
            string DateCellFormat = "dd-mm-yyyy";
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = UniqueReferenceID;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();

            List<AgeingReport> finilised = _reportService.AgeingReport(startDate, endDate, custid, userid, SelectedDC, SelectedStores, SelectedSKUs, ReportType, SelectedLocations, SelectedTypes, SelectedCategories);
            ViewBag.FinalizedReports = finilised.ToList();
            ViewBag.ReportType = ReportType;
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);

            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AgeingReportExportExcel(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, int ReportType, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
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
            var finilised = _reportService.AgeingReport(startDate, endDate, custid, userid, SelectedDC, SelectedStores, SelectedSKUs, ReportType, SelectedLocations, SelectedTypes, SelectedCategories);
            string Title = "";
            if (ReportType == 1)
            {
                Title = "Name: Booked not Invoiced";
            }
            if (ReportType == 2)
            {
                Title = "Name: Invoiced not delivered";
            }
            else
            {
                Title = "Name: Delivered no POD";
            }
            string Message = "Store Order Date: From: " + startDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " To: " + endDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "";
            var finilisedList = finilised.Select(head => new
            {
                head.Store_Code,
                head.A0_5,
                head.A6_10,
                head.A11_15,
                head.Above_16
            });
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].Value = Title;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Cells[2, 1].Value = Message;
                workSheet.Cells[2, 1].Style.Font.Bold = true;
                //string DateCellFormat = "dd-mm-yyyy";

                workSheet.Cells[4, 1].Value = "Store Code";
                workSheet.Cells[4, 2].Value = "Age 0-5";
                workSheet.Cells[4, 3].Value = "Age 6-10";
                workSheet.Cells[4, 4].Value = "Age 11-16";
                workSheet.Cells[4, 5].Value = "Age >16";
                using (ExcelRange Rng = workSheet.Cells["A4:AL4"])
                {
                    Rng.Style.Font.Bold = true;
                }
                workSheet.Cells[5, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AgeingReport.xlsx");
            }
        }
        public ActionResult AgeingDetailReport(List<string> SelectedDC, string SelectedStores, List<string> SelectedSKUs, string ReportType, string Age, string vAgeRange, string FromDate, string ToDate)//DateTime? FromDate, DateTime? ToDate, 
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            DateTime now = DateTime.Now;

            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));

            int PDayFrom = 0;
            int PdayTo = 0;
            if (vAgeRange == "1")
            {
                PDayFrom = 0;
                PdayTo = 5;
            }
            if (vAgeRange == "2")
            {
                PDayFrom = 6;
                PdayTo = 10;
            }
            if (vAgeRange == "3")
            {
                PDayFrom = 11;
                PdayTo = 15;
            }
            if (vAgeRange == "4")
            {
                PDayFrom = 16;
                PdayTo = 1000;
            }
            List<AgeingDetailReport> finilised = _reportService.AgeingDetailReport(startDate, endDate, custid, userid, SelectedDC, SelectedStores, SelectedSKUs, PDayFrom, PdayTo, Convert.ToInt32(ReportType));
            ViewBag.AgeingDetailReportList = finilised.ToList();

            return PartialView("_AgeingDetailReportPartial");
        }

        #endregion

        #region "Daily Order Report By Deepa 18/11/2019"

        [AdminAuthorization]
        public ActionResult DOR()
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            //ItemCategories
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            //ItemType
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
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
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", null);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", null);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DOR(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report");
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedBrands == null || SelectedBrands.Count() == 0 || SelectedBrands[0] == 0)
                SelectedBrands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report").Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();

            //List<DailyOrderReport> finilised = _reportService.DailyOrderReport(startDate, endDate, custid, userid, SelectedStores, SelectedSKUs);
            List<DailyOrderReport> finilised = _reportService.DailyOrderReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, SelectedLocations, SelectedTypes, SelectedCategories, SelectedBrands);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.FinalizedReports = finilised.ToList();
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", SelectedBrands);
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DORExportExcel(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report");
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedBrands == null || SelectedBrands.Count() == 0 || SelectedBrands[0] == 0)
                SelectedBrands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report").Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();

            List<DailyOrderReport> finilised = _reportService.DailyOrderReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, SelectedLocations, SelectedTypes, SelectedCategories, SelectedBrands);
            string Title = "Name: Daily Order Report";
            string Message = "Store Order Date: From: " + startDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " To: " + endDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "";
            var finilisedList = finilised.Select(head => new
            {
                head.PONumber,
                head.Unique_Reference_Id,
                Store_Order_Date = Convert.ToDateTime(head.Store_Order_Date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),

                head.EnterpriseCode,
                head.EnterpriseName,
                head.Store_Code,
                head.StoreName,
                head.City,
                head.DCName,
                head.PlaceOfSupply,

                head.Item_Code,
                head.item_desc,
                head.ItemCategoryType,
                head.Item_Type,
                head.Brand,
                head.UnitofMasureDescription,
                head.Case_Size,

                head.Original_Ordered_Qty,
                head.Revised_Ordered_Qty,
                head.Ordered_By,
                UploadedDate = Convert.ToDateTime(head.Upload_Date).ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                head.Status
            });
            //ExportToExcel(finilised);
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].Value = Title;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Cells[2, 1].Value = Message;
                workSheet.Cells[2, 1].Style.Font.Bold = true;
                string DateCellFormat = "dd/mm/yyyy";

                workSheet.Cells[4, 1].Value = "PO Number";
                workSheet.Cells[4, 2].Value = "Unique Reference ID";
                workSheet.Cells[4, 3].Value = "Store Order Date";

                workSheet.Cells[4, 4].Value = "Enterprise Code";
                workSheet.Cells[4, 5].Value = "Enterprise Name";
                workSheet.Cells[4, 6].Value = "Store Code";
                workSheet.Cells[4, 7].Value = "Store Name";
                workSheet.Cells[4, 8].Value = "City";
                workSheet.Cells[4, 9].Value = "DC";
                workSheet.Cells[4, 10].Value = "Place of Supply";

                workSheet.Cells[4, 11].Value = "SKU Code";
                workSheet.Cells[4, 12].Value = "SKU Decsription";
                workSheet.Cells[4, 13].Value = "SKU Category Type";
                workSheet.Cells[4, 14].Value = "SKU Category";
                workSheet.Cells[4, 15].Value = "Brand";
                workSheet.Cells[4, 16].Value = "UOM";
                workSheet.Cells[4, 17].Value = "Case Size";

                workSheet.Cells[4, 18].Value = "Original Order Quantity";
                workSheet.Cells[4, 19].Value = "Revised Order Quantity";
                workSheet.Cells[4, 20].Value = "Ordered By";
                //workSheet.Cells[4, 12].Value = "Dc";
                workSheet.Cells[4, 21].Value = "System DateTime";
                workSheet.Cells[4, 22].Value = "Status";

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
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Daily Order Report.xlsx");
            }
        }
        #endregion

        #region StoreReports
        [CustomAuthorization]
        public ActionResult DSRStore()
        {
            int userid = SessionValues.UserId;
            int StoreId = SessionValues.StoreId;
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
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");

            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);

            //ItemCategories
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid, StoreId);
            //ItemType
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid, StoreId);
            List<Brand> Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report", StoreId);

            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report", StoreId);
            DateTime now = DateTime.Now;
            var startDate = now.AddDays(-1).Date;
            var endDate = startDate.AddDays(1).Date;
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = null;
            //int userid = SessionValues.UserId;
            var selectedDCs = DCs.Select(x => x.Id).ToList();
            var selectedStores = Stores.Select(x => x.Id).ToList();
            var SelectedSKUs = Items.Select(x => x.Id).ToList();
            //var finilised = _reportService.DailySalesReport(startDate, endDate, custid, userid, null, selectedDCs, selectedStores, SelectedSKUs);
            //ViewBag.FinalizedReports = finilised.ToList();
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", null);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DSRStore(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            int userid = SessionValues.UserId;
            int custid = 0;
            int storeid = SessionValues.StoreId;
            if (SessionValues.LoggedInCustId != null)
            {
                custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            }
            if (SessionValues.StoreId != 0)
            {
                storeid = SessionValues.StoreId;
                custid = _storeService.GetStoreById(storeid).CustId;
            }
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report", storeid);
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);

            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid, storeid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid, storeid);
            List<Brand> Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report", storeid);

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
            SelectedLocations = SelectedLocations != null && SelectedLocations.Count() > 0 ? SelectedLocations : Locations.Select(x => x.LocationId).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes, storeid).Select(x => x.Id).ToList();

            if (SelectedBrands == null || SelectedBrands.Count() == 0 || SelectedBrands[0] == 0)
                SelectedBrands = Brands.Select(x => x.Id).ToList();

            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report", storeid).Select(x => x.Id).ToList();

            //List<DailySalesReport> finilised = _reportService.DailySalesReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, SelectedLocations, SelectedTypes, SelectedCategories);
            List<DailySalesReport> finilised = _reportService.DailySalesBasicReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, SelectedLocations, SelectedTypes, SelectedCategories, SelectedBrands);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", SelectedBrands);
            ViewBag.FinalizedReports = finilised.ToList();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DSRStoreExportExcel(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            int userid = SessionValues.UserId;
            int custid = 0;
            int storeid = SessionValues.StoreId;
            if (SessionValues.LoggedInCustId != null)
            {
                custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            }
            if (SessionValues.StoreId != 0)
            {
                storeid = SessionValues.StoreId;
                custid = _storeService.GetStoreById(storeid).CustId;
            }
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report", storeid);
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);

            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid, storeid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid, storeid);
            List<Brand> Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report", storeid);

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
            SelectedLocations = SelectedLocations != null && SelectedLocations.Count() > 0 ? SelectedLocations : Locations.Select(x => x.LocationId).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes, storeid).Select(x => x.Id).ToList();
            if (SelectedBrands == null || SelectedBrands.Count() == 0 || SelectedBrands[0] == 0)
                SelectedBrands = Brands.Select(x => x.Id).ToList();

            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report", storeid).Select(x => x.Id).ToList();

            List<DailySalesReport> finilised = _reportService.DailySalesBasicReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, SelectedLocations, SelectedTypes, SelectedCategories, SelectedBrands);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", SelectedBrands);
            //string Title = "Name: Daily Sales Report";
            //string Message = "Store Order Date: From: " + startDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " To: " + endDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "";
            var finilisedList = finilised.Select(head => new
            {
                head.PONumber,
                head.Unique_Reference_Id,

                Store_Order_Date = Convert.ToDateTime(head.Store_Order_Date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                UploadedDate = Convert.ToDateTime(head.Upload_Date).ToString("dd/MM/yyyy hh:mm", CultureInfo.InvariantCulture),
                head.Order_Type,

                head.EnterpriseCode,
                head.EnterpriseName,
                head.Store_Code,
                head.StoreName,
                head.City,
                head.PlaceOfSupply,

                head.Item_Code,
                head.Item_Desc,
                head.ItemCategoryType,
                head.Item_Type,
                head.BrandName,
                head.UnitofMasureDescription,
                head.Case_Size,

                head.Original_Ordered_Qty,
                head.Revised_Ordered_Qty,

                head.OrderNo,
                PODate = head.PODate.HasValue ? Convert.ToDateTime(head.PODate.Value).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                head.Rfpl_Ordered_Qty,
                head.Order_Status,
                head.Dc_Name,

                head.SoNumber,
                head.SoQuantity,

                head.Invoice_Number,
                head.Invoice_Date,
                head.Invoice_Quantity,

                head.RAQuantity,
                head.RAAmount,

                head.FillRateGap,

                head.Taken_Days,
                head.Slab
            });
            //ExportToExcel(finilised);
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                string DateCellFormat = "dd/mm/yyyy";
                workSheet.Cells[1, 1].Value = "PO Number";
                workSheet.Cells[1, 2].Value = "Unique Reference ID";

                workSheet.Cells[1, 3].Value = "Store Order Date";
                workSheet.Cells[1, 4].Value = "System DateTime";
                workSheet.Cells[1, 5].Value = "Order Type";

                workSheet.Cells[1, 6].Value = "Enterprise Code";
                workSheet.Cells[1, 7].Value = "Enterprise Name";
                workSheet.Cells[1, 8].Value = "Store Code";
                workSheet.Cells[1, 9].Value = "Store Name";
                workSheet.Cells[1, 10].Value = "City";
                workSheet.Cells[1, 11].Value = "Place of Supply";

                workSheet.Cells[1, 12].Value = "SKU Code";
                workSheet.Cells[1, 13].Value = "SKU Description";
                workSheet.Cells[1, 14].Value = "SKU Category Type";
                workSheet.Cells[1, 15].Value = "SKU Category";
                workSheet.Cells[1, 16].Value = "Brand";
                workSheet.Cells[1, 17].Value = "UOM";
                workSheet.Cells[1, 18].Value = "Case Size";

                workSheet.Cells[1, 19].Value = "Original Order Quantity";
                workSheet.Cells[1, 20].Value = "Revised Order Quantity";

                workSheet.Cells[1, 21].Value = "PO Number";
                workSheet.Cells[1, 22].Value = "PO Date";
                workSheet.Cells[1, 23].Value = "Rfpl Order Quantity";
                workSheet.Cells[1, 24].Value = "Order Status";
                workSheet.Cells[1, 25].Value = "DC";

                workSheet.Cells[1, 26].Value = "So Number";
                workSheet.Cells[1, 27].Value = "So Quantity";

                workSheet.Cells[1, 28].Value = "Invoice Number";
                workSheet.Cells[1, 29].Value = "Invoice Date";
                workSheet.Cells[1, 30].Value = "Invoice Quantity";
                workSheet.Cells[1, 31].Value = "Invoice Amount";

                workSheet.Cells[1, 32].Value = "RA Quantity";
                workSheet.Cells[1, 33].Value = "RA Amount";

                workSheet.Cells[1, 34].Value = "Fill Rate Gap";

                workSheet.Cells[1, 35].Value = "Taken Days";
                workSheet.Cells[1, 36].Value = "Slab";
                using (ExcelRange Rng = workSheet.Cells["A1:AN1"])
                {
                    Rng.Style.Font.Bold = true;
                }
                workSheet.Cells[2, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                using (ExcelRange Rng = workSheet.Cells["C:C"])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }
                using (ExcelRange Rng = workSheet.Cells["E:E"])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Daily Sales Report.xlsx");
            }
        }
        [CustomAuthorization]
        public ActionResult FillRateReportStore()
        {
            int userid = SessionValues.UserId;
            int custid = 0;
            int storeid = 0;
            if (SessionValues.LoggedInCustId != null)
            {
                custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            }
            if (SessionValues.StoreId != 0)
            {
                storeid = SessionValues.StoreId;
                custid = _storeService.GetStoreById(storeid).CustId;
            }
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report", storeid);
            //ItemCategories
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid, storeid);
            //ItemType
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid, storeid);
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

            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", null);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FillRateReportStore(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, int ReportType, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories)
        {
            int userid = SessionValues.UserId;
            int custid = 0;
            int storeid = 0;
            if (SessionValues.LoggedInCustId != null)
            {
                custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            }
            if (SessionValues.StoreId != 0)
            {
                storeid = SessionValues.StoreId;
                custid = _storeService.GetStoreById(storeid).CustId;
            }
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report", storeid);
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = UniqueReferenceID;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid, storeid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid, storeid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations, storeid).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes, storeid).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report", storeid).Select(x => x.Id).ToList();

            List<Report2> finilised = _reportService.Report2(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, ReportType, SelectedLocations, SelectedTypes, SelectedCategories);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.FinalizedReports = finilised.ToList();
            ViewBag.ReportType = ReportType;
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult FillRateReportStoreExportExcel(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, int ReportType, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = UniqueReferenceID;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();

            List<Report2> finilised = _reportService.Report2(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, ReportType, SelectedLocations, SelectedTypes, SelectedCategories);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.FinalizedReports = finilised.ToList();
            ViewBag.ReportType = ReportType;
            if (ReportType == 1 || ReportType == 5)
            {
                string Title = "Name: City - Order - Wise Report";
                if (ReportType == 5)
                    Title = "Name: City - Order - Wise Report- New";
                string Message = "Store Order Date: From: " + startDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " To: " + endDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "";
                var finilisedList = finilised.Select(head => new
                {
                    head.city,
                    head.OYO_Orders,
                    head.rfpl_correct_fullfilled_orders,
                    head.Correct_Fulfilment,
                    head.partial_orders,
                    head.Partial_Fulfilment,
                    head.pending_orders,
                    head.Pending
                });
                using (var excel = new ExcelPackage())
                {
                    var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells[1, 1].Value = Title;
                    workSheet.Cells[1, 1].Style.Font.Bold = true;
                    workSheet.Cells[2, 1].Value = Message;
                    workSheet.Cells[2, 1].Style.Font.Bold = true;
                    //string DateCellFormat = "dd-mm-yyyy";
                    if (ReportType == 1)
                    {
                        workSheet.Cells[4, 1].Value = "City";
                        workSheet.Cells[4, 2].Value = "No Of Lines";
                        workSheet.Cells[4, 3].Value = "Correct Fullfilment Lines";
                        workSheet.Cells[4, 4].Value = "Correct Fulfilment%";
                        workSheet.Cells[4, 5].Value = "Partial Fulfilment Lines";
                        workSheet.Cells[4, 6].Value = "Partial Fulfilment%";
                        workSheet.Cells[4, 7].Value = "Pending Fulfilment Lines";
                        workSheet.Cells[4, 8].Value = "Pending Fulfilment%";

                    }
                    else
                    {
                        workSheet.Cells[4, 1].Value = "City";
                        workSheet.Cells[4, 2].Value = "No Of Orders";
                        workSheet.Cells[4, 3].Value = "Correct Fullfilment Orders";
                        workSheet.Cells[4, 4].Value = "Correct Fulfilment%";
                        workSheet.Cells[4, 5].Value = "Partial Fulfilment Orders";
                        workSheet.Cells[4, 6].Value = "Partial Fulfilment%";
                        workSheet.Cells[4, 7].Value = "Pending Fulfilment Orders";
                        workSheet.Cells[4, 8].Value = "Pending Fulfilment%";
                    }

                    using (ExcelRange Rng = workSheet.Cells["A4:AL4"])
                    {
                        Rng.Style.Font.Bold = true;
                    }
                    workSheet.Cells[5, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                    workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                    return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FillRateReport.xlsx");
                }
            }
            else if (ReportType == 2 || ReportType == 6)
            {
                string Title = "Name: City - Order - Quantity Report";
                if (ReportType == 6)
                    Title = "Name: City - Order - Quantity Report New";
                string Message = "Store Order Date: From: " + startDate.ToString("dd-MM-yyyy") + " To: " + endDate.ToString("dd-MM-yyyy") + "";
                var finilisedList = finilised.Select(head => new
                {
                    head.city,
                    head.oyo_order_qty,
                    head.rfpl_order_qty,
                    head.Correct_Fulfilment,
                    head.partial_order_qty,
                    head.Partial_Fulfilment,
                    head.pending_order_qty,
                    head.Pending
                });
                using (var excel = new ExcelPackage())
                {
                    var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells[1, 1].Value = Title;
                    workSheet.Cells[1, 1].Style.Font.Bold = true;
                    workSheet.Cells[2, 1].Value = Message;
                    workSheet.Cells[2, 1].Style.Font.Bold = true;
                    //string DateCellFormat = "dd-mm-yyyy";
                    workSheet.Cells[4, 1].Value = "City";
                    workSheet.Cells[4, 2].Value = "Total Quantity";
                    workSheet.Cells[4, 3].Value = "Correct Fulfilment Quantity";
                    workSheet.Cells[4, 4].Value = "Correct Fulfilment%";
                    workSheet.Cells[4, 5].Value = "Partial Fulfilment Quantity";
                    workSheet.Cells[4, 6].Value = "Partial Fulfilment%";

                    workSheet.Cells[4, 7].Value = "Pending fulfilment Quantity";
                    workSheet.Cells[4, 8].Value = "Pending Fulfilment%";
                    using (ExcelRange Rng = workSheet.Cells["A4:AL4"])
                    {
                        Rng.Style.Font.Bold = true;
                    }
                    workSheet.Cells[5, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                    workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                    return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FillRateReport.xlsx");
                }
            }
            else if (ReportType == 3 || ReportType == 7)
            {
                string Title = "Name: Item - SubType - Order";
                if (ReportType == 7)
                    Title = "Name: Item - SubType - Order New";
                string Message = "Store Order Date: From: " + startDate.ToString("dd-MM-yyyy") + " To: " + endDate.ToString("dd-MM-yyyy") + "";
                var finilisedList = finilised.Select(head => new
                {
                    head.Item_Sub_Type,
                    head.OYO_Orders,
                    head.rfpl_correct_fullfilled_orders,
                    head.Correct_Fulfilment,
                    head.partial_orders,
                    head.Partial_Fulfilment,
                    head.pending_orders,
                    head.Pending
                });
                using (var excel = new ExcelPackage())
                {
                    var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells[1, 1].Value = Title;
                    workSheet.Cells[1, 1].Style.Font.Bold = true;
                    workSheet.Cells[2, 1].Value = Message;
                    workSheet.Cells[2, 1].Style.Font.Bold = true;
                    //string DateCellFormat = "dd-mm-yyyy";
                    if (ReportType == 3)
                    {
                        workSheet.Cells[4, 1].Value = "Item Sub Type";
                        workSheet.Cells[4, 2].Value = "No of Lines";
                        workSheet.Cells[4, 3].Value = "Correct Fulfilment Lines";
                        workSheet.Cells[4, 4].Value = "Correct Fulfilment%";
                        workSheet.Cells[4, 5].Value = "Partial Fulfilment Lines";
                        workSheet.Cells[4, 6].Value = "Partial Fulfilment%";
                        workSheet.Cells[4, 7].Value = "Pending Fulfilment Lines";
                        workSheet.Cells[4, 8].Value = "Pending Fulfilment%";
                    }
                    else
                    {
                        workSheet.Cells[4, 1].Value = "Item Sub Type";
                        workSheet.Cells[4, 2].Value = "No of Orders";
                        workSheet.Cells[4, 3].Value = "Correct Fulfilment Orders";
                        workSheet.Cells[4, 4].Value = "Correct Fulfilment%";
                        workSheet.Cells[4, 5].Value = "Partial Fulfilment Orders";
                        workSheet.Cells[4, 6].Value = "Partial Fulfilment%";
                        workSheet.Cells[4, 7].Value = "Pending Fulfilment Orders";
                        workSheet.Cells[4, 8].Value = "Pending Fulfilment%";
                    }

                    using (ExcelRange Rng = workSheet.Cells["A4:AL4"])
                    {
                        Rng.Style.Font.Bold = true;
                    }
                    workSheet.Cells[5, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                    workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                    return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FillRateReport.xlsx");
                }
            }
            else
            {
                string Title = "Name: Item - SubType - Quantity";
                if (ReportType == 8)
                    Title = "Name: Item - SubType - Quantity New";
                string Message = "Store Order Date: From: " + startDate.ToString("dd-MM-yyyy") + " To: " + endDate.ToString("dd-MM-yyyy") + "";
                var finilisedList = finilised.Select(head => new
                {
                    head.Item_Sub_Type,
                    head.oyo_order_qty,
                    head.rfpl_order_qty,
                    head.Correct_Fulfilment,
                    head.partial_order_qty,
                    head.Partial_Fulfilment,
                    head.pending_order_qty,
                    head.Pending
                });
                using (var excel = new ExcelPackage())
                {
                    var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells[1, 1].Value = Title;
                    workSheet.Cells[1, 1].Style.Font.Bold = true;
                    workSheet.Cells[2, 1].Value = Message;
                    workSheet.Cells[2, 1].Style.Font.Bold = true;
                    //string DateCellFormat = "dd-mm-yyyy";
                    workSheet.Cells[4, 1].Value = "Item Sub Type";
                    workSheet.Cells[4, 2].Value = "Total Quantity";
                    workSheet.Cells[4, 3].Value = "Total Fulfilment Quantity%";
                    workSheet.Cells[4, 4].Value = "Correct Fulfilment";
                    workSheet.Cells[4, 5].Value = "Partial Fulfilment Quantity";
                    workSheet.Cells[4, 6].Value = "Partial Fulfilment%";
                    workSheet.Cells[4, 7].Value = "Pending Fulfilment Quantity";
                    workSheet.Cells[4, 8].Value = "Pending Fulfilment";
                    using (ExcelRange Rng = workSheet.Cells["A4:AL4"])
                    {
                        Rng.Style.Font.Bold = true;
                    }
                    workSheet.Cells[5, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);

                    workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                    return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FillRateReport.xlsx");
                }
            }

        }

        [CustomAuthorization]
        public ActionResult PendencyOrderStoreReport()
        {
            int userid = SessionValues.UserId;
            int custid = 0;
            int storeid = 0;
            if (SessionValues.LoggedInCustId != null)
            {
                custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            }
            if (SessionValues.StoreId != 0)
            {
                storeid = SessionValues.StoreId;
                custid = _storeService.GetStoreById(storeid).CustId;
            }
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report", storeid);
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            //ItemCategories
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid, storeid);
            //ItemType
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid, storeid);

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
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", null);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);

            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult PendencyOrderStoreReport(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, String ReportType, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories)
        {
            int userid = SessionValues.UserId;
            int custid = 0;
            int storeid = 0;
            if (SessionValues.LoggedInCustId != null)
            {
                custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            }
            if (SessionValues.StoreId != 0)
            {
                storeid = SessionValues.StoreId;
                custid = _storeService.GetStoreById(storeid).CustId;
            }
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report", storeid);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = UniqueReferenceID;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid, storeid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid, storeid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations, storeid).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes, storeid).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report", storeid).Select(x => x.Id).ToList();
            List<PendencyOrderReport> finilised = _reportService.PendencyOrderReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, ReportType, SelectedLocations, SelectedTypes, SelectedCategories);

            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);

            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);

            ViewBag.FinalizedReports = finilised.ToList();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult PendencyOrderStoreReportExportExcel(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, String ReportType, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories)
        {
            int userid = SessionValues.UserId;
            int custid = 0;
            int storeid = 0;
            if (SessionValues.LoggedInCustId != null)
            {
                custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            }
            if (SessionValues.StoreId != 0)
            {
                storeid = SessionValues.StoreId;
                custid = _storeService.GetStoreById(storeid).CustId;
            }
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report", storeid);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = UniqueReferenceID;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid, storeid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid, storeid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations, storeid).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes, storeid).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report", storeid).Select(x => x.Id).ToList();
            List<PendencyOrderReport> finilised = _reportService.PendencyOrderReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, ReportType, SelectedLocations, SelectedTypes, SelectedCategories);

            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);

            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            //var finilised = _reportService.PendencyOrderReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, ReportType);
            string Title = "Name: Pendency Order Report";
            string Message = "Store Order Date: From: " + startDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " To: " + endDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "";
            List<PendencyOrderReportExportToExcel> finilisedList = finilised.Select(head => new PendencyOrderReportExportToExcel
            {
                PONumber = head.PONumber,
                Unique_Reference_Id = head.Unique_Reference_Id,
                DateOfOrder = Convert.ToDateTime(head.DateOfOrder).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                KitchenId = head.KitchenId,
                CityName = head.CityName,
                EXCode = head.EXCode,
                OrderQuantity = Convert.ToDecimal(head.OrderQuantity),
                DCCode = head.DCCode,
                Status = head.Status,
                PendingDay = head.PendingDay,
                Slab = head.Slab,
                InvoiceQuantity = head.InvoiceQuantity
            }).ToList();
            //ExportToExcel(finilised);
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].Value = Title;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Cells[2, 1].Value = Message;
                workSheet.Cells[2, 1].Style.Font.Bold = true;
                string DateCellFormat = "dd-mm-yyyy";
                workSheet.Cells[4, 1].Value = "PO Number";
                workSheet.Cells[4, 2].Value = "Unique Reference ID";
                workSheet.Cells[4, 3].Value = "Store Order Date";
                workSheet.Cells[4, 4].Value = "Store Code";
                workSheet.Cells[4, 5].Value = "City";
                workSheet.Cells[4, 6].Value = "SKU Code";
                workSheet.Cells[4, 9].Value = "Ordered Quantity";
                workSheet.Cells[4, 10].Value = "From DC";
                workSheet.Cells[4, 11].Value = "Status";
                workSheet.Cells[4, 12].Value = "Pending Days";
                workSheet.Cells[4, 13].Value = "Slab";
                workSheet.Cells[4, 14].Value = "Invoice Quantity";
                using (ExcelRange Rng = workSheet.Cells["A4:AL4"])
                {
                    Rng.Style.Font.Bold = true;
                }
                workSheet.Cells[5, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                using (ExcelRange Rng = workSheet.Cells["B:B"])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }
                using (ExcelRange Rng = workSheet.Cells["F:F"])
                {
                    Rng.Style.Numberformat.Format = "#";
                }
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PendencyOrderReport.xlsx");
            }
        }

        [CustomAuthorization]
        public ActionResult DeliveryStatusStoreReport()
        {
            int userid = SessionValues.UserId;
            int custid = 0;
            int storeid = 0;
            if (SessionValues.LoggedInCustId != null)
            {
                custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            }
            if (SessionValues.StoreId != 0)
            {
                storeid = SessionValues.StoreId;
                custid = _storeService.GetStoreById(storeid).CustId;
            }

            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report", storeid);
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            //ItemCategories
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid, storeid);
            //ItemType
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid, storeid);
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
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", null);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DeliveryStatusStoreReport(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, int ReportType, string InvoiceNo, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories)
        {
            int userid = SessionValues.UserId;
            int custid = 0;
            int storeid = 0;
            if (SessionValues.LoggedInCustId != null)
            {
                custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            }
            if (SessionValues.StoreId != 0)
            {
                storeid = SessionValues.StoreId;
                custid = _storeService.GetStoreById(storeid).CustId;
            }
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report", storeid);
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = UniqueReferenceID;
            ViewData["InvoiceNO"] = InvoiceNo;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();
            var finilised = _reportService.DeliveryStatusReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, ReportType, InvoiceNo, SelectedLocations, SelectedTypes, SelectedCategories);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            if (InvoiceNo != "")
            {

                finilised = finilised.Where(a => a.InvoiceNumber.Length > 0).ToList();
                finilised = finilised.Where(a => a.InvoiceNumber.Contains(InvoiceNo)).ToList();
            }
            ViewBag.FinalizedReports = finilised.ToList();
            ViewBag.ReportType = ReportType;
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DeliveryStatusStoreReportExportExcel(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, int ReportType, string InvoiceNo, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories)
        {
            int userid = SessionValues.UserId;
            string DateCellFormat = "dd-mm-yyyy";
            int custid = 0;
            int storeid = 0;
            if (SessionValues.LoggedInCustId != null)
            {
                custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            }
            if (SessionValues.StoreId != 0)
            {
                storeid = SessionValues.StoreId;
                custid = _storeService.GetStoreById(storeid).CustId;
            }
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report", storeid);
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = UniqueReferenceID;
            ViewData["InvoiceNO"] = InvoiceNo;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();
            var finilised = _reportService.DeliveryStatusReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, ReportType, InvoiceNo, SelectedLocations, SelectedTypes, SelectedCategories);
            if (InvoiceNo != "")
            {
                finilised = finilised.Where(a => a.InvoiceNumber.Length > 0).ToList();
                finilised = finilised.Where(a => a.InvoiceNumber.Contains(InvoiceNo)).ToList();
            }
            string Title = "";
            if (ReportType == 1)
            {
                Title = "Name: Invoice Level Delivery Report";
            }
            else
            {
                Title = "Name: Order Level Delivery Report";
            }
            string Message = "Store Order Date: From: " + startDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " To: " + endDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "";
            var finilisedList = finilised.Select(head => new
            {
                head.UniqueReferenceId,
                Store_Order_Date = Convert.ToDateTime(head.Store_Order_Date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                head.InvoiceDate,
                head.DeliveryDate,
                head.DCName,
                head.StoreCode,
                head.City,
                head.VehicleNo,

                head.InvoiceNumber,
                head.DeliveryStatus,
                head.Reason
            });
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].Value = Title;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Cells[2, 1].Value = Message;
                workSheet.Cells[2, 1].Style.Font.Bold = true;
                // string DateCellFormat = "dd-mm-yyyy";
                if (ReportType == 2)
                {
                    workSheet.Cells[4, 1].Value = "Unique Reference Id";
                }
                workSheet.Cells[4, 2].Value = "Store Order Date";
                workSheet.Cells[4, 3].Value = "Dispatch Date";
                workSheet.Cells[4, 4].Value = "Delivery Date";
                workSheet.Cells[4, 5].Value = "From DC";
                workSheet.Cells[4, 6].Value = "Store Code";
                workSheet.Cells[4, 7].Value = "City";
                workSheet.Cells[4, 8].Value = "Vehicle No";

                workSheet.Cells[4, 9].Value = "Invocie Number";
                workSheet.Cells[4, 10].Value = "Delivery Status";
                workSheet.Cells[4, 11].Value = "Remark";
                using (ExcelRange Rng = workSheet.Cells["A4:AL4"])
                {
                    Rng.Style.Font.Bold = true;
                }
                using (ExcelRange Rng = workSheet.Cells["B:B"])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }
                using (ExcelRange Rng = workSheet.Cells["C:C"])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }
                workSheet.Cells[5, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DeliveryStatusReport.xlsx");

            }
        }

        [CustomAuthorization]
        public ActionResult AgeingStoreReport()
        {
            int userid = SessionValues.UserId;
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
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            DateTime now = DateTime.Now;

            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            //ItemCategories
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            //ItemType
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);

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

            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", null);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AgeingStoreReport(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, int ReportType, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories)
        {
            int userid = SessionValues.UserId;
            int custid = 0;
            int storeid = 0;
            if (SessionValues.LoggedInCustId != null)
            {
                custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            }
            if (SessionValues.StoreId != 0)
            {
                storeid = SessionValues.StoreId;
                custid = _storeService.GetStoreById(storeid).CustId;
            }

            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report", storeid);
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            DateTime now = DateTime.Now;
            string DateCellFormat = "dd-mm-yyyy";
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = UniqueReferenceID;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid, storeid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid, storeid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations, storeid).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes, storeid).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report", storeid).Select(x => x.Id).ToList();

            List<AgeingReport> finilised = _reportService.AgeingReport2(startDate, endDate, custid, userid, SelectedDC, SelectedStores, SelectedSKUs, ReportType, SelectedLocations, SelectedTypes, SelectedCategories);
            ViewBag.FinalizedReports = finilised.ToList();
            ViewBag.ReportType = ReportType;
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);

            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AgeingStoreReportExportExcel(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, int ReportType, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories)
        {
            int userid = SessionValues.UserId;
            int custid = 0;
            if (SessionValues.LoggedInCustId != null)
            {
                custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            }
            if (SessionValues.StoreId != 0)
            {
                int storeid = SessionValues.StoreId;
                custid = _storeService.GetStoreById(storeid).CustId;
            }
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
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
            var finilised = _reportService.AgeingReport2(startDate, endDate, custid, userid, SelectedDC, SelectedStores, SelectedSKUs, ReportType, SelectedLocations, SelectedTypes, SelectedCategories);
            string Title = "";
            if (ReportType == 1)
            {
                Title = "Name: Invoice Level Delivery Report";
            }
            if (ReportType == 2)
            {
                Title = "Name: Invoice Level Delivery Report";
            }
            else
            {
                Title = "Name: Order Level Delivery Report";
            }
            string Message = "Store Order Date: From: " + startDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " To: " + endDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "";
            var finilisedList = finilised.Select(head => new
            {
                head.EnterpriseCode,
                head.EnterpriseName,
                head.Store_Code,
                head.StoreName,
                head.A0_5,
                head.A6_10,
                head.A11_15,
                head.Above_16
            });
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].Value = Title;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Cells[2, 1].Value = Message;
                workSheet.Cells[2, 1].Style.Font.Bold = true;
                //string DateCellFormat = "dd-mm-yyyy";

                workSheet.Cells[4, 1].Value = "Enterprise Code";
                workSheet.Cells[4, 2].Value = "Enterprise Name";
                workSheet.Cells[4, 3].Value = "Store Code";
                workSheet.Cells[4, 4].Value = "Store Name";
                workSheet.Cells[4, 5].Value = "Store Code";
                workSheet.Cells[4, 6].Value = "Age 0-5";
                workSheet.Cells[4, 7].Value = "Age 6-10";
                workSheet.Cells[4, 8].Value = "Age 11-16";
                workSheet.Cells[4, 9].Value = "Age >16";
                using (ExcelRange Rng = workSheet.Cells["A4:AL4"])
                {
                    Rng.Style.Font.Bold = true;
                }
                workSheet.Cells[5, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AgeingReport.xlsx");

            }

        }
        public ActionResult AgeingDetailStoreReport(List<string> SelectedDC, string SelectedStores, List<string> SelectedSKUs, string ReportType, string Age, string vAgeRange, string FromDate, string ToDate, List<int> SelectedLocations = null, List<int> SelectedTypes = null, List<int> SelectedCategories = null)//DateTime? FromDate, DateTime? ToDate, 
        {
            int userid = SessionValues.UserId;
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
            DateTime now = DateTime.Now;

            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));

            int PDayFrom = 0;
            int PdayTo = 0;
            if (vAgeRange == "1")
            {
                PDayFrom = 0;
                PdayTo = 5;
            }
            if (vAgeRange == "2")
            {
                PDayFrom = 6;
                PdayTo = 10;
            }
            if (vAgeRange == "3")
            {
                PDayFrom = 11;
                PdayTo = 15;
            }
            if (vAgeRange == "4")
            {
                PDayFrom = 16;
                PdayTo = 1000;
            }
            List<AgeingDetailReport> finilised = _reportService.AgeingDetailReport(startDate, endDate, custid, userid, SelectedDC, SelectedStores, SelectedSKUs, PDayFrom, PdayTo, Convert.ToInt32(ReportType));
            ViewBag.AgeingDetailReportList = finilised.ToList();

            return PartialView("_AgeingDetailReportPartial");
        }
        [CustomAuthorization]
        public ActionResult DORStore()
        {
            int userid = SessionValues.UserId;
            int custid = 0;
            int storeid = 0;
            if (SessionValues.LoggedInCustId != null)
            {
                custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            }
            if (SessionValues.StoreId != 0)
            {
                storeid = SessionValues.StoreId;
                custid = _storeService.GetStoreById(storeid).CustId;
            }

            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report", storeid);
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report", storeid);
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            //ItemCategories
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid, storeid);
            //ItemType
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid, storeid);
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
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", null);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", null);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DORStore(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            int userid = SessionValues.UserId;
            int custid = 0;
            int storeid = 0;
            if (SessionValues.LoggedInCustId != null)
            {
                custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            }
            if (SessionValues.StoreId != 0)
            {
                storeid = SessionValues.StoreId;
                custid = _storeService.GetStoreById(storeid).CustId;
            }
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report", storeid);
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report", storeid);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid, storeid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid, storeid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations, storeid).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes, storeid).Select(x => x.Id).ToList();
            if (SelectedBrands == null || SelectedBrands.Count() == 0 || SelectedBrands[0] == 0)
                SelectedBrands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report", storeid).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report", storeid).Select(x => x.Id).ToList();

            //List<DailyOrderReport> finilised = _reportService.DailyOrderReport(startDate, endDate, custid, userid, SelectedStores, SelectedSKUs);
            List<DailyOrderReport> finilised = _reportService.DailyOrderReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, SelectedLocations, SelectedTypes, SelectedCategories, SelectedBrands);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.FinalizedReports = finilised.ToList();
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", SelectedBrands);
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DORStoreExportExcel(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            int userid = SessionValues.UserId;
            int custid = 0;
            int storeid = 0;
            if (SessionValues.LoggedInCustId != null)
            {
                custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            }
            if (SessionValues.StoreId != 0)
            {
                storeid = SessionValues.StoreId;
                custid = _storeService.GetStoreById(storeid).CustId;
            }
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report", storeid);
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report", storeid);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid, storeid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid, storeid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations, storeid).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes, storeid).Select(x => x.Id).ToList();
            if (SelectedBrands == null || SelectedBrands.Count() == 0 || SelectedBrands[0] == 0)
                SelectedBrands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report", storeid).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report", storeid).Select(x => x.Id).ToList();

            //List<DailyOrderReport> finilised = _reportService.DailyOrderReport(startDate, endDate, custid, userid, SelectedStores, SelectedSKUs);
            List<DailyOrderReport> finilised = _reportService.DailyOrderReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, SelectedLocations, SelectedTypes, SelectedCategories, SelectedBrands);
            string Title = "Name: Daily Order Report";
            string Message = "Store Order Date: From: " + startDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " To: " + endDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "";
            var finilisedList = finilised.Select(head => new
            {
                head.PONumber,
                head.Unique_Reference_Id,
                Store_Order_Date = Convert.ToDateTime(head.Store_Order_Date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),

                head.EnterpriseCode,
                head.EnterpriseName,
                head.Store_Code,
                head.StoreName,
                head.City,
                head.DCName,
                head.PlaceOfSupply,

                head.Item_Code,
                head.item_desc,
                head.ItemCategoryType,
                head.Item_Type,
                head.Brand,
                head.UnitofMasureDescription,
                head.Case_Size,

                head.Original_Ordered_Qty,
                head.Revised_Ordered_Qty,
                head.Ordered_By,
                UploadedDate = Convert.ToDateTime(head.Upload_Date).ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                head.Status
            });
            //ExportToExcel(finilised);
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].Value = Title;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Cells[2, 1].Value = Message;
                workSheet.Cells[2, 1].Style.Font.Bold = true;
                string DateCellFormat = "dd/mm/yyyy";

                workSheet.Cells[4, 1].Value = "PO Number";
                workSheet.Cells[4, 2].Value = "Unique Reference ID";
                workSheet.Cells[4, 3].Value = "Store Order Date";

                workSheet.Cells[4, 4].Value = "Enterprise Code";
                workSheet.Cells[4, 5].Value = "Enterprise Name";
                workSheet.Cells[4, 6].Value = "Store Code";
                workSheet.Cells[4, 7].Value = "Store Name";
                workSheet.Cells[4, 8].Value = "City";
                workSheet.Cells[4, 9].Value = "DC";
                workSheet.Cells[4, 10].Value = "Place of Supply";

                workSheet.Cells[4, 11].Value = "SKU Code";
                workSheet.Cells[4, 12].Value = "SKU Decsription";
                workSheet.Cells[4, 13].Value = "SKU Category Type";
                workSheet.Cells[4, 14].Value = "SKU Category";
                workSheet.Cells[4, 15].Value = "Brand";
                workSheet.Cells[4, 16].Value = "UOM";
                workSheet.Cells[4, 17].Value = "Case Size";

                workSheet.Cells[4, 18].Value = "Original Order Quantity";
                workSheet.Cells[4, 19].Value = "Revised Order Quantity";
                workSheet.Cells[4, 20].Value = "Ordered By";
                //workSheet.Cells[4, 12].Value = "Dc";
                workSheet.Cells[4, 21].Value = "System DateTime";
                workSheet.Cells[4, 22].Value = "Status";

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
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Daily Order Report.xlsx");
            }
        }
        #endregion
        #region "OnTimeReport"
        [AdminAuthorization]
        public ActionResult OnTimeReport()
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var vLocations = _storeService.GetCitiesforUserCustomer(userid, custid);
            DateTime now = DateTime.Now;

            List<LocationView> locations = new List<LocationView>();
            for (int i = 0; i < vLocations.Count(); i++)
            {
                LocationView obj = new LocationView();
                obj.Id = vLocations[i].LocationId;
                obj.Name = vLocations[i].LocationName;
                locations.Add(obj);
            }
            var startDate = now.AddDays(-1).Date;

            var endDate = startDate.AddDays(1).Date;
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["VehicleNo"] = "";

            var selectedDCs = DCs.Select(x => x.Id).ToList();
            var selectedLocations = locations.Select(x => x.Id).ToList();

            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.selectedLocations = new MultiSelectList(locations.ToList(), "Id", "Name", null);
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult OnTimeReport(DateTime? FromDate, DateTime? ToDate, List<int> SelectedDC, List<int> selectedLocations, int ReportType)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");

            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["VehicleNo"] = "";
            var vLocations = _storeService.GetCitiesforUserCustomer(userid, custid);
            List<LocationView> locations = new List<LocationView>();
            for (int i = 0; i < vLocations.Count(); i++)
            {
                LocationView obj = new LocationView();
                obj.Id = vLocations[i].LocationId;
                obj.Name = vLocations[i].LocationName;
                locations.Add(obj);
            }

            SelectedDC = SelectedDC != null && SelectedDC.Count() > 0 ? SelectedDC : DCs.Select(x => x.Id).ToList();
            selectedLocations = selectedLocations != null && selectedLocations.Count() > 0 ? selectedLocations : locations.Select(x => x.Id).ToList();
            List<OnTimeReport> finilised = _reportService.OnTimeReport(startDate, endDate, custid, userid, SelectedDC, selectedLocations, ReportType);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.selectedLocations = new MultiSelectList(locations.ToList(), "Id", "Name", selectedLocations);
            ViewBag.FinalizedReports = finilised.ToList();
            ViewBag.ReportType = ReportType;
            return View();
        }

        public ActionResult OnTimeReportDrillDown(DateTime? FromDate, DateTime? ToDate, int? CityId, int? DCid, int ReportType)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            DateTime now = DateTime.Now;

            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));

            List<OnTimeDetailReport> finilised = _reportService.OnTimeReportDrillDown(startDate, endDate, custid, userid, DCid, CityId, ReportType);
            if (ReportType == 1)
                finilised = finilised.Where(a => a.IsOnTimeDelivery == 1).ToList();
            else
                finilised = finilised.Where(a => a.IsOnTimeDelivery == 0).ToList();

            ViewBag.OnTimeDetailReportList = finilised.ToList();

            return PartialView("_OnTImeReportDrillDown");
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult OnTimeReportExportExcel(DateTime? FromDate, DateTime? ToDate, List<int> SelectedDC, List<int> selectedLocations, int ReportType)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;

            var vLocations = _storeService.GetCitiesforUserCustomer(userid, custid);
            List<LocationView> locations = new List<LocationView>();
            for (int i = 0; i < vLocations.Count(); i++)
            {
                LocationView obj = new LocationView();
                obj.Id = vLocations[i].LocationId;
                obj.Name = vLocations[i].LocationName;
                locations.Add(obj);
            }


            selectedLocations = selectedLocations != null && selectedLocations.Count() > 0 ? selectedLocations : locations.Select(x => x.Id).ToList();

            var finilised = _reportService.OnTimeReport(startDate, endDate, custid, userid, SelectedDC, selectedLocations, ReportType);
            string Title = "";
            if (ReportType == 1)
            {
                Title = "Name: City Wise Ontime Report";
            }
            if (ReportType == 2)
            {
                Title = "Name: DC Wise ontime Report";
            }
            string Message = "Delivery Date: From: " + startDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " To: " + endDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "";
            var finilisedList = finilised.Select(head => new
            {
                Name = ReportType == 1 ? head.CityName : head.DCName,
                //   head.DCName,
                head.OnTimePercentage,
                head.NotOnTimePercentage,
                head.TotalRecords,
                head.OntimeDeliveryCount

            });
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].Value = Title;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Cells[2, 1].Value = Message;
                workSheet.Cells[2, 1].Style.Font.Bold = true;
                //string DateCellFormat = "dd-mm-yyyy";
                if (ReportType == 1)
                {
                    workSheet.Cells[4, 1].Value = "City Name";
                }
                else
                {
                    workSheet.Cells[4, 1].Value = "DC Name";
                }
                workSheet.Cells[4, 2].Value = "OnTime Percentage";
                workSheet.Cells[4, 3].Value = "Delay Percentage";
                workSheet.Cells[4, 4].Value = "Total Records";
                workSheet.Cells[4, 5].Value = "Ontime Delivery Count";
                using (ExcelRange Rng = workSheet.Cells["A4:AL4"])
                {
                    Rng.Style.Font.Bold = true;
                }
                workSheet.Cells[5, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OnTimeReport.xlsx");
            }
        }


        #endregion
        #region Tracking
        public ActionResult TrackingStatusList()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetTrackingList(string action, string uniqueReferenceID, string InvoiceNo, int custid)
        {
            List<TrackingBO> list1 = new List<TrackingBO>();
            if (action == "URID")
            {
                list1 = _reportService.GetTrackingList(action, uniqueReferenceID, InvoiceNo, custid);
                return Json(new { data = list1 }, JsonRequestBehavior.AllowGet);

            }
            if (action == "InvoiceNo")
            {
                list1 = _reportService.GetTrackingList(action, uniqueReferenceID, InvoiceNo, custid);
                return Json(new { data = list1 }, JsonRequestBehavior.AllowGet);
            }
            if (action == "URIDInvoiceNo")
            {
                list1 = _reportService.GetTrackingList(action, uniqueReferenceID, InvoiceNo, custid);
                return Json(new { data = list1 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetStatusList(string uniqueReferenceID, string InvoiceNo, int custid)
        {
            List<TrackingBO> obj = new List<TrackingBO>();
            obj = _reportService.GetStatusList(uniqueReferenceID, InvoiceNo, custid);
            return Json(new { data = obj }, JsonRequestBehavior.AllowGet);
        }
        public string GetInvoiceMonthYear(string InvoiceNo)
        {
            InvoiceHeader objInvoiceHeader = _orderService.GetInvoiceInformation(InvoiceNo);
            DateTime InvoiceDate = DateTime.ParseExact(objInvoiceHeader.InvoiceDate, "dd/MM/yyyy", null);
            string InvoiceMonth = InvoiceDate.ToString("MMM", CultureInfo.InvariantCulture).ToUpper();
            string InvoiceYear = InvoiceDate.Year.ToString();
            return InvoiceMonth + InvoiceYear;
        }

        public ActionResult GetPODImages(string uniqueReferenceID, string InvoiceNo, int custid)
        {
            List<PODImageList> obj = new List<PODImageList>();
            obj = _reportService.GetPODImages(uniqueReferenceID, InvoiceNo, custid);
            string InvoicePath = GetInvoiceMonthYear(InvoiceNo);
            InvoicePath = InvoicePath + '/' + InvoiceNo;
            return Json(new { data = obj, InvoicePath = InvoicePath }, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region StoreTop5SKUs
        public ActionResult GetTop5Skus(int Month, int Year)
        {
            int userid = SessionValues.UserId;
            int custid = 0;
            if (SessionValues.LoggedInCustId != null)
            {
                custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            }
            if (SessionValues.StoreId != 0)
            {
                int storeid = SessionValues.StoreId;
                custid = _storeService.GetStoreById(storeid).CustId;
            }
            List<Top5SKU> responses = _reportService.GetTop5SKUs(custid, userid, Month, Year);
            return Json(responses, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region StoreDayWiseOrderQty
        public ActionResult GetLineGraphDataForSkus(int Month, int Year, List<int> SKUs)
        {
            int userid = SessionValues.UserId;
            int custid = SessionValues.LoggedInCustId.Value;
            int storeid = 0;
            if (SessionValues.StoreId != 0)
            {
                storeid = SessionValues.StoreId;
                custid = _storeService.GetStoreById(storeid).CustId;
            }
            if (SKUs == null || SKUs.Count() == 0 || SKUs[0] == 0)
                SKUs = _itemService.GetSkuForUserCustomer(userid, custid, "Report", storeid).Select(x => x.Id).ToList();
            List<DAYWISEORDERQTY> responses = _reportService.GetStoreDayWiseOrderQty(custid, userid, Month, Year, storeid, SKUs);
            return Json(responses, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region "Fulfilment Report Drilldown"
        [HttpPost]
        public ActionResult FulfillmentReportDrillDown(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, int ReportType, string CityCategory, string ColumnName, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories)
        {
            int userid = SessionValues.UserId;
            int custid = 0;
            int storeid = 0;
            if (SessionValues.LoggedInCustId != null)
            {
                custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            }
            if (SessionValues.StoreId != 0)
            {
                storeid = SessionValues.StoreId;
                custid = _storeService.GetStoreById(storeid).CustId;
            }
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report", storeid);
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = UniqueReferenceID;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid, storeid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid, storeid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations, storeid).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes, storeid).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report", storeid).Select(x => x.Id).ToList();

            List<Report2> finilised = _reportService.FulfillmentReportDrillDown(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, ReportType, CityCategory, SelectedLocations, SelectedTypes, SelectedCategories);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.FinalizedReportsDrildown = finilised.ToList();
            ViewBag.ReportTypeDrilDown = ReportType;
            ViewBag.ColumnName = ColumnName;
            return PartialView("_FulfillmentReportDrillDownPartial");
        }
        #endregion
        #region StoreTOPSKUS_HalfYear
        [HttpPost]
        public ActionResult GetStoreTOPSKUS_HalfYear(int Year, int Type)
        {
            int userid = SessionValues.UserId;
            int custid = SessionValues.LoggedInCustId.Value;
            int storeid = 0;
            if (SessionValues.StoreId != 0)
            {
                storeid = SessionValues.StoreId;
                custid = _storeService.GetStoreById(storeid).CustId;
            }
            List<TOPSKUForYEARMonth> responses = _reportService.GetStoreTOPSKUS_HalfYear(userid, storeid, custid, Year, Type);

            return Json(responses, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region OrderDeleteCloseReport
        [AdminAuthorization]
        public ActionResult OrderDeleteCloseReport()
        {
            int userid = Convert.ToInt32(SessionValues.UserId);
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
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
        [AdminApiAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrderDeleteCloseReport(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedStores, List<int> SelectedSKUs, string Action)
        {
            int userid = Convert.ToInt32(SessionValues.UserId);
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
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
            SelectedStores = SelectedStores != null && SelectedStores.Count() > 0 ? SelectedStores : Stores.Select(x => x.Id).ToList();
            SelectedSKUs = SelectedSKUs != null && SelectedSKUs.Count() > 0 ? SelectedSKUs : Items.Select(x => x.Id).ToList();
            List<OrderDeleteCloseReport> finilised = _reportService.OrderDeleteCloseReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedStores, SelectedSKUs, Action);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.FinalizedReports = finilised.ToList();
            return View();
        }

        [AdminApiAuthorization]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult OrderDeleteCloseReportExportToExcel(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedStores, List<int> SelectedSKUs, String Action)
        {
            int userid = Convert.ToInt32(SessionValues.UserId);
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
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
            SelectedStores = SelectedStores != null && SelectedStores.Count() > 0 ? SelectedStores : Stores.Select(x => x.Id).ToList();
            SelectedSKUs = SelectedSKUs != null && SelectedSKUs.Count() > 0 ? SelectedSKUs : Items.Select(x => x.Id).ToList();
            List<OrderDeleteCloseReport> finilised = _reportService.OrderDeleteCloseReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedStores, SelectedSKUs, Action);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.FinalizedReports = finilised.ToList(); string Title = "";
            if (Action == "1")
            {
                Title = "Name: Order Close Report";
            }
            if (Action == "2")
            {
                Title = "Name: Order Delete Report";
            }
            string Message = "Date: From: " + startDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " To: " + endDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "";
            var finilisedList = finilised.Select(head => new
            {
                head.Unique_Reference_Id,
                DateOfOrderStr = head.DateOfOrder.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                head.StoreCode,
                head.SKUCode,
                head.OrderQuantity,
                head.Reason
            });
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].Value = Title;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Cells[2, 1].Value = Message;
                workSheet.Cells[2, 1].Style.Font.Bold = true;
                workSheet.Cells[4, 1].Value = "Unique Reference ID";
                workSheet.Cells[4, 2].Value = "Date of Order";
                workSheet.Cells[4, 3].Value = "Store Code";
                workSheet.Cells[4, 4].Value = "SKU Code";
                workSheet.Cells[4, 5].Value = "Order Quantity";
                workSheet.Cells[4, 6].Value = "Reason";

                //string DateCellFormat = "dd-mm-yyyy";                
                using (ExcelRange Rng = workSheet.Cells["A4:AL4"])
                {
                    Rng.Style.Font.Bold = true;
                }
                workSheet.Cells[5, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OrderDeleteCloseReport.xlsx");
            }
        }
        #endregion
        #region StoreClosingStock
        public ActionResult ClosingStockStore()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClosingStockStore(List<int> SelectedStores, List<int> SelectedSKUs)
        {
            int userid = SessionValues.UserId;
            int custid = 0;
            int storeid = SessionValues.StoreId;
            if (SessionValues.LoggedInCustId != null)
            {
                custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            }
            if (SessionValues.StoreId != 0)
            {
                storeid = SessionValues.StoreId;
                custid = _storeService.GetStoreById(storeid).CustId;
            }
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report", storeid);

            SelectedStores = SelectedStores != null && SelectedStores.Count() > 0 ? SelectedStores : Stores.Select(x => x.Id).ToList();
            SelectedSKUs = SelectedSKUs != null && SelectedSKUs.Count() > 0 ? SelectedSKUs : Items.Select(x => x.Id).ToList();
            List<ClosingStock> finilised = _reportService.ClosingStockReport(custid, userid, SelectedStores, SelectedSKUs);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.FinalizedReports = finilised.ToList();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ClosingStockExportExcel(List<int> SelectedStores, List<int> SelectedSKUs)
        {
            int userid = SessionValues.UserId;
            int custid = 0;
            int storeid = SessionValues.StoreId;
            if (SessionValues.LoggedInCustId != null)
            {
                custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            }
            if (SessionValues.StoreId != 0)
            {
                storeid = SessionValues.StoreId;
                custid = _storeService.GetStoreById(storeid).CustId;
            }
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report", storeid);

            SelectedStores = SelectedStores != null && SelectedStores.Count() > 0 ? SelectedStores : Stores.Select(x => x.Id).ToList();
            SelectedSKUs = SelectedSKUs != null && SelectedSKUs.Count() > 0 ? SelectedSKUs : Items.Select(x => x.Id).ToList();
            List<ClosingStock> finilised = _reportService.ClosingStockReport(custid, userid, SelectedStores, SelectedSKUs);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            string Title = "Name: Closing Stock Report";
            var finilisedList = finilised.Select(head => new
            {
                head.ORG_ID,
                head.ITEM_NUMBER,
                head.STOCK_QUANTITY,
                head.ISSUE_QUANTITY,
                REPORT_DATE = Convert.ToDateTime(head.REPORT_DATE).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
            });
            //ExportToExcel(finilised);
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].Value = Title;
                workSheet.Cells[1, 1].Style.Font.Bold = true;

                string DateCellFormat = "dd/mm/yyyy";
                workSheet.Cells[4, 1].Value = "StoreCode";
                workSheet.Cells[4, 2].Value = "SKU Code";

                workSheet.Cells[4, 3].Value = "Stock Quantity";
                workSheet.Cells[4, 4].Value = "Issue Quantity";

                workSheet.Cells[4, 5].Value = "Report Date";
                workSheet.Cells[1, 1, 1, 40].Style.Font.Bold = true;
                using (ExcelRange Rng = workSheet.Cells["A4:AN4"])
                {
                    Rng.Style.Font.Bold = true;
                }
                workSheet.Cells[5, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                using (ExcelRange Rng = workSheet.Cells["E:E"])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();

                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Daily Sales Report.xlsx");
            }
        }

        #endregion

        #region dashboards Widgets
        public ActionResult TotalSalesValue()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult GetTotalSalesValue(List<int> SelectedDc)
        {
            int UserId = SessionValues.UserId;
            int CustId = SessionValues.LoggedInCustId.Value;
            int storeid = 0;
            int Year = DateTime.Now.Year;
            if (SessionValues.StoreId != 0 || SessionValues.RoleId == 1)
            {
                storeid = SessionValues.StoreId;
                CustId = _storeService.GetStoreById(storeid).CustId;
            }
            var Stores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Report");
            var selectedStores = Stores.Select(x => x.Id).ToList();
            List<MonthWiseTotalSalesValue> responses = _reportService.GetTotalSalesValue(UserId, CustId, SelectedDc, selectedStores, Year);
            return Json(responses, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TotalSalesAmount()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult GetTotalSalesAmount(List<int> SelectedDc)
        {
            int UserId = SessionValues.UserId;
            int CustId = SessionValues.LoggedInCustId.Value;
            int storeid = 0;
            int Year = DateTime.Now.Year;
            if (SessionValues.StoreId != 0 || SessionValues.RoleId == 1)
            {
                storeid = SessionValues.StoreId;
                CustId = _storeService.GetStoreById(storeid).CustId;
            }
            var Stores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Report");
            var selectedStores = Stores.Select(x => x.Id).ToList();
            List<MonthWiseTotalSalesValue> responses = _reportService.GetTotalSalesAmount(UserId, CustId, SelectedDc, selectedStores, Year);
            return Json(responses, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Top10SKUByAmount()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult GetTop10SKUByAmount(List<int> SelectedDc)
        {
            int UserId = SessionValues.UserId;
            int CustId = SessionValues.LoggedInCustId.Value;
            int storeid = 0;
            int Year = DateTime.Now.Year;
            if (SessionValues.StoreId != 0 || SessionValues.RoleId == 1)
            {
                storeid = SessionValues.StoreId;
                CustId = _storeService.GetStoreById(storeid).CustId;
            }
            var Stores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Report");
            var selectedStores = Stores.Select(x => x.Id).ToList();
            List<TopSKUForCustomer> responses = _reportService.GetTop10SKUByAmount(UserId, CustId, selectedStores, Year, SelectedDc);
            return Json(responses, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Top10SKUByCase()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult GetTop10SKUByCase(List<int> SelectedDc)
        {
            int UserId = SessionValues.UserId;
            int CustId = SessionValues.LoggedInCustId.Value;
            int storeid = 0;
            int Year = DateTime.Now.Year;
            if (SessionValues.StoreId != 0 || SessionValues.RoleId == 1)
            {
                storeid = SessionValues.StoreId;
                CustId = _storeService.GetStoreById(storeid).CustId;
            }
            var Stores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Report");
            var selectedStores = Stores.Select(x => x.Id).ToList();
            List<TopSKUForCustomer> responses = _reportService.GetTop10SKUByCase(UserId, CustId, selectedStores, Year, SelectedDc);
            return Json(responses, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillRateByMonth()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult GetFillRateByMonth(List<int> SelectedDc)
        {
            int UserId = SessionValues.UserId;
            int CustId = SessionValues.LoggedInCustId.Value;
            int storeid = 0;
            int Year = DateTime.Now.Year;
            if (SessionValues.StoreId != 0 || SessionValues.RoleId == 1)
            {
                storeid = SessionValues.StoreId;
                CustId = _storeService.GetStoreById(storeid).CustId;
            }
            var Stores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Report");
            var selectedStores = Stores.Select(x => x.Id).ToList();
            List<MonthWiseFillRate> responses = _reportService.GetFillRateByMonth(UserId, CustId, selectedStores, Year, SelectedDc);
            return Json(responses, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ActiveStoreMasterReports
        public ActionResult ActiveStores()
        {
            int UserId = SessionValues.UserId;
            int CustId = SessionValues.LoggedInCustId.Value;
            var DCs = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Report");
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            return View();
        }
        [HttpPost]
        public ActionResult ActiveStores(List<int> SelectedDC, List<int> SelectedStores, string ReportType)
        {
            int UserId = SessionValues.UserId;
            int CustId = SessionValues.LoggedInCustId.Value;
            var DCs = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Report");
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Report").Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Report").Select(x => x.Id).ToList();

            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            List<StoreModel> finilised = _reportService.ActiveStoresReport(ReportType, CustId, UserId, SelectedDC, SelectedStores);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Reports = finilised.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult ActiveStoresExportExcel(List<int> SelectedDC, List<int> SelectedStores, string ReportType)
        {
            int UserId = SessionValues.UserId;
            int CustId = SessionValues.LoggedInCustId.Value;
            var DCs = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Report");
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Report").Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Report").Select(x => x.Id).ToList();

            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            List<StoreModel> finilised = _reportService.ActiveStoresReport(ReportType, CustId, UserId, SelectedDC, SelectedStores);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            string Title = "Name: Active Store Report";
            var finilisedList = finilised.Select(store => new
            {
                EnterpriseCode = store.EnterpriseCode,
                EnterpriseName = store.EnterpriseName,
                StoreCode = store.StoreCode,
                StoreName = store.StoreName,
                Address = store.Address,
                Location = store.Location,
                WareHouseDCName = store.WareHouseDCName,
                PlaceOfSupply = store.PlaceOfSupply,
                UserEmailAddress = store.UserEmailAddress,
                StoreContactNo = store.StoreContactNo,
                StoreManager = store.StoreManager,
                CreatedDate = store.CreatedDate
            }).ToList();
            //ExportToExcel(finilised);
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].Value = Title;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                string DateCellFormat = "dd-mm-yyyy";
                workSheet.Cells[2, 1].Value = "EnterpriseCode";
                workSheet.Cells[2, 2].Value = "EnterpriseName";
                workSheet.Cells[2, 3].Value = "StoreCode";
                workSheet.Cells[2, 4].Value = "StoreName";
                workSheet.Cells[2, 5].Value = "Address";
                workSheet.Cells[2, 6].Value = "Location";
                workSheet.Cells[2, 7].Value = "Dc";
                workSheet.Cells[2, 8].Value = "PlaceOfSupply";
                workSheet.Cells[2, 9].Value = "EmailAddress";
                workSheet.Cells[2, 10].Value = "StoreContactPerson";
                workSheet.Cells[2, 11].Value = "StoreContactNo";
                workSheet.Cells[2, 12].Value = "CreatedOn";
                using (ExcelRange Rng = workSheet.Cells["A2:J2"])
                {
                    Rng.Style.Font.Bold = true;
                }
                workSheet.Cells[3, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                using (ExcelRange Rng = workSheet.Cells["L:L"])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ActiveStores.xlsx");
            }
        }
        #endregion

        #region GetDashboardValuesForDC
        [HttpPost]
        public ActionResult GetDashboardValuesForDC(List<int> SelectedDc)
        {
            int custid = SessionValues.LoggedInCustId.Value;
            int userid = SessionValues.UserId;
            var OFRLIFR_MTD = _reportService.OFRLIFR_MTD(userid, custid, SelectedDc);
            var MTDCls = _reportService.MTDCls(userid, custid, SelectedDc);

            JsonResult result = Json(new { data = new { OFRLIFR_MTD, MTDCls }, JsonRequestBehavior.AllowGet });
            result.MaxJsonLength = int.MaxValue;
            return result;
        }
        #endregion

        #region InvoiceReport

        [AdminAuthorization]
        public ActionResult InvoiceReport()
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            DateTime now = DateTime.Now;
            var startDate = now.AddDays(-1).Date;
            var endDate = startDate.AddDays(1).Date;
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = null;
            var selectedStores = Stores.Select(x => x.Id).ToList();
            var SelectedSKUs = Items.Select(x => x.Id).ToList();
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InvoiceReport(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = UniqueReferenceID;
            SelectedStores = SelectedStores != null && SelectedStores.Count() > 0 ? SelectedStores : Stores.Select(x => x.Id).ToList();
            SelectedSKUs = SelectedSKUs != null && SelectedSKUs.Count() > 0 ? SelectedSKUs : Items.Select(x => x.Id).ToList();
            List<InvoiceView> finilised = _reportService.InvoiceReport(startDate, endDate, custid, userid, SelectedStores, SelectedSKUs);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.FinalizedReports = finilised.ToList();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult InvoiceReportExportExcel(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = UniqueReferenceID;
            SelectedStores = SelectedStores != null && SelectedStores.Count() > 0 ? SelectedStores : Stores.Select(x => x.Id).ToList();
            SelectedSKUs = SelectedSKUs != null && SelectedSKUs.Count() > 0 ? SelectedSKUs : Items.Select(x => x.Id).ToList();
            var finilised = _reportService.InvoiceReport(startDate, endDate, custid, userid, SelectedStores, SelectedSKUs);
            string Title = "Name: Daily Order Report";
            string Message = "Store Order Date: From: " + startDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " To: " + endDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "";
            var finilisedList = finilised.Select(head => new
            {
                head.Invoice_Number,
                head.DueDate,

                head.EnterpriseCode,
                head.EnterpriseName,
                head.Project_Name,
                head.StoreName,
                head.CreatedFrom,
                head.InvoiceDate,
                head.CustomerZone,
                head.Department,
                head.Class,
                head.Division,
                head.EmployeeDepartment,
                head.SOApproverPartner,

                head.CreateBy,
                head.DC_Location,
                head.Item,
                head.Description,
                head.HSNSAC,
                head.Location,
                head.TaxCode,
                head.TaxRate,
                head.Amount,
                head.TaxAmount,
                head.GrossAmount,
                head.TaxLiable,
                head.GoodsAndServices,
                head.ItemCategory,
                head.Quantity
            });
            //ExportToExcel(finilised);
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].Value = Title;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Cells[2, 1].Value = Message;
                workSheet.Cells[2, 1].Style.Font.Bold = true;
                string DateCellFormat = "dd/mm/yyyy";

                workSheet.Cells[4, 1].Value = "PO Number";
                workSheet.Cells[4, 2].Value = "Unique Reference ID";
                workSheet.Cells[4, 3].Value = "Store Order Date";

                workSheet.Cells[4, 4].Value = "Enterprise Code";
                workSheet.Cells[4, 5].Value = "Enterprise Name";
                workSheet.Cells[4, 6].Value = "Store Code";
                workSheet.Cells[4, 7].Value = "Store Name";
                workSheet.Cells[4, 8].Value = "City";
                workSheet.Cells[4, 9].Value = "DC";
                workSheet.Cells[4, 10].Value = "Place of Supply";

                workSheet.Cells[4, 11].Value = "SKU Code";
                workSheet.Cells[4, 12].Value = "SKU Decsription";
                workSheet.Cells[4, 13].Value = "SKU Category Type";
                workSheet.Cells[4, 14].Value = "SKU Category";
                workSheet.Cells[4, 15].Value = "UOM";
                workSheet.Cells[4, 16].Value = "Case Size";

                workSheet.Cells[4, 17].Value = "Original Order Quantity";
                workSheet.Cells[4, 18].Value = "Revised Order Quantity";
                workSheet.Cells[4, 19].Value = "Ordered By";
                //workSheet.Cells[4, 12].Value = "Dc";
                workSheet.Cells[4, 20].Value = "System DateTime";

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
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Daily Order Report.xlsx");
            }
        }
        #endregion

        #region DSRBasic
        [AdminAuthorization]
        public ActionResult DSR()
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            //ItemCategories
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            //ItemType
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
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
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", null);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", null);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DSR(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report");
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedBrands == null || SelectedBrands.Count() == 0 || SelectedBrands[0] == 0)
                SelectedBrands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report").Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();

            List<DailySalesReport> finilised = _reportService.DailySalesBasicReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, SelectedLocations, SelectedTypes, SelectedCategories, SelectedBrands);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.FinalizedReports = finilised.ToList();
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", SelectedBrands);
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DSRExportExcel(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report");
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedBrands == null || SelectedBrands.Count() == 0 || SelectedBrands[0] == 0)
                SelectedBrands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report").Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();

            List<DailySalesReport> finilised = _reportService.DailySalesBasicReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, SelectedLocations, SelectedTypes, SelectedCategories, SelectedBrands);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.FinalizedReports = finilised.ToList();

            var finilisedList = finilised.Select(head => new
            {
                head.PONumber,
                head.Unique_Reference_Id,

                Store_Order_Date = Convert.ToDateTime(head.Store_Order_Date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                UploadedDate = Convert.ToDateTime(head.Upload_Date).ToString("dd/MM/yyyy hh:mm", CultureInfo.InvariantCulture),
                head.Order_Type,

                head.EnterpriseCode,
                head.EnterpriseName,
                head.Store_Code,
                head.StoreName,
                head.City,
                head.PlaceOfSupply,

                head.Item_Code,
                head.Item_Desc,
                head.ItemCategoryType,
                head.Item_Type,
                head.BrandName,
                head.UnitofMasureDescription,
                head.Case_Size,

                head.Original_Ordered_Qty,
                head.Revised_Ordered_Qty,

                head.OrderNo,
                PODate = head.PODate.HasValue ? Convert.ToDateTime(head.PODate.Value).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                head.Rfpl_Ordered_Qty,
                head.Order_Status,
                head.Dc_Name,

                head.SoNumber,
                head.SoQuantity,

                head.Invoice_Number,
                head.Invoice_Date,
                head.Invoice_Quantity,
                head.Invoice_Amount,

                head.RAQuantity,
                head.RAAmount,
                head.FillRateGap,

                head.Taken_Days,
                head.Slab
            });
            //ExportToExcel(finilised);
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                string DateCellFormat = "dd/mm/yyyy";
                workSheet.Cells[1, 1].Value = "PO Number";
                workSheet.Cells[1, 2].Value = "Unique Reference ID";

                workSheet.Cells[1, 3].Value = "Store Order Date";
                workSheet.Cells[1, 4].Value = "System DateTime";
                workSheet.Cells[1, 5].Value = "Order Type";

                workSheet.Cells[1, 6].Value = "Enterprise Code";
                workSheet.Cells[1, 7].Value = "Enterprise Name";
                workSheet.Cells[1, 8].Value = "Store Code";
                workSheet.Cells[1, 9].Value = "Store Name";
                workSheet.Cells[1, 10].Value = "City";
                workSheet.Cells[1, 11].Value = "Place of Supply";

                workSheet.Cells[1, 12].Value = "SKU Code";
                workSheet.Cells[1, 13].Value = "SKU Description";
                workSheet.Cells[1, 14].Value = "SKU Category Type";
                workSheet.Cells[1, 15].Value = "SKU Category";
                workSheet.Cells[1, 16].Value = "Brand";
                workSheet.Cells[1, 17].Value = "UOM";
                workSheet.Cells[1, 18].Value = "MOQ";

                workSheet.Cells[1, 19].Value = "Original Order Quantity";
                workSheet.Cells[1, 20].Value = "Revised Order Quantity";

                workSheet.Cells[1, 21].Value = "PO Number";
                workSheet.Cells[1, 22].Value = "PO Date";
                workSheet.Cells[1, 23].Value = "Rfpl Order Quantity";
                workSheet.Cells[1, 24].Value = "Order Status";
                workSheet.Cells[1, 25].Value = "DC";

                workSheet.Cells[1, 26].Value = "So Number";
                workSheet.Cells[1, 27].Value = "So Quantity";

                workSheet.Cells[1, 28].Value = "Invoice Number";
                workSheet.Cells[1, 29].Value = "Invoice Date";
                workSheet.Cells[1, 30].Value = "Invoice Quantity";
                workSheet.Cells[1, 31].Value = "Invoice Amount";

                workSheet.Cells[1, 32].Value = "RA Quantity";
                workSheet.Cells[1, 33].Value = "RA Amount";

                workSheet.Cells[1, 34].Value = "Fill Rate Gap";

                workSheet.Cells[1, 35].Value = "Taken Days";
                workSheet.Cells[1, 36].Value = "Slab";
                using (ExcelRange Rng = workSheet.Cells["A1:AN1"])
                {
                    Rng.Style.Font.Bold = true;
                }
                workSheet.Cells[2, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                using (ExcelRange Rng = workSheet.Cells["C:C"])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }
                using (ExcelRange Rng = workSheet.Cells["E:E"])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Daily Sales Report.xlsx");
            }
        }

        #endregion

        #region AgeingReportNew
        [AdminAuthorization]
        public ActionResult AgeingReport2()
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            DateTime now = DateTime.Now;

            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            //ItemCategories
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            //ItemType
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);

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

            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", null);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AgeingReport2(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, int ReportType, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories)
        {

            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            DateTime now = DateTime.Now;
            string DateCellFormat = "dd-mm-yyyy";
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = UniqueReferenceID;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();

            List<AgeingReport> finilised = _reportService.AgeingReport2(startDate, endDate, custid, userid, SelectedDC, SelectedStores, SelectedSKUs, ReportType, SelectedLocations, SelectedTypes, SelectedCategories);
            ViewBag.FinalizedReports = finilised.ToList();
            ViewBag.ReportType = ReportType;
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);

            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AgeingReportExportExcel2(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, int ReportType, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
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
            var finilised = _reportService.AgeingReport2(startDate, endDate, custid, userid, SelectedDC, SelectedStores, SelectedSKUs, ReportType, SelectedLocations, SelectedTypes, SelectedCategories);
            string Title = "";
            if (ReportType == 1)
            {
                Title = "Name: Booked not Invoiced";
            }
            if (ReportType == 2)
            {
                Title = "Name: Invoiced not delivered";
            }
            else
            {
                Title = "Name: Delivered no POD";
            }
            string Message = "Store Order Date: From: " + startDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " To: " + endDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "";
            var finilisedList = finilised.Select(head => new
            {
                head.EnterpriseCode,
                head.EnterpriseName,
                head.Store_Code,
                head.StoreName,
                head.A0_3,
                head.A3_5,
                head.A5_7,
                head.A7_10,
                head.Above_10
            });
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].Value = Title;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Cells[2, 1].Value = Message;
                workSheet.Cells[2, 1].Style.Font.Bold = true;
                //string DateCellFormat = "dd-mm-yyyy";

                workSheet.Cells[4, 1].Value = "Enterprise Code";
                workSheet.Cells[4, 2].Value = "Enterprise Name";
                workSheet.Cells[4, 3].Value = "Store Code";
                workSheet.Cells[4, 4].Value = "Store Name";
                workSheet.Cells[4, 5].Value = "Store Code";
                workSheet.Cells[4, 6].Value = "Age 0-3";
                workSheet.Cells[4, 7].Value = "Age 3-5";
                workSheet.Cells[4, 8].Value = "Age 5-7";
                workSheet.Cells[4, 9].Value = "Age 7-10";
                workSheet.Cells[4, 10].Value = "Age >10";
                using (ExcelRange Rng = workSheet.Cells["A4:AL4"])
                {
                    Rng.Style.Font.Bold = true;
                }
                workSheet.Cells[5, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AgeingReport.xlsx");
            }
        }
        public ActionResult AgeingDetailReport2(List<string> SelectedDC, string SelectedStores, List<string> SelectedSKUs, string ReportType, string Age, string vAgeRange, string FromDate, string ToDate)//DateTime? FromDate, DateTime? ToDate, 
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            DateTime now = DateTime.Now;

            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));

            int PDayFrom = 0;
            int PdayTo = 0;
            if (vAgeRange == "1")
            {
                PDayFrom = 0;
                PdayTo = 5;
            }
            if (vAgeRange == "2")
            {
                PDayFrom = 6;
                PdayTo = 10;
            }
            if (vAgeRange == "3")
            {
                PDayFrom = 11;
                PdayTo = 15;
            }
            if (vAgeRange == "4")
            {
                PDayFrom = 16;
                PdayTo = 1000;
            }
            List<AgeingDetailReport> finilised = _reportService.AgeingDetailReport(startDate, endDate, custid, userid, SelectedDC, SelectedStores, SelectedSKUs, PDayFrom, PdayTo, Convert.ToInt32(ReportType));
            ViewBag.AgeingDetailReportList = finilised.ToList();

            return PartialView("_AgeingDetailReportPartial");
        }

        #endregion

        #region RAReportBasic
        [AdminAuthorization]
        public ActionResult RAReport()
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            //ItemCategories
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            //ItemType
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
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
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", null);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", null);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RAReport(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report");
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedBrands == null || SelectedBrands.Count() == 0 || SelectedBrands[0] == 0)
                SelectedBrands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report").Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();

            List<DailySalesReport> finilised = _reportService.RABasicReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, SelectedLocations, SelectedTypes, SelectedCategories, SelectedBrands);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.FinalizedReports = finilised.ToList();
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", SelectedBrands);
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult RAExportExcel(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report");
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedBrands == null || SelectedBrands.Count() == 0 || SelectedBrands[0] == 0)
                SelectedBrands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report").Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();

            List<DailySalesReport> finilised = _reportService.RABasicReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, SelectedLocations, SelectedTypes, SelectedCategories, SelectedBrands);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.FinalizedReports = finilised.ToList();

            var finilisedList = finilised.Select(head => new
            {
                head.PONumber,
                head.Unique_Reference_Id,

                Store_Order_Date = Convert.ToDateTime(head.Store_Order_Date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                UploadedDate = Convert.ToDateTime(head.Upload_Date).ToString("dd/MM/yyyy hh:mm", CultureInfo.InvariantCulture),
                head.Order_Type,

                head.EnterpriseCode,
                head.EnterpriseName,
                head.Store_Code,
                head.StoreName,
                head.City,
                head.PlaceOfSupply,

                head.Item_Code,
                head.Item_Desc,
                head.ItemCategoryType,
                head.Item_Type,
                head.BrandName,
                head.UnitofMasureDescription,
                head.Case_Size,

                head.Original_Ordered_Qty,
                head.Revised_Ordered_Qty,

                head.OrderNo,

                PODate = head.PODate.HasValue ? Convert.ToDateTime(head.PODate.Value).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                head.Rfpl_Ordered_Qty,
                head.Order_Status,
                head.Dc_Name,

                head.SoNumber,
                head.SoQuantity,

                head.Invoice_Number,
                head.Invoice_Date,
                head.Invoice_Quantity,

                head.RANumber,
                head.RAQuantity,
                RADate = head.RADate.HasValue ? Convert.ToDateTime(head.RADate.Value).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : ""
            });
            //ExportToExcel(finilised);
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                string DateCellFormat = "dd/mm/yyyy";
                workSheet.Cells[1, 1].Value = "PO Number";
                workSheet.Cells[1, 2].Value = "Unique Reference ID";

                workSheet.Cells[1, 3].Value = "Store Order Date";
                workSheet.Cells[1, 4].Value = "System DateTime";
                workSheet.Cells[1, 5].Value = "Order Type";

                workSheet.Cells[1, 6].Value = "Enterprise Code";
                workSheet.Cells[1, 7].Value = "Enterprise Name";
                workSheet.Cells[1, 8].Value = "Store Code";
                workSheet.Cells[1, 9].Value = "Store Name";
                workSheet.Cells[1, 10].Value = "City";
                workSheet.Cells[1, 11].Value = "Place of Supply";

                workSheet.Cells[1, 12].Value = "SKU Code";
                workSheet.Cells[1, 13].Value = "SKU Description";
                workSheet.Cells[1, 14].Value = "SKU Category Type";
                workSheet.Cells[1, 15].Value = "SKU Category";
                workSheet.Cells[1, 16].Value = "Brand";
                workSheet.Cells[1, 17].Value = "UOM";
                workSheet.Cells[1, 18].Value = "MOQ";

                workSheet.Cells[1, 19].Value = "Original Order Quantity";
                workSheet.Cells[1, 20].Value = "Revised Order Quantity";

                workSheet.Cells[1, 21].Value = "PO Number";
                workSheet.Cells[1, 22].Value = "PO Date";
                workSheet.Cells[1, 23].Value = "Rfpl Order Quantity";
                workSheet.Cells[1, 24].Value = "Order Status";
                workSheet.Cells[1, 25].Value = "DC";

                workSheet.Cells[1, 26].Value = "So Number";
                workSheet.Cells[1, 27].Value = "So Quantity";

                workSheet.Cells[1, 28].Value = "Invoice Number";
                workSheet.Cells[1, 29].Value = "Invoice Date";
                workSheet.Cells[1, 30].Value = "Invoice Quantity";
                workSheet.Cells[1, 31].Value = "RA Number";

                workSheet.Cells[1, 32].Value = "RA Quantity";
                workSheet.Cells[1, 33].Value = "RA Date";
                using (ExcelRange Rng = workSheet.Cells["A1:AN1"])
                {
                    Rng.Style.Font.Bold = true;
                }
                workSheet.Cells[2, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                using (ExcelRange Rng = workSheet.Cells["C:C"])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }
                using (ExcelRange Rng = workSheet.Cells["E:E"])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RAReport.xlsx");
            }
        }

        #endregion

        #region ItemActiveDeactiveReport
        [AdminAuthorization]
        public ActionResult ItemDCStoreActiveReport()
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", null);
            ViewBag.ReportType = "DC";
            return View();
        }
        [HttpPost]
        public ActionResult ItemDCStoreActiveReport(string ReportType, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");

            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = DCs.Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = Stores.Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedBrands == null || SelectedBrands.Count() == 0 || SelectedBrands[0] == 0)
                SelectedBrands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report").Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();
            List<DCStoreWiseItemReport> itemReport = _reportService.GetDCStoreWiseItemReport(ReportType, custid, userid, SelectedDC, SelectedStores, SelectedSKUs, SelectedTypes, SelectedCategories, SelectedBrands);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", null);
            ViewBag.ReportType = ReportType;
            ViewBag.Report = itemReport;
            return View();
        }
        [HttpPost]
        public ActionResult ItemDCStoreActiveReportExport(string ReportType, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");

            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = DCs.Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = Stores.Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedBrands == null || SelectedBrands.Count() == 0 || SelectedBrands[0] == 0)
                SelectedBrands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report").Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();
            List<DCStoreWiseItemReport> itemReport = _reportService.GetDCStoreWiseItemReport(ReportType, custid, userid, SelectedDC, SelectedStores, SelectedSKUs, SelectedTypes, SelectedCategories, SelectedBrands);
            if (ReportType == "DC")
            {
                var finilisedList = itemReport.Select(head => new
                {
                    head.DC,
                    head.ItemCode,
                    head.ItemName,
                    head.CaseConversion,
                    head.MinimumOrder,
                    head.MaximumOrderLimit,
                    head.UOMMaster,
                    head.CategoryType,
                    head.Category,
                    head.SubCategory,
                    head.Brand,
                    head.Active
                });
                using (var excel = new ExcelPackage())
                {
                    var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                    using (ExcelRange Rng = workSheet.Cells["A1:AN1"])
                    {
                        Rng.Style.Font.Bold = true;
                    }
                    workSheet.Cells[1, 1].LoadFromCollection(finilisedList, PrintHeaders: true, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                    workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                    return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DCwiseActiveItemReport.xlsx");
                }
            }
            if (ReportType == "STORE")
            {
                var finilisedList = itemReport.Select(head => new
                {
                    head.DC,
                    head.StoreCode,
                    head.StoreName,
                    head.ItemCode,
                    head.ItemName,
                    head.CaseConversion,
                    head.MinimumOrder,
                    head.MaximumOrderLimit,
                    head.UOMMaster,
                    head.CategoryType,
                    head.Category,
                    head.SubCategory,
                    head.Brand,
                    head.Active
                });

                using (var excel = new ExcelPackage())
                {
                    var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                    using (ExcelRange Rng = workSheet.Cells["A1:AN1"])
                    {
                        Rng.Style.Font.Bold = true;
                    }
                    workSheet.Cells[1, 1].LoadFromCollection(finilisedList, PrintHeaders: true, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                    workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                    return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "StorewiseActiveItemReport.xlsx");
                }
            }
            else return Content("Invalid Return Type: " + ReportType);
        }
        #endregion
        #region ItemMasterReprt
        public ActionResult ItemMasterReport()
        {
            int userid = SessionValues.UserId;
            int custid = 0;
            int StoreId = 0;
            if (SessionValues.RoleId == 1)
            {
                custid = Convert.ToInt32(SessionValues.LoggedInCustId);
                StoreId = SessionValues.StoreId;
            }
            if (SessionValues.RoleId == 2)
            {
                custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            }
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report", StoreId);
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report", StoreId);
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid, StoreId);
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid, StoreId);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", null);
            return View();
        }
        [HttpPost]
        public ActionResult ItemMasterReport(List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            int userid = SessionValues.UserId;
            int custid = 0;
            int StoreId = 0;
            if (SessionValues.RoleId == 1)
            {
                custid = Convert.ToInt32(SessionValues.LoggedInCustId);
                StoreId = SessionValues.StoreId;
            }
            if (SessionValues.RoleId == 2)
            {
                custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            }
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report", StoreId);
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report", StoreId);
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid, StoreId);
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid, StoreId);
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = DCs.Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = Stores.Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes, StoreId).Select(x => x.Id).ToList();
            if (SelectedBrands == null || SelectedBrands.Count() == 0 || SelectedBrands[0] == 0)
                SelectedBrands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report", StoreId).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report", StoreId).Select(x => x.Id).ToList();

            List<ItemReport> itemReport = _reportService.GetItemReport(custid, userid, SelectedDC, SelectedStores, SelectedSKUs, SelectedTypes, SelectedCategories, SelectedBrands);
            ViewBag.Report = itemReport;
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", null);
            return View();
        }
        [HttpPost]
        public ActionResult ItemMasteReportExportExcel(List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            try
            {

                int userid = SessionValues.UserId;
                int custid = 0;
                int StoreId = 0;
                if (SessionValues.RoleId == 1)
                {
                    custid = Convert.ToInt32(SessionValues.LoggedInCustId);
                    StoreId = SessionValues.StoreId;
                }
                if (SessionValues.RoleId == 2)
                {
                    custid = Convert.ToInt32(SessionValues.LoggedInCustId);
                }
                var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
                var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
                var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report", StoreId);
                var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report", StoreId);
                var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
                List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid, StoreId);
                List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid, StoreId);
                if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                    SelectedDC = DCs.Select(x => x.Id).ToList();
                if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                    SelectedStores = Stores.Select(x => x.Id).ToList();

                if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                    SelectedTypes = ItemType.Select(x => x.Id).ToList();
                if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                    SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes, StoreId).Select(x => x.Id).ToList();
                if (SelectedBrands == null || SelectedBrands.Count() == 0 || SelectedBrands[0] == 0)
                    SelectedBrands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report", StoreId).Select(x => x.Id).ToList();
                if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                    SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report", StoreId).Select(x => x.Id).ToList();

                List<ItemReport> itemReport = _reportService.GetItemReport(custid, userid, SelectedDC, SelectedStores, SelectedSKUs, SelectedTypes, SelectedCategories, SelectedBrands);
                var finilisedList = itemReport.Select(head => new
                {
                    head.Customer,
                    head.ItemCode,
                    head.ItemName,
                    head.PackingDescription,
                    head.MinimumOrder,
                    head.MaximumOrderLimit,
                    head.CaseConversion,
                    head.UOMMaster,
                    head.CategoryType,
                    head.Category,
                    head.SubCategory,
                    head.Brand
                });
                using (var excel = new ExcelPackage())
                {
                    var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                    using (ExcelRange Rng = workSheet.Cells["A1:AN1"])
                    {
                        Rng.Style.Font.Bold = true;
                    }
                    workSheet.Cells[1, 1].LoadFromCollection(finilisedList, PrintHeaders: true, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                    workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                    return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ItemReport.xlsx");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Exception occurred: {ex.Message}");
                throw; // Re-throw the exception to propagate it further if necessary
            }

        }
        #endregion
        #region StoreActiveDeactiveReport
        [AdminAuthorization]
        public ActionResult StoreActiveReport()
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", null);
            return View();
        }
        [HttpPost]
        public ActionResult StoreActiveReport(string ReportType, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedLocations)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);

            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();


            List<StoreModel> storeReport = _reportService.StoreWiseItemReport(ReportType, custid, userid, SelectedDC, SelectedLocations, SelectedStores);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.ReportType = ReportType;
            ViewBag.Report = storeReport;
            return View();
        }
        [HttpPost]
        public ActionResult StoreActiveReportExport(string ReportType, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedLocations)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);

            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            List<StoreModel> storeReport = _reportService.StoreWiseItemReport(ReportType, custid, userid, SelectedDC, SelectedLocations, SelectedStores);
            var finilisedList = storeReport.Select(head => new
            {
                head.CustomerName,
                head.StoreCode,
                head.StoreName,
                head.Address,
                head.Location,
                head.StoreEmailId,
                head.Region,
                head.Route,
                head.StoreManager,
                head.StoreContactNo,
                head.PlaceOfSupply,
                head.WareHouseDCName,
                head.UserEmailAddress,
                head.EnterpriseCode,
                head.EnterpriseName,
                head.ModifiedDate,
                head.Active
            });

            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].Value = "Customer";
                workSheet.Cells[1, 2].Value = "StoreCode";
                workSheet.Cells[1, 3].Value = "StoreName";
                workSheet.Cells[1, 4].Value = "Address";
                workSheet.Cells[1, 5].Value = "Location";
                workSheet.Cells[1, 6].Value = "Store EmailAddress";
                workSheet.Cells[1, 7].Value = "Region";
                workSheet.Cells[1, 8].Value = "Route";
                workSheet.Cells[1, 9].Value = "Store Manager";
                workSheet.Cells[1, 10].Value = "Store ContactNo";
                workSheet.Cells[1, 11].Value = "PlaceOfSupply";
                workSheet.Cells[1, 12].Value = "WareHouse";
                workSheet.Cells[1, 13].Value = "User Login Email";
                workSheet.Cells[1, 14].Value = "EnterpriseCode";
                workSheet.Cells[1, 15].Value = "EnterpriseName";
                workSheet.Cells[1, 16].Value = "Last Updated On";
                workSheet.Cells[1, 17].Value = "Active";
                using (ExcelRange Rng = workSheet.Cells["A1:AN1"])
                {
                    Rng.Style.Font.Bold = true;
                }
                using (ExcelRange Rng = workSheet.Cells["P:P"])
                {
                    Rng.Style.Numberformat.Format = "yyyy-MM-dd";
                }
                workSheet.Cells[2, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ActiveStoreReport.xlsx");
            }
        }
        #endregion

        #region ReportViewver
        //StoreOrdering ds = new StoreOrdering();
        //public ActionResult ReportEmployee()
        //{
        //    ReportViewer reportViewer = new ReportViewer();
        //    reportViewer.ProcessingMode = ProcessingMode.Local;
        //    reportViewer.SizeToReportContent = true;
        //    reportViewer.Width = Unit.Percentage(900);
        //    reportViewer.Height = Unit.Percentage(900);
        //    var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DbEmployeeConnectionString"].ConnectionString;
        //    SqlConnection conx = new SqlConnection(connectionString);
        //    SqlDataAdapter adp = new SqlDataAdapter("SELECT * FROM Employee_tbt", conx);
        //    adp.Fill(ds, ds..TableName);
        //    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\rptSOvsInv.rdlc";
        //    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MyDataSet", ds.Tables[0]));
        //    ViewBag.ReportViewer = reportViewer;
        //    return View();
        //}
        #endregion

        #region StoreWiseSkuReport
        [AdminAuthorization]
        public ActionResult StoreWiseSkuReport()
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            //ItemCategories
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            //ItemType
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
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
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", null);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", null);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StoreWiseSkuReport(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report");
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedBrands == null || SelectedBrands.Count() == 0 || SelectedBrands[0] == 0)
                SelectedBrands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report").Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();

            List<DailySalesReport> finilised = _reportService.DailySalesBasicReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, SelectedLocations, SelectedTypes, SelectedCategories, SelectedBrands);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.FinalizedReports = finilised.ToList();
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", SelectedBrands);
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult StoreWiseSkuReportTOExcel(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report");
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedBrands == null || SelectedBrands.Count() == 0 || SelectedBrands[0] == 0)
                SelectedBrands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report").Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();

            List<DailySalesReport> finilised = _reportService.DailySalesBasicReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, SelectedLocations, SelectedTypes, SelectedCategories, SelectedBrands);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.FinalizedReports = finilised.ToList();

            var finilisedList = finilised.Select(head => new
            {


                Store_Order_Date = Convert.ToDateTime(head.Store_Order_Date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                UploadedDate = Convert.ToDateTime(head.Upload_Date).ToString("dd/MM/yyyy hh:mm", CultureInfo.InvariantCulture),
                head.Order_Type,

                head.EnterpriseCode,
                head.EnterpriseName,
                head.Store_Code,
                head.StoreName,
                head.City,
                head.PlaceOfSupply,

                head.Item_Code,
                head.Item_Desc,
                head.ItemCategoryType,
                head.Item_Type,
                head.BrandName,
                head.UnitofMasureDescription,
                head.Case_Size,

                head.Original_Ordered_Qty,
                head.Revised_Ordered_Qty,

                head.OrderNo,
                PODate = head.PODate.HasValue ? Convert.ToDateTime(head.PODate.Value).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                head.Rfpl_Ordered_Qty,
                head.Order_Status,
                head.Dc_Name,

                head.SoNumber,
                head.SoQuantity,

                head.Invoice_Number,
                head.Invoice_Date,
                head.Invoice_Quantity,
                head.Invoice_Amount,

                head.RAQuantity,
                head.RAAmount,

            });
            //ExportToExcel(finilised);
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                string DateCellFormat = "dd/mm/yyyy";

                workSheet.Cells[1, 3].Value = "Store Order Date";
                workSheet.Cells[1, 4].Value = "System DateTime";
                workSheet.Cells[1, 5].Value = "Order Type";

                workSheet.Cells[1, 6].Value = "Enterprise Code";
                workSheet.Cells[1, 7].Value = "Enterprise Name";
                workSheet.Cells[1, 8].Value = "Store Code";
                workSheet.Cells[1, 9].Value = "Store Name";
                workSheet.Cells[1, 10].Value = "City";
                workSheet.Cells[1, 11].Value = "Place of Supply";

                workSheet.Cells[1, 12].Value = "SKU Code";
                workSheet.Cells[1, 13].Value = "SKU Description";
                workSheet.Cells[1, 14].Value = "SKU Category Type";
                workSheet.Cells[1, 15].Value = "SKU Category";
                workSheet.Cells[1, 16].Value = "Brand";
                workSheet.Cells[1, 17].Value = "UOM";
                workSheet.Cells[1, 18].Value = "MOQ";

                workSheet.Cells[1, 19].Value = "Original Order Quantity";
                workSheet.Cells[1, 20].Value = "Revised Order Quantity";

                workSheet.Cells[1, 21].Value = "PO Number";
                workSheet.Cells[1, 22].Value = "PO Date";
                workSheet.Cells[1, 23].Value = "Rfpl Order Quantity";
                workSheet.Cells[1, 24].Value = "Order Status";
                workSheet.Cells[1, 25].Value = "DC";

                workSheet.Cells[1, 26].Value = "So Number";
                workSheet.Cells[1, 27].Value = "So Quantity";

                workSheet.Cells[1, 28].Value = "Invoice Number";
                workSheet.Cells[1, 29].Value = "Invoice Date";
                workSheet.Cells[1, 30].Value = "Invoice Quantity";
                workSheet.Cells[1, 31].Value = "Invoice Amount";

                workSheet.Cells[1, 32].Value = "RA Quantity";
                workSheet.Cells[1, 33].Value = "RA Amount";

                using (ExcelRange Rng = workSheet.Cells["A1:AN1"])
                {
                    Rng.Style.Font.Bold = true;
                }
                workSheet.Cells[2, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                using (ExcelRange Rng = workSheet.Cells["C:C"])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }
                using (ExcelRange Rng = workSheet.Cells["E:E"])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "StoreWiseSku Report.xlsx");
            }
        }

        #endregion
        #region PFAReportBasic
       
        [AdminAuthorization]
        public ActionResult PFAReport()
        {
            int UserId = SessionValues.UserId;
            int CustId = SessionValues.LoggedInCustId ?? 0;

            int StoreId = 0;
            if (SessionValues.RoleId == 1)
            {
                CustId = Convert.ToInt32(SessionValues.LoggedInCustId);
                StoreId = SessionValues.StoreId;
            }
            var DCs = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Report");
            var Items = _itemService.GetSkuForUserCustomer(UserId, CustId, "Report", StoreId);
            var Brands = _itemService.GetBrandsForUserCustomer(UserId, CustId, "Report", StoreId);
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(UserId, CustId, StoreId);
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(UserId, CustId, StoreId);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);

            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDataforPFAReport(List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            int UserId = SessionValues.UserId;
            int CustId = SessionValues.LoggedInCustId.Value;
            var req = Request;
            int StoreId = 0;
            if (SessionValues.RoleId == 1)
            {
                CustId = SessionValues.LoggedInCustId.HasValue ? SessionValues.LoggedInCustId.Value : 0;
                StoreId = SessionValues.StoreId;
            }
            if (SessionValues.RoleId != 1)
            {
                CustId = SessionValues.LoggedInCustId == null ? 0 : SessionValues.LoggedInCustId.Value;
            }
            var DCs = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Report");
            var Items = _itemService.GetSkuForUserCustomer(UserId, CustId, "Report", StoreId);
            var Brands = _itemService.GetBrandsForUserCustomer(UserId, CustId, "Report", StoreId);
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(UserId, CustId, StoreId);
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(UserId, CustId, StoreId);
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Report").Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Report").Select(x => x.Id).ToList();
            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(UserId, CustId, SelectedTypes, StoreId).Select(x => x.Id).ToList();
            if (SelectedBrands == null || SelectedBrands.Count() == 0 || SelectedBrands[0] == 0)
                SelectedBrands = _itemService.GetBrandsForUserCustomer(UserId, CustId, "Report", StoreId).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(UserId, CustId, SelectedTypes, SelectedCategories, "Report", StoreId).Select(x => x.Id).ToList();
            List<PFAReport> finilised = _reportService.PFAReport(CustId, SelectedDC, SelectedStores);

            int draw = Convert.ToInt32(Request.Form["draw"]);//Page
            int StartIndex = Convert.ToInt32(Request.Form["start"]);
            int PageSize = Convert.ToInt32(Request.Form["length"]);
            int SortCol = Convert.ToInt32(Request.Form["order[0][column]"]);
            string SortDir = Request.Form["order[0][dir]"];
            string SearchField = Request.Form["search[value]"] != null ? Request.Form["search[value]"].FirstOrDefault().ToString().Trim() : "";
            int totalRecords = finilised.Count();
            //List<PFAReport> finilisedFiltered = finilised.Where(y =>  y.StoreCode.Contains(SearchField) || y.StoreName.Contains(SearchField) || y.SKUCode.Contains(SearchField)||y.SKUName.Contains(SearchField));
            List<PFAReport> dis = finilised.Skip(StartIndex).Take(PageSize).ToList();
            JsonResult result = Json(new { data = dis, TotalRecords = totalRecords, TotalDisplayRecords=dis.Count() }, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
            return result;

        }

        [HttpPost]

        public ActionResult PFAReport(List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            int UserId = SessionValues.UserId;
            int CustId = 0;
            int StoreId = 0;
            if (SessionValues.RoleId == 1)
            {
                CustId = SessionValues.LoggedInCustId.HasValue ? SessionValues.LoggedInCustId.Value : 0;
                StoreId = SessionValues.StoreId;
            }
            //int StoreId = 0;
            if (SessionValues.RoleId == 1)
            {
                CustId = SessionValues.LoggedInCustId.HasValue ? SessionValues.LoggedInCustId.Value : 0;
                StoreId = SessionValues.StoreId;
            }
            if (SessionValues.RoleId != 1)
            {
                CustId = SessionValues.LoggedInCustId == null ? 0 : SessionValues.LoggedInCustId.Value;
            }
            var DCs = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Report");
            var Items = _itemService.GetSkuForUserCustomer(UserId, CustId, "Report", StoreId);
            var Brands = _itemService.GetBrandsForUserCustomer(UserId, CustId, "Report", StoreId);
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(UserId, CustId, StoreId);
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(UserId, CustId, StoreId);
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Report").Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Report").Select(x => x.Id).ToList();
            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(UserId, CustId, SelectedTypes, StoreId).Select(x => x.Id).ToList();
            if (SelectedBrands == null || SelectedBrands.Count() == 0 || SelectedBrands[0] == 0)
                SelectedBrands = _itemService.GetBrandsForUserCustomer(UserId, CustId, "Report", StoreId).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(UserId, CustId, SelectedTypes, SelectedCategories, "Report", StoreId).Select(x => x.Id).ToList();
            List<ItemReport> itemReport = _reportService.GetItemReport(CustId, UserId, SelectedDC, SelectedStores, SelectedSKUs, SelectedTypes, SelectedCategories, SelectedBrands);
            ViewBag.Report = itemReport;
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            List<PFAReport> finilised = _reportService.PFAReport(CustId, SelectedDC, SelectedStores);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", null);

            ViewBag.FinalizedReports = finilised.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult PFAExportExcel(List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            int UserId = SessionValues.UserId;
            int CustId = 0;
            int StoreId = 0;
            if (SessionValues.RoleId == 1)
            {
                CustId = SessionValues.LoggedInCustId.HasValue ? SessionValues.LoggedInCustId.Value : 0;
                StoreId = SessionValues.StoreId;
            }
            if (SessionValues.RoleId == 1)
            {
                CustId = SessionValues.LoggedInCustId.HasValue ? SessionValues.LoggedInCustId.Value : 0;
                StoreId = SessionValues.StoreId;
            }
            if (SessionValues.RoleId != 1)
            {
                CustId = SessionValues.LoggedInCustId == null ? 0 : SessionValues.LoggedInCustId.Value;
            }
            var DCs = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Report");
            var Items = _itemService.GetSkuForUserCustomer(UserId, CustId, "Report", StoreId);
            var Brands = _itemService.GetBrandsForUserCustomer(UserId, CustId, "Report", StoreId);
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(UserId, CustId, StoreId);
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(UserId, CustId, StoreId);
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerId(UserId, CustId, "Report").Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresforUserCustomer(UserId, CustId, "Report").Select(x => x.Id).ToList();
            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(UserId, CustId, SelectedTypes, StoreId).Select(x => x.Id).ToList();
            if (SelectedBrands == null || SelectedBrands.Count() == 0 || SelectedBrands[0] == 0)
                SelectedBrands = _itemService.GetBrandsForUserCustomer(UserId, CustId, "Report", StoreId).Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(UserId, CustId, SelectedTypes, SelectedCategories, "Report", StoreId).Select(x => x.Id).ToList();


            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            List<PFAReport> finilised = _reportService.PFAReport(CustId, SelectedDC, SelectedStores);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            //string Title = "Name: PFAReport";
            ViewBag.FinalizedReports = finilised.ToList();

            var finilisedList = finilised.Select(head => new
            {

                head.Customer,
                head.StoreCode,
                head.StoreName,
                head.DCName,
                head.SKUCode,
                head.SKUName,
                head.ItemStatus,
                head.MinimumOrder,
                head.MaximumOrderLimit,
                head.CaseConversion,
                head.UOM,
                head.Category,
                head.SubCategory,
                head.BrandName
            });
            //ExportToExcel(finilised);
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                //string DateCellFormat = "dd/mm/yyyy";
                workSheet.Cells[1, 1].Value = "Customer";
                workSheet.Cells[1, 2].Value = "Store Code";
                workSheet.Cells[1, 3].Value = "Store Name";
                workSheet.Cells[1, 4].Value = "DC_Name";
                workSheet.Cells[1, 5].Value = "SKUCode";

                workSheet.Cells[1, 6].Value = "SKUName";
                workSheet.Cells[1, 7].Value = "Item_Status";
                workSheet.Cells[1, 8].Value = "MinimumOrder";
                workSheet.Cells[1, 9].Value = "MaximumOrderLimit";
                workSheet.Cells[1, 10].Value = "CaseConversion";
                workSheet.Cells[1, 11].Value = "UOM";
                workSheet.Cells[1, 12].Value = "Category";

                workSheet.Cells[1, 13].Value = "SubCategory";
                workSheet.Cells[1, 14].Value = "BrandName";

                using (ExcelRange Rng = workSheet.Cells["A1:AN1"])
                {
                    Rng.Style.Font.Bold = true;
                }
                workSheet.Cells[2, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                //using (ExcelRange Rng = workSheet.Cells["C:C"])
                //{
                //    Rng.Style.Numberformat.Format = DateCellFormat;
                //}
                //using (ExcelRange Rng = workSheet.Cells["E:E"])
                //{
                //    Rng.Style.Numberformat.Format = DateCellFormat;
                //}
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Item Status Report.xlsx");
            }
        }

        #endregion
        #region "Daily Order Report Admin"

        [AdminAuthorization]
        public ActionResult DORAdmin()
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            //ItemCategories
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            //ItemType
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
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
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", null);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", null);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DORAdmin(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report");
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedBrands == null || SelectedBrands.Count() == 0 || SelectedBrands[0] == 0)
                SelectedBrands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report").Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();

            //List<DailyOrderReport> finilised = _reportService.DailyOrderReport(startDate, endDate, custid, userid, SelectedStores, SelectedSKUs);
            List<DailyOrderReport> finilised = _reportService.DailyOrderReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, SelectedLocations, SelectedTypes, SelectedCategories, SelectedBrands);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.FinalizedReports = finilised.ToList();
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", SelectedBrands);
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DORAdminExportExcel(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report");
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedBrands == null || SelectedBrands.Count() == 0 || SelectedBrands[0] == 0)
                SelectedBrands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report").Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();

            List<DailyOrderReport> finilised = _reportService.DailyOrderReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, SelectedLocations, SelectedTypes, SelectedCategories, SelectedBrands);
            string Title = "Name: Daily Order Admin Report";
            string Message = "Store Order Date: From: " + startDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " To: " + endDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "";
            var finilisedList = finilised.Select(head => new
            {
                head.PONumber,
                head.Unique_Reference_Id,
                Store_Order_Date = Convert.ToDateTime(head.Store_Order_Date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),

                head.EnterpriseCode,
                head.EnterpriseName,
                head.Store_Code,
                head.StoreName,
                head.City,
                head.DCName,
                head.PlaceOfSupply,

                head.Item_Code,
                head.item_desc,
                head.ItemCategoryType,
                head.Item_Type,
                head.Brand,
                head.UnitofMasureDescription,
                head.Case_Size,

                head.Original_Ordered_Qty,
                head.Revised_Ordered_Qty,
                head.Ordered_By,
                UploadedDate = Convert.ToDateTime(head.Upload_Date).ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                head.Status
            });
            //ExportToExcel(finilised);
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells[1, 1].Value = Title;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Cells[2, 1].Value = Message;
                workSheet.Cells[2, 1].Style.Font.Bold = true;
                string DateCellFormat = "dd/mm/yyyy";

                workSheet.Cells[4, 1].Value = "PO Number";
                workSheet.Cells[4, 2].Value = "Unique Reference ID";
                workSheet.Cells[4, 3].Value = "Store Order Date";

                workSheet.Cells[4, 4].Value = "Enterprise Code";
                workSheet.Cells[4, 5].Value = "Enterprise Name";
                workSheet.Cells[4, 6].Value = "Store Code";
                workSheet.Cells[4, 7].Value = "Store Name";
                workSheet.Cells[4, 8].Value = "City";
                workSheet.Cells[4, 9].Value = "DC";
                workSheet.Cells[4, 10].Value = "Place of Supply";

                workSheet.Cells[4, 11].Value = "SKU Code";
                workSheet.Cells[4, 12].Value = "SKU Decsription";
                workSheet.Cells[4, 13].Value = "SKU Category Type";
                workSheet.Cells[4, 14].Value = "SKU Category";
                workSheet.Cells[4, 15].Value = "Brand";
                workSheet.Cells[4, 16].Value = "UOM";
                workSheet.Cells[4, 17].Value = "Case Size";

                workSheet.Cells[4, 18].Value = "Original Order Quantity";
                workSheet.Cells[4, 19].Value = "Revised Order Quantity";
                workSheet.Cells[4, 20].Value = "Ordered By";
                //workSheet.Cells[4, 12].Value = "Dc";
                workSheet.Cells[4, 21].Value = "System DateTime";
                workSheet.Cells[4, 22].Value = "Status";

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
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Daily Order Admin Report.xlsx");
            }
        }
        #endregion
        #region DSRBasic
        [AdminAuthorization]
        public ActionResult DSRAdmin()
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            //ItemCategories
            List<ItemCategoryView> Itemcategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            //ItemType
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
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
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", null);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", null);
            ViewBag.ItemCategories = new MultiSelectList(Itemcategories.ToList(), "Id", "CategoryName", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", null);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DSRAdmin(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report");
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedBrands == null || SelectedBrands.Count() == 0 || SelectedBrands[0] == 0)
                SelectedBrands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report").Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();

            List<DailySalesReport> finilised = _reportService.DailySalesBasicAdminReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, SelectedLocations, SelectedTypes, SelectedCategories, SelectedBrands);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.FinalizedReports = finilised.ToList();
            ViewBag.Brands = new MultiSelectList(Brands.ToList(), "Id", "BrandName", SelectedBrands);
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DSRAdminExportExcel(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, List<int> SelectedLocations, List<int> SelectedTypes, List<int> SelectedCategories, List<int> SelectedBrands)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid, "Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            List<CategoryTypeView> ItemType = _itemService.GetItemTypeByCustomer(userid, custid);
            List<ItemCategoryView> itemCategories = _itemService.GetItemCategoriesByCustomer(userid, custid);
            var Brands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report");
            if (SelectedLocations == null || SelectedLocations.Count() == 0 || SelectedLocations[0] == 0)
                SelectedLocations = Locations.Select(x => x.LocationId).ToList();
            if (SelectedDC == null || SelectedDC.Count() == 0 || SelectedDC[0] == 0)
                SelectedDC = SelectedDC = _wareHouseService.GetWareHouseForCustomerIdLocations(userid, custid, SelectedLocations).Select(x => x.Id).ToList();
            if (SelectedStores == null || SelectedStores.Count() == 0 || SelectedStores[0] == 0)
                SelectedStores = _storeService.GetStoresForWareHouse(userid, custid, SelectedDC, "Report", SelectedLocations).Select(x => x.Id).ToList();

            if (SelectedTypes == null || SelectedTypes.Count() == 0 || SelectedTypes[0] == 0)
                SelectedTypes = ItemType.Select(x => x.Id).ToList();
            if (SelectedCategories == null || SelectedCategories.Count() == 0 || SelectedCategories[0] == 0)
                SelectedCategories = _itemService.GetItemCategoriesByCustomerForItemTypes(userid, custid, SelectedTypes).Select(x => x.Id).ToList();
            if (SelectedBrands == null || SelectedBrands.Count() == 0 || SelectedBrands[0] == 0)
                SelectedBrands = _itemService.GetBrandsForUserCustomer(userid, custid, "Report").Select(x => x.Id).ToList();
            if (SelectedSKUs == null || SelectedSKUs.Count() == 0 || SelectedSKUs[0] == 0)
                SelectedSKUs = _itemService.GetSkuForUserCustomerItemCategory(userid, custid, SelectedTypes, SelectedCategories, "Report").Select(x => x.Id).ToList();

            List<DailySalesReport> finilised = _reportService.DailySalesBasicAdminReport(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, SelectedLocations, SelectedTypes, SelectedCategories, SelectedBrands);
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", SelectedLocations);
            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "Name", SelectedTypes);
            ViewBag.ItemCategories = new MultiSelectList(itemCategories.ToList(), "Id", "CategoryName", SelectedCategories);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            ViewBag.FinalizedReports = finilised.ToList();

            var finilisedList = finilised.Select(head => new
            {
                head.PONumber,
                head.Unique_Reference_Id,

                Store_Order_Date = Convert.ToDateTime(head.Store_Order_Date).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                UploadedDate = Convert.ToDateTime(head.Upload_Date).ToString("dd/MM/yyyy hh:mm", CultureInfo.InvariantCulture),
                head.Order_Type,

                head.EnterpriseCode,
                head.EnterpriseName,
                head.Store_Code,
                head.StoreName,
                head.City,
                head.PlaceOfSupply,

                head.Item_Code,
                head.Item_Desc,
                head.ItemCategoryType,
                head.Item_Type,
                head.BrandName,
                head.UnitofMasureDescription,
                head.Case_Size,

                head.Original_Ordered_Qty,
                head.Revised_Ordered_Qty,

                head.OrderNo,
                PODate = head.PODate.HasValue ? Convert.ToDateTime(head.PODate.Value).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                head.Rfpl_Ordered_Qty,
                head.Order_Status,
                head.Dc_Name,

                head.SoNumber,
                head.SoQuantity,

                head.Invoice_Number,
                head.Invoice_Date,
                head.Invoice_Quantity,
                head.Invoice_Amount,

                head.RAQuantity,
                head.RAAmount,
                head.FillRateGap,

                head.Taken_Days,
                head.Slab
            });
            //ExportToExcel(finilised);
            using (var excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                string DateCellFormat = "dd/mm/yyyy";
                workSheet.Cells[1, 1].Value = "PO Number";
                workSheet.Cells[1, 2].Value = "Unique Reference ID";

                workSheet.Cells[1, 3].Value = "Store Order Date";
                workSheet.Cells[1, 4].Value = "System DateTime";
                workSheet.Cells[1, 5].Value = "Order Type";

                workSheet.Cells[1, 6].Value = "Enterprise Code";
                workSheet.Cells[1, 7].Value = "Enterprise Name";
                workSheet.Cells[1, 8].Value = "Store Code";
                workSheet.Cells[1, 9].Value = "Store Name";
                workSheet.Cells[1, 10].Value = "City";
                workSheet.Cells[1, 11].Value = "Place of Supply";

                workSheet.Cells[1, 12].Value = "SKU Code";
                workSheet.Cells[1, 13].Value = "SKU Description";
                workSheet.Cells[1, 14].Value = "SKU Category Type";
                workSheet.Cells[1, 15].Value = "SKU Category";
                workSheet.Cells[1, 16].Value = "Brand";
                workSheet.Cells[1, 17].Value = "UOM";
                workSheet.Cells[1, 18].Value = "MOQ";

                workSheet.Cells[1, 19].Value = "Original Order Quantity";
                workSheet.Cells[1, 20].Value = "Revised Order Quantity";

                workSheet.Cells[1, 21].Value = "PO Number";
                workSheet.Cells[1, 22].Value = "PO Date";
                workSheet.Cells[1, 23].Value = "Rfpl Order Quantity";
                workSheet.Cells[1, 24].Value = "Order Status";
                workSheet.Cells[1, 25].Value = "DC";

                workSheet.Cells[1, 26].Value = "So Number";
                workSheet.Cells[1, 27].Value = "So Quantity";

                workSheet.Cells[1, 28].Value = "Invoice Number";
                workSheet.Cells[1, 29].Value = "Invoice Date";
                workSheet.Cells[1, 30].Value = "Invoice Quantity";
                workSheet.Cells[1, 31].Value = "Invoice Amount";

                workSheet.Cells[1, 32].Value = "RA Quantity";
                workSheet.Cells[1, 33].Value = "RA Amount";

                workSheet.Cells[1, 34].Value = "Fill Rate Gap";

                workSheet.Cells[1, 35].Value = "Taken Days";
                workSheet.Cells[1, 36].Value = "Slab";
                using (ExcelRange Rng = workSheet.Cells["A1:AN1"])
                {
                    Rng.Style.Font.Bold = true;
                }
                workSheet.Cells[2, 1].LoadFromCollection(finilisedList, PrintHeaders: false, TableStyle: OfficeOpenXml.Table.TableStyles.None);
                using (ExcelRange Rng = workSheet.Cells["C:C"])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }
                using (ExcelRange Rng = workSheet.Cells["E:E"])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                return File(excel.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Daily Sales Report.xlsx");
            }
        }

        #endregion

    }
}