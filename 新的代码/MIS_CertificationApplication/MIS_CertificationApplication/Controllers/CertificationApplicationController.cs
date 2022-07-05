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
            //userid = "298"; //财务部
            //userid = "444";
            if (userid == null)
            {
                userid = "";
            }
            Session["userid"] = userid;
            if (new Authority().GetQuanSql(userid, "Cert_A") > 0)
            {
                ViewBag.Add = 1;  //新增安规认证信息
            }
            //if (new Authority().GetQuanSql(userid, "CertMaster_A") > 0)
            //{
            //    ViewBag.Upadate = 1;  //维护安规认证主表
            //}
            ViewBag.userid = userid;
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
        /// Application Detail界面  详情
        /// </summary>
        /// <param name="CertificateRef">主表的申请编号</param>
        /// <returns></returns>
        public ActionResult Detail(string ApplicationRef)
        {
            try
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
                        //Factories
                        string factoriesSql = @"select top 1 STUFF((SELECT ';' + Name_cn FROM TBWords t2 where WordCode='FC' and Value in({0}) FOR XML PATH('')), 1, 1, '') AS Value  from TBWords cm ";
                        if (item.Factories != null)
                        {
                            item.Factories = "'" + item.Factories + "'";
                            string factoriesList = "";
                            if (item.Factories.IndexOf("|") != -1) //代表一个以上
                            {
                                string factories = item.Factories.Replace('|', ',');
                                var factoriesArr = factories.Split(',');
                                for (int i = 0; i < factoriesArr.Length; i++)
                                {

                                    if (i == 0 && factoriesArr[i] != "''")
                                    {
                                        factoriesList += factoriesArr[i] + "',";
                                    }
                                    else if (i == factoriesArr.Length - 1 && factoriesArr[i] != "''")
                                    {
                                        factoriesList += "'" + factoriesArr[i];
                                    }
                                    else if (factoriesArr[i] == "''")
                                    {
                                        factoriesList += "'" + factoriesArr[i] + "'";
                                    }
                                    else
                                    {
                                        factoriesList += "'" + factoriesArr[i] + "',";
                                    }

                                }
                            }
                            else
                            {
                                factoriesList = item.Factories;
                            }
                            factoriesSql = string.Format(factoriesSql, factoriesList);
                            item.Factories = db.Database.SqlQuery<string>(factoriesSql).FirstOrDefault();
                        }
                        else
                        {
                            item.Factories = "";
                        }
                        //country
                        string countrySql = @"select top 1 STUFF((SELECT ';' +'('+Icode+')'+ cntyName_s FROM Country t2 where Icode in({0})  FOR XML PATH('')), 1, 1, '') AS country from Country cm ";
                        if (item.CoverAreas != null)
                        {

                            item.CoverAreas = "'" + item.CoverAreas + "'";
                            string countryList = "";
                            if (item.Factories.IndexOf("|") != -1) //代表一个以上
                            {
                                string country = item.CoverAreas.Replace('|', ',');
                                var arr = country.Split(',');
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
                            }
                            else
                            {
                                countryList = item.CoverAreas;
                            }
                            countrySql = string.Format(countrySql, countryList);
                            item.CoverAreas = db.Database.SqlQuery<string>(countrySql).FirstOrDefault();
                        }
                        else
                        {
                            item.CoverAreas = "";
                        }
                    }
                    ViewBag.SonData = Sonlist;
                }
                ViewBag.userid = Session["userid"].ToString();
                return View();
            }
            catch (Exception ex)
            {

                LogHelper.Write(ex.Message);
                return Content("<script languge='javascript'>alert('发生错误，请联系电脑部！内部成员请查看日志文件');window.history.back();</script>");
                throw;
            }
        }

        /// <summary>
        /// Application FunctionInterface 功能界面
        /// </summary>
        /// <param name="type">1-新增，2-复制，3-修改</param>
        /// <param name="ApplicationRef"></param>
        /// <returns></returns>
        public ActionResult FunctionInterface(int type, string ApplicationRef)
        {
            try
            {
                ViewBag.userid = Session["userid"].ToString();
                //非新增给页面赋默认值
                if (type != 1)
                {
                    string sql = "select * from dbo.View_AllDataApplicationList where ApplicationRef='" + ApplicationRef + "' and ApplicationRef is not null";
                    View_AllDataApplicationList list = db.Database.SqlQuery<View_AllDataApplicationList>(sql).FirstOrDefault();
                    List<View_AllDataApplicationList> Sonlist = db.Database.SqlQuery<View_AllDataApplicationList>(sql).ToList();
                    //productModel
                    //将list.ProductModel赋值到前台
                    ViewBag.Models = list.ProductModel;
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
                        //Factories
                        string factoriesSql = @"select top 1 STUFF((SELECT ';' + Name_cn FROM TBWords t2 where WordCode='FC' and Value in({0}) FOR XML PATH('')), 1, 1, '') AS Value  from TBWords cm ";
                        if (item.Factories != null)
                        {
                            item.Factories = "'" + item.Factories + "'";
                            string factoriesList = "";
                            if (item.Factories.IndexOf("|") != -1) //代表一个以上
                            {
                                string factories = item.Factories.Replace('|', ',');
                                var factoriesArr = factories.Split(',');
                                for (int i = 0; i < factoriesArr.Length; i++)
                                {

                                    if (i == 0 && factoriesArr[i] != "''")
                                    {
                                        factoriesList += factoriesArr[i] + "',";
                                    }
                                    else if (i == factoriesArr.Length - 1 && factoriesArr[i] != "''")
                                    {
                                        factoriesList += "'" + factoriesArr[i];
                                    }
                                    else if (factoriesArr[i] == "''")
                                    {
                                        factoriesList += "'" + factoriesArr[i] + "'";
                                    }
                                    else
                                    {
                                        factoriesList += "'" + factoriesArr[i] + "',";
                                    }

                                }
                            }
                            else
                            {
                                factoriesList = item.Factories;
                            }
                            factoriesSql = string.Format(factoriesSql, factoriesList);
                            item.Factories = db.Database.SqlQuery<string>(factoriesSql).FirstOrDefault();
                        }
                        else
                        {
                            item.Factories = "";
                        }
                        //country
                        string countrySql = @"select top 1 STUFF((SELECT ';' +'('+Icode+')'+ cntyName_s FROM Country t2 where Icode in({0})  FOR XML PATH('')), 1, 1, '') AS country from Country cm ";
                        if (item.CoverAreas != null)
                        {

                            item.CoverAreas = "'" + item.CoverAreas + "'";
                            string countryList = "";
                            if (item.CoverAreas.IndexOf("|") != -1) //代表一个以上
                            {
                                string country = item.CoverAreas.Replace('|', ',');
                                var arr = country.Split(',');
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
                            }
                            else
                            {
                                countryList = item.CoverAreas;
                            }
                            countrySql = string.Format(countrySql, countryList);
                            item.CoverAreas = db.Database.SqlQuery<string>(countrySql).FirstOrDefault();
                        }
                        else
                        {
                            item.CoverAreas = "";
                        }
                    }
                    ViewBag.SonData = Sonlist;
                }
                else
                {
                    ViewBag.Data = new View_AllDataApplicationList();
                    ViewBag.SonData = new List<View_AllDataApplicationList>();
                }
                ViewBag.type = type;
                return View();
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                return Content("<script languge='javascript'>alert('发生错误，请联系电脑部！内部成员请查看日志文件');window.history.back();</script>");
                throw;
            }
        }
        #endregion

        #region 子表中工厂多选框|国家多选框|型号多选框|国家及区域单选框
        /// <summary>
        /// Select-Factories界面   子表中工厂多选框
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectFactories(string Factories)
        {
            ViewBag.Factories = Factories;
            return View();
        }
        /// <summary>
        /// Select-Factories界面   子表中国家多选框
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectCountriesAreas(string CoverAreas)
        {
            ViewBag.CoverAreas = CoverAreas;
            return View();
        }
        /// <summary>
        /// Select-Factories界面   子表中型号多选框
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectModels(string Models)
        {
            ViewBag.Models = Models;
            return View();
        }
        /// CertificatesManagement界面   国家及区域单选框
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectCertificatesCountriesAreas()
        {
            return View();
        }
        #endregion

        #region 子页面  认证证书管理
        /// <summary>
        /// Certificate_Update界面  认证管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Certificate_Update()
        {
            if (new Authority().GetQuanSql(Session["userid"].ToString(), "CertMaster_A") > 0)
            {
                ViewBag.Add = 1;  //新增维护安规认证主表
            }
            return View();
        }

        /// <summary>
        /// 打印Certificate_Update界面  认证管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Certificate_UpdatePrinting()
        {
            //将登录人三字代码传给打印人
            if (Session["userid"] != null)
            {
                ViewBag.username = new Authority().GetUserSql(Session["userid"].ToString());
            }
            return View();
        }

        /// <summary>
        /// CertificatesManagement界面   国家及区域
        /// </summary>
        /// <returns></returns>
        public ActionResult AddCertificates(NewCertificates_Master certificates)
        {
            if (certificates != null)
            {
                ViewBag.certificates = certificates;
            }
            else
            {
                ViewBag.certificates = new NewCertificates_Master();
            }

            return View();
        }
        /// <summary>
        /// Certificate Master Details 界面  作废和未作废的
        /// </summary>
        /// <returns></returns>
        public ActionResult CertificateMasterDetails(string CMSerial)
        {
            try
            {
                if (CMSerial != "")
                {
                    string sql = "select *,'('+c.Icode+')'+c.CntyName_s as CountryArea from  Certificates_Master cm ,Country c where c.icode=cm.Mkt_Cnty and CMSerial=" + CMSerial;
                    ViewBag.CertificatesManagement = db.Database.SqlQuery<NewCertificates_Master>(sql).FirstOrDefault();
                }
                else
                {
                    ViewBag.CertificatesManagement = new NewCertificates_Master();
                }
                return View();
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                return Content("<script languge='javascript'>alert('发生错误，请联系电脑部！内部成员请查看日志文件');window.history.back();</script>");
                throw;
            }

        }
        /// <summary>
        /// 点击复制图标进入的编辑认证证书的界面
        /// </summary>
        /// <returns></returns>
        public ActionResult CertificateEdit(string CertCode, string IsVoid)
        {
            try
            {
                if (CertCode != "")
                {
                    int index = 0;//是否作废  0 未作废  1 作废
                    if (IsVoid == "true")
                    {
                        index = 1;
                    }
                    string sql = "select *,'('+c.Icode+')'+c.CntyName_s as CountryArea from  Certificates_Master cm ,Country c where c.icode=cm.Mkt_Cnty and CertCode='" + CertCode + "' and cm.Isvoid = " + index;
                    ViewBag.CertificateEdit = db.Database.SqlQuery<NewCertificates_Master>(sql).Where(s => s.CertCode == CertCode).FirstOrDefault();
                }
                else
                {
                    ViewBag.CertificateEdit = new NewCertificates_Master();
                }
                return View();
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                return Content("<script languge='javascript'>alert('发生错误，请联系电脑部！内部成员请查看日志文件');window.history.back();</script>");
                throw;
            }

        }
        #endregion

        #region  子界面  产品模型管理
        /// <summary>
        /// Product-Model-Update界面   产品模型管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Product_Model_Update()
        {
            if (new Authority().GetQuanSql(Session["userid"].ToString(), "Model_A") > 0)
            {
                ViewBag.Add = 1;  //新增元器件型号信息
            }
            return View();
        }
        /// <summary>
        /// 打印Product-Model-Update界面   产品模型管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Product_Model_UpdatePrinting()
        {
            //将登录人三字代码传给打印人
            if (Session["userid"] != null)
            {
                ViewBag.username = new Authority().GetUserSql(Session["userid"].ToString());
            }
            return View();
        }
        /// <summary>
        /// AddNewProductModel 产品模型新增|复制|修改  三合一界面
        /// </summary>
        /// <param name="CPSerial">产品模型的唯一标识</param>
        /// <returns></returns>
        public ActionResult AddCopyEditProduct_Model(Component_Model component, string type)
        {
            try
            {
                ViewBag.type = type;
                if (component.CPSerial == 0)
                {
                    ViewBag.ComponentModel = component;
                }
                else
                {
                    string sql = "select * from component_Model where 1=1 and CPSerial=" + component.CPSerial;
                    ViewBag.ComponentModel = db.Database.SqlQuery<Component_Model>(sql).FirstOrDefault();
                }
                return View();
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                return Content("<script languge='javascript'>alert('发生错误，请联系电脑部！内部成员请查看日志文件');window.history.back();</script>");
                throw;
            }

        }
        /// <summary>
        /// Component Model Details  产品模型详情界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Component_Model_Details(int CPSerial)
        {
            try
            {
                if (CPSerial == 0)
                {
                    ViewBag.type = "4";
                    ViewBag.ComponentModel = new Component_Model();
                }
                else
                {
                    string sql = "select * from component_Model where 1=1 and CPSerial=" + CPSerial;
                    ViewBag.ComponentModel = db.Database.SqlQuery<Component_Model>(sql).FirstOrDefault();
                }
                return View();
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                return Content("<script languge='javascript'>alert('发生错误，请联系电脑部！内部成员请查看日志文件');window.history.back();</script>");
                throw;
            }

        }
        #endregion
    }
}