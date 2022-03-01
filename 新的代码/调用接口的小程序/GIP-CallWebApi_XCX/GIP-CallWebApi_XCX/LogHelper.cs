using System;
using System.IO;
using System.Windows.Forms;

namespace GIP_CallWebApi
{
    // Token: 0x02000012 RID: 18
    public class LogHelper
    {
        private const string _path = "";
        /// <summary>
        /// 创建日志文件路径
        /// </summary>
        private static string Path
        {
            get
            {
                string appPath = Application.StartupPath;
                DirectoryInfo topDir = Directory.GetParent(appPath);
                string topDirPath = topDir.FullName;//topDirPath即上层目录
                return topDirPath;
            }
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="content"></param>
        public static void Write(int id, string content)
        {
            Action<string> write = delegate (string text)
            {
                if (id == 0) //文档文本记录次数
                {
                    WriteLogs(0, text);
                }
                else
                {
                    //记录错误数据
                    WriteLogs(1, text);
                }
            };
            write(content);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="content"></param>
        public static void WriteLogs(int id, string content)
        {
            try
            {
                if (!Directory.Exists(LogHelper.Path))
                {
                    Directory.CreateDirectory(LogHelper.Path);
                }
                DateTime dt = DateTime.Now;
                string txtpath = null;
                if (id == 0)
                {
                    txtpath = Path + "\\计数专用" + ".txt";
                }
                else
                {
                    txtpath = Path + "\\Log" + dt.ToString("yyyy-MM-dd") + ".txt";
                }
                if (!File.Exists(txtpath))
                {
                    using (FileStream fs = new FileStream(txtpath, FileMode.Create, FileAccess.Write, FileShare.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            if (id == 0)
                            {
                                sw.WriteLine(content);
                                sw.Close();
                                fs.Close();
                                return;
                            }
                            else
                            {
                                sw.WriteLine(dt.ToString("yyyy-MM-dd HH:mm:ss:\n"));
                                sw.WriteLine(content + "\n");
                                sw.WriteLine("------------------------------------------------");
                            }
                        }
                        goto IL_FE;
                    }
                }
                using (FileStream fs2 = new FileStream(txtpath, FileMode.Append, FileAccess.Write, FileShare.Write))
                {
                    using (StreamWriter sw2 = new StreamWriter(fs2))
                    {
                        if (id == 0)
                        {
                            sw2.WriteLine(content);
                            sw2.Close();
                            fs2.Close();
                            return;
                        }
                        sw2.WriteLine(dt.ToString("yyyy-MM-dd        HH:mm:ss:\n"));
                        sw2.WriteLine(content + "\n");
                        sw2.WriteLine("------------------------------------------------");
                    }
                }
            IL_FE:;
            }
            catch (Exception ex)
            {
            }
        }
    }
}
