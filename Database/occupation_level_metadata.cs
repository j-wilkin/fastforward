//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace fastforward.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class occupation_level_metadata
    {
        public string onetsoc_code { get; set; }
        public string item { get; set; }
        public string response { get; set; }
        public Nullable<decimal> n { get; set; }
        public Nullable<decimal> percent { get; set; }
        public System.DateTime date_updated { get; set; }
    
        public virtual occupation_data occupation_data { get; set; }
    }
}
