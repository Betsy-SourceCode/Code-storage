using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KYH_NewEmailDomainPT.Models
{
    public class GetIndex
    {
        WebStationEntities db = new WebStationEntities();
        public List<NewEmailDomain2GIP> GetIndexListSql(NewEmailDomain2GIP list, string EndTime)
        {
            string sql = @"select * from [dbo].[NewEmailDomain2GIP]
            where DomainName like '%{0}%' and FindDate>= '{1}' and CONVERT(NVARCHAR(10),FindDate,23)<= '{2}'";
            sql = string.Format(sql, list.DomainName, list.FindDate, EndTime);
            List<NewEmailDomain2GIP> List = db.Database.SqlQuery<NewEmailDomain2GIP>(sql).ToList();
            return List;
        }
    }
}