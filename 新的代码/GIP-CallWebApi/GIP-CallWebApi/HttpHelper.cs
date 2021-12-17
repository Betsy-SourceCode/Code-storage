using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace GIP_CallWebApi
{
    public static class HttpHelper
    {
        public static string token;
        /// <summary>
        /// 调用WEBAPI方法
        /// </summary>
        ///<param name="id">1-登录，0-其他接口</param>
        /// <param name="url">地址</param>
        /// <param name="body">参数</param>
        /// <returns></returns>
        public static string HttpPost(int id, string url, string body, string method = "POST")
        {
            try
            {
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method;
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.ContentType = "application/json";
                if (id == 0 && token != null)  //登录进入之后调用接口加token
                {
                    request.Headers.Add("Authorization", "Bearer " + token);
                }
                byte[] buffer = encoding.GetBytes(body);
                request.ContentLength = buffer.Length;
                if (method.Equals("POST"))
                {
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    if (id == 1)
                    {
                        //将json字符串转换成泛型集合
                        Logon Logon = DeserializeJsonToObject<Logon>(reader.ReadToEnd());
                        token = Logon.data.token;
                    }
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {

                return null;
            }
        }

        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型<peparam>
        /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
        /// <returns>对象实体</returns>
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
        }

        /// <summary>
        /// 读取指定文件夹的日志档内容
        /// </summary>
        /// <param name="Path">文件地址</param>
        public static int ReadTxtContent(string Path)
        {
            string content = "0";
            try
            {
                //判断文本文档是否存在
                if (!File.Exists(Path))
                {
                    //不存在就创建一个
                    LogHelper.Write(0, "0");
                }
                else
                {
                    //如果文本文档里没内容写入0
                    FileInfo fi = new FileInfo(Path);
                    if (fi.Length == 0)
                    {
                        //为空
                        LogHelper.Write(0, "0");
                        content = "0";
                    }
                }
                StreamReader sr = new StreamReader(Path, Encoding.Default);
                if ((content = sr.ReadLine()) != null)  //一行行读取
                {
                    //得到数后+1覆盖写入到文档里
                    int Newcontent = int.Parse(content) + 1;
                    sr.Close();
                    //得到数后清空文本文档
                    FileStream fs = new FileStream(Path, FileMode.Truncate, FileAccess.ReadWrite);
                    fs.Close();
                    LogHelper.Write(0, Newcontent.ToString());
                }
                //MessageBox.Show("执行成功!");
                return int.Parse(content);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
