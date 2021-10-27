using System;
using System.Collections.Generic;
using System.Linq;
using KYH_ProductPrice.Models.PublicSqlMethods;

namespace KYH_ProductPrice.Models.SqlStatement
{
	// Token: 0x02000010 RID: 16
	public class UpdateProductPrice
	{
		/// <summary>
		/// 查询生效类型列表SQL
		/// </summary>
		/// <returns></returns>
		public List<Others> GetEffTypeListSql()
		{
			string sql = "Select Value,Name_EN AS Text from dbo.TBWords where wordcode = 'EF' order by orderby";
			return this.db.Database.SqlQuery<Others>(sql, new object[0]).ToList<Others>();
		}

		// Token: 0x0400007C RID: 124
		private WebStationEntities db = new WebStationEntities();
	}
}
