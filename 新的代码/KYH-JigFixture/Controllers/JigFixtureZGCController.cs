using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using Customer.Models.PublicSqlMethods;
using KYH_JigFixture.Models;
using KYH_JigFixture.Models.PublicSqlMethods;

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
        [ValidateInput(false)]
        public int AddFunction(int num, Tools_Acquire_Trace TAT, string userid)
        {
            JigFixtureController jigFixtureController = DependencyResolver.Current.GetService<JigFixtureController>();
            int a = 0;
            string name = jigFixtureController.GetUser(userid);
            string DeptID = jigFixtureController.GetDepartment(name);
            try
            {
                if (num == 0)
                {
                    TAT.UpdateNumber = 0;
                    this.db.Tools_Acquire_Trace.Add(TAT);
                }
                if (num == 1)
                {
                    Tools_Acquire_Trace oldTAT = new Tools_Acquire_Trace();
                    oldTAT = this.db.Set<Tools_Acquire_Trace>().AsNoTracking().FirstOrDefault((Tools_Acquire_Trace p) => p.FID == TAT.FID);
                    TAT.UpdateNumber = oldTAT.UpdateNumber + 1;
                    TAT.Remark = oldTAT.Remark;
                    DbEntityEntry<Tools_Acquire_Trace> entry = this.db.Entry<Tools_Acquire_Trace>(TAT);
                    entry.State = EntityState.Modified;
                    string User = jigFixtureController.GetUser(userid);
                    LogThread.ActionLog(User, DeptID, "修改了原采购单号(" + oldTAT.K3PO_Num + ")", "U", name);
                }
                a = this.db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString());
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
		public int UpdateFunction(int num, int id, string ZiDuan, string userid)
		{
			int a = 0;
			try
			{
				JigFixtureController jigFixtureController = DependencyResolver.Current.GetService<JigFixtureController>();
				string name = jigFixtureController.GetUser(userid);
				string DeptID = jigFixtureController.GetDepartment(name);
				Tools_Acquire_Trace found = this.db.Tools_Acquire_Trace.FirstOrDefault((Tools_Acquire_Trace e) => e.FID == id);
				found.UpdateNumber++;
				if (num == 1)
				{
					found.Remark = ZiDuan;
					DbEntityEntry<Tools_Acquire_Trace> entry = this.db.Entry<Tools_Acquire_Trace>(found);
					entry.Property<string>((Tools_Acquire_Trace e) => e.Remark).IsModified = true;
					entry.Property<int>((Tools_Acquire_Trace e) => e.UpdateNumber).IsModified = true;
				}
				if (num == 2)
				{
					if (ZiDuan == "")
					{
						found.Prop_Arr_Date = null;
					}
					else
					{
						found.Prop_Arr_Date = new DateTime?(DateTime.Parse(ZiDuan));
					}
					DbEntityEntry<Tools_Acquire_Trace> entry2 = this.db.Entry<Tools_Acquire_Trace>(found);
					entry2.Property<DateTime?>((Tools_Acquire_Trace e) => e.Prop_Arr_Date).IsModified = true;
					entry2.Property<int>((Tools_Acquire_Trace e) => e.UpdateNumber).IsModified = true;
					string User = jigFixtureController.GetUser(userid);
					LogThread.ActionLog(User, DeptID, "修改了原交期(" + found.Prop_Arr_Date.ToString() + ")", "U", name);
				}
				if (num == 3)
				{
					if (ZiDuan == "")
					{
						found.K3Recv_Date = null;
					}
					else
					{
						found.K3Recv_Date = new DateTime?(DateTime.Parse(ZiDuan));
					}
					DbEntityEntry<Tools_Acquire_Trace> entry3 = this.db.Entry<Tools_Acquire_Trace>(found);
					entry3.Property<DateTime?>((Tools_Acquire_Trace e) => e.K3Recv_Date).IsModified = true;
					entry3.Property<int>((Tools_Acquire_Trace e) => e.UpdateNumber).IsModified = true;
					string User2 = jigFixtureController.GetUser(userid);
					LogThread.ActionLog(User2, DeptID, "修改了原入库日期(" + found.K3Recv_Date.ToString() + ")", "U", name);
				}
				a = this.db.SaveChanges();
			}
			catch (Exception ex)
			{
				LogHelper.Write(ex.ToString());
				throw;
			}
			return a;
		}

	}
}