using KYH_NewEmailDomainPT.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public string IndexData(NewEmailDomain2GIP NewEmailDomain, string EndTime,int pageIndex = 1, int pageSize = 10, int pageRow = 0)
        {
            List<NewEmailDomain2GIP> data = new GetIndex().GetIndexListSql(NewEmailDomain, EndTime);
            var rowCount = data.Count; //总条数
             //分页
            temp = temp.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList<SelectOA_Result>();
            return JsonConvert.SerializeObject(data);
        }
    }
}