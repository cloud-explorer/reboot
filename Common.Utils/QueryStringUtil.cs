using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Routing;
using HttpUtility = RestSharp.Contrib.HttpUtility;

namespace Projects.Common.Core
{
    public static class QueryStringUtil
    {
        public static string ToQueryString(NameValueCollection nvc)
        {
            var array = (from key in nvc.AllKeys
                         from value in nvc.GetValues(key)
                         select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value)))
                .ToArray();
            return "?" + string.Join("&", array);
        }

        private static NameValueCollection Copy(this NameValueCollection keyValueCollection)
        {
            NameValueCollection nvc = new NameValueCollection();
            foreach (string str in keyValueCollection.AllKeys)
            {
                string s = keyValueCollection[str];
                nvc.Add(str, s);
            }
            return nvc;
        }

        public static NameValueCollection AddToQueryString(string key, string value)
        {
            NameValueCollection nvc =  HttpContext.Current.Request.QueryString.Copy(); 
            nvc.Set(key, value);
            return nvc;
        }

        public static NameValueCollection RemoveFromQueryString(string key)
        {
            NameValueCollection nvc = HttpContext.Current.Request.QueryString.Copy();
            nvc.Remove(key);
            return nvc;
        }

        public static string AddAndGetNewQueryString(string key, string value)
        {
            NameValueCollection nvc = AddToQueryString(key, value);
            string qs = ToQueryString(nvc);
            return qs;
        }

        public static string Get(string key)
        {
            NameValueCollection nvc = HttpContext.Current.Request.QueryString;
            string qs = nvc.Get(key);
            return qs;
        }

        public static string RemoveAndGetNewQueryString(string key)
        {
            NameValueCollection nvc = RemoveFromQueryString(key);
            string qs = ToQueryString(nvc);
            return qs;
        }

        public static bool IsinQueryString(string key, string value)
        {
            NameValueCollection nvc = HttpContext.Current.Request.QueryString.Copy();
            string s = nvc.Get(key);
            if (s == null) return false;
            return s.Equals(value, StringComparison.CurrentCultureIgnoreCase);
        }

        public static RouteValueDictionary AddToRouteValues(string key, string value)
        {
            NameValueCollection nvc = HttpContext.Current.Request.QueryString.Copy();
            if (nvc == null || !nvc.HasKeys()) return new RouteValueDictionary();
            nvc.Set(key, value);
            var routeValues = new RouteValueDictionary();
            foreach (string k in nvc.AllKeys)
                routeValues.Add(k, nvc[k]);

            return routeValues;
        }
    }
}
