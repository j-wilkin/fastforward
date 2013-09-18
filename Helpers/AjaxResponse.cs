using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace fastforward.Helpers
{
    public class AjaxResponse
    {
        public bool success { get; set; }
        public string msg { get; set; }
        public List<ModelError> errors { get; set; }
        public object data { get; set; }
    }
}