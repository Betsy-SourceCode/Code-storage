using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using Customer.Models.PublicSqlMethods;
using System;
namespace Customer.Models.SqlMethods
{
    public class GetIndex
    {
        CustomerEntities db = new CustomerEntities();
        public List<QnA> GetIndexPageRow(string KeyWords, string Area)
        {
            //还要加模糊查询
            List<QnA> totalCount = db.Database.SqlQuery<QnA>(@"select * from [dbo].[QnA] q").ToList();
            return totalCount;
        }

        public List<QnA> GetIndexSql(List<QnA> list, string KeyWords, string Area)
        {
            List<QnA> newlist = new List<QnA>();
            foreach (var item in list)
            {
                newlist.Add(list);
            }
            return newlist;
        }
    }

}