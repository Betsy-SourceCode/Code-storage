using System;
using System.Collections.Generic;
using System.Linq;
using KYH_ProductPrice.Models.PublicSqlMethods;

namespace KYH_ProductPrice.Models.SqlStatement
{
	// Token: 0x0200000F RID: 15
	public class SetupCustomerSql
	{
		/// <summary>
		/// 通过文本框条件获得员工列表SQL
		/// </summary>
		/// <param name="Name"></param>
		/// <returns></returns>
		public List<Others> GetCustomerListSql(string Name)
		{
			string sql = "SELECT A.EmpID AS Value, (A.AliasName + ' ' + A.LastName) + \r\n                        (CASE WHEN len(GivenName + SurName) > 8 THEN '' ELSE '-' + A.SurName + A.GivenName END +\r\n                        ' (' + B.DeptCode + '-' + A.EmpID + ')') AS EmpName FROM Employee A LEFT OUTER JOIN Department B ON A.DeptID = B.DeptID WHERE A.DeptID <> 'OO' AND A.Status = 'N' AND A.AliasName NOT LIKE '%test%'\r\n                        AND (A.AliasName + ' ' + A.LastName + ' ' + A.FirstName + ' ' + A.SurName + ' ' + A.GivenName LIKE '%{0}%') ORDER BY A.AliasName";
			sql = string.Format(sql, Name);
			return this.db.Database.SqlQuery<Others>(sql, new object[0]).ToList<Others>();
		}

		/// <summary>
		/// 通过员工三字代码获得未指派客户名单SQL
		/// </summary>
		/// <param name="ServiceBy"></param>
		/// <returns></returns>
		public List<Others> GetUnassignedListSql(string ServiceBy)
		{
			string sql = "SELECT A.CustomerID AS ID, ISNULL(B.ShortName, B.FullName_EN) + ' (' + ISNULL(B.CountryID, '') + '-' + ISNULL(C.ICode, '') + ')' AS Text FROM Customer AS A LEFT OUTER JOIN Company AS B ON A.SysID = B.CompanyID LEFT OUTER JOIN City AS C ON B.CityID = C.CityID  WHERE B.InBusiness = 'N' AND NOT (A.ServiceBy LIKE '%|{0}|%') OR B.FullName_EN NOT LIKE '%test%' AND A.ServiceBy IS NULL  ORDER BY B.ShortName";
			sql = string.Format(sql, ServiceBy);
			return this.db.Database.SqlQuery<Others>(sql, new object[0]).ToList<Others>();
		}

		/// <summary>
		///  通过员工三字代码获得已指派客户名单SQL
		/// </summary>
		/// <param name="ServiceBy"></param>
		/// <returns></returns>
		public List<Others> GetAssignListSql(string ServiceBy)
		{
			string sql = "SELECT A.CustomerID AS ID, ISNULL(B.ShortName, B.FullName_EN) + ' (' + ISNULL(B.CountryID, '') + '-' + ISNULL(C.ICode, '') + ')' AS Text FROM Customer AS A LEFT OUTER JOIN Company AS B ON A.SysID = B.CompanyID LEFT OUTER JOIN City AS C ON B.CityID = C.CityID WHERE B.InBusiness = 'N' AND A.ServiceBy LIKE '%|{0}|%' AND B.FullName_EN NOT LIKE '%test%' ORDER BY B.ShortName";
			sql = string.Format(sql, ServiceBy);
			return this.db.Database.SqlQuery<Others>(sql, new object[0]).ToList<Others>();
		}

		/// <summary>
		/// 是否已经是指定客户的业务员
		/// </summary>
		/// <param name="ServiceBy"></param>
		/// <param name="CustomerID"></param>
		/// <returns></returns>
		public int GetISServiceBySql(string ServiceBy, int CustomerID)
		{
			Others list = this.db.Database.SqlQuery<Others>("SELECT COUNT(1) AS ID FROM dbo.Customer WHERE ServiceBy like '%|" + ServiceBy + "|%' and CustomerID=" + CustomerID.ToString(), new object[0]).FirstOrDefault<Others>();
			return list.ID;
		}

		// Token: 0x0400007B RID: 123
		private WebStationEntities db = new WebStationEntities();
	}
}
