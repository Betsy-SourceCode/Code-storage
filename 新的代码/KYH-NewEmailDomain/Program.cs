using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KYH_NewEmailDomain
{
    static class Program
    {
        static WebStationEntities db = new WebStationEntities();

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //调用运行批处理文件
            if (ImplementBat("E:\\KYH\\getmaillog.bat"))
            {
                //读取指定文件夹的日志档内容
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start(); //  开始监视代码
                ReadTxtContent("E:\\测试\\maillog-" + DateTime.Now.ToString("yyyyMMdd"));
                stopwatch.Stop(); //  停止监视
                TimeSpan timeSpan = stopwatch.Elapsed; //  获取总时间
                double minutes = timeSpan.TotalMinutes;  // 分钟
                double seconds = timeSpan.TotalSeconds;  //  秒数
                MessageBox.Show("运行了:" + minutes + "分钟," + seconds + "秒");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //Application.Run(new Index());
                Application.Run();//不运行窗体
            }
        }
        /// <summary>
        /// 读取指定文件夹的日志档内容
        /// </summary>
        /// <param name="Path">文件地址</param>
        public static void ReadTxtContent(string Path)
        {
            string content = ""; string Newcontent = "";
            try
            {
                StreamReader sr = new StreamReader(Path, Encoding.Default);
                while ((content = sr.ReadLine()) != null)  //一行行读取
                {
                    //在文本里寻找邮件域名
                    if (content.IndexOf("from=<") >= 0)
                    {
                        int length = content.Length;
                        //截取新域名(@后面到>)
                        int startIndex = content.IndexOf("from=<");//开始位置

                        Newcontent = content.Substring(startIndex, length - startIndex);//从开始位置截取一个新的字符串
                        int qianIndex = Newcontent.IndexOf("@") + 1;
                        int houIndex = Newcontent.IndexOf(">");//结束位置
                        if (houIndex - qianIndex <= 0)
                        {
                            continue;
                        }
                        if (qianIndex != 0)
                        {
                            Newcontent = Newcontent.Substring(qianIndex, houIndex - qianIndex); //取域名

                        }
                        else
                        {
                            continue;
                        }
                        //判断是不是等于gipmail.com同时查询域名是否在表里存在,不存在就添加到表里
                        NewEmailDomain2GIP OldEmailDomain = db.NewEmailDomain2GIP.FirstOrDefault((NewEmailDomain2GIP e) => e.DomainName == Newcontent);
                        if (Newcontent != "gregiphlmail.gipmail.com" && OldEmailDomain == null && Newcontent != "gipmail.com" && Newcontent != "root")
                        {
                            NewEmailDomain2GIP NewEmailDomain = new NewEmailDomain2GIP();
                            //添加到新表  
                            NewEmailDomain.DomainName = Newcontent.ToLower();  //以小写的形式写入表
                            NewEmailDomain.FindDate = DateTime.Now;
                            NewEmailDomain.SourceType = "1";
                            db.NewEmailDomain2GIP.Add(NewEmailDomain);
                            int a = db.SaveChanges();
                            if (a < 0)
                            {
                                break;
                            }
                        }
                        /*Console.WriteLine(content.ToString());*/ //得到文本内容


                    }
                }
                //MessageBox.Show("执行成功!");
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString() + content + Newcontent); //错误写入日志
                MessageBox.Show("执行失败 ,发生错误，请联系电脑部！内部成员请查看日志文件!");
                throw;
            }

        }
        /// <summary>
        /// 调用运行批处理文件
        /// </summary>
        public static bool ImplementBat(string path)
        {
            try
            {
                //Application.StartupPath //取本目录
                string strDirPath = Path.GetDirectoryName(path);
                string strFilePath = Path.GetFileName(path);
                string targetDir = string.Format(strDirPath);//this is where mybatch.bat lies
                var proc = new Process();
                proc.StartInfo.WorkingDirectory = targetDir;
                proc.StartInfo.FileName = strFilePath;
                //proc.StartInfo.CreateNoWindow = true;
                //proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; //隐藏窗口
                proc.Start();
                proc.WaitForExit();
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.ToString()); //错误写入日志
                MessageBox.Show("执行失败 ,发生错误，请联系电脑部！内部成员请查看日志文件!");
                return false;
            }
        }

    }
}
