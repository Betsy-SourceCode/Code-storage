using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KYH_GetK3POInformation.Models.PublicSqlMethods
{
    public class IndexData
    {
        public int LPSerial { get; set; }
        public string PONum { get; set; }
        public string Fnumber { get; set; }
        public decimal LoadQty { get; set; }
        public string LoadUnit { get; set; }
        public string Supplier { get; set; }
        public string Material { get; set; }
        public Nullable<decimal> POQty { get; set; }
        public string POUnit { get; set; }
        public string POCurr { get; set; }
        public string UnitPrice { get; set; }
        public Nullable<System.DateTime> NeedDate { get; set; }
        public string Remarks { get; set; }
        public Nullable<decimal> USDRate { get; set; }
    }
}