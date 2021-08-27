using System.Data.SqlClient;
using System.Linq;
using System.Collections.Generic;
using System;
using KYH_Customer.Models.PublicSqlMethods;
using KYH_Customer.Models.ViewDataModel;

namespace KYH_Customer.Models.SqlMethods
{
    public class GetUpdateCustomer
    {
        WebStationEntities db = new WebStationEntities();
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
        /// 修改客户信用申请表第四页签表格显示
        /// </summary>
        /// <param name="ShortName"></param>
        /// <param name="Name_CN"></param>
        /// <param name="K3Code"></param>
        /// <returns></returns>
        public List<Contacts> GetUpdateCustomerTable(int CustomerID, int SysID)
        {
            List<Contacts> list = db.Database.SqlQuery<Contacts>(@"select tb.Name_CN ContactType,cs.Remark Remark,cs.City City,cs.Contact Contact,CASE cs.Share WHEN 'N' THEN '否' WHEN 'A' THEN '是' END AS Share,cs.* from TBWords tb left join Contacts cs on tb.Value=cs.ContactType  left join Customer c on cs.SourceID=c.SysID where WordCode='ct'  and c.SysID=@sysId  order by cs.CreateTime desc", new SqlParameter("@sysId", SysID)).ToList();
            return list;
        }

        #region 增删改
        /// <summary>
        /// 修改页面联系表格新增-第四页签
        /// </summary>
        /// <returns></returns>
        public int GetcustomerUpdateAdd(Contacts Con)
        {
            int i = 0;
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
            catch (System.Exception ex)
            {
                LogHelper.Write(ex.ToString());  //日志
                return i;
            }
            return i;
        }
        /// <summary>
        ///  修改后将客户表状态改为P,更新修改人，修改部门和时间-sql语句
        /// </summary>
        /// <param name="CustomerId">客户ID</param>
        /// <returns></returns>
        public int GetUpdateSJ(int CustomerId, string UpdateBy, string UpdateDept)
        {
            int i = 0;
            try
            {
                DateTime UpdateTime = DateTime.Now;
                string Updatesql = @"update Customer set Status='P',UpdateBy=@UpdateBy,UpdateDept=@UpdateDept,UpdateTime=@UpdateTime where CustomerId=@CustomerId";
                SqlParameter[] para = {
                       new SqlParameter("@CustomerId ", CustomerId),
                       new SqlParameter("@UpdateBy ",UpdateBy),
                       new SqlParameter("@UpdateDept ",UpdateDept),
                       new SqlParameter("@UpdateTime ",UpdateTime)};
                i = db.Database.ExecuteSqlCommand(Updatesql, para);
                return i;
            }
            catch (System.Exception ex)
            {
                LogHelper.Write(ex.ToString());  //日志
                return i;
            }

        }
        /// <summary>
        /// 根据类型名获得类型编号Sql
        /// </summary>
        /// <param name="TypeName"></param>
        /// <returns></returns>
        public string GetContactType(string TypeName)
        {
            try
            {
                var TypeId = db.Database.SqlQuery<Others>(@"select t.Value from TBWords t where t.WordCode='ct' and t.Name_CN=@TypeName", new SqlParameter("@TypeName", TypeName)).FirstOrDefault();
                if (TypeId == null)
                {
                    return "";
                }
                return TypeId.Value;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());  //日志
                return "";

            }
        }
        #endregion
    }
}