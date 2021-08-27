using Customer.Models.SqlMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KYH_JigFixture.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string OAFlow_Num, DateTime ApplyDate, string Mat_Code, string Mat_Name)
        {
            //分页
            var temp = new GetIndex().GetIndexPageRowOA(OAFlow_Num, ApplyDate, Mat_Code, Mat_Name);
            var rowCount = temp.Count; //总行数
            //进行分页
            return View();
        }
    }
}