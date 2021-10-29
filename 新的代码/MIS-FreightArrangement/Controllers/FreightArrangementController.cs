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
        WebStationEntities db = new WebStationEntities();
        // GET: FreightArrangement
        public ActionResult Index(string userid)
        {
            //userid = "444";
            DateTime now = DateTime.Now;
            var date = now.AddDays(-30.0).ToString("yyyy-MM-dd");
            var today = now.AddDays(7.0).ToString("yyyy-MM-dd");
            ViewBag.date = date;  //传值到时间控件里
            ViewBag.today = today;  //传值到时间控件里
            if (userid == null)
            {
                userid = "";
            }
            ViewBag.userid = userid;
            string username = new Authority().GetUserSql(userid);
           ViewBag.username= username;
            return View();
        }
        /// <summary>
        /// 查询首页列表
        /// </summary>
        /// <param name="Start_Date">走货开始日期</param>
        /// <param name="End_Date">走货结束日期</param>
        /// <param name="zhfs">走货方式</param>
        /// <param name="zzdd">走货地点</param>
        /// <param name="gjz">关键字</param>
        /// <param name="shdd">收货地点</param>
        /// <param name="zt">状态</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public string IndexData(string Start_Date,string End_Date,string zhfs, string zhdd,string gjz,string shdd,string zt, int pageIndex = 1, int pageSize = 15)
        {
            try
            {
                List<FreightArrangementIndex_Result> data = new GetIndex().GetIndexListSql(Start_Date, End_Date, zhfs, zhdd, gjz, shdd, zt);
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
        /// 导出列表
        /// </summary>
        /// <param name="Start_Date">走货开始日期</param>
        /// <param name="End_Date">走货结束日期</param>
        /// <param name="zhfs">走货方式</param>
        /// <param name="zzdd">走货地点</param>
        /// <param name="gjz">关键字</param>
        /// <param name="shdd">收货地点</param>
        /// <param name="zt">状态</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public string DaoChuIndexData(string Start_Date, string End_Date, string zhfs, string zhdd, string gjz, string shdd, string zt)
        {
            try
            {
                List<FreightArrangementIndex_Result> data = new GetIndex().GetIndexListSql(Start_Date, End_Date, zhfs, zhdd, gjz, shdd, zt);
                ResponseJson json = new ResponseJson
                {
                    Data = data
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