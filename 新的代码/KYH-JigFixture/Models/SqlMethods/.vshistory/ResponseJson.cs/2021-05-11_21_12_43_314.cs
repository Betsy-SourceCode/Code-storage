using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models.PublicSqlMethods
{
    public class ResponseJson
    {
        //大于0就是失败
        public int id { get; set; }
        public int Success { get; set; }
        public string Msg { get; set; }
        public object Data { get; set; }


    }
}