using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using DbLayer.Service;
using StoreOrderingDashBoard.Models;
using System.Web.Script.Serialization;
using BAL;

namespace StoreOrderingDashBoard.Controllers
{
    public class NetSuitController : Controller
    {
        private readonly IWareHouseService _wareHouseService;
        private readonly IAuditService _auditService;
        public NetSuitController(IWareHouseService wareHouseService,IAuditService auditService)
        {
            _wareHouseService = wareHouseService;
            _auditService = auditService;
        }
        // GET: NetSuit
        [AdminAuthorization]
        public ActionResult Index()
        {
            ViewBag.NetSuit = _wareHouseService.NetSuitMapCls();
            return View();
        }

        [AdminAuthorization]
        public ActionResult AddNetSuit()
        {
            return View();
        }

        [AdminAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNetSuit(NetSuitMapCls netSuitMapCls)
        {
            netSuitMapCls.CreateBy = SessionValues.UserId;
            netSuitMapCls.CreatedOn = DateTime.Now;
            var affected = _wareHouseService.InsertNetsuit(netSuitMapCls);
            if(affected>0)
            {
                try
                {
                    var auditJavascript = new JavaScriptSerializer();
                    var LocationAudit = auditJavascript.Serialize(netSuitMapCls);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), LocationAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                }
                catch(Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        [AdminAuthorization]
        public ActionResult EditNetsuit(int Id)
        {
            var netsuit = _wareHouseService.NetSuitMapCl(Id);
            return View(netsuit);
        }

        [AdminAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditNetsuit(NetSuitMapCls netSuitMapCls)
        {
            var netsuit = _wareHouseService.NetSuitMapCl(netSuitMapCls.Id);
            netsuit.Customer = netSuitMapCls.Customer;
            netsuit.Department = netSuitMapCls.Department;
            netsuit.Class = netSuitMapCls.Class;
            netsuit.Division = netSuitMapCls.Division;
            netsuit.TransactionType = netSuitMapCls.TransactionType;
            netsuit.ModifiedBy = SessionValues.UserId;
            netsuit.ModifiedOn = DateTime.Now;
            var affected = _wareHouseService.UpdateNetsuit(netsuit);
            if(affected>0)
            {
                try
                {
                    var auditJavascript = new JavaScriptSerializer();
                    var LocationAudit = auditJavascript.Serialize(netSuitMapCls);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), LocationAudit, Request.Url.OriginalString.ToString(),SessionValues.UserId, null);
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
        public JsonResult DeleteNetsuit(int Id)
        {
            bool cond = _wareHouseService.DeleteNetsuit(Id) > 0 ? true : false;
            try
            {
                var auditJavascript = new JavaScriptSerializer();
                var LocationAudit = auditJavascript.Serialize("NetsuitId: " + Id + ", IsDeleted:" + cond);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), LocationAudit, Request.Url.OriginalString.ToString(),SessionValues.UserId, null);
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