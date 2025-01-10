using BAL;
using DAL;
using DbLayer.Service;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using StoreOrderingDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
    

namespace StoreOrderingDashBoard.Controllers
{
    public class ReportChartsController : Controller
    {

        
        private readonly IReportChartsServices _reportChartsService;
        private readonly IOrderService _orderService;
        private readonly IWareHouseService _wareHouseService;
        private readonly IStoreService _storeService;
        private readonly IItemService _itemService;
        private readonly IReportService _reportService;
        // GET: Charts

        public ReportChartsController(IReportChartsServices reportChartsService, IOrderService orderService, IWareHouseService wareHouseService, IStoreService storeService, IItemService itemService, IReportService reportService)
        {
            _reportChartsService = reportChartsService;
            _orderService = orderService;
            _wareHouseService = wareHouseService;
            _storeService = storeService;
            _itemService = itemService;
            _reportService = reportService;
        }
        public ActionResult Index()
        {
            return View();
        }

        #region "Ageing Report By Deepa 18/12/2019"
        [AdminAuthorization]
        public ActionResult AgeingReportCharts()
        {
            
            //List<int> SelectedDC=null;
            //List<int> SelectedStores=null;
            //List<int> SelectedSKUs=null;
            //SelectedDC = SelectedDC != null && SelectedDC.Count() > 0 ? SelectedDC : DCs.Select(x => x.Id).ToList();
            //SelectedStores = SelectedStores != null && SelectedStores.Count() > 0 ? SelectedStores : Stores.Select(x => x.Id).ToList();
            //SelectedSKUs = SelectedSKUs != null && SelectedSKUs.Count() > 0 ? SelectedSKUs : Items.Select(x => x.Id).ToList();

            DateTime now = DateTime.Now;
            int Days = (now.Day) - 1;
            var startDate = now.AddDays(-Days).Date;

            var endDate = startDate.AddDays(1).Date;
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");

            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = null;

            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);

            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid,"Report");
            var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");

            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", DCs.Select(x => x.Id).ToList());
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", Stores.Select(x => x.Id).ToList());
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", Items.Select(x => x.Id).ToList());

