using System.Data.SqlClient;
using System.Linq;

namespace KYH_Customer.Models.SqlMethods
{
    public class GetACustomer
    {
        WebStationEntities db = new WebStationEntities();

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