using MIS_InventoryTracking.Models;
using MIS_InventoryTracking.Models.PublicSqlMethods;
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
        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ActionResult Index(string userid)
        {
            //userid = "467";
            if (userid == null)
            {
                userid = "";
            }
            ViewBag.userid = userid;
            string username = new SqlStatement().GetUserSql(userid);
            var bank = " order by  Invt_Time desc,Ledger desc ,Fnumber desc";//默认的排序

            //查表前先看看表是否存在，不存在则新建一张表
            if (new SqlStatement().CheckTable(username) == 0 && !new SqlStatement().CREATETable(username))
            {

            }
            else
            {
                List<Others> List = new SqlStatement().GetIndexListSecond(username, bank);
                ViewBag.count = List.Count;    
            }
            Session["username"] = username;
            Session["userid"] = userid;
            ViewBag.username = username;
            ViewBag.userid = userid;
            return View();
        }

        /// <summary>
        /// 打印首页
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ActionResult IndexPrint(string userid)
        {
            //userid = "467";
            if (userid == null)
            {
                userid = "";
            }
            ViewBag.userid = userid;
            string username = new SqlStatement().GetUserSql(Session["userid"].ToString());
            var bank = " order by  Invt_Time desc,Ledger desc ,Fnumber desc";//默认的排序
            List<Others> List = new SqlStatement().GetIndexListSecond(username, bank);
            ViewBag.username = username;
            ViewBag.count = List.Count;
            return View();
        }
        /// <summary>
        /// 首页加载列表数据
        /// </summary>
        /// <returns></returns>
        public string IndexData(string bank)
        {
            try
            {
                //查表前先看看表是否存在，不存在则新建一张表
                if (new SqlStatement().CheckTable(base.Session["username"].ToString()) == 0 && !new SqlStatement().CREATETable(base.Session["username"].ToString()))
                {
                    return "";
                }

                List<Others> List = new SqlStatement().GetIndexListSecond(base.Session["username"].ToString(), bank);

                foreach (var item in List)
                {
                    item.WIP_Qty = item.WIP_Qty == null ? "" : item.WIP_Qty;
                    item.MRB_Qty = item.MRB_Qty == null ? "" : item.MRB_Qty;
                    item.WH_Qty = item.WH_Qty == null ? "" : item.WH_Qty;
                    item.IQC_Qty = item.IQC_Qty == null ? "" : item.IQC_Qty;
                    item.OpenPO_Qty = item.OpenPO_Qty == null ? "" : item.OpenPO_Qty;
                }

                Session["count"] = List.Count;
                ResponseJson json = new ResponseJson
                {
                    Data = List
                };
                return JsonConvert.SerializeObject(json);



            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message.ToString());
                throw;
            }
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
                LogHelper.Write(ex.Message.ToString());
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
            var isSuccess = new SqlStatement().InsertDataSql(Session["username"].ToString(), check);
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
        /// <summary>
        /// 删除当前行数据
        /// </summary>
        /// <param name="Ledger"></param>
        /// <param name="Fnumber"></param>
        /// <returns></returns>
        public int DeleteThisData(string Ledger, String Fnumber)
        {
            var isSuccess = new SqlStatement().DeleteOneData(Session["username"].ToString(), Ledger, Fnumber);
            return isSuccess;
        }
        /// <summary>
        /// 更新当前行数据
        /// </summary>
        /// <param name="Ledger"></param>
        /// <param name="Fnumber"></param>
        /// <returns></returns>
        public int UpdateThisData(string Ledger, String Fnumber)
        {
            var isSuccess = new SqlStatement().UpdateOneData(Session["username"].ToString(), Ledger, Fnumber);
            return isSuccess;
        }
        /// <summary>
        /// 根据条件表格中的账套和物料号查询是否已存在数据表格当中
        /// </summary>
        /// <param name="Ledger"></param>
        /// <param name="Fnumber"></param>
        /// <returns></returns>
        public int SelectLedgerAndFnumberData(string Ledger, String Fnumber)
        {
            var isSuccess = new SqlStatement().SelectLedgerAndFnumberDataSQL(Session["username"].ToString(), Ledger, Fnumber);
            return isSuccess;
        }
        /// <summary>
        /// 执行采集数据
        /// </summary>
        /// <param name="Ledger"></param>
        /// <param name="Fnumber"></param>
        /// <returns></returns>
        public int Wholetablecapture(string bank)
        {
            var isSuccess = new SqlStatement().WholetablecaptureSQL(Session["username"].ToString(), bank);
            return isSuccess;
        }
    }
}