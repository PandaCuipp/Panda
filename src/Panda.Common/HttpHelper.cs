using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

#region 创建注释
/*
* 作    者 ：cuipp
* 创建时间 ：2018-11-26 15:03:51
* 机器名称 ：CUIPP 
* CLR 版本 ：4.0.30319.42000
*/
#endregion
namespace Panda.Common
{
    /// <summary>
    /// Http协议公共类
    /// author:chenhm
    /// time:2017-08-29
    /// </summary>
    public class HttpHelper
    {

        private static HttpClient _client;
        /// <summary>
        /// 
        /// </summary>
        public static HttpClient Client => _client;
        static HttpHelper()
        {
            _client = new HttpClient();
            _client.Timeout = new TimeSpan(0, 0, 20);
        }
        /// <summary>
        /// 发送Post请求
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Post(HttpRequestDto input)
        {
            string result = string.Empty;
            string strURL = input.Url;
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
            //Post请求方式
            request.Method = "POST";
            request.Timeout = 1000 * 5;
            // 内容类型
            request.ContentType = "application/x-www-form-urlencoded";
            // 参数经过URL编码
            byte[] payload;
            //将URL编码后的字符串转化为字节
            payload = System.Text.Encoding.UTF8.GetBytes(input.Data.ToString());
            //设置请求的 ContentLength 
            request.ContentLength = payload.Length;
            //获得请 求流
            System.IO.Stream writer = request.GetRequestStream();
            //将请求参数写入流
            writer.Write(payload, 0, payload.Length);
            // 关闭请求流
            writer.Close();
            System.Net.HttpWebResponse response;
            // 获得响应流
            response = (System.Net.HttpWebResponse)request.GetResponse();
            Stream stm = response.GetResponseStream();
            //stm = new System.IO.Compression.GZipStream(stm, System.IO.Compression.CompressionMode.Decompress);

            System.IO.StreamReader myreader = new System.IO.StreamReader(stm, Encoding.Default);
            result = myreader.ReadToEnd();
            myreader.Close();

            return result;
        }

        /// <summary>
        /// 发送Post请求
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<string> PostAsync(HttpRequestDto input)
        {
            string result = string.Empty;
            if (input.Url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            HttpResponseMessage response = null;
            try
            {
                HttpRequestMessage request = new HttpRequestMessage();
                request.Method = HttpMethod.Post;//请求类型
                request.RequestUri = new Uri(input.Url);

                if (input.Data != null)
                {
                    string dataStr = JsonConvert.SerializeObject(input.Data, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        DefaultValueHandling = DefaultValueHandling.Ignore
                    });
                    if (!string.IsNullOrWhiteSpace(dataStr))
                    {
                        request.Content = new StringContent(dataStr, Encoding.UTF8);
                    }
                }
                request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue(input.ContentType);
                if (input.Headers != null && input.Headers.Any())
                {
                    foreach (var item in input.Headers)
                    {
                        if (!string.IsNullOrEmpty(item.Key) && !string.IsNullOrEmpty(item.Value))
                        {
                            request.Headers.Add(item.Key, item.Value);
                        }
                    }
                }

                response = await _client.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                result = string.Empty;
                return result;
            }

            result = await response.Content.ReadAsStringAsync();
            return result;
        }
        /// <summary>
        /// 发送Post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<T> PostAsync<T>(HttpRequestDto input)
        {
            T result = default(T);
            var content = await PostAsync(input);
            result = JsonConvert.DeserializeObject<T>(content);
            return result;
        }

        /// <summary>
        /// 发送Get请求，入参为拼接的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<string> GetAsync(HttpRequestDto input)
        {
            string result = string.Empty;
            if (input.Url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpResponseMessage response = null;
            try
            {
                response = await _client.GetAsync(new Uri(input.Url));  //.ConfigureAwait(false)
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                result = string.Empty;
                return result;
            }

            result = await response.Content.ReadAsStringAsync();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(HttpRequestDto input)
        {
            T result = default(T);
            var content = await GetAsync(input);
            result = JsonConvert.DeserializeObject<T>(content);
            return result;
        }



        ///// <summary>
        ///// 设置https证书校验机制,默认返回True,验证通过
        ///// <para>调用：ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult)</para>
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="certificate"></param>
        ///// <param name="chain"></param>
        ///// <param name="errors"></param>
        ///// <returns></returns>
        //private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        //{
        //    return true; //总是接受
        //}

    }
    /// <summary>
    /// 
    /// </summary>
    public class HttpRequestDto
    {
        /// <summary>
        /// 请求Body格式，默认  "application/json"
        /// <para>其他格式：application/x-www-form-urlencoded</para>
        /// </summary>
        public string ContentType { get; set; } = "application/x-www-form-urlencoded";
        /// <summary>
        /// Header
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 入参
        /// </summary>
        public object Data { get; set; }
    }
}
