using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Customer.Models.SqlMethods
{
    public class GetACustomer
    {
        CustomerEntities db = new CustomerEntities();

        /// <summary>
        /// 新客户信用申请表显示Sql
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public SelectDetail GetACustomerSql(int CustomerID)
        {
            SelectDetail list = db.Database.SqlQuery<SelectDetail>(@"select * from  SelectDetail c where  c.CustomerID=@CustomerID", new SqlParameter("@CustomerID", CustomerID)).FirstOrDefault();
            return list;
        }
    }
}