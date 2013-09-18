using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fastforward.Models
{
    public class Result
    {
        public bool HasTakenSurvey { get; set; }
        public bool Calculate { get; set; }
        public List<Career> TopCareers { get; set; }
        public List<Career> RemainingCareers { get; set; }
    }
}