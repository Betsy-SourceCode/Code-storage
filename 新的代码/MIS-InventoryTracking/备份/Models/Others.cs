using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIS_InventoryTracking.Models
{
    public class Others
    {
        public string Text { get; set; }
        public int LPSerial { get; set; }
        public string Ledger { get; set; }
        public string Fnumber { get; set; }
        public string MeasureUnit { get; set; }
        public string Material { get; set; }
        public string WIP_Qty { get; set; }
        public string MRB_Qty { get; set; }
        public string WH_Qty { get; set; }
        public string IQC_Qty { get; set; }
        public string OpenPO_Qty { get; set; }
        public DateTime Invt_Time { get; set; }
    }
}