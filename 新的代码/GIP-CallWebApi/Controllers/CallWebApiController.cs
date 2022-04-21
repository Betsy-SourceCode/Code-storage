using GIP_CallWebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GIP_CallWebApi.Controllers
{
    public class CallWebApiController : Controller
    {
        public static string token;
        public ActionResult Index(string token)
        {
            ViewBag.Token = token;
            return View();
        }
        public ActionResult Login(string userid)
        {
            return View();
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="url"></param>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string LoginData(string url,string account,string password)
        {
            try
            {
               var LoginResult = HttpHelper.HttpPost(1,url, "{ \"account\":\"" + account + "\",\"password\":\"" + password + "\"}","");
                //var LoginResult = "aa";
                if (LoginResult != "")
                {
                    //取token值赋值给全局变量
                    token = LoginResult;
                    var obj = new
                    {
                        code = "0",
                        data = LoginResult.ToString()
                    };
                    string aa = JsonConvert.SerializeObject(obj);
                    return aa;
                }
                else
                {
                    return JsonConvert.SerializeObject(LoginResult);
                    //DevExpress.XtraEditors.XtraMessageBox.Show("登录失败，账号或者密码错误请检查后重新尝试登录！");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }

        }
        [HttpPost]
        /// <summary>
        /// 下拉多选框的数据查询
        /// </summary>
        /// <param name="typeid">点击的页签  1-Factory_processes 2-Factory_reworks</param>
        /// <param name="date">选择框的日期</param>
        /// <returns>下拉框的数据</returns>
        public string  DataList(int typeid, string date)
        {
            try
            {
                if (typeid == 1)
                {
                    return JsonConvert.SerializeObject(SqlStoredProcedure.GetGDnumberList(1,date));
                }
                if (typeid == 2)
                {
                    return JsonConvert.SerializeObject(SqlStoredProcedure.GetGDnumberList(2,date));
                }
                if (typeid == 3)
                {
                    return JsonConvert.SerializeObject(SqlStoredProcedure.GetGDnumberList(3,date));
                }
                return JsonConvert.SerializeObject("");
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }
        }

    }
}