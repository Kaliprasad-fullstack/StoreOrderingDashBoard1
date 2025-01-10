using BAL;
using DAL;
using DbLayer.Service;
using OfficeOpenXml;
using StoreOrderingDashBoard.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using DAL;
using DbLayer;
using System.Data.Entity.Validation;
using ClosedXML.Excel;
using System.Data;

namespace StoreOrderingDashBoard.Controllers
{
    public class TicketController : Controller
    {
        private readonly IStoreService _storeService;
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        private readonly ILocationService _locationService;
        private readonly IWareHouseService _wareHouseService;
        private readonly IMenuService _menuService;
        private readonly IAuditService _auditService;
        private readonly IUserService _userService;
        private readonly IItemService _itemService;

        public TicketController(ICustomerService customerService, IStoreService storeService)
        {
            _customerService = customerService;
            _storeService = storeService;
            //_orderService = orderService;
            //_locationService = locationService;
            //_wareHouseService = wareHouseService;
            //_menuService = menuService;
            //_auditService = auditService;
            //_userService = userService;
            //_itemService = itemService;

        }

        StoreContext db = new StoreContext();

        // GET: Ticket
        public ActionResult Index()
        {
            List<TicketDetails> list = null;
            List<TicketType> TicketTypes = db.TicketTypes.ToList();
            ViewBag.TicketTypes = TicketTypes;
            var enumValues = Enum.GetValues(typeof(TicketStatus)).Cast<TicketStatus>();
            var selectListItems = enumValues.Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = v.ToString()
            }).ToList();
            ViewBag.Status = selectListItems;
            if (SessionValues.RoleId == 1)
                list = _customerService.GetTickets(SessionValues.RoleId, SessionValues.UserId,
                    "", enumValues.Where(x => x.ToString() == "Open").Select(x => x.ToString()).ToList(), TicketTypes.Select(x => x.TicketTypeId).ToList(), null, null, SessionValues.StoreId, SessionValues.LoggedInCustId, null);
            else
            {
                list = _customerService.GetTickets(SessionValues.RoleId, SessionValues.UserId,
                    "", enumValues.Where(x => x.ToString() == "Open").Select(x => x.ToString()).ToList(), TicketTypes.Select(x => x.TicketTypeId).ToList(), null, null, SessionValues.StoreId, SessionValues.LoggedInCustId, null);

                int CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
                var Stores = _storeService.GetStoresByCustomerId(CustId);
                ViewBag.Stores = Stores.Select(x => new Store() { Id = x.Id, StoreCode = x.StoreCode + " | " + x.StoreName }).ToList();


            }
            //try
            //{
            //    int userid = SessionValues.UserId;
            //    int custid = 0;
            //    if (SessionValues.LoggedInCustId != null)
            //    {
            //        custid = SessionValues.LoggedInCustId.Value;
            //    }
            //    int storeid = SessionValues.StoreId;
            //    custid = _storeService.GetStoreById(storeid).CustId;

