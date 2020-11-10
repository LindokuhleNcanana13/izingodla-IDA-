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
    
    public partial class Project
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Project()
        {
            this.AssignedPMs = new HashSet<AssignedPM>();
        }
    
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public Nullable<System.DateTime> Date_Started { get; set; }
        public Nullable<System.DateTime> Date_Concluded { get; set; }
        public Nullable<int> ClientId { get; set; }
        public Nullable<int> EmpId { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> DateRequested { get; set; }
        public string ProjectType { get; set; }
        public string Status { get; set; }
        public string Feedback { get; set; }
        public Nullable<System.DateTime> AdvertDate { get; set; }
        public Nullable<System.DateTime> BriefingDate { get; set; }
        public Nullable<System.DateTime> SubmitionDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssignedPM> AssignedPMs { get; set; }
    }
}
