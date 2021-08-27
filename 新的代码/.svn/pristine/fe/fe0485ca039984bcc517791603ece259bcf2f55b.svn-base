using System;
using System.Collections.Generic;
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
            ReadTxtContent("E:\\maillog-20210804");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Index());
            Application.Run();//不运行窗体
        }
        /// <summary>
        /// 读取指定文件夹的日志档内容
        /// </summary>
        /// <param name="Path">文件地址</param>
        public static void ReadTxtContent(string Path)
        {
            StreamReader sr = new StreamReader(Path, Encoding.Default);
            string content;
            while ((content = sr.ReadLine()) != null)  //一行行读取
            {
                //在文本里寻找邮件域名
                if (content.IndexOf("from=<") >= 0)
                {
                    int length = content.Length;
                    //截取新域名(@后面到>)
                    //string frontstr = content.Substring(0, content.IndexOf("from=<"));
                    int startIndex = content.IndexOf("from=<");//开始位置
                    content = content.Substring(startIndex, length - startIndex);//从开始位置截取一个新的字符串
                    int qianIndex = content.IndexOf("@") + 1;
                    int houIndex = content.IndexOf(">");//结束位置
                    content = content.Substring(qianIndex, houIndex - qianIndex); //取域名
                    //判断是不是等于gipmail.com同时查询域名是否在表里存在,不存在就添加到表里
                    NewEmailDomain2GIP NewEmailDomain = db.NewEmailDomain2GIP.FirstOrDefault((NewEmailDomain2GIP e) => e.DomainName == content);
                    if (content != "gipmail.com" && NewEmailDomain == null)
                    {


                    }
                }
                /*Console.WriteLine(content.ToString());*/ //得到文本内容


            }
        }
    }
}
