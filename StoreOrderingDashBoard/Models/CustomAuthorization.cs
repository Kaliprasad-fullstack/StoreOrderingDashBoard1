using DAL;
using DbLayer.Repository;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace StoreOrderingDashBoard.Models
{
    public class CustomAuthorization : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            var url = filterContext.HttpContext.Request.Url.ToString();
            //string[] words = url.Split('/');
            string ControllerName = String.Format("{0}", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName);
            string ActionName = String.Format("{0}", filterContext.ActionDescriptor.ActionName);
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null && !authTicket.Expired)
                {
                    var userData = new JavaScriptSerializer().Deserialize<UserSession>(authTicket.UserData);
                    MenuRepository _menuRepository = new MenuRepository();
                    var menu = _menuRepository.GetMenu(ActionName, ControllerName, 1, userData.UserId, userData.PlanId);
                    if (menu != null || ControllerName == "Order")
                    {
                        //HttpContext.Current.Session["StoreId"] = userData.Id;
                        //HttpContext.Current.Session["UserId"] = userData.UserId;
                        //HttpContext.Current.Session["CustomerId"] = userData.CustomerId;
                        //HttpContext.Current.Session["StoreName"] = userData.StoreName;
                        //HttpContext.Current.Session["CustomerPlan"] = userData.PlanId;
                                                
                        //HttpContext.Current.Session["MkrChkrFlag"] = userData.MkrChkrFlag;
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{{ "controller", "Home" },
                                          { "action", "Error404" }});
                    }
                }
                else
                {

                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{{ "controller", "Store" },
                                          { "action", "Login" }});
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{{ "controller", "Store" },
                                          { "action", "Login" }});
            }

            base.OnActionExecuting(filterContext);

        }

    }

    public class OTPAuthorization : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var authCookie = HttpContext.Current.Request.Cookies["OTPCookie"];
            var url = filterContext.HttpContext.Request.Url.ToString();
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null && !authTicket.Expired)
                {
                    var userData = new JavaScriptSerializer().Deserialize<Users>(authTicket.UserData);
                    if (userData != null)
                    {
                        HttpContext.Current.Session["OTPUserData"] = userData.UserId.ToString() + "|" + userData.OTP;
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{{ "controller", "Store" },
                                          { "action", "Login" }});
                    }
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{{ "controller", "Store" },
                                          { "action", "Login" }});
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{{ "controller", "Store" },
                                          { "action", "Login" }});
            }

            base.OnActionExecuting(filterContext);
        }

    }

    public class CommanAuthorization : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            var url = filterContext.HttpContext.Request.Url.ToString();
            //string[] words = url.Split('/');
            string ControllerName = String.Format("{0}", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName);
            string ActionName = String.Format("{0}", filterContext.ActionDescriptor.ActionName);
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null && !authTicket.Expired)
                {
                    var userData = new JavaScriptSerializer().Deserialize<StoreLogin>(authTicket.UserData);
                    MenuRepository _menuRepository = new MenuRepository();
                    var menu = _menuRepository.GetMenu(ActionName, ControllerName, 1, userData.UserId, userData.PlanId);
                    if (menu != null || ControllerName == "Order")
                    {
                        HttpContext.Current.Session["StoreId"] = userData.Id;
                        HttpContext.Current.Session["StoreName"] = userData.StoreName;
                        HttpContext.Current.Session["CustomerPlan"] = userData.PlanId;
                        HttpContext.Current.Session["CustomerId"] = userData.CustomerId;
                        HttpContext.Current.Session["UserId"] = userData.UserId;
                        HttpContext.Current.Session["MkrChkrFlag"] = userData.MkrChkrFlag;
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{{ "controller", "Home" },
                                          { "action", "Error404" }});
                    }
                }
                else
                {

                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{{ "controller", "Store" },
                                          { "action", "Login" }});
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{{ "controller", "Store" },
                                          { "action", "Login" }});
            }
            base.OnActionExecuting(filterContext);
        }
    }



}