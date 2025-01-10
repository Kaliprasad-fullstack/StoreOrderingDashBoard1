using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAL;
using DAL;

namespace DbLayer.Repository
{
    public interface IMenuRepository
    {
        List<Menu> menuItems(int roleid,int planid,int userid);
        List<Menu> ChildItem(int id, int roleid,int userid, int custplanid);
        List<Menu> ParentmenuStore(int roleid, int planid, int userid, int storeid);
        List<Menu> ChildmenuStore(int id, int roleid, int userid, int CustPlanid);
        List<CustomerPlan> GetRoles();
        List<MenuItem> GetParentMenus(int Roleid);
        List<MenuItem> GetChildMenu(int Parentid, int roleid);
        Plan GetPlan(int roleid, int menuid);
        int DeletePermission(int roleid, int cusplanid);
        int Insertmenu(Plan plan);
        MenuItem MenuItem(int id);
        CustomerPlan CustomerPlan(int id);
        List<DashBoardPer> DashBoardPer(int roleid);
        DashBoardMapping DashBoardMapping(int custplanid, int dashid);
        int DeleteWidgetMaping(int Custplanid, int Roleid);
        int InserWidget(DashBoardMapping dashBoardMapping);
        DashBoardPer DashBoardPerbyid(int id);
        List<Menu> GetWidget(int Roleid,int Custplanid,int Userid);
        List<Menu> GetWidgetForStore(int Roleid, int Custplanid, int Storeid);
        List<Menu> ParentmenuStores(int roleid, int planid, int userid, int storeid);
        List<Menu> ChildmenuStores(int id, int roleid, int userid, int CustPlanid);
        int DeleteStorePlan(int storeid);
        Menu GetMenuForStore(int MenuId, int planid, int storeid);
        Store GetStore(int id);
        Menu CheckForStoreSave(int MenuId, int planid);
        int DeleteStoreUserWidget(int roleid, int userid);
        int IsUserMenu(int CustPlanid, int MenuItemId, int UserId);
        bool IsUserDashboard(int dashboardId, int customerPlanId, int userId);
        int DeleteWidgetMapingForUser(int custplanid, int roleId, int storeUserId);
        int InsertWidgetForUser(DashBoardMapping map);
        int DeleteUserPermission(int UserId);
        List<Plan> plans(int Custplanid);
        int InsertAdminPlan(Plan plan);
        Menu GetMenu(string ActionName,string ControlName,int RoleId,int UserId,int CustomerPlanId);
        Master GetMaster(string Text);
    }

    public class MenuRepository:IMenuRepository
    {
        private readonly StoreContext _storeContext;
        public MenuRepository()
        {
           
        }
        public MenuRepository(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }

