using MIS_InventoryTracking.Models;
using MIS_InventoryTracking.Models.PublicSqlMethods;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Text;
using NPOI.SS.Util;

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
            //userid = "444";
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
                Session["bank"] = bank;
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

        /// <summary>
        /// 用NPOI导出临时表
        /// </summary>
        /// <returns></returns>
        public FileResult DownLoad()
        {
            try
            {
                //创建Excel文件的对象
                HSSFWorkbook HSSFWorkbook = new HSSFWorkbook();
                //添加一个sheet(一个book可以有多个sheet)
                ISheet sheet = HSSFWorkbook.CreateSheet("数据表格");
                IRow cellone = sheet.CreateRow(0); //第一行
                cellone.CreateCell(0).SetCellValue("以下的物料库存量由用户" + Session["username"].ToString() + '于' + DateTime.Now.Year + '年' + DateTime.Now.Month + '月' + DateTime.Now.Day + "日导出");
                //合并单元格-起始行号，终止行号， 起始列号，终止列号
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 10));
                IRow celltwo = sheet.CreateRow(1); //第二行
                celltwo.CreateCell(0).SetCellValue("The inventory of the following materials is determined by the user " + Session["username"].ToString() + " to " + DateTime.Now.Year + " year " + DateTime.Now.Month + 1 + "  month " + DateTime.Now.Day + " day export");
                //合并单元格-起始行号，终止行号， 起始列号，终止列号
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 10));
                IRow cellthree = sheet.CreateRow(2);
                cellthree.CreateCell(0).SetCellValue("账套");
                cellthree.CreateCell(1).SetCellValue("物料号");
                cellthree.CreateCell(2).SetCellValue("物料名称规格");
                cellthree.CreateCell(3).SetCellValue("计量单位");
                cellthree.CreateCell(4).SetCellValue("生产库存");
                cellthree.CreateCell(5).SetCellValue("已报废");
                cellthree.CreateCell(6).SetCellValue("仓库库存");
                cellthree.CreateCell(7).SetCellValue("待检");
                cellthree.CreateCell(8).SetCellValue("未完成订单量");
                cellthree.CreateCell(9).SetCellValue("统计时间");
                IRow cellhour = sheet.CreateRow(3);
                cellhour.CreateCell(0).SetCellValue("Ledger");
                cellhour.CreateCell(1).SetCellValue("Material Code");
                cellhour.CreateCell(2).SetCellValue("Material Name & description");
                cellhour.CreateCell(3).SetCellValue("Unit");
                cellhour.CreateCell(4).SetCellValue("WIP");
                cellhour.CreateCell(5).SetCellValue("MRB");
                cellhour.CreateCell(6).SetCellValue("WH-Stock");
                cellhour.CreateCell(7).SetCellValue("IQC");
                cellhour.CreateCell(8).SetCellValue("Open PO");
                cellhour.CreateCell(9).SetCellValue("At-Time");
                List<CheckFullInventory_Temp_> list = new SqlStatement().GetIndexList(Session["username"].ToString(), Session["bank"].ToString());
                var j = 3;
                //创建CellStyle与DataFormat并加载格式样式
                IDataFormat dataformat = HSSFWorkbook.CreateDataFormat();
                ICellStyle style1 = HSSFWorkbook.CreateCellStyle();
                style1.DataFormat = dataformat.GetFormat("0.00"); //改变小数精度【小数点后有几个0表示精确到小数点后几位】
                for (int i = 0; i < list.Count; i++)
                {
                    IRow rowtemp = sheet.CreateRow(++j);
                    rowtemp.CreateCell(0).SetCellValue(list[i].Ledger.ToString());
                    rowtemp.CreateCell(1).SetCellValue(list[i].Fnumber.ToString());
                    rowtemp.CreateCell(2).SetCellValue(list[i].Material.ToString());
                    //不做判断数据为空会报错
                    if (list[i].MeasureUnit != null)
                    {
                        rowtemp.CreateCell(3).SetCellValue(list[i].MeasureUnit.ToString());
                    }
                    else
                    {
                        rowtemp.CreateCell(3).SetCellValue("");
                    }
                    if (list[i].WIP_Qty != null)
                    {
                        rowtemp.CreateCell(4).SetCellValue((float)list[i].WIP_Qty);
                        rowtemp.GetCell(4).CellStyle = style1;
                    }
                    else
                    {
                        rowtemp.CreateCell(4).SetCellValue("");
                    }
                    if (list[i].MRB_Qty != null)
                    {
                        rowtemp.CreateCell(5).SetCellValue((float)list[i].MRB_Qty);
                        rowtemp.GetCell(5).CellStyle = style1;
                    }
                    else
                    {
                        rowtemp.CreateCell(5).SetCellValue("");
                    }
                    if (list[i].WH_Qty != null)
                    {
                        rowtemp.CreateCell(6).SetCellValue((float)list[i].WH_Qty);
                        rowtemp.GetCell(6).CellStyle = style1;
                    }
                    else
                    {
                        rowtemp.CreateCell(6).SetCellValue("");
                    }
                    if (list[i].IQC_Qty != null)
                    {
                        rowtemp.CreateCell(7).SetCellValue((float)list[i].IQC_Qty);
                        rowtemp.GetCell(7).CellStyle = style1;
                    }
                    else
                    {
                        rowtemp.CreateCell(7).SetCellValue("");
                    }
                    if (list[i].OpenPO_Qty != null)
                    {
                        rowtemp.CreateCell(8).SetCellValue((float)list[i].OpenPO_Qty);
                        rowtemp.GetCell(8).CellStyle = style1;
                    }
                    else
                    {
                        rowtemp.CreateCell(8).SetCellValue("");
                    }
                    rowtemp.CreateCell(9).SetCellValue(list[i].Invt_Time.ToString());
                }
                MemoryStream bookStream = new MemoryStream();
                HSSFWorkbook.Write(bookStream);
                bookStream.Seek(0, SeekOrigin.Begin);
                bookStream.Flush();
                string fileName = "GIP-CheckFullInventory-" + Session["username"].ToString() + "-" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".csv";
                return File(bookStream, "application/vnd.ms-excel", fileName);
            }
            catch (Exception ex)
            {
                var aa = ex.Message;
                throw;
            }

        }
    }
}