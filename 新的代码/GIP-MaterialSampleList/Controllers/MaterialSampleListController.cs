using MIS_ECN.Models;
using MIS_ECN.Models.PublicSqlMethods;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MIS_ECN.Controllers
{
    public class MaterialSampleListController : Controller
    {
        C6Entities db = new C6Entities();
        // GET: ECN
        public ActionResult Index(string userid)
        {
            if (userid == null)
            {
                userid = "";
            }
            return View();
        }

        #region API
        /// <summary>
        /// 查询首页列表
        /// </summary>
        /// <param name="NewEmailDomain"></param>
        /// <param name="EndTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public string IndexData(SelectCENDataList cen, string StartDate, string EndDate)
        {
            try
            {
                List<SelectCENDataList> data = new GetIndex().GetIndexListSql(cen, StartDate, EndDate);
                //将处理过的工作耗时放入集合
                for (int i = 0; i < data.Count; i++)
                {
                    data[i].工作耗时 = IndexWeekend(data[i].接收时间.ToString(), data[i].审批时间.ToString());
                }
                return JsonConvert.SerializeObject(data);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message.ToString());
                throw;
            }

        }

        /// <summary>
        /// 二级菜单文件路径列表
        /// </summary>
        /// <param name="MainID"></param>
        /// <returns></returns>
        public string ByMainGetDjbhName(string CERNumber)
        {
            try
            {
                //先通过流程编号找到对应的MainId
                string MainId = new GetIndex().GetMainIdSql(CERNumber);
                List<Download> data = new GetIndex().ByMainGetDjbhNameSql(MainId);
                return JsonConvert.SerializeObject(data);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message.ToString());
                throw;
            }
        }

        /// <summary>
        /// 获得下拉框列表-生产工厂
        /// </summary>
        /// <returns></returns>
        public string GetProducePlantDrop()
        {
            try
            {
                List<string> DropDownList = new GetIndex().GetProducePlantDropSql();
                return JsonConvert.SerializeObject(DropDownList);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message.ToString());
                throw;
            }
        }

        /// <summary>
        /// 获得下拉框列表-主导工厂
        /// </summary>
        /// <returns></returns>
        public string GetDominFtyDrop()
        {
            try
            {
                List<string> DropDownList = new GetIndex().GetDominFtyDropSql();
                return JsonConvert.SerializeObject(DropDownList);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message.ToString());
                throw;
            }
        }

        /// <summary>
        /// 查询周末
        /// </summary>
        /// <param name="NewEmailDomain"></param>
        /// <param name="EndTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public int IndexWeekend(string submitTime, string approveTime)
        {
            try
            {
                int data = new GetIndex().GetWeekendSql(submitTime, approveTime);
                return data;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message.ToString());
                throw;
            }

        }
        #endregion
    }
}