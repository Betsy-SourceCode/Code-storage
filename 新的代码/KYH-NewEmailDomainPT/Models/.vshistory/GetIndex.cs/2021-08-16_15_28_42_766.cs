using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KYH_NewEmailDomainPT.Models
{
    public class GetIndex
    {
        WebStationEntities db = new WebStationEntities();
        /// <summary>
        /// 查询首页列表SQL
        /// </summary>
        /// <param name="list"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public List<NewEmailDomain2GIP_Select> GetIndexListSql(NewEmailDomain2GIP_Select list, string EndTime, string orderby)
        {
            string sql = @"select * from [dbo].[NewEmailDomain2GIP_Select]
            where DomainName like '%{0}%' and FindDate>= '{1}' and CONVERT(NVARCHAR(10),FindDate,23)<= '{2}'";
            sql = sql + orderby;
            sql = string.Format(sql, list.DomainName, list.FindDate, EndTime);
            List<NewEmailDomain2GIP_Select> NewEmailDomainList = db.Database.SqlQuery<NewEmailDomain2GIP_Select>(sql).ToList();
            return NewEmailDomainList;
        }
    }
}