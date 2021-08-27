using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using System;
using KYH_Customer.Models.PublicSqlMethods;

namespace KYH_Customer.Models.SqlMethods
{
    public class NEWSelectIndex
    {
        public SelectIndexs list;
        public int dep;
    }
    public class GetDefault
    {
        public int InfoPermission(string login_username, string login_Dept, string login_Position, string createBy, string createDept)
        {
            int Record = 0;
            if (login_username != "")
            {
                if (login_username == "ADM")
                    Record = 4;
                else if (login_username == createBy)
                    Record = 4;
                else if (login_username == "GUS")
                    Record = 2;
                else if (createDept.Contains(login_Dept) || (createBy == "" && createDept == ""))
                    if (login_Position == "M")
                        Record = 4;
                    else
                        Record = 3;
                else
                    Record = 1;
            }

            return Record;
        }
        WebStationEntities db = new WebStationEntities();
        /// <summary>
        /// 首页显示Sql
        /// </summary>
        /// <param name="ShortName"></param>
        /// <param name="Name_CN"></param>
        /// <param name="K3Code"></param>
        /// <returns></returns>
        public List<NEWSelectIndex> GetDefaultSql(List<SelectIndexs> list, string username, string DeptIDs, string Position)
        {
            List<NEWSelectIndex> newlist = new List<NEWSelectIndex>();
            foreach (var item in list)
            {
                newlist.Add(new NEWSelectIndex { list = item, dep = InfoPermission(username, DeptIDs, Position, item.CreateBy, item.CreateDept) });
            }
            return newlist;
        }

        /// <summary>
        /// 查询首页列表总行数
        /// </summary>
        /// <param name="ShortName"></param>
        /// <param name="Name_CN"></param>
        /// <param name="K3Code"></param>
        /// <returns></returns>
        public List<SelectIndexs> GetDefaultPageRow(string ShortName, string Name_CN, string K3Code, string username, string DeptIDs, string rank, int shenhe)
        {
            List<SelectIndexs> totalCount = db.Database.SqlQuery<SelectIndexs>(@"select * from SelectIndexs WHERE (ShortName +'|'+ isnull(FullName_CN,'')+'|'+ isnull(FullName_EN,'')) like @ShortName and isnull(Classify,'') like @Name_CN and isnull(K3Code,'') like @K3Code ORDER BY " + rank, new SqlParameter("@ShortName", "%" + ShortName + "%"), new SqlParameter("@Name_CN", "%" + Name_CN + "%"), new SqlParameter("@K3Code", "%" + K3Code + "%")).ToList();
            if (shenhe==0)
            {
                totalCount = totalCount.Where(p => p.Status == "有效" || ((!string.IsNullOrEmpty(DeptIDs) && p.CreateDept.StartsWith(DeptIDs)) || p.CreateBy == username)).ToList();
            }
            return totalCount;
        }

        /// <summary>
        ///  禁用/解禁功能-将客户表状态改为S/N,更新修改人，修改部门和时间-sql语句
        /// </summary>
        /// <param name="CustomerId">客户ID</param>
        /// <returns></returns>
        public int GetUpdateJY(int CustomerId, string UpdateBy, string UpdateDept, string Status)
        {
            int i = 0;
            try
            {
                DateTime UpdateTime = DateTime.Now;
                string Updatesql = @"update Customer set Status=@Status,UpdateBy=@UpdateBy,UpdateDept=@UpdateDept,UpdateTime=@UpdateTime where CustomerId=@CustomerId";
                SqlParameter[] para = {
                       new SqlParameter("@Status ", Status),
                       new SqlParameter("@CustomerId ", CustomerId),
                       new SqlParameter("@UpdateBy ",UpdateBy),
                       new SqlParameter("@UpdateDept ",UpdateDept),
                       new SqlParameter("@UpdateTime ",UpdateTime)};
                i = db.Database.ExecuteSqlCommand(Updatesql, para);
                return i;
            }
            catch (System.Exception ex)
            {
                LogHelper.Write(ex.ToString());  //日志
                return i;
            }

        }
    }
}