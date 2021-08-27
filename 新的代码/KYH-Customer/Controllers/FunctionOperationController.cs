using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using System.Web.Mvc;
using KYH_Customer.Filters;
using KYH_Customer.Models;
using KYH_Customer.Models.PublicSqlMethods;
using KYH_Customer.Models.SqlMethods;
using Newtonsoft.Json;

namespace KYH_Customer.Controllers
{
    [Localization]
    [IsSessionExpire(IsCheck = true)]
    public class FunctionOperationController : Controller
    {
        WebStationEntities db = new WebStationEntities();
        public ActionResult Index()
        {
            return base.View();
        }
        /// <summary>
        /// 新增页面保存功能
        /// </summary>
        /// <param name="company">公司表</param>
        /// <param name="customer">客户表</param>
        /// <param name="custBillShip">客户详细表</param>
        /// <param name="contacts">联系表（Json字符串）</param>
        /// <returns></returns>
        public string customerAdds(Company company, Models.Customer customer, CustBillShip custBillShip, string contacts)
        {
            int i = 0;
            ResponseJson json;
            using (DbContextTransaction trans = this.db.Database.BeginTransaction()) //事务，获取自增ID
            {
                try
                {
                    this.db.Configuration.ValidateOnSaveEnabled = false;
                    //添加到Company表
                    //设置默认值
                    company.Agreement = false;
                    company.TableName = "Customer";
                    company.GoodWill = "N";
                    company.InBusiness = "N";
                    this.FanShe<Company>(company);
                    this.db.Company.Add(company);
                    i = this.db.SaveChanges();
                    if (i < 0)
                    {
                        trans.Rollback();
                        return i.ToString();
                    }
                    //添加到Customer表
                    //设置默认值
                    customer.CustType = "C";
                    customer.Status = "P";
                    customer.Class = "G";
                    customer.Type = "O";
                    customer.K3Code = "";
                    customer.CreditPeriod = new int?(0);
                    DateTime dt = DateTime.Now;
                    customer.SysID = new int?(company.CompanyID);
                    customer.CreateBy = base.Session["username"].ToString();
                    customer.CreateDept = base.Session["DeptID"].ToString();
                    customer.CreateTime = dt;
                    customer.UpdateBy = base.Session["username"].ToString();
                    customer.UpdateDept = base.Session["DeptID"].ToString();
                    customer.UpdateTime = dt;
                    this.FanShe<Models.Customer>(customer);//反射
                    this.db.Customer.Add(customer);
                    i = this.db.SaveChanges();
                    if (i < 0)
                    {
                        trans.Rollback();
                        return i.ToString();
                    }
                    //添加到CustBillShip表
                    custBillShip.CustomerID = customer.CustomerID;
                    custBillShip.CreateBy = customer.CreateBy;
                    custBillShip.CreateDept = customer.CreateDept;
                    custBillShip.CreateTime = dt;
                    custBillShip.UpdateBy = customer.UpdateBy;
                    custBillShip.UpdateDept = customer.UpdateDept;
                    custBillShip.UpdateTime = dt;
                    this.FanShe<CustBillShip>(custBillShip); //反射 
                    this.db.CustBillShip.Add(custBillShip);
                    i = this.db.SaveChanges();
                    if (i < 0)
                    {
                        trans.Rollback();
                        return i.ToString();
                    }
                    //添加到Contacts表
                    foreach (Contacts Con in JsonConvert.DeserializeObject<List<Contacts>>(contacts))
                    {
                        Con.SourceID = company.CompanyID;
                        Con.Source = "Company";
                        Con.Share = "N";
                        this.FanShe<Contacts>(Con);
                        Con.CreateBy = customer.CreateBy;
                        Con.CreateDept = customer.CreateDept;
                        Con.CreateTime = dt;
                        Con.UpdateBy = customer.UpdateBy;
                        Con.UpdateDept = customer.UpdateDept;
                        this.db.Contacts.Add(Con);
                        Con.UpdateTime = dt;
                        i = this.db.SaveChanges();
                        if (i < 0)
                        {
                            trans.Rollback();
                            return i.ToString();
                        }
                    }
                    trans.Commit();
                    //添加日志
                    //LogThread oth = new LogThread();
                    //ActionLog2020 logParam = new ActionLog2020();
                    //logParam.UserID = Session["username"].ToString();
                    //logParam.DeptID = SelectDeptID(logParam.UserID);  //获得所在部门ID
                    //logParam.FunctionID = 123;
                    //logParam.ActionType = "N";  //新增
                    //logParam.TableName = "Customer";
                    //logParam.TableKey = "CustomerID";
                    //logParam.Description = logParam.UserID + "新增客户" +customer.CustomerID.ToString().PadLeft(6,'0') + "（" + company.ShortName + ")";
                    //oth.log = logParam;
                    //Thread th = new Thread(new ThreadStart(oth.GetActionLogAdd)); //创建线程                     
                    //th.Start(); //启动线程
                    json = new ResponseJson
                    {
                        id = i,
                        Success = customer.CustomerID
                    };
                }
                catch (Exception ex)
                {
                    LogHelper.Write(ex.ToString()); //日志
                    return i.ToString();
                }
            }
            return JsonConvert.SerializeObject(json);
        }

