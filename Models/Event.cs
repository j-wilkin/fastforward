using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace fastforward.Models
{
    public class Event
    {
        public string Headline { get; set; }
        public string TextContent { get; set; }
        public string Comment { get; set; }
        public bool IsSelfPost { get; set; }
        public int Index { get; set; }
        
        [Key]
        public int EventId { get; set; }
        public string Image { get; set; }
        public bool IsDivider { get; set; }
        public bool Major { get; set; }

        [ForeignKey("Career")]
        public int CareerId { get; set; }
        public virtual Career Career { get; set; }
    }
}