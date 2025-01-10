using DAL;
using DbLayer.Repository;
using DbLayer.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace StoreOrderingDashBoard.Models
{
    public class AdminAuthorization : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            var url = filterContext.HttpContext.Request.Url.ToString();
            string ControllerName = String.Format("{0}", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName);
            string ActionName = String.Format("{0}", filterContext.ActionDescriptor.ActionName);

            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null && !authTicket.Expired)
                {
                    var userData = new JavaScriptSerializer().Deserialize<UserSession>(authTicket.UserData);
                    List<ControllerAction> BypassControllerActionList = LoadJson();
                    if (ControllerName == "Home" && ActionName == "Customers")
                    {

                        //HttpContext.Current.Session["UserId"] = userData.Id;
                        //HttpContext.Current.Session["UserName"] = userData.Name;
                        //HttpContext.Current.Session["CustomerPlan"] = userData.PlanId;

                    }
                    else if (BypassControllerActionList != null && BypassControllerActionList.Count > 0)
                    {
                        if (BypassControllerActionList.Any(x => x.Action == ActionName && x.Controller == ControllerName))
                        {
                            //HttpContext.Current.Session["UserId"] = userData.Id;
                            //HttpContext.Current.Session["UserName"] = userData.Name;
                            //HttpContext.Current.Session["CustomerPlan"] = userData.PlanId;
                        }
                        else
                        {
                            MenuRepository _menuRepository = new MenuRepository();
                            var menu = _menuRepository.GetMenu(ActionName, ControllerName, 2, userData.UserId, userData.PlanId);
                            if (menu != null)
                            {
                                //HttpContext.Current.Session["UserId"] = userData.Id;
                                //HttpContext.Current.Session["UserName"] = userData.Name;
                                //HttpContext.Current.Session["CustomerPlan"] = userData.PlanId;
                            }
                            else
                            {
                                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{{ "controller", "Home" },
                                          { "action", "Error404" }});
                            }

                        }
                    }
                    else
                    {
                        MenuRepository _menuRepository = new MenuRepository();
                        var menu = _menuRepository.GetMenu(ActionName, ControllerName, 2, userData.UserId, userData.PlanId);
                        if (menu != null)
                        {
                            //HttpContext.Current.Session["UserId"] = userData.Id;
                            //HttpContext.Current.Session["UserName"] = userData.Name;
                            //HttpContext.Current.Session["CustomerPlan"] = userData.PlanId;
                        }
                        else
                        {
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{{ "controller", "Home" },
                                          { "action", "Error404" }});
                        }

                    }
                }
                else
                {

                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{{ "controller", "Home" },
                                          { "action", "Login" }});
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{{ "controller", "Home" },
                                          { "action", "Login" }});
            }

            base.OnActionExecuting(filterContext);

        }

        public static List<ControllerAction> LoadJson()
        {
            List<ControllerAction> ControllerActionList = new List<ControllerAction>();
            using (StreamReader r = new StreamReader(HttpContext.Current.Server.MapPath("~/Data/ByPassAuthControllerAction.json")))
            {
                var json = r.ReadToEnd();
                ControllerActionList = JsonConvert.DeserializeObject<List<ControllerAction>>(json);
            }
            return ControllerActionList;
        }
    }
}