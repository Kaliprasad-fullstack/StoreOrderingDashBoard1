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
    public class SubCategoryController : Controller
    {

        private readonly IItemService _itemService;
        private readonly IAuditService _auditService;
        public SubCategoryController(IItemService itemService, IAuditService auditService)
        {
            _auditService = auditService;
            _itemService = itemService;
        }

        // GET: SubCategory
        [AdminAuthorization]
        public ActionResult Index()
        {
            ViewBag.SubCat = _itemService.SubCategories();
            return View();
        }

        [AdminAuthorization]
        public ActionResult InsertSubCategory()
        {

            return View();
        }

        [AdminAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InsertSubCategory(SubCategory category)
        {
            category.Created = DateTime.Now;
            category.CreatedBy = SessionValues.UserId;
            var affected = _itemService.InsertSubCategory(category);
            if (affected > 0)
            {
                var auditJavascript = new JavaScriptSerializer();
                var SubCategoryAudit = auditJavascript.Serialize(category);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), SubCategoryAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                return RedirectToAction("Index");
            }
            return View();
        }

        [AdminAuthorization]
        public ActionResult EditSubCategory(int Id)
        {
            var category = _itemService.SubCategorymaster(Id);
            return View(category);
        }

        [AdminAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSubCategory(SubCategory category)
        {
            var cat = _itemService.SubCategorymaster(category.Id);
            cat.Name = category.Name;
            cat.Modified = DateTime.Now;
            cat.ModifiedBy = SessionValues.UserId;
            var affected = _itemService.UpdateSubCategory(cat);
            if (affected > 0)
            {
                var auditJavascript = new JavaScriptSerializer();
                var SubCategoryAudit = auditJavascript.Serialize(category);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), SubCategoryAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        [AdminApiAuthorization]
        public JsonResult DeleteSubCategory(int id)
        {
            bool cond = _itemService.DeleteSubCategory(id) > 0 ? true : false;
            try
            {
                var auditJavascript = new JavaScriptSerializer();
                var SubCategoryAudit = auditJavascript.Serialize("SubCategoryId: " + id + ", IsDeleted: " + cond);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), SubCategoryAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
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