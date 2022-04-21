using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GIP_CallWebApi.Models
{
    public class DataSet
    {
        //factory_processes接口
        public string 厂商代码 { get; set; }
        public string 编码 { get; set; }
        public string SN条码 { get; set; }
        public string 周期 { get; set; }
        public string Po订单号 { get; set; }
        public string 任务令 { get; set; }
        public string 工厂 { get; set; }
        public string 线别 { get; set; }
        public string 班次 { get; set; }
        public string 是否维修品 { get; set; }
        public string 工序名称 { get; set; }
        public string 加工结果 { get; set; }
        public string 开始时间 { get; set; }
        //factory_reworks接口
        public string 型号 { get; set; }
        public string 故障日期 { get; set; }
        public string 不良现象 { get; set; }
        public string 出现环节 { get; set; }
        public string 不良分类 { get; set; }
        public string 不良原因分析 { get; set; }
        public string 原因分类 { get; set; }
        public string 原因小类 { get; set; }
        public string 根因 { get; set; }
        public string 不良器件位置 { get; set; }
        public string 修理日期 { get; set; }
    }
}