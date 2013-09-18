
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace fastforward.Models
{
    public class Survey
    {
        [Required]
        public string SurveyName { get; set; }
        [Key]
        public int SurveyId { get; set; }
        public string Headline { get; set; }

        [Required]
        public virtual List<Question> Questions { get; set; } 

    }
}
