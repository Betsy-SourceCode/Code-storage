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
        /// <summary>
        /// 客户详情页补充意见
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public AuditInfo GetDetailBcyjSql(int CustomerID, string createBy, string createDept)
        {
            AuditInfo list = db.Database.SqlQuery<AuditInfo>(@"select (SELECT AliasName + ' ' + LastName AS Expr1 FROM Employee AS Employee_2 WHERE EmpID =c.ACCreateBy) AS ACCreateBy,(SELECT DeptName_CN AS Expr1 FROM Department WHERE (DeptID = RIGHT(c.ACCreateDept,2)))  AS ACCreateDept,(SELECT AliasName + ' ' + LastName AS Expr1 FROM Employee AS Employee_2 WHERE EmpID =c.MDCreateBy) AS MDCreateBy,
                  (SELECT DeptName_CN AS Expr1 FROM Department WHERE (DeptID = RIGHT(c.MDCreateDept, 2)))  AS MDCreateDept,c.* from dbo.Customer_AuditInfo c where c.CustomerId=@CustomerID", new SqlParameter("@createBy", createBy), new SqlParameter("@createDept", createDept), new SqlParameter("@CustomerID", CustomerID)).FirstOrDefault();
            return list;
        }

        public Others GetShenhe(int CustomerId)
        {
            Others list = db.Database.SqlQuery<Others>(@"select CityName_CN AS Text from City where CityID=@Cityid", new SqlParameter("@Cityid", Cityid)).FirstOrDefault();
            return list;
        }
    }
}