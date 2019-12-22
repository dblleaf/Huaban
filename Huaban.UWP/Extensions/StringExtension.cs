using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Huaban.UWP
{
    public static class StringExtension
    {
        public static int ToInt(this string str)
        {
            int i = 0;
            int.TryParse(str, out i);
            return i;
        }

        /// <summary> 移除引起一场的字符
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string ClearFuckChars(this string xml)
        {
            Regex rgx = new Regex("&#x[^;]+;");//最操蛋的玩意儿。特殊字符，引起反序列化异常
            xml = rgx.Replace(xml, "");
            return xml;
        }

        public static string ReverseString(this string str)
        {
            return string.Join(string.Empty, str.Reverse());
        }

        public static string ToMd5(this string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public static string ToBase64(this string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            return Convert.ToBase64String(bytes);
        }
    }
}
