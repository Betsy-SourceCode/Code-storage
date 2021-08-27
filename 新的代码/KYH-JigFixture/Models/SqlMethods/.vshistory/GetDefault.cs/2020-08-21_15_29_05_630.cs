using System.Data.SqlClient;
using System.Linq;
using Customer.Models.ViewDataModel;
using System.Collections.Generic;
using Customer.Models.PublicSqlMethods;

namespace Customer.Models.SqlMethods
{
    public class NEWSelectIndex 
    {
        public SelectIndex list;
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
        CustomerEntities db = new CustomerEntities();
        /// <summary>
        /// 首页显示Sql
        /// </summary>
        /// <param name="ShortName"></param>
        /// <param name="Name_CN"></param>
        /// <param name="K3Code"></param>
        /// <returns></returns>
        public List<NEWSelectIndex> GetDefaultSql(string ShortName, string Name_CN, string K3Code, int pageIndex, int pageSize, string Rank, string username, string DeptIDs, string Position)
        {
            
            List<SelectIndex> list = db.Database.SqlQuery<SelectIndex>(@"select * from (SELECT ROW_NUMBER() OVER(ORDER BY  "+Rank+") AS rownum,* FROM (select * from SelectIndexs) AS a  WHERE (ShortName +'|'+ isnull(FullName_CN,'')+'|'+ isnull(FullName_EN,''))  like @ShortName  and isnull(Classify,'') like @Name_CN and isnull(K3Code,'') like @K3Code ) as a  where rownum BETWEEN (@pageIndex-1)*@pageSize+1  AND @pageIndex * @pageSize order by  " + Rank, new SqlParameter("@ShortName", "%" + ShortName + "%"), new SqlParameter("@Name_CN", "%" + Name_CN + "%"), new SqlParameter("@K3Code", "%" + K3Code + "%"), new SqlParameter("@pageIndex", pageIndex), new SqlParameter("@pageSize", pageSize)).ToList();
            List<NEWSelectIndex> newlist = new List<NEWSelectIndex>();
            foreach (var item in list)
            {
                newlist.Add(new NEWSelectIndex { list = item, dep = InfoPermission(username, DeptIDs, Position, item.CreateDept, item.CreateBy)});
            }
            LogHelper.Write(list.ToString());  //日志
            return newlist;
        }
        /// <summary>
        /// 查询首页列表总行数
        /// </summary>
        /// <param name="ShortName"></param>
        /// <param name="Name_CN"></param>
        /// <param name="K3Code"></param>
        /// <returns></returns>
        public int GetDefaultPageRow(string ShortName, string Name_CN, string K3Code)
        {
            var totalCount = db.Database.SqlQuery<Others>(@"select count(1) AS id from SelectIndexs WHERE (ShortName +'|'+ isnull(FullName_CN,'')+'|'+ isnull(FullName_EN,'')) like @ShortName and isnull(Classify,'') like @Name_CN and isnull(K3Code,'') like @K3Code", new SqlParameter("@ShortName", "%" + ShortName + "%"), new SqlParameter("@Name_CN", "%" + Name_CN + "%"), new SqlParameter("@K3Code", "%" + K3Code + "%")).FirstOrDefault();
            return totalCount.id;
        }
    }
}