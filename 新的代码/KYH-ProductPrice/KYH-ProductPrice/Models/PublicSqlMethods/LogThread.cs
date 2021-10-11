using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace KYH_ProductPrice.Models.PublicSqlMethods
{
	// Token: 0x02000013 RID: 19
	public class LogThread
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00003281 File Offset: 0x00001481
		// (set) Token: 0x0600011B RID: 283 RVA: 0x00003278 File Offset: 0x00001478
		public ActionLog2021 log { get; set; }

		// Token: 0x0600011D RID: 285 RVA: 0x0000328C File Offset: 0x0000148C
		public void GetActionLogAdd()
		{
			try
			{
				string sql = "INSERT INTO ActionLog" + DateTime.Now.Year.ToString() + "(UserID,DeptID,FunctionID,ActionType,TableName,TableKey,Description)VALUES(@UserID,@DeptID,@FunctionID,@ActionType,@TableName,@TableKey,@Description)";
				SqlParameter[] para = new SqlParameter[]
				{
					new SqlParameter("@UserID", this.log.UserID),
					new SqlParameter("@DeptID", this.log.DeptID),
					new SqlParameter("@FunctionID", this.log.FunctionID),
					new SqlParameter("@ActionType", this.log.ActionType),
					new SqlParameter("@TableName", this.log.TableName),
					new SqlParameter("@TableKey", this.log.TableKey),
					new SqlParameter("@Description", this.log.Description)
				};
				Database database = this.db.Database;
				string sql2 = sql;
				object[] parameters = para;
				int i = database.ExecuteSqlCommand(sql2, parameters);
			}
			catch (Exception ex)
			{
				LogHelper.Write(ex.ToString());
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x000033B0 File Offset: 0x000015B0
		public static string GetLocalIP()
		{
			string result;
			try
			{
				string HostName = Dns.GetHostName();
				IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
				for (int i = 0; i < IpEntry.AddressList.Length; i++)
				{
					if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
					{
						string ip = IpEntry.AddressList[i].ToString();
						return IpEntry.AddressList[i].ToString();
					}
				}
				result = "";
			}
			catch (Exception ex)
			{
				result = ex.Message;
			}
			return result;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000343C File Offset: 0x0000163C
		public static void ActionLog(string UserID, string DeptID, string canshu, string ActionType, string name)
		{
			Thread th = new Thread(new ThreadStart(new LogThread
			{
				log = new ActionLog2021
				{
					UserID = UserID,
					DeptID = DeptID,
					FunctionID = 144,
					ActionType = ActionType,
					TableName = "Tools_Acquire_Trace",
					TableKey = "FID",
					Description = name + canshu
				}
			}.GetActionLogAdd));
			th.Start();
		}

		// Token: 0x0400007F RID: 127
		private WebStationEntities db = new WebStationEntities();
	}
}
