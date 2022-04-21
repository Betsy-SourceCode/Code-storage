using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GIP_CallWebApi.Models
{
    public class Receipt
    {

        public int code { get; set; }
        public string msg { get; set; }
        public object result { get; set; }
        public string Name { get; set; }
        public string value { get; set; }
        public string ERPMO { get; set; }
        public string message { get; set; }
        public string data { get; set; }
        public int typeid { get; set; }
        public int id { get; set; }//判断执行是否成功  0- 失败  1- 成功

    }
}