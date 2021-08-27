using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Customer.Models.PublicSqlMethods;
using KYH_JigFixture.Models;
using KYH_JigFixture.Models.SqlMethods;

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
        /// 新增/修改新表
        /// </summary>
        /// <param name="num">1-新增，2-修改</param>
        /// <param name="TAT">Tools_Acquire_Trace新表</param>
        /// <returns></returns>
        public int AddUpdateFunction(int num, Tools_Acquire_Trace TAT)
        {
            int a = 0;
            try
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                //新增
                if (num == 0)
                {
                    //因为是不同的两张表，所以只能赋值(ps:交期回复和入库日期新增默认为空)
                    //TAT.ApplyDate = (DateTime)OA.ApplyDate;
                    //TAT.OAFlow_Num = OA.Serial_Num;
                    //TAT.Mat_Code = OA.Mat_Code;
                    //TAT.Mat_Name = OA.Mat_Name;
                    //TAT.Unit = OA.Unit;
                    //TAT.Qty = (decimal)OA.Qty;
                    //TAT.Arr_Date = OA.RequireDate;
                    //TAT.Approval = OA.AppD_Name;
                    //TAT.K3PO_Num = K3.K3PO_Num;
                    //TAT.K3PO_Date = K3.K3PO_Date;
                    //TAT.Supplier = K3.Supplier;
                    //TAT.K3Qty = K3.K3FQty;
                    //TAT.Buyer = K3.Buyer;
                    db.Tools_Acquire_Trace.Add(TAT);

                }
                else
                {
                    //修改
                }
                a = db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());  //日志
                return a;
            }
            return a;
        }

        /// <summary>
        /// 修改备注字段
        /// </summary>
        /// <param name="id">唯一id</param>
        /// <param name="Remark">备注内容</param>
        /// <returns></returns>
        public int AddRemark(string id, string Remark)
        {
            Tools_Acquire_Trace found = db.Tools_Acquire_Trace.FirstOrDefault(e => e.FID == (int)id);//查以前的数据

        }

    }
}