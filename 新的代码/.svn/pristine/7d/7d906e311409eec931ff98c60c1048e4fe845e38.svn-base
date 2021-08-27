using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KYH_Customer.Filters;
using KYH_Customer.Models;
using KYH_Customer.Models.PublicSqlMethods;
using KYH_Customer.Models.SqlMethods;
using KYH_Customer.Models.ViewDataModel;
using Newtonsoft.Json;

namespace Customer.Controllers
{
    [Localization]
    [IsSessionExpire(IsCheck = true)]
    public class CustomerController : Controller
    {
        WebStationEntities db = new WebStationEntities();
        public string username;
        public bool C = true;  //添加权限参数控制
        public bool D = true;  //提交/审核权限参数控制
        #region 页面显示
        [IsSessionExpire(IsCheck = false)]
        public ActionResult Default(string ShortName, string Name_CN, string K3Code, string sortedField, string userid, string dayin, int pageIndex = 1, int pageSize = 20, int pageRow = 0)
        {
            try
            {
                userid = "23"; //测试
                var Rank = "";
                string username = this.GetUsername(userid);
                string DeptIDs = this.GetUser(username);
                string Position = this.GetPosition(username);
                // 登录信息存储
                if (HttpContext.Request.Cookies["userid"] == null || HttpContext.Request.Cookies["userid"].Value == "")
                {
                    HttpContext.Response.Cookies["userid"].Value = userid;
                    HttpContext.Response.Cookies["username"].Value = username;
                    HttpContext.Response.Cookies["deptid"].Value = DeptIDs;
                }
                if (Session["id"] == null || Session["id"] == (object)"")
                {
                    Session["id"] = HttpContext.Request.Cookies["userid"].Value;
                    Session["username"] = HttpContext.Request.Cookies["username"].Value;
                    Session["DeptID"] = HttpContext.Request.Cookies["DeptID"].Value;
                }
                if (sortedField == null)
                {
                    Rank = "CustomerIDs desc";//默认
                }
                else
                {
                    Rank = sortedField;
                }
                if (userid == null)
                {
                    userid = "";
                }
                int shenhe = 0;
                if (this.GetShenP(userid) != 0)
                {
                    ViewBag.SH = D;
                    shenhe = 1;
                }
                List<SelectIndexs> temp = new GetDefault().GetDefaultPageRow(ShortName, Name_CN, K3Code, username, DeptIDs, Rank, shenhe);
                int rowCount = temp.Count;
                temp = temp.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList<SelectIndexs>();
                List<NEWSelectIndex> roles = new GetDefault().GetDefaultSql(temp, username, DeptIDs, Position);//列表
                #region 前台所需参数
                ViewBag.pageIndex = pageIndex;
                ViewBag.pageSize = pageSize;
                ViewBag.pageRow = rowCount;
                ViewBag.sortedField = sortedField;
                // 获得总页数
                if (rowCount % pageSize == 0)
                {
                    pageRow = rowCount / pageSize;
                }
                else
                {
                    pageRow = rowCount / pageSize + 1;
                }
                ViewBag.pageCount = pageRow;
                ViewBag.AllCustomer = roles;
                ViewBag.dayin = dayin;
                ViewBag.id = Session["id"];
                ViewBag.userid = userid; //记录userid值
                ViewBag.Name = GetUsername(userid);
                ViewBag.username = Session["username"];
                ViewBag.DeptID = Session["DeptID"];
                int result = GetQuan(userid, "Customer_C"); //查看是否有添加权限,参数控制
                if (result == 0)
                {
                    C = false;
                    ViewBag.User = C;
                }
                else
                {
                    ViewBag.User = C;
                    ViewBag.Insert = "1";
                    ViewBag.id = userid;
                }
                //禁用/解禁权限
                if (this.GetJinYong(userid) != 0)
                {
                    ViewBag.JY = D;
                }
                #endregion
                return View(roles);
            }
            catch (Exception ex)
            {
                return Content("查询失败," + ex.Message);
            }
        }

