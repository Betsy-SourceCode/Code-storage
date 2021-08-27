using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Customer.Models.SqlMethods
{
    public class GetIndex
    {
        //KnowledgeBaseModelsContainer db = new KnowledgeBaseModelsContainer();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Question">问题</param>
        /// <param name="KeyWord">关键字</param>
        /// <param name="Answer">答案</param>
        /// <returns></returns>
        public List<SelectKnowledgeBaseIndex> GetIndexPageRow(string KeyWord, string TopicArea)
        {
            //模糊查询
            string queryIndexSql = "select * from SelectKnowledgeBaseIndex q where ((isnull(q.Question,'')+isnull(q.KeyWord,'')+isnull(q.Answer,'') like '%{0}%')) and q.TopicArea not in('{1}')";
            //queryIndexSql = string.Format(queryIndexSql, Question, KeyWord, Answer, TopicArea);
            //List<SelectKnowledgeBaseIndex> totalCount = db.Database.SqlQuery<SelectKnowledgeBaseIndex>(@"select * from SelectKnowledgeBaseIndex q where (isnull(q.Question,'') like @Question or isnull(q.KeyWord,'') like @KeyWord or isnull(q.Answer,'') like @Answer) and q.TopicArea not in(@TopicArea)", new SqlParameter("@Question", "%" + Question + "%"), new SqlParameter("@KeyWord", "%" + KeyWord + "%"), new SqlParameter("@Answer", "%" + Answer + "%"), new SqlParameter("@TopicArea", TopicArea)).ToList();
            queryIndexSql = string.Format(queryIndexSql, KeyWord, TopicArea);
            List<SelectKnowledgeBaseIndex> totalCount = db.Database.SqlQuery<SelectKnowledgeBaseIndex>(queryIndexSql).ToList();
            return totalCount;
        }
    }

}