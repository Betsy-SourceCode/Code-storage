using System;
using System.Collections.Generic;
using System.Linq;
using KYH_ProductPrice.Models.PublicSqlMethods;

namespace KYH_ProductPrice.Models.SqlStatement
{
	// Token: 0x0200000C RID: 12
	public class AddProductPriceSql
	{
		/// <summary>
		/// 通过GIP产品号获得产品描述和客户产品代码SQL
		/// </summary>
		/// <param name="GipFnumber"></param>
		/// <param name="num"></param>
		/// <param name="Ziduan"></param>
		/// <returns></returns>
		public Others GetProductByGipFnumberSql(string GipFnumber, string num, string Ziduan)
		{
			string sql = "SELECT Fnumber,(FName+FModel) AS CustProdName," + Ziduan + "  AS CustProdCode";
			string sql2 = string.Concat(new string[]
			{
				" FROM mis.",
				num,
				".dbo.t_ICItemCore t,mis.",
				num,
				".dbo.t_ICItemCustom ic WHERE t.FItemID=ic.FItemID and FNumber='",
				GipFnumber,
				"'"
			});
			string sql3 = string.Format(sql + sql2, new object[0]);
			return this.db.Database.SqlQuery<Others>(sql3, new object[0]).FirstOrDefault<Others>();
		}

		/// <summary>
		/// 获得所有账套SQL
		/// </summary>
		/// <returns></returns>
		public List<Others> GetLedgerSql()
		{
			string sql = "SELECT value AS Value, value +  ' ' + Name_CN  AS Text  from dbo.TBWords WHERE wordcode = 'FC' order by orderby";
			return this.db.Database.SqlQuery<Others>(sql, new object[0]).ToList<Others>();
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00002B9C File Offset: 0x00000D9C
		public List<Others> GetCurrListSql()
		{
			string sql = "SELECT Icode AS Value,Icode + '(' + AliasName + ')' AS Text FROM Currency ORDER BY Orderby";
			return this.db.Database.SqlQuery<Others>(sql, new object[0]).ToList<Others>();
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00002BD0 File Offset: 0x00000DD0
		public List<Others> GetCustomerListSql(string LoginUserName)
		{
			string sql = "SELECT A.CustomerID AS ID,B.ShortName + ' (' + ISNULL(C.CountryID,'')+'-'+ISNULL(C.ICode,'') + ')' AS Text FROM Customer AS A \r\n                           LEFT OUTER JOIN Company AS B ON A.SysID = B.CompanyID\r\n                           LEFT OUTER JOIN City AS C ON B.CityID = C.CityID\r\n                           WHERE(B.InBusiness = 'N') AND(A.ServiceBy LIKE '|%" + LoginUserName + "%|')ORDER BY B.ShortName";
			return this.db.Database.SqlQuery<Others>(sql, new object[0]).ToList<Others>();
		}

		// Token: 0x04000079 RID: 121
		private WebStationEntities db = new WebStationEntities();
	}
}
