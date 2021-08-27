using System.Data.SqlClient;
using System.Linq;
using KYH_Customer.Models.ViewDataModel;

namespace KYH_Customer.Models.PublicSqlMethods
{
    public class Other
	{
		/// <summary>
		/// 查询所属省份
		/// </summary>
		/// <param name="cityid">城市ID</param>
		/// <returns></returns>
		public Others GetProvinceId(string cityid)
		{
			return this.db.Database.SqlQuery<Others>("select Province AS id from City where cityid=@cityid", new object[]
			{
				new SqlParameter("@cityid", cityid)
			}).FirstOrDefault<Others>();
		}
		/// <summary>
		/// 查询所属公司ID
		/// </summary>
		/// <param name="ShortName">公司简称</param>
		/// <returns></returns>
		public Others GetCompanyID(string ShortName)
		{
			return this.db.Database.SqlQuery<Others>("select top 1 CompanyID AS id from company where TableName='Customer' and ShortName=@ShortName  order by CompanyID desc", new object[]
			{
				new SqlParameter("@ShortName", ShortName)
			}).FirstOrDefault<Others>();
		}
		/// <summary>
		/// 查询员工所属部门
		/// </summary>
		/// <param name="EmpID">员工ID</param>
		/// <returns></returns>
		public Others GetDeptID(string EmpID)
		{
			return this.db.Database.SqlQuery<Others>("select DeptID AS Text from dbo.Employee where EmpID=@EmpID", new object[]
			{
				new SqlParameter("@EmpID", EmpID)
			}).FirstOrDefault<Others>();
		}
		/// <summary>
		/// 查询最后个CustomerID
		/// </summary>
		/// <returns></returns>
		public Others GetLastCustomerID()
		{
			return this.db.Database.SqlQuery<Others>("select top 1 CustomerID AS id from dbo.Customer order by CustomerID desc", new object[0]).FirstOrDefault<Others>();
		}

		/// <summary>
		/// 查询所在城市
		/// </summary>
		/// <param name="Cityid">城市ID</param>
		/// <returns></returns>
		public Others GetCityID(string Cityid)
		{
			return this.db.Database.SqlQuery<Others>("select CityName_CN AS Text from City where CityID=@Cityid", new object[]
			{
				new SqlParameter("@Cityid", Cityid)
			}).FirstOrDefault<Others>();
		}

		// Token: 0x0400030B RID: 779
		private WebStationEntities db = new WebStationEntities();
	}
}
