using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models.SqlMethods
{
    public class GetUpdateCustomer
    {
        /// <summary>
        /// 修改客户信用申请表显示Sql
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public SelectCustomerAll GetUpdateCustomerSql(int CustomerID)
        {
            SelectCustomerAll list = db.Database.SqlQuery<SelectCustomerAll>(@"select * from SelectCustomerAll c  where  c.CustomerID=@CustomerID", new SqlParameter("@CustomerID", CustomerID)).FirstOrDefault();
            return list;
        }
    }
}