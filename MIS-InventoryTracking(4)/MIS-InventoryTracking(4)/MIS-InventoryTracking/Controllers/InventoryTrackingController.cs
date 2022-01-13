using MIS_InventoryTracking.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MIS_InventoryTracking.Controllers
{
    public class InventoryTrackingController : Controller
    {
        // GET: InventoryTracking
        public ActionResult Index(string userid)
        {
            userid = "23";
            if (userid == null)
            {
                userid = "";
            }
            ViewBag.userid = userid;
            string username = new SqlStatement().GetUserSql(userid);
            Session["username"] = username;
            return View();
        }
        /// <summary>
        /// 首页加载列表数据
        /// </summary>
        /// <returns></returns>
        public string IndexData(string bank)
        {
            //查表前先看看表是否存在，不存在则新建一张表
            if (new SqlStatement().CheckTable(base.Session["username"].ToString()) == 0 && !new SqlStatement().CREATETable(base.Session["username"].ToString()))
            {
                return "";
            }
            List<CheckFullInventory_Temp_> List = new SqlStatement().GetIndexList(base.Session["username"].ToString(),bank);
            ResponseJson json = new ResponseJson
            {
                Data = List
            };
            return JsonConvert.SerializeObject(json);
        }
        /// <summary>
        /// 通过账套和物料号带出物料名称规格
        /// </summary>
        /// <param name="Ledger">账套名</param>
        /// <param name="Fnumber"></param>
        /// <returns></returns>
        public string GetMaterialMessage(string Ledger, string Fnumber)
        {
            try
            {
                var result = new SqlStatement().GetMaterialMessageSQL(Ledger, Fnumber);
                return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                return "error";
                throw;
            }

        }
        /// <summary>
        /// 加入临时列表的操作
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public int InsertList(CheckFullInventory_Temp_ check)
        {
          var isSuccess= new SqlStatement().InsertDataSql(Session["username"].ToString(),check);
            return isSuccess;
        }
        /// <summary>
        /// 清空临时表数据
        /// </summary>
        /// <returns></returns>
        public int EmptyList()
        {
            var isSuccess = new SqlStatement().TRUNCATETABLE(Session["username"].ToString());
            return isSuccess;
        }
    }
}