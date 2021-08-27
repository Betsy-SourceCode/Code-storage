using KYH_JigFixture.Models;
using KYH_JigFixture.Models.PublicSqlMethods;
using KYH_JigFixture.Models.SqlMethods;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace KYH_JigFixture.Controllers
{
    public class JigFixtureController : Controller
    {
        WebStationEntities db = new WebStationEntities();
        /// <summary>
        /// 首页显示数据，要刷新页面
        /// </summary>
        /// <param name="OAFlow_Num">OA订单号</param>
        /// <param name="ApplyDate">申请日期</param>
        /// <param name="Mat_Code">物料代码</param>
        /// <param name="Mat_Name">物料名称</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">显示的行数</param>
        /// <param name="pageRow">总行数</param>
        /// <returns></returns>
        public ActionResult Index(string userid, string OAFlow_Num, string K3PO_Num, string Buyer, string Start_Date, string End_Date, string Mat_Code, string Mat_Name, string Status, int pageIndex = 1, int pageSize = 10, int pageRow = 0)
        {
            try
            {
                userid = "444";
                ViewBag.userid = userid;
                if (userid == null)
                {
                    userid = "";
                }
                string User = this.GetUser(userid);
                base.Session["User"] = User;
                ViewBag.User = User;
                string DeptIDs = this.GetDepartment(User);
                if (DeptIDs == "EM")
                {
                    ViewBag.RkrqQX = "T";  //仪保部门可修改入库日期
                }
                string Username = this.GetUserName(User);
                ViewBag.Username = Username;
                if (this.GetQuan(userid, "JigFixture_E") > 0)
                {
                    ViewBag.K3ddQX = "T";  //新建/修改K3订单权限

                }
                if (this.GetQuan(userid, "JigFixture_P") > 0)
                {
                    ViewBag.Daochu = "T";   //导出打印权限
                }
                Mat_Code = Mat_Name;  //物料值一致
                DateTime now = DateTime.Now;
                DateTime dt = new DateTime(now.Year, now.Month, 1);  //取当月第一天
                var date = dt.ToString("yyyy-MM-dd");
                var today = now.ToString("yyyy-MM-dd");
                //默认时间
                if (Start_Date == null)
                {
                    Start_Date = date;
                }
                if (End_Date == null)
                {
                    End_Date = today;
                }
                if (Status == null)
                {
                    Status = "非结案";
                }
                ViewBag.date = date;  //传值到时间控件里
                ViewBag.today = today;  //传值到时间控件里
                List<SelectOA_Result> temp = new GetIndex().GetIndexPageRowOA(OAFlow_Num, K3PO_Num, Buyer, Start_Date, End_Date, Mat_Code, Mat_Name, Status);
                var rowCount = temp.Count; //总行数
                                           //分页
                temp = temp.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList<SelectOA_Result>();
                ViewBag.count = rowCount;  //总条数
                ViewBag.pageIndex = pageIndex;  //当前页
                return View();
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 首页显示数据，不用刷新 页面
        /// </summary>
        /// <param name="OAFlow_Num">OA订单号</param>
        /// <param name="ApplyDate">申请日期</param>
        /// <param name="Mat_Code">物料代码</param>
        /// <param name="Mat_Name">物料名称</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">显示的行数</param>
        /// <param name="pageRow">总行数</param>
        /// <returns></returns>
        public string IndexData(string OAFlow_Num, string K3PO_Num, string Buyer, string Start_Date, string End_Date, string Mat_Code, string Mat_Name, string Status, int pageIndex = 1, int pageSize = 10, int pageRow = 0)
        {
            try
            {
                Mat_Code = Mat_Name;
                DateTime now = DateTime.Now;
                DateTime dt = new DateTime(now.Year, now.Month, 1);  //取当月第一天
                var date = dt.ToString("yyyy-MM-dd");
                var today = now.ToString("yyyy-MM-dd");
                //默认时间
                if (Start_Date == null)
                {
                    Start_Date = date;
                }
                if (End_Date == null)
                {
                    End_Date = today;
                }
                if (Status == null)
                {
                    Status = "非结案";
                }
                List<SelectOA_Result> temp = new GetIndex().GetIndexPageRowOA(OAFlow_Num, K3PO_Num, Buyer, Start_Date, End_Date, Mat_Code, Mat_Name, Status);
                var rowCount = temp.Count; //总条数
                                           //分页
                temp = temp.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList<SelectOA_Result>();
                object obj = new
                {
                    IndexList = temp
                };
                ResponseJson json = new ResponseJson
                {
                    Success = 0,
                    Data = obj,
                    count = rowCount
                };
                if (temp == null)
                {
                    json = new ResponseJson
                    {
                        Success = 1,
                        Data = obj,
                        count = 0
                    };
                }
                return JsonConvert.SerializeObject(json);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());
                throw;
            }

        }
        /// <summary>
        /// 导出数据加载
        /// </summary>
        /// <param name="OAFlow_Num"></param>
        /// <param name="K3PO_Num"></param>
        /// <param name="Buyer"></param>
        /// <param name="Start_Date"></param>
        /// <param name="End_Date"></param>
        /// <param name="Mat_Code"></param>
        /// <param name="Mat_Name"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public string DaoChuIndexData(string OAFlow_Num, string K3PO_Num, string Buyer, string Start_Date, string End_Date, string Mat_Code, string Mat_Name, string Status)
        {
            string result;
            try
            {
                Mat_Code = Mat_Name;
                DateTime now = DateTime.Now;
                DateTime dt = new DateTime(now.Year, now.Month, 1);
                string date = dt.ToString("yyyy-MM-dd");
                string today = now.ToString("yyyy-MM-dd");
                if (Start_Date == null)
                {
                    Start_Date = date;
                }
                if (End_Date == null)
                {
                    End_Date = today;
                }
                if (Status == null)
                {
                    Status = "非结案";
                }
                List<SelectOA_Result> temp = new GetIndex().GetIndexPageRowOA(OAFlow_Num, K3PO_Num, Buyer, Start_Date, End_Date, Mat_Code, Mat_Name, Status);
                int rowCount = temp.Count;
                object obj = new
                {
                    IndexList = temp
                };
                ResponseJson json = new ResponseJson
                {
                    Success = 0,
                    Data = obj,
                    count = rowCount
                };
                if (temp == null)
                {
                    json = new ResponseJson
                    {
                        Success = 1,
                        Data = obj,
                        count = 0
                    };
                }
                result = JsonConvert.SerializeObject(json);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());
                throw;
            }
            return result;
        }
        /// <summary>
        /// 查询K3订单代入信息
        /// </summary>
        /// <returns></returns>
        public string SelectK3PO_Num(Tools_Acquire_Trace TAT)
        {
            int i = 0;
            GetIndex getIndex = new GetIndex();
            JigFixtureZGCController otherController = DependencyResolver.Current.GetService<JigFixtureZGCController>();
            try
            {
                string userid = base.Session["userid"].ToString();
                int numid = 0;
                if (TAT.FID != 0)
                {
                    numid = 1;
                }
                if (TAT.K3PO_Num.Length > 3)
                {
                    string num = TAT.K3PO_Num.Substring(0, 2);
                    if (num == "HS")
                    {
                        Others K3 = getIndex.ByK3PO_NumSelect("HS_YHNPN", TAT.K3PO_Num, TAT.Mat_Code);
                        if (K3 != null)
                        {
                            TAT.K3PO_Num = K3.K3PO_Num;
                            TAT.K3PO_Date = K3.K3PO_Date;
                            TAT.Supplier = K3.Supplier;
                            TAT.K3Qty = K3.K3FQty;
                            TAT.K3Unit = K3.k3Unit;
                            TAT.Buyer = K3.Buyer;
                            int result = otherController.AddFunction(numid, TAT, userid);
                            i = result;
                        }
                    }
                    else if (num == "YH")
                    {
                        Others K4 = getIndex.ByK3PO_NumSelect("AIS20070103075708", TAT.K3PO_Num, TAT.Mat_Code);
                        if (K4 != null)
                        {
                            TAT.K3PO_Num = K4.K3PO_Num;
                            TAT.K3PO_Date = K4.K3PO_Date;
                            TAT.Supplier = K4.Supplier;
                            TAT.K3Qty = K4.K3FQty;
                            TAT.K3Unit = K4.k3Unit;
                            TAT.Buyer = K4.Buyer;
                            int result2 = otherController.AddFunction(numid, TAT, userid);
                            i = result2;
                        }
                    }
                    else
                    {
                        Others K5 = getIndex.ByK3PO_NumSelect("AIS20120702111936", TAT.K3PO_Num, TAT.Mat_Code);
                        if (K5 != null)
                        {
                            TAT.K3PO_Num = K5.K3PO_Num;
                            TAT.K3PO_Date = K5.K3PO_Date;
                            TAT.Supplier = K5.Supplier;
                            TAT.K3Qty = K5.K3FQty;
                            TAT.K3Unit = K5.k3Unit;
                            TAT.Buyer = K5.Buyer;
                            int result3 = otherController.AddFunction(numid, TAT, userid);
                            i = result3;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());
                throw;
            }
            if (i > 0)
            {
                Others json = new Others
                {
                    Data = TAT
                };
                return JsonConvert.SerializeObject(json);
            }
            return "0";
        }

        /// <summary>
        /// 根据员工号获得三字代码
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetUser(string userid)
        {
            if (userid == null || userid == "")
            {
                return "";
            }
            Others UserIDs = new Authority().GetUserSql(userid);
            return UserIDs.Text.ToString();
        }
        /// <summary>
        /// 根据三字代码获得所属部门
        /// </summary>
        /// <param name="EmpID"></param>
        /// <returns></returns>
        public string GetDepartment(string EmpID)
        {
            if (EmpID == null || EmpID == "")
            {
                return "";
            }
            Others user = new Authority().GetDepartmentSql(EmpID);
            return user.Text;
        }
        /// <summary>
        /// 根据三字代码查询员工完整名字（姓+名）
        /// </summary>
        /// <param name="EmpID"></param>
        /// <returns></returns>
        public string GetUserName(string EmpID)
        {
            if (EmpID == null || EmpID == "")
            {
                return "";
            }
            Others user = new Authority().GetUserNameSql(EmpID);
            return user.Text;
        }

        /// <summary>
        /// 查询是否有指定权限
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="PermissionCode"></param>
        /// <returns></returns>
        public int GetQuan(string userid, string PermissionCode)
        {
            if (userid == null || userid == "")
            {
                return 0;
            }
            Others UserIDs = new Authority().GetQuanSql(userid, PermissionCode);
            return UserIDs.id;
        }
        /// <summary>
        /// 获得采购员名单
        /// </summary>
        /// <returns></returns>
        public string SelectCGY()
        {
            string result;
            try
            {
                List<Others> trade = new GetIndex().GetCGY();
                object obj = new
                {
                    DropDownList = trade
                };
                ResponseJson responseJson = new ResponseJson();
                responseJson.Success = 0;
                responseJson.Data = obj;
                ResponseJson json = new ResponseJson
                {
                    Success = 1,
                    Data = obj
                };
                result = JsonConvert.SerializeObject(json);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());
                throw;
            }
            return result;
        }
    }
}