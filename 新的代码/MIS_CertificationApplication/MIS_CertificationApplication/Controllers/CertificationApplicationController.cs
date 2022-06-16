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
        #region 页面
        // GET: CertificationApplication
        /// <summary>
        /// Application List界面 认 证 申 请 及 管 理
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string userid)
        {
            userid = "444";
            Session["userid"] = userid;
            return View();
        }
        /// <summary>
        /// Application Create界面  新增
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
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
                return View();
            }
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
                var aa = list.QuoteFileName.LastIndexOf(".") + 1;
                var bb = list.QuoteFileName.ToString().Length;
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
            return View();
        }
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
        /// CertificatesManagement界面   国家及区域单选框
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectCertificatesCountriesAreas()
        {
            return View();
        }
        /// <summary>
        /// CertificatesManagement界面   国家及区域
        /// </summary>
        /// <returns></returns>
        public ActionResult AddCertificates(NewCertificates_Master  certificates)
        {
            if (certificates!=null)
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
        /// Select-Factories界面   子表中型号多选框
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectModels()
        {
            return View();
        }
        /// <summary>
        /// Certificate Master Details 界面  作废和未作废的
        /// </summary>
        /// <returns></returns>
        public ActionResult CertificateMasterDetails(string CertCode)
        {
            if (CertCode != "")
            {
                string sql = "select *,'('+c.Icode+')'+c.CntyName_s as CountryArea from  Certificates_Master cm ,Country c where c.icode=cm.Mkt_Cnty and CertCode='"+CertCode+"'";
                ViewBag.CertificatesManagement = db.Database.SqlQuery<NewCertificates_Master>(sql).Where(s=>s.CertCode==CertCode).FirstOrDefault();
            }
            else
            {
                ViewBag.CertificatesManagement = new NewCertificates_Master();
            }
            return View();
        }
        /// <summary>
        /// 点击复制图标进入的编辑认证证书的界面
        /// </summary>
        /// <returns></returns>
         public ActionResult CertificateEdit(string CertCode ,string IsVoid)
        {
            if (CertCode != "")
            {
                int index = 0;//是否作废  0 未作废  1 作废
                if (IsVoid=="true")
                {
                    index = 1;
                }
                string sql = "select *,'('+c.Icode+')'+c.CntyName_s as CountryArea from  Certificates_Master cm ,Country c where c.icode=cm.Mkt_Cnty and CertCode='" + CertCode + "' and cm.Isvoid = "+index;
                ViewBag.CertificateEdit = db.Database.SqlQuery<NewCertificates_Master>(sql).Where(s => s.CertCode == CertCode).FirstOrDefault();
            }
            else
            {
                ViewBag.CertificateEdit = new NewCertificates_Master();
            }
            return View();
        }
        #endregion


    }
}