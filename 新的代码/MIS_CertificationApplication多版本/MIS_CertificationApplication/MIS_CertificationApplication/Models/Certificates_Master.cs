//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MIS_CertificationApplication.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Certificates_Master
    {
        public int CMSerial { get; set; }
        public string CertCode { get; set; }
        public string CertName { get; set; }
        public string Mkt_Cnty { get; set; }
        public string StdFee { get; set; }
        public string StdTime { get; set; }
        public string Remark { get; set; }
        public string CreateBy { get; set; }
        public string CreateDept { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateDept { get; set; }
        public System.DateTime UpdateTime { get; set; }
        public bool IsVoid { get; set; }
    }
}
