using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualBasic.ApplicationServices;
using MIS_CertificationApplication.Models;
using Newtonsoft;
using Newtonsoft.Json;
using RestSharp;

namespace MIS_CertificationApplication.Controllers
{
    public class CertificationApplicationSQLController : Controller
    {
        WebStationEntities db = new WebStationEntities();
        // GET: CertificationApplicationSQL
        /// <summary>
        /// 首页列表数据
        /// </summary>
        /// <param name="Application">申请编号/认证编号</param>
        /// <param name="ModelCode">型号编码</param>
        /// <param name="ExpireSelect">过去/未来</param>
        /// <param name="day">有效期天数</param>
        /// <param name="Customer">客户</param>
        /// <param name="KeyWords">供应商/认证名称</param>
        /// <param name="Status">状态代码</param>
        /// <returns></returns>
        public string CertificationApplicationList(string Application, string ModelCode, string ExpireSelect, string day, string Customer, string KeyWords, string Status)
        {
            try
            {
                DateTime dt = new DateTime();
                DateTime now = DateTime.Now;
                string sql = "select * from View_ApplicationList where 1=1 ";
                if (Application != null) //申请编号/认证编号模糊查询
                {
                    sql = sql + " AND (ApplicationRef like '%" + Application + "%' or CertificateRef like '%" + Application + "%')";
                }
                if (ModelCode != "") //型号编码下拉框
                {
                    sql = sql + " AND ProductModel ='" + ModelCode + "'";
                }
                if (day != "") //有效期日期
                {
                    int days = int.Parse(day);
                    if (ExpireSelect == "past") //过去
                    {
                        dt = now.AddDays(-days);  //n天前
                    }
                    else
                    {
                        //未来(coming)
                        dt = now.AddDays(days);  //n天后
                    }
                    sql = sql + " AND (ExpiryDate>='" + DateTime.Now + "' and ExpiryDate<='" + DateTime.Now + "')";
                }
                if (Customer != "") //客户下拉框
                {
                    sql = sql + " AND Customer='" + Customer + "'";
                }
                if (KeyWords != "") //供应商/认证名称模糊查询
                {
                    sql = sql + " AND (Supplier like '%" + KeyWords + "%' or CertificateName like '%" + KeyWords + "%')";
                }
                if (Status != "") //状态下拉框
                {
                    sql = sql + " AND Status='" + Status + "'";
                }
                sql += " ORDER BY ApplicationRef DESC,CertificateRef desc,ApplyCountry desc";
                List<View_ApplicationList> certifications = db.Database.SqlQuery<View_ApplicationList>(sql).ToList();
                foreach (var item in certifications)
                {
                    string modelSql = @"select top 1 STUFF((SELECT ';' + ModelCode FROM component_Model t2 where CPSerial in ({0}) FOR XML PATH('')), 1, 1, '') AS model
                                    from component_Model cm ";
                    string countrySql = @"select top 1 STUFF((SELECT ';' +'('+Icode+')'+ cntyName_s FROM Country t2 where Icode in({0})  FOR XML PATH('')), 1, 1, '') AS country from Country cm ";

                    item.ApplyCountry = "'" + item.ApplyCountry + "'";
                    string country = item.ApplyCountry.Replace('|', ',');
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
                    //countryList = countryList.Remove(countryList.LastIndexOf(','));
                    string model = item.ProductModel.Replace('|', ',');
                    modelSql = string.Format(modelSql, model);
                    countrySql = string.Format(countrySql, countryList);

                    item.ProductModel = db.Database.SqlQuery<string>(modelSql).FirstOrDefault();
                    item.ApplyCountry = db.Database.SqlQuery<string>(countrySql).FirstOrDefault();
                }
                //打印页面专用
                Session["certifications"] = certifications;
                return JsonConvert.SerializeObject(certifications);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 打印界面专用加载数据
        /// </summary>
        /// <returns></returns>
        public string PrintCertificationApplicationList()
        {
            var certifications = Session["certifications"];
            return JsonConvert.SerializeObject(certifications);
        }
        #region 下拉框
        /// <summary>
        /// 获得型号编码列表
        /// </summary>
        /// <returns></returns>
        public string GetModelList()
        {
            try
            {
                string sql = "SELECT CPSerial as Value,ModelCode as Text from dbo.Component_Model";
                List<Others> DataList = db.Database.SqlQuery<Others>(sql).ToList();
                return JsonConvert.SerializeObject(DataList);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 获得客户列表
        /// </summary>
        /// <returns></returns>
        public string GetCustomerList()
        {
            try
            {
                string sql = "SELECT DISTINCT CustGrp FROM Customer ORDER BY CustGrp";
                List<string> DataList = db.Database.SqlQuery<string>(sql).ToList();
                return JsonConvert.SerializeObject(DataList);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 获得委任申请公司下拉框
        /// </summary>
        /// <returns></returns>
        public string GetApplicantList()
        {
            try
            {
                string sql = "select Value as Name,Name_CN as Text from TBWords where WordCode='CO'";
                List<Others> DataList = db.Database.SqlQuery<Others>(sql).ToList();
                return JsonConvert.SerializeObject(DataList);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 获得制造商下拉框
        /// </summary>
        /// <returns></returns>
        public string GetManufacturerList()
        {
            try
            {
                string sql = "select Value as Name,Name_CN as Text from TBWords where WordCode='FC'";
                List<Others> DataList = db.Database.SqlQuery<Others>(sql).ToList();
                return JsonConvert.SerializeObject(DataList);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 获得费用货币列表
        /// </summary>
        /// <returns></returns>
        public string GetCurrencyList()
        {
            try
            {
                string sql = "select  Icode as Name,Icode+'-'+CurrencyName_CN as Text from  Currency";
                List<Others> DataList = db.Database.SqlQuery<Others>(sql).ToList();
                return JsonConvert.SerializeObject(DataList);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 获取子表认证编号的下拉框值
        /// </summary>
        /// <returns></returns>
        public string GetCertificateNameList()
        {
            try
            {
                string sql = @"select CMSerial,certcode+' - '+certname as Text from dbo.Certificates_Master order by CMSerial";
                List<Others> DataList = db.Database.SqlQuery<Others>(sql).ToList();
                return JsonConvert.SerializeObject(DataList);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 获取 多 选 国 家 区 域 列表
        /// </summary>
        /// <returns></returns>
        public string GetCountryAreaList()
        {
            try
            {
                string sql = "Select Top 60 Icode as Name,'(' + Icode + ') ' + cntyName_s as Text from Country order by OrderBy DESC";
                List<Others> DataList = db.Database.SqlQuery<Others>(sql).ToList();
                return JsonConvert.SerializeObject(DataList);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 获取  子表中  productModel字段的列表
        /// </summary>
        /// <returns></returns>
        public string GetProductsModels()
        {
            try
            {
                string sql = "select CPSerial,ModelCode as Text from Component_Model";
                List<Others> DataList = db.Database.SqlQuery<Others>(sql).ToList();
                return JsonConvert.SerializeObject(DataList);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }
        }
        public View_AllDataApplicationList EditPage(string CA_Ref)
        {
            //View_AllDataApplicationList list = db.View_AllDataApplicationList.Where(a => a.CA_Ref == CA_Ref).FirstOrDefault();
            //ViewBag.Data = list;
            //return list;
            return null;
        }
        #endregion

        #region 增删改
        /// <summary>
        /// 创建认证记录
        /// </summary>
        /// <param name="Main">主表</param>
        /// <param name="Son">子表Json字符串</param>
        /// <returns></returns>
        public string InsertData(Cert_Apply_Case Main, string Son)
        {
            try
            {
                int result = 0; //成功/失败
                using (DbContextTransaction trans = this.db.Database.BeginTransaction()) //事务，获取自增ID
                {
                    this.db.Configuration.ValidateOnSaveEnabled = false;
                    Cert_Apply_Case LatestData = db.Cert_Apply_Case.OrderByDescending(a => a.CA_Ref).FirstOrDefault();
                    var NewCA_Ref = LatestData.CA_Ref.Substring(LatestData.CA_Ref.Length - 3, 3);
                    string num = (int.Parse(NewCA_Ref) + 1).ToString();
                    string xuhao = num.Length == 1 ? "00" + num : num.Length == 2 ? "0" + num : num.ToString();
                    List<Certificates> JsonsList = JsonConvert.DeserializeObject<List<Certificates>>(Son);
                    #region 图片做特殊操作
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        Stream inputStream = Request.Files[i].InputStream;
                        byte[] PicBt = new byte[inputStream.Length];
                        inputStream.Read(PicBt, 0, PicBt.Length);
                        if (Request.Files.AllKeys[i] == "Mainfile")  //主表
                        {
                            Main.QuoteFile = PicBt;
                            Main.QuoteFileName = Request.Files[i].FileName;
                        }
                        else
                        {
                            for (int j = 0; j < JsonsList.Count; j++)
                            {
                                if (JsonsList[j].CertFileName == Request.Files.AllKeys[i])
                                {
                                    JsonsList[j].CertFile = PicBt;
                                    JsonsList[j].CertFileName = Request.Files[i].FileName;
                                }
                            }
                            //var aa = JsonConvert.DeserializeObject<List<Certificates>>(Son);
                            //Son[i].CertFile = PicBt;
                        }
                    }
                    #endregion
                    //添加到认证申请主表
                    //设置默认值
                    if (Main.Applicant == null)
                    {
                        Main.Applicant = "HK";
                    }
                    if (Main.Manufacturer == null)
                    {
                        Main.Manufacturer = "HL";
                    }
                    if (Main.Currency == null)
                    {
                        Main.Currency = "USD";
                    }
                    if (Main.QuoteFile == null)
                    {
                        Main.QuoteFileName = null;
                    }
                    Main.CA_Ref = "CA" + DateTime.Now.ToString("yyMMdd") + xuhao;
                    Main.CreateBy = new Authority().GetUserSql(Session["userid"].ToString());
                    Main.CreateDept = new Authority().GetDepartmentSql(Main.CreateBy, 0);
                    Main.CreateTime = DateTime.Now;
                    Main.UpdateBy = Main.CreateBy;
                    Main.UpdateDept = Main.CreateDept;
                    Main.UpdateTime = DateTime.Now;
                    db.Cert_Apply_Case.Add(Main);
                    result = db.SaveChanges();
                    if (result < 0)
                    {
                        trans.Rollback(); //回滚
                        result = 0;
                    }
                    //添加到认证申请子表
                    //设置默认值
                    if (Son != "[]" && result != 0) //主表添加成功才能去添加子表
                    {
                        //批量添加
                        foreach (var item in JsonsList)
                        {
                            //设置默认值
                            item.CA_Ref = Main.CA_Ref;  //主表的认证申请编号
                            if (item.CertFile == null)
                            {
                                item.CertFileName = null;
                            }
                            DateTime Expiry = Convert.ToDateTime(item.Expiry);
                            DateTime now = DateTime.Now;
                            //在办-新增了记录，但没有认证编号的
                            if (item.Cert_Ref == "")
                            {
                                item.Status = "P";
                            }
                            //有效-有认证编号，失效期为空，或未过(起始小于结束)
                            var aa = Expiry.CompareTo(now);
                            if (item.Cert_Ref != "" && (item.Expiry == null || Expiry.CompareTo(now) < 0))
                            {
                                item.Status = "A";
                            }
                            //失效-已过有效期
                            if (Expiry.CompareTo(now) > 0)
                            {
                                item.Status = "E";
                            }
                            item.CreateBy = new Authority().GetUserSql(Session["userid"].ToString());
                            item.CreateDept = new Authority().GetDepartmentSql(item.CreateBy, 0);
                            item.CreateTime = DateTime.Now;
                            item.UpdateBy = item.CreateBy;
                            item.UpdateDept = item.CreateDept;
                            item.UpdateTime = DateTime.Now;
                            db.Certificates.Add(item);
                        }
                        result = this.db.SaveChanges();
                        if (result < 0)
                        {
                            trans.Rollback(); //回滚
                            result = 0;
                        }
                    }
                    trans.Commit();
                    Others ReturnResult = new Others
                    {
                        ApplicationRef = Main.CA_Ref,
                        ReturnResultNum = result
                    };
                    return JsonConvert.SerializeObject(ReturnResult);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                return null;
            }
        }
        #endregion

        #region 文件上传
        /// <summary>
        /// 查询文件后缀名是否符合上传档案格式标准
        /// </summary>
        /// <param name="FileSuffix"></param>
        /// <returns></returns>
        public int IsUploadStandard(string FileSuffix)
        {
            return this.db.Database.SqlQuery<int>("SELECT COUNT(1) FROM TBWords where  WordCode='FT' and Remark in('Offd','Pics') and Name_EN='" + FileSuffix + "'").FirstOrDefault();
        }
        #endregion
    }
}