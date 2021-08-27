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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Question">问题</param>
        /// <param name="KeyWord">关键字</param>
        /// <param name="Answer">答案</param>
        /// <returns></returns>
        public List<QnA> GetIndexPageRow(string Question, string KeyWord, string Answer)
        {
            //模糊查询
            List<QnA> totalCount = db.Database.SqlQuery<QnA>(@"select * from [dbo].[QnA] where (isnull(Question,'') like @Question or isnull(KeyWord,'') like @KeyWord or isnull(Answer,'') like @Answer", new SqlParameter("@Question", "%" + Question + "%"), new SqlParameter("@KeyWord", "%" + KeyWord + "%")).ToList();
            return totalCount;
        }
    }

}