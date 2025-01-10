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

namespace StoreOrderingDashBoard.Controllers
{
    public class StoreController : Controller
    {
        // GET: Store
        private readonly IStoreService _storeService;
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        private readonly ILocationService _locationService;
        private readonly IWareHouseService _wareHouseService;
        private readonly IMenuService _menuService;
        private readonly IAuditService _auditService;
        private readonly IUserService _userService;
        private readonly IItemService _itemService;
        // private readonly JavaScriptSerializer _javaScriptSerializer;

        public StoreController(IStoreService storeService, ICustomerService customerService, IOrderService orderService, ILocationService locationService, IWareHouseService wareHouseService, IMenuService menuService, IAuditService auditService, IUserService userService, IItemService itemService)
        {
            _customerService = customerService;
            _storeService = storeService;
            _orderService = orderService;
            _locationService = locationService;
            _wareHouseService = wareHouseService;
            _menuService = menuService;
            _auditService = auditService;
            _userService = userService;
            _itemService = itemService;
            
        }

        [AdminAuthorization]
        public ActionResult Index(int Id = 0)
        {
            try
            {
                var stores = _storeService.GetStoresByCustomerId(Id);
                ViewBag.stores = stores;
                return View();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return RedirectToAction("Error404", "Home");
            }
        }

        [AdminAuthorization]
        public ActionResult AddStore()
        {
            try
            {
                var CustomerList = _customerService.GetAllCustomers();
                var LocationList = _locationService.GetAllLocations();
                var warehouse = _wareHouseService.wareHouseDCs();
                var netsuit = _wareHouseService.NetSuitMapCls();
                ViewBag.CustomerList = CustomerList;
                ViewBag.LocationList = LocationList;
                ViewBag.warehouse = warehouse;
                ViewBag.netsuit = netsuit;
                DAL.Store store = new Store();
                return View(store);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return RedirectToAction("Error404", "Home");
            }
        }

