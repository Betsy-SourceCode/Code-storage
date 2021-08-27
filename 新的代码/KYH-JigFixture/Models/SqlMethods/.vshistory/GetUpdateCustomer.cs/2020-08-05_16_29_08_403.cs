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
        /// <summary>
        /// 修改页面联系表格新增-第四页签
        /// </summary>
        /// <returns></returns>
        public int GetcustomerUpdateAdd()
        {
            try
            {
                var Contactssql = @"insert into Contacts(Source,SourceID,City,ContactType,Contact,Remark,Share,CreateBy,CreateDept,UpdateBy,UpdateDept,CreateTime,UpdateTime) values(@Source,@SourceID,@City,@ContactType,@Contact,@Remark,@Share,@CreateBy,@CreateDept,@UpdateBy,@UpdateDept,@CreateTime,@UpdateTime)";
                //添加到Contacts表
                if (Con.Contact == null)
                {
                    Con.Contact = "";
                }
                if (Con.Remark == null)
                {
                    Con.Remark = "";
                }
                SqlParameter[] para4 = {
                            new SqlParameter("@Source", Con.Source),
                            new SqlParameter("@SourceID", Con.SourceID),
                            new SqlParameter("@City", Con.City),
                            new SqlParameter("@ContactType", Con.ContactType),
                            new SqlParameter("@Contact", Con.Contact),
                            new SqlParameter("@Remark", Con.Remark),
                            new SqlParameter("@Share", Con.Share),
                            new SqlParameter("@CreateBy ",Con.CreateBy),
                            new SqlParameter("@CreateDept ",Con.CreateDept),
                            new SqlParameter("@UpdateBy ",Con.UpdateBy),
                            new SqlParameter("@UpdateDept ",Con.UpdateDept),
                            new SqlParameter("@CreateTime ",Con.CreateTime),
                            new SqlParameter("@UpdateTime ",Con.UpdateTime)
                        };
                i = db.Database.ExecuteSqlCommand(Contactssql, para4);
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}