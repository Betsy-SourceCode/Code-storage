//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace KYH_JigFixture.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
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
    
        public virtual DbSet<Tools_Acquire_Trace> Tools_Acquire_Trace { get; set; }
        public virtual DbSet<ActionLog2021> ActionLog2021 { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
    }
}
