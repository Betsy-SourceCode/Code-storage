using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KYH_ProductPrice.Models.PublicSqlMethods
{
    public class GetProductPriceListss
    {
        public int CPSerial  { get; set; }
        public string CreateTime { get; set; }
        public string CreateBy { get; set; }
        public string Ledger { get; set; }
        public string CustomerDisplayName { get; set; }
        public string FNumber { get; set; }
        public string CustProdCode { get; set; }
        public string CustProdName { get; set; }
        public string PrvCurr { get; set; }
        public decimal PrvUnit { get; set; }
        public string UpdCurr { get; set; }
        public decimal UpdUnit { get; set; }
        public string EffType { get; set; }
        public string EffDetail { get; set; }
        public string Remarks_MD { get; set; }
        public string Remarks_FD { get; set; }
        public bool Cancel { get; set; }
    }
}