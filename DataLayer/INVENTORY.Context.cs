﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tb_Company> tb_Company { get; set; }
        public virtual DbSet<tb_Customer> tb_Customer { get; set; }
        public virtual DbSet<tb_Department> tb_Department { get; set; }
        public virtual DbSet<tb_Inventory> tb_Inventory { get; set; }
        public virtual DbSet<tb_Invoice> tb_Invoice { get; set; }
        public virtual DbSet<tb_InvoiceDetail> tb_InvoiceDetail { get; set; }
        public virtual DbSet<tb_Origin> tb_Origin { get; set; }
        public virtual DbSet<tb_ProductCategory> tb_ProductCategory { get; set; }
        public virtual DbSet<tb_Supplier> tb_Supplier { get; set; }
        public virtual DbSet<tb_SYS_FUNC> tb_SYS_FUNC { get; set; }
        public virtual DbSet<tb_SYS_GROUP> tb_SYS_GROUP { get; set; }
        public virtual DbSet<tb_SYS_REPORT> tb_SYS_REPORT { get; set; }
        public virtual DbSet<tb_SYS_RIGHT> tb_SYS_RIGHT { get; set; }
        public virtual DbSet<tb_SYS_RIGHT_REP> tb_SYS_RIGHT_REP { get; set; }
        public virtual DbSet<tb_SYS_SEQ> tb_SYS_SEQ { get; set; }
        public virtual DbSet<tb_SYS_USER> tb_SYS_USER { get; set; }
        public virtual DbSet<tb_Unit> tb_Unit { get; set; }
        public virtual DbSet<V_FUNC_SYS_RIGHT> V_FUNC_SYS_RIGHT { get; set; }
        public virtual DbSet<V_INVOICE_DETAIL> V_INVOICE_DETAIL { get; set; }
        public virtual DbSet<V_REP_SYS_RIGHT_REP> V_REP_SYS_RIGHT_REP { get; set; }
        public virtual DbSet<V_USER_IN_GROUP> V_USER_IN_GROUP { get; set; }
        public virtual DbSet<V_USER_NOTIN_GROUP> V_USER_NOTIN_GROUP { get; set; }
        public virtual DbSet<tb_Product> tb_Product { get; set; }
    
        public virtual int Stock_Evaluation_Date(ObjectParameter dATEC, ObjectParameter dATED, ObjectParameter year, ObjectParameter period)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Stock_Evaluation_Date", dATEC, dATED, year, period);
        }
    }
}