            //    int StoreId = SessionValues.StoreId;
            //    return View(list);
            //}
            //catch (Exception ex)
            //{
            //    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
            //    return RedirectToAction("Error404", "Home");
            //}
            return View(list);
        }
        [HttpPost]
        public ActionResult Index(DateTime? FromDate, DateTime? ToDate, List<int> TicketTypeIds, List<string> TicketStatus, string OrderNumber, List<int> SelectedstoreIds)
        {

            List<TicketDetails> list = null;
            List<TicketType> TicketTypes = db.TicketTypes.ToList();
            ViewBag.TicketTypes = TicketTypes;
            var enumValues = Enum.GetValues(typeof(TicketStatus)).Cast<TicketStatus>();
            var selectListItems = enumValues.Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = v.ToString()
            }).ToList();
            ViewBag.Status = selectListItems;
            if (FromDate.HasValue && ToDate.HasValue && FromDate > ToDate)
            {
                ModelState.AddModelError("", "From Date is less than To Date");
                list = new List<TicketDetails>();
                return View(list);
            }

            if (SessionValues.RoleId == 1)
            {
                list = _customerService.GetTickets(SessionValues.RoleId, SessionValues.UserId,
                    OrderNumber, TicketStatus, TicketTypeIds, FromDate, ToDate, SessionValues.StoreId, SessionValues.LoggedInCustId, SelectedstoreIds);
                // db.TicketGenration.Where(x =>
                // x.StoreId == SessionValues.StoreId
                // && (x.CreatedDate.HasValue
                //&& x.CreatedDate.Value >= FromDate.Date
                // && x.CreatedDate.Value <= ntodate)
                // && TicketTypeIds.Contains(x.TicketTypeId)
                // && (TicketStatus.Contains(x.TicketStatus))
                // && x.OrderNo.Contains(OrderNumber)
                // ).ToList();

            }
            else
            {
                int CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
                var Stores = _storeService.GetStoresByCustomerId(CustId);
                //ViewBag.Stores = Stores;
                ViewBag.Stores = Stores.Select(x => new Store() { Id = x.Id, StoreCode = x.StoreCode + " | " + x.StoreName }).ToList();

                list =
           _customerService.GetTickets(SessionValues.RoleId, SessionValues.UserId,
                    OrderNumber, TicketStatus, TicketTypeIds, FromDate, ToDate, SessionValues.StoreId, SessionValues.LoggedInCustId, SelectedstoreIds);

            }
            //try
            //{
            //    int userid = SessionValues.UserId;
            //    int custid = 0;
            //    if (SessionValues.LoggedInCustId != null)
            //    {
            //        custid = SessionValues.LoggedInCustId.Value;
            //    }
            //    int storeid = SessionValues.StoreId;
            //    custid = _storeService.GetStoreById(storeid).CustId;

            //    int StoreId = SessionValues.StoreId;
            //    return View(list);
            //}
            //catch (Exception ex)
            //{
            //    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
            //    return RedirectToAction("Error404", "Home");
            //}
            return View(list);
        }

        public ActionResult AddTicket()
        {
            List<TicketType> list = db.TicketTypes.ToList();
            ViewBag.TicketTypeName = list;


            //int storeId;

            //// Assuming you fetch the StoreId from the Store table using your data access layer
            //using (var context = new StoreContext())
            //{
            //    // Fetch the StoreId from the Store table based on your logic
            //    storeId = context.Stores.FirstOrDefault()?.Id ?? 0; // Example: FirstOrDefault returns null if no Store is found
            //}

            //// Create a new Ticket object and assign the fetched StoreId
            //TicketGenration ticket = new TicketGenration
            //{
            //    StoreId = storeId
            //};

            //// Save the Ticket object to the database using your data access layer (e.g., Entity Framework)
            //using (var context = new StoreContext())
            //{
            //    context.TicketGenration.Add(ticket);
            //    context.SaveChanges();
            //}
            //var warehouse = _wareHouseService.wareHouseDCs();
            return View();
        }

        [HttpPost]

        public ActionResult AddTicket(TicketGenration t)
        {
            int custid = 0;
            if (SessionValues.LoggedInCustId != null)
            {
                custid = SessionValues.LoggedInCustId.Value;
            }
            int storeid = SessionValues.StoreId;
            int StoreId = storeid;
            custid = _storeService.GetStoreById(storeid).CustId;
            t.CreatedBy = SessionValues.UserId;
            t.CreatedDate = DateTime.Now;
            t.TicketStatus = "Open";
            //Request.Files;
            if (ModelState.IsValid)
            {
                TicketGenration ticket = new TicketGenration
                {
                    TicketId = t.TicketId,
                    TicketTypeId = t.TicketTypeId,
                    OrderNo = t.OrderNo,
                    ContactNo = t.ContactNo,
                    Description = t.Description,
                    CreatedBy = t.CreatedBy,
                    CreatedDate = t.CreatedDate,
                    CustId = custid,
                    IsDeleted = t.IsDeleted,
                    TicketRaisedBy = t.TicketRaisedBy,
                    AdminRemark = t.AdminRemark,
                    TicketStatus = t.TicketStatus,
                    StoreId = StoreId// Assign the StoreId fetched above
                };

                TicketGenration ticket_ = db.TicketGenration.Add(ticket);
                db.SaveChanges();
                if (t.ImageFile != null && ticket.TicketId != 0)
                {
                    foreach (HttpPostedFileBase file in t.ImageFile)
                    {
                        if (file != null)
                        {
                            string FileName = Path.GetFileNameWithoutExtension(file.FileName);

                            //To Get File Extension  
                            string FileExtension = Path.GetExtension(file.FileName);
                            string OriginalFileName = file.FileName;
                            //Add Current Date To Attached File Name  
                            FileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + SessionValues.UserId + "-" + ticket_.TicketId + FileExtension;
                            //string UploadPath = ConfigurationManager.AppSettings["UserImagePath"].ToString();
                            string _path = Path.Combine(Server.MapPath("~/Tickets"), "" + FileName);
                            if (!Directory.Exists(Server.MapPath("~/Tickets")))
                            {
                                Directory.CreateDirectory(Server.MapPath("~/Tickets"));
                            }
                            string ImagePath = _path;
                            file.SaveAs(ImagePath);
                            TicketImages ti = new TicketImages();
                            ti.TicketId = ticket_.TicketId;
                            ti.CreatedBy = SessionValues.UserId;
                            ti.ImagePath = FileName;
                            ti.OriginalFileName = OriginalFileName;
                            ti.CreatedDate = DateTime.Now;
                            string msg = _customerService.SaveTicketFile(ti);
                            if (msg != "Done!")
                            {
                                TempData["ERROR"] = "Ticket Images not uploaded.";
                            }
                        }
                    }
                }



                TempData["SUCCESS"] = "Ticket Saved Successfully";
                return RedirectToAction("Index", "Ticket");
            }
            else
            {
                List<TicketType> list = db.TicketTypes.ToList();
                ViewBag.TicketTypeName = list;
                return View(t);
            }

        }
        [HttpGet]
        public ActionResult EditTicket(int TicketId)
        {

            List<TicketType> list = db.TicketTypes.ToList();
            ViewBag.TicketTypeName = list;
            var enumValues = Enum.GetValues(typeof(TicketStatus)).Cast<TicketStatus>();
            var selectListItems = enumValues.Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = v.ToString()
            }).ToList();
            ViewBag.Status = selectListItems;
            TicketGenration t = db.TicketGenration.Find(TicketId);
            if (t == null)
            {
                TempData["ERROR"] = "Ticket Not Found";
                return RedirectToAction("index");
            }

            if (SessionValues.RoleId == 1 && (t == null || t.StoreId != SessionValues.StoreId))
            {
                //ModelState.AddModelError("Description", "Ticket Not Found");
                TempData["ERROR"] = "Ticket Not Found";
                return RedirectToAction("index");
            }
            if (t.TicketStatus == "Open")
            {
                t.TicketImages = _customerService.GetTicketImages(t.TicketId);
                return View(t);
            }
            else
                // ModelState.AddModelError("", "Ticket Cannot Edit");
                TempData["ERROR"] = "Ticket cannot Edit";
            return RedirectToAction("index");
        }

        public ActionResult EditTicket(TicketGenration t)
        {
            if (SessionValues.RoleId == 2)
            {
                ModelState.Remove("TicketTypeId");
                ModelState.Remove("OrderNo");
                ModelState.Remove("ContactNo");
                ModelState.Remove("Description");
                ModelState.Remove("TicketRaisedBy");
                if (String.IsNullOrEmpty(t.AdminRemark) || t.AdminRemark.Trim() == "")
                {
                    ModelState.AddModelError("AdminRemark", "Remark is Required");
                }
                if (!ModelState.IsValid)
                {
                    List<TicketType> list = db.TicketTypes.ToList();
                    ViewBag.TicketTypeName = list;
                    var enumValues = Enum.GetValues(typeof(TicketStatus)).Cast<TicketStatus>();
                    var selectListItems = enumValues.Select(v => new SelectListItem
                    {
                        Text = v.ToString(),
                        Value = v.ToString()
                    }).ToList();
                    ViewBag.Status = selectListItems;
                    return View(t);
                }
                else
                {
                    TicketGenration oldticket = db.TicketGenration.Where(x => x.TicketId == t.TicketId && x.TicketStatus == "Open").FirstOrDefault();
                    oldticket.AdminRemark = t.AdminRemark;
                    oldticket.TicketStatus = t.TicketStatus;
                    oldticket.ClosedDate = null;
                    if (t.TicketStatus == "Resolved")
                        oldticket.ClosedDate = DateTime.Now;
                    db.Entry<TicketGenration>(oldticket).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["SUCCESS"] = "Ticket Updated Successfully";
                }
            }
            else
            {


                if (!ModelState.IsValid)
                {
                    List<TicketType> list = db.TicketTypes.ToList();
                    ViewBag.TicketTypeName = list;
                    return View(t);
                }
                else
                {
                    TicketGenration oldticket = db.TicketGenration.Where(x => x.TicketId == t.TicketId && x.TicketStatus == "Open").FirstOrDefault();
                    oldticket.TicketTypeId = t.TicketTypeId;
                    oldticket.OrderNo = t.OrderNo;
                    oldticket.ContactNo = t.ContactNo;
                    oldticket.Description = t.Description;
                    oldticket.TicketRaisedBy = t.TicketRaisedBy;
                    db.Entry<TicketGenration>(oldticket).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    if (t.ImageFile != null && t.TicketId != 0)
                    {
                        foreach (HttpPostedFileBase file in t.ImageFile)
                        {
                            if (file != null)
                            {
                                string FileName = Path.GetFileNameWithoutExtension(file.FileName);

                                //To Get File Extension  
                                string FileExtension = Path.GetExtension(file.FileName);
                                string OriginalFileName = file.FileName;
                                //Add Current Date To Attached File Name  
                                FileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + SessionValues.UserId + "-" + t.TicketId + FileExtension;
                                //string UploadPath = ConfigurationManager.AppSettings["UserImagePath"].ToString();
                                string _path = Path.Combine(Server.MapPath("~/Tickets"), "" + FileName);
                                if (!Directory.Exists(Server.MapPath("~/Tickets")))
                                {
                                    Directory.CreateDirectory(Server.MapPath("~/Tickets"));
                                }
                                string ImagePath = _path;
                                file.SaveAs(ImagePath);
                                TicketImages ti = new TicketImages();
                                ti.TicketId = t.TicketId;
                                ti.CreatedBy = SessionValues.UserId;
                                ti.ImagePath = FileName;
                                ti.OriginalFileName = OriginalFileName;
                                ti.CreatedDate = DateTime.Now;
                                string msg = _customerService.SaveTicketFile(ti);
                                if (msg != "Done!")
                                {
                                    TempData["ERROR"] = "Ticket Images not uploaded.";
                                }
                            }
                        }
                    }
                    TempData["SUCCESS"] = "Ticket Updated Successfully";
                }
            }
            return RedirectToAction("Index", "Ticket");
        }

        public ActionResult DeleteTicket(int TicketId)
        {
            TicketGenration t = db.TicketGenration.Find(TicketId);
            db.TicketGenration.Remove(t);
            db.SaveChanges();
            return RedirectToAction("Index", "Ticket");
        }
        //[System.Web.Mvc.Authorize]
        //[CustomAuthorization]
        public ActionResult DownloadTicketImages(string ImageFile, int TicketId)
        {
            var dir = Server.MapPath("/Tickets");
            List<TicketImages> list = _customerService.GetTicketImages(TicketId);
            if (list == null || list.Count() == 0 || !list.Exists(x => x.ImagePath == ImageFile))
            {
                HttpContext.Response.StatusCode = 404;
                return Content("404");
            }
            //Path.Combine(Server.MapPath("~/Tickets"), "" + FileName);
            var path = Path.Combine(dir, ImageFile); //validate the path for security or use other means to generate the path.
            if (System.IO.File.Exists(path))
            {
                return base.File(path, "image/jpeg");
            }
            else
            {
                HttpContext.Response.StatusCode = 404;
                return Content("404");
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteImageFile(int TicketId, int ImageId)
        {
            string msg = _customerService.DeleteTicketImage(ImageId, TicketId, SessionValues.UserId);
            if (msg != "Done!")
            {
                msg = "Image not Deleted";
            }
            else
            {
                TempData["SUCCESS"] = "Ticket Image Deleted Successfully";
            }
            JsonResult result = Json(new { Created = true }, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
            return result;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Export(DateTime? FromDate, DateTime? ToDate, List<int> TicketTypeIds, List<string> TicketStatus, string OrderNumber, List<int> SelectedstoreIds, string Flag)
        {
           // List<TicketDetails> list = null;
           // List<TicketType> TicketTypes = db.TicketTypes.ToList();
           // JsonResult result;
           // if (FromDate.HasValue && ToDate.HasValue && FromDate > ToDate)
           // {
           //     ModelState.AddModelError("", "From Date is less thane To Date");
           //     result = Json(new { Created = false, ErrorMessage = "From Date is less thane To Date" }, JsonRequestBehavior.AllowGet);
           //     return result;
           // }

           // if (SessionValues.RoleId == 1)
           // {
           //     list = _customerService.GetTickets(SessionValues.RoleId, SessionValues.UserId,
           //         OrderNumber, TicketStatus, TicketTypeIds, FromDate, ToDate, SessionValues.StoreId, SessionValues.LoggedInCustId, SelectedstoreIds);
           // }
           // else
           // {
           //     int CustId = Convert.ToInt32(SessionValues.LoggedInCustId.ToString());
           //     var Stores = _storeService.GetStoresByCustomerId(CustId);
           //     //ViewBag.Stores = Stores;
           //     ViewBag.Stores = Stores.Select(x => new Store() { Id = x.Id, StoreCode = x.StoreCode + " | " + x.StoreName }).ToList();

           //     list =
           //_customerService.GetTickets(SessionValues.RoleId, SessionValues.UserId,
           //         OrderNumber, TicketStatus, TicketTypeIds, FromDate, ToDate, SessionValues.StoreId, SessionValues.LoggedInCustId, SelectedstoreIds);
           // }
           // string msg = "";
            //if (Flag == "R")
            //{
                //return file
                var wb = new XLWorkbook();
                DataTable dt = _customerService.GetReportDatable(SessionValues.RoleId, SessionValues.UserId,
                    OrderNumber, TicketStatus, TicketTypeIds, FromDate, ToDate, SessionValues.StoreId, SessionValues.LoggedInCustId, SelectedstoreIds);
                wb.Worksheets.Add(dt, "TicketDetails");             
                var stream = new MemoryStream();
                wb.SaveAs(stream);
                var content = stream.ToArray();              
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(content, contentType, "TicketDetails.xlsx");
            
            //if (list == null || list.Count <= 0)
            //{
            //    msg = "No Records Found";
            //}

            //result = Json(new { Created = msg == "", ErrorMessage = msg }, JsonRequestBehavior.AllowGet);
            //return result;
        }
        //public ActionResult AcceptTicket(int TicketId)
        //{
        //    var ticket = db.TicketGenration.Find(TicketId);
        //    if (ticket != null)
        //    {
        //        ticket.TicketStatus = "Open";
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("TicketList");
        //}

        //public ActionResult RejectTicket(int TicketId)
        //{
        //    var ticket = db.TicketGenration.Find(TicketId);
        //    if (ticket != null)
        //    {
        //        ticket.TicketStatus = "Close";
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("TicketList");
        //}
    }
}