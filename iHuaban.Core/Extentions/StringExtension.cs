using System;
using System.Security.Cryptography;
using System.Text;

namespace iHuaban.Core.Extentions
{
    public static class StringExtension
    {
        public static string ToMd5String(this string paramString)
        {
            string str = string.Empty;

            using (MD5 md5 = MD5.Create())
            {
                var md5bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(paramString));
                foreach (var bt in md5bytes)
                {
                    str += (bt).ToString("x2");
                }
            }

            return str;
        }

        public static string ToBase64String(this string code)
        {
            string encode = string.Empty;
            byte[] bytes = Encoding.UTF8.GetBytes(code);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = code;
            }
            return encode;
        }
    }
}
