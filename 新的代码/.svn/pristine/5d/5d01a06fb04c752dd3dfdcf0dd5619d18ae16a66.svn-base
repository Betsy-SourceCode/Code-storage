using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace KYH_JigFixture.Models.PublicSqlMethods
{
    public class LogHelper
    {
        private const string _path = @"C:\ATMLog";
        private static string Path
        {
            get { return _path; }
        }



        public static void Write(string content)
        {
            Action<string> write = (text) => { WriteLogs(text); };
            write(content);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content">错误信息</param>
        //public static void WriteLogs(string content)
        //{
        //    try
        //    {
        //        //如果Log文件夹不存在
        //        if (Directory.Exists(Path) == false)
        //        {
        //            //创建Log文件夹
        //            Directory.CreateDirectory(Path);
        //        }

        //        DateTime dt = DateTime.Now;
        //        // 判断日志txt是否存在，不存在则创建再写入，否则打开文本写入
        //        string txtPath = Path + "\\" + "Log" + dt.ToString("yyyy-MM-dd") + ".txt";
        //        if (File.Exists(txtPath) == false)
        //        {
        //            //创建日志，获取写入权限
        //            using (FileStream fs1 = new FileStream(txtPath, FileMode.Create, FileAccess.Write, FileShare.Write))
        //            {
        //                using (StreamWriter sw = new StreamWriter(fs1))
        //                {
        //                    //写入
        //                    sw.WriteLine(dt.ToString("yyyy-MM-dd HH:mm:ss" + "：" + "\t\n"));
        //                    sw.WriteLine(content + "\t\n");
        //                    sw.WriteLine("--------------------------------------------------------------------------" + "\t\n");
        //                }
        //            }
        //        }
        //        else
        //        {
        //            //打开日志，获取写入权限
        //            using (FileStream fs = new FileStream(txtPath, FileMode.Append, FileAccess.Write, FileShare.Write))
        //            {
        //                using (StreamWriter sw = new StreamWriter(fs))
        //                {
        //                    //写入
        //                    sw.WriteLine(dt.ToString("yyyy-MM-dd HH:mm:ss" + "：" + "\t\n"));
        //                    sw.WriteLine(content + "\t\n");
        //                    sw.WriteLine("--------------------------------------------------------------------------" + "\t\n");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return;
        //    }
        //}



        //默写：创建日志
        public static void WriteLogs(string content)
        {
            try
            {
                if (Directory.Exists(Path)==false)
                {
                    Directory.CreateDirectory(Path);
                }
                DateTime dt = DateTime.Now;
                string txtpath = Path + "\\" + "Log" + dt.ToString("yyyy-MM-dd") + ".txt";
                if (File.Exists(txtpath)==false)
                {
                    using (FileStream fs=new FileStream(txtpath,FileMode.Create,FileAccess.Write,FileShare.Write))
                    {
                        using (StreamWriter sw=new StreamWriter(fs))
                        {
                            sw.WriteLine(dt.ToString("yyyy-MM-dd HH:mm:ss"+":"+"\n"));
                            sw.WriteLine(content+"\n");
                            sw.WriteLine("------------------------------------------------");
                        }
                    }
                }
                else
                {
                    using (FileStream fs = new FileStream(txtpath, FileMode.Append, FileAccess.Write, FileShare.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            sw.WriteLine(dt.ToString("yyyy-MM-dd        HH:mm:ss" + ":" + "\n"));
                            sw.WriteLine(content + "\n");
                            sw.WriteLine("------------------------------------------------");
                        }
                    }
                }
            }
            catch (Exception)
            {
                
                return ;
            }
        }
    }
}