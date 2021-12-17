using Microsoft.OData.Edm;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace GIP_CallWebApi
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //查看计数文档
            string appPath = Application.StartupPath;
            DirectoryInfo topDir = Directory.GetParent(appPath);
            string topDirPath = topDir.FullName;//topDirPath即上层目录
            if (HttpHelper.ReadTxtContent(topDirPath + "\\计数专用" + ".txt") > 0)  //自动录入
            {
                //自动录入
                var url1 = "http://api.eos-ts.h3c.com/user/v1.0/account/token";
                var url2 = "http://api.eos-ts.h3c.com/odm-api/v1.0/factory/process";
                var CanShu1 = HttpHelper.HttpPost(1, url1, "{ \"account\":\"eos_scmdip_guest\",\"password\":\"tukw0df49ccfizgv\"}");
                //获得当前时间戳（17位）
                string time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var CanShu2 = HttpHelper.HttpPost(0, url2, "{\"transaction_id\":\"ODM-POW012-" + time + "-00001\",\"factory_processes\":[{\"factory_code\":\"111\"}]} }");
                Receipt Receipt = null;
                if (CanShu1 != null)
                {
                    Receipt = HttpHelper.DeserializeJsonToObject<Receipt>(CanShu2);
                }
                if (Receipt.code == 1100)
                {
                    MessageBox.Show("新增成功");
                }
                else
                {
                    MessageBox.Show("新增失败");
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run();//不运行窗体
            }
            else
            {
                //手动录入显示界面
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new LogonForm());
            }
        }
    }
}
