using System;
using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using KYH_JigFixture.Models;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using KYH_JigFixture.Models.PublicSqlMethods;

namespace Customer.Models.PublicSqlMethods
{
    public class LogThread
    {
        WebStationEntities db = new WebStationEntities();
        public ActionLog2021 log { set; get; }
        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public void GetActionLogAdd()
        {
            int i = 0;
            try
            {
                var sql = @"INSERT INTO ActionLog" + DateTime.Now.Year + "(UserID,DeptID,FunctionID,ActionType,TableName,TableKey,Description)VALUES(@UserID,@DeptID,@FunctionID,@ActionType,@TableName,@TableKey,@Description)";
                //var sql = @"INSERT INTO ActionLog2020(UserID,DeptID,FunctionID,ActionType,TableName,TableKey,Description)VALUES(@UserID,@DeptID,@FunctionID,@ActionType,@TableName,@TableKey,@Description)";  //测试
                SqlParameter[] para = {
                new SqlParameter("@UserID",log.UserID),
                new SqlParameter("@DeptID",log.DeptID),
                new SqlParameter("@FunctionID",log.FunctionID),
                new SqlParameter("@ActionType",log.ActionType),
                new SqlParameter("@TableName",log.TableName),
                new SqlParameter("@TableKey",log.TableKey),
                 new SqlParameter("@Description",log.Description),
                };
                i = db.Database.ExecuteSqlCommand(sql, para);
            }
            catch (System.Exception ex)
            {

                LogHelper.Write(ex.ToString());  //日志

            }
        }
        /// <summary>
        /// 取本机主机ip
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIP()
        {
            try
            {

                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        string ip = "";
                        ip = IpEntry.AddressList[i].ToString();
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// 异动记录
        /// </summary>
        /// <param name="canshu"></param>
        /// <param name="ActionType"></param>
        public static void ActionLog(string UserID, string DeptID, string canshu, string ActionType, string name)
        {
            Thread th = new Thread(new ThreadStart(new LogThread
            {
                log = new ActionLog2021
                {
                    UserID = UserID,
                    DeptID = DeptID,
                    FunctionID = 144,
                    ActionType = ActionType,
                    TableName = "Tools_Acquire_Trace",
                    TableKey = "FID",
                    Description = name + canshu
                }
            }.GetActionLogAdd));
            th.Start();
        }
    }
}