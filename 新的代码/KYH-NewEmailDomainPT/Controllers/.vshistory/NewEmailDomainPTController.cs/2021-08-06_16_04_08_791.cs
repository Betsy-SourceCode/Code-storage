using KYH_NewEmailDomainPT.Models;
using KYH_NewEmailDomainPT.Models.PublicSqlMethods;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KYH_NewEmailDomainPT.Controllers
{
    public class NewEmailDomainPTController : Controller
    {
        WebStationEntities db = new WebStationEntities();
        // GET: NewEmailDomainPT
        public ActionResult Index()
        {
            DateTime now = DateTime.Now;
            DateTime dt = new DateTime(now.Year, now.Month, 1);  //取当月第一天
            var date = dt.ToString("yyyy-MM-dd");
            var today = now.ToString("yyyy-MM-dd");
            ViewBag.date = date;  //传值到时间控件里
            ViewBag.today = today;  //传值到时间控件里
            return View();
        }
        /// <summary>
        /// 查询首页列表
        /// </summary>
        /// <param name="NewEmailDomain"></param>
        /// <param name="EndTime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public string IndexData(NewEmailDomain2GIP NewEmailDomain, string EndTime, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                List<NewEmailDomain2GIP> data = new GetIndex().GetIndexListSql(NewEmailDomain, EndTime);
                var rowCount = data.Count; //总条数
                data = data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList<NewEmailDomain2GIP>();//分页
                ResponseJson json = new ResponseJson
                {
                    Data = data,
                    count = rowCount
                };
                return JsonConvert.SerializeObject(json);
            }
            catch (Exception)
            {
                return null;
                throw;
            }

        }
        /// <summary>
        /// 修改新到的电邮域名信息
        /// </summary>
        /// <param name="NewEmailDomain"></param>
        /// <returns></returns>
        public int Update(NewEmailDomain2GIP NewEmailDomain)
        {
            try
            {
                DbEntityEntry<NewEmailDomain2GIP> entry = this.db.Entry<NewEmailDomain2GIP>(NewEmailDomain);
                entry.State = EntityState.Modified;
                int a = this.db.SaveChanges();
                return a;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());
                return 0;
                throw;
            }

        }
    }
}