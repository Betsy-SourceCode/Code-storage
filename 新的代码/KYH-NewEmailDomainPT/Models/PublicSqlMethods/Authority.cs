using System;
using System.Data.SqlClient;
using System.Linq;

namespace KYH_NewEmailDomainPT.Models.PublicSqlMethods
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
		/// 根据三字代码获得所在部门SQL
		/// </summary>
		/// <param name="EmpID"></param>
		/// <param name="num"></param>
		/// <returns></returns>
		public string GetDepartmentSql(string EmpID, int num)
		{
			Others list = this.db.Database.SqlQuery<Others>("select d.DeptID AS Text,d.DeptCode AS Value from Department d,Employee e where d.deptid=e.deptid and EmpID=@EmpID", new object[]
			{
				new SqlParameter("@EmpID", EmpID)
			}).FirstOrDefault<Others>();
			string result = "";
			if (list == null)
			{
				return "";
			}
			if (num == 0)
			{
				result = list.Text;
			}
			if (num == 1)
			{
				result = list.Value;
			}
			return result;
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
	}
}
