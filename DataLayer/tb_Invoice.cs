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
    
    public partial class tb_Invoice
    {
        public System.Guid InvoiceID { get; set; }
        public string Invoice { get; set; }
        public Nullable<System.DateTime> Day { get; set; }
        public string Invoice2 { get; set; }
        public Nullable<System.DateTime> Day2 { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<double> TotalPrice { get; set; }
        public string Description { get; set; }
        public string CompanyID { get; set; }
        public string DepartmentID { get; set; }
        public string ReceivingDepartmentID { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}
