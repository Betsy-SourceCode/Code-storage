using System;

namespace KYH_GetK3POInformation.Models.PublicSqlMethods
{
    // Token: 0x0200000D RID: 13
    public class Others
    {
        public int ID { get; set; }
        public int Serial_No { get; set; }
        public string CustomerList { get; set; }
        public string EmpName { get; set; }
        public string Fnumber { get; set; }
        public string GIP_PO { get; set; }
        public string Part_No { get; set; }
        public string Qty { get; set; }
        public string Unit { get; set; }
        //2022-6-14新增两个字段CurrencyID和InvUPrice
        public string CurrencyID { get; set; }
        public Nullable<decimal> InvUPrice { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public object ArrayList { get; set; }
    }
}
