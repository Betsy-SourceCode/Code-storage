using MIS_InventoryTracking.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MIS_InventoryTracking.Controllers
{
    public class InventoryTrackingController : Controller
    {
        // GET: InventoryTracking
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 通过账套和物料号带出物料名称规格
        /// </summary>
        /// <param name="Ledger">账套名</param>
        /// <param name="Fnumber"></param>
        /// <returns></returns>
        public string GetMaterialMessage(string Ledger, string Fnumber)
        {
            try
            {
                var result = new SqlStatement().GetMaterialMessageSQL(Ledger, Fnumber);
                return JsonConvert.SerializeObject(result);
            }
            catch (Exception ex)
            {
                return "error";
                throw;
            }

        }
    }
}