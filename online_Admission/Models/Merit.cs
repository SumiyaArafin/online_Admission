//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace online_Admission.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Merit
    {
        public int m_id { get; set; }
        public Nullable<int> m_applicant_id { get; set; }
        public Nullable<int> m_dept_id { get; set; }
        public Nullable<int> m_appstatus_id { get; set; }
    
        public virtual department department { get; set; }
        public virtual student_registration student_registration { get; set; }
        public virtual status status { get; set; }
    }
}