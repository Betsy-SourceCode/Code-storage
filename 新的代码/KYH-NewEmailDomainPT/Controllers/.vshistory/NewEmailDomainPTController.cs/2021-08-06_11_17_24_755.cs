using KYH_NewEmailDomainPT.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KYH_NewEmailDomainPT.Controllers
{
    public class NewEmailDomainPTController : Controller
    {
        WebStationEntities db = new WebStationEntities();
        // GET: NewEmailDomainPT
        public ActionResult Index()
        {
            return View();
        }
        public string IndexData(NewEmailDomain2GIP NewEmailDomain, string EndTime)
        {
            NewEmailDomain2GIP newEmailDomain2GIP = new GetIndex().GetIndexListSql(NewEmailDomain, EndTime);
            return JsonConvert.SerializeObject(newEmailDomain2GIP);
        }
    }
}