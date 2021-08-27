using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using KYH_ProductPrice.Models;
using KYH_ProductPrice.Models.PublicSqlMethods;
using KYH_ProductPrice.Models.SqlStatement;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;

namespace KYH_ProductPrice.Controllers
{
    // Token: 0x02000016 RID: 22
    public class ProductPriceController : Controller
    {
        #region 页面
        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ActionResult Index(string userid)
        {
            userid = "444";
            if (userid == null)
            {
                userid = "";
            }
            string username = new Authority().GetUserSql(userid);
            string DeptIDs = new Authority().GetDepartmentSql(username, 0);
            string Position = new Authority().GetPositionSql(username);
            base.Session["userid"] = userid;
            base.Session["username"] = username;
            base.Session["DeptIDs"] = DeptIDs;
            DateTime now = DateTime.Now;
            string date = now.AddDays(-50.0).ToString("yyyy-MM-dd");
            ViewBag.date = date;
            ViewBag.today = now.ToString("yyyy-MM-dd");  //给前台时间控件赋值
            if (new Authority().GetQuanSql(userid, "CustProductPriceRecords_C") > 0)
            {
                ViewBag.Add = 1;  //新增权限
            }
            if (new Authority().GetQuanSql(userid, "CustProductPriceRecords_P") > 0)
            {
                ViewBag.PrintExport = 1; //打印权限
            }
            if (new Authority().GetQuanSql(userid, "CustSvcr_S") > 0)
            {
                ViewBag.Assign = 1; //设定业务员权限
            }
            if (new Authority().GetDepartmentSql(username, 1) != "ARV")
            {
                ViewBag.GuanLi = 1; //作废权限
            }
            if (new Authority().GetDepartmentSql(username, 1) == "ARV")
            {

                ViewBag.ISARV = 1; //财务部权限
            }
            return base.View();
        }

        /// <summary>
        /// 首页打印页面
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ActionResult IndexPrinting(string userid)
        {
            ViewBag.username = Session["username"].ToString();
            return base.View();
        }

        /// <summary>
        /// 设定业务员页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SetupCustomer()
        {
            ViewBag.userid = Session["userid"].ToString();
            return base.View();
        }

        /// <summary>
        /// 新增客户产品价格信息记录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AddProductPrice()
        {
            ViewBag.userid = Session["userid"].ToString();
            return base.View();
        }

        /// <summary>
        /// 客户产品价格信息记录详情页面
        /// </summary>
        /// <param name="CPSerial"></param>
        /// <returns></returns>
        public ActionResult DetailsProductPrice(int CPSerial)
        {
            ViewBag.userid = Session["userid"].ToString();
            List<GetProductPriceList> ProductPriceList = new PublicSqlMethodsSql().GetDetailsListSql(CPSerial);
            ViewBag.ProductPriceList = ProductPriceList[0];
            ViewBag.mydate = ProductPriceList[0].CreateTime;
            ViewBag.Cancel = ProductPriceList[0].Cancel;
            if (new Authority().GetQuanSql(base.Session["userid"].ToString(), "CustProductPriceRecords_C") > 0)
            {
                ViewBag.Add = 1;  //新增权限
            }
            if (new Authority().GetDepartmentSql(base.Session["username"].ToString(), 1) != "ARV")
            {
                ViewBag.GuanLi = 1; //作废权限
            }
            if (new Authority().GetDepartmentSql(base.Session["username"].ToString(), 1) == "ARV")
            {
                ViewBag.ISARV = 1; //财务部权限
            }
            return base.View();
        }

        /// <summary>
        /// 日期格式化（香港格式）
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public string getMyDate(DateTime date)
        {
            string[] i = new string[]
            {
                "Jan",
                "Feb",
                "Mar",
                "Apr",
                "May",
                "Jun",
                "Jul",
                "Aug",
                "Spt",
                "Oct",
                "Nov",
                "Dec"
            };
            return string.Concat(new string[]
            {
                date.Day.ToString(),
                "-",
                i[date.Month],
                "-",
                date.Year.ToString(),
                " ",
                date.Hour.ToString(),
                ":",
                date.Minute.ToString(),
                ":",
                date.Second.ToString()
            });
        }

        /// <summary>
        /// 修改客户产品价格信息记录页面
        /// </summary>
        /// <param name="CPSerial"></param>
        /// <returns></returns>
        public ActionResult UpdateProductPrice(int CPSerial)
        {
            ViewBag.userid = Session["userid"].ToString();
            List<GetProductPriceList> ProductPriceList = new PublicSqlMethodsSql().GetDetailsListSql(CPSerial);
            ViewBag.ProductPriceList = ProductPriceList[0];
            ViewBag.mydate = ProductPriceList[0].CreateTime;
            ViewBag.Cancel = ProductPriceList[0].Cancel;
            if (ProductPriceList[0].EffDetail == "新增信息记录")
            {
                ProductPriceList[0].EffDetail = "";
            }
            ViewBag.EffDetail = ProductPriceList[0].EffDetail;
            return base.View();
        }
        #endregion

        #region API
        /// <summary>
        /// 首页数据加载
        /// </summary>
        /// <param name="list"></param>
        /// <param name="EndTime"></param>
        /// <param name="CustProd"></param>
        /// <returns></returns>
        public string IndexData(GetProductPriceList list, string EndTime, string CustProd)
        {
            List<GetProductPriceList> ProductPriceList = new List<GetProductPriceList>();
            if (new Authority().GetDepartmentSql(base.Session["username"].ToString(), 1) == "ARV" || new Authority().GetDesignatedPersonSql(base.Session["username"].ToString()) > 0)
            {
                ProductPriceList = new PublicSqlMethodsSql().GetDetailsListSql(list.CreateBy, CustProd, list.CustomerDisplayName, list.CreateTime, EndTime, list.Remarks_MD, list.Cancel, null, null);
            }
            else
            {
                string login_Dept = new Authority().GetIsmanagementlayer(base.Session["username"].ToString());
                if (login_Dept == "")
                {
                    login_Dept = null;
                }
                ProductPriceList = new PublicSqlMethodsSql().GetDetailsListSql(list.CreateBy, CustProd, list.CustomerDisplayName, list.CreateTime, EndTime, list.Remarks_MD, list.Cancel, base.Session["username"].ToString(), login_Dept);
            }
            ResponseJson json = new ResponseJson
            {
                Data = ProductPriceList
            };
            return JsonConvert.SerializeObject(json);
        }

