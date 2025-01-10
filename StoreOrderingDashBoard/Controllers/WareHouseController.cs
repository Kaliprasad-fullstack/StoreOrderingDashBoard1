using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DbLayer.Service;
using DAL;
using StoreOrderingDashBoard.Models;
using System.Web.Script.Serialization;
using BAL;

namespace StoreOrderingDashBoard.Controllers
{
    public class WareHouseController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IWareHouseService _wareHouseService;
        private readonly ILocationService _locationService;
        private readonly IAuditService _auditService;
        public WareHouseController(ICustomerService customerService, IWareHouseService wareHouseService, ILocationService locationService, IAuditService auditService)
        {
            _customerService = customerService;
            _wareHouseService = wareHouseService;
            _locationService = locationService;
            _auditService = auditService;
        }
        // GET: WareHouse
        [AdminAuthorization]
        public ActionResult Index()
        {
            ViewBag.WareHouseDcs = _wareHouseService.wareHouseDCs();
            return View();
        }

        [AdminAuthorization]
        public ActionResult AddWarehouse()
        {
            ViewBag.Companies = _customerService.GetCompanies();
            ViewBag.Location = _locationService.GetAllLocations();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorization]
        public ActionResult AddWarehouse(WareHouseDC wareHouseDC)
        {
            wareHouseDC.Company = _wareHouseService.Company(wareHouseDC.Company.Id);
            wareHouseDC.CreatedOn = DateTime.Now;
            var affected = _wareHouseService.InsertWareHouse(wareHouseDC);
            if (affected > 0)
            {
                try
                {
                    var auditJavascript = new JavaScriptSerializer();
                    var WareHouse = new WareHouseDC(wareHouseDC.Id, wareHouseDC.Name, wareHouseDC.LocationId, wareHouseDC.Company.Id, wareHouseDC.PermitableCls, wareHouseDC.Stores);
                    var WareHouseAudit = auditJavascript.Serialize(WareHouse);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), WareHouseAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    throw ex;
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        [AdminAuthorization]
        public ActionResult EditWareHouse(int Id)
        {
            var warehouse = _wareHouseService.WareHouseDC(Id);
            ViewBag.Companies = _customerService.GetCompanies();
            ViewBag.Location = _locationService.GetAllLocations();
            return View(warehouse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorization]
        public ActionResult EditWareHouse(WareHouseDC wareHouseDC)
        {
            var affected = _wareHouseService.UpdateWareHouse(wareHouseDC);
            if (affected > 0)
            {
                try
                {
                    var auditJavascript = new JavaScriptSerializer();
                    var WareHouse = new WareHouseDC(wareHouseDC.Id, wareHouseDC.Name, wareHouseDC.LocationId, wareHouseDC.Company.Id, wareHouseDC.PermitableCls, wareHouseDC.Stores);
                    var WareHouseAudit = auditJavascript.Serialize(WareHouse);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), WareHouseAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    throw ex;
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        [AdminApiAuthorization]
        public JsonResult DeleteWareHouse(int Id)
        {
            bool cond = _wareHouseService.DeleteWareHouse(Id) > 0 ? true : false;
            try
            {
                var auditJavascript = new JavaScriptSerializer();
                var WareHouseAudit = auditJavascript.Serialize("wareHouseDCId: " + Id + ", IsDeleted:" + cond);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), WareHouseAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            return Json(cond, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetWareHouses()
        {
            int LoggedInUserId = SessionValues.UserId;
            int LoggedInCustomer = Convert.ToInt32(SessionValues.LoggedInCustId);
            List<WareHouseDCView> dCs = _wareHouseService.GetWareHouseForCustomerId(LoggedInUserId, LoggedInCustomer, "Report");
            return Json(new { data = dCs }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetWareHousesStoreItem()
        {
            int LoggedInUserId = SessionValues.UserId;
            int LoggedInCustomer = Convert.ToInt32(SessionValues.LoggedInCustId);
            List<WareHouseDCView2> dCs = _wareHouseService.GetWareHousesStoreItem(LoggedInUserId, LoggedInCustomer);
            return Json(new { data = dCs }, JsonRequestBehavior.AllowGet);
        }
    }
}