        public List<Menu> menuItems(int roleid, int planid, int userid)
        {
            try
            {
                StoreContext _Context = new StoreContext();               
                return _Context.Database.SqlQuery<Menu>("exec USP_ParentMenu @UserId,@PlanId,@RoleId",
                   new SqlParameter("@UserId", userid),
                   new SqlParameter("@PlanId", planid),
                   new SqlParameter("@RoleId", roleid)
                   ).ToList();
                //return item;
            }
            catch(Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<Menu> ChildItem(int id, int roleid,int userid,int custplanid)
        {
            try
            {
                StoreContext _Context = new StoreContext();             
                return _Context.Database.SqlQuery<Menu>("exec USP_ChildMenu @UserId,@Parentid,@RoleId,@CustPlanId",
                  new SqlParameter("@UserId", userid),
                  new SqlParameter("@Parentid", id),
                  new SqlParameter("@RoleId", roleid),
                  new SqlParameter("@CustPlanId", custplanid)
                  ).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<Menu> ParentmenuStores(int roleid, int planid, int userid, int storeid)
        {
            try
            {
             

                return _storeContext.Database.SqlQuery<Menu>("exec USP_ParentMenuStore @UserId,@PlanId,@RoleId,@StoreId",
                   new SqlParameter("@UserId", userid),
                   new SqlParameter("@PlanId", planid),
                   new SqlParameter("@RoleId", roleid),
                   new SqlParameter("@StoreId", storeid)
                   ).ToList();
                //return item;
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<Menu> ParentmenuStore(int roleid, int planid, int userid,int storeid)
        {
            try
            {
                StoreContext _Context = new StoreContext();
              
                return _Context.Database.SqlQuery<Menu>("exec USP_ParentMenuStore @UserId,@PlanId,@RoleId,@StoreId",
                   new SqlParameter("@UserId", userid),
                   new SqlParameter("@PlanId", planid),
                   new SqlParameter("@RoleId", roleid),
                   new SqlParameter("@StoreId", storeid)
                   ).ToList();
                //return item;
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<Menu> ChildmenuStore(int id, int roleid, int userid,int CustPlanid)
        {
            try
            {
                StoreContext _Context = new StoreContext();               
                return _Context.Database.SqlQuery<Menu>("exec USP_ChildMenuStore @UserId,@Parentid,@RoleId,@CustPlanid",
                  new SqlParameter("@UserId", userid),
                  new SqlParameter("@Parentid", id),
                  new SqlParameter("@RoleId", roleid),
                  new SqlParameter("@CustPlanid", CustPlanid)
                  ).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<Menu> ChildmenuStores(int id, int roleid, int userid, int CustPlanid)
        {
            try
            {
                
                return _storeContext.Database.SqlQuery<Menu>("exec USP_ChildMenuStore @UserId,@Parentid,@RoleId,@CustPlanid",
                  new SqlParameter("@UserId", userid),
                  new SqlParameter("@Parentid", id),
                  new SqlParameter("@RoleId", roleid),
                  new SqlParameter("@CustPlanid", CustPlanid)
                  ).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }
        public List<CustomerPlan> GetRoles()
        {
            try
            {
                return _storeContext.customerPlans.Where(x => x.IsDeleted == false).ToList();
            }
            catch(Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<MenuItem> GetParentMenus(int Roleid)
        {
            try
            {
                return _storeContext.menuItems.Where(x => x.Role.Id == Roleid &&  x.ParentMenuId==null && x.IsDeleted == false).ToList();
            }
            catch(Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<MenuItem> GetChildMenu(int Parentid, int roleid)
        {
            try
            {
                return _storeContext.menuItems.Where(x =>  x.ParentMenuId == Parentid && x.IsDeleted == false && x.Role.Id==roleid).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public Plan GetPlan(int roleid, int menuid)
        {
            try
            {
                return _storeContext.plans.Where(x => x.CustomerPlan.Id == roleid && x.MenuItem.Id == menuid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int DeletePermission(int roleid, int cusplanid)
        {
             try
            {
                return _storeContext.Database.ExecuteSqlCommand("exec USP_RemovePreviousPermission @CustPlanId,@RoleId",
                 new SqlParameter("@CustPlanId", cusplanid),
                 new SqlParameter("@RoleId", roleid));
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int Insertmenu(Plan plan)
        {
            try
            {
                _storeContext.plans.Add(plan);
                return _storeContext.SaveChanges();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public MenuItem MenuItem(int id)
        {
            try
            {
                
                return _storeContext.menuItems.Find(id);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public CustomerPlan CustomerPlan(int id)
        {
            try
            {
                return _storeContext.customerPlans.Find(id);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<DashBoardPer> DashBoardPer(int roleid)
        {
            try
            {
                return _storeContext.dashBoardPers.Where(x => x.IsDeleted == false && x.Role.Id==roleid).ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DashBoardMapping DashBoardMapping(int custplanid, int dashid)
        {
            try
            {
                return _storeContext.dashBoardMappings.Where(x => x.CustomerPlan.Id == custplanid && x.DashBoardPer.Id==dashid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteWidgetMaping(int Custplanid, int Roleid)
        {
            try
            {
                return _storeContext.Database.ExecuteSqlCommand("exec USP_DeleteWidget @CustPlanid,@RoleId",
                 new SqlParameter("@CustPlanid", Custplanid),
                 new SqlParameter("@RoleId", Roleid));
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int InserWidget(DashBoardMapping dashBoardMapping)
        {
            try
            {
                _storeContext.dashBoardMappings.Add(dashBoardMapping);
                return _storeContext.SaveChanges();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }

        }

        public DashBoardPer DashBoardPerbyid(int id)
        {
            try
            {
                return _storeContext.dashBoardPers.Find(id);
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<Menu> GetWidget(int Roleid, int Custplanid, int Userid)
        {
            try
            {
                StoreContext _Context = new StoreContext();

                return _Context.Database.SqlQuery<Menu>("exec USP_GetWidget @UserId,@CustPlanId,@RoleId",
                              new SqlParameter("@UserId", Userid),
                              new SqlParameter("@CustPlanId", Custplanid),
                              new SqlParameter("@RoleId", Roleid)
                              ).ToList();
            }
            catch(Exception ex)
            {

                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
         
           
        }

        public List<Menu> GetWidgetForStore(int Roleid, int Custplanid, int Storeid)
        {
            try
            {
                StoreContext _Context = new StoreContext();

                return _Context.Database.SqlQuery<Menu>("exec USP_GetWidgetForStore @UserId,@CustPlanId,@RoleId",
                   new SqlParameter("@UserId", Storeid),
                   new SqlParameter("@CustPlanId", Custplanid),
                   new SqlParameter("@RoleId", Roleid)
                   ).ToList();
            }
            catch(Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
           
        }

        public int DeleteStorePlan(int storeid)
        {
            try
            {
                return _storeContext.Database.ExecuteSqlCommand("exec USP_DeleteStorePlan @StoreId",
                  new SqlParameter("@StoreId", storeid));
            }
            catch(Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public Menu GetMenuForStore(int MenuId, int planid,int storeid)
        {
            try
            {
                return _storeContext.Database.SqlQuery<Menu>("exec USP_CheckMenuForStore @Custplanid,@menuId,@StoreId",
                  new SqlParameter("@Custplanid", planid),
                  new SqlParameter("@menuId", MenuId),
                  new SqlParameter("@StoreId", storeid)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public Store GetStore(int id)
        {
            try
            {
                return _storeContext.Stores.Find(id);
            }
            catch(Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public Menu CheckForStoreSave(int MenuId, int planid)
        {
            try
            {
                return _storeContext.Database.SqlQuery<Menu>("exec USP_CheckMenuForStoreSave @Custplanid,@menuId",
               new SqlParameter("@Custplanid", planid),
               new SqlParameter("@menuId", MenuId)).FirstOrDefault();
            }
            catch(Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int DeleteStoreUserWidget(int roleid, int userid)
        {
            throw new NotImplementedException();
        }

        public int IsUserMenu(int CustPlanid, int MenuItemId, int UserId)
        {
            try
            {
                return _storeContext.Database.SqlQuery<int>("exec USP_IsUserMenu @CustPlanId,@MenuItemId,@UserId",
               new SqlParameter("@Custplanid", CustPlanid),
               new SqlParameter("@MenuItemId", MenuItemId),
               new SqlParameter("@UserId", UserId)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public bool IsUserDashboard(int dashboardId, int customerPlanId, int userId)
        {
            try
            {
                return _storeContext.Database.SqlQuery<bool>("exec USP_IsUserDashboardForUser @DashboardId,@CustPlanId,@UserId",
               new SqlParameter("@DashboardId", dashboardId),
               new SqlParameter("@CustPlanId", customerPlanId),
               new SqlParameter("@UserId", userId)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int DeleteWidgetMapingForUser(int CustPlanId, int RoleId, int UserId)
        {
            try
            {
                return _storeContext.Database.ExecuteSqlCommand("exec USP_DeleteWidgetMapingForUser @RoleId, @UserId",
                 new SqlParameter("@RoleId", RoleId),
                 new SqlParameter("@UserId", UserId));
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int InsertWidgetForUser(DashBoardMapping map)
        {
            try
            {
                return _storeContext.Database.SqlQuery<int>("exec USP_AddDashboardWidgetForUser @DashBoardPerId,@UserId,@IsShow",
               new SqlParameter("@DashBoardPerId", map.DashBoardPer.Id),
               new SqlParameter("@UserId", map.Users.Id),
               new SqlParameter("@IsShow", map.IsShow)
               ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int DeleteUserPermission(int UserId)
        {
            try
            {
                return _storeContext.Database.SqlQuery<int>("exec USP_DeleteUserPermission @UserId",
               new SqlParameter("@UserId", UserId)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public List<Plan> plans(int Custplanid)
        {
            try
            {
                return _storeContext.plans.Where(x => x.CustomerPlan.Id == Custplanid && x.MenuItem.Role.Id == 2 && x.IsShow==true).ToList();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public int InsertAdminPlan(Plan plan)
        {
            try
            {
                return _storeContext.Database.SqlQuery<int>("exec USP_AddAdminPlan @UserId,@MenuId,@IsShow",
               new SqlParameter("@UserId", plan.UserID),
               new SqlParameter("@MenuId", plan.MenuItemID),
               new SqlParameter("@IsShow", plan.IsShow)).FirstOrDefault();

            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public Menu GetMenu(string ActionName, string ControlName, int RoleId, int UserId, int CustomerPlanId)
        {
            try
            {
                StoreContext storeContext = new StoreContext();
                return storeContext.Database.SqlQuery<Menu>("exec USP_GetMenu @ActionName,@ControlName,@RoleId,@UserId,@CustomerPlanId",
               new SqlParameter("@ActionName", ActionName),
               new SqlParameter("@ControlName", ControlName),
               new SqlParameter("@RoleId", RoleId),
               new SqlParameter("@UserId", UserId),
               new SqlParameter("@CustomerPlanId",CustomerPlanId)).FirstOrDefault();

            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
        }

        public Master GetMaster(string Text)
        {
            try
            {
                return _storeContext.masters.Where(x => x.Text == Text).FirstOrDefault();
            }
            catch (Exception ex)
            {
                HelperCls.DebugLog(ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
                
            }
        }
    }
}
