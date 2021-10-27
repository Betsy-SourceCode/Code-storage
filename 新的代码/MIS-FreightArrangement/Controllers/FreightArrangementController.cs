using MIS_FreightArrangement.Models;
using MIS_FreightArrangement.Models.PublicSqlMethods;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MIS_FreightArrangement.Controllers
{
    public class FreightArrangementController : Controller
    {
        WebStationEntitiess db = new WebStationEntitiess();
        // GET: FreightArrangement
        public ActionResult Index(string userid)
        {
            userid = "444";
            return View();
        }
        /// <summary>
        /// 查询首页列表
        /// </summary>
        /// <returns></returns>
        public string IndexData(int pageIndex = 1, int pageSize = 15)
        {
            try
            {
                List<FreightArrangementIndex_Result> data = new GetIndex().GetIndexListSql();
                var rowCount = data.Count; //总条数
                data = data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList<FreightArrangementIndex_Result>();//分页
                ResponseJson json = new ResponseJson
                {
                    Data = data,
                    count = rowCount
                };
                return JsonConvert.SerializeObject(json);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());
                throw;
            }

        }
        /// <summary>
        /// 走货方式(TW)/走货地点(LP)下拉框
        /// </summary>
        /// <returns></returns>
        public string SelectDropDownList(string WordCode)
        {
            try
            {
                List<Others> trade = new GetIndex().GetDropDownList(WordCode);
                ResponseJson json = new ResponseJson
                {
                    Data = trade
                };
                return JsonConvert.SerializeObject(json);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}