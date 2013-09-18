using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using fastforward.Database;

namespace fastforward.Models
{
    public class CareerSearchResultDetailsModel
    {
        public string CareerName { get; set; }
        public string SearchText { get; set; }

        public spGetCareerInfoBasic_Result DescriptionDetails { get; set; }
        public List<spGetCareerInfoCareerTasks_Result> Tasks { get; set; }
        public List<spGetCareerInfoKnowledge_Result> KnowledgeItems { get; set; }
        public List<spGetCareerInfoSkills_Result> Skills { get; set; }
        public List<spGetCareerInfoAbilities_Result> Abilities { get; set; }
        public List<spGetCareerInfoWorkActivities_Result> Activities { get; set; }
        public List<spGetCareerInfoInterests_Result> Interests { get; set; }
        public List<spGetCareerInfoWorkStyles_Result> Styles { get; set; }
    }
}