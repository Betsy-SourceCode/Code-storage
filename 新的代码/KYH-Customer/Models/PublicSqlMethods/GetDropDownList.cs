using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using KYH_Customer.Models.ViewDataModel;

namespace KYH_Customer.Models.PublicSqlMethods
{
    public class GetDropDownList
	{
		/// <summary>
		/// 查询出Default页面的行业下拉框绑定
		/// </summary>
		/// <returns></returns>
		public List<Others> GetTrade()
		{
			return this.db.Database.SqlQuery<Others>("select distinct Classify as Value ,Name_CN AS Text from company,TBWords t WHERE WordCode='CR' and Classify=t.Value", new object[0]).ToList<Others>();
		}

		/// <summary>
		///  /// <summary>
		/// 所有行业/公司下拉框绑定
		/// </summary>
		/// <param name="WordCode">CR-行业,BT-公司</param>
		/// <returns></returns>
		/// </summary>
		/// <param name="WordCode"></param>
		/// <returns></returns>
		public List<Others> GetTBWord(string WordCode)
		{
			return this.db.Database.SqlQuery<Others>("Select Value, Name_EN + ' (' + Name_CN + ')' as Text from TBWords where WordCode=@WordCode order by OrderBy", new object[]
			{
				new SqlParameter("@WordCode", WordCode)
			}).ToList<Others>();
		}

		/// <summary>
		/// 查询出所有国家下拉框绑定
		/// </summary>
		/// <returns></returns>
		public List<Others> GetCountry()
		{
			return this.db.Database.SqlQuery<Others>("SELECT Icode AS Value, Continent + ' - ' + CntyName_CN + ' (' + Icode + ')' AS Text FROM Country order by OrderBy desc,Continent,CntyName_s", new object[0]).ToList<Others>();
		}

		/// <summary>
		/// 二级联动-城市下拉框绑定
		/// </summary>
		/// <param name="Country"></param>
		/// <returns></returns>
		public List<Others> GetCity(string Country)
		{
			return this.db.Database.SqlQuery<Others>("SELECT CONVERT(varchar(10),cityID) AS Value,replace(B.ProvName_CN,CHAR(13)+ CHAR(10),'') + ' - ' + replace(replace(A.CityName_CN,CHAR(13)+ CHAR(10),''),'','') + ' (' + A.ICode + ')' AS  Text FROM City A LEFT OUTER JOIN Province B ON A.Province = B.ProvID  WHERE (B.CountryID =@Country) OR (A.CountryID =@Country) ORDER BY A.OrderBy DESC,Text", new object[]
			{
				new SqlParameter("@Country", Country)
			}).ToList<Others>();
		}

		/// <summary>
		/// 三级联动-城市下拉框绑定
		/// </summary>
		/// <param name="Province">州省ID</param>
		/// <returns></returns>
		public List<Others> GetLDCity(string Province)
		{
			return this.db.Database.SqlQuery<Others>("SELECT CONVERT(varchar(10),cityID) AS Value, (SELECT replace(ProvName_CN,CHAR(13)+ CHAR(10),'') FROM Province WHERE (ProvID = City.Province))  + ' - ' + replace(CityName_CN,CHAR(13)+ CHAR(10),'') + ' (' + isnull(ICode,'') + ')' AS Text FROM City WHERE (Province =@Province) ORDER BY OrderBy DESC", new object[]
			{
				new SqlParameter("@Province", Province)
			}).ToList<Others>();
		}

		/// <summary>
		/// 三级联动-州省下拉框绑定
		/// </summary>
		/// <param name="Country">国家ID</param>
		/// <returns></returns>
		public List<Others> GetProvince(string Country)
		{
			return this.db.Database.SqlQuery<Others>("SELECT CONVERT(varchar(10),provid) AS Value, \r\n            replace(ProvName_CN, CHAR(13) + CHAR(10), '') + ' (' + replace(replace(ProvName_EN, CHAR(13) + CHAR(10), ''), ' ', '') + ' - ' + CountryID + ')'\r\n            as Text  FROM Province WHERE(CountryID = @Country) ORDER BY OrderBy DESC", new object[]
			{
				new SqlParameter("@Country", Country)
			}).ToList<Others>();
		}

		/// <summary>
		/// 查询出客户性质下拉框绑定
		/// </summary>
		/// <returns></returns>
		public List<Others> GetAllClass()
		{
			return this.db.Database.SqlQuery<Others>("SELECT value AS Value, Name_CN + ' (' + Name_EN + ')'  Text FROM TBWords WHERE (WordCode = 'CM') ORDER BY OrderBy", new object[0]).ToList<Others>();
		}

		/// <summary>
		/// 查询出客户递属下拉框绑定
		/// </summary>
		/// <returns></returns>
		public List<Others> GetAllBelongs()
		{
			return this.db.Database.SqlQuery<Others>("SELECT A.DeptID AS Value, B.DeptName_CN + ' (' + A.DeptName_CN + ')' AS Text FROM Department AS A LEFT OUTER JOIN Department AS B  ON RIGHT(A.UpperDept, 2) = B.DeptID WHERE(LEN(A.UpperDept) > 1) ORDER BY LEN(A.UpperDept), A.UpperDept", new object[0]).ToList<Others>();
		}

		/// <summary>
		/// 查询出货币下拉框绑定
		/// </summary>
		/// <returns></returns>
		public List<Others> GetCurr()
		{
			return this.db.Database.SqlQuery<Others>("SELECT Icode AS Value,CurrencyName_CN + ' ' + CurrencyName_EN + ' (' + Icode + ')' as Text  FROM Currency ORDER BY Orderby", new object[0]).ToList<Others>();
		}

		/// <summary>
		/// 修改页面模态框联系类型下拉框查询
		/// </summary>
		/// <returns></returns>
		public List<Others> GetLXType()
		{
			return this.db.Database.SqlQuery<Others>("select Value,Name_CN AS Text from TBWords where WordCode='ct' order by OrderBy", new object[0]).ToList<Others>();
		}

		/// <summary>
		/// 修改页面模态框城市下拉框查询
		/// </summary>
		/// <returns></returns>
		public List<Others> GetLXCity()
		{
			return this.db.Database.SqlQuery<Others>("select replace(CityName_CN,CHAR(13)+ CHAR(10),'') AS Value,(select replace(CityName_s,CHAR(13)+ CHAR(10),'') from Country where (ICode=City.CountryID)) as Text  from City order by OrderBy desc,CountryID", new object[0]).ToList<Others>();
		}

		// Token: 0x04000307 RID: 775
		private WebStationEntities db = new WebStationEntities();
	}
}