            //-------------------------------------------
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);
            List<ItemSubTypeByCustomer> ItemSubTypeByCustomer = _itemService.GetItemSubTypeByCustomer(userid, custid);
            List<ItemCategoryView> ItemType = _itemService.GetItemCategoriesByCustomer(userid, custid);

            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", Locations.Select(x => x.LocationId).ToList());
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "CategoryName", ItemType.Select(x => x.Id).ToList());
            ViewBag.ItemCategories = new MultiSelectList(ItemSubTypeByCustomer.ToList(), "Id", "CategoryName", ItemSubTypeByCustomer.Select(x => x.Id).ToList());

            //-------------------------------------------

            return View();
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult AgeingReportCharts(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, string Status, List<int> City, List<int> ItemType, List<int> ItemSubType, string AgeCompareFrom,string AgeCompareTo, int CustomRangeFrom, int CustomRangeTO, string IsCustomRange,  int OrderedQuantityFrom, int OrderedQuantityTo, string ByOrderDateOrInvoiceDate)  
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = UniqueReferenceID;
            List<AgeingReportChart> FinalData = _reportChartsService.AgeingReportChartsSearch(startDate, endDate, custid, userid, SelectedDC, SelectedStores, SelectedSKUs,0, City, ItemType, ItemSubType, AgeCompareFrom,AgeCompareTo, CustomRangeFrom, CustomRangeTO,IsCustomRange, OrderedQuantityFrom,  OrderedQuantityTo,  ByOrderDateOrInvoiceDate);

            List<NameValuePair> AgeingPair = new List<NameValuePair>();


            #region "Get the AgePair and StatusPair"
            var StatusPair = FinalData
               .GroupBy(x => x.StatusName)
                .Select(x => new
               {
                   value = x.Count(),
                   name = x.Key,
                   selected = false,
               }).ToList().Where(a=>a.value>0);

            if (Status != "")
                {
                    FinalData = FinalData.Where(a => a.StatusName.Contains(Status)).ToList();
                }

            var AgeGroup = FinalData
                .GroupBy(x => x.DefaultCoulmn)

                .Select(x => new
                {
                      A0_5 = x.Sum(y=>y.A0_5),
                      A6_10 = x.Sum(y => y.A6_10),
                      A11_15 = x.Sum(y => y.A11_15),
                      Above_16 = x.Sum(y => y.Above_16),
                      CustomRange = x.Sum(y => y.CustomRange),
                }).ToList();

            
            if (AgeGroup.Count() > 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    NameValuePair age = new NameValuePair();
                    if (i == 0)
                    {
                        age.name = "0 to 5";
                        age.value = AgeGroup[0].A0_5;
                    }
                    if (i == 1)
                    {
                        age.name = "6 to 10";
                        age.value = AgeGroup[0].A6_10;
                    }

                    if (i == 2)
                    {
                        age.name = "11 to 15";
                        age.value = AgeGroup[0].A11_15;
                    }

                    if (i == 3)
                    {
                        age.name = "Above 16";
                        age.value = AgeGroup[0].Above_16;
                    }
                    if (i == 4)
                    {
                        age.name = "Custom Range:"+CustomRangeFrom+" to "+CustomRangeTO;
                        age.value = AgeGroup[0].CustomRange;
                    }
                    if (age.value > 0)
                    {
                        AgeingPair.Add(age);
                    }
                }
            }
            #endregion
           // ViewBag.ReportType = ReportType;
            return Json(new { AgeingPair, StatusPair }, JsonRequestBehavior.AllowGet);
            
        }
     
        [HttpPost]
        public JsonResult GetCityAndDCWiseData(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, string AgeRange, string Status, List<int> City, List<int> ItemType, List<int> ItemSubType, string AgeCompareFrom, string AgeCompareTo, int CustomRangeFrom, int CustomRangeTO, string IsCustomRange, int OrderedQuantityFrom, int OrderedQuantityTo, string ByOrderDateOrInvoiceDate)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            List<AgeingReportChart> Data = _reportChartsService.AgeingReportChartsSearch(startDate, endDate, custid, userid, SelectedDC, SelectedStores, SelectedSKUs,0, City, ItemType, ItemSubType, AgeCompareFrom, AgeCompareTo, CustomRangeFrom, CustomRangeTO, IsCustomRange, OrderedQuantityFrom, OrderedQuantityTo, ByOrderDateOrInvoiceDate);
            int A=0, B=5;

            if (AgeRange == "6 to 10")
            {
                A = 6;
                B = 10;
            }
            if (AgeRange == "11 to 15")
            {
                A = 11;
                B = 15;
            }
            if (AgeRange == "Above 16")
            {
                A = 16;
                B = 500;
            }
           

            List<AgeingReportChart> FinalData= new List<AgeingReportChart>();
            FinalData = Data;


            if (Status!="")
            {
                FinalData = FinalData.Where(a => a.StatusName.Contains(Status)).ToList();
            }

            if ((AgeRange != "") && (IsCustomRange == "N"))
            {
                FinalData = FinalData.Where(a => a.PendingDays >= A && a.PendingDays <= B).ToList();
            }

         
            var DCPair = FinalData
              .GroupBy(x => x.DCName)
              .Select(x => new
              {
                  value = x.Count(),
                  name = x.Key,
              }).ToList().Where(a=>a.value>0);

            var CityPair = FinalData
              .GroupBy(x => x.City)
              .Select(x => new
              {
                  value = x.Count(),
                  name = x.Key,
              }).ToList().Where(a => a.value > 0);

            return Json(new {DCPair,CityPair}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetStoreAndItemCategoryData(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, string AgeRange, string Status,string City,string DC, List<int> CityList, List<int> ItemType, List<int> ItemSubType, string AgeCompareFrom, string AgeCompareTo, int CustomRangeFrom, int CustomRangeTO, string IsCustomRange, int OrderedQuantityFrom, int OrderedQuantityTo, string ByOrderDateOrInvoiceDate)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            List<AgeingReportChart> Data = _reportChartsService.AgeingReportChartsSearch(startDate, endDate, custid, userid, SelectedDC, SelectedStores, SelectedSKUs, 0,CityList, ItemType, ItemSubType, AgeCompareFrom, AgeCompareTo, CustomRangeFrom, CustomRangeTO, IsCustomRange, OrderedQuantityFrom, OrderedQuantityTo, ByOrderDateOrInvoiceDate);
            int A = 0, B = 5;

            if (AgeRange == "6 to 10")
            {
                A = 6;
                B = 10;
            }
            if (AgeRange == "11 to 15")
            {
                A = 11;
                B = 15;
            }
            if (AgeRange == "Above 16")
            {
                A = 16;
                B = 500;
            }
            
            List<AgeingReportChart> FinalData = new List<AgeingReportChart>();
            FinalData = Data;

            if (Status != "")
            {
                FinalData = FinalData.Where(a => a.StatusName.Contains(Status)).ToList();
            }

            if ((AgeRange != "")&& (IsCustomRange == "N"))
            {
                FinalData = FinalData.Where(a => a.PendingDays >= A && a.PendingDays <= B).ToList();
            }
            if(City!="")
            {
                FinalData = FinalData.Where(a => a.City.Contains(City)).ToList();
            }
           else if (DC != "")
            {
                FinalData = FinalData.Where(a => a.DCName.Contains(DC)).ToList();
            }

            var StoreCodePair = FinalData
               //.GroupBy(x => x.Store_Code,x=>x.StoreName) 
               .GroupBy(x => new { x.Store_Code, x.StoreName})

               .Select(x => new
               {
                   value = x.Count(),
                   name = x.Key.Store_Code,
                   percentage= Math.Round((Convert.ToDecimal(x.Count() * 100) / FinalData.Count()), 2),
                   Store_Name=x.Key.StoreName,
               }).ToList().Where(a=>a.value>0);

            var CategoryPair = FinalData
             .GroupBy(x => x.SubCategoryName)
             .Select(x => new
             {
                 // value = x.Sum(a => a.Quantity),
                 value = x.Count(),
                 name = x.Key,
                 percentage = Math.Round((Convert.ToDecimal(x.Count() * 100) / FinalData.Count()),2),
             }).Where(a => a.value > 0);

            return Json(new { StoreCodePair, CategoryPair }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetStoreWiseItemCategoryData(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, string AgeRange, string Status, string City, string DC,string Store, List<int> CityList, List<int> ItemType, List<int> ItemSubType, string AgeCompareFrom, string AgeCompareTo, int CustomRangeFrom, int CustomRangeTO, string IsCustomRange, int OrderedQuantityFrom, int OrderedQuantityTo, string ByOrderDateOrInvoiceDate)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            List<AgeingReportChart> Data = _reportChartsService.AgeingReportChartsSearch(startDate, endDate, custid, userid, SelectedDC, SelectedStores, SelectedSKUs, 0, CityList,ItemType, ItemSubType, AgeCompareFrom, AgeCompareTo, CustomRangeFrom, CustomRangeTO, IsCustomRange, OrderedQuantityFrom, OrderedQuantityTo, ByOrderDateOrInvoiceDate);
            int A = 0, B = 5;

            if (AgeRange == "6 to 10")
            {
                A = 6;
                B = 10;
            }
            if (AgeRange == "11 to 15")
            {
                A = 11;
                B = 15;
            }
            if (AgeRange == "Above 16")
            {
                A = 16;
                B = 500;
            }

            City=City.Replace("\n", string.Empty);
            City = City.Trim();

            DC = DC.Replace("\n", string.Empty);
            DC = DC.Trim();

            Store = Store.Replace("\n", string.Empty);
            Store = Store.Trim();
            List<AgeingReportChart> FinalData = new List<AgeingReportChart>();
            FinalData = Data;

            if (Status != "")
            {
                FinalData = FinalData.Where(a => a.StatusName.Contains(Status)).ToList();
            }

            if ((AgeRange != "")&& (IsCustomRange == "N"))
            {
                FinalData = FinalData.Where(a => a.PendingDays >= A && a.PendingDays <= B).ToList();
            }
            if (City != "")
            {
                FinalData = FinalData.Where(a => a.City.Contains(City)).ToList();
            }
            else if (DC != "")
            {
                FinalData = FinalData.Where(a => a.DCName.Contains(DC)).ToList();
            }

            if(Store!="")
            {
                FinalData = FinalData.Where(a => a.Store_Code.Contains(Store)).ToList();
            }
           
            var CategoryPair = FinalData
             .GroupBy(x => x.SubCategoryName)
             .Select(x => new
             {
                 value = x.Count(),
                 name = x.Key,
                 percentage = Math.Round((Convert.ToDecimal(x.Count() * 100) / FinalData.Count()), 2),
                 Store_Name="",
             }).ToList().Where(a => a.value > 0);

            return Json(new {CategoryPair }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetItemCategoryWiseStoreData(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, string AgeRange, string Status, string City, string DC, string ItemCategory, List<int> CityList, List<int> ItemType, List<int> ItemSubType, string AgeCompareFrom, string AgeCompareTo, int CustomRangeFrom, int CustomRangeTO, string IsCustomRange, int OrderedQuantityFrom, int OrderedQuantityTo, string ByOrderDateOrInvoiceDate)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            List<AgeingReportChart> Data = _reportChartsService.AgeingReportChartsSearch(startDate, endDate, custid, userid, SelectedDC, SelectedStores, SelectedSKUs, 0,CityList, ItemType, ItemSubType,  AgeCompareFrom, AgeCompareTo, CustomRangeFrom, CustomRangeTO, IsCustomRange, OrderedQuantityFrom, OrderedQuantityTo, ByOrderDateOrInvoiceDate);
            int A = 0, B = 5;

            if (AgeRange == "6 to 10")
            {
                A = 6;
                B = 10;
            }
            if (AgeRange == "11 to 15")
            {
                A = 11;
                B = 15;
            }
            if (AgeRange == "Above 16")
            {
                A = 16;
                B = 500;
            }

            City = City.Replace("\n", string.Empty);
            City = City.Trim();

            DC = DC.Replace("\n", string.Empty);
            DC = DC.Trim();

            ItemCategory = ItemCategory.Replace("\n", string.Empty);
            ItemCategory = ItemCategory.Trim();

            List<AgeingReportChart> FinalData = new List<AgeingReportChart>();
            FinalData = Data;

            if (Status != "")
            {
                FinalData = FinalData.Where(a => a.StatusName.Contains(Status)).ToList();
            }

            if ((AgeRange != "") &&(IsCustomRange=="N"))
            {
                FinalData = FinalData.Where(a => a.PendingDays >= A && a.PendingDays <= B).ToList();
            }
            if (City != "")
            {
                FinalData = FinalData.Where(a => a.City.Contains(City)).ToList();
            }
            else if (DC != "")
            {
                FinalData = FinalData.Where(a => a.DCName.Contains(DC)).ToList();
            }

            if (ItemCategory != "")
            {
                FinalData = FinalData.Where(a => a.SubCategoryName.Contains(ItemCategory)).ToList();
            }

            var CategoryPair = FinalData
              .GroupBy(x => new { x.Store_Code, x.StoreName })
             .Select(x => new
             {
                 value = x.Count(),
                 name = x.Key.Store_Code,
                 percentage = Math.Round((Convert.ToDecimal(x.Count() * 100) / FinalData.Count()), 2),
                 Store_Name = x.Key.StoreName,
             }).ToList().Where(a => a.value > 0);

            return Json(new { CategoryPair }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region "Delivery Status report By Deepa 23/12/2019"
        [AdminAuthorization]
        public ActionResult DeliveryStatusReport()
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
            var Stores = _storeService.GetStoresforUserCustomer(userid, custid,"Report");
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

            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", null);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", null);
            return View();
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult DeliveryStatusReport(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, int ReportType)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);

            DateTime now = DateTime.Now;
            string DateCellFormat = "dd-mm-yyyy";
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = UniqueReferenceID;
            List<DeliveryStatusReportChart> finilised = _reportChartsService.DeliveryStatusReportChartsSearch(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, ReportType);
            // ViewBag.FinalizedReports = finilised.ToList();
            //ViewBag.ReportType = ReportType;
            var StatusPair = finilised
               .GroupBy(x => x.Status)

               .Select(x => new
               {
                   value = x.Count(),
                   name = x.Key,
               }).ToList();
            return Json(new { StatusPair }, JsonRequestBehavior.AllowGet);
        }


        #endregion

               
         #region "Chart For Fulfillment Report_ 06/1/2020"
         [AdminAuthorization]
         public ActionResult FillRateReportChart()
         {
             int userid = SessionValues.UserId;
             int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
             var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
             var Stores = _storeService.GetStoresforUserCustomer(userid, custid,"Report");
             var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report");
             DateTime now = DateTime.Now;
            int Days = (now.Day)-1;
             var startDate = now.AddDays(-Days).Date;
            //startDate = startDate.AddMonths(-2); //TO REMOVE
             var endDate = now.AddDays(0).Date;
             string sd = startDate.Date.ToString("yyyy-MM-dd");
             string ed = endDate.Date.ToString("yyyy-MM-dd");
             ViewData["StartDate"] = sd;
             ViewData["EndDate"] = ed;
             ViewData["UniqueReferenceID"] = null;

            List<int> SelectedDC = null;
            List<int> SelectedStores = null;
            List<int> SelectedSKUs = null;

            //-------------------------------------------
            var Locations = _storeService.GetCitiesforUserCustomer(userid, custid);

            List<ItemSubTypeByCustomer> ItemSubTypeByCustomer = _itemService.GetItemSubTypeByCustomer(userid, custid);
            List<ItemCategoryView> ItemType = _itemService.GetItemCategoriesByCustomer(userid, custid);
           
            ViewBag.Locations = new MultiSelectList(Locations.ToList(), "LocationId", "LocationName", Locations.Select(x => x.LocationId).ToList());
            ViewBag.ItemTypes = new MultiSelectList(ItemType.ToList(), "Id", "CategoryName", ItemType.Select(x => x.Id).ToList() );
            ViewBag.ItemCategories = new MultiSelectList(ItemSubTypeByCustomer.ToList(), "Id", "CategoryName", ItemSubTypeByCustomer.Select(x => x.Id).ToList());
           
            //-------------------------------------------

            SelectedDC = SelectedDC != null && SelectedDC.Count() > 0 ? SelectedDC : DCs.Select(x => x.Id).ToList();
            SelectedStores = SelectedStores != null && SelectedStores.Count() > 0 ? SelectedStores : Stores.Select(x => x.Id).ToList();
            SelectedSKUs = SelectedSKUs != null && SelectedSKUs.Count() > 0 ? SelectedSKUs : Items.Select(x => x.Id).ToList();

            ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", SelectedDC);
            ViewBag.Stores = new MultiSelectList(Stores.ToList(), "Id", "StoreCode", SelectedStores);
            ViewBag.Items = new MultiSelectList(Items.ToList(), "Id", "SKUCode", SelectedSKUs);
            return View();
         }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult FillRateReportChart(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, int ReportType, string ByOrderDateOrInvoiceDate, List<int> City, List<int> State, string Invoice_Number, List<int> ItemType, List<int> item_sub_type, int? OrderedQuantityFrom, int? orderQuantityTO, string FillRateCompare, string FillRatePercentage, string FillRateCompareTO, string FillRatePercentageTO, string SelectedFullfillmentType)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = UniqueReferenceID;
            
            //if(City[0]==0)
            //    City= _storeService.GetCitiesforUserCustomer(userid, custid).Select(a=>a.LocationId).ToList<int>();
            //if (item_sub_type[0] == 0)
            //    item_sub_type = _itemService.GetItemSubTypeByCustomer(userid, custid).Select(a => a.Id).ToList<int>();
            //if (ItemType[0] == 0)
            //    ItemType = _itemService.GetItemCategoriesByCustomer(userid, custid).Select(a => a.Id).ToList<int>();

             
            List<FillRateReportChart> FinalData = _reportChartsService.FillRateReportCharts(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, ReportType, ByOrderDateOrInvoiceDate, City, State, Invoice_Number, ItemType, item_sub_type, ((OrderedQuantityFrom!=null)?Convert.ToInt32(OrderedQuantityFrom):0), ((orderQuantityTO != null) ? Convert.ToInt32(orderQuantityTO) : 100000), SelectedFullfillmentType);

            if ((FillRatePercentage != ""))
            {
                if (FillRateCompare == "G")
                {
                    FinalData = FinalData.Where(a => a.FinalFulfilment >= Convert.ToInt32(FillRatePercentage)).ToList();
                }
                if (FillRateCompare == "L")
                {
                    FinalData = FinalData.Where(a => a.FinalFulfilment <= Convert.ToInt32(FillRatePercentage)).ToList();
                }
                if (FillRateCompare == "E")
                {
                    FinalData = FinalData.Where(a => a.FinalFulfilment == Convert.ToInt32(FillRatePercentage)).ToList();
                }
            }
            if ((FillRatePercentage != "") && (FillRatePercentageTO != "")) // TODO rework
            {

                if (FillRateCompareTO == "L")
                {
                    FinalData = FinalData.Where(a => a.FinalFulfilment <= Convert.ToInt32(FillRatePercentageTO)).ToList();
                }
            }
            
            if (FinalData.Count() > 0)
            {
                var CompanyFillRate = FinalData.Where(x => x.ReportSubType.Contains("City")).Average(x => x.FinalFulfilment);

                var DCPair = FinalData
                    .Where(x => x.DCName.Length > 0)
                    .GroupBy(g => g.DCName, r => r.FinalFulfilment)
                   .Select(g => new
                   {
                       name = g.Key,
                       value = Math.Round(g.Average(), 2)
                   }).ToList().Where(a => a.value > 0);

                var CityPair = FinalData
                    .Where(x => x.city != null && x.city.Length > 0)
                 .GroupBy(g => g.city, r => r.FinalFulfilment)
                   .Select(g => new
                   {
                       name = g.Key,
                       value = Math.Round(g.Average(), 2)
                   }).ToList().Where(a => a.value > 0);

                return Json(new { CompanyFillRate, CityPair, DCPair }, JsonRequestBehavior.AllowGet);
            }
            else
            { 
                var CompanyFillRate = 0;
                return Json(new { CompanyFillRate }, JsonRequestBehavior.AllowGet);
            }
        }
                
        [HttpPost]
        public JsonResult GetFillRateStoreWiseData(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, int ReportType, string ByOrderDateOrInvoiceDate, List<int> City, List<int> State, string Invoice_Number, List<int> ItemType, List<int> item_sub_type, int? OrderedQuantityFrom, int? orderQuantityTO, string FillRateCompare, string FillRatePercentage, string FillRateCompareTO, string FillRatePercentageTO,string SelectedFullfillmentType, string ClickCity,  string DC,string Store, string ItemCategory)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = UniqueReferenceID;
            string OuterChartData = "NO";
            if (Store=="" &&ItemCategory=="")
            {
                OuterChartData = "YES";
            }
            if (ClickCity != "")
            {
                City.Clear();
                City.Add(_storeService.GetCitiesforUserCustomer(userid, custid).Where(a => a.LocationName.Contains(ClickCity)).FirstOrDefault().LocationId);
            }
            
            List<FillRateReportChart> FinalData = _reportChartsService.FillRateReportChartsDrillDown(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, ReportType, ByOrderDateOrInvoiceDate, City, State, Invoice_Number, ItemType, item_sub_type, ((OrderedQuantityFrom != null) ? Convert.ToInt32(OrderedQuantityFrom) : 0), ((orderQuantityTO != null) ? Convert.ToInt32(orderQuantityTO) : 100000) ,DC, OuterChartData, SelectedFullfillmentType);


            if ((FillRatePercentage != ""))
            {
                if (FillRateCompare == "G")
                {
                    FinalData = FinalData.Where(a => a.FinalFulfilment >= Convert.ToInt32(FillRatePercentage)).ToList();
                }
                if (FillRateCompare == "L")
                {
                    FinalData = FinalData.Where(a => a.FinalFulfilment <= Convert.ToInt32(FillRatePercentage)).ToList();
                }
                if (FillRateCompare == "E")
                {
                    FinalData = FinalData.Where(a => a.FinalFulfilment == Convert.ToInt32(FillRatePercentage)).ToList();
                }
            }
            if ((FillRatePercentage != "") && (FillRatePercentageTO != "")) // TODO rework
            {

                if (FillRateCompareTO == "L")
                {
                    FinalData = FinalData.Where(a => a.FinalFulfilment <= Convert.ToInt32(FillRatePercentageTO)).ToList();
                }
            }

            // Inner Data of same Chart
            var StorePair = FinalData
                .GroupBy(g => new { g.store_code, g.StoreName }, r => r.FinalFulfilment)
                .Select(g => new
               {
                   name = g.Key.store_code,
                   value =Math.Round( g.Average(),2),
                   Store_name=g.Key.StoreName,
                   selected = false,
               }).ToList().Where(a => a.value > 0 && a.name!="");

            //Outer Data of Same chart
               var ItemCategoryPair=FinalData
               .Where((g => g.Item_Sub_Type.Length > 0 && g.store_code.Contains(Store)))
               .GroupBy(g => g.Item_Sub_Type, r => r.FinalFulfilment)
               .Select(g => new
               {
                  name = g.Key,
                  value =Math.Round( g.Average(),2),
                   selected = false,
               }).ToList().Where(a => a.value > 0); ;

            return Json(new { StorePair, ItemCategoryPair }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFillRateItemCategoryWiseData(DateTime? FromDate, DateTime? ToDate, string UniqueReferenceID, List<int> SelectedDC, List<int> SelectedStores, List<int> SelectedSKUs, int ReportType, string ByOrderDateOrInvoiceDate, List<int> City, List<int> State, string Invoice_Number, List<int> ItemType, List<int> item_sub_type, int? OrderedQuantityFrom, int? orderQuantityTO, string FillRateCompare, string FillRatePercentage, string FillRateCompareTO, string FillRatePercentageTO, string SelectedFullfillmentType, string ClickCity, string DC, string Store, string ItemCategory)
        {
            int userid = SessionValues.UserId;
            int custid = Convert.ToInt32(SessionValues.LoggedInCustId);
            DateTime now = DateTime.Now;
            var startDate = (FromDate == null ? now.AddDays(-1).Date : Convert.ToDateTime(FromDate));
            var endDate = (ToDate == null ? startDate.AddDays(1).Date : Convert.ToDateTime(ToDate));
            string sd = startDate.Date.ToString("yyyy-MM-dd");
            string ed = endDate.Date.ToString("yyyy-MM-dd");
            ViewData["StartDate"] = sd;
            ViewData["EndDate"] = ed;
            ViewData["UniqueReferenceID"] = UniqueReferenceID;
            string OuterChartData = "NO";
            if (Store == "" && ItemCategory == "")
            {
                OuterChartData = "YES";
            }
            if (ClickCity != "")
            {
                City.Clear();
                City.Add(_storeService.GetCitiesforUserCustomer(userid, custid).Where(a => a.LocationName.Contains(ClickCity)).FirstOrDefault().LocationId);
            }
          
            List<FillRateReportChart> FinalData = _reportChartsService.FillRateReportChartsDrillDown(startDate, endDate, custid, userid, UniqueReferenceID, SelectedDC, SelectedStores, SelectedSKUs, ReportType, ByOrderDateOrInvoiceDate, City, State, Invoice_Number, ItemType, item_sub_type, ((OrderedQuantityFrom != null) ? Convert.ToInt32(OrderedQuantityFrom) : 0), ((orderQuantityTO != null) ? Convert.ToInt32(orderQuantityTO) : 100000) ,DC, OuterChartData,SelectedFullfillmentType);

            if ((FillRatePercentage != ""))
            {
                if (FillRateCompare == "G")
                {
                    FinalData = FinalData.Where(a => a.FinalFulfilment >= Convert.ToInt32(FillRatePercentage)).ToList();
                }
                if (FillRateCompare == "L")
                {
                    FinalData = FinalData.Where(a => a.FinalFulfilment <= Convert.ToInt32(FillRatePercentage)).ToList();
                }
                if (FillRateCompare == "E")
                {
                    FinalData = FinalData.Where(a => a.FinalFulfilment == Convert.ToInt32(FillRatePercentage)).ToList();
                }
            }
            if ((FillRatePercentage != "") && (FillRatePercentageTO != "")) // TODO rework
            {

                if (FillRateCompareTO == "L")
                {
                    FinalData = FinalData.Where(a => a.FinalFulfilment <= Convert.ToInt32(FillRatePercentageTO)).ToList();
                }
            }
            // Inner Data of Same chart
            var ItemCategoryPair = FinalData
            .GroupBy(g => g.Item_Sub_Type, r => r.FinalFulfilment)
            .Select(g => new
            {
                name = g.Key,
                value =Math.Round( g.Average(),2),
                selected = false,
            }).ToList().Where(a => a.value > 0 && a.name!="");
            
            // Outer Data of Same Chart
            var StorePair = FinalData
              .Where((g => g.store_code.Length > 0 && g.Item_Sub_Type.Contains(ItemCategory)))
                .GroupBy(g => new { g.store_code, g.StoreName}, r => r.FinalFulfilment)
               .Select(g => new
               {
                   name = g.Key.store_code,
                   value =Math.Round( g.Average(),2),
                   selected = false,
                   Store_Name=g.Key.StoreName,
               }).ToList().Where(a => a.value > 0);

            return Json(new { StorePair, ItemCategoryPair }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Store_Home GetStoreMonthWiseOrderDateQtyLineGraph 26/06/2021
        [HttpPost]
        public ActionResult GetStoreMonthWiseOrderDateQtyLineGraph(int Month, int Year)
        {
            int userid = SessionValues.UserId;
            int custid = SessionValues.LoggedInCustId.Value;
            int storeid = 0;
            if (SessionValues.StoreId != 0)
            {
                storeid = SessionValues.StoreId;
                custid = _storeService.GetStoreById(storeid).CustId;
            }
            
            List<int>  SKUs = _itemService.GetSkuForUserCustomer(userid, custid, "Report", storeid).Select(x => x.Id).ToList();
            List<MonthWiseStoreOrderQty> responses = _reportService.GetStoreMonthWiseOrderDateQty(custid, userid, Month, Year, storeid, SKUs);
            return Json(responses, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}