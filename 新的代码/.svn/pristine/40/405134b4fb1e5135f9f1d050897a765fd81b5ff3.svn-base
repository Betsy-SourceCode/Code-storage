using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using KYH_GetK3POInformation.Models.PublicSqlMethods;

namespace KYH_GetK3POInformation.Models.SqlMethods
{
    // Token: 0x0200000A RID: 10
    public class GetIndex
    {
        /// <summary>
        /// 查询K3数据SQL
        /// </summary>
        /// <param name="Bname"></param>
        /// <param name="FBillNo"></param>
        /// <param name="Mat_Code"></param>
        /// <param name="Fname"></param>
        /// <returns></returns>
        public LoadingListAddPOdata_Temp ByK3PO_NumSelect(string Bname, string FBillNo, string Mat_Code, string Fname)
        {
            LoadingListAddPOdata_Temp list = new LoadingListAddPOdata_Temp();
            LoadingListAddPOdata_Temp result;
            try
            {
                string queryIndexSql = "SELECT Top(1) 0 as LPSerial,0.00 as LoadQty,'' as LoadUnit,B.FBillNo PONum, C.FNumber, F.FName Supplier, left(C.FName + ' ' + C.FModel + ' ' + C.{3},300) Material, CAST(A.FAuxQty AS DECIMAL(10,3)) POQty , D.FName POUnit, E.FName POCurr, CAST(A.FAuxPrice AS DECIMAL(14,6)) UnitPrice, A.FDate AS NeedDate, A.FNote Remarks, CAST ((SELECT FExchangeRate FROM mis.{0}.dbo.t_ExchangeRateEntry WHERE (FCyTo = '1000') AND FExChangeRateType = 1 AND (DATEDIFF(d, FBegDate, GETDATE()) >= 0) AND (DATEDIFF(d, GETDATE(), FEndDate) >= 0)) / (SELECT FExchangeRate FROM mis.{0}.dbo.t_ExchangeRateEntry WHERE (FCyTo = B.FCurrencyID) AND FExChangeRateType = 1 AND (DATEDIFF(d, FBegDate, GETDATE()) >= 0) AND (DATEDIFF(d, GETDATE(), FEndDate) >= 0)) AS DECIMAL(12,6)) USDRate FROM   mis.{0}.dbo.POOrderEntry A LEFT OUTER JOIN mis.{0}.dbo.POOrder B ON A.FInterID = B.FInterID AND B.FCancellation = 'False' \r\n                                       LEFT OUTER JOIN mis.{0}.dbo.t_ICItem C ON C.FItemID = A.FItemID \r\n                                       LEFT OUTER JOIN mis.{0}.dbo.t_MeasureUnit D ON D.FMeasureUnitID = A.FUnitID \r\n                                       LEFT OUTER JOIN mis.{0}.dbo.t_Currency E ON E.FCurrencyID = B.FCurrencyID \r\n                                       LEFT OUTER JOIN mis.{0}.dbo.t_Supplier F ON F.FItemID = B.FSupplyID\r\n                WHERE  (B.FBillNo = '{1}') AND C.FNumber = '{2}'\r\n                ORDER by FAuxQty DESC";
                queryIndexSql = string.Format(queryIndexSql, new object[]
                {
                    Bname,
                    FBillNo,
                    Mat_Code,
                    Fname
                });
                LoadingListAddPOdata_Temp totalCount = this.db.Database.SqlQuery<LoadingListAddPOdata_Temp>(queryIndexSql, new object[0]).FirstOrDefault<LoadingListAddPOdata_Temp>();
                result = totalCount;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());
                list.Material = "error";  //如果报错就给字段赋值告诉系统
                return list;
            }
            return result;
        }

        /// <summary>
        /// 首页列表加载SQL
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public List<LoadingListAddPOdata_Temp_Select> GetIndexList(string username)
        {
            List<LoadingListAddPOdata_Temp_Select> result;
            try
            {
                string sql = "SELECT   TOP (100) PERCENT LEFT(PONum, 2) AS Ledger, CONVERT(varchar(50), CONVERT(decimal(20, 2), LoadQty * UnitPrice)) AS OriCurr_tt_Amt, CONVERT(varchar(50), CONVERT(decimal(12, 6), USDRate)) AS USDRate, CONVERT(varchar(50), \r\n                CONVERT(decimal(20, 6), UnitPrice / USDRate)) AS USD_Unit_Price, CONVERT(varchar(50), CONVERT(decimal(20, 2), \r\n                LoadQty * (UnitPrice / USDRate))) AS USD_tt_Amt, CONVERT(varchar(50), CONVERT(decimal(10, 2), POQty)) AS POQty, \r\n                CONVERT(varchar(50), CONVERT(decimal(10, 3), LoadQty)) AS LoadQty, CONVERT(varchar(50), UnitPrice) AS UnitPrice, \r\n                PONum, Fnumber, Remarks, POCurr, POUnit, Material, Supplier, LoadUnit, LPSerial, NeedDate FROM  dbo.LoadingListAddPOdata_Temp_" + username + "     ORDER BY PONum, Fnumber,LoadQty";
                List<LoadingListAddPOdata_Temp_Select> list = this.db.Database.SqlQuery<LoadingListAddPOdata_Temp_Select>(sql, new object[0]).ToList<LoadingListAddPOdata_Temp_Select>();
                result = list;
            }
            catch (Exception ex)
            {
                return null;
            }
            return result;
        }