        /// <summary>
        /// 通过文本框条件获得员工列表SQL
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public string GetSetupCustomer(string Name)
        {
            string result;
            try
            {
                List<Others> CustomerList = new SetupCustomerSql().GetCustomerListSql(Name);
                ResponseJson json = new ResponseJson
                {
                    Data = CustomerList
                };
                result = JsonConvert.SerializeObject(json);
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 通过员工三字代码获得未指派客户名单
        /// </summary>
        /// <param name="ServiceBy"></param>
        /// <returns></returns>
        public string GetUnassignedList(string ServiceBy)
        {
            string result;
            try
            {
                List<Others> UnassignedList = new SetupCustomerSql().GetUnassignedListSql(ServiceBy);
                ResponseJson json = new ResponseJson
                {
                    Data = UnassignedList
                };
                result = JsonConvert.SerializeObject(json);
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 通过员工三字代码获得已指派客户名单
        /// </summary>
        /// <param name="ServiceBy"></param>
        /// <returns></returns>
        public string GetAssignList(string ServiceBy)
        {
            string result;
            try
            {
                List<Others> AssignList = new SetupCustomerSql().GetAssignListSql(ServiceBy);
                ResponseJson json = new ResponseJson
                {
                    Data = AssignList
                };
                result = JsonConvert.SerializeObject(json);
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 通过GIP产品号获得产品描述和客户产品代码
        /// </summary>
        /// <param name="GipFnumber">GIP产品号</param>
        /// <param name="Num">账套</param>
        /// <returns></returns>
        public string GetProductByGipFnumber(string GipFnumber, string Num)
        {
            Others Product = null;
            string result;
            try
            {
                if (Num == "HL")
                {
                    Product = new AddProductPriceSql().GetProductByGipFnumberSql(GipFnumber, "AIS20151013110946", "ic.F_103 ");
                }
                if (Num == "YH")
                {
                    Product = new AddProductPriceSql().GetProductByGipFnumberSql(GipFnumber, "AIS20170316112450", "ic.F_113");
                }
                if (Num == "HS")
                {
                    Product = new AddProductPriceSql().GetProductByGipFnumberSql(GipFnumber, "HSYH_New", "ic.F_103");
                }
                if (Num == "LK")
                {
                    Product = new AddProductPriceSql().GetProductByGipFnumberSql(GipFnumber, "AIS20181011094554", "ic.F_110");
                }
                ResponseJson json;
                if (Product != null)
                {
                    json = new ResponseJson
                    {
                        Success = 1,
                        Data = Product
                    };
                }
                else
                {
                    json = new ResponseJson
                    {
                        Success = 0,
                        Data = Product
                    };
                }
                result = JsonConvert.SerializeObject(json);
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 查询账套列表
        /// </summary>
        /// <returns></returns>
        public string GetLedger()
        {
            string result;
            try
            {
                List<Others> Ledger = new AddProductPriceSql().GetLedgerSql();
                ResponseJson json;
                if (Ledger != null)
                {
                    json = new ResponseJson
                    {
                        Success = 1,
                        Data = Ledger
                    };
                }
                else
                {
                    json = new ResponseJson
                    {
                        Success = 0,
                        Data = Ledger
                    };
                }
                result = JsonConvert.SerializeObject(json);
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 查询货币列表
        /// </summary>
        /// <returns></returns>
        public string GetCurrList()
        {
            string result;
            try
            {
                List<Others> CurrList = new AddProductPriceSql().GetCurrListSql();
                ResponseJson json;
                if (CurrList != null)
                {
                    json = new ResponseJson
                    {
                        Success = 1,
                        Data = CurrList
                    };
                }
                else
                {
                    json = new ResponseJson
                    {
                        Success = 0,
                        Data = CurrList
                    };
                }
                result = JsonConvert.SerializeObject(json);
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 查询登录人下的客户列表
        /// </summary>
        /// <returns></returns>
        public string GetCustomerList()
        {
            string LoginUserName = base.Session["username"].ToString();
            string result;
            try
            {
                List<Others> CurrList = new AddProductPriceSql().GetCustomerListSql(LoginUserName);
                ResponseJson json;
                if (CurrList != null)
                {
                    json = new ResponseJson
                    {
                        Success = 1,
                        Data = CurrList
                    };
                }
                else
                {
                    json = new ResponseJson
                    {
                        Success = 0,
                        Data = CurrList
                    };
                }
                result = JsonConvert.SerializeObject(json);
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 查询生效类型列表
        /// </summary>
        /// <returns></returns>
        public string GetEffType()
        {
            string result;
            try
            {
                List<Others> EffTypeList = new UpdateProductPrice().GetEffTypeListSql();
                ResponseJson json;
                if (EffTypeList != null)
                {
                    json = new ResponseJson
                    {
                        Success = 1,
                        Data = EffTypeList
                    };
                }
                else
                {
                    json = new ResponseJson
                    {
                        Success = 0,
                        Data = EffTypeList
                    };
                }
                result = JsonConvert.SerializeObject(json);
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        #endregion
    }
}
