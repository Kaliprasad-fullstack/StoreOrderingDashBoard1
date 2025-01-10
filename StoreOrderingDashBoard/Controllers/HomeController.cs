using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BAL;
using DbLayer.Service;
using StoreOrderingDashBoard.Models;
using DAL;
using System.Web.Security;
using System.Web.Configuration;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security.Cookies;
using System.Reflection;

namespace StoreOrderingDashBoard.Controllers
{

    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IReportService _reportService;
        private readonly IItemService _itemService;
        private readonly IWareHouseService _wareHouseService;
        private readonly ICustomerService _customerService;
        private readonly ILocationService _locationService;
        private readonly IMenuService _menuService;
        //public HomeController(IUserService userService,IReportService reportService,IItemService itemService,ICustomerService customerService)
        private readonly IAuditService _auditService;
        public HomeController(IUserService userService, IReportService reportService, IItemService itemService, ICustomerService customerService, IAuditService AuditService, ILocationService LocationService, IMenuService menuService, IWareHouseService wareHouseService)
        {
            _userService = userService;
            _reportService = reportService;
            _itemService = itemService;
            _customerService = customerService;
            _auditService = AuditService;
            _locationService = LocationService;
            _menuService = menuService;
            _wareHouseService = wareHouseService;
        }

        [HttpPost]
        public ActionResult ForgotAnswred(int UserId)
        {
            var admin = _userService.GetAdminUserByUserId(UserId);
            HelperCls.SendEmail(admin.EmailAddress, "Forgot Answer", HelperCls.PopulateForgetAnswer(admin.Name, HelperCls.EncodeServerName(admin.Id.ToString())));
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorization]
        public ActionResult Customers()
        {
            if (SessionValues.UserId != 0)
            {
                int userid = SessionValues.UserId;
                var customer = _customerService.customerLists(userid);
                var question = _customerService.GetQuestion(userid);
                ViewData["Attempt"] = _menuService.GetMaster("Question Attempt").Value;
                List<CustomerList> customerLists = new List<CustomerList>();
                Random random = new Random();
                foreach (CustomerList list in customer)
                {
                    var indexes = Enumerable.Range(0, question.Count()).OrderBy(i => random.Next());
                    int a = indexes.ToList().FirstOrDefault();
                    list.Question = question[a].ques;
                    list.QuestionId = question[a].Id;
                    customerLists.Add(list);
                }
                ViewBag.Customer = customerLists;
                return View();
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }

        }

        [AdminApiAuthorization]
        public JsonResult DisableUser()
        {
            bool cond = _userService.DeleteAdminUser(SessionValues.UserId) > 0 ? true : false;
            return Json(cond, JsonRequestBehavior.AllowGet);
        }

