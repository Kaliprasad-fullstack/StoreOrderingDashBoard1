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
    public class BrandController : Controller
    {

        private readonly IItemService _itemService;
        private readonly IAuditService _auditService;
        public BrandController(IItemService itemService, IAuditService auditService)
        {
            _auditService = auditService;
            _itemService = itemService;
        }

        // GET: SubCategory
        [AdminAuthorization]
        public ActionResult Index()
        {
            List<Brand> brands = _itemService.GetBrands("All");
            ViewBag.brands = brands;
            return View();
        }

        [AdminAuthorization]
        public ActionResult AddBrand()
        {
            Brand brand = new Brand();
            ViewBag.BrandTitle = "AddBrand";
            return View(brand);
        }

        [AdminAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBrand(Brand brand)
        {
            brand.CreatedDate = DateTime.Now;
            brand.CreatedBy = SessionValues.UserId;
            List<Brand> brands = _itemService.GetBrands("All");
            if (brands.Where(x => x.BrandName == brand.BrandName).Count() > 0)
            {
                ModelState.AddModelError("BrandName", "Brand Name Already Exists");
            }
            else
            {
                var affected = _itemService.AddBrand(brand);
                if (affected > 0)
                {
                    TempData["Messege"] = "Brand Inserted";
                    return RedirectToAction("Index");
                }
            }
            ViewBag.BrandTitle = "AddBrand";
            return View(brand);
        }

        [AdminAuthorization]
        public ActionResult EditBrand(int Id)
        {
            var Brand = _itemService.GetBrandById(Id);
            if (Brand == null)
            {
                TempData["ErrorMsg"] = "Brand @Id=" + Id + " is not accessible please select valid record.";
                return RedirectToAction("Index", "Brand");
            }
            ViewBag.BrandTitle = "EditBrand";
            return View(Brand);
        }

        [AdminAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBrand(Brand Brand)
        {
            List<Brand> brands = _itemService.GetBrands("All");
            if (brands.Where(x => x.BrandName == Brand.BrandName && x.Id != Brand.Id).Count() > 0)
            {
                ModelState.AddModelError("BrandName", "Brand Name Already Exists");
            }
            else
            {
                var cat = _itemService.GetBrandById(Brand.Id);
                cat.BrandName = Brand.BrandName;
                cat.ModifiedDate = DateTime.Now;
                cat.ModifiedBy = SessionValues.UserId;
                cat.IsDeleted = Brand.IsDeleted;
                try
                {
                    var affected = _itemService.UpdateBrand(cat);
                    if (affected > 0)
                    {
                        TempData["Messege"] = "Brand Updated";
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            ViewBag.BrandTitle = "EditBrand";
            return View(Brand);
        }

        [HttpPost]
        [AdminApiAuthorization]
        public JsonResult DeleteBrand(int id, bool IsDelete)
        {
            bool cond = _itemService.DeleteBrand(id, SessionValues.UserId, IsDelete) > 0 ? true : false;
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