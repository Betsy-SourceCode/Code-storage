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
        /// <summary>
        /// 1-factory_processes，2-factory_reworks
        /// </summary>
        public int typeid { set; get; }//参数
        public string url { set; get; }//参数
        public int Result { set; get; }//返回值
        public int i = 0; //循环专用变量
        public MyThread(int typeid, string url)
        {
            this.typeid = typeid;
            this.url = url;
        }
        /// <summary>
        /// 调用多线程方法
        /// </summary>
        /// <param name="typeid">1-factory_processes，2-factory_reworks</param>
        public void ThreadMethod()
        {
            string ERPMO = "";
            //获得当前年月日
            DateTime dt = DateTime.Now;
            DateTime date = DateTime.Now;  //默认为当天
            //string date = dt.ToString("yyyy-MM-dd");
            /*DateTime date = dt.AddDays(-1.0); *///默认为当前时间的前两天

            Receipt obj = new Receipt();
            Receipt receipt = new Receipt();
            var items = new List<Receipt>(); //设置默认值
            try
            {
                if (typeid == 1)
                {
                    items = SqlStoredProcedure.GetGDnumberList("Station", date.ToString()); //查当天的所有工单号
                }
                if (typeid == 2)
                {
                    items = SqlStoredProcedure.GetGDnumberList("Repair", date.ToString()); //查当天的所有工单号
                }
                //判断为空return掉，不然循环会报错
                if (items == null || items.Count == 0)
                {
                    return;
                }
                int count = 0;
                foreach (var item in items)
                {
                    //通过日期和工单号查出列表集合
                    ERPMO = item.ERPMO.ToString();
                    List<DataSet> list = SqlStoredProcedure.ByDateAndERPMOGetListSql(this.typeid, date.ToString(), ERPMO);
                    count = list.Count;
                    var DataSetList = ""; //接口数据
                    var index = 0;//用于记录第几个200条
                    string time = DateTime.Now.ToString("yyyyMMddHHmmssfff");  //获得当前时间戳（17位）
                    //随机五位数
                    Random rnd = new Random();
                    int number = rnd.Next(10000, 100000);
                    //流水号
                    var Transaction_id = "ODM-" + list.FirstOrDefault().厂商代码 + "-" + time + number + "";
                    if (typeid == 1) //Factory_processes接口
                    {
                        DataSetList = "{\"transaction_id\":\"" + Transaction_id +
                                   "\",\"factory_processes\":["; //数据集合
                    }
                    if (typeid == 2) //Factory_reworks接口
                    {
                        DataSetList = "{\"transaction_id\":\"" + Transaction_id +
                                   "\",\"factory_reworks\":["; //数据集合
                    }
                    foreach (var dataSet in list)
                    {

                        //Factory_processes接口参数
                        if (typeid == 1)
                        {
                            DataSetList += "{\"factory_code\":\"" + dataSet.厂商代码.Trim() + "\",\"item_code\":\"" + dataSet.编码.Trim() + "\",\"sn\":\"" + dataSet.SN条码.Trim() + "\",\"order_number\":\"" + dataSet.Po订单号 + "\",\"mo_number\":\"" + dataSet.任务令.Trim() + "\",\"factory\":\"" + dataSet.工厂.Trim() + "\",\"line_code\":\"" + dataSet.线别.Trim() + "\",\"class_code\":\"" + dataSet.班次.Trim() + "\",\"is_maintenance\":\"" + dataSet.是否维修品.Trim() + "\",\"section_code\":\"" + dataSet.工序名称.Trim() + "\",\"process_result\":\"" + dataSet.加工结果.Trim() + "\",\"start_time\":\"" + dataSet.开始时间.Trim() + "\"},";
                        }
                        //Factory_reworks接口参数
                        if (typeid == 2)
                        {
                            DataSetList += "{\"factory_code\":\"" + dataSet.厂商代码.Trim() + "\",\"item_code\":\"" + dataSet.编码.Trim() + "\"," +
                                            "\"item_type\":\"" + dataSet.型号.Trim() + "\",\"sn\":\"" + dataSet.SN条码.Trim() + "\",\"fault_date\":\"" + dataSet.故障日期.Trim() + "\",\"fault_appearance\":\"" + dataSet.不良现象.Trim() + "\",\"appearance_link\":\"" + dataSet.出现环节.Trim() + "\",\"fault_type\":\"" + dataSet.不良分类.Trim() + "\",\"fault_cause\":\"" + dataSet.不良原因分析.Trim() + "\",\"cause_type\":\"" + dataSet.原因分类.Trim() + "\",\"cause_subcategory\":\"" + dataSet.原因小类.Trim() + "\",\"root_cause\":\"" + dataSet.根因.Trim() + "\",\"defect_location\":\"" + dataSet.不良器件位置.Trim() + "\",\"repair_date\":\"" + dataSet.修理日期.Trim() + "\"},";
                        }
                        if (list.Count < 200)
                        {
                            if (i >= list.Count - 1)
                            {
                                DataSetList += "]}";
                                DataSetList = DataSetList.Remove(DataSetList.LastIndexOf(','), 1);

                                //接口返回结果
                                string JKResult = HttpHelper.HttpPost(0, url, DataSetList);
                                //JsonToObject data1 = JsonConvert.DeserializeObject<JsonToObject>(JKResult);
                                // string aa = data1.code.ToString();
                                receipt = HttpHelper.DeserializeJsonToObject<Receipt>(JKResult);
                                if (receipt.code != 1100)
                                {
                                    ////数据新增失败，写入数据表里:记录时间和工单号
                                    //SqlStoredProcedure.AddLog_ErrorData(receipt.code, ERPMO, receipt.message + receipt.msg + receipt.data + receipt.result, dt);
                                    obj.data = receipt.msg;
                                    obj.id = 0;
                                    break; //跳过当前工单号直接到下一个工单号

                                }
                                //接口返回新增成功
                                else
                                {
                                    i = 0;
                                    if (typeid == 1) //Factory_processes接口
                                    {
                                        DataSetList = "{\"transaction_id\":\"" + Transaction_id +
                                                   "\",\"factory_processes\":["; //数据集合
                                    }
                                    if (typeid == 2) //Factory_reworks接口
                                    {
                                        DataSetList = "{\"transaction_id\":\"" + Transaction_id +
                                                   "\",\"factory_reworks\":["; //数据集合
                                    }
                                    obj.data = receipt.msg;
                                    obj.id = 1;
                                }
                            }
                            else
                            {
                                i++;
                            }

                        }
                        else
                        {
                            if (i == 199)
                            {
                                index++;//循环几个200
                                DataSetList += "]}";
                                DataSetList = DataSetList.Remove(DataSetList.LastIndexOf(','), 1);
                                //接口返回结果
                                var JKResult = HttpHelper.HttpPost(0, url, DataSetList);
                                receipt = HttpHelper.DeserializeJsonToObject<Receipt>(JKResult);
                                if (receipt.code != 1100)
                                {
                                    ////数据新增失败，写入数据表里:记录时间和工单号
                                    //SqlStoredProcedure.AddLog_ErrorData(receipt.code, ERPMO, receipt.message + receipt.msg + receipt.data + receipt.result, dt);
                                    obj.data = receipt.msg;
                                    obj.id = 0;
                                    break; //跳过当前工单号直接到下一个工单号

                                }
                                //接口返回新增成功
                                else
                                {
                                    i = 0;
                                    obj.data = receipt.msg;
                                    obj.id = 1;
                                    if (typeid == 1) //Factory_processes接口
                                    {
                                        DataSetList = "{\"transaction_id\":\"" + Transaction_id +
                                                   "\",\"factory_processes\":["; //数据集合
                                    }
                                    if (typeid == 2) //Factory_reworks接口
                                    {
                                        DataSetList = "{\"transaction_id\":\"" + Transaction_id +
                                                   "\",\"factory_reworks\":["; //数据集合
                                    }
                                    //测试：接口调用成功
                                    //DevExpress.XtraEditors.XtraMessageBox.Show(receipt.msg);
                                }
                            }
                            else
                            {

                                if (list.Count - (index * 200) < 200)
                                {
                                    int num = list.Count - (index * 200) - 1;
                                    if (i == list.Count - (index * 200) - 1)
                                    {
                                        DataSetList += "]}";
                                        DataSetList = DataSetList.Remove(DataSetList.LastIndexOf(','), 1);
                                        //接口返回结果
                                        var JKResult = HttpHelper.HttpPost(0, url, DataSetList);
                                        receipt = HttpHelper.DeserializeJsonToObject<Receipt>(JKResult);
                                        if (receipt.code != 1100)
                                        {
                                            ////数据新增失败，写入数据表里:记录时间和工单号
                                            //SqlStoredProcedure.AddLog_ErrorData(receipt.code, ERPMO, receipt.message + receipt.msg + receipt.data + receipt.result, dt);
                                            obj.data = receipt.msg;
                                            obj.id = 0;
                                            break; //跳过当前工单号直接到下一个工单号

                                        }
                                        //接口返回新增成功
                                        else
                                        {
                                            i = 0;
                                            obj.data = receipt.msg;
                                            obj.id = 1;
                                            if (typeid == 1) //Factory_processes接口
                                            {
                                                DataSetList = "{\"transaction_id\":\"" + Transaction_id +
                                                           "\",\"factory_processes\":["; //数据集合
                                            }
                                            if (typeid == 2) //Factory_reworks接口
                                            {
                                                DataSetList = "{\"transaction_id\":\"" + Transaction_id +
                                                           "\",\"factory_reworks\":["; //数据集合
                                            }
                                        }
                                    }
                                    else
                                    {
                                        i++;
                                    }
                                }
                                else
                                {
                                    i++;
                                }
                            }
                        }

                    }
                    //写入数据表里:记录时间和工单号
                    if (typeid == 1) //Factory_processes接口
                    {
                        SqlStoredProcedure.AddLog_ErrorData(receipt.code, ERPMO, receipt.message + receipt.msg + receipt.data + receipt.result, date, count, "过站记录");
                    }
                    if (typeid == 2) //Factory_reworks接口
                    {
                        SqlStoredProcedure.AddLog_ErrorData(receipt.code, ERPMO, receipt.message + receipt.msg + receipt.data + receipt.result, date, count, "维修记录");
                    }
                }

            }
            catch (Exception ex)
            {
                //数据新增失败，写入数据表里:记录时间和工单号
                if (typeid == 1) //Factory_processes接口
                {
                    SqlStoredProcedure.AddLog_ErrorData(0, ERPMO, ex.Message, dt, 0, "过站记录");
                }
                if (typeid == 2) //Factory_reworks接口
                {
                    SqlStoredProcedure.AddLog_ErrorData(0, ERPMO, ex.Message, dt, 0, "维修记录");
                }
                obj.data = ex.Message;
                throw;
            }
        }
    }
}
