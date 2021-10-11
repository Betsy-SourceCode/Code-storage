using System;
using System.Data.SqlClient;
using System.Linq;

namespace KYH_ProductPrice.Models.PublicSqlMethods
{
	// Token: 0x02000011 RID: 17
	public class Authority
	{
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
		/// 通过三字代码判断是否为管理层
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

		// Token: 0x06000114 RID: 276 RVA: 0x00003004 File Offset: 0x00001204
		public string GetIsmanagementlayer(string EmpID)
		{
			Others list = this.db.Database.SqlQuery<Others>("SELECT d.UpperDept+d.DeptID AS Value from Department d,Employee e\r\n            where d.deptid = e.deptid and e.Position in ('A', 'B', 'G', 'M', 'O', 'V') AND e.EmpID ='" + EmpID + "'", new object[0]).FirstOrDefault<Others>();
			if (list == null)
			{
				return "";
			}
			return list.Value;
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000304C File Offset: 0x0000124C
		public int GetQuanSql(string userid, string PermissionCode)
		{
			Others list = this.db.Database.SqlQuery<Others>("SELECT COUNT(1) AS ID  FROM dbo.SYS_Permission P\r\n            LEFT JOIN dbo.SYS_PermissionList PL ON P.PLId = PL.PLId  WHERE P.UserID=@userid AND PL.PermissionCode=@PermissionCode", new object[]
			{
				new SqlParameter("@userid", userid),
				new SqlParameter("@PermissionCode", PermissionCode)
			}).FirstOrDefault<Others>();
			return list.ID;
		}

		// Token: 0x0400007D RID: 125
		private WebStationEntities db = new WebStationEntities();
	}
}
