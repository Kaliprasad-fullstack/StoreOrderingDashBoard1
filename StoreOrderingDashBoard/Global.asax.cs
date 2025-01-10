using StoreOrderingDashBoard.App_Start;
using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace StoreOrderingDashBoard
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityConfig.RegisterComponents();
            //Start Added By nikhil Kadam 05-Feb-2019 removed X-AspNetMvc-Version from response header
            MvcHandler.DisableMvcResponseHeader = true;
            //end

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            try
            {
                if (exception.InnerException != null)
                {
                    BAL.HelperCls.DebugLog(exception.Message + System.Environment.NewLine + exception.InnerException.Message
                        + System.Environment.NewLine + exception.StackTrace);

                    HttpContext.Current.Session["AppErrSession"] = exception.Message + System.Environment.NewLine + exception.InnerException.Message;
                }
                else
                {
                    BAL.HelperCls.DebugLog(exception.Message + System.Environment.NewLine + exception.StackTrace);
                    HttpContext.Current.Session["AppErrSession"] = exception.Message;
                }
            }
            catch (Exception ex)
            {

            }
            
            Server.ClearError();
            
            Response.Redirect("~/Home/Error500");
        }
    }
}
