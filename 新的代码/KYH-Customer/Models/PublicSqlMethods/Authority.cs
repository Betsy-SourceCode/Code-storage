using KYH_Customer.Models.ViewDataModel;
using System.Data.SqlClient;
using System.Linq;

namespace KYH_Customer.Models.PublicSqlMethods
{
    // Token: 0x02000007 RID: 7
    public class Authority
	{
		/// <summary>
		/// 获得登陆人姓名
		/// </summary>
		/// <param name="userid"></param>
		/// <returns></returns>
		public Others GetUsernameSql(string userid)
		{
			return this.db.Database.SqlQuery<Others>("select UserName AS Text From Users where UserID=@userid", new object[]
			{
				new SqlParameter("@userid", userid)
			}).FirstOrDefault<Others>();
		}

		/// <summary>
		/// 查询权限
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

		/// <summary>
		/// 获得登陆人部门信息
		/// </summary>
		/// <param name="EmpID"></param>
		/// <returns></returns>
		public Others GetUserSql(string EmpID)
		{
			return this.db.Database.SqlQuery<Others>("select UpperDept+d.DeptID Text from Department d,Employee e where d.deptid=e.deptid and EmpID=@EmpID", new object[]
			{
				new SqlParameter("@EmpID", EmpID)
			}).FirstOrDefault<Others>();
		}

		/// <summary>
		/// 获得登陆人职位信息
		/// </summary>
		/// <param name="EmpID"></param>
		/// <returns></returns>
		public Others GetPositionSql(string EmpID)
		{
			return this.db.Database.SqlQuery<Others>("select Position AS Text from dbo.Employee where EmpID=@EmpID", new object[]
			{
				new SqlParameter("@EmpID", EmpID)
			}).FirstOrDefault<Others>();
		}

		/// <summary>
		/// 是否有审核权限
		/// </summary>
		/// <param name="userid"></param>
		/// <returns></returns>
		public Others GetShenPSql(string userid)
		{
			return this.db.Database.SqlQuery<Others>("SELECT COUNT(1) AS id  FROM dbo.SYS_Permission P\r\n            LEFT JOIN dbo.SYS_PermissionList PL ON P.PLId = PL.PLId  WHERE P.UserID=@userid AND PL.PermissionCode='Customer_E'", new object[]
			{
				new SqlParameter("@userid", userid)
			}).FirstOrDefault<Others>();
		}

		/// <summary>
		/// /是否有禁用/解禁权限
		/// </summary>
		/// <param name="userid"></param>
		/// <returns></returns>
		public Others GetJinYongSql(string userid)
		{
			return this.db.Database.SqlQuery<Others>("SELECT COUNT(1) AS id  FROM dbo.SYS_Permission P\r\n            LEFT JOIN dbo.SYS_PermissionList PL ON P.PLId = PL.PLId  WHERE P.UserID=@userid AND PL.PermissionCode='Customer_CD'", new object[]
			{
				new SqlParameter("@userid", userid)
			}).FirstOrDefault<Others>();
		}

		/// <summary>
		/// 获得登录名字和所属部门id
		/// </summary>
		/// <param name="EmpID"></param>
		/// <returns></returns>
		public Others GetDeptID(string EmpID)
		{
			return this.db.Database.SqlQuery<Others>("select AliasName AS Value,DeptID AS Text from dbo.Employee where EmpID=@EmpID", new object[]
			{
				new SqlParameter("@EmpID", EmpID)
			}).FirstOrDefault<Others>();
		}

		
		public string SelectDeptID(string EmpID)
		{
			if (EmpID == "")
			{
				return null;
			}
			return this.GetDeptID(EmpID).Text.ToString();
		}

		
		public string SelectEName(string EmpID)
		{
			if (EmpID == "")
			{
				return null;
			}
			return this.GetDeptID(EmpID).Value.ToString();
		}

		/// <summary>
		/// 查询数据创建（修改）人信息
		/// </summary>
		/// <param name="num"></param>
		/// <param name="canshu"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public string SelectCreateUpdateDept(int num, string canshu, int id)
		{
			Others list = this.GetCreateUpdateDept(canshu, id);
			if (num == 1)
			{
				return list.Value.ToString();//英文名+姓
			}
			return list.Text.ToString(); //所在部门中文信息
		}

		/// <summary>
		/// 查询数据创建人信息SQL
		/// </summary>
		/// <param name="canshu"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public Others GetCreateUpdateDept(string canshu, int id)
		{
			return this.db.Database.SqlQuery<Others>(string.Concat(new object[]
			{
				"SELECT dbo.Employee.AliasName+dbo.Employee.LastName AS Value, dbo.Department.DeptName_CN  AS Text,dbo.Department.DeptName_EN FROM  dbo.Employee INNER JOIN dbo.QnA ON dbo.Employee.EmpID = dbo.QnA.",
				canshu,
				"   INNER JOIN  dbo.Department ON dbo.Employee.DeptID = dbo.Department.DeptID where qna.qnaid=",
				id
			}), new object[0]).FirstOrDefault<Others>();
		}

		// Token: 0x04000009 RID: 9
		private WebStationEntities db = new WebStationEntities();
	}
}
