
using DAL;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace StoreOrderingDashBoard.Models
{
    public class AdminApiAuthorization : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null && !authTicket.Expired)
                {
                    var userData = new JavaScriptSerializer().Deserialize<UserSession>(authTicket.UserData);
                   
                        //HttpContext.Current.Session["UserId"] = userData.Id;
                        //HttpContext.Current.Session["UserName"] = userData.Name;
                        //HttpContext.Current.Session["CustomerPlan"] = userData.PlanId;
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
    }
}