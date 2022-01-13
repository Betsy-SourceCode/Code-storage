using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIS_InventoryTracking.Models
{
    public class ResponseJson
    {
        public string FModel { get; set; }
        public List<CheckFullInventory_Temp_> Data { get; set; }
    }
}