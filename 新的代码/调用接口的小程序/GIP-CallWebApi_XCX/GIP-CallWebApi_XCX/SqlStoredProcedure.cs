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
        private static readonly object locker1 = new object();//线程锁，防止并发同事操作数据库
        /// <summary>
        /// 通过日期时间获得工单号列表Sql
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static List<Receipt> GetGDnumberList(string typeName, string date)
        {
            try
            {
                lock (locker1)//加线程锁，防止并发
                {
                    string sql = "exec mis.gipwip_R1.dbo.P_MOInStockInterface '{0}','{1}'";
                    sql = string.Format(sql, typeName, date);
                    List<Receipt> FreightArrangementList = db.Database.SqlQuery<Receipt>(sql).ToList();
                    return FreightArrangementList;
                }
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
                lock (locker1)//加线程锁，防止并发
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
        public static int AddLog_ErrorData(int code, string ERPMO, string msg, DateTime dt, int count, string typeName)
        {
            try
            {
                lock (locker1)//加线程锁，防止并发
                {
                    var Error_Data = "日期：" + dt.ToString("yyyy-MM-dd") + "，工单号：" + ERPMO;
                    Log_ErrorData log = new Log_ErrorData();
                    log.Error_Data = Error_Data;
                    if (code == 1100)
                    {
                        log.Error_Reason = "自动模式下，" + typeName + "接口正确代码为：" + code + "，接口返回：" + msg + ",实际返回：" + count + "条";
                    }
                    else
                    {
                        log.Error_Reason = "自动模式下，" + typeName + "接口错误代码为：" + code + "，接口返回：" + msg;
                    }
                    if (code == 0 || code == 1) //0-程序进入登录后报错，1-程序登录报错
                    {
                        log.Error_Reason = "自动模式下，" + typeName + "接口错误代码为：" + code + "，程序返回：" + msg;
                    }
                    log.Error_Date = DateTime.Parse(DateTime.Now.ToString("G"));
                    //添加到错误日志表
                    gipwip_R1.Log_ErrorData.Add(log);
                    int i = gipwip_R1.SaveChanges();
                    return i;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
