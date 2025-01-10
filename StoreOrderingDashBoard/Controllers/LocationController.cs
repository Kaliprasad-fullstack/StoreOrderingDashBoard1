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
    public class LocationController : Controller
    {
        private readonly ILocationService _locationService;
        private readonly IAuditService _auditService;

        public LocationController(ILocationService locationService, IAuditService auditService)
        {
            _locationService = locationService;
            _auditService = auditService;
        }
        // GET: Location
        [AdminAuthorization]
        public ActionResult Index()
        {
            ViewBag.location = _locationService.GetAllLocations();
            return View();
        }

        [AdminAuthorization]
        public ActionResult AddLocation()
        {
            ViewBag.Regions = _locationService.GetAllRegions();
            var Location = new Locations();
            Location.Region = new Region();
            return View(Location);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorization]
        public ActionResult AddLocation(Locations locations)
        {
            if (ModelState.IsValid)
            {
                //wareHouseDC.Company = _wareHouseService.Company(wareHouseDC.Company.Id);
                locations.Region = _locationService.GetRegionById(locations.Region.Id);
                locations.Created = DateTime.Now;
                locations.CreatedBy = SessionValues.UserId;
                var affected = _locationService.AddLocation(locations);
                if (affected > 0)
                {
                    try
                    {
                        var Location = new Locations(locations.LocationId, locations.Name, locations.Pincode, locations.Region, locations.IsDeleted, locations.Users, locations.WareHouseDCs, locations.Stores);
                        var auditJavascript = new JavaScriptSerializer();
                        var LocationAudit = auditJavascript.Serialize(Location);
                        _auditService.InsertAudit(HelperCls.GetIPAddress(), LocationAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                    }
                    catch (Exception ex)
                    {
                        HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    }
                    return RedirectToAction("Index");
                }
            }

            ViewBag.Regions = _locationService.GetAllRegions();
            return View();
        }

        [AdminAuthorization]
        public ActionResult EditLocation(int Id)
        {
            var warehouse = _locationService.GetLocationById(Id);
            ViewBag.Regions = _locationService.GetAllRegions();
            return View(warehouse);
        }

        [HttpPost]
        [AdminAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult EditLocation(Locations locations)
        {
            var loc = _locationService.GetLocationById(locations.LocationId);
            loc.Name = locations.Name;
            loc.Pincode = locations.Pincode;
            loc.Modified = DateTime.Now;
            loc.Region = _locationService.GetRegionById(locations.Region.Id);
            loc.ModifiedBy = SessionValues.UserId;
            var affected = _locationService.UpdateLocation(loc);
            if (affected > 0)
            {
                try
                {
                    var Location = new Locations(loc.LocationId, loc.Name, loc.Pincode, loc.Region, loc.IsDeleted, loc.Users, loc.WareHouseDCs, loc.Stores);
                    var auditJavascript = new JavaScriptSerializer();
                    var LocationAudit = auditJavascript.Serialize(Location);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), LocationAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
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

        [AdminApiAuthorization]
        [HttpPost]
        public JsonResult DeleteLocation(int Id)
        {
            bool cond = _locationService.DeleteLocation(Id) > 0 ? true : false;
            try
            {
                var auditJavascript = new JavaScriptSerializer();
                var LocationAudit = auditJavascript.Serialize("LocationId: " + Id + ", IsDeleted: " + cond);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), LocationAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            return Json(cond, JsonRequestBehavior.AllowGet);
        }
    }
}