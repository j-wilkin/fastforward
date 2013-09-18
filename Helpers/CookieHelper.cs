using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fastforward.Helpers
{
    public static class CookieHelper
    {
        public static string GetCookie(string cookieName)
        {
            if (string.IsNullOrEmpty(cookieName))
                throw new Exception("Cookie name not specified.");

            string cookieVal = String.Empty;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
                cookieVal = cookie.Value;
            return cookieVal;
        }


        public static void SetCookie(string cookieName, string value, int? expirationMinutes = null)
        {

            if (string.IsNullOrEmpty(cookieName))
                throw new Exception("Cookie name not specified.");

            HttpCookie cookie = new HttpCookie(cookieName, value);
            if (expirationMinutes.HasValue)
                cookie.Expires = DateTime.Now.AddMinutes(expirationMinutes.Value);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }




        /// <summary>
        /// Delete the Cookie from the cache
        /// </summary>
        public static void DeleteCookie(string cookieName)
        {
            if (string.IsNullOrEmpty(cookieName))
                throw new Exception("Cookie name not specified.");

            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddYears(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static bool CookieExists(string cookieName)
        {
            bool exists = false;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
                exists = true;
            return exists;
        }

        public static Dictionary<string, string> GetAllCookies()
        {
            Dictionary<string, string> cookies = new Dictionary<string, string>();
            foreach (string key in HttpContext.Current.Request.Cookies.AllKeys)
            {
                cookies.Add(key, HttpContext.Current.Request.Cookies[key].Value);
            }
            return cookies;
        }

        public static void DeleteAllCookies()
        {
            var cookies = HttpContext.Current.Request.Cookies.AllKeys;
            foreach (var cookie in cookies)
            {
                DeleteCookie(cookie);
            }
        }
    }
}