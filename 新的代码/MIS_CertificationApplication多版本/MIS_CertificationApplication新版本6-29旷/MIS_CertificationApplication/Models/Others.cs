using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIS_CertificationApplication.Models
{
    public class Others
    {
        public int Value { get; set; }
        public int ID { get; set; }
        public int ReturnResultNum { get; set; }
        public int CMSerial { get; set; }
        public int CPSerial { get; set; }
        public string Name { get; set; }
        public string K3Parts { get; set; }
        public string Text { get; set; }
        public string ApplicationRef { get; set; }
        /// <summary>
        /// 所属部门
        /// </summary>
        public string DEP { get; set; }
        /// <summary>
        /// 所属兼任部门
        /// </summary>
        public string SDP { get; set; }
        /// <summary>
        /// 所属职位
        /// </summary>
        public string PST { get; set; }
        /// <summary>
        /// 所属兼任职位
        /// </summary>
        public string SPT { get; set; }
        /// <summary>
        /// 代理人帐号
        /// </summary>
        public string CSB { get; set; }

    }
}