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
    public class MenuController : Controller
    {

        private readonly IMenuService _menuService;
        private readonly IAuditService _auditService;

        public MenuController(IMenuService menuService, IAuditService auditService)
        {
            _menuService = menuService;
            _auditService = auditService;
        }

        [AdminAuthorization]
        public ActionResult PlanManegementForStore()
        {
            ViewBag.CustomerPlan = _menuService.GetRoles();
            return View();
        }

        [AdminAuthorization]
        public ActionResult PlanManegementForDcUser()
        {
            ViewBag.CustomerPlan = _menuService.GetRoles();

            return View();
        }

        [AdminAuthorization]
        public ActionResult Management(int Roleid, int custplanid)
        {
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
                        var child2 = _menuService.GetChildMenu(chi1.Id,Roleid);
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
            return View();
        }

        [AdminApiAuthorization]
        [HttpPost]
        public JsonResult SaveMenuManagement(List<Menu> menus, int RoleId)
        {
            int custplanid = menus.FirstOrDefault().custplanid;
            int afeectedrow = _menuService.DeletePermission(RoleId, custplanid);
            try
            {
                var auditJavascript = new JavaScriptSerializer();
                var LocationAudit = auditJavascript.Serialize("RoleId: " + RoleId + ", CustomerPlanId" + custplanid);
                _auditService.InsertAudit(HelperCls.GetIPAddress(), LocationAudit, Request.Url.OriginalString.ToString(), SessionValues.UserId, null);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            foreach (Menu menu in menus)
            {
                Plan plan = new Plan();
                //plan.CustomerPlan = new CustomerPlan();
                plan.CustomerPlan = _menuService.CustomerPlan(menu.custplanid);
                //plan.MenuItem = new MenuItem();
                plan.MenuItem = _menuService.MenuItem(menu.Id);
                plan.IsShow = true;
                int afec = menu.PlanId != null ? _menuService.Insertmenu(plan) : 0;
                try
                {
                    Plan planAudit = new Plan();
                    planAudit.Id = plan.Id;
                    planAudit.CustomerPlanID = plan.CustomerPlan.Id;
                    planAudit.MenuItemID = plan.MenuItem.Id;
                    planAudit.StoreID = plan.StoreID;
                    planAudit.IsShow = plan.IsShow;
                    var auditJavascript = new JavaScriptSerializer();
                    var MenuAudit = auditJavascript.Serialize(planAudit);
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

        [AdminApiAuthorization]
        [HttpPost]
        public JsonResult SaveWidget(List<Menu> menus, int RoleId)
        {
            int custplanid = menus.FirstOrDefault().custplanid;
            int afeectedrow = _menuService.DeleteWidgetMaping(custplanid, RoleId);
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
                //plan.CustomerPlan = new CustomerPlan();
                map.CustomerPlan = _menuService.CustomerPlan(menu.custplanid);
                //plan.MenuItem = new MenuItem();
                map.DashBoardPer = _menuService.DashBoardPerbyid(menu.Id);
                map.IsShow = true;
                int afec = menu.PlanId != null ? _menuService.InserWidget(map) : 0;
                try
                {
                    var mapAudit = new DashBoardMapping(map.Id, map.CustomerPlan.Id, map.Store != null ? map.Store.Id : (int?)null, map.Store != null ? map.Users.Id : (int?)null, map.DashBoardPer != null ? map.DashBoardPer.Id : (int?)null, map.IsShow);
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
        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }
    }
}