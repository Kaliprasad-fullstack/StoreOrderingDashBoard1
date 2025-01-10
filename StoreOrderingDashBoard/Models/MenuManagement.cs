using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using DbLayer.Repository;

namespace StoreOrderingDashBoard.Models
{
    public class MenuManagement
    {
        public static List<Menu> ParentMenu
        {
            get
            {
                if (HttpContext.Current.Session["PARENTMENU"] == null) { UpdateUserParentMenuSession(); }
                return (HttpContext.Current.Session["PARENTMENU"] as List<Menu>);
            }

        }

        private static void UpdateUserParentMenuSession()
        {
            MenuRepository menu = new MenuRepository();
            HttpContext.Current.Session["PARENTMENU"] = menu.menuItems(2, SessionValues.PlanId, SessionValues.UserId);
        }

        public static List<Menu> SubMenu(int ParentMenuId, int RoleId)
        {
            if (HttpContext.Current.Session["CHILDMENU"] == null) { UpdateUserChildMenuSession(ParentMenuId, RoleId); }
            return (HttpContext.Current.Session["CHILDMENU"] as List<Menu>).Where(x=>x.ParentMenuId==ParentMenuId).ToList();
        }

        private static void UpdateUserChildMenuSession(int ParentMenuId, int RoleId)
        {
            MenuRepository menu = new MenuRepository();
            List<Menu> childmenus = new List<Menu>();
            foreach (Menu parent in ParentMenu)
            {
                List<Menu> child = menu.ChildItem(parent.Id, 2, SessionValues.UserId, SessionValues.PlanId);
                if (child != null)
                    childmenus.AddRange(child);
            }
            HttpContext.Current.Session["CHILDMENU"] = childmenus;
        }

        //public static List<Menu> SubChildMenu(int SubMenuId, int RoleId)
        //{
        //    if (HttpContext.Current.Session["SUBCHILDMENU"] == null) { UpdateUserSubChildMenuSession(SubMenuId, RoleId); }
        //    return (HttpContext.Current.Session["SUBCHILDMENU"] as List<Menu>);
        //}

        //private static void UpdateUserSubChildMenuSession(int ParentMenuId, int RoleId)
        //{
        //    MenuRepository menu = new MenuRepository();
        //    List<Menu> childmenus = new List<Menu>();
        //    foreach (Menu child in )
        //    {
        //        List<Menu> child = menu.ChildItem(parent.Id, 2, SessionValues.UserId, SessionValues.PlanId);
        //        if (child != null)
        //            childmenus.AddRange(child);
        //    }
        //    HttpContext.Current.Session["SUBCHILDMENU"] = childmenus;
        //}



        public static List<Menu> StoreParentMenu
        {
            get
            {
                if (HttpContext.Current.Session["STOREPARENTMENU"] == null) { UpdateStoreUserParentMenuSession(); }
                return (HttpContext.Current.Session["STOREPARENTMENU"] as List<Menu>);
            }

        }

        private static void UpdateStoreUserParentMenuSession()
        {
            MenuRepository menu = new MenuRepository();
            HttpContext.Current.Session["STOREPARENTMENU"] = menu.ParentmenuStore(1, SessionValues.PlanId,SessionValues.LoggedInCustId.Value, SessionValues.UserId);
        }
        public static List<Menu> SubStoreMenu(int ParentMenuId, int RoleId)
        {
            if (HttpContext.Current.Session["CHILDSTOREMENU"] == null) { UpdateUserChildStoreMenuSession(ParentMenuId, RoleId); }
            return (HttpContext.Current.Session["CHILDSTOREMENU"] as List<Menu>).Where(x => x.ParentMenuId == ParentMenuId).ToList();
        }

        private static void UpdateUserChildStoreMenuSession(int ParentMenuId, int RoleId)
        {
            MenuRepository menu = new MenuRepository();
            List<Menu> childmenus = new List<Menu>();
            foreach (Menu parent in StoreParentMenu)
            {
                List<Menu> child = menu.ChildmenuStore(parent.Id, RoleId,SessionValues.StoreId, SessionValues.PlanId);
                if (child != null)
                    childmenus.AddRange(child);
            }
            HttpContext.Current.Session["CHILDSTOREMENU"] = childmenus;
        }

    }
}