        /// <summary>
        /// 修改页面保存功能-首页签
        /// </summary>
        /// <param name="company">公司表</param>
        /// <param name="customer">客户表</param>
        /// <returns></returns>
        public int UpdateIndex(Company company, Models.Customer customer)
        {
            int i = 0;
            DbConnection con = ((IObjectContextAdapter)this.db).ObjectContext.Connection;//事务
            con.Open();
            using (DbTransaction tran = con.BeginTransaction())
            {
                this.db.Configuration.ValidateOnSaveEnabled = false;
                try
                {
                    //修改company表
                    this.FanShe<Company>(company); //反射
                    DbEntityEntry<Company> entry = this.db.Entry<Company>(company);
                    entry.State = EntityState.Unchanged;
                    entry.Property<string>((Company a) => a.FullName_CN).IsModified = true;
                    entry.Property<string>((Company a) => a.FullName_EN).IsModified = true;
                    entry.Property<string>((Company a) => a.ShortName).IsModified = true;
                    entry.Property<string>((Company a) => a.CountryID).IsModified = true;
                    entry.Property<int?>((Company a) => a.ProvinceID).IsModified = true;
                    entry.Property<int?>((Company a) => a.CityID).IsModified = true;
                    entry.Property<string>((Company a) => a.Location).IsModified = true;
                    entry.Property<string>((Company a) => a.Classify).IsModified = true;
                    entry.Property<string>((Company a) => a.Manager).IsModified = true;
                    entry.Property<bool>((Company a) => a.Agreement).IsModified = true;
                    entry.Property<string>((Company a) => a.GoodWill).IsModified = true;
                    entry.Property<string>((Company a) => a.InBusiness).IsModified = true;
                    entry.Property<string>((Company a) => a.UrgNotice).IsModified = true;
                    entry.Property<string>((Company a) => a.Info_CN).IsModified = true;
                    entry.Property<string>((Company a) => a.Info_EN).IsModified = true;
                    entry.Property<string>((Company a) => a.Remarks).IsModified = true;
                    i = this.db.SaveChanges();
                    if (i < 0)
                    {
                        tran.Rollback();
                        return i;
                    }
                    //修改customer表
                    customer.Status = "P";
                    customer.UpdateBy = base.Session["username"].ToString();
                    customer.UpdateDept = base.Session["DeptID"].ToString();
                    customer.UpdateTime = DateTime.Now;
                    if (customer.K3Code == null)
                    {
                        customer.K3Code = "";
                    }
                    this.FanShe<Models.Customer>(customer);
                    DbEntityEntry<Models.Customer> entry2 = this.db.Entry<Models.Customer>(customer);
                    entry2.State = EntityState.Unchanged;
                    entry2.Property<string>((Models.Customer b) => b.Status).IsModified = true;
                    entry2.Property<string>((Models.Customer b) => b.K3Code).IsModified = true;
                    entry2.Property<string>((Models.Customer b) => b.CueCode).IsModified = true;
                    entry2.Property<string>((Models.Customer b) => b.Belongs).IsModified = true;
                    entry2.Property<string>((Models.Customer b) => b.Class).IsModified = true;//客户性质
                    entry2.Property<string>((Models.Customer b) => b.AnnualRevenue).IsModified = true; //年度收入
                    entry2.Property<string>((Models.Customer b) => b.CreditLimits).IsModified = true;//信用额度
                    entry2.Property<int?>((Models.Customer b) => b.CreditPeriod).IsModified = true;//信用期限
                    entry2.Property<string>((Models.Customer b) => b.Type).IsModified = true; //客户级别
                    entry2.Property<string>((Models.Customer b) => b.UpdateBy).IsModified = true;
                    entry2.Property<string>((Models.Customer b) => b.UpdateDept).IsModified = true;
                    entry2.Property<DateTime>((Models.Customer b) => b.UpdateTime).IsModified = true;
                    i = this.db.SaveChanges();
                    if (i < 0)
                    {
                        tran.Rollback();
                        return i;
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    LogHelper.Write(ex.ToString());//日志
                    return i;
                }
            }
            con.Close();
            return i;
        }

        /// <summary>
        /// 修改页面保存功能-第二页签
        /// </summary>
        /// <param name="custBillShip">客户详细表</param>
        /// <returns></returns>
        public int UpdateTwo(CustBillShip custBillShip)
        {
            this.db.Configuration.ValidateOnSaveEnabled = false;
            int i = 0;
            try
            {
                custBillShip.UpdateBy = base.Session["username"].ToString();
                custBillShip.UpdateDept = base.Session["DeptID"].ToString();
                custBillShip.UpdateTime = DateTime.Now;
                this.FanShe<CustBillShip>(custBillShip);
                //修改CustBillShip表
                DbEntityEntry<CustBillShip> entry = this.db.Entry<CustBillShip>(custBillShip);
                entry.State = EntityState.Unchanged;
                entry.Property<string>((CustBillShip a) => a.Inv2EMail).IsModified = true;
                entry.Property<string>((CustBillShip a) => a.Inv2Comp).IsModified = true;
                entry.Property<string>((CustBillShip a) => a.Inv2Addr).IsModified = true;
                entry.Property<int>((CustBillShip a) => a.Inv2City).IsModified = true;
                entry.Property<int?>((CustBillShip a) => a.Inv2State).IsModified = true;
                entry.Property<string>((CustBillShip a) => a.Inv2Post).IsModified = true;
                entry.Property<string>((CustBillShip a) => a.Inv2Cnty).IsModified = true;
                entry.Property<string>((CustBillShip a) => a.Ship2Comp).IsModified = true;
                entry.Property<string>((CustBillShip a) => a.Ship2Addr).IsModified = true;
                entry.Property<int>((CustBillShip a) => a.Ship2City).IsModified = true;
                entry.Property<int?>((CustBillShip a) => a.Ship2State).IsModified = true;
                entry.Property<string>((CustBillShip a) => a.Ship2Post).IsModified = true;
                entry.Property<string>((CustBillShip a) => a.Ship2Cnty).IsModified = true;
                entry.Property<string>((CustBillShip a) => a.APName).IsModified = true;
                entry.Property<string>((CustBillShip a) => a.APTitle).IsModified = true;
                entry.Property<string>((CustBillShip a) => a.APPhone).IsModified = true;
                entry.Property<string>((CustBillShip a) => a.APFax).IsModified = true;
                entry.Property<string>((CustBillShip a) => a.APMobile).IsModified = true;
                entry.Property<string>((CustBillShip a) => a.APEmail).IsModified = true;
                entry.Property<string>((CustBillShip a) => a.ARName).IsModified = true;
                entry.Property<string>((CustBillShip a) => a.ARTitle).IsModified = true;
                entry.Property<string>((CustBillShip a) => a.ARPhone).IsModified = true;
                entry.Property<string>((CustBillShip a) => a.ARFax).IsModified = true;
                entry.Property<string>((CustBillShip a) => a.ARMobile).IsModified = true;
                entry.Property<string>((CustBillShip a) => a.AREmail).IsModified = true;
                entry.Property<string>((CustBillShip a) => a.UpdateBy).IsModified = true;
                entry.Property<string>((CustBillShip a) => a.UpdateDept).IsModified = true;
                entry.Property<DateTime>((CustBillShip a) => a.UpdateTime).IsModified = true;
                i = this.db.SaveChanges();
                if (i < 0)
                {
                    return i;
                }
                if (this.UpdateSJ(custBillShip.CustomerID) < 0)
                {
                    return i;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());
                return i;
            }
            return i;
        }

        /// <summary>
        /// 修改页面保存功能-第三页签
        /// </summary>
        /// <param name="customer">客户表</param>
        /// <param name="IsList">勾选框</param>
        /// <returns></returns>
        public int UpdateThree(Models.Customer customer, bool IsList)
        {
            this.db.Configuration.ValidateOnSaveEnabled = false;
            int i = 0;
            try
            {
                //修改customer表
                this.FanShe<Models.Customer>(customer);
                customer.UpdateBy = base.Session["username"].ToString();
                customer.UpdateDept = base.Session["DeptID"].ToString();
                customer.IsList = IsList;
                DateTime dt = DateTime.Now;
                customer.CreateTime = dt;
                customer.UpdateTime = dt;
                DbEntityEntry<Models.Customer> entry2 = this.db.Entry<Models.Customer>(customer);
                entry2.State = EntityState.Unchanged;
                entry2.Property<bool>((Models.Customer b) => b.IsList).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.BankName).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.BankAddr).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.iBAN).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.SWIFT).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Owner1Name).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Owner1Addr).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Owner1Web).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Owner1Cnty).IsModified = true;
                entry2.Property<DateTime?>((Models.Customer b) => b.Owner1Date).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Owner1Type).IsModified = true;
                entry2.Property<decimal?>((Models.Customer b) => b.Owner1Ship).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Owner2Name).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Owner2Addr).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Owner2Web).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Owner2Cnty).IsModified = true;
                entry2.Property<DateTime?>((Models.Customer b) => b.Owner2Date).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Owner2Type).IsModified = true;
                entry2.Property<decimal?>((Models.Customer b) => b.Owner2Ship).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Ref1Name).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Ref1Addr).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Ref1Contact).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Ref1Title).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Ref1Phone).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Ref2Name).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Ref2Addr).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Ref2Contact).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Ref2Title).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Ref2Phone).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Curr1).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Curr2).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Curr3).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Curr4).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Yr1Forcast).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.Yr2Forcast).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.CreditLimits).IsModified = true;
                entry2.Property<string>((Models.Customer b) => b.CreditTerms).IsModified = true;//付款条款
                i = this.db.SaveChanges();
                if (i < 0)
                {
                    return i;
                }
                if (this.UpdateSJ(customer.CustomerID) < 0)
                {
                    return i;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString()); //日志
                return i;
            }
            return i;
        }
        #region 第四页签
        /// <summary>
        /// 修改页面联系表格新增功能
        /// </summary>
        /// <param name="contacts">联系表</param>
        /// <param name="CustomerId">客户ID</param>
        /// <returns></returns>
        public int customerUpdateAdd(Contacts contacts, int CustomerId)
        {
            int i = 0;
            contacts.CreateBy = base.Session["username"].ToString();
            contacts.CreateDept = base.Session["DeptID"].ToString();
            contacts.UpdateBy = base.Session["username"].ToString();
            contacts.UpdateDept = base.Session["DeptID"].ToString();
            contacts.Source = "Company";
            contacts.Share = "N";
            DateTime dt = DateTime.Now;
            contacts.CreateTime = dt;
            contacts.UpdateTime = dt;
            this.FanShe<Contacts>(contacts);
            int result;
            try
            {
                i = new GetUpdateCustomer().GetcustomerUpdateAdd(contacts);
                if (i < 0)
                {
                    result = i;
                }
                else
                {
                    this.UpdateSJ(CustomerId);
                    result = i;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString()); //日志
                result = i;
            }
            return result;
        }

        /// <summary>
        /// 修改页面联系表格删除功能
        /// </summary>
        /// <param name="ContactID">主键</param>
        /// <param name="CustomerId">客户ID</param>
        /// <returns></returns>
        public int customerUpdateDel(int ContactID, int CustomerId)
        {
            int i = 0;
            try
            {
                Contacts cs = new Contacts();
                cs.UpdateBy = base.Session["username"].ToString();
                cs.UpdateDept = base.Session["DeptID"].ToString();
                cs.UpdateTime = DateTime.Now;
                cs.ContactID = ContactID;
                this.db.Contacts.Attach(cs);
                this.db.Contacts.Remove(cs);
                i = this.db.SaveChanges();
                if (i < 0)
                {
                    return i;
                }
                if (this.UpdateSJ(CustomerId) < 0)
                {
                    return i;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());//日志
                return i;
            }
            return i;
        }

        /// <summary>
        /// 修改页面联系表格修改功能
        /// </summary>
        /// <param name="contacts">联系表</param>
        /// <param name="CustomerId">客户ID</param>
        /// <returns></returns>
        public int customerUpdateUpd(Contacts contacts, int CustomerId)
        {
            int i = 0;
            //修改Contacts表
            contacts.UpdateBy = base.Session["username"].ToString();
            contacts.UpdateDept = base.Session["DeptID"].ToString();
            DateTime dt = DateTime.Now;
            contacts.CreateTime = dt;
            contacts.UpdateTime = dt;
            this.FanShe<Contacts>(contacts);
            this.db.Configuration.ValidateOnSaveEnabled = false;
            try
            {
                DbEntityEntry<Contacts> entry = this.db.Entry<Contacts>(contacts);
                entry.State = EntityState.Unchanged;
                entry.Property<string>((Contacts a) => a.City).IsModified = true;
                entry.Property<string>((Contacts a) => a.Contact).IsModified = true;
                entry.Property<string>((Contacts a) => a.ContactType).IsModified = true;
                entry.Property<string>((Contacts a) => a.Remark).IsModified = true;
                entry.Property<string>((Contacts a) => a.UpdateBy).IsModified = true;
                entry.Property<string>((Contacts a) => a.UpdateDept).IsModified = true;
                entry.Property<DateTime>((Contacts a) => a.UpdateTime).IsModified = true;
                i = this.db.SaveChanges();
                if (i < 0)
                {
                    return i;
                }
                if (this.UpdateSJ(CustomerId) < 0)
                {
                    return i;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());//日志
                return i;
            }
            return i;
        }

        /// <summary>
        /// 修改页面联系表格共享功能
        /// </summary>
        /// <param name="ContactID">主键</param>
        /// <returns></returns>
        public int UpdateShare(Contacts contacts, int CustomerId)
        {
            int i = 0;
            contacts.UpdateBy = base.Session["username"].ToString();
            contacts.UpdateDept = base.Session["DeptID"].ToString();
            contacts.UpdateTime = DateTime.Now;
            try
            {
                this.FanShe<Contacts>(contacts);
                if (contacts.Share == "A")
                {
                    contacts.Share = "N";
                }
                else if (contacts.Share == "N")
                {
                    contacts.Share = "A";
                }
                //更新Contacts表
                DbEntityEntry<Contacts> entry = this.db.Entry<Contacts>(contacts);
                entry.State = EntityState.Unchanged;
                entry.Property<string>((Contacts a) => a.Share).IsModified = true;
                entry.Property<string>((Contacts a) => a.UpdateBy).IsModified = true;
                entry.Property<string>((Contacts a) => a.UpdateDept).IsModified = true;
                entry.Property<DateTime>((Contacts a) => a.UpdateTime).IsModified = true;
                i = this.db.SaveChanges();
                if (i < 0)
                {
                    return i;
                }
                if (this.UpdateSJ(CustomerId) < 0)
                {
                    return i;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());
                return i;
            }
            return i;
        }
        #endregion

        /// <summary>
        /// 修改后将客户表状态改为P,更新修改人，修改部门和时间
        /// </summary>
        /// <param name="CustomerId">客户ID</param>
        /// <returns></returns>
        public int UpdateSJ(int CustomerId)
        {
            int i = 0;
            int result;
            try
            {
                string UpdateBy = base.Session["username"].ToString();
                string UpdateDept = base.Session["DeptID"].ToString();
                i = new GetUpdateCustomer().GetUpdateSJ(CustomerId, UpdateBy, UpdateDept);
                result = i;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());
                result = i;
            }
            return result;
        }

        /// <summary>
        /// 新增/修改市场部补充意见+改状态
        /// </summary>
        /// <returns></returns>
        public int AuditInfoAdd(Customer_AuditInfo info)
        {
            int i = 0;
            int result;
            try
            {
                info.MDCreateBy = base.Session["username"].ToString();
                info.MDCreateDept = base.Session["DeptID"].ToString();
                this.FanShe<Customer_AuditInfo>(info);
                i = new GetDetail().GetAuditInfoAdd(info);
                result = i;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());
                result = i;
            }
            return result;
        }

        /// <summary>
        /// 修改补充意见+改状态
        /// </summary>
        /// <returns></returns>
        public int AuditInfoUpdate(Customer_AuditInfo info, int num)
        {
            int i = 0;
            int result;
            try
            {
                this.FanShe<Customer_AuditInfo>(info);
                info.ACCreateBy = base.Session["username"].ToString();
                info.ACCreateDept = base.Session["DeptID"].ToString();
                i = new GetDetail().GetAuditInfoUpdate(info, num);
                result = i;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());
                result = i;
            }
            return result;
        }

        /// <summary>
        /// 禁用/解禁功能
        /// </summary>
        /// <param name="CustomerId">客户ID</param>
        /// <returns></returns>
        public int UpdateJY(int CustomerId, string Status)
        {
            int i = 0;
            int result;
            try
            {
                string UpdateBy = base.Session["username"].ToString();
                string UpdateDept = base.Session["DeptID"].ToString();
                i = new GetDefault().GetUpdateJY(CustomerId, UpdateBy, UpdateDept, Status);
                result = i;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());
                result = i;
            }
            return result;
        }

        /// <summary>
        /// 反射
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void FanShe<T>(T t)
        {
            Type infoType = t.GetType();
            foreach (PropertyInfo item in infoType.GetProperties())
            {
                item.GetValue(t);
                if (item.GetValue(t) == null && item.PropertyType.Name == "String")
                {
                    infoType.GetProperty(item.Name).SetValue(t, "");
                }
            }
        }
    }
}
