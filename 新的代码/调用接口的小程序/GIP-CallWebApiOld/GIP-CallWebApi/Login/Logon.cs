using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIP_CallWebApi
{
    class Logon
    {
        public bool flag { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public string traceId { get; set; }
        public Data data { get; set; }
}
    /// <summary>
    /// 内部类data
    /// </summary>
    class Data
    {
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
