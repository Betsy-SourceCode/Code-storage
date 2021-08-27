using System.Data.SqlClient;
using System.Linq;
using System.Data.Entity.Infrastructure;
using System;
using KYH_Customer.Models.PublicSqlMethods;
using KYH_Customer.Models.ViewDataModel;

namespace KYH_Customer.Models.SqlMethods
{
    public class GetDetail
    {
        WebStationEntities db = new WebStationEntities();
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
            AuditInfo list = db.Database.SqlQuery<AuditInfo>(@"select (SELECT AliasName + ' ' + LastName AS Expr1 FROM Employee AS Employee_2 WHERE EmpID =c.ACCreateBy) AS ACCreateBy,(SELECT DeptName_CN AS Expr1 FROM Department WHERE (DeptID = RIGHT(c.ACCreateDept,2)))  AS ACCreateDept,(SELECT AliasName + ' ' + LastName AS Expr1 FROM Employee AS Employee_2 WHERE EmpID =c.MDCreateBy) AS MDCreateBy,(SELECT DeptName_CN AS Expr1 FROM Department WHERE (DeptID = RIGHT(c.MDCreateDept, 2)))  AS MDCreateDept,c.* from dbo.Customer_AuditInfo c where c.CustomerId=@CustomerID", new SqlParameter("@createBy", createBy), new SqlParameter("@createDept", createDept), new SqlParameter("@CustomerID", CustomerID)).FirstOrDefault();
            return list;
        }
        /// <summary>
        /// 判断客户信息是否审核
        /// </summary>
        /// <param name="CustomerId">客户ID</param>
        /// <returns></returns>
        public int GetShenhe(int CustomerId)
        {
            int list = db.Database.SqlQuery<int>(@"SELECT COUNT(1) AS id FROM dbo.Customer_AuditInfo c  where c.CustomerId=@CustomerId", new SqlParameter("@CustomerId", CustomerId)).FirstOrDefault();
            return list;
        }

        /// <summary>
        /// 新增/修改市场部补充意见+改状态
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int GetAuditInfoAdd(Customer_AuditInfo info)
        {
            #region 开始事务
            int i = 0;
            System.Data.Common.DbConnection con = ((IObjectContextAdapter)db).ObjectContext.Connection;
            con.Open();
            using (var tran = con.BeginTransaction())
            {
                try
                {
                    //添加到Customer_AuditInfo表
                    string AuditInfosql = @"insert into dbo.Customer_AuditInfo(CustomerId,SORcvEntity,MFGProcEntity,FamilyTree,DocCheckList,SCACOpinion,MDCreateBy,MDCreateDept,MDCreateTime)  values (@CustomerId,@SORcvEntity,@MFGProcEntity,@FamilyTree,@DocCheckList,@SCACOpinion,@MDCreateBy,@MDCreateDept,@MDCreateTime)";

                    //修改到Customer_AuditInfo表
                    string AuditInfoupsql = @"update dbo.Customer_AuditInfo set SORcvEntity=@SORcvEntity,MFGProcEntity=@MFGProcEntity,
                    FamilyTree=@FamilyTree,DocCheckList=@DocCheckList,SCACOpinion=@SCACOpinion,MDCreateBy=@MDCreateBy,
                    MDCreateDept=@MDCreateDept,MDCreateTime=@MDCreateTime where CustomerId=@CustomerId";


                    string Updatesql = @"update Customer set Status='A' where CustomerId=@CustomerId";
                    SqlParameter[] para1 = {
                        new SqlParameter("@CustomerId ",info.CustomerId),
                        new SqlParameter("@SORcvEntity ",info.SORcvEntity),
                        new SqlParameter("@MFGProcEntity ",info.MFGProcEntity),
                        new SqlParameter("@FamilyTree ",info.FamilyTree),
                        new SqlParameter("@DocCheckList ",info.DocCheckList),
                        new SqlParameter("@SCACOpinion ",info.SCACOpinion),
                        new SqlParameter("@MDCreateBy ",info.MDCreateBy),
                        new SqlParameter("@MDCreateDept ",info.MDCreateDept),
                        new SqlParameter("@MDCreateTime ",info.MDCreateTime)
                    };
                    if (GetShenhe(info.CustomerId) == 1)    //是否提交过数据
                    {
                        i = db.Database.ExecuteSqlCommand(AuditInfoupsql, para1);
                    }
                    else
                    {
                        i = db.Database.ExecuteSqlCommand(AuditInfosql, para1);
                    }
                    if (i < 0)
                    {
                        tran.Rollback();
                        return i;
                    }
                    SqlParameter[] para2 = {
                        new SqlParameter("@CustomerId ",info.CustomerId)
                    };
                    i = db.Database.ExecuteSqlCommand(Updatesql, para2);
                    if (i < 0)
                    {
                        tran.Rollback();
                        return i;
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    LogHelper.Write(ex.ToString());  //日志
                    return i;
                }
                con.Close();
                #endregion 结束事务
                return i;
            }


        }
        /// <summary>
        /// 修改补充意见+改状态
        /// </summary>
        /// <returns></returns>
        public int GetAuditInfoUpdate(Customer_AuditInfo info, int num)
        {
            #region 开始事务
            int i = 0;
            System.Data.Common.DbConnection con = ((IObjectContextAdapter)db).ObjectContext.Connection;
            con.Open();
            using (var tran = con.BeginTransaction())
            {
                try
                {
                    //修改Customer_AuditInfo表
                    string AuditInfosql = @"update dbo.Customer_AuditInfo set CWACOpinion=@CWACOpinion,ACCreateBy=@ACCreateBy,ACCreateDept=@ACCreateDept,ACCreateTime=@ACCreateTime where CustomerId=@CustomerId";

                    string Updatesql1 = @"update Customer set Status='N' where CustomerId=@CustomerId";  //通过

                    string Updatesql2 = @"update Customer set Status='P' where CustomerId=@CustomerId";  //退回
                    SqlParameter[] para1 = {
                        new SqlParameter("@CustomerId ",info.CustomerId),
                        new SqlParameter("@CWACOpinion ",info.CWACOpinion),
                        new SqlParameter("@ACCreateBy ",info.ACCreateBy),
                        new SqlParameter("@ACCreateDept ",info.ACCreateDept),
                        new SqlParameter("@ACCreateTime ",info.ACCreateTime)
                    };
                    i = db.Database.ExecuteSqlCommand(AuditInfosql, para1);
                    if (i < 0)
                    {
                        tran.Rollback();
                        return i;
                    }
                    SqlParameter[] para2 = {
                        new SqlParameter("@CustomerId ",info.CustomerId)
                    };
                    SqlParameter[] para3 = {
                        new SqlParameter("@CustomerId ",info.CustomerId)
                    };
                    if (num == 0)   //退回
                    {
                        i = db.Database.ExecuteSqlCommand(Updatesql2, para3);
                    }
                    else
                    {
                        i = db.Database.ExecuteSqlCommand(Updatesql1, para2);
                    }
                    if (i < 0)
                    {
                        tran.Rollback();
                        return i;
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    LogHelper.Write(ex.ToString());  //日志
                    return i;
                }
            }
            con.Close();
            #endregion 结束事务
            return i;
        }
    }
}