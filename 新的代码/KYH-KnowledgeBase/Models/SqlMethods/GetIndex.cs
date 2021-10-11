using System.Collections.Generic;
using System.Linq;

namespace KYH_KnowledgeBase.Models.SqlMethods
{
    // Token: 0x02000005 RID: 5
    public class GetIndex
    {
        /// <summary>
        /// 页面首页显示SQL
        /// </summary>
        /// <param name="KeyWord">关键字</param>
        /// <param name="TopicArea">板块</param>
        /// <returns></returns>
        public List<SelectKnowledgeBaseIndex> GetIndexPageRow(string KeyWord, string TopicArea)
        {
            string queryIndexSql = "select * from SelectKnowledgeBaseIndex q where ((isnull(q.Question,'')+isnull(q.KeyWord,'')+isnull(q.Answer,'') like '%{0}%')) and q.TopicArea not in('{1}')";
            queryIndexSql = string.Format(queryIndexSql, KeyWord, TopicArea);
            return this.db.Database.SqlQuery<SelectKnowledgeBaseIndex>(queryIndexSql, new object[0]).ToList<SelectKnowledgeBaseIndex>();
        }

        /// <summary>
        /// 查最后一条数据的ID
        /// </summary>
        public int GetLastQnAID()
        {
            return db.Database.SqlQuery<int>("select top(1) qnAID from qnA order by qnAID desc").FirstOrDefault();
        }
        // Token: 0x04000005 RID: 5
        private WebStationEntitiess db = new WebStationEntitiess();
    }
}
