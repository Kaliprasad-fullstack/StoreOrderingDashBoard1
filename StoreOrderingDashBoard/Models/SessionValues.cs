using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using DbLayer.Service;
using DAL;
using DbLayer.Repository;
using BAL;
using System.Web.Script.Serialization;

namespace StoreOrderingDashBoard.Models
{
    public class OTPSession
    {
        public static int UserId
        {
            get
            {
                if (HttpContext.Current.Session["OTPUserData"] == null) { UpdateOTPUserSession(); }
                return Convert.ToInt32(HttpContext.Current.Session["OTPUserData"].ToString().Split('|')[0]);
            }
        }
        public static string OTP
        {
            get
            {
                if (HttpContext.Current.Session["OTPUserData"] == null) { UpdateOTPUserSession(); }
                return HttpContext.Current.Session["OTPUserData"].ToString().Split('|')[1];
            }
        }

        private static void UpdateOTPUserSession()
        {
            if (HttpContext.Current.User != null)
            {
                var authCookie = HttpContext.Current.Request.Cookies["OTPCookie"];
                if (authCookie != null)
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (authTicket != null && !authTicket.Expired)
                    {
                        StoreRepository _storeService = new StoreRepository(new DbLayer.StoreContext());
                        var userData = new JavaScriptSerializer().Deserialize<UserSession>(authTicket.UserData);
                        if (userData!=null)
                        {
                            StoreLogin store = _storeService.StoreLoginSessionExtensionDetails(userData.EmailAddress, HelperCls.GetIPAddress());
                            if (store != null)
                            {
                                var difference = (DateTime.Now.Date - store.ModifiedDate.Date).TotalDays;
                                HttpContext.Current.Session["OTPUserData"] = store.UserId.ToString() + "|" + userData.OTP;
                            }
                        }                        
                    }
                }
            }
        }
    }
    public class SessionValues
    {
        public static int RoleId
        {
            get
            {
                if (HttpContext.Current.Session["AuthUserData"] == null) { UpdateAuthUserSession(); }
                return Convert.ToInt32(HttpContext.Current.Session["AuthUserData"].ToString().Split('|')[0]);
            }

        }
        public static int StoreId
        {
            get
            {
                if (HttpContext.Current.Session["AuthUserData"] == null) { UpdateAuthUserSession(); }
                return Convert.ToInt32(HttpContext.Current.Session["AuthUserData"].ToString().Split('|')[1]);
            }

        }
        public static int UserId
        {
            get
            {
                if (HttpContext.Current.Session["AuthUserData"] == null) { UpdateAuthUserSession(); }
                return Convert.ToInt32(HttpContext.Current.Session["AuthUserData"].ToString().Split('|')[2]);
            }
        }
        public static int? LoggedInCustId
        {
            get
            {
                if (HttpContext.Current.Session["AuthUserData"] == null) { UpdateAuthUserSession(); }
                return Convert.ToInt32(HttpContext.Current.Session["AuthUserData"].ToString().Split('|')[3]);
            }

        }
        public static string StoreUserName
        {
            get
            {
                if (HttpContext.Current.Session["AuthUserData"] == null) { UpdateAuthUserSession(); }
                return HttpContext.Current.Session["AuthUserData"].ToString().Split('|')[4];
            }
        }
        public static int PlanId
        {
            get
            {
                if (HttpContext.Current.Session["AuthUserData"] == null) { UpdateAuthUserSession(); }
                return Convert.ToInt32(HttpContext.Current.Session["AuthUserData"].ToString().Split('|')[5]);
            }

        }
        public static string MkrChkrFlag
        {
            get
            {
                if (HttpContext.Current.Session["AuthUserData"] == null) { UpdateAuthUserSession(); }
                return HttpContext.Current.Session["AuthUserData"].ToString().Split('|')[6];
            }
        }
        public static string SessionTimeOutStore
        {
            get
            {
                if (HttpContext.Current.Session["AuthUserData"] == null) { UpdateAuthUserSession(); }
                return HttpContext.Current.Session["AuthUserData"].ToString().Split('|')[7];
            }
        }
        public static string EmailAddress
        {
            get
            {
                if (HttpContext.Current.Session["AuthUserData"] == null) { UpdateAuthUserSession(); }
                return HttpContext.Current.Session["AuthUserData"].ToString().Split('|')[8];
            }
        }
        public static int Remainingdays
        {
            get
            {
                if (HttpContext.Current.Session["AuthUserData"] == null) { UpdateAuthUserSession(); }
                return Convert.ToInt32(HttpContext.Current.Session["AuthUserData"].ToString().Split('|')[9]);
            }

        }
        public static string Name
        {
            get
            {
                if (HttpContext.Current.Session["AuthUserData"] == null) { UpdateAuthUserSession(); }
                return HttpContext.Current.Session["AuthUserData"].ToString().Split('|')[10];
            }
        }
        public static int AdminCount {
            get
            {
                if (HttpContext.Current.Session["AdminCount"] == null) { HttpContext.Current.Session["AdminCount"] = 0; }
                return Convert.ToInt32(HttpContext.Current.Session["AdminCount"].ToString());
            }
            set
            {                
                HttpContext.Current.Session["AdminCount"] = value;
            }
        }
        public static int StoreCount
        {
            get
            {
                if (HttpContext.Current.Session["StoreCount"] == null) { HttpContext.Current.Session["StoreCount"] = 0; }
                return Convert.ToInt32(HttpContext.Current.Session["StoreCount"].ToString());
            }
            set
            {
                HttpContext.Current.Session["StoreCount"] = value;
            }
        }
        private static void UpdateAuthUserSession()
        {
            if (HttpContext.Current.User != null)
            {
                var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie!=null)
                    {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (authTicket != null && !authTicket.Expired)
                    {
                        StoreRepository _storeService = new StoreRepository(new DbLayer.StoreContext());                                                
                        var userData = new JavaScriptSerializer().Deserialize<UserSession>(authTicket.UserData);
                        if (userData.RoleId == 1)
                        {
                            StoreLogin store = _storeService.StoreLoginSessionExtensionDetails(userData.EmailAddress, HelperCls.GetIPAddress());
                            if (store != null)
                            {
                                var difference = (DateTime.Now.Date - store.ModifiedDate.Date).TotalDays;
                                // HttpContext.Current.Session["AuthUserData"] = store.Id.ToString() + "|" + store.UserId + "|" + store.CustomerId + "|" + data.ToString() + "|" + (Convert.ToDouble(SessionTime) * 60) + "|" + SessionTime;
                                HttpContext.Current.Session["AuthUserData"] = store.RoleId + "|" + store.Id.ToString() + "|" + store.UserId + "|" + store.CustomerId + "|" + userData.EmailAddress.ToString() + "|" + store.PlanId + "|" + store.MkrChkrFlag + "|" + userData.SessionTimeout + "|" + (store.EmailAddress != null ? store.EmailAddress : "") + "|" + difference + "|" + store.StoreName;
                            }
                        }
                        else if(userData.RoleId!=0)
                        {
                            UsersRepository _userRepository = new UsersRepository(new DbLayer.StoreContext());
                            var user = _userRepository.GetUsersByEmail(userData.UserName);
                            if(user!=null){
                                HttpContext.Current.Session["AuthUserData"] = user.RoleId + "|" + 0 + "|" + user.Id + "|" + userData.CustomerId + "|" + userData.UserName + "|" + user.CustomerPlan.Id + "|" + 0 + "|" + userData.SessionTimeout + "|" + (user.EmailAddress != null ? user.EmailAddress : "") + "|" + 0 + "|" + user.Name;
                            }
                        }
                    }
                }
            }
        }
    }
}