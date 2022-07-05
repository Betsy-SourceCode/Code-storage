using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIS_CertificationApplication.Models
{
    public class NewCertificates_Master
    {
        public int CMSerial { get; set; }
        public string CertCode { get; set; }
        public string CertName { get; set; }
        public string Mkt_Cnty { get; set; }
        public string StdFee { get; set; }
        public string StdTime { get; set; }
        public string Remark { get; set; }
        public string CreateBy { get; set; }
        public string CountryArea { get; set; }
        public string CreateDept { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateDept { get; set; }
        public string RLV { get; set; }
        public System.DateTime UpdateTime { get; set; }
        public bool IsVoid { get; set; }
    }
}