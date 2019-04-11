using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

#region 创建注释
/*
* 作    者 ：cuipp
* 创建时间 ：2018-12-03 10:45:36
* 机器名称 ：CUIPP 
* CLR 版本 ：4.0.30319.42000
*/
#endregion
namespace Panda.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class DEncryptHelper
    {
        /// <summary>
        /// 不可逆的64位 MD5加密
        /// </summary>
        /// <param name="strPwd"></param>
        /// <returns></returns>
        public static string MD5(string strPwd)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(strPwd);//将字符编码为一个字节序列 
            byte[] md5data = md5.ComputeHash(data);//计算data字节数组的哈希值 

            string b16 = BitConverter.ToString(md5data); //16
            //string b64 = Convert.ToBase64String(md5data);


            //md5.Clear();
            //string str = "";
            //for (int i = 0; i < md5data.Length - 1; i++)
            //{
            //    str += md5data[i].ToString("x");
            //}
            return b16.Replace("-","").ToLower(); //32
        }
    }
    
}
