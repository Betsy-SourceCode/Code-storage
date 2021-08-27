using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KYH_JigFixture.Models;

namespace KYH_JigFixture.Controllers
{
    public class JigFixtureZGCController : Controller
    {
        WebStationEntities db = new WebStationEntities();
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 新增/修改
        /// </summary>
        /// <param name="num">1-新增，2-修改</param>
        /// <param name="TAT">Tools_Acquire_Trace新表</param>
        /// <returns></returns>
        public string AddUpdateFunction(int num, List<Tools_Acquire_Trace> TAT)
        {
            try
            {
                //新增
                if (num == 0)
                {

                }
                else
                {
                    //修改
                }
                return null;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}