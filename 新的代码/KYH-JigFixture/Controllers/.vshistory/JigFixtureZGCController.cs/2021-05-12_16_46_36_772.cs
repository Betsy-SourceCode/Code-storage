using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
        /// 新增/修改
        /// </summary>
        /// <param name="num">1-新增，2-修改</param>
        /// <param name="TAT">Tools_Acquire_Trace新表</param>
        /// <returns></returns>
        public string AddUpdateFunction(int num, SelectOA_Result OA, Others K3)
        {
            try
            {
                Tools_Acquire_Trace TAT = new Tools_Acquire_Trace();
                db.Configuration.ValidateOnSaveEnabled = false;
                //新增
                if (num == 0)
                {
                    //因为是不同的两张表，所以只能赋值
                    TAT.ApplyDate = (DateTime)OA.ApplyDate;
                    TAT.OAFlow_Num = OA.Serial_Num;
                    TAT.Mat_Code = OA.Mat_Code;
                    TAT.Mat_Name = OA.Mat_Name;
                    TAT.Unit = OA.Unit;
                    TAT.Qty = (decimal)OA.Qty;
                    TAT.Arr_Date = OA.RequireDate;
                    TAT.Approval = OA.AppD_Name;
                    TAT.K3PO_Num = K3.K3PO_Num;
                    TAT.K3PO_Date = K3.K3PO_Date;
                    TAT.Supplier = K3.Supplier;
                    TAT.K3Qty = K3.K3FQty;
                    TAT.Buyer = K3.Buyer;
                    //db.Tools_Acquire_Trace.Add(OA);
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