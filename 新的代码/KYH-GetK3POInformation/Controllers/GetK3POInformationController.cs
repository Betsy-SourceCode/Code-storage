using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using KYH_GetK3POInformation.Models;
using KYH_GetK3POInformation.Models.PublicSqlMethods;
using KYH_GetK3POInformation.Models.SqlMethods;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace KYH_GetK3POInformation.Controllers
{
    // Token: 0x02000010 RID: 16
    public class GetK3POInformationController : Controller
    {
        /// <summary>
        /// 首页加载
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
            string username = new Authority().GetUserSql(userid);
            ViewBag.username = username;
            Session["username"] = username;
            return View();
        }

        /// <summary>
        /// 查询K3数据，批量添加到临时表
        /// </summary>
        /// <returns></returns>
        public string SelectK3PO_Num()
        {
            AddLoadingListAddPOdata_TempController otherController = DependencyResolver.Current.GetService<AddLoadingListAddPOdata_TempController>();
            string result2;
            try
            {
                int result = 0;
                int List = new GetIndex().GetIndexList(base.Session["username"].ToString()).Count;
                for (int a = 1; a <= List; a++)
                {
                    LoadingListAddPOdata_Temp LoadingList = new GetIndex().GetIndexListFirst(base.Session["username"].ToString(), a);
                    string num = LoadingList.PONum.Substring(0, 2);
                    LoadingListAddPOdata_Temp K3 = new LoadingListAddPOdata_Temp();
                    if (num == "HS")
                    {
                        K3 = new GetIndex().ByK3PO_NumSelect("HSYH_New", LoadingList.PONum, LoadingList.Fnumber, "F_105");
                        if (K3 == null)
                        {
                            K3 = new LoadingListAddPOdata_Temp();
                        }
                        if (K3.Material == "error")  //查询SQL报错，告诉系统发生错误传到前台给出提示
                        {
                            result = 0;
                            break;
                        }
                        K3.PONum = LoadingList.PONum;
                        K3.Fnumber = LoadingList.Fnumber;
                        K3.LoadQty = LoadingList.LoadQty;
                        K3.LoadUnit = LoadingList.LoadUnit;
                        K3.LPSerial = a;
                        result = otherController.UpdFunction(base.Session["username"].ToString(), K3);
                    }
                    else if (num == "YH")
                    {
                        K3 = new GetIndex().ByK3PO_NumSelect("AIS20170316112450", LoadingList.PONum, LoadingList.Fnumber, "F_109");
                        if (K3 == null)
                        {
                            K3 = new LoadingListAddPOdata_Temp();
                        }
                        if (K3.Material == "error") //查询SQL报错，告诉系统发生错误传到前台给出提示
                        {
                            result = 0;
                            break;
                        }
                        K3.PONum = LoadingList.PONum;
                        K3.Fnumber = LoadingList.Fnumber;
                        K3.LoadQty = LoadingList.LoadQty;
                        K3.LoadUnit = LoadingList.LoadUnit;
                        K3.LPSerial = a;
                        result = otherController.UpdFunction(base.Session["username"].ToString(), K3);
                    }
                    else if (num == "LK")
                    {
                        K3 = new GetIndex().ByK3PO_NumSelect("AIS20181011094554", LoadingList.PONum, LoadingList.Fnumber, "F_102");
                        if (K3 == null)
                        {
                            K3 = new LoadingListAddPOdata_Temp();
                        }
                        if (K3.Material == "error") //查询SQL报错，告诉系统发生错误传到前台给出提示
                        {
                            result = 0;
                            break;
                        }
                        K3.PONum = LoadingList.PONum;
                        K3.Fnumber = LoadingList.Fnumber;
                        K3.LoadQty = LoadingList.LoadQty;
                        K3.LoadUnit = LoadingList.LoadUnit;
                        K3.LPSerial = a;
                        result = otherController.UpdFunction(base.Session["username"].ToString(), K3);
                        if (result == 0)
                        {
                            return "";
                        }
                    }
                    else if (num == "HL")
                    {
                        K3 = new GetIndex().ByK3PO_NumSelect("AIS20151013110946", LoadingList.PONum, LoadingList.Fnumber, "F_102");
                        if (K3 == null)
                        {
                            K3 = new LoadingListAddPOdata_Temp();
                        }
                        if (K3.Material == "error") //查询SQL报错，告诉系统发生错误传到前台给出提示
                        {
                            result = 0;
                            break;
                        }
                        K3.PONum = LoadingList.PONum;
                        K3.Fnumber = LoadingList.Fnumber;
                        K3.LoadQty = LoadingList.LoadQty;
                        K3.LoadUnit = LoadingList.LoadUnit;
                        K3.LPSerial = a;
                        result = otherController.UpdFunction(base.Session["username"].ToString(), K3);
                    }
                }
                if (result > 0)
                {
                    ResponseJson json = new ResponseJson
                    {
                        Success = result
                    };
                    result2 = JsonConvert.SerializeObject(json);
                }
                else
                {
                    ResponseJson json2 = new ResponseJson
                    {
                        Success = result,
                        Msg = "发生错误，请联系电脑部！内部成员请查看日志文件！"
                    };
                    result2 = JsonConvert.SerializeObject(json2);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());
                throw;
            }
            return result2;
        }

        /// <summary>
        /// 首页加载列表数据
        /// </summary>
        /// <returns></returns>
        public string IndexData()
        {
            //查表前先看看表是否存在，不存在则新建一张表
            if (new GetIndex().CheckTable(base.Session["username"].ToString()) == 0 && !new GetIndex().CREATETable(base.Session["username"].ToString()))
            {
                return "";
            }
            List<LoadingListAddPOdata_Temp_Select> List = new GetIndex().GetIndexList(base.Session["username"].ToString());
            ResponseJson json = new ResponseJson
            {
                Data = List
            };
            return JsonConvert.SerializeObject(json);
        }

        /// <summary>
        /// 用NPOI导出临时表，根据新的需求已作废
        /// </summary>
        /// <returns></returns>
        public ActionResult DownLoad()
        {
            HSSFWorkbook HSSFWorkbook = new HSSFWorkbook();
            ISheet sheet = HSSFWorkbook.CreateSheet("32562");
            IRow cells = sheet.CreateRow(0);
            cells.CreateCell(0).SetCellValue("LPSerial");
            cells.CreateCell(1).SetCellValue("PONum");
            cells.CreateCell(2).SetCellValue("Fnumber");
            cells.CreateCell(3).SetCellValue("LoadQty");
            cells.CreateCell(4).SetCellValue("LoadUnit");
            cells.CreateCell(5).SetCellValue("Supplier");
            cells.CreateCell(6).SetCellValue("Material");
            cells.CreateCell(7).SetCellValue("POQty");
            cells.CreateCell(8).SetCellValue("POUnit");
            cells.CreateCell(9).SetCellValue("POCurr");
            cells.CreateCell(10).SetCellValue("UnitPrice");
            cells.CreateCell(11).SetCellValue("NeedDate");
            cells.CreateCell(12).SetCellValue("Remarks");
            cells.CreateCell(13).SetCellValue("USDRate");
            List<LoadingListAddPOdata_Temp> list = new GetIndex().GetIndexListAll(base.Session["username"].ToString());
            for (int i = 0; i < list.Count; i++)
            {
                IRow rowtemp = sheet.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(list[i].LPSerial.ToString());
                rowtemp.CreateCell(1).SetCellValue(list[i].PONum.ToString());
                rowtemp.CreateCell(2).SetCellValue(list[i].Fnumber.ToString());
                rowtemp.CreateCell(3).SetCellValue(list[i].LoadQty.ToString());
                rowtemp.CreateCell(4).SetCellValue(list[i].LoadUnit.ToString());
                if (list[i].Supplier != null)
                {
                    rowtemp.CreateCell(5).SetCellValue(list[i].Supplier.ToString());
                }
                else
                {
                    rowtemp.CreateCell(5).SetCellValue("");
                }
                if (list[i].Material != null)
                {
                    rowtemp.CreateCell(6).SetCellValue(list[i].Material.ToString());
                }
                else
                {
                    rowtemp.CreateCell(6).SetCellValue("");
                }
                if (list[i].POQty != null)
                {
                    rowtemp.CreateCell(7).SetCellValue(list[i].POQty.ToString());
                }
                else
                {
                    rowtemp.CreateCell(7).SetCellValue("");
                }
                if (list[i].POUnit != null)
                {
                    rowtemp.CreateCell(8).SetCellValue(list[i].POUnit.ToString());
                }
                else
                {
                    rowtemp.CreateCell(8).SetCellValue("");
                }
                if (list[i].POCurr != null)
                {
                    rowtemp.CreateCell(9).SetCellValue(list[i].POCurr.ToString());
                }
                else
                {
                    rowtemp.CreateCell(9).SetCellValue("");
                }
                if (list[i].UnitPrice != null)
                {
                    rowtemp.CreateCell(10).SetCellValue(list[i].UnitPrice.ToString());
                }
                else
                {
                    rowtemp.CreateCell(8).SetCellValue("");
                }
                if (list[i].NeedDate != null)
                {
                    rowtemp.CreateCell(11).SetCellValue(list[i].NeedDate.ToString());
                }
                else
                {
                    rowtemp.CreateCell(11).SetCellValue("");
                }
                if (list[i].Remarks != null)
                {
                    rowtemp.CreateCell(12).SetCellValue(list[i].Remarks.ToString());
                }
                else
                {
                    rowtemp.CreateCell(12).SetCellValue("");
                }
                if (list[i].USDRate != null)
                {
                    rowtemp.CreateCell(13).SetCellValue(list[i].USDRate.ToString());
                }
                else
                {
                    rowtemp.CreateCell(13).SetCellValue("");
                }
            }
            string fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + "〈" + base.Session["username"].ToString() + "〉LoadingList w PODate.xls";
            MemoryStream bookStream = new MemoryStream();
            HSSFWorkbook.Write(bookStream);
            bookStream.Seek(0L, SeekOrigin.Begin);
            return this.File(bookStream, "application/vnd.ms-excel", fileName);
        }

        // Token: 0x0400003B RID: 59
        private WebStationEntities db = new WebStationEntities();
    }
}
