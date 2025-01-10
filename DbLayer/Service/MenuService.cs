using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DbLayer.Repository;

namespace DbLayer.Service
{
    public interface IMenuService
    {
        List<Menu> menuItems(int roleid, int planid, int userid);
        List<CustomerPlan> GetRoles();
        List<MenuItem> GetParentMenus(int Roleid);
        Plan GetPlan(int roleid, int menuid);
        List<MenuItem> GetChildMenu(int Parentid, int roleid);
        int DeletePermission(int roleid, int cusplanid);
        int Insertmenu(Plan plan);
        MenuItem MenuItem(int id);
        CustomerPlan CustomerPlan(int id);
        List<DashBoardPer> DashBoardPer(int roleid);
        DashBoardMapping DashBoardMapping(int custplanid, int dashid);
        int DeleteWidgetMaping(int Custplanid, int Roleid);
        int InserWidget(DashBoardMapping dashBoardMapping);
        DashBoardPer DashBoardPerbyid(int id);
        List<Menu> ParentmenuStores(int roleid, int planid, int userid, int storeid);
        List<Menu> ChildmenuStores(int id, int roleid, int userid, int CustPlanid);
        // List<Menu> ChildItem(int id, int roleid, int userid);
        int DeleteStorePlan(int storeid);
        Menu GetMenuForStore(int MenuId, int planid, int storeid);
        Store GetStore(int id);
        Menu CheckForStoreSave(int MenuId, int planid);
        int IsUserMenu(int CustPlanid, int MenuItemId, int UserId);
        bool IsUserDashboard(int id, int CustomerPlanId, int userId);
        int DeleteWidgetMapingForUser(int custplanid, int roleId, int storeUserId);
        int InsertWidgetForUser(DashBoardMapping map);
        int DeleteUserPermission(int UserId);
        List<Plan> plans(int Custplanid);
        int InsertAdminPlan(Plan plan);
        Menu GetMenu(string ActionName, string ControlName, int RoleId, int UserId, int CustomerPlanId);
        Master GetMaster(string Text);
    }
    public class MenuService:IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        public MenuService(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        //public List<Menu> ChildItem(int id, int roleid, int userid)
        //{
        //    return _menuRepository.ChildItem(id,roleid,userid);
        //}

        public List<Menu> menuItems(int roleid, int planid, int userid)
        {
            return _menuRepository.menuItems(roleid,planid,userid);
        }


        public List<CustomerPlan> GetRoles()
        {
            return _menuRepository.GetRoles();
        }

        public List<MenuItem> GetParentMenus(int Roleid)
        {
            return _menuRepository.GetParentMenus(Roleid);
        }

        public Plan GetPlan(int roleid, int menuid)
        {
            return _menuRepository.GetPlan(roleid, menuid);
        }

        public List<MenuItem> GetChildMenu(int Parentid, int roleid)
        {
            return _menuRepository.GetChildMenu(Parentid, roleid);
        }

        public int DeletePermission(int roleid, int cusplanid)
        {
            return _menuRepository.DeletePermission(roleid, cusplanid);
        }

        public int Insertmenu(Plan plan)
        {
            return _menuRepository.Insertmenu(plan);
        }

        public MenuItem MenuItem(int id)
        {
            return _menuRepository.MenuItem(id);
        }

        public CustomerPlan CustomerPlan(int id)
        {
            return _menuRepository.CustomerPlan(id);
        }

        public List<DashBoardPer> DashBoardPer(int roleid)
        {
            return _menuRepository.DashBoardPer(roleid);
        }

        public DashBoardMapping DashBoardMapping(int custplanid, int dashid)
        {
            return _menuRepository.DashBoardMapping(custplanid,dashid);
        }

        public int DeleteWidgetMaping(int Custplanid, int Roleid)
        {
            return _menuRepository.DeleteWidgetMaping(Custplanid,Roleid);
        }

        public int InserWidget(DashBoardMapping dashBoardMapping)
        {
            return _menuRepository.InserWidget(dashBoardMapping);
        }

        public DashBoardPer DashBoardPerbyid(int id)
        {
            return _menuRepository.DashBoardPerbyid(id);
        }

        public List<Menu> ParentmenuStores(int roleid, int planid, int userid, int storeid)
        {
            return _menuRepository.ParentmenuStore(roleid,planid,userid,storeid);
        }

        public List<Menu> ChildmenuStores(int id, int roleid, int userid, int CustPlanid)
        {
            return _menuRepository.ChildmenuStores(id,roleid,userid,CustPlanid);
        }

        public int DeleteStorePlan(int storeid)
        {
            return _menuRepository.DeleteStorePlan(storeid);
        }

        public Menu GetMenuForStore(int MenuId, int planid, int storeid)
        {
            return _menuRepository.GetMenuForStore(MenuId, planid,storeid);
        }

        public Store GetStore(int id)
        {
            return _menuRepository.GetStore(id);
        }

        public Menu CheckForStoreSave(int MenuId, int planid)
        {
            return _menuRepository.CheckForStoreSave(MenuId, planid);
        }

        public int IsUserMenu(int CustPlanid, int MenuItemId, int UserId)
        {
            return _menuRepository.IsUserMenu(CustPlanid, MenuItemId, UserId);
        }
        public bool IsUserDashboard(int DashboardId, int CustomerPlanId, int userId)
        {
            return _menuRepository.IsUserDashboard(DashboardId, CustomerPlanId, userId); ;
        }
        public int DeleteWidgetMapingForUser(int custplanid, int roleId, int storeUserId)
        {
            return _menuRepository.DeleteWidgetMapingForUser(custplanid, roleId, storeUserId);
        }
        public int InsertWidgetForUser(DashBoardMapping map)
        {
            return _menuRepository.InsertWidgetForUser(map);
        }
        public int DeleteUserPermission(int UserId)
        {
            return _menuRepository.DeleteUserPermission(UserId);
        }
        public List<Plan> plans(int Custplanid)
        {
            return _menuRepository.plans(Custplanid);
        }
        public int InsertAdminPlan(Plan plan)
        {
            return _menuRepository.InsertAdminPlan(plan);
        }

        public Menu GetMenu(string ActionName, string ControlName, int RoleId, int UserId, int CustomerPlanId)
        {
            return _menuRepository.GetMenu(ActionName, ControlName, RoleId, UserId, CustomerPlanId);
        }

        public Master GetMaster(string Text)
        {
            return _menuRepository.GetMaster(Text);
        }
    }
}
