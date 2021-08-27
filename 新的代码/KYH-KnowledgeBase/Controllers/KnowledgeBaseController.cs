using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using KYH_KnowledgeBase.Filters;
using KYH_KnowledgeBase.Models;
using KYH_KnowledgeBase.Models.PublicSqlMethods;
using KYH_KnowledgeBase.Models.SqlMethods;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;

namespace KYH_KnowledgeBase.Controllers
{
    [Localization]
    [IsSessionExpire(IsCheck = true)]
    public class KnowledgeBaseController : Controller
    {
        WebStationEntitiess db = new WebStationEntitiess();
        public string username;
        public bool C = true; //添加权限参数控制
        private Authority authority = new Authority();
        public static int Logon = 1;
        #region 页面显示
        /// <summary>
        /// 系统技术知识库首页
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="KeyWord"></param>
        /// <param name="TopicArea"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [IsSessionExpire(IsCheck = false)]
        public ActionResult Index(string userid, string KeyWord, string TopicArea, int pageIndex = 1, int pageSize = 10)
        {
            ActionResult result;
            try
            {
                userid = "444";
                string username = this.GetUsername(userid);
                string DeptIDs = this.GetUser(username);
                this.GetPosition(username);
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
                if (TopicArea == null)
                {
                    TopicArea = "";
                }
                List<SelectKnowledgeBaseIndex> temp = new GetIndex().GetIndexPageRow(KeyWord, TopicArea);
                int count = temp.Count;
                temp = temp.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList<SelectKnowledgeBaseIndex>(); //分页
                if (this.GetQuan(userid, "KnowledgeBase_C") == 0)
                {
                    this.C = false;
                    ViewBag.User = C;
                }
                else
                {
                    ViewBag.User = C;
                }
                ViewBag.pageIndex = pageIndex;
                ViewBag.userid = userid;
                ViewBag.AllCustomer = temp;
                string UserID = base.Session["username"].ToString();
                if (UserID != null && UserID != "")
                {
                    string name = this.authority.SelectEName(UserID);
                    string DeptID = this.authority.SelectDeptID(UserID);
                    string ip = LogThread.GetLocalIP();
                    if (KnowledgeBaseController.Logon == 1)
                    {
                        LogThread.ActionLog(UserID, DeptID, "从电脑" + ip + "登入 WebStation 系统", "S", name);
                        KnowledgeBaseController.Logon = 0;
                    }
                }
                result = base.View();
            }
            catch (Exception ex)
            {
                result = base.Content("查询失败," + ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 新增系统技术知识记录页面
        /// </summary>
        /// <param name="QnAID"></param>
        /// <returns></returns>
        public ActionResult AddKnowledgeBase(int QnAID)
        {
            try
            {
                if (base.Session["id"] == null)
                {
                    base.Session["id"] = "";
                }
                //进入页面再判断一次
                string userid = base.Session["id"].ToString();
                int result = GetQuan(userid, "KnowledgeBase_C"); //查看是否有添加权限,参数控制
                if (result == 0)
                {
                    C = false;
                    ViewBag.User = C;
                }
                else
                {
                    ViewBag.User = C;
                }
                QnA found = this.db.QnA.FirstOrDefault((QnA e) => e.QnAID == QnAID);
                ViewBag.AllCustomer = found;
                ViewBag.QnAID = QnAID;
                ViewBag.userid = Session["id"];
            }
            catch (Exception ex)
            {
                return base.Content("查询失败," + ex.Message);
            }
            return base.View();
        }

        /// <summary>
        /// 系统技术知识记录详细页面
        /// </summary>
        /// <param name="QnAID"></param>
        /// <returns></returns>
        public ActionResult DetailsKnowledgeBase(int QnAID)
        {
            try
            {
                if (base.Session["id"] == null)
                {
                    base.Session["id"] = "";
                }
                QnA found = this.db.QnA.FirstOrDefault((QnA e) => e.QnAID == QnAID);
                ViewBag.AllCustomer = found;
                ViewBag.CreateBy = authority.SelectCreateUpdateDept(1, "CreateBy", QnAID);
                ViewBag.CreateDept = authority.SelectCreateUpdateDept(2, "CreateBy", QnAID);
                ViewBag.UpdateBy = authority.SelectCreateUpdateDept(1, "UpdateBy", QnAID);
                ViewBag.UpdateDept = authority.SelectCreateUpdateDept(2, "UpdateBy", QnAID);
                ViewBag.userid = Session["id"];
            }
            catch (Exception ex)
            {
                return base.Content("查询失败," + ex.Message);
            }
            return base.View();
        }

        /// <summary>
        /// 修改系统技术知识记录页面
        /// </summary>
        /// <param name="QnAID"></param>
        /// <returns></returns>
        public ActionResult UpdateKnowledgeBase(int QnAID)
        {
            try
            {
                if (base.Session["id"] == null)
                {
                    base.Session["id"] = "";
                }
                QnA found = this.db.QnA.FirstOrDefault((QnA e) => e.QnAID == QnAID);
                ViewBag.AllCustomer = found;
                ViewBag.CreateBy = authority.SelectCreateUpdateDept(1, "CreateBy", QnAID);
                ViewBag.CreateDept = authority.SelectCreateUpdateDept(2, "CreateBy", QnAID);
                ViewBag.UpdateBy = authority.SelectCreateUpdateDept(1, "UpdateBy", QnAID);
                ViewBag.UpdateDept = authority.SelectCreateUpdateDept(2, "UpdateBy", QnAID);
                ViewBag.userid = Session["id"];
            }
            catch (Exception ex)
            {
                return base.Content("查询失败," + ex.Message);
            }
            return base.View();
        }

        /// <summary>
        /// 打印系统技术知识记录详细页面
        /// </summary>
        /// <param name="QnAID"></param>
        /// <returns></returns>
        public ActionResult DetailsPrinting(int QnAID)
        {
            try
            {
                if (base.Session["id"] == null)
                {
                    base.Session["id"] = "";
                }
                QnA found = this.db.QnA.FirstOrDefault((QnA e) => e.QnAID == QnAID);
                ViewBag.AllCustomer = found;
                ViewBag.CreateBy = authority.SelectCreateUpdateDept(1, "CreateBy", QnAID);
                ViewBag.CreateDept = authority.SelectCreateUpdateDept(2, "CreateBy", QnAID);
                ViewBag.UpdateBy = authority.SelectCreateUpdateDept(1, "UpdateBy", QnAID);
                ViewBag.UpdateDept = authority.SelectCreateUpdateDept(2, "UpdateBy", QnAID);
                ViewBag.userid = Session["id"];
                ViewBag.username = Session["username"];
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
        /// 显示首页列表
        /// </summary>
        /// <param name="KeyWord"></param>
        /// <param name="TopicArea"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public string QueryIndexData(string KeyWord, string TopicArea, int pageIndex = 1, int pageSize = 10)
        {
            string result;
            try
            {
                List<SelectKnowledgeBaseIndex> temp = new GetIndex().GetIndexPageRow(KeyWord, TopicArea);
                int rowCount = temp.Count;
                temp = temp.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList<SelectKnowledgeBaseIndex>();
                PageJson json = new PageJson
                {
                    success = 0,
                    count = rowCount,
                    size = pageSize,
                    data = temp
                };
                if (temp == null)
                {
                    json = new PageJson
                    {
                        success = 1,
                        count = 0,
                        size = pageSize,
                        data = ""
                    };
                }
                result = JsonConvert.SerializeObject(json);
            }
            catch (Exception)
            {
                result = JsonConvert.SerializeObject(new PageJson
                {
                    success = 1,
                    count = 0,
                    size = pageSize,
                    data = ""
                });
            }
            return result;
        }

        /// <summary>
        /// 权限查询
        /// </summary>
        /// <param name="createBy">创建人</param>
        /// <param name="createDept">创建部门</param>
        /// <returns></returns>
        public int Authority(string createBy, string createDept)
        {
            string userid = base.Session["id"].ToString();
            string username = (base.Session["username"] != null) ? base.Session["username"].ToString() : null;
            if (username == null || username == "")
            {
                userid = "";
                Session["username"] = this.GetUsername(userid);
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
        /// 获得登陆人部门信息
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
        /// <param name="login_username">登录人</param>
        /// <param name="login_Dept">登录人部门</param>
        /// <param name="login_Position">登录人职位</param>
        /// <param name="createBy">信息创建人</param>
        /// <param name="createDept">信息创建部门</param>
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
        /// 查询出板块下拉框绑定
        /// </summary>
        /// <returns></returns>
        public string SelectBK(string Language, string id)
        {
            object obj = new
            {
                DropDownList = new GetBKDropDownList().GetBK(Language, id)
            };
            ResponseJson responseJson = new ResponseJson();
            responseJson.Success = 0;
            responseJson.Data = obj;
            return JsonConvert.SerializeObject(new ResponseJson
            {
                Success = 1,
                Data = obj
            });
        }
        #endregion
    }
}
