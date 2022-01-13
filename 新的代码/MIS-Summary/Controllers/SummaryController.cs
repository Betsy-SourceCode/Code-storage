using MIS_Summary.Models;
using MIS_Summary.Models.PublicSqlMethods;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MIS_Summary.Controllers
{
    public class SummaryController : Controller
    {
        #region //页面
        public ActionResult Index(string userid)
        {
            //userid = "444";
            return View();
        }
        public ActionResult Export()
        {
            return View();
        }
        #endregion
        /// <summary>
        /// 查询电脑及周边设备验收单汇总表格内容
        /// </summary>
        /// <param name="DocumentNo"></param>
        /// <param name="Mat_Code"></param>
        /// <param name="Mat_Name"></param>
        /// <param name="ApplyStartDate"></param>
        /// <param name="ApplyEndDate"></param>
        /// <returns></returns>
        public string IndexData(string DocumentNo, string Mat_Code, string Mat_Name, string ApplyStartDate, string ApplyEndDate)
        {
            try
            {
                List<View_Summary> data = SummaryMethods.SelectSummaryListSQL(DocumentNo, Mat_Code, Mat_Name, ApplyStartDate, ApplyEndDate);
                var obj = new
                {
                    code = 0,
                    msg = "",
                    count = data.Count,
                    data = data
                };
                return JsonConvert.SerializeObject(obj);
            }
            catch (System.Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }
            
        }
        /// <summary>
        /// 查询没有验收单的数据
        /// </summary>
        /// <returns></returns>
        public string NoApproveData(string Serial_Num)
        {
            try
            {
               var obj= SummaryMethods.SelectNoApproveListSQL(Serial_Num);//查询没有验收单的数据
                return JsonConvert.SerializeObject(obj);
            }
            catch (System.Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }
        }
    }
}