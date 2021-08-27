using System;
using System.Collections.Generic;
using System.Linq;
using KYH_KnowledgeBase.Models.ViewDataModel;

namespace KYH_KnowledgeBase.Models.PublicSqlMethods
{
    // Token: 0x02000008 RID: 8
    public class GetBKDropDownList
	{
		/// <summary>
		/// 所有板块下拉框绑定
		/// </summary>
		/// <param name="Language"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public List<Others> GetBK(string Language, string id)
		{
			List<Others> result;
			try
			{
				if (id != null)
				{
					result = this.db.Database.SqlQuery<Others>(string.Concat(new string[]
					{
						"SELECT Value,",
						Language,
						"  AS Text FROM TBWords WHERE (WordCode = 'QT') and Value=(select TopicArea from QnA where QnA.QnAID=",
						id,
						")  ORDER BY OrderBy"
					}), new object[0]).ToList<Others>();
				}
				else
				{
					result = this.db.Database.SqlQuery<Others>("SELECT Value," + Language + "  AS Text FROM TBWords WHERE (WordCode = 'QT') ORDER BY OrderBy", new object[0]).ToList<Others>();
				}
			}
			catch (Exception ex)
			{
				LogHelper.Write(ex.ToString());
				result = null;
			}
			return result;
		}

		// Token: 0x0400000A RID: 10
		private WebStationEntitiess db = new WebStationEntitiess();
	}
}
