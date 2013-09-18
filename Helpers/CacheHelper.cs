using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fastforward.Helpers
{
    public static class CacheHelper
    {
        public static void AddItemToCache(string cacheKeyUnformatted, object data)
        {
            string userId = CookieHelper.GetCookie(AppConstants.UserIdCookieName);

            if (!String.IsNullOrEmpty(userId))
            {
                string cacheKey = String.Format(cacheKeyUnformatted, userId);
                HttpContext.Current.Cache[cacheKey] = data;
            }
        }

        public static bool ExistsInCache(string cacheKeyUnformatted)
        {
            string userId = CookieHelper.GetCookie(AppConstants.UserIdCookieName);
            if (!String.IsNullOrEmpty(userId))
            {
                string cacheKey = String.Format(cacheKeyUnformatted, userId);
                return HttpContext.Current.Cache[cacheKey] != null;
            }

            return false;
        }

        public static object GetItemFromCache(string cacheKeyUnformatted)
        {
            string userId = CookieHelper.GetCookie(AppConstants.UserIdCookieName);
            if (!String.IsNullOrEmpty(userId))
            {
                string cacheKey = String.Format(cacheKeyUnformatted, userId);
                return HttpContext.Current.Cache[cacheKey];
            }

            return null;
        }
    }
}