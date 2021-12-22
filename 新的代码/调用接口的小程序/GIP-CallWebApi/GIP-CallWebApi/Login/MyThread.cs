using GIP_CallWebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Login
{
    public class MyThread
    {

        public int id { set; get; }//参数
        public string url { set; get; }//参数
        public int Result { set; get; }//返回值
                                       //构造函数
        public MyThread(int id, string url)
        {
            this.id = id;
            this.url = url;
        }

        /// <summary>
        /// 调用多线程方法
        /// </summary>
        /// <param name="id">1-factory_processes，2-factory_reworks</param>
        public void ThreadMethod()
        {
            string ERPMO = "";
            //获得当前年月日
            DateTime dt = DateTime.Now;
            //string date = dt.ToString("yyyy-MM-dd");
            string date = "2021-12-19"; //测试
            try
            {
                var items = SqlStoredProcedure.GetGDnumberList(date); //查当天的所有工单号
                //判断为空return掉，不然循环会报错
                if (items == null)
                {
                    return;
                }
                foreach (var item in items)
                {
                    //通过日期和工单号查出列表集合
                    ERPMO = item.ERPMO.ToString();
                    List<DataSet> list = SqlStoredProcedure.ByDateAndERPMOGetListSql(this.id, date, ERPMO);
                    var i = 1;
                    var DataSetList = ""; //接口数据
                    foreach (var dataSet in list)
                    {
                        //获得当前时间戳（17位）
                        string time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        //随机五位数
                        Random rnd = new Random();
                        int number = rnd.Next(10000, 100000);
                        //流水号
                        var Transaction_id = "ODM-" + dataSet.厂商代码 + "-" + time + number + "";
                        //Factory_processes接口参数
                        if (this.id == 1)
                        {
                            DataSetList += "{\"transaction_id\":\"" + Transaction_id +
                                   "\",\"factory_processes\":[{\"factory_code\":\"" + dataSet.厂商代码 + "\"," +
                                    "\"item_code\":\"" + dataSet.编码 + "\"," +
                                    "\"sn\":\"" + dataSet.SN条码 + "\"," +
                                    "\"order_number\":\"" + dataSet.Po订单号 + "\"," +
                                    "\"mo_number\":\"" + dataSet.任务令 + "\"," +
                                    "\"factory\":\"" + dataSet.工厂 + "\"," +
                                    "\"line_code\":\"" + dataSet.线别 + "\"," +
                                    "\"class_code\":\"" + dataSet.班次 + "\"," +
                                    "\"is_maintenance\":\"" + dataSet.是否维修品 + "\"," +
                                    "\"section_code\":\"" + dataSet.工序名称 + "\"," +
                                    "\"process_result\":\"" + dataSet.加工结果 + "\"," +
                                    "\"start_time\":\"" + dataSet.开始时间 + "\"}]";
                        }
                        //Factory_reworks接口参数
                        if (this.id == 2)
                        {
                            DataSetList += "{\"transaction_id\":\"" + Transaction_id +
                                           "\",\"factory_reworks\":[{\"factory_code\":\"" + dataSet.厂商代码 + "\"," +
                                            "\"item_code\":\"" + dataSet.编码 + "\"," +
                                            "\"item_type\":\"" + dataSet.型号 + "\"," +
                                            "\"sn\":\"" + dataSet.SN条码 + "\"," +
                                            "\"fault_date\":\"" + dataSet.故障日期 + "\"," +
                                            "\"fault_appearance\":\"" + dataSet.不良现象 + "\"," +
                                            "\"appearance_link\":\"" + dataSet.出现环节 + "\"," +
                                            "\"fault_type\":\"" + dataSet.不良分类 + "\"," +
                                            "\"fault_case\":\"" + dataSet.不良原因分析 + "\"," +
                                            "\"cause_type\":\"" + dataSet.原因分类 + "\"," +
                                            "\"cause_subcategory\":\"" + dataSet.原因小类 + "\"," +
                                            "\"root_cause\":\"" + dataSet.根因 + "\"," +
                                            "\"defect_location\":\"" + dataSet.不良器件位置 + "\"," +
                                            "\"repair_date\":\"" + dataSet.修理日期 + "\"}]";
                        }
                        DataSetList += "}";
                        DataSetList += ",";
                        if (i >= 200 || i >= list.Count)
                        {
                            DataSetList = DataSetList.TrimEnd(',');
                            //接口返回结果
                            var JKResult = HttpHelper.HttpPost(0, this.url, DataSetList);
                            Receipt receipt = HttpHelper.DeserializeJsonToObject<Receipt>(JKResult);
                            ////接口返回新增成功
                            if (receipt.code != 1100)
                            {
                                var shuju = "错误日期：" + date + "，工单号：" + ERPMO;
                                //数据新增失败，写入数据表里:记录时间和工单号
                                SqlStoredProcedure.AddLog_ErrorData(dt, ERPMO, receipt.msg);
                            }
                            else
                            {
                                //接口调用成功
                                DevExpress.XtraEditors.XtraMessageBox.Show(receipt.msg);
                            }
                            i = 1;
                        }
                        else
                        {
                            i++;
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                //数据新增失败，写入数据表里:记录时间和工单号
                SqlStoredProcedure.AddLog_ErrorData(dt, ERPMO, ex.Message);
                throw;
            }
        }
    }
}
