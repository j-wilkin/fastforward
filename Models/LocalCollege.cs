using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace fastforward.Models
{
    public class LocalCollege
    {
        [Key]
        public int Id  { get; set; }
        public string College { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
        public string Website { get; set; }
    }
}