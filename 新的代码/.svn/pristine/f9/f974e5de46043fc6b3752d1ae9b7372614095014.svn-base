using System;
using System.Collections.Generic;
using System.Linq;

namespace KYH_ProductPrice.Models.SqlStatement
{
	// Token: 0x0200000E RID: 14
	public class PublicSqlMethodsSql
	{
		/// <summary>
		/// 获得详情页面列表SQL(带条件)
		/// </summary>
		/// <param name="CreateBy"></param>
		/// <param name="CustProd"></param>
		/// <param name="Customer"></param>
		/// <param name="CreateTime"></param>
		/// <param name="EndTime"></param>
		/// <param name="Remarks_MD"></param>
		/// <param name="Cancel"></param>
		/// <param name="username"></param>
		/// <param name="login_Dept"></param>
		/// <returns></returns>
		public List<GetProductPriceList> GetDetailsListSql(string CreateBy, string CustProd, string Customer, DateTime CreateTime, string EndTime, string Remarks_MD, bool Cancel, string username, string login_Dept)
		{
			List<GetProductPriceList> result;
			try
			{
				string sql = "SELECT * FROM GetProductPriceList WHERE CreateBy LIKE '%{0}%'\r\n                    AND(FNumber LIKE '%{1}%' OR CustProdCode LIKE '%{1}%' OR CustProdName LIKE '%{1}%') AND CustomerDisplayName  LIKE '%{2}%' AND (CreateTime >= '{3}' AND CONVERT(NVARCHAR(10),CreateTime,23) <= '{4}') AND  ISNULL(Remarks_MD,'')  LIKE '%{5}%'";
				if (login_Dept != null)
				{
					sql = string.Concat(new string[]
					{
						sql,
						"  AND CreateDept+UpperDept LIKE '%",
						login_Dept,
						"%'  or   CreateBy='",
						username,
						"'"
					});
				}
				else if (username != null)
				{
					sql = sql + "  AND CreateBy='" + username + "'";
				}
				if (Cancel)
				{
					sql += "  AND Cancel=1";
				}
				string Ordersql = "  Order by CreateTime DESC";
				sql = string.Format(sql + Ordersql, new object[]
				{
					CreateBy,
					CustProd,
					Customer,
					CreateTime,
					EndTime,
					Remarks_MD
				});
				List<GetProductPriceList> list = this.db.Database.SqlQuery<GetProductPriceList>(sql, new object[0]).ToList<GetProductPriceList>();
				result = list;
			}
			catch (Exception ex)
			{
				throw;
			}
			return result;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00002D08 File Offset: 0x00000F08
		public List<GetProductPriceList> GetDetailsListSql(int CPSerial)
		{
			string sql = "SELECT * FROM GetProductPriceList list WHERE list.CPSerial=" + CPSerial.ToString();
			return this.db.Database.SqlQuery<GetProductPriceList>(sql, new object[0]).ToList<GetProductPriceList>();
		}

		// Token: 0x0400007A RID: 122
		private WebStationEntities db = new WebStationEntities();
	}
}
