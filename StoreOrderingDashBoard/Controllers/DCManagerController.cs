using BAL;
using DAL;
using DbLayer.Service;
using StoreOrderingDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace StoreOrderingDashBoard.Controllers
{
    [AdminAuthorization]
    public class DCManagerController : Controller
    {
        //tested SVN Tested
        private readonly ICustomerService _customerService;
        private readonly ILocationService _locationService;
        private readonly IUserService _userService;
        private readonly IMenuService _menuService;
        private readonly IStoreService _storeService;
        private readonly IAuditService _auditService;

        public DCManagerController(ICustomerService customerService, ILocationService locationService, IUserService userService, IMenuService menuService, IStoreService storeService, IAuditService auditService)
        {
            _customerService = customerService;
            _locationService = locationService;
            _userService = userService;
            _menuService = menuService;
            _storeService = storeService;
            _auditService = auditService;
        }

        public ActionResult AddDcManager()
        {
            ViewBag.Companies = _customerService.GetCompanies();
            ViewBag.CustomerPlan = _customerService.GetCustomerPlans();
            return View();
        }
        // GET: DCManager
        public ActionResult Index()
        {
            ViewBag.AdminUsers = _userService.GetAdminUsers();
            return View();
        }
        //      [AdminAuthorization]
        public ActionResult AddAdminUser()
        {
            List<Locations> locations = _locationService.GetAllLocations();
            List<Company> companies = _customerService.GetCompanies();
            List<CustomerPlan> plans = _customerService.GetCustomerPlans();
            List<String> Departments = _userService.GetDepartments();
            List<AllQuestion> allQuestions = _customerService.allQuestions();
            List<Customer> customers = _customerService.GetAllCustomers();
            //List<Role> Roles = _userService.GetRoles();
            Users users = new Users();            
            users.RoleId = 2;
            List<SelectListItem> SelectLocations = locations.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.Name,
                    Value = a.LocationId.ToString(),
                    Selected = false
                };
            });
            List<SelectListItem> SelectCustomers = customers.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.Name,
                    Value = a.Id.ToString(),
                    Selected = false
                };
            });
            ViewBag.Locations = SelectLocations;
            ViewBag.Customers = SelectCustomers;
            ViewBag.Companies = companies;
            ViewBag.Plans = plans;
            ViewBag.Departments = Departments;
            ViewBag.Questions = allQuestions;
            users.AllQuestions = allQuestions;
            //users.
            users.Customers = customers;
            //ViewBag.Roles = Roles;
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAdminUser(Users user)
        {
            //var JavaScriptSerializer = new JavaScriptSerializer();
            //List<JsonStore> stores = JavaScriptSerializer.Deserialize<List<JsonStore>>(user.Stored);
            if (ModelState.IsValid)
            {
                var Users = _userService.GetUsersByEmail(user.EmailAddress);
                if (Users != null)
                {
                    List<Locations> locations = _locationService.GetAllLocations();
                    List<Company> companies = _customerService.GetCompanies();
                    List<CustomerPlan> plans = _customerService.GetCustomerPlans();
                    List<String> Departments = _userService.GetDepartments();
                    List<AllQuestion> allQuestions = _customerService.allQuestions();
                    List<Customer> customers = _customerService.GetAllCustomers();
                    user.RoleId = 2;

                    List<SelectListItem> SelectLocations = locations.ConvertAll(a =>
                    {
                        return new SelectListItem()
                        {
                            Text = a.Name,
                            Value = a.LocationId.ToString(),
                            Selected = user.Locations.LocationId == a.LocationId ? true : false
                        };
                    });
                    List<SelectListItem> SelectCustomers = customers.ConvertAll(a =>
                    {
                        return new SelectListItem()
                        {
                            Text = a.Name,
                            Value = a.Id.ToString(),
                            Selected = user.Customer.Contains(a.Id) ? true : false
                        };
                    });
                    ViewBag.Locations = SelectLocations;
                    ViewBag.Companies = companies;
                    ViewBag.Plans = plans;
                    ViewBag.Departments = Departments;
                    ViewBag.Questions = allQuestions;
                    ViewBag.Customers = SelectCustomers;
                    user.AllQuestions = allQuestions;
                    user.Customers = customers;
                    ModelState.AddModelError("EmailAddress", "Email Address is Already Exist");
                    return View(user);
                }
                else
                {
                    user.IsDeleted = false;
                    user.CreatedOn = DateTime.Now;
                    user.ReportToId = 1;
                    user.RoleId = 2;
                    user.CreatedBy = SessionValues.UserId;
                    var userid = _userService.AddAdminUser(user);
                    foreach (AllQuestion allQuestion in user.AllQuestions)
                    {
                        Answer answer = new Answer();
                        answer.Id = allQuestion.Id;
                        answer.Answers = allQuestion.answer;
                        answer.Users = new Users();
                        answer.Users.Id = userid;
                        _userService.InsertAnswer(answer);
                        try
                        {
                            var auditJavascript = new JavaScriptSerializer();
                            var answerAudit = auditJavascript.Serialize(answer);
                            _auditService.InsertAudit(HelperCls.GetIPAddress(), answerAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                        }
                        catch (Exception ex)
                        {
                            HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                            throw ex;
                        }
                    }

                    if (user.Stored != null && user.Stored != "")
                    {
                        var JavaScriptSerializer = new JavaScriptSerializer();
                        List<JsonStore> stores = JavaScriptSerializer.Deserialize<List<JsonStore>>(user.Stored);
                        string[] store = user.Stored.Split(',');
                        foreach (JsonStore jsonstore in stores)
                        {
                            PermitableCls permitable = new PermitableCls();
                            permitable.Users = new Users();
                            permitable.CreatedBy = (int)user.CreatedBy;
                            //permitable.CreatedOn=
                            permitable.Users.Id = userid;
                            permitable.Customer = new Customer();
                            permitable.StoreId = Convert.ToInt32(jsonstore.StoreId);
                            _userService.InsertPermitable(permitable);
                            try
                            {
                                var auditJavascript = new JavaScriptSerializer();
                                var permitableAudit = auditJavascript.Serialize(permitable);
                                _auditService.InsertAudit(HelperCls.GetIPAddress(), permitableAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                            }
                            catch (Exception ex)
                            {
                                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                                throw ex;
                            }
                        }
                        foreach (int customer in user.Customer)
                        {
                            if (!(stores.Any(x => x.CustId == customer)))
                            {
                                PermitableCls permitable = new PermitableCls();
                                permitable.CreatedBy = (int)user.CreatedBy;
                                //permitable.CreatedOn=
                                permitable.Users = new Users();
                                permitable.Users.Id = userid;
                                permitable.Customer = new Customer();
                                permitable.Customer.Id = customer;
                                _userService.InsertPermitable(permitable);
                                try
                                {
                                    var auditJavascript = new JavaScriptSerializer();
                                    var permitableAudit = auditJavascript.Serialize(permitable);
                                    _auditService.InsertAudit(HelperCls.GetIPAddress(), permitableAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                                }
                                catch (Exception ex)
                                {
                                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                                    throw ex;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (int customer in user.Customer)
                        {
                            PermitableCls permitable = new PermitableCls();
                            permitable.CreatedBy = (int)user.CreatedBy;
                            //permitable.CreatedOn=
                            permitable.Users = new Users();
                            permitable.Users.Id = userid;
                            permitable.Customer = new Customer();
                            permitable.Customer.Id = customer;
                            _userService.InsertPermitable(permitable);
                            try
                            {
                                var auditJavascript = new JavaScriptSerializer();
                                var permitableAudit = auditJavascript.Serialize(permitable);
                                _auditService.InsertAudit(HelperCls.GetIPAddress(), permitableAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                            }
                            catch (Exception ex)
                            {
                                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                                throw ex;
                            }
                        }
                    }
                    try
                    {
                        var auditJavascript = new JavaScriptSerializer();
                        var UserAudit = auditJavascript.Serialize(user);
                        _auditService.InsertAudit(HelperCls.GetIPAddress(), UserAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                    }
                    catch (Exception ex)
                    {
                        HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                        throw ex;
                    }
                    return RedirectToAction("Index", "DcManager");
                }
            }
            else
            {
                List<Locations> locations = _locationService.GetAllLocations();
                List<Company> companies = _customerService.GetCompanies();
                List<CustomerPlan> plans = _customerService.GetCustomerPlans();
                List<String> Departments = _userService.GetDepartments();
                List<AllQuestion> allQuestions = _customerService.allQuestions();
                List<Customer> customers = _customerService.GetAllCustomers();
                //List<Role> Roles = _userService.GetRoles();
                user.RoleId = 2;
                List<SelectListItem> SelectLocations = locations.ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = a.Name,
                        Value = a.LocationId.ToString(),
                        Selected = user.Locations.LocationId == a.LocationId ? true : false
                    };
                });
                List<SelectListItem> SelectCustomers = customers.ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = a.Name,
                        Value = a.Id.ToString(),
                        Selected = user.Customer.Contains(a.Id) ? true : false
                    };
                });
                ViewBag.Locations = SelectLocations;
                ViewBag.Customers = SelectCustomers;
                ViewBag.Companies = companies;
                ViewBag.Plans = plans;
                ViewBag.Departments = Departments;
                ViewBag.Questions = allQuestions;
                user.AllQuestions = allQuestions;
                user.Customers = customers;
                //ViewBag.Roles = Roles;
                return View(user);
            }
        }

        public ActionResult MenuRights(int custplanid)
        {
            int Roleid = 2;
            var parent = _menuService.GetParentMenus(Roleid);
            List<Menu> menulist = new List<Menu>();
            foreach (MenuItem par in parent)
            {
                Menu menu = new Menu();
                // menu.IsShow =( _menuService.GetPlan(custplanid, par.Id)!=null?true:false);
                var plan = _menuService.GetPlan(custplanid, par.Id);
                menu.PlanId = plan != null ? plan.Id : 0;
                menu.MenuName = par.MenuName;
                menu.Id = par.Id;
                menulist.Add(menu);
                var child = _menuService.GetChildMenu(par.Id,Roleid);
                foreach (MenuItem chi in child)
                {
                    Menu menu1 = new Menu();
                    menu1.MenuName = menu.MenuName + "=>" + chi.MenuName;
                    //menu1.IsShow= (_menuService.GetPlan(custplanid, chi.Id) != null ? true : false);
                    var plan1 = _menuService.GetPlan(custplanid, chi.Id);
                    menu1.PlanId = plan1 != null ? plan1.Id : 0;
                    menu1.Id = chi.Id;
                    menulist.Add(menu1);
                    var child1 = _menuService.GetChildMenu(chi.Id, Roleid);
                    foreach (MenuItem chi1 in child1)
                    {
                        Menu menu2 = new Menu();
                        menu2.MenuName = chi.MenuName + "=>" + chi1.MenuName;
                        //menu2.IsShow = (_menuService.GetPlan(custplanid, chi1.Id) != null ? true : false);
                        var plan2 = _menuService.GetPlan(custplanid, chi1.Id);
                        menu2.PlanId = plan2 != null ? plan2.Id : 0;
                        menu2.Id = chi1.Id;
                        menulist.Add(menu2);
                        var child2 = _menuService.GetChildMenu(chi1.Id, Roleid);
                        foreach (MenuItem chi2 in child2)
                        {
                            Menu menu3 = new Menu();
                            menu3.MenuName = chi1.MenuName + "=>" + chi2.MenuName;
                            // menu3.IsShow = (_menuService.GetPlan(custplanid, chi2.Id) != null ? true : false);
                            var plan3 = _menuService.GetPlan(custplanid, chi2.Id);
                            menu3.PlanId = plan3 != null ? plan3.Id : 0;
                            menu3.Id = chi2.Id;
                            menulist.Add(menu3);
                        }
                    }
                }
                ViewBag.Itemlist = menulist;
            }
            ViewBag.Menu = menulist;
            //var dashboard = _menuService.DashBoardPer(Roleid);
            //List<Menu> DashBoardlist = new List<Menu>();
            //foreach (DashBoardPer per in dashboard)
            //{
            //    Menu m = new Menu();
            //    m.Id = per.Id;
            //    m.MenuName = per.Name;
            //    var plan = _menuService.DashBoardMapping(custplanid, per.Id);
            //    m.PlanId = plan != null ? plan.Id : 0;
            //    DashBoardlist.Add(m);
            //}
            //ViewBag.Dash = DashBoardlist;
            //return View();
            return PartialView("_MenuRights");
        }

        public ActionResult CustomerStores(string CustId)
        {
            List<int> custIds = CustId.Split(',').Select(int.Parse).ToList();
            // int Roleid = 2;
            List<Store> Stores = _storeService.GetAllStores();
            var AllowedStoreList = from Store in Stores
                                   where custIds.Contains(Store.CustId)
                                   select Store;
            ViewBag.AllowedStoreList = AllowedStoreList.ToList();
            return PartialView("_CustomerStores");
        }

        public ActionResult MenuRightsForUser(int UserId, int custplanid)
        {
            int Roleid = 2;
            var parent = _menuService.GetParentMenus(Roleid);
            ViewData["AdminID"] = UserId;
            ViewData["CustPlanId"] = custplanid;
            List<Menu> menulist = new List<Menu>();
            foreach (MenuItem par in parent)
            {
                Menu menu = new Menu();
                // menu.IsShow =( _menuService.GetPlan(custplanid, par.Id)!=null?true:false);
                var plan = _menuService.IsUserMenu(custplanid, par.Id, UserId);
                menu.PlanId = plan > 0 ? plan : 0;
                menu.MenuName = par.MenuName;
                menu.Id = par.Id;
                menulist.Add(menu);
                var child = _menuService.GetChildMenu(par.Id, Roleid);
                foreach (MenuItem chi in child)
                {
                    Menu menu1 = new Menu();
                    menu1.MenuName = menu.MenuName + "=>" + chi.MenuName;
                    //menu1.IsShow= (_menuService.GetPlan(custplanid, chi.Id) != null ? true : false);
                    var plan1 = _menuService.IsUserMenu(custplanid, chi.Id, UserId);
                    menu1.PlanId = plan1 > 0 ? plan1 : 0;
                    menu1.Id = chi.Id;
                    menulist.Add(menu1);
                    var child1 = _menuService.GetChildMenu(chi.Id, Roleid);
                    foreach (MenuItem chi1 in child1)
                    {
                        Menu menu2 = new Menu();
                        menu2.MenuName = chi.MenuName + "=>" + chi1.MenuName;
                        //menu2.IsShow = (_menuService.GetPlan(custplanid, chi1.Id) != null ? true : false);
                        var plan2 = _menuService.IsUserMenu(custplanid, chi1.Id, UserId);
                        menu2.PlanId = plan2 > 0 ? plan2 : 0;
                        menu2.Id = chi1.Id;
                        menulist.Add(menu2);
                        var child2 = _menuService.GetChildMenu(chi1.Id, Roleid);
                        foreach (MenuItem chi2 in child2)
                        {
                            Menu menu3 = new Menu();
                            menu3.MenuName = chi1.MenuName + "=>" + chi2.MenuName;
                            // menu3.IsShow = (_menuService.GetPlan(custplanid, chi2.Id) != null ? true : false);
                            var plan3 = _menuService.IsUserMenu(custplanid, chi2.Id, UserId);
                            menu3.PlanId = plan3 > 0 ? plan3 : 0;
                            menu3.Id = chi2.Id;
                            menulist.Add(menu3);
                        }
                    }
                }

            }
            ViewBag.Menu = menulist;

            return PartialView("_MenuRights");
        }

        [AdminApiAuthorization]
        [HttpPost]
        public JsonResult SaveMenuManagement(List<Menu> menus, int UserId, int CustPlanId)
        {
            int afeectedrow = _menuService.DeleteUserPermission(UserId);
            try
            {
                var auditJavascript = new JavaScriptSerializer();
                var LocationAudit = auditJavascript.Serialize("UserId: " + UserId);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), LocationAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            foreach (Menu menu in menus)
            {
                var custplan = _menuService.plans(CustPlanId);
                if (custplan.Where(x => x.MenuItem.Id == menu.Id).ToList().Count() > 0)
                {
                    Plan plans = new Plan();
                    plans.MenuItemID = menu.Id;
                    if (!menu.IsShow)
                    {
                        plans.IsShow = menu.IsShow;
                        plans.UserID = UserId;
                        var affectedrow = _menuService.InsertAdminPlan(plans);
                        try
                        {

                            var auditJavascript = new JavaScriptSerializer();
                            var MenuAudit = auditJavascript.Serialize(plans);
                            _auditService.InsertAudit(HelperCls.GetIPAddress(), MenuAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                        }
                        catch (Exception ex)
                        {
                            HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                            throw ex;
                        }
                    }

                }
                else
                {
                    Plan plans = new Plan();
                    plans.MenuItemID = menu.Id;
                    if (menu.IsShow)
                    {
                        plans.IsShow = menu.IsShow;
                        plans.UserID = UserId;
                        var affectedrow = _menuService.InsertAdminPlan(plans);
                        try
                        {

                            var auditJavascript = new JavaScriptSerializer();
                            var MenuAudit = auditJavascript.Serialize(plans);
                            _auditService.InsertAudit(HelperCls.GetIPAddress(), MenuAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                        }
                        catch (Exception ex)
                        {
                            HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                            throw ex;
                        }
                    }
                }

            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditAdminUser(int UserId)
        {
            Users users = _userService.GetAdminUserByUserId(UserId);
            //Locations SelectedLocation = _locationService.GetLocationForUserId(users.Id);
            List<Locations> locations = _locationService.GetAllLocations();
            List<Company> companies = _customerService.GetCompanies();
            List<CustomerPlan> plans = _customerService.GetCustomerPlans();
            List<String> Departments = _userService.GetDepartments();
            List<AllQuestion> allQuestions = _customerService.allQuestions();
            List<Answer> answers = _customerService.GetquestionAnswersForUser(users.Id);
            List<Customer> customers = _customerService.GetAllCustomers();
            List<ViewPermitableCls> permitables = _userService.VGetPermitablesForUserId(users.Id);
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            List<JsonStore> jsonStores = new List<JsonStore>();
            if (permitables.Any(x => x.Store_Id != null))
            {
                foreach (ViewPermitableCls viewPermitable in permitables)
                {
                    if (viewPermitable.Store_Id != null)
                    {
                        JsonStore jsonStore = new JsonStore();
                        jsonStore.CustId = viewPermitable.Customer_Id;
                        jsonStore.UserId = viewPermitable.Users_Id;
                        jsonStore.StoreId = viewPermitable.Store_Id;
                        jsonStores.Add(jsonStore);
                    }
                }
                users.Stored = javaScriptSerializer.Serialize(jsonStores);
            }
            users.Customer = permitables.Select(x => x.Customer_Id).Distinct().ToArray();
            //List<Role> Roles = _userService.GetRoles();            
            //users.RoleId = 2;
            //List<PermitableCls> permissions = _userService.GetPermitableClsByUserId(users.Id);
            //List<Customer> AllowedCustomers = _customerService.GetCustomerByUserId(users.Id);
            List<SelectListItem> SelectLocations = locations.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.Name,
                    Value = a.LocationId.ToString(),
                    Selected = false
                };
            });

            foreach (AllQuestion question in allQuestions)
            {
                if (answers.Any(X => X.Question.Id == question.Id))
                {
                    question.answer = answers.Where(x => x.Question.Id == question.Id).FirstOrDefault().Answers;
                }
            }
            List<SelectListItem> SelectCustomers = customers.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.Name,
                    Value = a.Id.ToString(),
                    Selected = false
                };
            });
            ViewBag.Locations = SelectLocations;
            ViewBag.Companies = companies;
            ViewBag.Plans = plans;
            ViewBag.Departments = Departments;
            ViewBag.Questions = allQuestions;
            users.AllQuestions = allQuestions;
            users.Customers = customers;
            ViewBag.Customers = SelectCustomers;
            users.Customers = customers;
            //ViewBag.Roles = Roles;
            return View(users);
        }
        [HttpPost]
        public ActionResult EditAdminUser(Users user)
        {
            if (ModelState.IsValid)
            {
                var Users = _userService.GetUsersByEmail(user.EmailAddress);
                if (Users != null && Users.Id != user.Id)
                {
                    List<Locations> locations = _locationService.GetAllLocations();
                    List<Company> companies = _customerService.GetCompanies();
                    List<CustomerPlan> plans = _customerService.GetCustomerPlans();
                    List<String> Departments = _userService.GetDepartments();
                    List<AllQuestion> allQuestions = _customerService.allQuestions();
                    List<Customer> customers = _customerService.GetAllCustomers();
                    user.RoleId = 2;

                    List<SelectListItem> SelectLocations = locations.ConvertAll(a =>
                    {
                        return new SelectListItem()
                        {
                            Text = a.Name,
                            Value = a.LocationId.ToString(),
                            Selected = user.Locations.LocationId == a.LocationId ? true : false
                        };
                    });
                    List<SelectListItem> SelectCustomers = customers.ConvertAll(a =>
                    {
                        return new SelectListItem()
                        {
                            Text = a.Name,
                            Value = a.Id.ToString(),
                            Selected = user.Customer.Contains(a.Id) ? true : false
                        };
                    });
                    ViewBag.Locations = SelectLocations;
                    ViewBag.Companies = companies;
                    ViewBag.Plans = plans;
                    ViewBag.Departments = Departments;
                    ViewBag.Questions = allQuestions;
                    ViewBag.Customers = SelectCustomers;
                    user.AllQuestions = allQuestions;
                    user.Customers = customers;
                    ModelState.AddModelError("EmailAddress", "Email Address is Already Exist");
                    return View(user);
                }
                else
                {
                    user.IsDeleted = false;
                    user.CreatedOn = DateTime.Now;
                    user.ReportToId = 1;
                    user.RoleId = 2;
                    user.ModifiedBy = SessionValues.UserId;
                    var userid = _userService.EditAdminUser(user);
                    foreach (AllQuestion allQuestion in user.AllQuestions)
                    {
                        Answer answer = new Answer();
                        answer.Id = allQuestion.Id;
                        answer.Answers = allQuestion.answer;
                        answer.Users = new Users();
                        answer.Users.Id = userid;
                        _userService.UpdateORInsertAnswer(answer);
                        try
                        {
                            var answerJavascript = new JavaScriptSerializer();
                            var answerAudit = answerJavascript.Serialize(answer);
                            _auditService.InsertAudit(HelperCls.GetIPAddress(), answerAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                        }
                        catch (Exception ex)
                        {
                            HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                            throw ex;
                        }
                    }

                    List<ViewPermitableCls> permitables = _userService.VGetPermitablesForUserId(user.Id);
                    var auditJavascript = new JavaScriptSerializer();
                    var permitablesAudit = auditJavascript.Serialize(permitables);
                    _auditService.InsertAudit(HelperCls.GetIPAddress(), permitablesAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                    _userService.DeletePermissionsforUser(user.Id);
                    if (user.Stored != null && user.Stored != "")
                    {
                        var JavaScriptSerializer = new JavaScriptSerializer();
                        List<JsonStore> stores = JavaScriptSerializer.Deserialize<List<JsonStore>>(user.Stored);
                        string[] store = user.Stored.Split(',');
                        foreach (JsonStore jsonstore in stores)
                        {
                            PermitableCls permitable = new PermitableCls();
                            permitable.Users = new Users();
                            permitable.CreatedBy = SessionValues.UserId;
                            //permitable.CreatedOn=
                            permitable.Users.Id = userid;
                            permitable.Customer = new Customer();
                            permitable.StoreId = Convert.ToInt32(jsonstore.StoreId);
                            _userService.InsertPermitable(permitable);
                            try
                            {
                                var permitableJavascript = new JavaScriptSerializer();
                                var permitableAudit = permitableJavascript.Serialize(permitable);
                                _auditService.InsertAudit(HelperCls.GetIPAddress(), permitableAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                            }
                            catch (Exception ex)
                            {
                                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                                throw ex;
                            }
                        }
                        foreach (int customer in user.Customer)
                        {
                            if (!(stores.Any(x => x.CustId == customer)))
                            {
                                PermitableCls permitable = new PermitableCls();
                                permitable.CreatedBy = SessionValues.UserId;
                                //permitable.CreatedOn=
                                permitable.Users = new Users();
                                permitable.Users.Id = userid;
                                permitable.Customer = new Customer();
                                permitable.Customer.Id = customer;
                                _userService.InsertPermitable(permitable);
                                try
                                {
                                    var permitableJavascript = new JavaScriptSerializer();
                                    var permitableAudit = permitableJavascript.Serialize(permitable);
                                    _auditService.InsertAudit(HelperCls.GetIPAddress(), permitableAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                                }
                                catch (Exception ex)
                                {
                                    HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                                    throw ex;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (int customer in user.Customer)
                        {
                            PermitableCls permitable = new PermitableCls();
                            permitable.CreatedBy = SessionValues.UserId;
                            //permitable.CreatedOn=
                            permitable.Users = new Users();
                            permitable.Users.Id = userid;
                            permitable.Customer = new Customer();
                            permitable.Customer.Id = customer;
                            _userService.InsertPermitable(permitable);
                            try
                            {
                                var permitableJavascript = new JavaScriptSerializer();
                                var permitableAudit = permitableJavascript.Serialize(permitable);
                                _auditService.InsertAudit(HelperCls.GetIPAddress(), permitableAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                            }
                            catch (Exception ex)
                            {
                                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                                throw ex;
                            }
                        }
                    }
                    try
                    {
                        var userJavascript = new JavaScriptSerializer();
                        var userAudit = userJavascript.Serialize(user);
                        _auditService.InsertAudit(HelperCls.GetIPAddress(), userAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
                    }
                    catch (Exception ex)
                    {
                        HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                        throw ex;
                    }
                    return RedirectToAction("Index", "DcManager");
                }
            }
            else
            {
                List<Locations> locations = _locationService.GetAllLocations();
                List<Company> companies = _customerService.GetCompanies();
                List<CustomerPlan> plans = _customerService.GetCustomerPlans();
                List<String> Departments = _userService.GetDepartments();
                List<AllQuestion> allQuestions = _customerService.allQuestions();
                List<Customer> customers = _customerService.GetAllCustomers();
                //List<Role> Roles = _userService.GetRoles();
                user.RoleId = 2;
                List<SelectListItem> SelectLocations = locations.ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = a.Name,
                        Value = a.LocationId.ToString(),
                        Selected = user.Locations.LocationId == a.LocationId ? true : false
                    };
                });
                List<SelectListItem> SelectCustomers = customers.ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = a.Name,
                        Value = a.Id.ToString(),
                        Selected = user.Customer.Contains(a.Id) ? true : false
                    };
                });
                ViewBag.Locations = SelectLocations;
                ViewBag.Customers = SelectCustomers;
                ViewBag.Companies = companies;
                ViewBag.Plans = plans;
                ViewBag.Departments = Departments;
                ViewBag.Questions = allQuestions;
                user.AllQuestions = allQuestions;
                user.Customers = customers;
                //ViewBag.Roles = Roles;
                return View(user);
            }
        }
        [HttpPost]
        public ActionResult DeleteAdminUser(int UserId)
        {
            bool cond = _userService.DeleteAdminUser(UserId) > 0 ? true : false;
            try
            {
                var auditJavascript = new JavaScriptSerializer();
                var WareHouseAudit = auditJavascript.Serialize("AdminUser : " + UserId + ", IsDeleted:" + cond);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), WareHouseAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            return Json(cond, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorization]
        public ActionResult WidgetsForUser(int UserId, int custplanid)
        {
            int Roleid = 2;
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
                var plan = _menuService.DashBoardMapping(custplanid, per.Id);
                m.PlanId = plan != null ? plan.Id : 0;
                m.IsShow = _menuService.IsUserDashboard(per.Id, custplanid, UserId);
                DashBoardlist.Add(m);
            }
            ViewBag.Dash = DashBoardlist;
            return PartialView("_WidgetRights");
        }

        [AdminApiAuthorization]
        [HttpPost]
        public JsonResult SaveWidgetForUser(List<Menu> menus, int CustPlanId, int UserId)
        {
            int RoleId = 2;
            int custplanid = menus.FirstOrDefault().custplanid;
            int afeectedrow = _menuService.DeleteWidgetMapingForUser(custplanid, RoleId, UserId);
            try
            {
                var auditJavascript = new JavaScriptSerializer();
                var MenuAudit = auditJavascript.Serialize("Delete WidgetMaping for DcUser UserId: " + UserId + ", CustPlanId: " + custplanid + ", RoleId: " + RoleId);
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
                //plan.CustomerPlan = new CustomerPlan();
                //map.CustomerPlan = _menuService.CustomerPlan(menu.custplanid);
                //plan.MenuItem = new MenuItem();
                map.DashBoardPer = _menuService.DashBoardPerbyid(menu.Id);
                map.Users = _userService.GetAdminUserByUserId(UserId);
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
    }
}