using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Mvc.Facebook;
using Newtonsoft.Json;

namespace fastforward.Models
{
    public class Timeline
    {
        [Key]
        public int TimelineId { get; set; }
        [ForeignKey("Career")]
        public int CareerId { get; set; }
        public virtual Career Career { get; set; }
        public List<Event> Events { get; set; }
        public List<LocalCollege> LocalColleges { get; set; }
        public List<RelatedOccupation> RelatedOccupations { get; set; }
        public MyAppUser User { get; set; }
        public List<MyAppUserFriend> Friends { get; set; }

    }
}