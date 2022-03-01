using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIS_ECN.Models
{
    public class Download
    {
        public string filename { get; set; }//文件名称
        public string filepos { get; set; }//文件路径名
        public string attachid { get; set; }//文件表id
    }
}