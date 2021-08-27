using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KYH_JigFixture.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //分页
            var temp = new GetIndex().GetIndexPageRow(KeyWord, TopicArea);
            var rowCount = temp.Count; //总行数
            temp = temp.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return View();
        }
    }
}