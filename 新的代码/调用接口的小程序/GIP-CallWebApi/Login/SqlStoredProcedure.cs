using GIP_CallWebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login
{
    class SqlStoredProcedure
    {
        static WebStationEntities db = new WebStationEntities();
        static gipwip_R1Entities gipwip_R1 = new gipwip_R1Entities();
        /// <summary>
        /// 通过日期时间获得工单号列表Sql
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static List<Receipt> GetGDnumberList(string date)
        {
            try
            {
                string sql = "exec mis.gipwip_R1.dbo.P_MOInStockInterface '" + date + "'";
                List<Receipt> FreightArrangementList = db.Database.SqlQuery<Receipt>(sql).ToList();
                return FreightArrangementList;
            }
            catch (Exception ex)
            {

                return null;
                throw;
            }

        }

        /// <summary>
        /// 通过日期和工单号获得集合Sql
        /// </summary>
        /// <param name="typeid">1-factory_processes，2-factory_reworks</param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static List<DataSet> ByDateAndERPMOGetListSql(int typeid, string date, string ERPMO)
        {
            try
            {
                string sql = "";
                if (typeid == 1)
                {
                    sql = "exec mis.gipwip_R1.dbo.P_MOStationRecordInterface '{0}','{1}'";
                }
                if (typeid == 2)
                {
                    sql = "exec mis.gipwip_R1.dbo.P_MORepairRecordInterface  '{0}','{1}'";
                }
                sql = string.Format(sql, date, ERPMO);
                List<DataSet> DataSet = db.Database.SqlQuery<DataSet>(sql).ToList();
                return DataSet;
            }
            catch (Exception ex)
            {

                return null;
                throw;
            }

        }

        /// <summary>
        /// 数据新增失败，写入数据表里:记录时间和工单号
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ERPMO"></param>
        /// <param name="msg"></param>
        public static int AddLog_ErrorData(DateTime dt, string ERPMO, string msg)
        {
            var Error_Data = "错误日期：" + dt.ToString("yyyy-MM-dd") + "，工单号：" + ERPMO;
            Log_ErrorData log = new Log_ErrorData();
            log.Error_Data = Error_Data;
            log.Error_Reason = "数据格式错误,接口返回：" + msg;
            log.Error_Date = DateTime.Now;
            //添加到错误日志表
            gipwip_R1.Log_ErrorData.Add(log);
            int i = gipwip_R1.SaveChanges();
            return i;
        }
    }
}
