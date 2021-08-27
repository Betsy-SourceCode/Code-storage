using System;
using System.Data.SqlClient;
using System.Linq;

namespace KYH_JigFixture.Models.SqlMethods
{
    // Token: 0x02000011 RID: 17
    public class Authority
    {
        private WebStationEntities db = new WebStationEntities();
        /// <summary>
        /// 根据员工号获得三字代码SQL
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Others GetUserSql(string userid)
        {
            return db.Database.SqlQuery<Others>("select UserName AS Text From Users where UserID=@userid", new object[]
            {
                new SqlParameter("@userid", userid)
            }).FirstOrDefault<Others>();
        }

        /// <summary>
        /// 根据三字代码获得所在部门SQL
        /// </summary>
        /// <param name="EmpID"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public Others GetDepartmentSql(string EmpID)
        {
            return this.db.Database.SqlQuery<Others>("select d.DeptID Text from Department d,Employee e where d.deptid=e.deptid and EmpID=@EmpID", new object[]
            {
                new SqlParameter("@EmpID", EmpID)
            }).FirstOrDefault<Others>();
        }

        /// <summary>
        /// 根据三字代码获得所属职位SQL
        /// </summary>
        /// <param name="EmpID"></param>
        /// <returns></returns>
        public string GetPositionSql(string EmpID)
        {
            Others list = this.db.Database.SqlQuery<Others>("select Position AS Text from dbo.Employee where EmpID=@EmpID", new object[]
            {
                new SqlParameter("@EmpID", EmpID)
            }).FirstOrDefault<Others>();
            if (list == null)
            {
                return "";
            }
            return list.Text;
        }

        /// <summary>
        /// 根据三字代码查询是否为指定人员或者管理层人员SQL
        /// </summary>
        /// <param name="EmpID"></param>
        /// <returns></returns>
        public int GetDesignatedPersonSql(string EmpID)
        {
            int result;
            try
            {
                int list = this.db.Database.SqlQuery<int>("SELECT count(1) FROM Employee A \r\n                LEFT OUTER JOIN Department B ON A.DeptID = B.DeptID WHERE ((A.DeptID = 'GF' or B.UpperDept = 'ZZCO') AND A.Position IN ('A','B','G','M','O','V')) AND A.EmpID ='" + EmpID + "'", new object[0]).FirstOrDefault<int>();
                result = list;
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        /// <summary>
        /// 根据三字代码查询员工完整名字（姓+名）SQL
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public Others GetUserNameSql(string UserName)
        {
            return this.db.Database.SqlQuery<Others>("select SurName+GivenName as Text,* from Employee e,Users u where e.EmpID=u.UserName  AND E.EMPID=@UserName", new object[]
            {
                new SqlParameter("@UserName", UserName)
            }).FirstOrDefault<Others>();
        }
        /// <summary>
        /// 查询是否有操作K3订单号的权限SQL（作废）
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Others GetK3PO_NumQXSql(string userid)
        {
            return this.db.Database.SqlQuery<Others>("SELECT COUNT(1) AS id  FROM dbo.SYS_Permission P\r\n            LEFT JOIN dbo.SYS_PermissionList PL ON P.PLId = PL.PLId  WHERE P.UserID=@userid AND PL.PermissionCode='JigFixture_E'", new object[]
            {
                new SqlParameter("@userid", userid)
            }).FirstOrDefault<Others>();
        }
        /// <summary>
        /// 查询指定权限SQL
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="PermissionCode"></param>
        /// <returns></returns>
        public Others GetQuanSql(string userid, string PermissionCode)
        {
            return this.db.Database.SqlQuery<Others>("SELECT COUNT(1) AS id  FROM dbo.SYS_Permission P\r\n            LEFT JOIN dbo.SYS_PermissionList PL ON P.PLId = PL.PLId  WHERE P.UserID=@userid AND PL.PermissionCode=@PermissionCode", new object[]
            {
                new SqlParameter("@userid", userid),
                new SqlParameter("@PermissionCode", PermissionCode)
            }).FirstOrDefault<Others>();
        }
    }
}
