using GIP_CallWebApi.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace GIP_CallWebApi.Models
{
    public static class HttpHelper
    {
        /// <summary>
        /// 调用WEBAPI方法
        /// </summary>
        ///<param name="id">0-自动接口，1-登录，2-手动接口</param>
        /// <param name="url">地址</param>
        /// <param name="body">参数</param>
        /// <returns></returns>
        public static string HttpPost(int id, string url, string body,string token, string method = "POST")
        {
            try
            {

                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                StringBuilder json = new StringBuilder();
                request.Method = method;
                //request.Headers.Add("accept", "text/html, application/xhtml+xml, */*");
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.ContentType = "Application/json";
                if (id == 0 && token != null)  //登录进入之后调用接口加token
                {
                    request.Headers.Add("Authorization", "Bearer " + token);
                }
                //手动模式下接收登录界面的token
                if (id == 2)
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
                        Logon Logon = JsonConvert.DeserializeObject<Logon>(reader.ReadToEnd());
                        token = Logon.data.token;
                         return token;
                    }
                    else
                    {
                        return reader.ReadToEnd();
                    }
                }
                

            }
            catch (WebException ex)
            {
                return ex.Message; //登录失败
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

    }
}