using MIS_InventoryTracking.Models.PublicSqlMethods;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
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
            if (Ledger == "HS")
            {
                Ledger = "HSYH_New";
            }
            string sql = "SELECT left((FName + ' ' + FModel),80) FModel FROM mis.{0}.dbo.t_ICItemCore where Fnumber='{1}'";
            sql = string.Format(sql, Ledger, Fnumber);
            return this.db.Database.SqlQuery<string>(sql).FirstOrDefault<string>();
        }
        /// <summary>
        /// 检查临时表是否存在SQL
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int CheckTable(string username)
        {
            return this.db.Database.SqlQuery<int>("select count(1) from sys.objects where name = 'CheckFullInventory_Temp_" + username + "'", new object[0]).FirstOrDefault<int>();
        }
        /// <summary>
        /// 根据员工号获得三字代码SQL
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetUserSql(string userid)
        {
            Others list = this.db.Database.SqlQuery<Others>("select UserName AS Text From Users where UserID=@userid", new object[]
            {
            new SqlParameter("@userid", userid)
            }).FirstOrDefault<Others>();
            if (list == null)
            {
                return "";
            }
            return list.Text;
        }
        /// <summary>
        /// 获取首页主表数据
        /// </summary>
        /// <returns></returns>
        public List<CheckFullInventory_Temp_> GetIndexList(string username, string bank)
        {
            string sql = "select * from CheckFullInventory_Temp_" + username + bank;
            List<CheckFullInventory_Temp_> list = this.db.Database.SqlQuery<CheckFullInventory_Temp_>(sql, new object[0]).ToList<CheckFullInventory_Temp_>();
            return list;
        }
        public List<Others> GetIndexListSecond(string username, string bank)
        {
            string sql = "select Ledger,Material,Fnumber,MeasureUnit, Material,convert(varchar(max),WIP_Qty) as WIP_Qty,convert(varchar(max),MRB_Qty) as MRB_Qty,convert(varchar(max),WH_Qty) as WH_Qty,convert(varchar(max),IQC_Qty) as IQC_Qty,convert(varchar(max),OpenPO_Qty) as OpenPO_Qty,Invt_Time from CheckFullInventory_Temp_" + username + bank;
            List<Others> list = this.db.Database.SqlQuery<Others>(sql, new object[0]).ToList<Others>();
            return list;
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
                DBNull sql = this.db.Database.SqlQuery<DBNull>("CREATE TABLE dbo.CheckFullInventory_Temp_" + username + "([LPSerial] [int] primary key identity(1,1) NOT NULL, [Ledger] [char](2) NOT NULL,[Fnumber][nvarchar](20) NOT NULL,[Material][nvarchar](80) NOT NULL,[MeasureUnit][nvarchar](8) NULL,[WIP_Qty][decimal](10,2)  NULL,[MRB_Qty][decimal](10,2) NULL,[WH_Qty][decimal](10, 2) NULL,[IQC_Qty][decimal](10,2) NULL,[OpenPO_Qty][decimal](10,2) NULL,[Invt_Time][datetime] NULL UNIQUE)", new object[]
                {
                    new SqlParameter("@username", username)
                }).SingleOrDefault<DBNull>();
                result = true;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message.ToString());
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
        public int InsertDataSql(string username, CheckFullInventory_Temp_ List)
        {
            int result;
            try
            {
                string sql = "INSERT INTO dbo.CheckFullInventory_Temp_" + username + "(Ledger, Fnumber, Material,Invt_Time) VALUES('{0}','{1}','{2}','{3}')";
                sql = string.Format(sql, new object[]
                {
                     List.Ledger,
                    List.Fnumber,
                    List.Material,
                    DateTime.Now
                });
                DBNull totalCount = this.db.Database.SqlQuery<DBNull>(sql, new object[0]).SingleOrDefault<DBNull>();
                result = 1;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message.ToString());
                result = 0;
            }
            return result;
        }
        /// <summary>
        /// 清空临时表数据SQL
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int TRUNCATETABLE(string username)
        {
            int result;
            try
            {
                this.db.Database.SqlQuery<DBNull>("TRUNCATE TABLE CheckFullInventory_Temp_" + username, new object[0]).SingleOrDefault<DBNull>();
                result = 1;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message.ToString());
                result = 0;
            }
            return result;
        }
        /// <summary>
        /// 删除当前行数据
        /// </summary>
        /// <param name="username"></param>
        /// <param name="Ledger"></param>
        /// <param name="Fnumber"></param>
        /// <returns></returns>
        public int DeleteOneData(string username, string Ledger, string Fnumber)
        {
            int result;
            try
            {
                this.db.Database.SqlQuery<DBNull>("delete  CheckFullInventory_Temp_" + username + " where Fnumber ='" + Fnumber + "' and  Ledger = '" + Ledger + "'", new object[0]).SingleOrDefault<DBNull>();
                result = 1;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message.ToString());
                result = 0;
            }
            return result;

        }
        /// <summary>
        /// 更新当前行数据
        /// </summary>
        /// <param name="username"></param>
        /// <param name="Ledger"></param>
        /// <param name="Fnumber"></param>
        /// <returns></returns>
        public int UpdateOneData(string username, string Ledger, string Fnumber)
        {
            int result;
            try
            {
                this.db.Database.SqlQuery<DBNull>("update  CheckFullInventory_Temp_" + username + " set Invt_Time = '" + DateTime.Now.ToString("G") + "' where Fnumber ='" + Fnumber + "' and  Ledger = '" + Ledger + "'", new object[0]).SingleOrDefault<DBNull>();
                result = 1;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message.ToString());
                result = 0;
            }
            return result;

        }
        /// <summary>
        /// 根据条件表格中的账套和物料号查询是否已存在数据表格当中
        /// </summary>
        /// <param name="username"></param>
        /// <param name="Ledger"></param>
        /// <param name="Fnumber"></param>
        /// <returns></returns>
        public int SelectLedgerAndFnumberDataSQL(string username, string Ledger, string Fnumber)
        {
            try
            {
                var result = this.db.Database.SqlQuery<int>("select count(*) from   CheckFullInventory_Temp_" + username + "  where Fnumber ='" + Fnumber + "' and  Ledger = '" + Ledger + "'", new object[0]).SingleOrDefault<int>();
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message.ToString());
                return 0;
            }
        }
        /// <summary>
        /// 执行采集数据
        /// </summary>
        /// <param name="Ledger"></param>
        /// <param name="Fnumber"></param>
        /// <returns></returns>
        public int WholetablecaptureSQL(string username, string bank)
        {
            int result;
            string sql1 = "";
            try
            {
                List<CheckFullInventory_Temp_> check = GetIndexList(username, bank);//查询主表数据
                string tableName = "";
                foreach (var flag in check)
                {
                    if (flag.Ledger == "LK")
                    {
                        tableName = "AIS20181011094554";
                    }
                    if (flag.Ledger == "HL")
                    {
                        tableName = "AIS20151013110946";
                    }
                    if (flag.Ledger == "YH")
                    {
                        tableName = "AIS20170316112450";
                    }
                    if (flag.Ledger == "HS")
                    {
                        tableName = "HSYH_New";
                    }
                    string sql = "execute dbo.InventoryTrackingProc '" + tableName + "','" + flag.Fnumber + "'";
                    List<Others> AllList = this.db.Database.SqlQuery<Others>(sql).ToList();

                    foreach (var item in AllList)
                    {
                        item.Ledger = flag.Ledger;
                        if (item.Fnumber == flag.Fnumber)
                        {
                            item.WIP_Qty = decimal.Round(decimal.Parse(item.WIP_Qty), 2).ToString();
                            item.MRB_Qty = decimal.Round(decimal.Parse(item.MRB_Qty), 2).ToString();
                            item.WH_Qty = decimal.Round(decimal.Parse(item.WH_Qty), 2).ToString();
                            item.IQC_Qty = decimal.Round(decimal.Parse(item.IQC_Qty), 2).ToString();
                            item.OpenPO_Qty = decimal.Round(decimal.Parse(item.OpenPO_Qty), 2).ToString();
                            sql1 = "update  CheckFullInventory_Temp_" + username + " set Invt_Time = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',MeasureUnit ='" + item.MeasureUnit + "',WIP_Qty =" + item.WIP_Qty + ",MRB_Qty = " + item.MRB_Qty + ",WH_Qty =" + item.WH_Qty + " ,IQC_Qty = " + item.IQC_Qty + ",OpenPO_Qty = " + item.OpenPO_Qty + "  where Fnumber ='" + item.Fnumber + "' and  Ledger = '" + item.Ledger + "'";
                            this.db.Database.SqlQuery<DBNull>(sql1).SingleOrDefault<DBNull>();
                            #region 第一种形式
                            var task_1 = Task.Run(async delegate
                            {
                                await Task.Delay(1000);
                            });
                            #endregion
                        }
                    }

                }
                result = 1;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message.ToString());
                string aa = ex.Message + sql1;
                result = 0;
                throw;
            }
            return result;
        }
    }
}