using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HttpUtility = RestSharp.Contrib.HttpUtility;

namespace Projects.Common.Utils
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

        public static NameValueCollection AddToQueryString(string key, string value)
        {
            NameValueCollection nvc = new NameValueCollection();
            NameValueCollection queryString = HttpContext.Current.Request.QueryString;      
            foreach (string str in queryString.AllKeys)
            {
                string s = queryString[str];
                nvc.Add(str, s);
            }

            if (nvc.AllKeys.Any(q => String.Equals(q, key, StringComparison.CurrentCultureIgnoreCase)))
            {
                string val = nvc[key];
                val = !string.IsNullOrEmpty(val) ? val + "|" : string.Empty;
                val = val + value;
                nvc[key] = val;
            }
            else
            {
                nvc.Add(key, value);
            }

            return nvc;
        }

        public static string AddAndGetNewQueryString(string key, string value)
        {
            NameValueCollection nvc = AddToQueryString(key, value);
            string qs = ToQueryString(nvc);
            return qs;
        }
    }
}
