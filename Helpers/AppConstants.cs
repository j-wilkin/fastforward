using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fastforward.Helpers
{
    public static class AppConstants
    {
        public static int DefaultCookieLifetimeInMinutes = 10080;
        public static string ResultsCookieName = "results";
        public static string CareerIdCookieName = "careerId";
        public static string UserIdCookieName = "fbUserId";
        public static string CollegeConnectionsCacheKey = "collegeConnections-{0}";
        public static string ResultsCacheKey = "surveyResults-{0}";
        public static string BaseUrl = "baseUrl";

    }
}