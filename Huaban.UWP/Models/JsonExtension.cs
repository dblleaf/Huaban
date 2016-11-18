using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Huaban.UWP.Models
{
    public static class JsonExtension
    {
        public static T GetObject<T>(this JObject obj, string propertyName)
        {
            try
            {
                if (obj[propertyName] == null)
                    return default(T);
                var a = obj[propertyName].ToString();
                return obj[propertyName].ToObject<T>();
            }
            catch (Exception ex)
            {
                string e = ex.Message;
            }
            return default(T);
        }
    }
}