        /// <summary>
        /// 新客户信用申请表
        /// </summary>
        /// <returns></returns>
        public ActionResult ACustomer(int CustomerID)
        {
            try
            {
                if (base.Session["id"] == null)
                {
                    base.Session["id"] = "";
                }
                ViewBag.id = Session["id"];
                ViewBag.username = Session["username"].ToString();
                ViewBag.DeptID = Session["DeptID"].ToString();
                SelectDetail arr = new GetACustomer().GetACustomerSql(CustomerID);
                if (arr == null)
                {
                    arr = new SelectDetail();
                }
                ViewBag.AllCustomer = arr;
            }
            catch (Exception ex)
            {
                return base.Content("查询失败," + ex.Message);
            }
            return base.View();
        }

        /// <summary>
        /// 修改客户信用申请表
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public ActionResult UpdateCustomer(int CustomerID, string num)
        {
            try
            {
                if (base.Session["id"] == null)
                {
                    base.Session["id"] = "";
                }
                ViewBag.num = num;
                ViewBag.id = Session["id"];
                ViewBag.username = Session["username"].ToString();
                ViewBag.DeptID = Session["DeptID"].ToString();
                SelectCustomerAll arr = new GetUpdateCustomer().GetUpdateCustomerSql(CustomerID);
                if (arr == null)
                {
                    SelectCustomerAll selectCustomerAll = new SelectCustomerAll();
                    arr = selectCustomerAll;
                }
                ViewBag.AllCustomer = arr;
                //第四页签表格
                List<Contacts> selectsql = new GetUpdateCustomer().GetUpdateCustomerTable(CustomerID, arr.SysID);
                if (selectsql == null)
                {
                    selectsql = new List<Contacts>();
                }
                ViewBag.All = selectsql;
            }
            catch (Exception ex)
            {
                return base.Content("查询失败," + ex.Message);
            }
            return base.View();
        }

        /// <summary>
        ///  客户详情页(维护/审核中)
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="createBy"></param>
        /// <param name="createDept"></param>
        /// <returns></returns>
        public ActionResult Detail(int CustomerID, string createBy, string createDept)
        {
            try
            {
                if (base.Session["id"] == null)
                {
                    base.Session["id"] = "";
                }
                ViewBag.id = Session["id"];
                ViewBag.createBy = Session["username"].ToString();
                ViewBag.createDept = Session["DeptID"].ToString();
                SelectDetail arr = new GetDetail().GetDetailSql(CustomerID);
                if (arr == null)
                {
                    arr = new SelectDetail();
                }
                ViewBag.AllCustomer = arr;
                //审核权限
                if (this.GetShenP(base.Session["id"].ToString()) != 0)
                {
                    ViewBag.SP = D;
                }
                //显示补充意见
                AuditInfo sql = new GetDetail().GetDetailBcyjSql(CustomerID, createBy, createDept);
                if (sql == null)
                {
                    sql = new AuditInfo();
                }
                ViewBag.AuditInfo = sql;
            }
            catch (Exception ex)
            {
                return base.Content("查询失败," + ex.Message);
            }
            return base.View();
        }

