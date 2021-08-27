using System;
using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace KYH_KnowledgeBase.Models.PublicSqlMethods
{
    public class LogThread
    {
        WebStationEntitiess db = new WebStationEntitiess();
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

        public static string GetLocalIP()
        {
            string result;
            try
            {
                IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        IpEntry.AddressList[i].ToString();
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                result = "";
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        public static void ActionLog(string UserID, string DeptID, string canshu, string ActionType, string name)
        {
            new Thread(new ThreadStart(new LogThread
            {
                log = new ActionLog2021
                {
                    UserID = UserID,
                    DeptID = DeptID,
                    FunctionID = 136,
                    ActionType = ActionType,
                    TableName = "QnA",
                    TableKey = "QnAID",
                    Description = name + canshu
                }
            }.GetActionLogAdd)).Start();
        }
    }
}