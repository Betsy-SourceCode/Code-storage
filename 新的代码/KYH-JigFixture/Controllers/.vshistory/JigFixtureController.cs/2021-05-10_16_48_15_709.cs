using Customer.Models.SqlMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KYH_JigFixture.Controllers
{
    public class JigFixtureController : Controller
    {
        public ActionResult Index(string OAFlow_Num, string ApplyDate, string Mat_Code, string Mat_Name, int pageIndex = 1, int pageSize = 10)
        {
            DateTime now = DateTime.Now;
            DateTime dt = new DateTime(now.Year, now.Month, 1);  //取当月第一天
            var date = dt.ToString("yyyy-MM-dd");
            ViewBag.date = date;  //传值到时间控件里
            var temp = new GetIndex().GetIndexPageRowOA(OAFlow_Num, ApplyDate, Mat_Code, Mat_Name);
            var rowCount = temp.Count; //总行数
            //分页
            temp = temp.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.All = temp;
            return View();
        }
    }
}