        [HttpPost]
        [AdminAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult AddStore(Store store)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    store.CreatedBy = 1;
                    store.CreatedDate = DateTime.Now;
                    store.ModifiedDate = DateTime.Now;
                    var existstore = _storeService.GetStoreByEmailAddress(store);
                    if (existstore != null)
                    {
                        ModelState.AddModelError("StoreEmailId", "Email Address Already Exist");
                        ModelState.AddModelError("StoreCode", "StoreCode Already Exist");
                        ModelState.AddModelError("StoreName", "Store Name Already Exist");
                        var CustomerList = _customerService.GetAllCustomers();
                        var LocationList = _locationService.GetAllLocations();
                        var warehouse = _wareHouseService.wareHouseDCs();
                        var netsuit = _wareHouseService.NetSuitMapCls();
                        ViewBag.CustomerList = CustomerList;
                        ViewBag.LocationList = LocationList;
                        ViewBag.warehouse = warehouse;
                        ViewBag.netsuit = netsuit;
                        return View(store);
                    }
                    store.Password = HelperCls.EncodeServerName("Rfpl@123");
                    var javascript = new JavaScriptSerializer();
                    try
                    {
                        var Auditstore = new Store(store.Id, store.StoreCode, store.StoreName, store.Address, store.StoreEmailId, store.Region, store.Route, store.StoreManager, store.StoreContactNo, store.DeliveryInTime, store.DeliveryOutTime, store.CreatedBy, store.ModifiedBy, store.Password, store.PlaceOfSupply, store.CustId, store.IsDeleted, store.CreatedDate, store.ModifiedDate, store.LocationId, store.NetSuitMapClsId);
                        var str = javascript.Serialize(store);
                        _auditService.InsertAudit(HelperCls.GetIPAddress(), str, Request.Url.OriginalString.ToString(), Convert.ToInt32(SessionValues.UserId), null);
                    }
                    catch (Exception ex)
                    {
                        HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                        throw ex;
                    }
                    int warehouseid = store.WareHouseDC.Id;
                    int NetSuitMapCls = store.NetSuitMapCls.Id;
                    store.WareHouseDC = _storeService.WareHouseDC(store.WareHouseDC.Id);
                    store.NetSuitMapCls = _storeService.NetSuitMapCl(store.NetSuitMapCls.Id);
                    int rowsaffected = _storeService.AddStore2(store, warehouseid, NetSuitMapCls);
                    if (rowsaffected > 0)
                    {
                        return RedirectToAction("Index", "Store", new { Id = store.CustId });
                    }
                    else {

                        ModelState.AddModelError("StoreEmailId", "Try again");
                        ModelState.AddModelError("StoreCode", "Try again");
                        var CustomerList = _customerService.GetAllCustomers();
                        var LocationList = _locationService.GetAllLocations();
                        var warehouse = _wareHouseService.wareHouseDCs();
                        var netsuit = _wareHouseService.NetSuitMapCls();
                        ViewBag.CustomerList = CustomerList;
                        ViewBag.LocationList = LocationList;
                        ViewBag.warehouse = warehouse;
                        ViewBag.netsuit = netsuit;
                        return View(store);

                    }
                }
                return View(store);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return RedirectToAction("Error500", "Home");
            }
        }

        [AdminAuthorization]
        public ActionResult EditStore(int Id)
        {
            try
            {
                var store = _storeService.GetStoreById(Id);
                var CustomerList = _customerService.GetAllCustomers();
                var LocationList = _locationService.GetAllLocations();
                if (store != null && CustomerList != null && LocationList != null)
                {
                    if (store.Locations != null)
                    {
                        store.LocationId = store.Locations.LocationId;
                    }
                    var warehouse = _wareHouseService.wareHouseDCs();
                    var netsuit = _wareHouseService.NetSuitMapCls();
                    ViewBag.CustomerList = CustomerList;
                    ViewBag.LocationList = LocationList;
                    ViewBag.warehouse = warehouse;
                    ViewBag.netsuit = netsuit;
                    return View(store);
                }
                return RedirectToAction("Error404", "Store");
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return RedirectToAction("Error404", "Home");
            }
        }

        [AdminAuthorization]
        public ActionResult MenuMangementForStore(int custplanid, int custid, int storeid, int roleid)
        {
            var parentmenu = _menuService.GetParentMenus(roleid);

            List<Menu> listofmenu = new List<Menu>();
            foreach (MenuItem par in parentmenu)
            {
                Menu m = new Menu();
                m.Id = par.Id;
                m.MenuName = par.MenuName;
                var plan = _menuService.GetMenuForStore(m.Id, custplanid, storeid);
                m.PlanId = plan != null ? plan.Id : 0;
                listofmenu.Add(m);
                var child = _menuService.GetChildMenu(par.Id,roleid);
                foreach (MenuItem chi in child)
                {
                    Menu m1 = new Menu();
                    m1.Id = chi.Id;
                    m1.MenuName = par.MenuName + "=>" + chi.MenuName;
                    var plan1 = _menuService.GetMenuForStore(m1.Id, custplanid, storeid);
                    m1.PlanId = plan1 != null ? plan1.Id : 0;
                    listofmenu.Add(m1);
                    var child1 = _menuService.GetChildMenu(chi.Id,roleid);
                    foreach (MenuItem chi1 in child1)
                    {
                        Menu m2 = new Menu();
                        m2.Id = chi1.Id;
                        m2.MenuName = chi.MenuName + "=>" + chi1.MenuName;
                        var plan2 = _menuService.GetMenuForStore(m1.Id, custplanid, storeid);
                        m2.PlanId = plan2 != null ? plan2.Id : 0;
                        listofmenu.Add(m2);
                    }
                }

            }
            ViewBag.Menu = listofmenu;

            var dashboard = _menuService.DashBoardPer(roleid);
            List<Menu> DashBoardlist = new List<Menu>();
            foreach (DashBoardPer per in dashboard)
            {
                Menu m = new Menu();
                m.Id = per.Id;
                m.MenuName = per.Name;
                var plan = _menuService.DashBoardMapping(custplanid, per.Id);
                m.PlanId = plan != null ? plan.Id : 0;
                DashBoardlist.Add(m);
            }
            ViewBag.Dash = DashBoardlist;
            return View();
        }

        [HttpPost]
        [AdminApiAuthorization]
        public JsonResult SavemenuManagementForStore(List<Menu> menus, int storeid, int custplanid)
        {

            int affectedrow = _menuService.DeleteStorePlan(storeid);
            foreach (Menu menu in menus)
            {
                Plan plan = new Plan();
                plan.MenuItem = _menuService.MenuItem(menu.Id);
                plan.Store = _menuService.GetStore(storeid);
                int affected = 0;
                if (menu.PlanId != null)
                {

                    var menus1 = _menuService.GetMenuForStore(menu.Id, custplanid, storeid);
                    if (menus1 == null)
                    {
                        plan.IsShow = true;
                        affected = _menuService.Insertmenu(plan);
                    }

                }
                else
                {
                    var menus1 = _menuService.GetMenuForStore(menu.Id, custplanid, storeid);
                    if (menus1 != null)
                    {
                        plan.IsShow = false;
                        affected = _menuService.Insertmenu(plan);
                    }

                }
            }

            return Json(true, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        [AdminAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult EditStore(Store store)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var storeed = _storeService.GetStoreById(store.Id);
                    storeed.Address = store.Address;
                    storeed.CreatedBy = 1;
                    storeed.CustId = store.CustId;
                    storeed.ModifiedBy = 1;
                    storeed.ModifiedDate = DateTime.Now;
                    storeed.Region = store.Region;
                    storeed.Route = store.Route;
                    storeed.StoreCode = store.StoreCode;
                    storeed.StoreContactNo = store.StoreContactNo;
                    storeed.StoreEmailId = store.StoreEmailId;
                    storeed.StoreManager = store.StoreManager;
                    storeed.StoreName = store.StoreName;
                    storeed.LocationId = store.LocationId;
                    storeed.PlaceOfSupply = store.PlaceOfSupply;
                    storeed.WareHouseDC = _storeService.WareHouseDC(store.WareHouseDC.Id);
                    storeed.NetSuitMapCls = _storeService.NetSuitMapCl(store.NetSuitMapCls.Id);
                   
                    int rowsaffected = _storeService.EditStore(storeed);
                    try
                    {
                        var Auditstore = new Store(storeed.Id, storeed.StoreCode, storeed.StoreName, storeed.Address, storeed.StoreEmailId, storeed.Region, storeed.Route, storeed.StoreManager, storeed.StoreContactNo, storeed.DeliveryInTime, storeed.DeliveryOutTime, storeed.CreatedBy, storeed.ModifiedBy, storeed.Password, storeed.PlaceOfSupply, storeed.CustId, storeed.IsDeleted, storeed.CreatedDate, storeed.ModifiedDate, storeed.LocationId, storeed.NetSuitMapClsId);
                        var auditJavascript = new JavaScriptSerializer();
                        var str = auditJavascript.Serialize(Auditstore);
                        _auditService.InsertAudit(HelperCls.GetIPAddress(), str, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                    }
                    catch (Exception ex)
                    {
                        HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                        throw ex;
                    }
                    return RedirectToAction("Index", "Store", new { Id = store.CustId });
                }
                else
                    return View();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return RedirectToAction("Error500", "Home");
            }
        }

        [HttpPost]
        [AdminAuthorization]
        public ActionResult DeleteStore(int Store)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var store = _storeService.GetStoreById(Store);
                    store.IsDeleted = true;
                    int rowsaffected = _storeService.DeleteStore(store);
                    try
                    {
                        var Auditstore = new Store(store.Id, store.StoreCode, store.StoreName, store.Address, store.StoreEmailId, store.Region, store.Route, store.StoreManager, store.StoreContactNo, store.DeliveryInTime, store.DeliveryOutTime, store.CreatedBy, store.ModifiedBy, null, store.PlaceOfSupply, store.CustId, store.IsDeleted, store.CreatedDate, store.ModifiedDate, store.LocationId, store.NetSuitMapClsId);
                        var auditJavascript = new JavaScriptSerializer();
                        var str = auditJavascript.Serialize(Auditstore);
                        _auditService.InsertAudit(HelperCls.GetIPAddress(), str, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                    }
                    catch (Exception ex)
                    {
                        HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                        throw ex;
                    }
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return RedirectToAction("Error500", "Home");
            }
        }

        public ActionResult Login(string IsSessionExpired,string Error,string Message)
        {
            SessionValues.StoreCount = SessionValues.StoreCount + 1;
            ViewData["Attempt"] = _menuService.GetMaster("Login Attempt For Store").Value;
            ViewBag.Password = "false";
            ViewBag.IsSessionExpired = IsSessionExpired;
            ViewBag.Error = Error;
            ViewBag.Message = Message;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login login)
        {
            try
            {
                ViewBag.Password = "true";
                var store = _storeService.StoreLogin(login.Username, login.Password, HelperCls.GetIPAddress());
                var date = DateTime.Now.Date;
                if (store != null)
                {
                    if (store.Error == null)
                    {
                        var difference = (date - store.ModifiedDate.Date).TotalDays;
                        if (store.Password != "Rfpl@123")
                        {
                            string SessionTime = _menuService.GetMaster("Store Login Session").Value;
                            store.StoreUserName = login.Username;
                            store.SessionTimeout = SessionTime;
                            var userData = new JavaScriptSerializer().Serialize(store);
                            Session["AuthUserData"] = (store.RoleId!=0?store.RoleId:1 )+ "|" + store.Id.ToString() + "|" + store.UserId + "|" + store.CustomerId + "|" + store.StoreUserName.ToString() + "|" + store.PlanId + "|" + store.MkrChkrFlag + "|" + SessionTime + "|" + (store.EmailAddress != null ? store.EmailAddress : "") + "|" + difference + "|" + store.StoreName;
                            var ticket = new FormsAuthenticationTicket(1, (store.Id.ToString() + store.StoreName), DateTime.Now, DateTime.Now.AddMinutes(Convert.ToDouble(SessionTime)), true, userData);
                            string encTicket = FormsAuthentication.Encrypt(ticket);
                            SessionValues.StoreCount = 0;
                            FormsAuthentication.SetAuthCookie((store.Id.ToString() + store.StoreName), true);
                            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                            if (BAL.GlobalValues.PasswordPolicyDays > difference)
                            {
                                ViewBag.Password = "false";
                                //audit 
                                var javascript = new JavaScriptSerializer();
                                _auditService.InsertAudit(HelperCls.GetIPAddress(), javascript.Serialize(login), Url.Action("Login", "Store"), store.UserId, store.Id);
                                return RedirectToAction("Home", "Store");
                            }
                            else
                            {
                                return RedirectToAction("ChangePassword", "Store", new { id = HelperCls.EncodeServerName(store.UserId.ToString()) });
                            }
                        }
                        else
                        {
                            return RedirectToAction("ChangePassword", "Store", new { id = HelperCls.EncodeServerName(store.UserId.ToString()) });
                        }
                    }
                    else
                    {
                        ViewBag.Password = "false";
                        ViewBag.Error = store.Error;
                        return View("Login");
                    }
                }
                SessionValues.StoreCount = SessionValues.StoreCount + 1;
                ViewData["Attempt"] = _menuService.GetMaster("Login Attempt For Store").Value;
                if (SessionValues.StoreCount == Convert.ToInt32(ViewData["Attempt"].ToString()))
                    _userService.DisableAdminUser(login.Username, 1);
                return View("Login");
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return View("Login");
            }
        }
        public ActionResult ChangeCustomerStore()
        {
            return Content("");
        }

        [CustomAuthorization]
        public ActionResult Home()
        {
            try
            {
                int userid = SessionValues.UserId; 
                int custid = 0;
                if (SessionValues.LoggedInCustId != null)
                {
                    custid = SessionValues.LoggedInCustId.Value;
                }
                int storeid = SessionValues.StoreId;
                custid = _storeService.GetStoreById(storeid).CustId;
               
                    int StoreId = SessionValues.StoreId;
                //var MakeCopy = _orderService.MakeCopyWithLFR(StoreId);
                //var finilisedorder = _orderService.MakeaCopy(StoreId).ToList();
                //var finilisedorder1 = _orderService.GetFinilizeOrder(StoreId);
                //var Processedorder = _orderService.GetProcessedOrder(StoreId).ToList();
                //var saveasdraft = _orderService.GetSaveAsDraftOrders(StoreId).ToList();
                //var soapprove = _orderService.SoApprove(StoreId);
                //var fullfillment = _orderService.FullfillmentCount(StoreId).ToList();
                //var invoice = _orderService.InvoiceCount(StoreId).ToList();
                //var Items = _itemService.GetSkuForUserCustomer(userid, custid, "Report", StoreId);
                //ViewBag.Items = new SelectList(Items.ToList(), "Id", "SKUCode", null);
                //ViewData["FinilizeOrder"] = finilisedorder1.Count;
                //ViewData["SaveAsDraftOrder"] = saveasdraft.Count(); 
                //ViewData["ProcessedOrder"] = Processedorder.Count();
                //ViewData["SoApproved"] = soapprove.Count();
                //ViewData["FullFillment"] = fullfillment.Count();
                //ViewData["Invoice"] = invoice.Count();
                //ViewBag.SaveasDraftOrderList = saveasdraft.Take(5);
                //ViewBag.FiniliseOrderList = MakeCopy;
                //DateTime now = DateTime.Now;
                //var startDate = new DateTime(now.Year, now.Month, 1);
                //var endDate = startDate.AddMonths(1).AddDays(-1);
                //var monthfinilize = from a in finilisedorder1
                //                    where a.OrderDate >= startDate && a.OrderDate <= endDate
                //                    select a;
                //ViewData["MonthFinilize"] = monthfinilize.Count();
                //var monthProcess = from b in Processedorder
                //                   where b.OrderDate >= startDate && b.OrderDate <= endDate
                //                   select b;
                //ViewData["MonthProcess"] = monthProcess.Count();
                //var monthsaveasdraft = from c in saveasdraft
                //                       where c.OrderDate >= startDate && c.OrderDate <= endDate
                //                       select c;
                //ViewData["MonthSaveDraft"] = monthsaveasdraft.Count();
                //var monthsoapprove = from d in soapprove
                //                     where d.OrderDate >= startDate && d.OrderDate <= endDate
                //                     select d;
                //ViewData["MonthSoApprove"] = monthsoapprove.Count();
                //var monthfullfillment = from e in fullfillment
                //                        where e.OrderDate >= startDate && e.OrderDate <= endDate
                //                        select e;
                //ViewData["MonthFullfillment"] = monthfullfillment.Count();
                //var monthinvoice = from f in invoice
                //                   where f.OrderDate >= startDate && f.OrderDate <= endDate
                //                   select f;
                //ViewData["MonthInvoice"] = monthinvoice.Count();
                //List<int> finilizelist = new List<int>();
                //List<int> Processedlist = new List<int>();

                //for (int i = 1; i <= 12; i++)
                //{
                //    var startDate1 = new DateTime(now.Year, i, 1);
                //    var endDate1 = startDate1.AddMonths(1).AddDays(-1);
                //    var monthfinilize1 = from a in finilisedorder
                //                         where a.OrderDate >= startDate1 && a.OrderDate <= endDate1
                //                         select a;
                //    finilizelist.Add(monthfinilize1.Count());

                //    var monthProcess1 = from b in Processedorder
                //                        where b.ProcessedDate >= startDate1 && b.ProcessedDate <= endDate1
                //                        select b;
                //    Processedlist.Add(monthProcess1.Count());

                //}
                //JavaScriptSerializer js = new JavaScriptSerializer();
                //string s1 = js.Serialize(finilizelist);
                //ViewData["FinilizeList"] = s1;
                //string s2 = js.Serialize(Processedlist);
                //ViewData["ProcessedList"] = s2;
                return View();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return RedirectToAction("Error500", "Home");
            }
        }

        [CustomAuthorization]
        public ActionResult LogOut()
        {
            if (SessionValues.UserId!= 0 )
            {
                _userService.logout(SessionValues.UserId);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), SessionValues.UserId.ToString(), Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
            }
            FormsAuthentication.SignOut();
            Session.Abandon();

            // clear authentication cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
            SessionStateSection sessionStateSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
            HttpCookie cookie2 = new HttpCookie(sessionStateSection.CookieName, "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);
            //Session.Clear();
            return RedirectToAction("Login", "Store");
        }

        [HttpPost]
        public JsonResult ForgotPassword(string Username)
        {
            var storeuser = _storeService.GetUsers(Username);
            bool cond = false;
            ReturnResponse response = new ReturnResponse();
            response.UserId = null;
            if (storeuser != null)
            {
                string otp = HelperCls.GenerateOTP();
                Users OTPuser = new Users(storeuser.Id, storeuser.EmailAddress, otp);
                cond = HelperCls.SendEmail(storeuser.EmailAddress, "Forgot Password Link", HelperCls.PopulateOTP(storeuser.Name, HelperCls.EncodeServerName(storeuser.Id.ToString()), otp));
                if (cond)
                {
                    var userData = new JavaScriptSerializer().Serialize(OTPuser);
                    var ticket = new FormsAuthenticationTicket(1, (storeuser.EmailAddress), DateTime.Now, DateTime.Now.AddMinutes(30), true, userData);
                    // Encrypt the ticket.
                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    // Create the cookie.
                    Response.Cookies.Add(new HttpCookie("OTPCookie", encTicket));
                    response.UserId = HelperCls.EncodeServerName(storeuser.Id.ToString());
                }
            }
            response.Status = cond;
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LostPassword(string id)
        {
            ViewData["Id"] = HelperCls.DecodeServerName(id);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LostPassword(int Id, string Password)
        {
            var storeuser = _storeService.GetUserById(Id);
            storeuser.Password = Password;
            storeuser.Modified = DateTime.Now;
            _storeService.EditUser(storeuser);
            try
            {
                var auditJavascript = new JavaScriptSerializer();
                var StoreAudit = auditJavascript.Serialize("Id: " + storeuser.Id + ", Password: " + storeuser.Password);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), StoreAudit, Request.Url.OriginalString.ToString(), storeuser.Id, null);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            return RedirectToAction("LogIn", "Store");
        }

        public ActionResult ChangePassword(string id)
        {
            ViewBag.Password = "True";
            ViewData["Id"] = HelperCls.DecodeServerName(id);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(int Id, string Password, string OldPassword)
        {
            var store = _storeService.GetUserById(Id);
            ViewBag.Password = "False";
            if (store.Password == OldPassword)
            {
                ViewBag.Password = "True";
                store.Password = Password;
                store.Modified = DateTime.Now;
                int affectedrow = _storeService.EditUser(store);
                try
                {
                    var auditJavascript = new JavaScriptSerializer();
                    var StoreAudit = auditJavascript.Serialize("Id: " + store.Id + ", Password: " + Password + ", OldPassword: " + OldPassword);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), StoreAudit, Request.Url.OriginalString.ToString(), store.Id, null);
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    throw ex;
                }
                return RedirectToAction("Logout", "Store");
            }
            //return RedirectToAction("ChangePassword", new { id = HelperCls.EncodeServerName(Id.ToString()) });
            ViewBag.Password = "False";
            ViewData["Id"] = Id.ToString();
            return View("ChangePassword");
        }

        public ActionResult ChangePasswordSetting()
        {
            ViewBag.Password = "True";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePasswordSetting(string Password, string OldPassword)
        {
            ViewBag.Password = "True";
            int id = Convert.ToInt32(SessionValues.UserId);
            var storeuser = _storeService.GetUserById(id);
            if (storeuser.Password == OldPassword)
            {
                storeuser.Password = Password;
                storeuser.Modified = DateTime.Now;
                storeuser.ModifiedBy = Convert.ToInt32(SessionValues.UserId);
                var affected = _storeService.EditUser(storeuser);
                if (affected > 0)
                {
                    //Session["Remainingdays"] = 0;
                }
                try
                {
                    var auditJavascript = new JavaScriptSerializer();
                    var StoreAudit = auditJavascript.Serialize("Id: " + storeuser.Id + ", Password: " + Password + ", OldPassword: " + OldPassword);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), StoreAudit, Request.Url.OriginalString.ToString(), Convert.ToInt32(SessionValues.UserId), Convert.ToInt32(SessionValues.StoreId));
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    throw ex;
                }
                return RedirectToAction("Home", "Store");
            }
            else
            {
                ViewBag.Password = "False";
            }

            return View();
        }

        [CustomAuthorization]
        public ActionResult FAQ()
        {
           
            return View();
        }

        [CustomAuthorization]
        public ActionResult UserManual()
        {
           
            return View();
        }

        [CustomAuthorization]
        public ActionResult InfoVideo()
        {
            
            return View();
        }

        [AdminAuthorization]
        public ActionResult AddBulkUsers()
        {
            return View();
        }
        [AdminAuthorization]
        public void ImportFormat()
        {
            StringWriter sw = new StringWriter();
            var filename = "Store User Upload Format" + DateTime.Now.ToString("dd/MM/yyyy") + ".xlsx";
             var data = new[]{
                               new{ StoreCode="",StoreName="",Address="",StoreEmailId="",Region="",Route="",StoreManager="",StoreContactNo="",Customer_Name="",Location="",PlaceOfSupply="",WarehouseDc="",EmailAddress="",PlanName="",MkrChkrFlag="",NetSuitMapCls=""},
                      };
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells[1, 1].LoadFromCollection(data, true);
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + filename);

                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }

        }

        [AdminAuthorization]
        [HttpPost]
        public ActionResult AddBulkUsers(HttpPostedFileBase CSVUpload)
        {
            List<StoreUserExcelImport> im = null;
            if (Request.Files["CSVUpload"].ContentLength > 0)
            {
                HttpPostedFileBase file = Request.Files["CSVUpload"];
                var Stores = _storeService.GetAllStores();
                var Customers = _customerService.GetAllCustomers();
                var Locations = _locationService.GetAllLocations();
                var warehouseDcs = _wareHouseService.wareHouseDCs();
                var Plans = _customerService.GetCustomerPlans();
                var ChkrMkrList = new List<string> { "Chkr", "Mkr", "ChkrMkr" };
                List<NetSuitMapCls> NetsuiteMapCls = _customerService.GetNetSuiteCls();
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                 }
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    var imports = new List<StoreUserExcelImport>();
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            var import = new StoreUserExcelImport();
                            import.StoreCode = workSheet.Cells[rowIterator, 1].Value != null ? workSheet.Cells[rowIterator, 1].Value.ToString().Trim() : null;
                            import.StoreName = workSheet.Cells[rowIterator, 2].Value != null ? workSheet.Cells[rowIterator, 2].Value.ToString().Trim() : null;
                            import.Address = workSheet.Cells[rowIterator, 3].Value != null ? workSheet.Cells[rowIterator, 3].Value.ToString().Trim() : null;
                            import.StoreEmailId = workSheet.Cells[rowIterator, 4].Value != null ? workSheet.Cells[rowIterator, 4].Value.ToString().Trim() : null;
                            import.Region = workSheet.Cells[rowIterator, 5].Value != null ? workSheet.Cells[rowIterator, 5].Value.ToString().Trim() : null;
                            import.Route = workSheet.Cells[rowIterator, 6].Value != null ? workSheet.Cells[rowIterator, 6].Value.ToString().Trim() : null;
                            import.StoreManager = workSheet.Cells[rowIterator, 7].Value != null ? workSheet.Cells[rowIterator, 7].Value.ToString().Trim() : null;
                            import.StoreContactNo = workSheet.Cells[rowIterator, 8].Value != null ? workSheet.Cells[rowIterator, 8].Value.ToString().Trim() : null;
                            import.CustomerName = workSheet.Cells[rowIterator, 9].Value != null ? workSheet.Cells[rowIterator, 9].Value.ToString().Trim() : null;
                            import.Location = workSheet.Cells[rowIterator, 10].Value != null ? workSheet.Cells[rowIterator, 10].Value.ToString().Trim() : null;
                            import.PlaceOfSupply = workSheet.Cells[rowIterator, 11].Value != null ? workSheet.Cells[rowIterator, 11].Value.ToString().Trim() : null;
                            import.WarehouseDc = workSheet.Cells[rowIterator, 12].Value != null ? workSheet.Cells[rowIterator, 12].Value.ToString().Trim() : null;
                            import.UserEmailAddress = workSheet.Cells[rowIterator, 13].Value != null ? workSheet.Cells[rowIterator, 13].Value.ToString().Trim() : null;
                            import.PlanName = workSheet.Cells[rowIterator, 14].Value != null ? workSheet.Cells[rowIterator, 14].Value.ToString().Trim() : null;
                            import.MkrChkrFlag = workSheet.Cells[rowIterator, 15].Value != null ? workSheet.Cells[rowIterator, 15].Value.ToString().Trim() : null;
                            import.NetSuitMapCls_Id = workSheet.Cells[rowIterator, 16].Value != null ? Convert.ToInt32(workSheet.Cells[rowIterator, 16].Value.ToString().Trim()) : (int?)null;
                            import.Error = CheckIsValidStoreUser(import.StoreCode, import.StoreName, import.Address,
                                import.StoreEmailId, import.Region, import.Route, import.StoreManager,
                                import.StoreContactNo, import.CustomerName, import.Location, import.PlaceOfSupply, import.WarehouseDc,
                                import.UserEmailAddress, import.PlanName, import.MkrChkrFlag, import.NetSuitMapCls_Id,
                                Stores, Customers, Locations, warehouseDcs, Plans, NetsuiteMapCls, ChkrMkrList, imports);
                            imports.Add(import);
                        }
                    }
                    im = imports
                           .GroupBy(l => new { l.StoreCode, l.UserEmailAddress, l.MkrChkrFlag })
                           .Select(cl => new StoreUserExcelImport
                           {
                               StoreCode = cl.First().StoreCode,
                               StoreName = cl.First().StoreName,
                               Address = cl.First().Address,
                               StoreEmailId = cl.First().StoreEmailId,
                               Region = cl.First().Region,
                               Route = cl.First().Route,
                               StoreManager = cl.First().StoreManager,
                               StoreContactNo = cl.First().StoreContactNo,
                               CustomerName = cl.First().CustomerName,
                               Location = cl.First().Location,
                               PlaceOfSupply = cl.First().PlaceOfSupply,
                               WarehouseDc = cl.First().WarehouseDc,
                               UserEmailAddress = cl.First().UserEmailAddress,
                               PlanName = cl.First().PlanName,
                               MkrChkrFlag = cl.First().MkrChkrFlag,
                               NetSuitMapCls_Id = cl.First().NetSuitMapCls_Id,
                               Error = cl.First().Error
                           }).ToList();
                    ViewBag.ExcelUsers = im;
                    return View("AddBulkUsers");
                 
                }
            }
            return View("AddBulkUsers");
            
        }

        private List<string> CheckIsValidStoreUser(string storeCode, string storeName, string address, string storeEmailId, string region, string route, string storeManager,
            string storeContactNo, string customerName, string location, string PlaceOfSupply, string warehouseDc, string userEmailAddress, string planName, string mkrChkrFlag, int? NetSuitMapCls_Id,
            List<Store> Stores, List<Customer> Customers, List<Locations> Locations, List<WareHouseDC> warehouseDcs, List<CustomerPlan> Plans, List<NetSuitMapCls> NetsuiteMapCls,
            List<string> ChkrMkrList = null, List<StoreUserExcelImport> excelImports = null)
        {
            List<string> Errors = new List<string>();
            if (excelImports == null)
                excelImports = new List<StoreUserExcelImport>();
            if (ChkrMkrList == null)
                ChkrMkrList = new List<string> { "Chkr", "Mkr", "ChkrMkr" };
            if (storeCode == null || storeCode == "")
            {
                Errors.Add("storecode Cannot be empty.");
            }

            if (storeName == null || storeName == "")
                Errors.Add("StoreName Cannot be empty.");

            if (address == null || storeName == "")
                Errors.Add("address cannot be empty");

            if (storeEmailId == null)
                Errors.Add("please enter email Address");
            else
            {
                if (BAL.HelperCls.IsValidEmail(storeEmailId))
                {
                    var CheckAnotherStoreWithEmailId = excelImports.Any(x => x.StoreCode != storeCode && x.StoreEmailId == storeEmailId);
                    if (CheckAnotherStoreWithEmailId)
                    {
                        Errors.Add("store email Address exist another store in excel");
                    }
                }
                else
                {
                    Errors.Add("inValid email Address");
                }
            }

            bool StoreExist = Stores.Any(x => x.StoreEmailId == storeEmailId);
            if (StoreExist)
                Errors.Add("email address exist");

            bool routecheck = Stores.Any(x => x.Route == route);
            if (route == null || route == "" || !routecheck)
                Errors.Add("invalid route");

            bool regioncheck = Stores.Any(x => x.Region == region);
            if (region == null || region == "" || !regioncheck)
                Errors.Add("invalid region");

            if (storeManager == null || storeManager == "")
                Errors.Add("invalid store manager name.");
            
            bool Customerscheck = Customers.Any(x => x.Name.Trim() == customerName);
            if (customerName == null || customerName == "" || !Customerscheck)
                Errors.Add("invalid customer name");

            bool LocationCheck = Locations.Any(x => x.Name.Trim() == location);
            if (location == null || location == "" || !LocationCheck)
                Errors.Add("invalid location name");

            if (PlaceOfSupply == null || PlaceOfSupply == "")
                Errors.Add("PlaceOfSupply cannot be empty");

            bool warehouseDccheck = warehouseDcs.Any(x => x.Name.Trim() == warehouseDc);
            if (warehouseDc == null || warehouseDc == "" || !warehouseDccheck)
                Errors.Add("invalid warehouseDc");

            if (userEmailAddress == null || userEmailAddress == "" || !(BAL.HelperCls.IsValidEmail(userEmailAddress)))
                Errors.Add("inValid user email Address");
            else
            {
                var usercheck = _userService.GetUsersByEmail(userEmailAddress);
                if (usercheck != null)
                    Errors.Add("user Email Address already exist");
                else
                {
                    var UserAlreadyExist = excelImports.Any(x => x.UserEmailAddress == userEmailAddress);
                    if (UserAlreadyExist)
                        Errors.Add("user Email Address already exist in excel");
                }
            }

            bool plancheck = Plans.Any(x => x.PlanName.Trim() == planName);
            if (planName == null || planName == "" || !plancheck)
                Errors.Add("invalid planName");

            bool mkrChkrFlagchk = ChkrMkrList.Contains(mkrChkrFlag);
            if (mkrChkrFlag == null || mkrChkrFlag == "" || !mkrChkrFlagchk)
                Errors.Add("invalid MkrChkr");

            bool isnetsuiteMap = NetsuiteMapCls.Any(x => x.Id == NetSuitMapCls_Id);
            if (NetSuitMapCls_Id == null || !isnetsuiteMap)
                Errors.Add("invalid NetSuitMapClass_Id");
            return Errors;
        }

        [HttpPost]
        public ActionResult SaveUploadStoreUsers(List<StoreUserExcelImport> excelImports)
        {
            bool IsAllStoresUploaded = true;
            if (excelImports.Count > 0)
            {
                int userid = SessionValues.UserId;
                var data1 = excelImports.Where(x => x.StoreCode != null || x.StoreCode != "" || x.Error != null).GroupBy(x => x.StoreCode).Select(grp => grp.ToList());

                var Stores = data1.ToList();
                for (int i = 0; i < Stores.Count(); i++)
                {
                    var store = Stores[i];
                    var StoreInsert = Stores[i].FirstOrDefault();
                    long StoreId = _storeService.AddStoreExcel(StoreInsert, userid);
                    if (StoreId == 0)
                    {
                        IsAllStoresUploaded = false;
                        break;
                    }
                    else
                        foreach (var StoreUser in store)
                        {
                            StoreUser.StoreId = StoreId;
                            long UserId = _storeService.AddStoreUser(StoreUser, userid);
                            if (UserId == 0)
                            {
                                IsAllStoresUploaded = false;
                                break;
                            }
                        }
                }

               
                var auditJavascript = new JavaScriptSerializer();
                var orderAudit = auditJavascript.Serialize(excelImports);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), orderAudit, Request.Url.OriginalString.ToString(), userid, null);
                return Json(IsAllStoresUploaded, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        [AdminAuthorization]
        public ActionResult MenuRightsForUser(int UserId, int custplanid)
        {
            int Roleid = 1;
            var parent = _menuService.GetParentMenus(Roleid);
            ViewData["AdminID"] = UserId;
            ViewData["CustPlanId"] = custplanid;
            List<Menu> menulist = new List<Menu>();
            foreach (MenuItem par in parent)
            {
                Menu menu = new Menu();
                var plan = _menuService.IsUserMenu(custplanid, par.Id, UserId);
                menu.PlanId = plan > 0 ? plan : 0;
                menu.MenuName = par.MenuName;
                menu.Id = par.Id;
                menulist.Add(menu);
                var child = _menuService.GetChildMenu(par.Id,Roleid);
                foreach (MenuItem chi in child)
                {
                    Menu menu1 = new Menu();
                    menu1.MenuName = menu.MenuName + "=>" + chi.MenuName;
                    var plan1 = _menuService.IsUserMenu(custplanid, chi.Id, UserId);
                    menu1.PlanId = plan1 > 0 ? plan1 : 0;
                    menu1.Id = chi.Id;
                    menulist.Add(menu1);
                    var child1 = _menuService.GetChildMenu(chi.Id,Roleid);
                    foreach (MenuItem chi1 in child1)
                    {
                        Menu menu2 = new Menu();
                        menu2.MenuName = chi.MenuName + "=>" + chi1.MenuName;
                        var plan2 = _menuService.IsUserMenu(custplanid, chi1.Id, UserId);
                        menu2.PlanId = plan2 > 0 ? plan2 : 0;
                        menu2.Id = chi1.Id;
                        menulist.Add(menu2);
                        var child2 = _menuService.GetChildMenu(chi1.Id,Roleid);
                        foreach (MenuItem chi2 in child2)
                        {
                            Menu menu3 = new Menu();
                            menu3.MenuName = chi1.MenuName + "=>" + chi2.MenuName;
                            var plan3 = _menuService.IsUserMenu(custplanid, chi2.Id, UserId);
                            menu3.PlanId = plan3 > 0 ? plan3 : 0;
                            menu3.Id = chi2.Id;
                            menulist.Add(menu3);
                        }
                    }
                }

            }
            ViewBag.Menu = menulist;
            var dashboard = _menuService.DashBoardPer(Roleid);
            List<Menu> DashBoardlist = new List<Menu>();
            foreach (DashBoardPer per in dashboard)
            {
                Menu m = new Menu();
                m.Id = per.Id;
                m.MenuName = per.Name;
                var plan = _menuService.DashBoardMapping(custplanid, per.Id);
                m.PlanId = plan != null ? plan.Id : 0;
                DashBoardlist.Add(m);
            }
            ViewBag.Dash = DashBoardlist;
            return PartialView("_MenuRights");
        }
        [AdminAuthorization]
        public ActionResult StoreUsers(int StoreId)
        {
            Store store = _storeService.GetStoreById(StoreId);
            List<StoreUser> StoreUsers = _storeService.GetUsersByStoreId(StoreId);
            ViewBag.StoreUsers = StoreUsers;
            return View(store);
        }
        [AdminAuthorization]
        public ActionResult AddUser(int StoreId)
        {
            Store store = _storeService.GetStoreById(StoreId);
            if (store != null)
            {
                List<Locations> locations = _locationService.GetAllLocations();
                List<SelectListItem> SelectLocations = locations.ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = a.Name,
                        Value = a.LocationId.ToString(),
                        Selected = false
                    };
                });
                List<SelectListItem> ChrMkrList = new List<SelectListItem>()
                    {
                        new SelectListItem(){Text="ChkrMkr",Value="ChkrMkr"},
                        new SelectListItem(){Text="Mkr",Value="Mkr"},
                        new SelectListItem(){Text="Chkr",Value="Chkr"}
                    };
                Users StoreUser = new Users();
                StoreUser.Department = "Store";
                StoreUser.StoreId = store.Id;
                StoreUser.StoreName = store.StoreName;
                ViewBag.Locations = SelectLocations;
                ViewBag.ChrMkrList = ChrMkrList;
                return View(StoreUser);
            }
            else
            {
                return Content("Sorry Store Doesn't Exist");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(Users user)
        {
            if (ModelState.IsValid)
            {
                var Users = _userService.GetStoreUsersByEmail(user.EmailAddress);
                if (Users != null)
                {
                    List<Locations> locations = _locationService.GetAllLocations();
                    List<SelectListItem> SelectLocations = locations.ConvertAll(a =>
                    {
                        return new SelectListItem()
                        {
                            Text = a.Name,
                            Value = a.LocationId.ToString(),
                            Selected = false
                        };
                    });
                    List<SelectListItem> ChrMkrList = new List<SelectListItem>()
                    {
                        new SelectListItem(){Text="ChkrMkr",Value="ChkrMkr"},
                        new SelectListItem(){Text="Mkr",Value="Mkr"},
                        new SelectListItem(){Text="Chkr",Value="Chkr"}
                    };
                    Users StoreUser = new Users();
                    StoreUser.Department = "Store";
                    ViewBag.Locations = SelectLocations;
                    ViewBag.ChrMkrList = ChrMkrList;
                    ModelState.AddModelError("EmailAddress", "Email Address is Already Exist");
                    return View(user);
                }
                else
                {
                    user.IsDeleted = false;
                    user.CreatedOn = DateTime.Now;
                    user.ReportToId = 1;
                    user.RoleId = 1;
                    user.CreatedBy = Convert.ToInt32(SessionValues.UserId);
                    int userid = _userService.AddStoreUser(user);
                    return RedirectToAction("StoreUsers", "Store", new { StoreId = user.StoreId });
                }
            }
            else
            {
                List<Locations> locations = _locationService.GetAllLocations();
                List<SelectListItem> SelectLocations = locations.ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = a.Name,
                        Value = a.LocationId.ToString(),
                        Selected = false
                    };
                });
                List<SelectListItem> ChrMkrList = new List<SelectListItem>()
                    {
                        new SelectListItem(){Text="ChkrMkr",Value="ChkrMkr"},
                        new SelectListItem(){Text="Mkr",Value="Mkr"},
                        new SelectListItem(){Text="Chkr",Value="Chkr"}
                    };
                Users StoreUser = new Users();
                StoreUser.Department = "Store";
                ViewBag.ChrMkrList = ChrMkrList;
                ViewBag.Locations = SelectLocations;
                return View(user);
              
            }
        }
        [AdminAuthorization]
        public ActionResult EditUser(int UserId)
        {
            Users storeuser = _storeService.GetUserById(UserId);
            if (storeuser != null)
            {
                List<Locations> locations = _locationService.GetAllLocations();
                List<SelectListItem> SelectLocations = locations.ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = a.Name,
                        Value = a.LocationId.ToString(),
                        Selected = false
                    };
                });
                List<SelectListItem> ChrMkrList = new List<SelectListItem>()
                    {
                        new SelectListItem(){Text="ChkrMkr",Value="ChkrMkr"},
                        new SelectListItem(){Text="Mkr",Value="Mkr"},
                        new SelectListItem(){Text="Chkr",Value="Chkr"}
                    };
                UserStore userStore = _storeService.GetUserStore(storeuser.Id);
                storeuser.ChrMkrFlag = userStore.MkrChkrFlag;
                storeuser.StoreId = userStore.StoreId;
                ViewBag.Locations = SelectLocations;
                ViewBag.ChrMkrList = ChrMkrList;
                return View(storeuser);
            }
            else
            {
                return Content("Sorry User Doesn't Exist");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(Users user)
        {
            if (ModelState.IsValid)
            {
                var Users = _userService.GetStoreUsersByEmail(user.EmailAddress);
                if (Users != null && Users.Id != user.Id)
                {
                    List<Locations> locations = _locationService.GetAllLocations();
                    List<SelectListItem> SelectLocations = locations.ConvertAll(a =>
                    {
                        return new SelectListItem()
                        {
                            Text = a.Name,
                            Value = a.LocationId.ToString(),
                            Selected = false
                        };
                    });
                    List<SelectListItem> ChrMkrList = new List<SelectListItem>()
                    {
                        new SelectListItem(){Text="ChkrMkr",Value="ChkrMkr"},
                        new SelectListItem(){Text="Mkr",Value="Mkr"},
                        new SelectListItem(){Text="Chkr",Value="Chkr"}
                    };
                    UserStore userStore = _storeService.GetUserStore(user.Id);
                    user.ChrMkrFlag = userStore.MkrChkrFlag;
                    user.StoreId = userStore.StoreId;
                    ViewBag.Locations = SelectLocations;
                    ViewBag.ChrMkrList = ChrMkrList;
                    ModelState.AddModelError("EmailAddress", "Email Address is Already Exist");
                    return View(user);
                }
                else
                {
                    user.IsDeleted = false;
                    user.CreatedOn = DateTime.Now;
                    user.ReportToId = 1;
                    user.RoleId = 1;
                    user.ModifiedBy = Convert.ToInt32(SessionValues.UserId);
                    int userid = _userService.EditStoreUser(user);
                    return RedirectToAction("StoreUsers", "Store", new { StoreId = user.StoreId });
                }
            }
            else
            {
                List<Locations> locations = _locationService.GetAllLocations();
                List<SelectListItem> SelectLocations = locations.ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = a.Name,
                        Value = a.LocationId.ToString(),
                        Selected = false
                    };
                });
                List<SelectListItem> ChrMkrList = new List<SelectListItem>()
                    {
                        new SelectListItem(){Text="ChkrMkr",Value="ChkrMkr"},
                        new SelectListItem(){Text="Mkr",Value="Mkr"},
                        new SelectListItem(){Text="Chkr",Value="Chkr"}
                    };
                UserStore userStore = _storeService.GetUserStore(user.Id);
                user.ChrMkrFlag = userStore.MkrChkrFlag;
                user.StoreId = userStore.StoreId;
                ViewBag.Locations = SelectLocations;
                ViewBag.ChrMkrList = ChrMkrList;
                return View(user);
                
            }
        }
        [HttpPost]
        [AdminAuthorization]
        public ActionResult DeleteStoreUser(int StoreUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var storeuser = _storeService.GetUserById(StoreUser);
                    storeuser.IsDeleted = true;
                    int storeuserdeleted = _storeService.DeleteStoreUser(storeuser.Id);
                    try
                    {
                        var Auditstore = new Users(storeuser.Id, storeuser.UserId, storeuser.Name, storeuser.Department, storeuser.EmailAddress, storeuser.Password, storeuser.RoleId);
                        var auditJavascript = new JavaScriptSerializer();
                        var str = auditJavascript.Serialize(Auditstore);
                        _auditService.InsertAudit(HelperCls.GetIPAddress(), str, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                        
                    }
                    catch (Exception ex)
                    {
                        HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                        throw ex;
                    }
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return RedirectToAction("Error500", "Home");
            }
        }

        [AdminAuthorization]
        public ActionResult WidgetsForUser(int UserId, int custplanid)
        {
            int Roleid = 1;
            var parent = _menuService.GetParentMenus(Roleid);
            ViewData["AdminID"] = UserId;
            ViewData["CustPlanId"] = custplanid;
            var dashboard = _menuService.DashBoardPer(Roleid);
            List<Menu> DashBoardlist = new List<Menu>();
            foreach (DashBoardPer per in dashboard)
            {
                Menu m = new Menu();
                m.Id = per.Id;
                m.MenuName = per.Name;
                m.IsShow = _menuService.IsUserDashboard(per.Id, custplanid, UserId);
                DashBoardlist.Add(m);
            }
            ViewBag.Dash = DashBoardlist;
            return PartialView("_WidgetRights");
        }

        [AdminApiAuthorization]
        [HttpPost]
        public JsonResult SaveWidgetForStoreUser(List<Menu> menus, int CustPlanId, int StoreUserId)
        {
            int RoleId = 1;
            int custplanid = menus.FirstOrDefault().custplanid;
            int afeectedrow = _menuService.DeleteWidgetMapingForUser(custplanid, RoleId, StoreUserId);
            try
            {
                var auditJavascript = new JavaScriptSerializer();
                var MenuAudit = auditJavascript.Serialize("Delete WidgetMaping CustPlanId: " + custplanid + ", RoleId: " + RoleId);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), MenuAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            foreach (Menu menu in menus)
            {
                DashBoardMapping map = new DashBoardMapping();
                map.DashBoardPer = _menuService.DashBoardPerbyid(menu.Id);
                map.Users = _storeService.GetUserById(StoreUserId);
                map.IsShow = menu.IsShow;
                int afec = _menuService.InsertWidgetForUser(map);
                try
                {
                    var mapAudit = new DashBoardMapping(map.Id, map.CustomerPlan != null ? map.CustomerPlan.Id : (int?)null, map.Store != null ? map.Store.Id : (int?)null, map.Store != null ? map.Users.Id : (int?)null, map.DashBoardPer != null ? map.DashBoardPer.Id : (int?)null, map.IsShow);
                    var auditJavascript = new JavaScriptSerializer();
                    var MenuAudit = auditJavascript.Serialize(mapAudit);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), MenuAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    throw ex;
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorization]
        public ActionResult ResetStoreUserPassword()
        {
            return View();
        }
        [AdminAuthorization]
        public ActionResult AutoStoreCode(string search)
        {
            int CustId =Convert.ToInt32( SessionValues.LoggedInCustId);
            List<Store> stores = _storeService.GetStoresByCustomerId(CustId);
            List<StoreSuggesions> StoreSuggesions;
            if (search == null || search == "")
                StoreSuggesions = (from store in stores
                                   select new StoreSuggesions { data = store.Id, value = store.StoreCode }).ToList();
            else
                StoreSuggesions = (from store in stores
                                   where store.StoreCode.Contains(search)
                                   select new StoreSuggesions { data = store.Id, value = store.StoreCode }).ToList();
            var suggesions = new Suggesions() { query = search, suggestions = StoreSuggesions };
            return Json(StoreSuggesions, JsonRequestBehavior.AllowGet);
          
        }
        [AdminAuthorization]
        public ActionResult GetStoreUserForStoreId(int StoreId, string StoreCode)
        {
            int CustId = Convert.ToInt32(SessionValues.LoggedInCustId);
            List<StoreUser> storeusers = new List<StoreUser>();
            if (StoreId == 0)
                return Json(new { data = storeusers }, JsonRequestBehavior.AllowGet);
            else
            {
                storeusers = _storeService.GetUserByStoreId(StoreId);
                return Json(new { data = storeusers }, JsonRequestBehavior.AllowGet);
            }
           
        }
        [AdminApiAuthorization]
        [HttpPost]
        public JsonResult ResetPasswordForUser(int UserId)
        {
            Users storeuser = _storeService.GetUserById(UserId);
            int LoggedInUserId = SessionValues.UserId;
            if (storeuser != null)
            {
                int rowsAffected = _storeService.ResetPasswordForUser(storeuser, LoggedInUserId);
                if (rowsAffected > 0)
                {
                    try
                    {
                        var auditJavascript = new JavaScriptSerializer();
                        var StoreAudit = auditJavascript.Serialize("StoreUserId: " + storeuser.Id + ", Password: " + storeuser.Password);
                        bool cond = HelperCls.SendEmail(storeuser.EmailAddress, "Password Reset", HelperCls.PopulateResetPassword(storeuser.Name));
                        _auditService.InsertAudit(HelperCls.GetIPAddress(), StoreAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                    }
                    catch (Exception ex)
                    {
                        HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    }
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        //[OTPAuthorization]
        public ActionResult OTP(string id)
        {
            if (id != null)
            {
                try
                {
                    if (HelperCls.DecodeServerName(id) != OTPSession.UserId.ToString())
                    {
                        FormsAuthentication.SignOut();
                        Session.Abandon();

                        // clear authentication cookie
                        HttpCookie cookie1 = new HttpCookie("OTPCookie", "");
                        cookie1.Expires = DateTime.Now.AddYears(-1);
                        Response.Cookies.Add(cookie1);

                        // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
                        SessionStateSection sessionStateSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
                        HttpCookie cookie2 = new HttpCookie("OTPCookie", "");
                        cookie2.Expires = DateTime.Now.AddYears(-1);
                        Response.Cookies.Add(cookie2);
                        return RedirectToAction("Login", new { Error = "Invalid Link." });
                    }
                    else
                    {
                        try
                        {
                            OTPViewModel OtpViewModel = new OTPViewModel();
                            OtpViewModel.UserId = Convert.ToInt32(HelperCls.DecodeServerName(id));
                            return View(OtpViewModel);
                        }
                        catch (Exception ex)
                        {
                            FormsAuthentication.SignOut();
                            Session.Abandon();

                            // clear authentication cookie
                            HttpCookie cookie1 = new HttpCookie("OTPCookie", "");
                            cookie1.Expires = DateTime.Now.AddYears(-1);
                            Response.Cookies.Add(cookie1);

                            // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
                            SessionStateSection sessionStateSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
                            HttpCookie cookie2 = new HttpCookie("OTPCookie", "");
                            cookie2.Expires = DateTime.Now.AddYears(-1);
                            Response.Cookies.Add(cookie2);
                            return RedirectToAction("Login", new { Error = "Invalid Link." });
                        }
                    }
                }
                catch (Exception Ex)
                {
                    Session.Clear();
                    HelperCls.DebugLog("StackTrace: " + Ex.Message + " \n StackTrace: " + Ex.StackTrace);
                    return RedirectToAction("Login", new { Error = "Invalid Link." });
                }
            }
            else
            {
                Session.Clear();
                HelperCls.DebugLog("HashUserID: " + id);
                return RedirectToAction("Login", new { Error = "Invalid Link." });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OTPAuthorization]
        public ActionResult OTP(FormCollection fomr)
        {
            OTPViewModel OTP = new OTPViewModel();
            OTP.UserId = Convert.ToInt32(Request.Form["UserId"].ToString());
            OTP.OTP = Request.Form["OTP"].ToString();
            if (ModelState.IsValid)
            {
                if (OTPSession.UserId != 0 && OTPSession.OTP != null && OTP.UserId != 0)
                {
                    try
                    {
                        if (OTP.UserId != OTPSession.UserId)
                        {
                            Session.Clear();
                            FormsAuthentication.SignOut();
                            Session.Abandon();

                            // clear authentication cookie
                            HttpCookie cookie1 = new HttpCookie("OTPCookie", "");
                            cookie1.Expires = DateTime.Now.AddYears(-1);
                            Response.Cookies.Add(cookie1);

                            // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
                            SessionStateSection sessionStateSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
                            HttpCookie cookie2 = new HttpCookie("OTPCookie", "");
                            cookie2.Expires = DateTime.Now.AddYears(-1);
                            Response.Cookies.Add(cookie2);
                            return RedirectToAction("Login", new { Error = "Invalid Link." });
                        }
                        else if (OTP.OTP != OTPSession.OTP.ToString())
                        {
                            ModelState.AddModelError("OTP", "Wrong OTP");
                            return View(OTP);
                        }
                        else
                        {
                            var storeuser = _storeService.GetUserById(OTP.UserId);
                            if (storeuser != null)
                            {
                                storeuser.Password = "Rfpl@123";
                                storeuser.Modified = DateTime.Now;
                                _storeService.EditUser(storeuser);
                                Session.Clear();
                                FormsAuthentication.SignOut();
                                Session.Abandon();

                                // clear authentication cookie
                                HttpCookie cookie1 = new HttpCookie("OTPCookie", "");
                                cookie1.Expires = DateTime.Now.AddYears(-1);
                                Response.Cookies.Add(cookie1);

                                // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
                                SessionStateSection sessionStateSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
                                HttpCookie cookie2 = new HttpCookie("OTPCookie", "");
                                cookie2.Expires = DateTime.Now.AddYears(-1);
                                Response.Cookies.Add(cookie2);
                                return RedirectToAction("Login", "Store",new { Message="Password is reset."});
                            }
                            else
                            {
                                Session.Clear();
                                FormsAuthentication.SignOut();
                                Session.Abandon();

                                // clear authentication cookie
                                HttpCookie cookie1 = new HttpCookie("OTPCookie", "");
                                cookie1.Expires = DateTime.Now.AddYears(-1);
                                Response.Cookies.Add(cookie1);

                                // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
                                SessionStateSection sessionStateSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
                                HttpCookie cookie2 = new HttpCookie("OTPCookie", "");
                                cookie2.Expires = DateTime.Now.AddYears(-1);
                                Response.Cookies.Add(cookie2);
                                return RedirectToAction("Login", new { Error = "Invalid Data." });
                            }
                        }
                    }
                    catch (Exception Ex)
                    {
                        Session.Clear();
                        HelperCls.DebugLog("StackTrace: " + Ex.Message + " \n StackTrace: " + Ex.StackTrace);
                        return RedirectToAction("Login", new { Error = "Invalid Link." });
                    }
                }
                else
                {
                    FormsAuthentication.SignOut();
                    Session.Abandon();

                    // clear authentication cookie
                    HttpCookie cookie1 = new HttpCookie("OTPCookie", "");
                    cookie1.Expires = DateTime.Now.AddYears(-1);
                    Response.Cookies.Add(cookie1);

                    // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
                    SessionStateSection sessionStateSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
                    HttpCookie cookie2 = new HttpCookie(sessionStateSection.CookieName, "");
                    cookie2.Expires = DateTime.Now.AddYears(-1);
                    Response.Cookies.Add(cookie2);
                    return RedirectToAction("Login", new { Error = "Session TimeOut." });
                }
            }
            else
            {
                return View(OTP);
            }
        }
    }
}