//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class V_USER_IN_GROUP
    {
        public Nullable<int> UserID { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CompanyID { get; set; }
        public string DepartmentID { get; set; }
        public string LastPasswordChange { get; set; }
        public Nullable<bool> IsGroup { get; set; }
        public Nullable<bool> IsDisable { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public int Groups { get; set; }
        public int Member { get; set; }
    }
}
