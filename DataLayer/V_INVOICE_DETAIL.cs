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
    
    public partial class V_INVOICE_DETAIL
    {
        public System.Guid InvoiceDetail_ID { get; set; }
        public Nullable<System.Guid> InvoiceID { get; set; }
        public string BARCODE { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> TotalPrice { get; set; }
        public Nullable<System.DateTime> Day { get; set; }
        public Nullable<int> STT { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public string Invoice { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<int> InvoiceType { get; set; }
        public string Invoice2 { get; set; }
        public Nullable<System.DateTime> Day2 { get; set; }
        public Nullable<int> InvoiceQuantity { get; set; }
        public Nullable<double> TotalInvoicePrice { get; set; }
        public string Description { get; set; }
        public string CompanyID { get; set; }
        public string DepartmentID { get; set; }
        public string ReceivingDepartmentID { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}
