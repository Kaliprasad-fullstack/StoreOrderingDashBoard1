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
    public class UOMController : Controller
    {

        private readonly IItemService _itemService;
        private readonly IAuditService _auditService;

        public UOMController(IItemService itemService, IAuditService auditService)
        {
            _auditService = auditService;
            _itemService = itemService;
        }

        // GET: UOM
        [AdminAuthorization]
        public ActionResult Index()
        {
            ViewBag.UOM = _itemService.UOMmasters();
            return View();
        }

        [AdminAuthorization]
        public ActionResult UOMInsert()
        {

            return View();
        }

        [HttpPost]
        [AdminAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult UOMInsert(string UOMMaster, string UnitofMasureDescription)
        {
            var uOMmaster = new UOMmaster();
            uOMmaster.UOMMaster = UOMMaster;
            uOMmaster.UnitofMasureDescription = UnitofMasureDescription;
            uOMmaster.Created = DateTime.Now;
            uOMmaster.CreatedBy = SessionValues.UserId;
            var uom = _itemService.InsertUOM(uOMmaster);
            if (uom > 0)
            {
                try
                {
                    var auditJavascript = new JavaScriptSerializer();
                    var UOMAudit = auditJavascript.Serialize(uOMmaster);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), UOMAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
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
        public ActionResult EditUOM(int Id)
        {
            var UOM = _itemService.UOMmaster(Id);
            return View(UOM);
        }

        [HttpPost]
        [AdminAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult EditUOM(int Id, string UOMMaster, string UnitofMasureDescription)
        {
            var UOM = _itemService.UOMmaster(Id);
            UOM.UOMMaster = UOMMaster;
            UOM.UnitofMasureDescription = UnitofMasureDescription;
            UOM.Modified = DateTime.Now;
            UOM.ModifiedBy = SessionValues.UserId;
            var affected = _itemService.UpdateUOM(UOM);
            if (affected > 0)
            {
                try
                {
                    var auditJavascript = new JavaScriptSerializer();
                    var UOMAudit = auditJavascript.Serialize(UOM);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), UOMAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
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
        public ActionResult DeleteUOM(int id)
        {
            bool cond = _itemService.DeleteUOM(id) > 0 ? true : false;
            try
            {
                var auditJavascript = new JavaScriptSerializer();
                var UOMAudit = auditJavascript.Serialize("UOMId: " + id + ", IsDeleted: " + cond);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), UOMAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
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