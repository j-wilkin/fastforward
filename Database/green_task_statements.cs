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
    
    public partial class green_task_statements
    {
        public string onetsoc_code { get; set; }
        public decimal task_id { get; set; }
        public string green_task_type { get; set; }
    
        public virtual occupation_data occupation_data { get; set; }
        public virtual task_statements task_statements { get; set; }
    }
}
