using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace fastforward.Models
{
    public class Career 
    {
        public string CareerName { get; set; }
        [Key]
        public int CareerId { get; set; }
        public string Salary { get; set; }
        public string Education { get; set; }
        public string Image { get; set; }
        [ForeignKey("Group")]
        public int GroupId { get; set; }
        public string Summary { get; set; }
        public virtual CareerGroup Group { get; set; }
        public string Description { get; set; }

    }
}
