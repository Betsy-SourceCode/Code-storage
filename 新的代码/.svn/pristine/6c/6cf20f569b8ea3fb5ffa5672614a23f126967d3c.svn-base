using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using Customer.Models.PublicSqlMethods;
using System;
namespace Customer.Models.SqlMethods
{
    public class GetIndex
    {
        KnowledgeBaseModels db = new KnowledgeBaseModels();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Question">问题</param>
        /// <param name="KeyWord">关键字</param>
        /// <param name="Answer">答案</param>
        /// <returns></returns>
        public List<SelectKnowledgeBaseIndex> GetIndexPageRow(string Question, string KeyWord, string Answer)
        {
            //模糊查询
            List<SelectKnowledgeBaseIndex> totalCount = db.Database.SqlQuery<SelectKnowledgeBaseIndex>(@"select * from SelectKnowledgeBaseIndex q where (isnull(q.Question,'') like @Question or isnull(q.KeyWord,'') like @KeyWord or isnull(q.Answer,'') like @Answer)", new SqlParameter("@Question", "%" + Question + "%"), new SqlParameter("@KeyWord", "%" + KeyWord + "%"), new SqlParameter("@Answer", "%" + Answer + "%")).ToList();
            return totalCount;
        }
    }

}