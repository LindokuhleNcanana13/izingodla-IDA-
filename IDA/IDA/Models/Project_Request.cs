//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IDA.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Project_Request
    {
        public int id { get; set; }
        public string ProjectName { get; set; }
        public Nullable<System.DateTime> DateRequested { get; set; }
        public Nullable<int> ClientId { get; set; }
    }
}
