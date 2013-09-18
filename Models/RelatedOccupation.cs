using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace fastforward.Models
{
    public class RelatedOccupation
    {
        [Key]
        public int RelatedId { get; set; }
        public string Name { get; set; }
        public int CareerId { get; set; }
        
    }
}