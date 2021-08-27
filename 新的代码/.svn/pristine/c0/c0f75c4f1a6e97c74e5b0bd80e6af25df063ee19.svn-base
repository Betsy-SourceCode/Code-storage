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
                    //截取新域名（@后面到>）判断是不是等于gipmail.com
                    String str = content.Substring(0, content.IndexOf("from=<")); //先截取掉前面
                    content = content.Substring(str.Length + 1, content.Length);
                    content = content.Substring(0, content.IndexOf("@"));
                    content = content.Substring(0, content.IndexOf(">"));
                }
                /*Console.WriteLine(content.ToString());*/ //得到文本内容


            }
        }
    }
}
