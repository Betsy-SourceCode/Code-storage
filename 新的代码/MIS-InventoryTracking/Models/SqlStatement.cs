using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIS_InventoryTracking.Models
{
    public class SqlStatement
    {
        WebStationEntities db = new WebStationEntities();
        /// <summary>
        /// 通过账套和物料号带出物料名称规格SQL
        /// </summary>
        /// <param name="Ledger">账套名</param>
        /// <param name="Fnumber"></param>
        /// <returns></returns>
        public string GetMaterialMessageSQL(string Ledger, string Fnumber)
        {
            if (Ledger == "LK")
            {
                Ledger = "AIS20181011094554";
            }
            if (Ledger == "HL")
            {
                Ledger = "AIS20151013110946";
            }
            if (Ledger == "YH")
            {
                Ledger = "AIS20170316112450";
            }
            if (Ledger == "HSYH")
            {
                Ledger = "HSYH_New";
            }
            string sql = "SELECT left((FName + ' ' + FModel),80) FModel FROM mis.{0}.dbo.t_ICItemCore where Fnumber='{1}'";
            sql = string.Format(sql, Ledger, Fnumber);
            return this.db.Database.SqlQuery<string>(sql).FirstOrDefault<string>();
        }
    }
}