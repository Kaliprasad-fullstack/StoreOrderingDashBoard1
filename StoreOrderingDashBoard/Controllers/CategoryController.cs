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
    public class CategoryController : Controller
    {
        private readonly IItemService _itemService;
        private readonly IAuditService _auditService;
        public CategoryController(IItemService itemService, IAuditService auditService)
        {
            _itemService = itemService;
            _auditService = auditService;
        }
        // GET: Category
        [AdminAuthorization]
        public ActionResult Index()
        {
            ViewBag.Category = _itemService.Categories();
            return View();
        }

        [AdminAuthorization]
        public ActionResult InsertCategory()
        {
            return View();
        }

        [AdminAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InsertCategory(Category category)
        {
            category.Created = DateTime.Now;
            category.CreatedBy = SessionValues.UserId;
            var affected = _itemService.InsertCategory(category);
            if (affected > 0)
            {
                try
                {
                    var auditJavascript = new JavaScriptSerializer();
                    var categoryAudit = auditJavascript.Serialize(category);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), categoryAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
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
        public ActionResult EditCategory(int Id)
        {
            var category = _itemService.Categorymaster(Id);
            return View(category);
        }

        [AdminAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory(Category category)
        {
            var cat = _itemService.Categorymaster(category.Id);
            cat.CategoryDescription = category.CategoryDescription;
            cat.CaseConversion = category.CaseConversion;
            cat.TotalWeightPerCase = category.TotalWeightPerCase;
            cat.Modified = DateTime.Now;
            cat.ModifiedBy = SessionValues.UserId;
            var affected = _itemService.UpdateCategory(cat);
            if (affected > 0)
            {
                try
                {
                    var auditJavascript = new JavaScriptSerializer();
                    var categoryAudit = auditJavascript.Serialize(category);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), categoryAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
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
        public JsonResult DeleteCategory(int id)
        {
            bool cond = _itemService.DeleteCategory(id) > 0 ? true : false;
            try
            {
                var auditJavascript = new JavaScriptSerializer();
                var categoryAudit = auditJavascript.Serialize("CategoryId: " + id + ", IsDeleted:" + cond);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), categoryAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
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