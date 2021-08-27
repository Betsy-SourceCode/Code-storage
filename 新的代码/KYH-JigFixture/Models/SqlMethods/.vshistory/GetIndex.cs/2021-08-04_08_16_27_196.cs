using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using System;
using KYH_JigFixture.Models;

namespace KYH_JigFixture.Models.SqlMethods
{
    public class GetIndex
    {
        C6Entities2 c6db = new C6Entities2();
        WebStationEntities Webdb = new WebStationEntities();
        /// <summary>
        /// 查询OA表数据
        /// </summary>
        /// <param name="OAFlow_Num">OA申请单号</param>
        /// <param name="ApplyDate">申请日期</param>
        /// <param name="Mat_Code">物料代码</param>
        /// <param name="Mat_Name">物料名称</param>
        /// <returns></returns>
        public List<SelectOA_Result> GetIndexPageRowOA(string OAFlow_Num, string K3PO_Num, string Buyer, string Start_Date, string End_Date, string Mat_Code, string Mat_Name, string Status)
        {
            string queryIndexSql = "exec SelectOA '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'";
            queryIndexSql = string.Format(queryIndexSql, new object[]
            {
                OAFlow_Num,
                K3PO_Num,
                Buyer,
                Start_Date,
                End_Date,
                Mat_Code,
                Mat_Name,
                Status
            });
            return this.c6db.Database.SqlQuery<SelectOA_Result>(queryIndexSql, new object[0]).ToList<SelectOA_Result>();
        }

        /// <summary>
        /// 查询新表数据
        /// </summary>
        /// <param name="OAFlow_Num">OA申请单号</param>
        /// <param name="K3PO_Num">K3申请单号</param>
        /// <param name="Buyer">采购员</param>
        /// <param name="ApplyDate">申请日期</param>
        /// <param name="Mat_Code">物料代码</param>
        /// <param name="Mat_Name">物料名称</param>
        /// <returns></returns>
        //public List<SelectOA_Result> GetIndexPageRow(string OAFlow_Num, string K3PO_Num, string Buyer, DateTime ApplyDate, string Mat_Code, string Mat_Name)
        //{
        //    //默认显示非结案
        //    //带过滤条件查询OA数据
        //    string queryIndexSql = "exec SelectOA '{0}','{1}','{2}','{3}'";
        //    queryIndexSql = string.Format(queryIndexSql, OAFlow_Num, ApplyDate, Mat_Code, Mat_Name);
        //    List<SelectOA_Result> totalCount = c6db.Database.SqlQuery<SelectOA_Result>(queryIndexSql).ToList();
        //    return totalCount;
        //}


        public Others ByK3PO_NumSelect(string Bname, string FBillNo, string Mat_Code)
        {
            Others result;
            try
            {
                string queryIndexSql = "SELECT POOrder.FBillNo as K3PO_Num,POOrder.FDate as K3PO_Date,t_Supplier.FName as Supplier,t_item.FName as Buyer,t_measureunit.FName as k3Unit,POOrderEntry.FQty as K3FQty FROM {0}.dbo.POOrder\r\n                     INNER JOIN {0}.dbo.POOrderEntry ON POOrderEntry.FInterID = POOrder.FInterID\r\n                     INNER JOIN {0}.dbo.t_Supplier ON t_Supplier.FItemID = POOrder.FSupplyID\r\n                     INNER JOIN {0}.dbo.t_Item ON t_Item.FItemID=POOrder.FEmpID\r\n                     INNER JOIN {0}.dbo.t_icitem ON t_ICItem.FItemID = POOrderEntry.FItemID\r\n                     INNER JOIN {0}.dbo.t_measureunit ON POOrderEntry.FUnitID = t_measureunit.FItemID\r\n                     WHERE POOrder.FBillNo='{1}' and t_icitem.FNumber='{2}'";
                queryIndexSql = string.Format(queryIndexSql, Bname, FBillNo, Mat_Code);
                Others totalCount = this.c6db.Database.SqlQuery<Others>(queryIndexSql, new object[0]).FirstOrDefault<Others>();
                result = totalCount;
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
    }

}