using System;
using System.Data.SqlClient;
using System.Linq;

namespace MIS_CertificationApplication.Models
{
    // Token: 0x0200000B RID: 11
    public class Authority
    {
        private WebStationEntities db = new WebStationEntities();
        /// <summary>
        /// 根据员工号获得三字代码SQL
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetUserSql(string userid)
        {
            Others list = this.db.Database.SqlQuery<Others>("select UserName AS Text From Users where UserID=@userid", new object[]
            {
                new SqlParameter("@userid", userid)
            }).FirstOrDefault<Others>();
            if (list == null)
            {
                return "";
            }
            return list.Text;
        }

        /// <summary>
        /// 根据三字代码获得所在部门和兼任部门SQL
        /// </summary>
        /// <param name="EmpID">三字代码</param>
        /// <param name="num">0-所在部门，1-兼任部门</param>
        /// <returns></returns>
        public string GetDepartmentSql(string EmpID, int num)
        {
            try
            {
                Others list = this.db.Database.SqlQuery<Others>("select d.UpperDept + e.DeptID AS DEP,d.UpperDept + e.ActDeptID AS SDP from Department d,Employee e where d.deptid=e.deptid and EmpID=@EmpID", new SqlParameter("@EmpID", EmpID)).FirstOrDefault<Others>();
                string result = "";
                if (list == null)
                {
                    return "";
                }
                if (num == 0)
                {
                    result = list.DEP;
                }
                if (num == 1)
                {
                    result = list.SDP;
                }
                return result;
            }
            catch (Exception ex)
            {
                string Message = ex.Message;
                throw;
            }
        }

        /// <summary>
        /// 根据三字代码获得所属职位和兼任职位及代理人SQL
        /// </summary>
        /// <param name="EmpID">三字代码</param>
        /// <param name="num">0-所在职位，1-兼任职位，2-代理人</param>
        /// <returns></returns>
        public string GetPositionSql(string EmpID, int num)
        {
            try
            {
                Others list = this.db.Database.SqlQuery<Others>("select Position AS PST,ActPosition AS SPT,Deputy AS CSB from dbo.Employee where EmpID=@EmpID", new SqlParameter("@EmpID", EmpID)).FirstOrDefault();
                string result = "";
                if (list == null)
                {
                    return "";
                }
                if (num == 0)
                {
                    result = list.PST;
                }
                if (num == 1)
                {
                    result = list.SPT;
                }
                if (num == 2)
                {
                    result = list.CSB;
                }
                return result;
            }
            catch (Exception ex)
            {
                string Message = ex.Message;
                throw;
            }
        }

        /// <summary>
        /// 查询指定权限SQL
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="PermissionCode">权限名</param>
        /// <returns></returns>
        public int GetQuanSql(string userid, string PermissionCode)
        {
            Others list = this.db.Database.SqlQuery<Others>("SELECT COUNT(1) AS ID  FROM dbo.SYS_Permission P\r\n            LEFT JOIN dbo.SYS_PermissionList PL ON P.PLId = PL.PLId  WHERE P.UserID=@userid AND PL.PermissionCode=@PermissionCode", new object[]
            {
                new SqlParameter("@userid", userid),
                new SqlParameter("@PermissionCode", PermissionCode)
            }).FirstOrDefault<Others>();
            return list.ID;
        }

        /// <summary>
        /// 权限赋值
        /// </summary>
        /// <param name="userid">登录人id</param>
        /// <param name="CreateBy">数据创建人</param>
        /// <param name="CreateDept">数据创建部门</param>
        /// <returns></returns>
        public string GetRLV(string userid, string CreateBy, string CreateDept)
        {
            try
            {
                //权限:A’= 全能、‘B’= 维护、‘C’= 基本
                string RLV = "";
                //登录者三字用户帐号 
                string USR = GetUserSql(userid);
                //登录者职位身份
                string PST = GetPositionSql(USR, 0);
                //登录者所属部门
                string DEP = GetDepartmentSql(USR, 0);
                //登录者兼任职位
                string SPT = GetPositionSql(USR, 1);
                if (SPT == null)
                {
                    SPT = "";
                }
                //登录者兼任部门
                string SDP = GetDepartmentSql(USR, 1);
                if (SDP == null)
                {
                    SDP = "";
                }
                //创建者现在的代理人帐号
                string CSB = GetPositionSql(CreateBy, 2);
                if (CSB == null)
                {
                    CSB = "";
                }
                //创建者
                string CTR = CreateBy;
                //创建部门
                string CDP = CreateDept;
                if (USR == CTR || USR == CSB || USR == "ADM")
                {
                    RLV = "A";
                }
                else
                {
                    RLV = "C";
                }
                if (CDP.Contains(DEP) && (PST == "A" || PST == "B" || PST == "G" || PST == "M" || PST == "O" || PST == "V"))
                {
                    RLV = "A";
                }
                else
                {
                    RLV = "B";
                }
                if (CDP.Contains(SDP) && (SPT == "A" || SPT == "B" || SPT == "G" || SPT == "M" || SPT == "O" || SPT == "V"))
                {
                    RLV = "A";
                }
                else
                {
                    RLV = "B";
                }
                return RLV;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                return "";
            }
        }

    }
}
