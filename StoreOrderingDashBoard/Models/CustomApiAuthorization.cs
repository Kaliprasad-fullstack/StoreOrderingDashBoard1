using DAL;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace StoreOrderingDashBoard.Models
{
    public class CustomApiAuthorization : ActionFilterAttribute
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
                    //HttpContext.Current.Session["StoreId"] = userData.Id;
                    //HttpContext.Current.Session["StoreName"] = userData.StoreName;
                    //HttpContext.Current.Session["CustomerPlan"] = userData.PlanId;
                    //HttpContext.Current.Session["CustomerId"] = userData.CustomerId;
                    //HttpContext.Current.Session["UserId"] = userData.UserId;
                    //HttpContext.Current.Session["MkrChkrFlag"] = userData.MkrChkrFlag;
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