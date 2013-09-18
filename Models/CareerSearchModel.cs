using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using fastforward.Database;

namespace fastforward.Models
{
    public class CareerSearchModel
    {
        public CareerSearchModel()
        {
            RelatedOccupations = new List<RelatedOccupation>();    
            SearchResults = new List<spGetCareerListByName_Result>();
        }

        [Required(ErrorMessage = "Search Text is required")]
        public string SearchText { get; set; }
        public List<RelatedOccupation> RelatedOccupations { get; set; }
        public List<spGetCareerListByName_Result> SearchResults { get; set; }

        //public bool FromSearchAction { get; set; }

        public bool HasRelatedOccupations
        {
            get { return RelatedOccupations.Any(); }
        }

        public bool HasSearchResults {
            get { return SearchResults.Any(); }
        }
    }
}