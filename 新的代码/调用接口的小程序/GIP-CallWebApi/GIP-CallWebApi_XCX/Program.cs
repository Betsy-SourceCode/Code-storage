using System;
using System.Windows.Forms;
using System.IO;
using GIP_CallWebApi;
using Login;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

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
            ////查看计数文档
            //string appPath = Application.StartupPath;
            //DirectoryInfo topDir = Directory.GetParent(appPath);
            //string topDirPath = topDir.FullName;//topDirPath即上层目录
            bool flag = true; //根据需求只要自动录入
            //HttpHelper.ReadTxtContent(topDirPath + "\\计数专用" + ".txt") < 0
            if (flag)  //自动录入
            {
                //接口地址
                var url = "https://api.eos.h3c.com";
                //登录调用接口
                var Account = "eos_scmdip_gre";
                var Password = "sswtpvi4se8d7frr";
                if (HttpHelper.HttpPost(1, url + "/user/v1.0/account/token", "{\"account\":\"" + Account + "\",\"password\":\"" + Password + "\"}") != "")
                {
                    #region 多线程
                    //Factory/process接口
                    //ThreadMethod(1, url + "/odm-api/v1.0/factory/process");
                    // MyThread 
                    //Factory_reworks接口
                    //ThreadMethod(2, url + "/odm-api/v1.0/factory/rework");
                    //Thread th = new Thread(new ThreadStart(ThreadMethod(2, url + "/odm-api/v1.0/factory/process"))); //创建线程                
                    //th.Start(); //启动线程

                    //开启第一个线程
                    MyThread mt = new MyThread(1, url + "/odm-api/v1.0/factory/process");
                    ThreadStart threadStart = new ThreadStart(mt.ThreadMethod);
                    Thread thread = new Thread(threadStart);
                    thread.Start();

                    //开启第二个线程
                    MyThread mt1 = new MyThread(2, url + "/odm-api/v1.0/factory/rework");
                    ThreadStart threadStart1 = new ThreadStart(mt1.ThreadMethod);
                    Thread thread1 = new Thread(threadStart1);
                    thread1.Start();
                    #endregion
                }
                else
                {
                    //写入数据
                    var shuju = "调用登录接口失败";
                    MessageBox.Show(shuju);
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
                Application.Run(new LoginView());
            }
        }

        #region 多线程在另一个类里,这里只是备用
        /// <summary>
        /// 调用多线程方法
        /// </summary>
        /// <param name="id">1-factory_processes，InvalidOperationException: 线程间操作无效: 从不是创建控件“”的线程访问它。2-factory_reworks</param>
        //static void ThreadMethod(int id, string url)
        //{
        //    string ERPMO = "";
        //    try
        //    {
        //        //获得当前年月日
        //        //string date = DateTime.Now.ToString("yyyy-MM-dd");
        //        string date = "2021-12-19";
        //        //查当天的所有工单号
        //        foreach (var item in SqlStoredProcedure.GetGDnumberList(date))
        //        {
        //            //通过日期和工单号查出列表集合
        //            ERPMO = item.ERPMO.ToString();
        //            List<DataSet> list = SqlStoredProcedure.ByDateAndERPMOGetListSql(id, date, ERPMO);
        //            var i = 1;
        //            var DataSetList = ""; //接口数据                
        //            string time = DateTime.Now.ToString("yyyyMMddHHmmssfff"); //获得当前时间戳（17位）
        //            //随机五位数
        //            Random rnd = new Random();
        //            int number = rnd.Next(10000, 100000);
        //            //流水号
        //            var Transaction_id = "ODM-" + list.FirstOrDefault().厂商代码 + "-" + time + number + "";

        //            foreach (var dataSet in list)
        //            {

        //                //Factory_processes接口参数
        //                if (id == 1)
        //                {
        //                    DataSetList += "{\"transaction_id\":\"" + Transaction_id +
        //                           "\",\"factory_processes\":[{\"factory_code\":\"" + dataSet.厂商代码 + "\"," +
        //                            "\"item_code\":\"" + dataSet.编码 + "\"," +
        //                            "\"sn\":\"" + dataSet.SN条码 + "\"," +
        //                            "\"order_number\":\"" + dataSet.Po订单号 + "\"," +
        //                            "\"mo_number\":\"" + dataSet.任务令 + "\"," +
        //                            "\"factory\":\"" + dataSet.工厂 + "\"," +
        //                            "\"line_code\":\"" + dataSet.线别 + "\"," +
        //                            "\"class_code\":\"" + dataSet.班次 + "\"," +
        //                            "\"is_maintenance\":\"" + dataSet.是否维修品 + "\"," +
        //                            "\"section_code\":\"" + dataSet.工序名称 + "\"," +
        //                            "\"process_result\":\"" + dataSet.加工结果 + "\"," +
        //                            "\"start_time\":\"" + dataSet.开始时间 + "\"}]";
        //                }
        //                //Factory_reworks接口参数
        //                if (id == 2)
        //                {
        //                    DataSetList += "{\"transaction_id\":\"" + Transaction_id +
        //                                   "\",\"factory_reworks\":[{\"factory_code\":\"" + dataSet.厂商代码 + "\"," +
        //                                    "\"item_code\":\"" + dataSet.编码 + "\"," +
        //                                    "\"item_type\":\"" + dataSet.型号 + "\"," +
        //                                    "\"sn\":\"" + dataSet.SN条码 + "\"," +
        //                                    "\"fault_date\":\"" + dataSet.故障日期 + "\"," +
        //                                    "\"fault_appearance\":\"" + dataSet.不良现象 + "\"," +
        //                                    "\"appearance_link\":\"" + dataSet.出现环节 + "\"," +
        //                                    "\"fault_type\":\"" + dataSet.不良分类 + "\"," +
        //                                    "\"fault_case\":\"" + dataSet.不良原因分析 + "\"," +
        //                                    "\"cause_type\":\"" + dataSet.原因分类 + "\"," +
        //                                    "\"cause_subcategory\":\"" + dataSet.原因小类 + "\"," +
        //                                    "\"root_cause\":\"" + dataSet.根因 + "\"," +
        //                                    "\"defect_location\":\"" + dataSet.不良器件位置 + "\"," +
        //                                    "\"repair_date\":\"" + dataSet.修理日期 + "\"}]";
        //                }
        //                DataSetList += "}";
        //                DataSetList += ",";
        //                if (i >= 200 || i >= list.Count)
        //                {
        //                    DataSetList = DataSetList.TrimEnd(',');
        //                    //去掉空格
        //                    //DataSetList = DataSetList.Trim();
        //                    //接口返回结果
        //                    var JKResult = HttpHelper.HttpPost(0, url, DataSetList);
        //                    Receipt receipt = HttpHelper.DeserializeJsonToObject<Receipt>(JKResult);
        //                    ////接口返回新增成功
        //                    if (receipt.code != 1100)
        //                    {
        //                        //接口调用失败，写入数据表里:记录时间和工单号
        //                        var shuju = "错误日期：" + date + "，工单号：" + ERPMO;
        //                        DevExpress.XtraEditors.XtraMessageBox.Show(receipt.msg);
        //                    }
        //                    else
        //                    {
        //                        if (id == 1)
        //                        {
        //                            MessageBox.Show("aaaaaaaaaa");
        //                        }
        //                        else
        //                        {
        //                            //接口调用成功
        //                            DevExpress.XtraEditors.XtraMessageBox.Show(receipt.msg);
        //                        }

        //                    }
        //                    i = 1;
        //                }
        //                else
        //                {
        //                    i++;
        //                }

        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //数据新增失败，写入数据表里:记录时间和工单号
        //        DateTime dt = DateTime.Now;
        //        //SqlStoredProcedure.AddLog_ErrorData(receipt.code, ERPMO, receipt.message + receipt.msg + receipt.data + receipt.result, dt);
        //        throw;
        //    }
        //}
        #endregion
    }
}