        [AdminApiAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Customerwiselogin(int UserId, int custId)
        {
            try
            {
                int LoggedUserId = SessionValues.UserId;
                //var cust = _customerService.customerLists(LoggedUserId);
                var Loggingcustomer = _customerService.GetCustomerforUserId(custId, LoggedUserId);
                if (Loggingcustomer != null && Loggingcustomer.Count() > 0)
                {
                    try
                    {
                        var javascript = new JavaScriptSerializer();
                        //var answers = new Answer(cust.Id, cust.Answers);
                        var str = "";//javascript.Serialize(answers);
                        _auditService.InsertAudit(HelperCls.GetIPAddress(), str, Request.Url.OriginalString.ToString(), UserId, null);
                    }
                    catch (Exception ex)
                    {
                        HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                        throw ex;
                    }
                    var request = Request;
                    string SessionTime = _menuService.GetMaster("Session Time").Value;
                    ViewBag.Password = "True";
                    var user = _userService.GetUsersByEmail(SessionValues.StoreUserName);
                    HttpContext.Items.Add("User", user);
                    ViewBag.Password = "True";
                    UserSession userSession = new UserSession();
                    userSession.RoleId = user.RoleId != 0 ? user.RoleId : 1;
                    userSession.StoreId = 0;
                    userSession.UserId = user.Id;
                    userSession.CustomerId = custId;
                    userSession.UserName = SessionValues.StoreUserName;
                    userSession.PlanId = user.CustomerPlan.Id;
                    userSession.MkrChkrFlag = 0.ToString();
                    userSession.SessionTimeout = SessionTime;
                    userSession.EmailAddress = (user.EmailAddress != null ? user.EmailAddress : "");
                    userSession.StoreName = user.Name;
                    var userData = new JavaScriptSerializer().Serialize(userSession);
                    Session["AuthUserData"] = user.RoleId + "|" + 0 + "|" + user.Id + "|" + custId + "|" + SessionValues.StoreUserName.ToString() + "|" + user.CustomerPlan.Id + "|" + 0 + "|" + SessionTime + "|" + (user.EmailAddress != null ? user.EmailAddress : "") + "|" + 0 + "|" + user.Name;
                    var ticket = new FormsAuthenticationTicket(1, (user.Id.ToString() + user.Name), DateTime.Now, DateTime.Now.AddMinutes(Convert.ToDouble(SessionTime)), true, userData);
                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    // Create the cookie.
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                }
                bool cond = Loggingcustomer.Count() > 0 && Loggingcustomer != null ? true : false;
                return Json(cond, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);

                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        [AdminAuthorization]
        public ActionResult Index()
        {
            if (SessionValues.LoggedInCustId != null)
            {
                #region DashBoardCount
                int custid = SessionValues.LoggedInCustId.Value;
                int userid = SessionValues.UserId;
                var DCs = _wareHouseService.GetWareHouseForCustomerId(userid, custid, "Report");
                //var OFRLIFR_MTD = _reportService.OFRLIFR_MTD(userid, custid);
                //var count = _reportService.MTDCls(userid, custid);
                //var finilized = _reportService.ProcessedData(false, true, false, false, false, custid, userid);
                //var processed = _reportService.ProcessedData(true, true, false, false, false, custid, userid);

                //var fullfillment = _reportService.ProcessedData(true, true, true, true, false, custid, userid);

                //var invoice = _reportService.ProcessedData(true, true, true, true, true, custid, userid);

                //var soapproved = _reportService.SoApproved(custid, userid);
                //int OrdersStoresToBePlaced = _reportService.GetOrdersStoresToBePlaced(custid, userid);                
                //ViewData["Finilized"] = count[0].Finalized_order_count;
                //ViewData["Processed"] = count[0].Processed_order_count;
                //ViewData["SoApproved"] = count[0].SO_Approved_count;
                //ViewData["FullFillment"] = count[0].Fullfilled_count;
                //ViewData["Invoice"] = count[0].Invoiced_count;
                //ViewData["OFR"] = String.Format("{0:0.00}", OFRLIFR_MTD.OFRPercentage) + "%";
                //ViewData["LIFR"] = String.Format("{0:0.00}", OFRLIFR_MTD.LIFRPercentage) + "%";
                //ViewData["OrdersStoresToBePlaced"] = OrdersStoresToBePlaced;
                //DateTime now = DateTime.Now;
                //var startDate = new DateTime(now.Year, now.Month, 1);
                //var endDate = startDate.AddMonths(1).AddDays(-1);
                //ViewData["MonthFinilize"] = count[1].Finalized_order_count; 
                //ViewData["MonthProcess"] = count[1].Processed_order_count;
                //ViewData["MonthSoApprove"] = count[1].SO_Approved_count;
                //ViewData["MonthFullFillment"] = count[1].Fullfilled_count;
                //ViewData["MonthInvoice"] = count[1].Invoiced_count;
                ViewBag.DCs = new MultiSelectList(DCs.ToList(), "Id", "Name", null);
                #endregion
                #region Graph
                //List<int> finilizelist = new List<int>();
                //List<int> Processedlist = new List<int>();
                //for (int i = 1; i <= 12; i++)
                //{
                //    var startDate1 = new DateTime(now.Year, i, 1);
                //    var endDate1 = startDate1.AddMonths(1).AddDays(-1);
                //    var monthfinilize1 = from a in finilized
                //                         where a.OrderDate >= startDate1 && a.OrderDate <= endDate1
                //                         select a;
                //    finilizelist.Add(monthfinilize1.Count());

                //    var monthProcess1 = from b in processed
                //                        where b.ProcessedDate >= startDate1 && b.ProcessedDate <= endDate1
                //                        select b;
                //    Processedlist.Add(monthProcess1.Count());

                //    //var monthsaveasdraft1 = from c in saveasdraft
                //    //                       where c.OrderDate >= startDate && c.OrderDate <= endDate
                //    //                       select c;
                //    //Draftlist.Add(monthsaveasdraft.Count());
                //}
                //JavaScriptSerializer js = new JavaScriptSerializer();
                //string s1 = js.Serialize(finilizelist);
                //ViewData["FinilizeList"] = s1;
                //string s2 = js.Serialize(Processedlist);
                //ViewData["ProcessedList"] = s2;
                ////graph
                //var top5sku = _itemService.Top5SKUs(startDate, endDate, custid, userid);
                //string s3 = js.Serialize(top5sku);
                //ViewData["Top5SKU"] = s3;

                //var top5store = _reportService.Top5Stores(startDate, endDate, custid, userid);
                //string s4 = js.Serialize(top5store);
                //ViewData["Top5Store"] = s4;
                #endregion
                return View();
            }
            else
            {
                return RedirectToAction("Customers", "Home");
            }
        }

        [AdminApiAuthorization]
        public JsonResult BarGraphResult()
        {
            int custid = SessionValues.LoggedInCustId.Value;
            var saveasdraft = _reportService.GetSaveAsDraftOrders();
            var finilized = _reportService.GetFinilizeOrder(custid);
            var processed = _reportService.GetProcessedOrder(custid);
            List<int> finilizelist = new List<int>();
            List<int> Processedlist = new List<int>();
            List<int> Draftlist = new List<int>();
            DateTime now = DateTime.Now;
            for (int i = 1; i <= 12; i++)
            {
                var startDate = new DateTime(now.Year, i, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                var monthfinilize = from a in finilized
                                    where a.OrderDate >= startDate && a.OrderDate <= endDate
                                    select a;
                finilizelist.Add(monthfinilize.Count());


            }
            return Json(finilizelist);
        }

        public ActionResult LogIn(string ForgotAnswers, string Exp, string Error, string IsSessionExpired)
        {
            ViewBag.Password = "True";
            SessionValues.AdminCount = 0;
            ViewData["Attempt"] = _menuService.GetMaster("Login Attempt For Admin").Value;
            ViewBag.IsSessionExpired = IsSessionExpired;

            if (Error == null)
            {
                ViewBag.ForgotAnswers = (ForgotAnswers != null ? ForgotAnswers : "");
                ViewBag.Expires = (Exp != null ? Exp : "");
            }
            ViewBag.Errors = Error;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotAnwerLogin(Login login)
        {
            ViewBag.Password = "False";
            var user = _userService.GetUser(login.Username, login.Password);
            if (user != null)
            {
                //HttpContext.Items.Add("User", user);
                var request = Request;
                ViewBag.Password = "True";
                try
                {
                    var javascript = new JavaScriptSerializer();
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), javascript.Serialize(login), Request.Url.OriginalString.ToString(), user.Id, null);
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    throw ex;
                }
                return RedirectToAction("Customers", "Home");
            }
            return View("LogIn");
        }

        public ActionResult LogOut()
        {
            if (Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.SignOut(OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
            }
            FormsAuthentication.SignOut();
            Session.Abandon();
            Response.Cookies.Clear();
            Response.Cookies[FormsAuthentication.FormsCookieName].Expires = DateTime.Now.AddDays(-1);
            Session.Clear();
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(Login login)
        {
            ViewBag.Password = "False";
            ViewBag.ForgotAnswers = login.ForgotAnswers;
            ViewBag.Expires = login.Expires;
            var user = _userService.GetUser(login.Username, login.Password);
            if (user != null)
            {
                HttpContext.Items.Add("User", user);
                var request = Request;
                ViewBag.Password = "True";
                //Admin admin = new Admin(user.Id, user.Name, user.CustomerPlan.Id);
                string SessionTime = _menuService.GetMaster("Session Time").Value;
                UserSession userSession = new UserSession();
                userSession.RoleId = user.RoleId != 0 ? user.RoleId : 1;
                userSession.StoreId = 0;
                userSession.UserId = user.Id;
                userSession.UserName = login.Username;
                userSession.PlanId = user.CustomerPlan.Id;
                userSession.MkrChkrFlag = 0.ToString();
                userSession.SessionTimeout = SessionTime;
                userSession.EmailAddress = (user.EmailAddress != null ? user.EmailAddress : "");
                userSession.StoreName = user.Name;

                var customers = _customerService.customerLists(user.Id);
                int FirstCustId = customers != null && customers.Count() > 0 ? customers.FirstOrDefault().Id : 0;
                if (FirstCustId == 0)
                {
                    return RedirectToAction("Customers", "Home");
                }
                userSession.CustomerId = FirstCustId;
                var userData = new JavaScriptSerializer().Serialize(userSession);

                Session["AuthUserData"] = user.RoleId + "|" + 0 + "|" + user.Id + "|" + FirstCustId + "|" + login.Username.ToString() + "|" + user.CustomerPlan.Id + "|" + 0 + "|" + SessionTime + "|" + (user.EmailAddress != null ? user.EmailAddress : "") + "|" + 0 + "|" + user.Name;
                var ticket = new FormsAuthenticationTicket(1, (user.Id.ToString() + user.Name), DateTime.Now, DateTime.Now.AddMinutes(Convert.ToDouble(SessionTime)), true, userData);
                // Encrypt the ticket.
                SessionValues.AdminCount = 0;
                string encTicket = FormsAuthentication.Encrypt(ticket);
                // Create the cookie.
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                if (BAL.GlobalValues.PasswordPolicyStatus)
                {
                    DateTime? PasswordUpdatedDate = user.Modified != null ? user.Modified : user.CreatedOn;
                    if (PasswordUpdatedDate != null)
                    {
                        DateTime NowDate = DateTime.Now;
                        double Difference = NowDate.Subtract(PasswordUpdatedDate.Value).Days;
                        if (BAL.GlobalValues.PasswordPolicyDays <= Difference)
                        {
                            return RedirectToAction("ChangePassword", "Home");
                        }
                    }
                    else
                    {
                        return RedirectToAction("ChangePassword", "Home");
                    }
                }
                try
                {
                    var javascript = new JavaScriptSerializer();
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), javascript.Serialize(login), Request.Url.OriginalString.ToString(), user.Id, null);
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    throw ex;
                }

                if (login.ForgotAnswers != null && login.ForgotAnswers != "")
                {
                    return RedirectToActionPermanent("ForgotAnswer", "Home", new { Id = login.ForgotAnswers, Exp = login.Expires });
                }
                // return RedirectToAction("Index", "Home");                
                return RedirectToAction("Index", "Home");
            }
            SessionValues.AdminCount = SessionValues.AdminCount + 1;
            ViewData["Attempt"] = _menuService.GetMaster("Login Attempt For Admin").Value;
            if (SessionValues.AdminCount == Convert.ToInt32(ViewData["Attempt"].ToString()))
                _userService.DisableAdminUser(login.Username, 2);
            return View("LogIn");
        }

        [HttpGet]
        public ActionResult ExtendSession()
        {
            var request = Request;
            string SessionTime = _menuService.GetMaster("Session Time").Value;
            ViewBag.Password = "True";
            var user = _userService.GetUsersByEmail(SessionValues.StoreUserName);
            HttpContext.Items.Add("User", user);
            ViewBag.Password = "True";
            UserSession userSession = new UserSession();
            userSession.RoleId = user.RoleId != 0 ? user.RoleId : 1;
            userSession.StoreId = 0;
            userSession.UserId = user.Id;
            userSession.UserName = SessionValues.StoreUserName;
            userSession.PlanId = user.CustomerPlan.Id;
            userSession.MkrChkrFlag = 0.ToString();
            userSession.SessionTimeout = SessionTime;
            userSession.EmailAddress = (user.EmailAddress != null ? user.EmailAddress : "");
            userSession.StoreName = user.Name;

            var customers = _customerService.customerLists(user.Id);
            userSession.CustomerId = SessionValues.LoggedInCustId.Value;
            var userData = new JavaScriptSerializer().Serialize(userSession);
            Session["AuthUserData"] = user.RoleId + "|" + 0 + "|" + user.Id + "|" + SessionValues.LoggedInCustId.Value + "|" + SessionValues.StoreUserName.ToString() + "|" + user.CustomerPlan.Id + "|" + 0 + "|" + SessionTime + "|" + (user.EmailAddress != null ? user.EmailAddress : "") + "|" + 0 + "|" + user.Name;
            var ticket = new FormsAuthenticationTicket(1, (user.Id.ToString() + user.Name), DateTime.Now, DateTime.Now.AddMinutes(Convert.ToDouble(SessionTime)), true, userData);
            string encTicket = FormsAuthentication.Encrypt(ticket);
            // Create the cookie.
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
            var data = new { IsSuccess = true };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AdminApiAuthorization]
        [HttpPost]
        public JsonResult ReturnURL(string Email, string LastName, string ProfileURL, string ForgotAnswer, string Expires)
        {
            try
            {
                var user = _userService.GetUsersByEmail(Email);
                bool cond = user != null ? true : false;
                try
                {
                    var javascript = new JavaScriptSerializer();
                    Users us = new Users(user.Id, user.UserId, user.Name, user.Department, user.EmailAddress, user.Password, user.RoleId);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), javascript.Serialize(us), Request.Url.OriginalString.ToString(), user.Id, null);
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    throw ex;
                }
                return Json(cond, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Error500()
        {
            if (Session["AppErrSession"] != null)
            {
                ViewData["AppError"] = Session["AppErrSession"].ToString();
            }
            return View();
        }

        public ActionResult Error404()
        {
            return View();
        }

        public ActionResult DashBoard2()
        {
            return View();
        }

        public ActionResult ForgotAnswer(string Id, string Exp)
        {
            if (SessionValues.UserId != 0)
            {
                try
                {
                    int UserId = Convert.ToInt32(HelperCls.DecodeServerName(Id));
                    DateTime Expire = Convert.ToDateTime(HelperCls.DecodeServerName(Exp));
                    int result = DateTime.Compare(Expire, DateTime.Now);
                    if (result < 0)
                    {
                        Session.Clear();
                        return RedirectToAction("Login", new { Error = "Forgot Password Link Expire..." });
                    }
                    int LoggedInUserId = SessionValues.UserId;
                    if (UserId == LoggedInUserId)
                    {
                        List<Answer> answers = _userService.GetAnswersForUserId(LoggedInUserId);
                        ViewBag.UserId = Id;
                        try
                        {
                            var javascript = new JavaScriptSerializer();
                            var AnswerList = from answer in answers
                                             select new { answer.Answers, QuestionId = answer.Question.Id, Question = answer.Question.Ques, UserId = answer.Users.Id };
                            _auditService.InsertAudit(HelperCls.GetIPAddress(), javascript.Serialize(AnswerList), Request.Url.OriginalString.ToString(), LoggedInUserId, null);
                        }
                        catch (Exception ex)
                        {
                            HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                            throw ex;
                        }
                        return View(answers);
                    }
                    else
                    {
                        Session.Clear();
                        return RedirectToAction("Logout");
                    }
                }
                catch (Exception ex)
                {
                    Session.Clear();
                    return RedirectToAction("Logout");
                }
            }
            else
            {
                Session.Clear();
                return RedirectToActionPermanent("Logout");
            }
        }

        [AdminAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotAnswer(List<AllQuestion> AllQuestions)
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangeQuestionAnswers(List<AllQuestion> AllQuestions, string UserId)
        {
            if (ModelState.IsValid)
            {
                if (AllQuestions.Any(x => x.answer == "" || x.answer == null || x.Id == 0))
                {
                    if (SessionValues.UserId != 0)
                    {
                        try
                        {
                            int LoggedUserId = Convert.ToInt32(HelperCls.DecodeServerName(UserId));
                            int LoggedInUserId = SessionValues.UserId;
                            if (LoggedUserId == LoggedInUserId)
                            {
                                List<Answer> answers = _userService.GetAnswersForUserId(LoggedInUserId);
                                foreach (Answer answer in answers)
                                {
                                    if (AllQuestions.Any(x => x.Id == answer.Question.Id))
                                        answer.Answers = AllQuestions.Where(x => x.Id == answer.Question.Id).Select(x => x.answer).FirstOrDefault();
                                }
                                return View(answers);
                            }
                            else
                            {
                                Session.Clear();
                                return RedirectToAction("Logout");
                            }
                        }
                        catch (Exception ex)
                        {
                            //var javascript = new JavaScriptSerializer();
                            //Users us = new Users(user.Id, user.UserId, user.Name, user.Department, user.EmailAddress, user.Password, user.RoleId);
                            //_auditService.InsertAudit(HelperCls.GetIPAddress(), javascript.Serialize(us), Request.Url.OriginalString.ToString(), user.Id, null);
                            Session.Clear();
                            return RedirectToAction("Logout");
                        }
                    }
                    else
                    {
                        Session.Clear();
                        return RedirectToActionPermanent("Logout");
                    }
                }
                else
                {
                    if (SessionValues.UserId != 0)
                    {
                        try
                        {
                            int LoggedUserId = Convert.ToInt32(HelperCls.DecodeServerName(UserId));
                            int LoggedInUserId = SessionValues.UserId;
                            if (LoggedUserId == LoggedInUserId)
                            {
                                List<Answer> answers = _userService.GetAnswersForUserId(LoggedInUserId);
                                int SavedRows = 0;
                                foreach (AllQuestion question in AllQuestions)
                                {
                                    question.UserId = LoggedInUserId;
                                    if (answers.Any(x => x.Question.Id == question.Id))
                                    {
                                        int rowsaffected = _userService.ChangeAnswerForUserQuestions(question);
                                        if (rowsaffected > 0)
                                            SavedRows += rowsaffected;
                                    }
                                }
                                Session.Clear();
                                return RedirectToAction("Logout");
                            }
                            else
                            {
                                Session.Clear();
                                return RedirectToAction("Logout");
                            }
                        }
                        catch (Exception ex)
                        {
                            //var javascript = new JavaScriptSerializer();
                            //Users us = new Users(user.Id, user.UserId, user.Name, user.Department, user.EmailAddress, user.Password, user.RoleId);
                            //_auditService.InsertAudit(HelperCls.GetIPAddress(), javascript.Serialize(us), Request.Url.OriginalString.ToString(), user.Id, null);
                            Session.Clear();
                            return RedirectToAction("Logout");
                        }
                    }
                    else
                    {
                        Session.Clear();
                        return RedirectToActionPermanent("Login", new { ForgotAnswers = UserId });
                    }
                }
            }
            return View();
        }

        public ActionResult ChangePassword()
        {
            if (SessionValues.UserId != 0)
            {
                ViewBag.SamePassword = false;
                ViewBag.Password = "True";
                try
                {
                    int LoggedInUserId = SessionValues.UserId;
                    Users LoggedInUser = _userService.GetAdminUserByUserId(LoggedInUserId);
                    if (LoggedInUser != null)
                    {
                        return View(LoggedInUser);
                    }
                    else
                    {
                        Session.Clear();
                        return RedirectToAction("Login");
                    }
                }
                catch (Exception ex)
                {
                    Session.Clear();
                    return RedirectToAction("Login");
                }
            }
            else
            {
                Session.Clear();
                return RedirectToActionPermanent("Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(Users user)
        {
            var users = _userService.GetAdminUserByUserId(user.Id);
            ViewBag.Password = "False";
            ViewBag.SamePassword = false;
            if (users.Password == user.Password)
            {
                if (BAL.GlobalValues.SameOldNewPassWord && users.Password == user.NewPassword)
                {
                    ViewBag.Password = "True";
                    ViewBag.SamePassword = true;
                    return View(users);
                }
                else
                {
                    ViewBag.Password = "True";
                    users.Password = user.NewPassword;
                    users.Modified = DateTime.Now;
                    int affectedrow = _userService.EditAdminUser(users);
                    try
                    {
                        var auditJavascript = new JavaScriptSerializer();
                        var StoreAudit = auditJavascript.Serialize("Id: " + users.Id + ", Password: " + user.NewPassword + ", OldPassword: " + user.Password);
                        _auditService.InsertAudit(HelperCls.GetIPAddress(), StoreAudit, Request.Url.OriginalString.ToString(), users.Id, null);
                    }
                    catch (Exception ex)
                    {
                        HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                        throw ex;
                    }
                    Session.Clear();
                    return RedirectToAction("Login", "Home");
                }
            }
            ViewBag.SamePassword = false;
            return View(users);
        }

        public async Task<ActionResult> ExternalLogin()
        {
            if (Request.IsAuthenticated)
            {
                string Email = ClaimsPrincipal.Current.FindFirst("preferred_username").Value;
                if (string.IsNullOrEmpty(Email))
                {
                    return RedirectToAction("LogOut", "Home");
                }
                ViewBag.UserName = Email;
                try
                {
                    var user = _userService.GetUsersByEmail(Email);
                    bool cond = user != null ? true : false;
                    if (cond)
                    {

                        try
                        {
                            HttpContext.Items.Add("User", user);
                            var request = Request;
                            ViewBag.Password = "True";
                            string SessionTime = _menuService.GetMaster("Session Time").Value;
                            UserSession userSession = new UserSession();
                            userSession.RoleId = user.RoleId != 0 ? user.RoleId : 1;
                            userSession.StoreId = 0;
                            userSession.UserId = user.Id;
                            userSession.UserName = Email;
                            userSession.PlanId = user.CustomerPlan.Id;
                            userSession.MkrChkrFlag = 0.ToString();
                            userSession.SessionTimeout = SessionTime;
                            userSession.EmailAddress = (user.EmailAddress != null ? user.EmailAddress : "");
                            userSession.StoreName = user.Name;

                            var customers = _customerService.customerLists(user.Id);
                            int FirstCustId = customers != null && customers.Count() > 0 ? customers.FirstOrDefault().Id : 0;
                            if (FirstCustId == 0)
                            {
                                return RedirectToAction("Customers", "Home");
                            }
                            userSession.CustomerId = FirstCustId;
                            var userData = new JavaScriptSerializer().Serialize(userSession);
                            Session["AuthUserData"] = user.RoleId + "|" + 0 + "|" + user.Id + "|" + FirstCustId + "|" + Email + "|" + user.CustomerPlan.Id + "|" + 0 + "|" + SessionTime + "|" + (user.EmailAddress != null ? user.EmailAddress : "") + "|" + 0 + "|" + user.Name;
                            var ticket = new FormsAuthenticationTicket(1, (user.Id.ToString() + user.Name), DateTime.Now, DateTime.Now.AddMinutes(Convert.ToDouble(SessionTime)), true, userData);
                            string encTicket = FormsAuthentication.Encrypt(ticket);
                            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                        }
                        catch (Exception ex)
                        {
                            HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                            throw ex;
                        }
                        return RedirectToAction("Customers", "Home");
                    }
                    return RedirectToAction("LogOut", "Home");
                }
                catch (Exception ex)
                {
                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                    return RedirectToAction("LogOut", "Home");
                }
            }
            else
                return RedirectToAction("Login", "Home");
        }


        public void SignIn()
        {
            //if (!Request.IsAuthenticated)
            //{
            HttpContext.GetOwinContext().Authentication.Challenge(properties: new AuthenticationProperties { RedirectUri = Url.Action("ExternalLogin", "Home", null, Request.Url.Scheme) }, authenticationTypes: OpenIdConnectAuthenticationDefaults.AuthenticationType);
            //}
        }

    }
}