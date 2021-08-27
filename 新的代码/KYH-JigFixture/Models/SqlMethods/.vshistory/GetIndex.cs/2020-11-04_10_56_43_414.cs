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
            List<QnA> totalCount = db.Database.SqlQuery<QnA>(@"select count(q.QnAID) from [dbo].[QnA] q").ToList();
        }
    }

}