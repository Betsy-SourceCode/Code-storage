using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace GIP_CallWebApi.Models
{
    [DataContract]
    class Logon
    {
        [DataMember]
        public bool flag { get; set; }
        [DataMember]
        public int code { get; set; }
        [DataMember]
        public object message { get; set; }
        [DataMember]
        public object traceId { get; set; }
        [DataMember]
        public Data data { get; set; }
    }
    [DataContract]
    /// <summary>
    /// 内部类data
    /// </summary>
    class Data
    {
        [DataMember]
        public string token { get; set; }
        public Info info { get; set; }
    }

    /// <summary>
    /// 内部类info
    /// </summary>
    class Info
    {
        public string account { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public int status { get; set; }
        public string desci { get; set; }
        public string defender { get; set; }
        public int useCount { get; set; }
        public string updateTime { get; set; }
        public string createTime { get; set; }
        public string remark { get; set; }
    }
}