        /// <summary>
        /// 客户详情页(有效/停用)
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="num"></param>
        /// <param name="createBy"></param>
        /// <param name="createDept"></param>
        /// <returns></returns>
        public ActionResult OnlyReadDetail(int CustomerID, string num, string createBy, string createDept)
        {
            try
            {
                if (base.Session["id"] == null)
                {
                    base.Session["id"] = "";
                }
                ViewBag.num = num;
                ViewBag.id = Session["id"];
                ViewBag.username = Session["username"];
                ViewBag.DeptID = Session["DeptID"];
                SelectCustomerAll arr = new GetUpdateCustomer().GetUpdateCustomerSql(CustomerID);
                if (arr == null)
                {
                    SelectCustomerAll selectCustomerAll = new SelectCustomerAll();
                    arr = selectCustomerAll;
                }
                List<Contacts> selectsql = new GetUpdateCustomer().GetUpdateCustomerTable(CustomerID, arr.SysID);
                if (selectsql == null)
                {
                    selectsql = new List<Contacts>();
                }
                ViewBag.All = selectsql;
                //审核权限
                if (this.GetShenP(base.Session["id"].ToString()) != 0)
                {
                    ViewBag.SP = D;
                }
                //显示补充意见
                AuditInfo sql = new GetDetail().GetDetailBcyjSql(CustomerID, createBy, createDept);
                if (sql == null)
                {
                    sql = new AuditInfo();
                }
                ViewBag.AuditInfo = sql;
            }
            catch (Exception ex)
            {
                return base.Content("查询失败," + ex.Message);
            }
            return base.View();
        }
        #endregion
        #region API
        /// <summary>
        /// 查询所属省份-新增/修改
        /// </summary>
        /// <param name="cityid">城市ID</param>
        /// <returns></returns>
        public string SelectProvinceId(string cityid)
        {
            if (cityid == null || cityid == "")
            {
                return "";
            }
            Others city = new Other().GetProvinceId(cityid);
            if (city == null)
            {
                city.id = 0;
            }
            return city.id.ToString();
        }

        /// <summary>
        /// 查询员工所属部门-新增
        /// </summary>
        /// <param name="EmpID">员工ID</param>
        /// <returns></returns>
        public string SelectDeptID(string EmpID)
        {
            return new Other().GetDeptID(EmpID).Text.ToString();
        }

        /// <summary>
        /// /查询所在城市-新增
        /// </summary>
        /// <param name="Cityid">城市ID</param>
        /// <returns></returns>
        public string SelectCityID(string Cityid)
        {
            return new Other().GetCityID(Cityid).Text.ToString();
        }

        /// <summary>
        /// 根据类型名获得类型编号-修改
        /// </summary>
        /// <param name="TypeName"></param>
        /// <returns></returns>
        public string SelectContactType(string TypeName)
        {
            return new GetUpdateCustomer().GetContactType(TypeName);
        }
        #region 下拉框
        [IsSessionExpire(IsCheck = false)]
        /// <summary>
        /// 查询出Default行业下拉框绑定
        /// </summary>
        /// <returns></returns>
        /// </summary>
        /// <returns></returns>
        public string SelectTrade()
        {
            var trade = new GetDropDownList().GetTrade();
            object obj = new
            {
                DropDownList = trade
            };
            ResponseJson json = new ResponseJson() { Success = 0, Data = obj };
            if (trade == null)
            {
                json = new ResponseJson() { Success = 1, Data = obj };
            }
            return JsonConvert.SerializeObject(json);
        }

        /// <summary>
        /// 查询出所有行业/公司下拉框绑定
        /// </summary>
        /// <param name="WordCode"></param>
        /// <returns></returns>
        public string SelectTBWord(string WordCode)
        {
            if (WordCode == null)
            {
                WordCode = "";
            }
            var trade = new GetDropDownList().GetTBWord(WordCode);
            object obj = new
            {
                DropDownList = trade
            };
            ResponseJson json = new ResponseJson
            {
                Success = 0,
                Data = obj
            };
            if (trade == null)
            {
                json = new ResponseJson
                {
                    Success = 1,
                    Data = obj
                };
            }
            return JsonConvert.SerializeObject(json);
        }

        /// <summary>
        /// 查询出所有国家下拉框绑定
        /// </summary>
        /// <returns></returns>
        public string SelectCountry()
        {
            var Country = new GetDropDownList().GetCountry();
            object obj = new
            {
                DropDownList = Country
            };
            ResponseJson json = new ResponseJson
            {
                Success = 0,
                Data = obj
            };
            if (Country == null)
            {
                json = new ResponseJson
                {
                    Success = 1,
                    Data = obj
                };
            }
            return JsonConvert.SerializeObject(json);
        }

