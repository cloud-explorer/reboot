using System.Web.Mvc;
using System.Web.Routing;

namespace Projects.Common.Core
{
    public static class UrlHelperExtension
    {
        public static string AddPage(this UrlHelper helper, int page)
        {

            var routeValueDict = new RouteValueDictionary
            {
                { "controller", helper.RequestContext.RouteData.Values["controller"] },
                { "action" , helper.RequestContext.RouteData.Values["action"]}
            };

            if (helper.RequestContext.RouteData.Values["id"] != null)
            {
                routeValueDict.Add("id", helper.RequestContext.RouteData.Values["id"]);
            }

            foreach (string name in helper.RequestContext.HttpContext.Request.QueryString)
            {
                routeValueDict.Add(name, helper.RequestContext.HttpContext.Request.QueryString[name]);
            }

            routeValueDict.Add("pagenumber", page);

            return helper.RouteUrl(routeValueDict);
        }
    }
}