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
    
    public partial class tb_Department
    {
        public string DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentPhone { get; set; }
        public string DepartmentFax { get; set; }
        public string DepartmentEmail { get; set; }
        public string DepartmentAddress { get; set; }
        public string CompanyID { get; set; }
        public Nullable<bool> Warehouse { get; set; }
        public string Symbol { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> RestoredDate { get; set; }
        public Nullable<bool> IsDisabled { get; set; }
    }
}
