//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MIS_InventoryTracking.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CheckFullInventory_Temp_
    {
        public int LPSerial { get; set; }
        public string Ledger { get; set; }
        public string Fnumber { get; set; }
        public string MeasureUnit { get; set; }
        public Nullable<decimal> WIP_Qty { get; set; }
        public Nullable<decimal> MRB_Qty { get; set; }
        public Nullable<decimal> WH_Qty { get; set; }
        public Nullable<decimal> IQC_Qty { get; set; }
        public Nullable<decimal> OpenPO_Qty { get; set; }
        public Nullable<System.DateTime> Invt_Time { get; set; }
        public string Material { get; set; }
    }
}
