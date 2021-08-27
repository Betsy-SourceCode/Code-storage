using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using KYH_KnowledgeBase.Models;
using KYH_KnowledgeBase.Models.PublicSqlMethods;
using Newtonsoft.Json;

namespace KYH_KnowledgeBase.Controllers
{
    // Token: 0x02000010 RID: 16
    public class KnowledgeBaseZSGController : Controller
    {
        public ActionResult Index()
        {
            return base.View();
        }

        /// <summary>
        /// 新增/修改
        /// </summary>
        /// <param name="num">1:新增/2:整体修改/3:只修改查看次数</param>
        /// <param name="qnA.QnAID">主键</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public string AddUpdateFunction(int num, QnA qnA)
        {
            int a = 0;
            int QnAIDs = qnA.QnAID;
            string CreateBy = base.Session["username"].ToString();
            string CreateDept = base.Session["deptid"].ToString();
            string UserID = base.Session["username"].ToString();
            string name = this.authority.SelectEName(UserID);
            string DeptID = this.authority.SelectDeptID(UserID);
            QnA found = this.db.QnA.FirstOrDefault((QnA e) => e.QnAID == QnAIDs);//查以前的数据
            try
            {
                this.db.Configuration.ValidateOnSaveEnabled = false;
                #region 图片做特殊操作
                for (int i = 0; i < base.Request.Files.Count; i++)
                {
                    Stream inputStream = base.Request.Files[i].InputStream;
                    byte[] PicBt = new byte[inputStream.Length];
                    inputStream.Read(PicBt, 0, PicBt.Length);
                    if (i == 0)
                    {
                        qnA.QPic01 = PicBt;
                    }
                    else if (i == 1)
                    {
                        qnA.QPic02 = PicBt;
                    }
                    else if (i == 2)
                    {
                        qnA.QPic03 = PicBt;
                    }
                    else if (i == 3)
                    {
                        qnA.QPic04 = PicBt;
                    }
                    else if (i == 4)
                    {
                        qnA.APic01 = PicBt;
                    }
                    else if (i == 5)
                    {
                        qnA.APic02 = PicBt;
                    }
                    else if (i == 6)
                    {
                        qnA.APic03 = PicBt;
                    }
                    else if (i == 7)
                    {
                        qnA.APic04 = PicBt;
                    }
                }
                #endregion
                if (num == 1)
                {
                    //设默认值
                    qnA.CreateBy = CreateBy;
                    qnA.CreateDept = CreateDept;
                    qnA.UpdateBy = CreateBy;
                    qnA.UpdateDept = CreateDept;
                    qnA.UpdateTime = new DateTime?(DateTime.Now);
                    qnA.CreateTime = new DateTime?(DateTime.Now);
                    qnA.ClickTimes = new short?(0);
                    this.db.QnA.Add(qnA);
                    LogThread.ActionLog(UserID, DeptID, "新增了一条系统技术知识记录(作者名为" + qnA.Author + ")", "N", name);
                }
                if (num == 2)
                {
                    this.FanShe<QnA>(qnA, found); //反射
                    found.QnAID = QnAIDs;
                    found.UpdateTime = new DateTime?(DateTime.Now);
                    found.UpdateBy = CreateBy;
                    found.UpdateDept = CreateDept;
                    if (qnA.KeyWord == null || qnA.KeyWord == "")
                    {
                        found.KeyWord = null;
                    }
                    this.db.Entry<QnA>(found).State = EntityState.Modified;
                    LogThread.ActionLog(UserID, DeptID, "修改了一条系统技术知识记录(作者名为" + qnA.Author + ")", "U", name);
                }
                if (num == 3)
                {
                    if (found.ClickTimes == null)   //防止没赋默认值的情况
                    {
                        found.ClickTimes = 0;
                    }
                    found.ClickTimes++; //查看次数+1
                    this.db.Entry<QnA>(found).Property<short?>((QnA e) => e.ClickTimes).IsModified = true;
                }
                if (num == 4 && found != null)
                {
                    this.db.QnA.Remove(found);
                    LogThread.ActionLog(UserID, DeptID, "删除了一条系统技术知识记录(作者名为" + found.Author + ")", "D", name);
                }
                a = this.db.SaveChanges();
                if (a < 0)
                {
                    return a.ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());//日志
                return a.ToString();
            }
            return JsonConvert.SerializeObject(new ResponseJson
            {
                id = a,
                Success = qnA.QnAID
            });
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="Author"></param>
        public void DaPrinting(string Author)
        {
            string UserID = base.Session["username"].ToString();
            if (UserID != null && UserID != "")
            {
                string name = this.authority.SelectEName(UserID);
                string DeptID = this.authority.SelectDeptID(UserID);
                LogThread.ActionLog(UserID, DeptID, "打印了一条系统技术知识记录(作者名为" + Author + ")", "P", name);
            }
        }

        /// <summary>
        /// 反射
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="a"></param>
        public void FanShe<T>(T t, T a)
        {
            Type infoType = t.GetType();
            PropertyInfo[] properties = infoType.GetProperties();
            Type infoType2 = a.GetType();
            foreach (PropertyInfo item in properties)
            {
                object ia = item.GetValue(t);
                if ((ia != null && ia.GetType().BaseType == typeof(Array) && ((byte[])ia).Length != 0) || (ia != null && ia.GetType().BaseType != typeof(Array)))
                {
                    infoType.GetProperty(item.Name);
                    infoType2.GetProperty(item.Name).SetValue(a, ia);
                }
            }
        }

        // Token: 0x0400001E RID: 30
        private WebStationEntitiess db = new WebStationEntitiess();

        // Token: 0x0400001F RID: 31
        private Authority authority = new Authority();
    }
}
