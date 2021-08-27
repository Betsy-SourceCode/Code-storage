using System;
using System.IO;

namespace KYH_NewEmailDomain
{
	// Token: 0x0200000C RID: 12
	public class LogHelper
	{
		/// <summary>
		/// 创建日志文件路径
		/// </summary>
		private static string Path
		{
			get
			{
				return "C:\\ATMLog";
			}
		}

		/// <summary>
		/// 写入日志
		/// </summary>
		/// <param name="content"></param>
		public static void Write(string content)
		{
			Action<string> write = delegate(string text)
			{
				LogHelper.WriteLogs(text);
			};
			write(content);
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="content"></param>
		public static void WriteLogs(string content)
		{
			try
			{
				if (!Directory.Exists(LogHelper.Path))
				{
					Directory.CreateDirectory(LogHelper.Path);
				}
				DateTime dt = DateTime.Now;
				string txtpath = LogHelper.Path + "\\Log" + dt.ToString("yyyy-MM-dd") + ".txt";
				if (!File.Exists(txtpath))
				{
					using (FileStream fs = new FileStream(txtpath, FileMode.Create, FileAccess.Write, FileShare.Write))
					{
						using (StreamWriter sw = new StreamWriter(fs))
						{
							sw.WriteLine(dt.ToString("yyyy-MM-dd HH:mm:ss:\n"));
							sw.WriteLine(content + "\n");
							sw.WriteLine("------------------------------------------------");
						}
						goto IL_FE;
					}
				}
				using (FileStream fs2 = new FileStream(txtpath, FileMode.Append, FileAccess.Write, FileShare.Write))
				{
					using (StreamWriter sw2 = new StreamWriter(fs2))
					{
						sw2.WriteLine(dt.ToString("yyyy-MM-dd        HH:mm:ss:\n"));
						sw2.WriteLine(content + "\n");
						sw2.WriteLine("------------------------------------------------");
					}
				}
				IL_FE:;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x04000028 RID: 40
		private const string _path = "C:\\ATMLog";
	}
}
