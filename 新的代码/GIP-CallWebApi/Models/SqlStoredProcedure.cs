using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GIP_CallWebApi.Models
{
    public class SqlStoredProcedure
    {
        static gipwip_R1Entities gipwip_R1 = new gipwip_R1Entities();
        /// <summary>
        /// 通过日期时间获得工单号列表Sql
        /// </summary>
        /// <param name="id">1-过站   2-维修</param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static List<Receipt> GetGDnumberList(int id,string date)
        {
            try
            {
                string sql="";
                if (id == 1)
                {
                    sql = "exec gipwip_R1.dbo.P_MOInStockInterface 'Station','" + date + "'";
                }
                if (id == 2)
                {
                    sql = "exec gipwip_R1.dbo.P_MOInStockInterface 'Repair','" + date + "'";
                }
                if (id == 3)
                {
                    sql = "exec gipwip_R1.dbo.P_MOInStockInterface '','" + date + "'";
                }
                List<Receipt> FreightArrangementList = gipwip_R1.Database.SqlQuery<Receipt>(sql).ToList();
                //List<Receipt> NewFreightArrangementList = new List<Receipt>();//用于装工单号下查出数据不为空的工单列表
                //foreach (var item in FreightArrangementList)
                //{
                //    List<DataSet> list = SqlStoredProcedure.ByDateAndERPMOGetListSql(2, DateTime.Parse(date).ToString("d"), item.ERPMO); //日期年月日  查询工单下是否有数据
                //    if (list.Count != 0)
                //    {
                //        NewFreightArrangementList.Add(item);
                //    }
                //}
                return FreightArrangementList;
            }
            catch (Exception ex)
            {
                string aa = ex.Message;
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
                    sql = "exec gipwip_R1.dbo.P_MOStationRecordInterface '{0}','{1}'";
                }
                if (typeid == 2)
                {
                    sql = "exec gipwip_R1.dbo.P_MORepairRecordInterface  '{0}','{1}'";
                }
                sql = string.Format(sql, date, ERPMO);
                List<DataSet> DataSet = gipwip_R1.Database.SqlQuery<DataSet>(sql).ToList();
                return DataSet;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
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
        public static int AddLog_ErrorData(int code, string ERPMO, string msg,DateTime dt,int count,int typeid)
        {
            try
            {
                var Error_Data = "日期：" + dt.ToString("yyyy-MM-dd") + "，工单号：" + ERPMO;
                Log_ErrorData log = new Log_ErrorData();
                log.Error_Data = Error_Data;
                if (typeid==1)
                {
                    if (code == 1100)
                    {
                        log.Error_Reason = "手动模式下，过站记录接口正确代码为：" + code + "，接口返回：" + msg + "，实际成功：" + count + "条";
                    }
                    else if (code == 0)
                    {
                        log.Error_Reason = "手动模式下，过站记录接口程序报错返回：" + msg;
                    }
                    else
                    {
                        log.Error_Reason = "手动模式下，过站记录接口错误代码为：" + code + "，接口返回：" + msg;
                    }
                }
                else
                {
                    if (code == 1100)
                    {
                        log.Error_Reason = "手动模式下，维修记录接口正确代码为：" + code + "，接口返回：" + msg + "，实际成功：" + count + "条";
                    }
                    else if (code == 0)
                    {
                        log.Error_Reason = "手动模式下，维修记录接口程序报错返回：" + msg;
                    }
                    else
                    {
                        log.Error_Reason = "手动模式下，维修记录接口错误代码为：" + code + "，接口返回：" + msg;
                    }
                }
                
                
                log.Error_Date = DateTime.Parse(DateTime.Now.ToString("G"));
                //添加到错误日志表
                gipwip_R1.Log_ErrorData.Add(log);
                int i = gipwip_R1.SaveChanges();
                return i;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }
        }
    }
}