        /// <summary>
        /// 二级联动-城市下拉框绑定
        /// </summary>
        /// <param name="Country">国家ID</param>
        /// <returns></returns>
        public string SelectCity(string Country)
        {
            var city = new GetDropDownList().GetCity(Country);
            object obj = new
            {
                DropDownList = city
            };
            ResponseJson json = new ResponseJson
            {
                Success = 0,
                Data = obj
            };
            if (city == null)
            {
                json = new ResponseJson
                {
                    Success = 1,
                    Data = obj
                };
            }
            return JsonConvert.SerializeObject(json);
        }

        /// <summary>
        /// 三级联动-州省下拉框绑定
        /// </summary>
        /// <param name="Country">国家ID</param>
        /// <returns></returns>
        public string SelectProvince(string Country)
        {
            var Province = new GetDropDownList().GetProvince(Country);
            object obj = new
            {
                DropDownList = Province
            };
            ResponseJson json = new ResponseJson
            {
                Success = 0,
                Data = obj
            };
            if (Province == null)
            {
                json = new ResponseJson
                {
                    Success = 1,
                    Data = obj
                };
            }
            return JsonConvert.SerializeObject(json);
        }

        /// <summary>
        ///三级联动-城市下拉框绑定
        /// </summary>
        /// <param name="Province">州省ID</param>
        /// <returns></returns>
        public string SelectLDCity(string Province)
        {
            var city = new GetDropDownList().GetLDCity(Province);
            object obj = new
            {
                DropDownList = city
            };
            ResponseJson json = new ResponseJson
            {
                Success = 0,
                Data = obj
            };
            if (city == null)
            {
                json = new ResponseJson
                {
                    Success = 1,
                    Data = obj
                };
            }
            return JsonConvert.SerializeObject(json);
        }

        /// <summary>
        /// 查询出客户性质下拉框绑定
        /// </summary>
        /// <returns></returns>
        public string SelectAllClass()
        {
            var Class = new GetDropDownList().GetAllClass();
            object obj = new
            {
                DropDownList = Class
            };
            ResponseJson json = new ResponseJson
            {
                Success = 0,
                Data = obj
            };
            if (Class == null)
            {
                json = new ResponseJson
                {
                    Success = 1,
                    Data = obj
                };
            }
            return JsonConvert.SerializeObject(json);
        }

        /// <summary>
        /// 查询出客户递属下拉框绑定
        /// </summary>
        /// <returns></returns>
        public string SelectAllBelongs()
        {
            var Belongs = new GetDropDownList().GetAllBelongs();
            object obj = new
            {
                DropDownList = Belongs
            };
            ResponseJson json = new ResponseJson
            {
                Success = 0,
                Data = obj
            };
            if (Belongs == null)
            {
                json = new ResponseJson
                {
                    Success = 1,
                    Data = obj
                };
            }
            return JsonConvert.SerializeObject(json);
        }

        /// <summary>
        /// 查询出货币下拉框绑定
        /// </summary>
        /// <returns></returns>
        public string SelectCurr()
        {
            var curr = new GetDropDownList().GetCurr();
            object obj = new
            {
                DropDownList = curr
            };
            ResponseJson json = new ResponseJson
            {
                Success = 0,
                Data = obj
            };
            if (curr == null)
            {
                json = new ResponseJson
                {
                    Success = 1,
                    Data = obj
                };
            }
            return JsonConvert.SerializeObject(json);
        }

        /// <summary>
        /// 修改页面模态框联系类型下拉框查询
        /// </summary>
        /// <returns></returns>
        public string SelectLXType()
        {
            var Type = new GetDropDownList().GetLXType();
            object obj = new
            {
                DropDownList = Type
            };
            ResponseJson json = new ResponseJson
            {
                Success = 0,
                Data = obj
            };
            if (Type == null)
            {
                json = new ResponseJson
                {
                    Success = 1,
                    Data = obj
                };
            }
            return JsonConvert.SerializeObject(json);
        }

