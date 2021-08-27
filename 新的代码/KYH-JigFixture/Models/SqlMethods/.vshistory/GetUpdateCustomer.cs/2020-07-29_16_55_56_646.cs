using System.Data.SqlClient;
using System.Linq;
using Customer.Models.ViewDataModel;
using System.Collections.Generic;

namespace Customer.Models.SqlMethods
{
    public class GetUpdateCustomer
    {
        CustomerEntities db = new CustomerEntities();
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
        /// <summary>
        /// 修改客户信用申请表第四页签表格
        /// </summary>
        /// <param name="ShortName"></param>
        /// <param name="Name_CN"></param>
        /// <param name="K3Code"></param>
        /// <returns></returns>
        public List<Contacts> GetUpdateCustomerTable(int CustomerID, int SysID)
        {
            List<Contacts> list = db.Database.SqlQuery<Contacts>(@"select tb.Name_CN ContactType,cs.Remark Remark,cs.City City,cs.Contact Contact,cs.* from TBWords tb left join Contacts cs on tb.Value=cs.ContactType  left join Customer c on cs.SourceID=c.SysID where WordCode='ct'  and c.SysID=@sysId  order by cs.CreateTime desc", new SqlParameter("@sysId", SysID)).ToList();
            return list;
        }
    }
}