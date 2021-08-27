using System;
using System.Collections.Generic;
using System.Web.Mvc;
using KYH_GetK3POInformation.Models;
using KYH_GetK3POInformation.Models.PublicSqlMethods;
using KYH_GetK3POInformation.Models.SqlMethods;
using Newtonsoft.Json;

namespace KYH_GetK3POInformation.Controllers
{
    // Token: 0x0200000F RID: 15
    public class AddLoadingListAddPOdata_TempController : Controller
    {
        // Token: 0x0600008F RID: 143 RVA: 0x00002E09 File Offset: 0x00001009
        public ActionResult Index()
        {
            return base.View();
        }

        /// <summary>
        /// 新增临时表数据 ps:因为表名不固定所以ef改用SQL了
        /// </summary>
        /// <param name="ArrayList"></param>
        /// <returns></returns>
        public int AddFunction(string ArrayList)
        {
            int result = 0;
            try
            {
                int a = 0;
                if (new GetIndex().CheckTable(base.Session["username"].ToString()) == 0)
                {
                    if (!new GetIndex().CREATETable(base.Session["username"].ToString()))
                    {
                        return 0;
                    }
                }
                else if (!new GetIndex().TRUNCATETABLE(base.Session["username"].ToString()))
                {
                    return 0;
                }
                ArrayList = ArrayList.Replace("GIP-PO", "GIP_PO");
                ArrayList = ArrayList.Replace("Part-No", "Part_No");
                List<Others> List = JsonConvert.DeserializeObject<List<Others>>(ArrayList);
                foreach (Others Con in List)
                {
                    List[a].Unit = List[a].Unit.ToUpper(); //转大写
                    //List[a].Qty = List[a].Qty.Replace("，", ""); //去掉中文逗号
                    //List[a].Qty = List[a].Qty.Replace(",", ""); //去掉英文逗号
                    if (!new GetIndex().InsertData(base.Session["username"].ToString(), List[a]))
                    {
                        break;
                        return 0;
                    }
                    a++;
                    result = 1;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());
                return 0;
            }
            return result;
        }

        /// <summary>
        /// 修改临时表数据，（添加K3数据到临时表） ps:因为表名不固定所以ef改用SQL了
        /// </summary>
        /// <param name="username"></param>
        /// <param name="ArrayList"></param>
        /// <returns></returns>
        public int UpdFunction(string username, LoadingListAddPOdata_Temp ArrayList)
        {
            int result;
            try
            {
                if (!new GetIndex().UpdateData(username, ArrayList))
                {
                    result = 0;
                }
                else
                {
                    result = 1;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());
                return 0;
            }
            return result;
        }

        // Token: 0x0400003A RID: 58
        private WebStationEntities db = new WebStationEntities();
    }
}
