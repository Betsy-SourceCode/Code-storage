//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace KYH_KnowledgeBase.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SelectKnowledgeBaseIndex
    {
        public int QnAID { get; set; }
        public string TopicArea { get; set; }
        public string Author { get; set; }
        public string KeyWord { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public Nullable<short> ClickTimes { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string CreateDept { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public string UpdateDept { get; set; }
        public string Name_CN { get; set; }
    }
}
