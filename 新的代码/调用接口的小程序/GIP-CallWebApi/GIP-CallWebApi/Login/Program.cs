using System;
using System.Windows.Forms;
using System.IO;
using GIP_CallWebApi;
using Login;

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
            if (HttpHelper.ReadTxtContent(topDirPath + "\\计数专用" + ".txt")== 0)  //自动录入
            {
                //接口地址
                var url = "http://api.eos-ts.h3c.com";
                //登录调用接口
                var Account = "eos_scmdip_guest";
                var Password = "tukw0df49ccfizgv";
                var LoginReceive = HttpHelper.HttpPost(1, url + "/user/v1.0/account/token", "{\"account\":\"" + Account + "\",\"password\":\"" + Password + "\"}");
                //获得当前时间戳（17位）
                string time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                //随机五位数
                Random rnd = new Random();
                int number = rnd.Next(10000, 100000);
                //流水号
                var Transaction_id = "ODM-POW012-" + time + number + "";
                //Factory_processes数据集合
                var Factory_processes_List = "[{\"factory_code\":\"POW012\"," +
                            "\"item_code\":\"9801A1G2\"," +
                            "\"sn\":\"GHL75258214800315\"," +
                            "\"order_number\":\"\"," +
                            "\"mo_number\":\"KSYHWORK012149\"," +
                            "\"factory\":\"POW012\"," +
                            "\"line_code\":\"YH1-组装-大功率\"," +
                            "\"class_code\":\"day\"," +
                            "\"is_maintenance\":\"否\"," +
                            "\"section_code\":\"BI\"," +
                            "\"process_result\":\"PASS\"," +
                            "\"start_time\":\"2021-12-13 10:24:52.107\"}]";
                //Factory_reworks数据集合
                var Factory_reworks_List = "[{\"factory_code\":\"POW012\"," +
                           "\"item_code\":\"9801A1G2\"," +
                           "\"item_type\":\"PSR380-24A\"," +
                           "\"sn\":\"GHL75258214800315\"," +
                           "\"fault_date\":\"2021-11-28 01:45:09.173\"," +
                           "\"fault_appearance\":\"功率效率\"," +
                           "\"appearance_link\":\"FATE\"," +
                           "\"fault_type\":\"电性能\"," +
                           "\"fault_case\":\"R167连锡\"," +
                           "\"cause_type\":\"PROCESS(生产线作业)\"," +
                           "\"cause_subcategory\":\"插件/执锡不良\"," +
                           "\"root_cause\":\"PROCESS(生产线作业)\"," +
                            "\"defect_location\":\"R167\"," +
                           "\"repair_date\":\"2021-11-30 21:52:57.197\"}]";
                var JKfactory_processes = HttpHelper.HttpPost(0, url + "/odm-api/v1.0/factory/process", "{\"transaction_id\":\"" + Transaction_id + "\",\"factory_processes\":" + Factory_processes_List + "}");
                var JKfactory_reworks = HttpHelper.HttpPost(0, url + "/odm-api/v1.0/factory/rework", "{\"transaction_id\":\"" + Transaction_id + "\",\"factory_reworks\":" + Factory_reworks_List + "}");
                Receipt Receipt = null;
                if (LoginReceive != "")
                {
                    //Receipt = HttpHelper.DeserializeJsonToObject<Receipt>(JKfactory_processes);
                    Receipt = HttpHelper.DeserializeJsonToObject<Receipt>(JKfactory_reworks);
                }
                //接口返回新增成功
                if (Receipt.code == 1100)
                {
                    //MessageBox.Show();
                    DevExpress.XtraEditors.XtraMessageBox.Show(Receipt.msg);
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("新增失败");
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
                Application.Run(new Details());
            }
        }
    }
}
