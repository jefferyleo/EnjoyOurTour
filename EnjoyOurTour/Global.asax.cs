using EnjoyOurTour.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace EnjoyOurTour
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (exception != null)
            {
                Logging.WriteLog("Message: " + exception.Message);
                Logging.WriteLog("Stack Trace: " + exception.StackTrace);
                Logging.WriteLog("Inner Exception: " + exception.InnerException);
            }
            if (exception is CryptographicException)
            {
                FormsAuthentication.SignOut();
                Response.Redirect("~/Account/Login");
            }
        }
    }
}
