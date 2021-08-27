using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using KYH_ProductPrice.Models;
using KYH_ProductPrice.Models.PublicSqlMethods;
using KYH_ProductPrice.Models.SqlStatement;
using Newtonsoft.Json;

namespace KYH_ProductPrice.Controllers
{
    // Token: 0x02000017 RID: 23
    public class ProductPriceZGController : Controller
    {
        private WebStationEntities db = new WebStationEntities();

        public ActionResult Index()
        {
            return base.View();
        }

        /// <summary>
        /// 新增/整体修改
        /// </summary>
        /// <param name="CPP"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public string AddCustProductPrice(CustProductPriceRecords CPP, int num)
        {
            string username = base.Session["username"].ToString();
            string DeptIDs = base.Session["DeptIDs"].ToString();
            string result;
            try
            {
                this.db.Configuration.ValidateOnSaveEnabled = false;
                if (num == 0)  //新增
                {
                    CPP.UpdCurr = CPP.PrvCurr;
                    CPP.UpdUnit = CPP.PrvUnit;
                    CPP.EffType = "NW";
                    CPP.EffDetail = "新增信息记录";
                }
                if (num == 1)  //整体修改
                {
                    CustProductPriceRecords oldcpp = this.db.CustProductPriceRecords.FirstOrDefault((CustProductPriceRecords e) => e.CPSerial == CPP.CPSerial);
                    CPP.Ledger = oldcpp.Ledger;
                    CPP.CustomerID = oldcpp.CustomerID;
                    CPP.FNumber = oldcpp.FNumber;
                    CPP.CustProdCode = oldcpp.CustProdCode;
                    CPP.CustProdName = oldcpp.CustProdName;
                    CPP.PrvCurr = oldcpp.PrvCurr;
                    CPP.PrvUnit = oldcpp.PrvUnit;
                    CPP.Remarks_FD = oldcpp.Remarks_FD;
                }
                CPP.CreateBy = username;
                CPP.CreateDept = DeptIDs;
                CPP.Cancel = true;
                CPP.CreateTime = DateTime.Now;
                CPP.UpdateBy = username;
                CPP.UpdateDept = DeptIDs;
                CPP.UpdateTime = DateTime.Now;
                this.db.CustProductPriceRecords.Add(CPP);
                int a = this.db.SaveChanges();
                ResponseJson json = new ResponseJson
                {
                    id = CPP.CPSerial,
                    Success = a
                };
                result = JsonConvert.SerializeObject(json);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());
                throw;
            }
            return result;
        }

        /// <summary>
        /// 单个字段修改
        /// </summary>
        /// <param name="CPP"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public string ChangeCustProductPriceRecords(CustProductPriceRecords CPP, int num)
        {
            string result;
            try
            {
                string username = base.Session["username"].ToString();
                string DeptIDs = base.Session["DeptIDs"].ToString();
                CustProductPriceRecords oldcpp = this.db.CustProductPriceRecords.FirstOrDefault((CustProductPriceRecords e) => e.CPSerial == CPP.CPSerial);  //查以前的数据
                if (num == 1) //作废状态
                {
                    oldcpp.Cancel = CPP.Cancel;
                }
                if (num == 2)  //备注
                {
                    if (CPP.Remarks_MD != null)
                    {
                        oldcpp.Remarks_MD = CPP.Remarks_MD; //业务备注
                    }
                    if (CPP.Remarks_FD != null)
                    {
                        oldcpp.Remarks_FD = CPP.Remarks_FD; //财务备注
                    }
                }
                oldcpp.UpdateBy = username;
                oldcpp.UpdateDept = DeptIDs;
                oldcpp.UpdateTime = DateTime.Now;
                DbEntityEntry<CustProductPriceRecords> entry = this.db.Entry<CustProductPriceRecords>(oldcpp);
                entry.State = EntityState.Modified;
                int a = this.db.SaveChanges();
                ResponseJson json = new ResponseJson
                {
                    id = CPP.CPSerial,
                    Success = a
                };
                result = JsonConvert.SerializeObject(json);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());
                throw;
            }
            return result;
        }

        /// <summary>
        /// 添加/删除业务员
        /// </summary>
        /// <param name="num"></param>
        /// <param name="id"></param>
        /// <param name="ServiceBy"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public int ChangeServiceBy(int num, int id, string ServiceBy)
        {
            int result;
            try
            {
                int a = 0;
                Customer Customer = this.db.Customer.FirstOrDefault((Customer e) => e.CustomerID == id);
                if (num == 0) //添加
                {
                    if (new SetupCustomerSql().GetISServiceBySql(ServiceBy, id) > 0)
                    {
                        return a;
                    }
                    if (Customer.ServiceBy == null || Customer.ServiceBy == "")
                    {
                        Customer.ServiceBy = "|" + ServiceBy + "|";
                    }
                    else
                    {
                        Customer.ServiceBy = Customer.ServiceBy + ServiceBy + "|";
                    }
                }
                if (num == 1) //删除
                {
                    Customer.ServiceBy = Customer.ServiceBy.Replace(ServiceBy + "|", "");
                    if (Customer.ServiceBy.Length <= 3)
                    {
                        Customer.ServiceBy = null;
                    }
                }
                DbEntityEntry<Customer> entry = this.db.Entry<Customer>(Customer);
                entry.Property<string>((Customer e) => e.ServiceBy).IsModified = true;
                a = this.db.SaveChanges();
                result = a;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());
                throw;
            }
            return result;
        }

    }
}
