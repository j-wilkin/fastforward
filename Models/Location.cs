using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fastforward.Models
{
    public class Location
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
    }
}