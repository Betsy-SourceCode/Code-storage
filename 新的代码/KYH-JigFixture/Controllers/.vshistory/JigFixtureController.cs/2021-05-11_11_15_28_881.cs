using Customer.Models.SqlMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KYH_JigFixture.Controllers
{
    public class JigFixtureController : Controller
    {/// <summary>
     /// 首页显示SQL
     /// </summary>
     /// <param name="OAFlow_Num"></param>
     /// <param name="ApplyDate"></param>
     /// <param name="Mat_Code"></param>
     /// <param name="Mat_Name"></param>
     /// <param name="pageIndex"></param>
     /// <param name="pageSize"></param>
     /// <param name="pageRow"></param>
     /// <returns></returns>
        public ActionResult Index(string OAFlow_Num, string ApplyDate, string Mat_Code, string Mat_Name, int pageIndex = 1, int pageSize = 10, int pageRow = 0)
        {
            DateTime now = DateTime.Now;
            DateTime dt = new DateTime(now.Year, now.Month, 1);  //取当月第一天
            var date = dt.ToString("yyyy-MM-dd");
            var today = now.ToString("yyyy-MM-dd");
            ViewBag.date = date;  //传值到时间控件里
            ViewBag.today = today;  //传值到时间控件里
            var temp = new GetIndex().GetIndexPageRowOA(OAFlow_Num, ApplyDate, Mat_Code, Mat_Name);
            var rowCount = temp.Count; //总行数
            //分页
            temp = temp.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.count = rowCount;  //总条数
            // 获得总页数
            if (rowCount % pageSize == 0)
            {
                pageRow = rowCount / pageSize;
            }
            else
            {
                pageRow = (rowCount / pageSize) + 1;
            }
            ViewBag.pageIndex = pageIndex;  //当前页
            ViewBag.pageCount = pageRow; //总页数
            ViewBag.All = temp;
            return View();
        }

        /// <summary>
        /// 查询K3订单代入信息
        /// </summary>
        /// <returns></returns>
        public int SelectK3PO_Num()
        {
            return 0;

        }
    }
}