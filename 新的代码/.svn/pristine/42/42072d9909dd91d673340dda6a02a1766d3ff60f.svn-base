using System;
using System.Data.SqlClient;

namespace KYH_Customer.Models.PublicSqlMethods
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
    }
}