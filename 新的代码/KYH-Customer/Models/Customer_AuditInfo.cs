//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace KYH_Customer.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Customer_AuditInfo
    {
        public int CustomerId { get; set; }
        public string SORcvEntity { get; set; }
        public string MFGProcEntity { get; set; }
        public string FamilyTree { get; set; }
        public string DocCheckList { get; set; }
        public string SCACOpinion { get; set; }
        public string MDCreateBy { get; set; }
        public string MDCreateDept { get; set; }
        public Nullable<System.DateTime> MDCreateTime { get; set; }
        public string CWACOpinion { get; set; }
        public string ACCreateBy { get; set; }
        public string ACCreateDept { get; set; }
        public Nullable<System.DateTime> ACCreateTime { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}
