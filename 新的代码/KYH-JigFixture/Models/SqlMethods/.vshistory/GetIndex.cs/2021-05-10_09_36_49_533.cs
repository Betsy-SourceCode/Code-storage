using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using System;
using KYH_JigFixture.Models;

namespace Customer.Models.SqlMethods
{
    public class GetIndex
    {
        C6Entities c6db = new C6Entities();
        WebStationEntities Webdb = new WebStationEntities();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Question">OA申请单号</param>
        /// <param name="Question">申请日期</param>
        /// <param name="KeyWord">物料</param>
        /// <returns></returns>
        public List<SelectOA_Result> GetIndexPageRow(int Num)
        {
            if (Num == 0)
            {
                //模糊查询
                string queryIndexSql1 = "exec SelectOA'{0}'";
            }
            else
            {
                //模糊查询
                string queryIndexSql2 = "select * from  WebStation.dbo.Tools_Acquire_Trace";
            }
            queryIndexSql = string.Format(queryIndexSql);
            //List<SelectOA_Result> totalCount = db.Database.SqlQuery<SelectOA_Result>(@"select * from SelectKnowledgeBaseIndex q where (isnull(q.Question,'') like @Question or isnull(q.KeyWord,'') like @KeyWord or isnull(q.Answer,'') like @Answer) and q.TopicArea not in(@TopicArea)", new SqlParameter("@Question", "%" + Question + "%"), new SqlParameter("@KeyWord", "%" + KeyWord + "%"), new SqlParameter("@Answer", "%" + Answer + "%"), new SqlParameter("@TopicArea", TopicArea)).ToList();
            //queryIndexSql = string.Format(queryIndexSql);
            List<SelectKnowledgeBaseIndex> totalCount = db.Database.SqlQuery<SelectKnowledgeBaseIndex>(queryIndexSql).ToList();
            return totalCount;
        }
    }

}