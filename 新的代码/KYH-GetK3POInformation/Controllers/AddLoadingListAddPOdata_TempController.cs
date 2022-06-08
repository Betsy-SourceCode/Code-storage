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
        GetK3POInformationController otherController = DependencyResolver.Current.GetService<GetK3POInformationController>();
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
                ArrayList = ArrayList.Replace("Serial-No", "Serial_No");
                List<Others> List = JsonConvert.DeserializeObject<List<Others>>(ArrayList);
                foreach (Others Con in List)
                {
                    List[a].Unit = List[a].Unit.ToUpper(); //转大写
                    List[a].Qty = List[a].Qty.Replace("，", ""); //去掉中文逗号
                    List[a].Qty = List[a].Qty.Replace(",", ""); //去掉英文逗号
                    if (!new GetIndex().InsertData(base.Session["username"].ToString(), List[a]))
                    {
                        result = 0;
                        break;
                    }
                    a++;
                    result = 1;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write("AddFunction:" + ex.ToString());
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
        public int UpdFunction(string username, NewLoadingListAddPOdata_Temp ArrayList)
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


        /// <summary>
        /// 查账套抓出数据批量修改到主表（优化）
        /// </summary>
        /// <param name="DatabaseName">服务器的数据库名</param>
        /// <param name="TableName">副表名</param>
        /// <param name="oginName">登录者的主表名</param>
        /// <returns></returns>
        public string NewUpdFunction()
        {
            try
            {
                string LoginName = "LoadingListAddPOdata_Temp_" + Session["username"].ToString();
                int result = new GetIndex().BatchUpdateSql(LoginName);
                if (result > 0)
                {
                    //调用查询的存储过程
                    List<LoadingListAddPOdata_Temp_Select> list = new GetIndex().NewGetIndexList(LoginName);
                    return JsonConvert.SerializeObject(list);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write("NewUpdFunction:" + ex.Message);
                return null;
            }
        }




        // Token: 0x0400003A RID: 58
        private WebStationEntities db = new WebStationEntities();
    }
}
