using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using System;
using KYH_JigFixture.Models;

namespace Customer.Models.SqlMethods
{
    public class GetIndex
    {
        C6Entities1 c6db = new C6Entities1();
        WebStationEntities Webdb = new WebStationEntities();
        //  Other Other = new Other();
        /// <summary>
        /// 查询OA表数据
        /// </summary>
        /// <param name="OAFlow_Num">OA申请单号</param>
        /// <param name="ApplyDate">申请日期</param>
        /// <param name="Mat_Code">物料代码</param>
        /// <param name="Mat_Name">物料名称</param>
        /// <returns></returns>
        public List<SelectOA_Result> GetIndexPageRowOA(string OAFlow_Num, string ApplyDate, string Mat_Code, string Mat_Name)
        {
            //默认显示非结案
            //带过滤条件查询OA数据
            string queryIndexSql = "exec SelectOA '{0}','{1}','{2}','{3}'";
            queryIndexSql = string.Format(queryIndexSql, OAFlow_Num, ApplyDate, Mat_Code, Mat_Name);
            List<SelectOA_Result> totalCount = c6db.Database.SqlQuery<SelectOA_Result>(queryIndexSql).ToList();
            return totalCount;
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
        /// 
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


        public List<SelectOA_Result> ByK3PO_NumSelect(string Bname, string FBillNo)
        {
            try
            {
                string queryIndexSql = @"  SELECT POOrder.FBillNo as K3PO_Num,POOrder.FDate as K3PO_Date,t_Supplier.FName as Supplier,t_item.FName as Buyer,t_measureunit.FName as k3Unit,POOrderEntry.FQty as K3FQty FROM HS_YHNPN.dbo.POOrder
                     INNER JOIN {0}.dbo.POOrderEntry ON POOrderEntry.FInterID = POOrder.FInterID
                     INNER JOIN {0}.dbo.t_Supplier ON t_Supplier.FItemID = POOrder.FSupplyID
                     INNER JOIN {0}.dbo.t_Item ON t_Item.FItemID=POOrder.FEmpID
                     INNER JOIN {0}.dbo.t_icitem ON t_ICItem.FItemID = POOrderEntry.FItemID
                     INNER JOIN {0}.dbo.t_measureunit ON POOrderEntry.FUnitID = t_measureunit.FItemID
                     WHERE POOrder.FBillNo='{1}'";
                queryIndexSql = string.Format(queryIndexSql, Bname, FBillNo);
                List<SelectOA_Result> totalCount = c6db.Database.SqlQuery<SelectOA_Result>(queryIndexSql).ToList();
                return totalCount;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }

}