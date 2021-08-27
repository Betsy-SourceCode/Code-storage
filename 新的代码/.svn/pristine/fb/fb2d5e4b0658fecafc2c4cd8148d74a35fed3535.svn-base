using System.Data.SqlClient;
using System.Linq;
using Customer.Models.ViewDataModel;
using System.Collections.Generic;

namespace Customer.Models.SqlMethods
{
    public class GetDetail
    {
        CustomerEntities db = new CustomerEntities();
        /// <summary>
        /// 客户详情页显示Sql
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public SelectDetail GetDetailSql(int CustomerID)
        {
            SelectDetail list = db.Database.SqlQuery<SelectDetail>(@"select * from  SelectDetail c where  c.CustomerID=@CustomerID", new SqlParameter("@CustomerID", CustomerID)).FirstOrDefault();
            return list;
        }
    }
}