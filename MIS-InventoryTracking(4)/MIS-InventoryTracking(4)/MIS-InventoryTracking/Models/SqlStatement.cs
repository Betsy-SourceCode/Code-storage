using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        public List<CheckFullInventory_Temp_> GetIndexList(string username,string bank)
        {
            string sql = "select * from CheckFullInventory_Temp_" + username + " order by "+bank;
            List<CheckFullInventory_Temp_> list = this.db.Database.SqlQuery<CheckFullInventory_Temp_>(sql, new object[0]).ToList<CheckFullInventory_Temp_>();
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
                string aa = ex.Message;
                //LogHelper.Write(ex.ToString());
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
            catch (Exception)
            {
                result = 0;
            }
            return result;
        }
    }
}