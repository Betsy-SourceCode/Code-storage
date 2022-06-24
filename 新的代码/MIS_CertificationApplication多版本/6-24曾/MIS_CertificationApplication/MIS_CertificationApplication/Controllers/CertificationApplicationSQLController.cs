using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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

        #region 数据加载
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
                    //ProductModel
                    string modelSql = @"select top 1 STUFF((SELECT ';' + ModelCode FROM component_Model t2 where CPSerial in ({0}) FOR XML PATH('')), 1, 1, '') AS model
                                    from component_Model cm ";
                    string model = item.ProductModel.Replace('|', ',');
                    modelSql = string.Format(modelSql, model);
                    item.ProductModel = db.Database.SqlQuery<string>(modelSql).FirstOrDefault();
                    //ApplyCountry
                    string countrySql = @"select top 1 STUFF((SELECT ';' +'('+Icode+')'+ cntyName_s FROM Country t2 where Icode in({0})  FOR XML PATH('')), 1, 1, '') AS country from Country cm ";
                    if (item.ApplyCountry != null)
                    {
                        item.ApplyCountry = "'" + item.ApplyCountry + "'";
                        string countryList = "";
                        if (item.ApplyCountry.IndexOf("|") != -1) //代表一个以上
                        {
                            string country = item.ApplyCountry.Replace('|', ',');
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
                            countryList = item.ApplyCountry;
                        }
                        countrySql = string.Format(countrySql, countryList);
                        item.ApplyCountry = db.Database.SqlQuery<string>(countrySql).FirstOrDefault();
                    }
                    else
                    {
                        item.ApplyCountry = "";
                    }
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
        #endregion

        #region 主表下拉框
        /// <summary>
        /// 获得主表型号编码列表
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
        #endregion

        #region 子表下拉框
        /// <summary>
        /// 获得子表认证名称默认值列表
        /// </summary>
        /// <param name="CA_Ref">子表的认证申请单编号</param>
        /// <returns></returns>
        public string GetCertificateNameListDefault(string CA_Ref)
        {
            try
            {
                string sql = @"select cer.CM_Serial as CMSerial from dbo.Certificates_Master master,
                dbo.Certificates cer where master.CMSerial = cer.CM_Serial and CA_Ref ='" + CA_Ref + "'";
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
        /// 获取子表认证名称的下拉框值
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
        /// 获取认证编号是否存在Certificates_Master表
        /// </summary>
        /// <param name="CertCode">认证编号</param>
        /// <returns></returns>
        public string GetCertCodeISExsits(string CertCode)
        {
            try
            {
                string sql = @"select CMSerial,certcode+' - '+certname as Text from dbo.Certificates_Master where CertCode ='" + CertCode.TrimStart() + "' and IsVoid<>1 order by CMSerial ";
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
        /// 获取新增界面的多选国家区域列表
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
        /// 获取认证管理界面的多选国家区域列表
        /// </summary>
        /// <returns></returns>
        public string GetCertificatesCountryAreaList()
        {
            try
            {
                string sql = "SELECT Icode as Name, Continent + ' - ' + CntyName_s + '（' + Icode + '）' AS Text FROM Country WHERE  (OrderBy >= 50) ORDER BY Continent, CntyName_s";
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
        /// 获取子表中productModel字段的列表
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
                    db.Configuration.ValidateOnSaveEnabled = false;
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
                        result = db.SaveChanges();
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

        /// <summary>
        /// 复制/修改认证记录
        /// </summary>
        /// <param name="type">2-复制，3-修改</param>
        /// <param name="Main">主表</param>
        /// <param name="Son">子表</param>
        /// <param name="Mainflag">主表标志位</param>
        /// <returns></returns>
        public string CopyOrUpdateData(int type, Cert_Apply_Case Main, string Son, bool Mainflag)
        {
            try
            {
                int result = 0; //成功/失败
                using (DbContextTransaction trans = this.db.Database.BeginTransaction()) //事务，获取自增ID
                {
                    //旧主表数据
                    var OldMain = db.Cert_Apply_Case.Where(a => a.CA_Ref == Main.CA_Ref).AsNoTracking().FirstOrDefault();
                    //新子表序列化
                    List<Certificates> JsonsList = JsonConvert.DeserializeObject<List<Certificates>>(Son);
                    #region 图片做特殊操作
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        Stream inputStream = Request.Files[i].InputStream;
                        byte[] PicBt = new byte[inputStream.Length];
                        inputStream.Read(PicBt, 0, PicBt.Length);
                        if (Request.Files.AllKeys[i] == "Mainfile" && OldMain.QuoteFile == null)  //主表
                        {
                            Main.QuoteFile = PicBt;
                            Main.QuoteFileName = Request.Files[i].FileName;
                        }
                        else
                        {
                            for (int j = 0; j < JsonsList.Count; j++)
                            {
                                if (JsonsList[j].CertFileName == Request.Files.AllKeys[i] && JsonsList[j].CertFile == null && JsonsList[j].Sonflag)
                                {
                                    JsonsList[j].CertFile = PicBt;
                                    JsonsList[j].CertFileName = Request.Files[i].FileName;
                                }
                            }
                        }
                        //主表原本就有文件并且修改时没有上传新的文件
                        if (OldMain.QuoteFile != null && Main.QuoteFile == null && Mainflag)
                        {
                            Main.QuoteFile = OldMain.QuoteFile;
                            Main.QuoteFileName = OldMain.QuoteFileName;
                        }
                    }
                    #endregion
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
                    Main.CreateBy = OldMain.CreateBy;
                    Main.CreateDept = OldMain.CreateDept;
                    Main.CreateTime = OldMain.CreateTime;
                    Main.UpdateBy = new Authority().GetUserSql(Session["userid"].ToString());
                    Main.UpdateDept = new Authority().GetDepartmentSql(Main.UpdateBy, 0);
                    Main.UpdateTime = DateTime.Now;
                    if (type == 2) //复制（新增）
                    {
                        Cert_Apply_Case LatestData = db.Cert_Apply_Case.OrderByDescending(a => a.CA_Ref).FirstOrDefault();
                        var NewCA_Ref = LatestData.CA_Ref.Substring(LatestData.CA_Ref.Length - 3, 3);
                        string num = (int.Parse(NewCA_Ref) + 1).ToString();
                        string xuhao = num.Length == 1 ? "00" + num : num.Length == 2 ? "0" + num : num.ToString();
                        Main.CA_Ref = "CA" + DateTime.Now.ToString("yyMMdd") + xuhao;
                        db.Cert_Apply_Case.Add(Main);
                    }
                    else
                    {   //修改
                        DbEntityEntry<Cert_Apply_Case> entry = db.Entry(Main);
                        entry.State = EntityState.Modified;
                    }
                    result = this.db.SaveChanges();
                    if (result < 0)
                    {
                        trans.Rollback(); //回滚
                        result = 0;
                    }
                    else if (Son != "[]" && result != 0) //主表添加成功才能去添加子表
                    {
                        int SonResult = 0;
                        //子表方法
                        if (type == 2) //复制
                        {
                            //复制需要传旧的认证编号
                            SonResult = CopyOrUpdateSonData(type, JsonsList, Main, OldMain.CA_Ref);
                        }
                        else
                        {
                            SonResult = CopyOrUpdateSonData(type, JsonsList, Main, null);
                        }
                        if (SonResult <= 0)
                        {
                            trans.Rollback(); //回滚
                            result = 0;
                        }
                        else
                        {
                            result = SonResult;
                        }

                    }
                    if (result != 0)
                    {
                        trans.Commit();
                    }
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

        /// <summary>
        /// 复制/修改子表数据
        /// </summary>
        /// <param name="type">2-复制，3-修改</param>
        /// <param name="JsonsList">子表</param>
        /// <param name="Main">主表</param>
        /// <returns></returns>
        public int CopyOrUpdateSonData(int type, List<Certificates> JsonsList, Cert_Apply_Case Main, string MainCA_Ref)
        {
            try
            {
                //子表
                var index = 0;
                List<Certificates> OldSon = null;
                if (type == 2)
                {
                    //旧子表数据,复制需要查旧的认证编号
                    OldSon = db.Certificates.Where(a => a.CA_Ref == MainCA_Ref).AsNoTracking().ToList();
                }
                else
                {
                    //旧子表数据
                    OldSon = db.Certificates.Where(a => a.CA_Ref == Main.CA_Ref).AsNoTracking().ToList();
                }

                foreach (var item in JsonsList)
                {
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
                    if (index < OldSon.Count)
                    {
                        //修改
                        item.CreateBy = OldSon[index].CreateBy;
                        item.CreateDept = OldSon[index].CreateDept;
                        item.CreateTime = OldSon[index].CreateTime;
                    }
                    else
                    {
                        //新增
                        item.CreateBy = new Authority().GetUserSql(Session["userid"].ToString());
                        item.CreateDept = new Authority().GetDepartmentSql(item.CreateBy, 0);
                        item.CreateTime = DateTime.Now;
                    }
                    item.UpdateBy = new Authority().GetUserSql(Session["userid"].ToString());
                    item.UpdateDept = new Authority().GetDepartmentSql(item.CreateBy, 0);
                    item.UpdateTime = DateTime.Now;
                    if (index < OldSon.Count)
                    {
                        item.CA_Ref = OldSon[index].CA_Ref;
                        if (OldSon[index].CertFile != null && item.CertFile == null && item.Sonflag) //新旧文件替换
                        {
                            item.CertFile = OldSon[index].CertFile;
                            item.CertFileName = OldSon[index].CertFileName;
                        }
                        if (type == 2) //复制
                        {
                            //执行新增
                            item.CA_Ref = Main.CA_Ref;
                            db.Certificates.Add(item);
                        }
                        else
                        {
                            //执行修改
                            item.CFSerial = OldSon[index].CFSerial;
                            DbEntityEntry<Certificates> Sonentry = db.Entry(item);
                            Sonentry.State = EntityState.Modified;
                        }

                    }
                    else
                    {
                        //执行新增
                        item.CA_Ref = Main.CA_Ref;
                        db.Certificates.Add(item);
                    }
                    index++;
                }
                return db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// 删除主表认证记录（没有认证子表记录）
        /// </summary>
        /// <param name="ContactID">主键</param>
        /// <param name="CustomerId">客户ID</param>
        /// <returns></returns>
        public int DelData(string CA_Ref)
        {
            int i = 0;
            try
            {
                //查询需要删除的数据
                Cert_Apply_Case Main = db.Cert_Apply_Case.Where(a => a.CA_Ref == CA_Ref).AsNoTracking().FirstOrDefault();
                this.db.Cert_Apply_Case.Attach(Main);
                this.db.Cert_Apply_Case.Remove(Main);
                i = this.db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());//日志
            }
            return i;
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

        #region 认证管理界面的方法合集
        /// <summary>
        /// 查询认证管理数据
        /// </summary>
        /// <param name="CountryAreas"> 国家区域  对应字段  Mkt_Cnty</param>
        /// <param name="key_words">关键字</param>
        /// <returns></returns>
        public string GetCertificatesManagementList(string CountryAreas, string key_words, string Cancel)
        {
            try
            {
                string sql = "select *,'('+c.Icode+')'+c.CntyName_s as CountryArea from  Certificates_Master cm ,Country c where c.icode=cm.Mkt_Cnty";
                if (CountryAreas != "" && CountryAreas != null)
                {
                    sql += " and c.Icode = '" + CountryAreas + "'";
                }
                if (key_words != null && key_words != "")
                {
                    sql += " and cm.CertCode+cm.CertName like '%" + key_words + "%'";
                }
                if (Cancel != "true")
                {
                    sql += " and IsVoid=0";
                }

                List<NewCertificates_Master> DataList = db.Database.SqlQuery<NewCertificates_Master>(sql).ToList();
                return JsonConvert.SerializeObject(DataList);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }

        }
        /// <summary>
        /// 新增认证管理的数据
        /// </summary>
        /// <returns></returns>
        public int AddCertificatesManagementList(Certificates_Master Certificates_Master)
        {
            try
            {
                Certificates_Master.Remark = Certificates_Master.Remark.Trim();
                Certificates_Master.CreateBy = new Authority().GetUserSql(Session["userid"].ToString());
                Certificates_Master.CreateDept = new Authority().GetDepartmentSql(Certificates_Master.CreateBy, 0);
                Certificates_Master.CreateTime = DateTime.Now;
                Certificates_Master.UpdateBy = Certificates_Master.CreateBy;
                Certificates_Master.UpdateDept = Certificates_Master.CreateDept;
                Certificates_Master.UpdateTime = DateTime.Now;

                db.Certificates_Master.Add(Certificates_Master);
                int i = db.SaveChanges();
                if (i > 0)
                {
                    return Certificates_Master.CMSerial;
                }
                else
                {
                    return 0;//新增失败
                }

            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 作废数据的方法
        /// </summary>
        /// <param name="CMSerail">数据的唯一标识 </param>
        /// <returns></returns>
        public int UpdateCertificatesISVoid(string CMSerial)
        {
            try
            {
                string sql = "select * from Certificates_Master where CMSerial='" + CMSerial + "'";
                Certificates_Master DataList = db.Database.SqlQuery<Certificates_Master>(sql).FirstOrDefault();
                DataList.IsVoid = true;
                DataList.Remark = DataList.Remark.Trim();
                DataList.CreateBy = new Authority().GetUserSql(Session["userid"].ToString());
                DataList.CreateDept = new Authority().GetDepartmentSql(DataList.CreateBy, 0);
                DataList.CreateTime = DateTime.Now;
                DataList.UpdateBy = DataList.CreateBy;
                DataList.UpdateDept = DataList.CreateDept;
                DataList.UpdateTime = DateTime.Now;
                DbEntityEntry<Certificates_Master> entry = this.db.Entry<Certificates_Master>(DataList);
                entry.State = EntityState.Modified;
                return db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 修改认证证书事件  得  save
        /// </summary>
        /// <param name="certificates">修改的那一条数据</param>
        /// <returns></returns>
        public int CertificateEdit(Certificates_Master certificates)
        {
            try
            {
                string sql = "select * from Certificates_Master where CertCode='" + certificates.CertCode.Trim() + "'";
                Certificates_Master DataList = db.Database.SqlQuery<Certificates_Master>(sql).First();
                DataList.IsVoid = DataList.IsVoid;
                DataList.Remark = certificates.Remark.Trim();
                DataList.CertName = certificates.CertName;
                DataList.CountryArea = certificates.CountryArea;
                DataList.Mkt_Cnty = certificates.Mkt_Cnty;
                DataList.StdFee = certificates.StdFee;
                DataList.StdTime = certificates.StdTime;
                DataList.UpdateBy = DataList.CreateBy;
                DataList.UpdateDept = DataList.CreateDept;
                DataList.UpdateTime = DateTime.Now;
                DbEntityEntry<Certificates_Master> entry = this.db.Entry<Certificates_Master>(DataList);
                entry.State = EntityState.Modified;
                return db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }

        }
        #endregion

        #region  产品模型界面的方法合集
        /// <summary>
        /// 查询产品模型数据
        /// </summary>
        /// <param name="K3ItemsNum"> 模糊查询 K3-Related-Items</param>
        /// <param name="key_words">关键字  Model-Code + Model-Name + Specification</param>
        /// <returns></returns>
        public string GetComponentModelManagementList(string K3ItemsNum, string key_words)
        {
            try
            {
                string sql = "select * from component_Model where 1=1";
                if (K3ItemsNum != "" && K3ItemsNum != null)
                {
                    sql += " and k3parts like '%" + K3ItemsNum + "%'";

                }
                if (key_words != null && key_words != "")
                {
                    sql += " and modelcode+modelName+isnull(ModelSpec,'') like '%" + key_words + "%'";
                }
                List<Component_Model> DataList = db.Database.SqlQuery<Component_Model>(sql).ToList();
                return JsonConvert.SerializeObject(DataList);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }

        }
        /// <summary>
        /// 检查是否存在于GIPComponent 表
        /// </summary>
        /// <param name="Fnumber">k3代码</param>
        /// <returns></returns>
        public string GetK3PartsISExsits(string Fnumber)
        {
            try
            {
                string sql = @"select * from GIPComponent where FNumber='" + Fnumber + "'";
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
        /// 删除  产品模型数据
        /// </summary>
        /// <param name="CPSerial"></param>
        /// <returns></returns>
        public int ComponentModelISDel(string CPSerial)
        {
            int id = int.Parse(CPSerial);
            Component_Model DataList = db.Component_Model.Where(s => s.CPSerial == id).FirstOrDefault();
            db.Component_Model.Remove(DataList);
            int i = db.SaveChanges();
            return i;
        }
        /// <summary>
        /// 新增  产品模型数据
        /// </summary>
        /// <param name="Certificates_Master">产品模型对象</param>
        /// <returns></returns>
        public int AddComponentModelList(Component_Model component)
        {
            try
            {
                component.ModelSpec = component.ModelSpec.Trim();
                component.CreateBy = new Authority().GetUserSql(Session["userid"].ToString());
                component.CreateDept = new Authority().GetDepartmentSql(component.CreateBy, 0);
                component.CreateTime = DateTime.Now;
                component.UpdateBy = component.CreateBy;
                component.UpdateDept = component.CreateDept;
                component.UpdateTime = DateTime.Now;
                db.Component_Model.Add(component);
                return db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 比较物料号的大小
        /// </summary>
        /// <param name="fnumber1">物料号1</param>
        /// <param name="fnumber2">物料号2</param>
        /// <returns></returns>
        public string CompareK3Parts(string fnumber1, string fnumber2)
        {
            try
            {
                string sql = @"DECLARE @a VARCHAR(20)
                            DECLARE @b VARCHAR(20)
                            set @a='" + fnumber1 + "'" +
                            "set @b='" + fnumber2 + "'" +
                            "DECLARE @T TABLE (K3Parts " +
                            "VARCHAR(200))if (@a<>'' and @b<>'')" +
                            "INSERT INTO @T" +
                            "( K3Parts )" +
                            " SELECT (CASE WHEN @a=@b THEN @a ELSE(CASE WHEN @a<@B THEN " +
                            "@a+'-'+@b ELSE @b+'-'+@a end) END )" +
                            "if(@a=''or @b='')" +
                            "INSERT INTO @T" +
                            "( K3Parts )" +
                            "SELECT (CASE WHEN @b<>'' THEN @b ELSE (CASE WHEN @a<>'' THEN @a ELSE '' END) END )" +
                            "SELECT * FROM @T";
                Others data = db.Database.SqlQuery<Others>(sql).FirstOrDefault();
                return JsonConvert.SerializeObject(data.K3Parts);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 修改  模型产品
        /// </summary>
        /// <param name="CMSerial">模型产品的唯一标识</param>
        /// <returns></returns>
        public int UpdateProductModel(Component_Model component_Model)
        {
            try
            {
                string sql = "select * from component_Model where CPSerial='" + component_Model.CPSerial + "'";
                Component_Model DataList = db.Database.SqlQuery<Component_Model>(sql).FirstOrDefault();
                component_Model.UpdateBy = DataList.CreateBy;
                component_Model.UpdateDept = DataList.CreateDept;
                component_Model.UpdateTime = DateTime.Now;
                DbEntityEntry<Component_Model> entry = this.db.Entry<Component_Model>(component_Model);
                entry.State = EntityState.Modified;
                return db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }

        }
        #endregion
    }
}