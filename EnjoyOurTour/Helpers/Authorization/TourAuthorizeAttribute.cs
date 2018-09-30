using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace EnjoyOurTour.Helpers.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class TourAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Controller.TempData["Unauthorized"] = "You're not authorized to view this page.";
            //filterContext.Result = new RedirectResult("~/Account/Login");
            filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "Account",
                                action = "Login",
                                returnUrl = filterContext.HttpContext.Request.Url.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped)
                            }));
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var formsAuthentication = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName] != null ? FormsAuthentication.Decrypt(httpContext.Request.Cookies[FormsAuthentication.FormsCookieName].Value) : null;

            if (string.IsNullOrWhiteSpace(formsAuthentication?.Name))
                return false;

            return Roles.Contains(formsAuthentication.UserData);
        }

        public TourAuthorizeAttribute(params string[] roles) : base()
        {
            Roles = string.Join(",", roles);
        }

        public static class TourRole
        {
            public const string Superadmin = "Superadmin";
            public const string Admin = "Admin";
            public const string Agent = "Agent";
            public const string Customer = "Customer";
        }

        public static class TourRoleId
        {
            public const int Superadmin = 1;
            public const int Admin = 2;
            public const int Customer = 3;
            public const int Agent = 4;
        }
    }
}