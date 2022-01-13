using MIS_FreightArrangement.Models.PublicSqlMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIS_FreightArrangement.Models
{
    public class GetIndex
    {
        WebStationEntities db = new WebStationEntities();
        /// <summary>
        /// 查询首页列表SQL
        /// </summary>
        /// <returns></returns>
        public List<FreightArrangementIndex_Result> GetIndexListSql(string Start_Date, string End_Date, string zhfs, string zhdd, string gjz, string shdd, string zt)
        {
            try
            {
                string sql = @"exec [FreightArrangementIndex] '"+ Start_Date + "','"+ End_Date + "','" + gjz+"','"+shdd+"','"+zhdd+"','"+zhfs+"','"+zt+"'";
                List<FreightArrangementIndex_Result> FreightArrangementList = db.Database.SqlQuery<FreightArrangementIndex_Result>(sql).ToList();
                return FreightArrangementList;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());
                throw;
            }
        }
        /// <summary>
        /// 走货方式(TW)/走货地点(LP)SQL
        /// </summary>
        /// <returns></returns>
        public List<Others> GetDropDownList(string WordCode)
        {
            try
            {
                List<Others> list = this.db.Database.SqlQuery<Others>(" select Value,Name_CN as Text from TBWords where WordCode='"+ WordCode+"' order by orderby", new object[0]).ToList<Others>();
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}