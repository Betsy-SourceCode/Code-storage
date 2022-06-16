using MIS_CertificationApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MIS_CertificationApplication.Controllers
{
    public class CertificationApplicationController : Controller
    {
        WebStationEntities db = new WebStationEntities();
        #region 主页面
        // GET: CertificationApplication
        /// <summary>
        /// Application Index界面 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string userid)
        {
            userid = "444";
            Session["userid"] = userid;
            return View();
        }
        /// <summary>
        /// Application IndexPrinting界面 首页-打印页面
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ActionResult IndexPrinting()
        {
            //将登录人三字代码传给打印人
            if (Session["userid"] != null)
            {
                ViewBag.username = new Authority().GetUserSql(Session["userid"].ToString());
            }
            return View();
        }
        /// <summary>
        /// Application FunctionInterface  新增
        /// </summary>
        /// <param name="type">1-新增，2-复制，3-修改</param>
        /// <param name="ApplicationRef"></param>
        /// <returns></returns>
        public ActionResult FunctionInterface(int type, string ApplicationRef)
        {
            ViewBag.userid = Session["userid"].ToString();
            //非新增给页面赋默认值
            if (type != 1)
            {
                string sql = "select * from dbo.View_AllDataApplicationList where ApplicationRef='" + ApplicationRef + "' and ApplicationRef is not null";
                View_AllDataApplicationList list = db.Database.SqlQuery<View_AllDataApplicationList>(sql).FirstOrDefault();
                List<View_AllDataApplicationList> Sonlist = db.Database.SqlQuery<View_AllDataApplicationList>(sql).ToList();
                //productModel
                string modelSql = @"select top 1 STUFF((SELECT ';' + ModelCode FROM component_Model t2 where CPSerial in ({0}) FOR XML PATH('')), 1, 1, '') AS model  from component_Model cm ";
                string model = list.ProductModel.Replace('|', ',');
                modelSql = string.Format(modelSql, model);
                list.ProductModel = db.Database.SqlQuery<string>(modelSql).FirstOrDefault();
                //主表的文件后缀名
                if (list.QuoteFile != null)
                {
                    ViewBag.MainfileName = list.QuoteFileName.Substring(list.QuoteFileName.LastIndexOf(".") + 1, list.QuoteFileName.ToString().Length - (list.QuoteFileName.LastIndexOf(".") + 1));
                }
                ViewBag.Data = list;
                foreach (var item in Sonlist)
                {

                    string countrySql = @"select top 1 STUFF((SELECT ';' +'('+Icode+')'+ cntyName_s FROM Country t2 where Icode in({0})  FOR XML PATH('')), 1, 1, '') AS country from Country cm ";

                    item.CoverAreas = "'" + item.CoverAreas + "'";
                    string country = item.CoverAreas.Replace('|', ',');
                    var arr = country.Split(',');
                    string countryList = "";
                    for (int i = 0; i < arr.Length; i++)
                    {

                        if (i == 0 && arr[i] != "''")
                        {
                            countryList += arr[i] + "',";
                        }
                        else if (i == arr.Length - 1 && arr[i] != "''")
                        {
                            countryList += "'" + arr[i];
                        }
                        else if (arr[i] == "''")
                        {
                            countryList += "'" + arr[i] + "'";
                        }
                        else
                        {
                            countryList += "'" + arr[i] + "',";
                        }

                    }
                    countrySql = string.Format(countrySql, countryList);
                    item.CoverAreas = db.Database.SqlQuery<string>(countrySql).FirstOrDefault();
                }
                ViewBag.SonData = Sonlist;
            }
            ViewBag.type = type;
            return View();
        }
        /// <summary>
        /// Application Edit界面  编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            return View();
        }
        /// <summary>
        /// Application Detail界面  详情
        /// </summary>
        /// <param name="CertificateRef">主表的申请编号</param>
        /// <returns></returns>
        public ActionResult Detail(string ApplicationRef)
        {
            if (ApplicationRef == null)
            {
                ViewBag.Data = new View_AllDataApplicationList();
                ViewBag.SonData = new List<View_AllDataApplicationList>();
            }
            else
            {
                string sql = "select * from dbo.View_AllDataApplicationList where ApplicationRef='" + ApplicationRef + "' and ApplicationRef is not null";
                View_AllDataApplicationList list = db.Database.SqlQuery<View_AllDataApplicationList>(sql).FirstOrDefault();
                List<View_AllDataApplicationList> Sonlist = db.Database.SqlQuery<View_AllDataApplicationList>(sql).ToList();
                //productModel
                string modelSql = @"select top 1 STUFF((SELECT ';' + ModelCode FROM component_Model t2 where CPSerial in ({0}) FOR XML PATH('')), 1, 1, '') AS model  from component_Model cm ";
                string model = list.ProductModel.Replace('|', ',');
                modelSql = string.Format(modelSql, model);
                list.ProductModel = db.Database.SqlQuery<string>(modelSql).FirstOrDefault();
                //主表的文件后缀名
                if (list.QuoteFile != null)
                {
                    ViewBag.MainfileName = list.QuoteFileName.Substring(list.QuoteFileName.LastIndexOf(".") + 1, list.QuoteFileName.ToString().Length - (list.QuoteFileName.LastIndexOf(".") + 1));
                }
                ViewBag.Data = list;
                foreach (var item in Sonlist)
                {

                    string countrySql = @"select top 1 STUFF((SELECT ';' +'('+Icode+')'+ cntyName_s FROM Country t2 where Icode in({0})  FOR XML PATH('')), 1, 1, '') AS country from Country cm ";

                    item.CoverAreas = "'" + item.CoverAreas + "'";
                    string country = item.CoverAreas.Replace('|', ',');
                    var arr = country.Split(',');
                    string countryList = "";
                    for (int i = 0; i < arr.Length; i++)
                    {

                        if (i == 0 && arr[i] != "''")
                        {
                            countryList += arr[i] + "',";
                        }
                        else if (i == arr.Length - 1 && arr[i] != "''")
                        {
                            countryList += "'" + arr[i];
                        }
                        else if (arr[i] == "''")
                        {
                            countryList += "'" + arr[i] + "'";
                        }
                        else
                        {
                            countryList += "'" + arr[i] + "',";
                        }

                    }
                    countrySql = string.Format(countrySql, countryList);
                    item.CoverAreas = db.Database.SqlQuery<string>(countrySql).FirstOrDefault();
                }
                ViewBag.SonData = Sonlist;
            }
            ViewBag.userid = Session["userid"].ToString();
            return View();
        }
        #endregion

        #region 子页面
        /// <summary>
        /// Product-Model-Update界面   产品模型管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Product_Model_Update()
        {
            return View();
        }
        /// <summary>
        /// Certificate_Update界面  认证管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Certificate_Update()
        {
            return View();
        }
        /// <summary>
        /// Select-Factories界面   子表中工厂多选框
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectFactories()
        {
            return View();
        }
        /// <summary>
        /// Select-Factories界面   子表中国家多选框
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectCountriesAreas()
        {
            return View();
        }
        /// <summary>
        /// Select-Factories界面   子表中型号多选框
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectModels()
        {
            return View();
        }
        #endregion
    }
}