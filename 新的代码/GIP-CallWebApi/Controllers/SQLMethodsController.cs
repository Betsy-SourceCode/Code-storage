using GIP_CallWebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GIP_CallWebApi.Controllers
{
    public class SQLMethodsController : Controller
    {
        gipwip_R1Entities gipwip_R1 = new gipwip_R1Entities();
        List<Log_ErrorData> listSource = new List<Log_ErrorData>();
        public int i = 0;
        /// <summary>
        /// 错误数据表格的查询
        /// </summary>
        /// <param name="startDate">入库开始日期</param>
        /// <param name="endDate">入库结束日期</param>
        /// <param name="ERPMO">工单号</param>
        /// <returns></returns>
        public string log_ErrorDatas(string startDate, string endDate, string ERPMO,string DataType)
        {
            try
            {
                //错误数据表倒序显示
                string sql = "select * from Log_ErrorData where 1=1";
                if (startDate != "" && endDate != "")
                {
                    sql += " and CONVERT(NVARCHAR(10),Error_Date,120)  between '" + startDate + "' and  '" + endDate + "'";
                }
                if (ERPMO != "")
                {
                    sql += " and Error_Data like '%" + ERPMO + "%'";
                }
                if (DataType=="成功")
                {
                    sql += " and Error_Reason like '%正确%'";
                }
                if (DataType == "失败")
                {
                    sql += " and Error_Reason like '%错误%'";
                }
                sql += " order by Error_Date desc";
                var data = gipwip_R1.Database.SqlQuery<Log_ErrorData>(sql, new object[0]).ToList<Log_ErrorData>();
                int i = 1;
                foreach (var item in data)
                {
                    
                    Log_ErrorData model = new Log_ErrorData()
                    {

                        Error_Number = i,
                        Error_Data = item.Error_Data,
                        Error_Reason = item.Error_Reason,
                        Error_Date = item.Error_Date
                    };
                    listSource.Add(model);
                    i++;
                }
                var obj = new
                {
                    code = 0,
                    msg = "",
                    count = listSource.Count,
                    data = listSource
                };
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }
        }


        [HttpPost]
        /// <summary>
        /// 接口页签-调用接口
        /// </summary>
        /// <param name="typeid">1-factory_processes，2-factory_reworks</param>
        /// <param name="url"></param>
        public string UseInterface(int typeid, string url, string ERPMO, string date,string token)
        {
            try
            {
                //var token = HttpHelper.HttpPost(1, "https://api.eos.h3c.com/user/v1.0/account/token", "{ \"account\":\"eos_scmdip_gre\",\"password\":\"sswtpvi4se8d7frr\"}", "");
                DateTime dt = DateTime.Parse(date);
                var arr = ERPMO.Split(',');
                Receipt reult=new Receipt();
                foreach (var item in arr)
                {
                    //通过日期和工单号查出列表集合
                    reult= UseInterfaceMethod(typeid, url, item.ToString(), dt, token);//返回值 0-失败，1-成功
                }
                return JsonConvert.SerializeObject(reult);
               
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex.Message);
                throw;
            }
            #region 数据测试
            //流水号
            //var Transaction_id = "ODM-" + "POW012" + "-" + time + number + "";
            //Factory_reworks数据集合
            //var Factory_reworks_List = "[{\"factory_code\":\"POW012\"," +
            //           "\"item_code\":\"9801A1G2\"," +
            //           "\"item_type\":\"PSR380-24A\"," +
            //           "\"sn\":\"GHL75258214800315\"," +
            //           "\"fault_date\":\"2021-11-28 01:45:09.173\"," +
            //           "\"fault_appearance\":\"功率效率\"," +
            //           "\"appearance_link\":\"FATE\"," +
            //           "\"fault_type\":\"电性能\"," +
            //           "\"fault_case\":\"R167连锡\"," +
            //           "\"cause_type\":\"PROCESS(生产线作业)\"," +
            //           "\"cause_subcategory\":\"插件/执锡不良\"," +
            //           "\"root_cause\":\"PROCESS(生产线作业)\"," +
            //            "\"defect_location\":\"R167\"," +
            //           "\"repair_date\":\"2021-11-30 21:52:57.197\"}]";
            //var JKfactory_reworks = HttpHelper.HttpPost(2, url + "/odm-api/v1.0/factory/rework", "{\"transaction_id\":\"" + Transaction_id + "\",\"factory_reworks\":" + Factory_reworks_List + "}");
            #endregion
        }

        /// <summary>
        /// 接口页签-获得接口参数调用接口
        /// </summary>
        /// <param name="typeid">1-factory_processes，2-factory_reworks</param>
        /// <param name="url"></param>
        /// <param name="ERPMO"></param>
        private Receipt UseInterfaceMethod( int typeid, string url, string ERPMO, DateTime dt, string token) //返回值 2-工单号没数据 0-失败，1-成功
        {
            Receipt obj = new Receipt();
            try
            {
                
                string time = dt.ToString("yyyyMMddHHmmssfff"); //时间控件的值格式化为时间戳格式（17位）
                //string flag = "";  //默认成功
                
                //随机五位数
                Random rnd = new Random();
                List<DataSet> list = SqlStoredProcedure.ByDateAndERPMOGetListSql(typeid, dt.ToString("yyyy-MM-dd"), ERPMO); //日期年月日 
                if (list==null||list.Count==0)
                {
                    obj.id = 2;
                    return obj;
                }
                var index = 0;//用于记录第几个200条
                int number = rnd.Next(10000, 100000);
                var DataSetList = "";
                //流水号
                var Transaction_id = "ODM-" + list.FirstOrDefault().厂商代码 + "-" + time + number + "";
                if (typeid==1)
                {
                    DataSetList = "{\"transaction_id\":\"" + Transaction_id +
                               "\",\"factory_processes\":["; //数据集合
                }
                if (typeid==2)
                {
                    DataSetList = "{\"transaction_id\":\"" + Transaction_id +
                               "\",\"factory_reworks\":["; //数据集合
                }
                Receipt receipt = new Receipt();
                foreach (var dataSet in list)
                {
                    
                    //Factory_processes接口参数
                    if (typeid == 1)
                    {
                        DataSetList += "{\"factory_code\":\"" + dataSet.厂商代码.Trim() + "\",\"item_code\":\"" + dataSet.编码.Trim() + "\",\"sn\":\"" + dataSet.SN条码.Trim() + "\",\"order_number\":\"" + dataSet.Po订单号+ "\",\"mo_number\":\"" + dataSet.任务令.Trim() + "\",\"factory\":\"" + dataSet.工厂.Trim() + "\",\"line_code\":\"" + dataSet.线别.Trim() + "\",\"class_code\":\"" + dataSet.班次.Trim() + "\",\"is_maintenance\":\"" + dataSet.是否维修品.Trim() + "\",\"section_code\":\"" + dataSet.工序名称.Trim() + "\",\"process_result\":\"" + dataSet.加工结果.Trim() + "\",\"start_time\":\"" + dataSet.开始时间.Trim() + "\"},";
                    }
                    //Factory_reworks接口参数
                    if (typeid == 2)
                    {
                        DataSetList += "{\"factory_code\":\"" + dataSet.厂商代码.Trim() + "\",\"item_code\":\"" + dataSet.编码.Trim() + "\",\"item_type\":\"" + dataSet.型号.Trim() + "\",\"sn\":\"" + dataSet.SN条码.Trim() + "\",\"fault_date\":\"" + dataSet.故障日期.Trim() + "\",\"fault_appearance\":\"" + dataSet.不良现象.Trim() + "\",\"appearance_link\":\"" + dataSet.出现环节.Trim() + "\",\"fault_type\":\"" + dataSet.不良分类.Trim() + "\",\"fault_cause\":\"" + dataSet.不良原因分析.Replace('\n',' ').Replace('\r',' ').Trim() + "\",\"cause_type\":\"" + dataSet.原因分类.Trim() + "\",\"cause_subcategory\":\"" + dataSet.原因小类.Trim() + "\",\"root_cause\":\"" + dataSet.根因.Trim() + "\",\"defect_location\":\"" + dataSet.不良器件位置.Trim() + "\",\"repair_date\":\"" + dataSet.修理日期.Trim() + "\"},";
                    }

                    
                    if (list.Count < 200)
                    {

                        if (i >= list.Count - 1)
                        {
                            DataSetList += "]}";
                            DataSetList = DataSetList.Remove(DataSetList.LastIndexOf(','), 1);

                            //接口返回结果
                            string JKResult = HttpHelper.HttpPost(2, url, DataSetList, token);
                            //JsonToObject data1 = JsonConvert.DeserializeObject<JsonToObject>(JKResult);
                           // string aa = data1.code.ToString();
                             receipt = HttpHelper.DeserializeJsonToObject<Receipt>(JKResult);
                            ////接口返回新增成功
                            if (receipt.code != 1100)
                            {
                                obj.data = receipt.msg;
                                obj.id = 0;
                                break; //跳过当前工单号直接到下一个工单号
                                ////数据新增失败，写入数据表里:记录时间和工单号
                                //SqlStoredProcedure.AddLog_ErrorData(receipt.code, ERPMO, receipt.message + receipt.msg + receipt.data + receipt.result, dt);
                            }
                            else
                            {
                                i = 0;
                                if (typeid == 1)
                                {
                                    DataSetList = "{\"transaction_id\":\"" + Transaction_id +
                                               "\",\"factory_processes\":["; //数据集合
                                }
                                if (typeid == 2)
                                {
                                    DataSetList = "{\"transaction_id\":\"" + Transaction_id +
                                               "\",\"factory_reworks\":["; //数据集合
                                }
                                obj.data = receipt.msg;
                                ////数据新增失败，写入数据表里:记录时间和工单号
                                //SqlStoredProcedure.AddLog_ErrorData(receipt.code, ERPMO, receipt.message + receipt.msg + receipt.data + receipt.result, dt);
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
                            var JKResult = HttpHelper.HttpPost(2, url, DataSetList, token);
                             receipt = HttpHelper.DeserializeJsonToObject<Receipt>(JKResult);
                            ////接口返回新增成功
                            if (receipt.code != 1100)
                            {
                                ////数据新增失败，写入数据表里:记录时间和工单号
                                //SqlStoredProcedure.AddLog_ErrorData(receipt.code, ERPMO, receipt.message + receipt.msg + receipt.data + receipt.result, dt);
                                obj.data = receipt.msg;
                                obj.id = 0;
                                break; //跳过当前工单号直接到下一个工单号

                            }
                            else
                            {
                                i = 0;
                                obj.data = receipt.msg;
                                obj.id = 1;
                                // flag = receipt.data;
                                ////数据新增失败，写入数据表里:记录时间和工单号
                                //SqlStoredProcedure.AddLog_ErrorData(receipt.code, ERPMO, receipt.message + receipt.msg + receipt.data + receipt.result, dt);
                                if (typeid == 1)
                                {
                                    DataSetList = "{\"transaction_id\":\"" + Transaction_id +
                                               "\",\"factory_processes\":["; //数据集合
                                }
                                if (typeid == 2)
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
                           
                            if (list.Count-(index*200) < 200)
                            {
                                int num = list.Count - (index * 200) - 1;
                                if (i== list.Count - (index * 200)-1)
                                {
                                    DataSetList += "]}";
                                    DataSetList = DataSetList.Remove(DataSetList.LastIndexOf(','), 1);
                                    //接口返回结果
                                    var JKResult = HttpHelper.HttpPost(2, url, DataSetList, token);
                                     receipt = HttpHelper.DeserializeJsonToObject<Receipt>(JKResult);
                                    ////接口返回新增成功
                                    if (receipt.code != 1100)
                                    {
                                        ////数据新增失败，写入数据表里:记录时间和工单号
                                        //SqlStoredProcedure.AddLog_ErrorData(receipt.code, ERPMO, receipt.message + receipt.msg + receipt.data +       receipt.result, dt);
                                        obj.data = receipt.msg;
                                        obj.id = 0;
                                        break; //跳过当前工单号的当前数据直接到下一个数据

                                    }
                                    else
                                    {
                                        i = 0;
                                        obj.data = receipt.msg;
                                        obj.id = 1;
                                        if (typeid == 1)
                                        {
                                            DataSetList = "{\"transaction_id\":\"" + Transaction_id +
                                                       "\",\"factory_processes\":["; //数据集合
                                        }
                                        if (typeid == 2)
                                        {
                                            DataSetList = "{\"transaction_id\":\"" + Transaction_id +
                                                       "\",\"factory_reworks\":["; //数据集合
                                        }
                                        ////数据新增失败，写入数据表里:记录时间和工单号
                                        //SqlStoredProcedure.AddLog_ErrorData(receipt.code, ERPMO, receipt.message + receipt.msg + receipt.data + receipt.result, dt);
                                        //测试：接口调用成功
                                        //DevExpress.XtraEditors.XtraMessageBox.Show(receipt.msg);
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
                //数据新增失败，写入数据表里:记录时间和工单号
                SqlStoredProcedure.AddLog_ErrorData(receipt.code, ERPMO, receipt.message + receipt.msg + receipt.data + receipt.result, dt,list.Count,typeid);
                return obj;
            }
            catch (Exception ex)
            {
                //数据新增失败，写入数据表里:记录时间和工单号
                SqlStoredProcedure.AddLog_ErrorData(0,ERPMO, ex.Message, dt,0,0);
                obj.data = ex.Message;
                return obj;
                throw;
            }
        }
    }
}