        /// <summary>
        /// 查询临时表SQL（单条）
        /// </summary>
        /// <param name="username"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public LoadingListAddPOdata_Temp GetIndexListFirst(string username, int index)
        {
            LoadingListAddPOdata_Temp result;
            try
            {
                string sql = "SELECT * from LoadingListAddPOdata_Temp_" + username + "  where LPSerial=" + index.ToString();
                LoadingListAddPOdata_Temp list = this.db.Database.SqlQuery<LoadingListAddPOdata_Temp>(sql, new object[0]).FirstOrDefault<LoadingListAddPOdata_Temp>();
                result = list;
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 查询临时表SQL（全部）
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public List<LoadingListAddPOdata_Temp> GetIndexListAll(string username)
        {
            List<LoadingListAddPOdata_Temp> result;
            try
            {
                string sql = "SELECT * from LoadingListAddPOdata_Temp_" + username;
                List<LoadingListAddPOdata_Temp> list = this.db.Database.SqlQuery<LoadingListAddPOdata_Temp>(sql, new object[0]).ToList<LoadingListAddPOdata_Temp>();
                result = list;
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 检查临时表是否存在SQL
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int CheckTable(string username)
        {
            return this.db.Database.SqlQuery<int>("select count(1) from sys.objects where name = 'LoadingListAddPOdata_Temp_" + username + "'", new object[0]).FirstOrDefault<int>();
        }

        /// <summary>
        /// 创建临时表SQL
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool CREATETable(string username)
        {
            bool result;
            try
            {
                DBNull sql = this.db.Database.SqlQuery<DBNull>("CREATE TABLE dbo.LoadingListAddPOdata_Temp_" + username + "([LPSerial] [int] IDENTITY(1,1) NOT NULL,[PONum] [nvarchar](20) NOT NULL,[Fnumber] [nvarchar](20) NOT NULL,[LoadQty] [decimal](10, 3) NOT NULL,[LoadUnit] [nvarchar](8) NOT NULL,[Supplier] [nvarchar](100) NULL,[Material] [nvarchar](300) NULL,[POQty] [decimal](10, 3) NULL,[POUnit] [nvarchar](8) NULL,[POCurr] [nchar](3) NULL,[UnitPrice] [decimal](14, 6) NULL,[NeedDate] [datetime] NULL,[Remarks] [nvarchar](80) NULL,[USDRate] [decimal](12, 6) NULL,PRIMARY KEY( LPSerial ))", new object[]
                {
                    new SqlParameter("@username", username)
                }).SingleOrDefault<DBNull>();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 添加临时表数据SQL
        /// </summary>
        /// <param name="username"></param>
        /// <param name="List"></param>
        /// <returns></returns>
        public bool InsertData(string username, Others List)
        {
            bool result;
            try
            {
                string sql = "INSERT INTO dbo.LoadingListAddPOdata_Temp_" + username + "(PONum, Fnumber, LoadQty, LoadUnit) VALUES('{0}','{1}','{2}','{3}')";
                sql = string.Format(sql, new object[]
                {
                    List.GIP_PO,
                    List.Part_No,
                    List.Qty,
                    List.Unit
                });
                DBNull totalCount = this.db.Database.SqlQuery<DBNull>(sql, new object[0]).SingleOrDefault<DBNull>();
                result = true;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 修改临时表数据SQL
        /// </summary>
        /// <param name="username"></param>
        /// <param name="List"></param>
        /// <returns></returns>
        public bool UpdateData(string username, LoadingListAddPOdata_Temp List)
        {
            bool result;
            try
            {
                string sql = "update  dbo.LoadingListAddPOdata_Temp_" + username + "    SET Supplier ='{0}',Material='{1}',POQty ={2},POUnit ='{3}',POCurr ='{4}',UnitPrice ={5},NeedDate ='{6}',Remarks = '{7}',USDRate={8} WHERE LPSerial=" + List.LPSerial.ToString();
                if (List.POQty == null && List.UnitPrice == null && List.USDRate == null)
                {
                    sql = "update  dbo.LoadingListAddPOdata_Temp_" + username + "    SET Supplier ='{0}',Material='{1}',POQty =null,POUnit ='{3}',POCurr ='{4}',UnitPrice =null,NeedDate ='{6}',Remarks = '{7}',USDRate=null  WHERE LPSerial=" + List.LPSerial.ToString();
                }
                sql = string.Format(sql, new object[]
                {
                    List.Supplier,
                    List.Material,
                    List.POQty,
                    List.POUnit,
                    List.POCurr,
                    List.UnitPrice,
                    List.NeedDate,
                    List.Remarks,
                    List.USDRate
                });
                DBNull totalCount = this.db.Database.SqlQuery<DBNull>(sql, new object[0]).SingleOrDefault<DBNull>();
                result = true;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 清空临时表数据SQL
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool TRUNCATETABLE(string username)
        {
            bool result;
            try
            {
                this.db.Database.SqlQuery<DBNull>("TRUNCATE TABLE LoadingListAddPOdata_Temp_" + username, new object[0]).SingleOrDefault<DBNull>();
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        // Token: 0x04000026 RID: 38
        private WebStationEntities db = new WebStationEntities();
    }
}
