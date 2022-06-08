using KYH_GetK3POInformation.Models.PublicSqlMethods;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

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
        public NewLoadingListAddPOdata_Temp ByK3PO_NumSelect(string Bname, string FBillNo, string Mat_Code, string Fname)
        {
            NewLoadingListAddPOdata_Temp list = new NewLoadingListAddPOdata_Temp();
            try
            {
                string queryIndexSql = @"SELECT Top(1)(Select count(FItemID) from mis.{0}.dbo.POOrderEntry where FInterID=A.FInterID and FItemID=A.FItemID and FAuxPrice<>A.FAuxPrice) AS OtherUnitPrice,0 as LPSerial,0.00 as LoadQty,'' as LoadUnit, B.FBillNo PONum, C.FNumber, F.FName Supplier, 
                                        left(C.FName + ' ' + C.FModel + ' ' + isnull(C.{3},''),300) Material, CAST(A.FAuxQty AS DECIMAL(10,3)) POQty ,
                                         D.FName POUnit, E.FName POCurr, CAST(A.FAuxPrice AS DECIMAL(14,6)) UnitPrice, A.FDate AS NeedDate, A.FNote Remarks, 
                                        CAST ((SELECT (Select FExchangeRate from mis.AIS20181011094554.dbo.t_ExchangeRateEntry
                                        where FCyTo = '1000' and FExchangeRateType =1 and datediff( D,FbegDate,getDate())>=0 
                                        and (DateDiff( d,getdate(),FEndDate)>=0) ) / G.FExchangeRate  AS ExRate
                                        FROM mis.AIS20181011094554.dbo.t_ExchangeRateEntry G
                                        Where (select FNumber from mis.AIS20181011094554.dbo.t_Currency H
                                        where H.FCurrencyID = G.FCyTo )= E.FNumber and  FExchangeRateType =1 
                                        and DateDiff(D,FBegDate,GETDATE()) >= 0 and DateDiff(d,getdate(),FEndDate) >= 0 ) AS DECIMAL(12,6)) USDRate
                                        FROM mis.{0}.dbo.POOrderEntry A LEFT OUTER JOIN mis.{0}.dbo.POOrder B  ON A.FInterID = B.FInterID AND B.FCancellation = 'False'  LEFT OUTER JOIN mis.{0}.dbo.t_ICItem C ON C.FItemID = A.FItemID                                    
                                        LEFT OUTER JOIN mis.{0}.dbo.t_MeasureUnit D ON D.FMeasureUnitID = A.FUnitID                        
                                         LEFT OUTER JOIN mis.{0}.dbo.t_Currency E ON E.FCurrencyID = B.FCurrencyID                            
                                          LEFT OUTER JOIN mis.{0}.dbo.t_Supplier F ON F.FItemID = B.FSupplyID          
                                        WHERE  (B.FBillNo = '{1}') AND C.FNumber = '{2}'
                                        ORDER by FAuxQty DESC";
                queryIndexSql = string.Format(queryIndexSql, Bname, FBillNo, Mat_Code, Fname);
                var aa = queryIndexSql;
                list = this.db.Database.SqlQuery<NewLoadingListAddPOdata_Temp>(queryIndexSql).FirstOrDefault();

            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                list.Material = "error";  //如果报错就给字段赋值告诉系统
            }
            return list;
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
                string sql = "SELECT   LEFT(PONum, 2) AS Ledger, CONVERT(varchar(50), CONVERT(decimal(20, 2), LoadQty * UnitPrice)) AS OriCurr_tt_Amt, CONVERT(varchar(50), CONVERT(decimal(12, 6), USDRate)) AS USDRate, CONVERT(varchar(50), \r\n                CONVERT(decimal(20, 6), UnitPrice / USDRate)) AS USD_Unit_Price, CONVERT(varchar(50), CONVERT(decimal(20, 2), \r\n                LoadQty * (UnitPrice / USDRate))) AS USD_tt_Amt, CONVERT(varchar(50), CONVERT(decimal(10, 2), POQty)) AS POQty, \r\n                CONVERT(varchar(50), CONVERT(decimal(10, 3), LoadQty)) AS LoadQty, CONVERT(varchar(50), UnitPrice) AS UnitPrice, \r\n                PONum, Fnumber, Remarks, POCurr, POUnit, Material, Supplier, LoadUnit, LPSerial, NeedDate FROM  dbo.LoadingListAddPOdata_Temp_" + username + "  ORDER BY LPSerial";
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
        /// 优化：首页列表加载SQL
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public List<LoadingListAddPOdata_Temp_Select> NewGetIndexList(string LoginName)
        {
            try
            {
                //执行存储过程
                string sql = "exec porc_GetK3POInformationIndexData " + LoginName;
                List<LoadingListAddPOdata_Temp_Select> list = db.Database.SqlQuery<LoadingListAddPOdata_Temp_Select>(sql).ToList();
                return list;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                return null;
            }
        }






        /// <summary>
        /// 查询临时表SQL（单条）
        /// </summary>
        /// <param name="username"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public NewLoadingListAddPOdata_Temp GetIndexListFirst(string username, int index)
        {
            NewLoadingListAddPOdata_Temp result;
            try
            {
                string sql = "SELECT * from LoadingListAddPOdata_Temp_" + username + "  where LPSerial=" + index.ToString();
                NewLoadingListAddPOdata_Temp list = this.db.Database.SqlQuery<NewLoadingListAddPOdata_Temp>(sql, new object[0]).FirstOrDefault<NewLoadingListAddPOdata_Temp>();
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
                DBNull sql = this.db.Database.SqlQuery<DBNull>("CREATE TABLE dbo.LoadingListAddPOdata_Temp_" + username + "([LPSerial] [int] NOT NULL, [PONum] [nvarchar](20) NOT NULL,[Fnumber][nvarchar](20) NOT NULL,[LoadQty][decimal](10, 3) NOT NULL,[LoadUnit][nvarchar](8) NOT NULL,[Supplier][nvarchar](100) NULL,[Material][nvarchar](300) NULL,[POQty][decimal](10, 3) NULL,[POUnit][nvarchar](8) NULL,[POCurr][nchar](3) NULL,[UnitPrice][decimal](14, 6) NULL,[NeedDate][datetime] NULL,[Remarks][nvarchar](80) NULL,[USDRate][decimal](12, 6) NULL)", new object[]
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
                string sql = "INSERT INTO dbo.LoadingListAddPOdata_Temp_" + username + "(LPSerial,PONum, Fnumber, LoadQty, LoadUnit) VALUES('{0}','{1}','{2}','{3}','{4}')";
                sql = string.Format(sql, new object[]
                {
                     List.Serial_No,
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
        public bool UpdateData(string username, NewLoadingListAddPOdata_Temp List)
        {
            bool result;
            string Remarks = List.Remarks;
            if (Remarks == null)
            {
                Remarks = "";
            }
            try
            {
                if (Remarks.Length >= 80 && Remarks != "")
                {
                    Remarks = List.Remarks.Substring(0, 80);
                }

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
                    Remarks,
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
        /// 优化：修改临时表数据SQL-账套抓出数据批量修改到主表Sql
        /// </summary>
        /// <param name="DatabaseName">服务器的数据库名</param>
        /// <param name="TableName">副表名</param>
        /// <param name="LoginName">登录者的主表名</param>
        /// <returns></returns>
        public int BatchUpdateSql(string LoginName)
        {
            try
            {
                //执行存储过程
                string sql = "exec porc_GetK3POInformation " + LoginName;
                int list = db.Database.SqlQuery<int>(sql).FirstOrDefault();
                return list;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }

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
