﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MIS_InventoryTracking.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class WebStationEntities : DbContext
    {
        public WebStationEntities()
            : base("name=WebStationEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CheckFullInventory_Temp_> CheckFullInventory_Temp_ { get; set; }
    
        public virtual int InventoryTrackingProc(string tableName, string fnumber, string querySql, string querySql2)
        {
            var tableNameParameter = tableName != null ?
                new ObjectParameter("tableName", tableName) :
                new ObjectParameter("tableName", typeof(string));
    
            var fnumberParameter = fnumber != null ?
                new ObjectParameter("Fnumber", fnumber) :
                new ObjectParameter("Fnumber", typeof(string));
    
            var querySqlParameter = querySql != null ?
                new ObjectParameter("querySql", querySql) :
                new ObjectParameter("querySql", typeof(string));
    
            var querySql2Parameter = querySql2 != null ?
                new ObjectParameter("querySql2", querySql2) :
                new ObjectParameter("querySql2", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InventoryTrackingProc", tableNameParameter, fnumberParameter, querySqlParameter, querySql2Parameter);
        }
    }
}
