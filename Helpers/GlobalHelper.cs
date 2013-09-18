using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace fastforward.Helpers
{
    public static class GlobalHelper
    {
        public static string GetConfigSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}