        /// <summary>
        /// 修改页面模态框城市下拉框查询
        /// </summary>
        /// <returns></returns>
        public string SelectLXCity()
        {
            var City = new GetDropDownList().GetLXCity();
            object obj = new
            {
                DropDownList = City
            };
            ResponseJson json = new ResponseJson
            {
                Success = 0,
                Data = obj
            };
            if (City == null)
            {
                json = new ResponseJson
                {
                    Success = 1,
                    Data = obj
                };
            }
            return JsonConvert.SerializeObject(json);
        }
        #endregion
        #region 权限查询
        /// <summary>
        ///  权限查询
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="createBy"></param>
        /// <param name="createDept"></param>
        /// <returns></returns>
        public int Authority(string userid, string createBy, string createDept)
        {
            //权限
            string username = (base.Session["username"] != null) ? base.Session["username"].ToString() : null;
            if (username == null || username == "")
            {
                userid = "";
                base.Session["username"] = this.GetUsername(userid);
                ViewBag.Name = GetUsername(userid);
                username = base.Session["username"].ToString();
            }
            if (base.Session["DeptID"] == null)
            {
                base.Session["DeptID"] = "";
            }
            string DeptID = this.GetUser(username);
            base.Session["DeptID"] = DeptID;
            string Position = this.GetPosition(username);
            //修改权限
            return this.InfoPermission(username, DeptID, Position, createBy, createDept);
        }

        /// <summary>
        /// 获得登陆人姓名
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetUsername(string userid)
        {
            if (userid == null || userid == "")
            {
                return "";
            }
            return new Authority().GetUsernameSql(userid).Text.ToString();
        }

        /// <summary>
        /// 查询权限
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
            return new Authority().GetQuanSql(userid, PermissionCode).id;
        }

        /// <summary>
        ///获得登陆人部门信息
        /// </summary>
        /// <param name="EmpID"></param>
        /// <returns></returns>
        public string GetUser(string EmpID)
        {
            if (EmpID == null || EmpID == "")
            {
                return "";
            }
            return new Authority().GetUserSql(EmpID).Text;
        }

        /// <summary>
        /// 获得登陆人职位信息
        /// </summary>
        /// <param name="EmpID"></param>
        /// <returns></returns>
        public string GetPosition(string EmpID)
        {
            if (EmpID == null || EmpID == "")
            {
                return "";
            }
            return new Authority().GetPositionSql(EmpID).Text.ToString();
        }

        /// <summary>
        /// 修改权限
        /// </summary>
        /// <param name="login_username"></param>
        /// <param name="login_Dept"></param>
        /// <param name="login_Position"></param>
        /// <param name="createBy"></param>
        /// <param name="createDept"></param>
        /// <returns></returns>
        public int InfoPermission(string login_username, string login_Dept, string login_Position, string createBy, string createDept)
        {
            int Record = 0;
            if (login_username != "")
            {
                if (login_username == "ADM")
                {
                    Record = 4;
                }
                else if (login_username == createBy)
                {
                    Record = 4;
                }
                else if (login_username == "GUS")
                {
                    Record = 2;
                }
                else if (createDept.Contains(login_Dept) || (createBy == "" && createDept == ""))
                {
                    if (login_Position == "M")
                    {
                        Record = 4;
                    }
                    else
                    {
                        Record = 3;
                    }
                }
                else
                {
                    Record = 1;
                }
            }
            return Record;
        }

        /// <summary>
        /// 是否有审核权限
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int GetShenP(string userid)
        {
            if (userid == null || userid == "")
            {
                return 0;
            }
            return new Authority().GetShenPSql(userid).id;
        }

        /// <summary>
        /// 是否有禁用/解禁权限
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int GetJinYong(string userid)
        {
            if (userid == null || userid == "")
            {
                return 0;
            }
            return new Authority().GetJinYongSql(userid).id;
        }
        #endregion
        #endregion
    }
}
