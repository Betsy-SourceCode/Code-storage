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
        public List<NewEmailDomain2GIP> GetIndexListSql(NewEmailDomain2GIP list, string EndTime)
        {
            string sql = @"select DNSerial,DomainName,FindDate,(CASE SourceType WHEN 0 THEN ' Normal 正常'WHEN 1 THEN 'Intrude 突如其来' WHEN 2 THEN 'Market 市场推销' WHEN 3 THEN 'Phish 钓鱼电邮' WHEN 4 THEN  'Scam 欺骗电邮' WHEN 5 THEN 'Virus 病毒木马'  END) as SourceType,Remarks from [dbo].[NewEmailDomain2GIP]
            where DomainName like '%{0}%' and FindDate>= '{1}' and CONVERT(NVARCHAR(10),FindDate,23)<= '{2}'";
            sql = string.Format(sql, list.DomainName, list.FindDate, EndTime);
            List<NewEmailDomain2GIP> NewEmailDomainList = db.Database.SqlQuery<NewEmailDomain2GIP>(sql).ToList();
            return NewEmailDomainList;
        }
    }
}