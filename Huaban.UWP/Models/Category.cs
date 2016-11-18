using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Huaban.UWP.Models
{
    public class Category
    {
        public string id { set; get; }
        public string name { set; get; }
        public string nav_link { set; get; }
        public string urlname { set; get; }

        public static Category Parse(JObject obj)
        {
            if (obj == null)
                return null;
            Category item = new Category();

            item.name = obj.GetObject<string>("name");
            item.id = obj.GetObject<string>("id");
            item.urlname = obj.GetObject<string>("urlname");
            item.nav_link = $"favorite/{item.urlname}";

            return item;
        }

        public static List<Category> ParseList(JArray arr)
        {
            var list = new List<Category>();
            if (arr == null)
                return list;
            try
            {
                foreach (var obj in arr)
                {
                    var item = Parse(obj as JObject);
                    if (item != null)
                        list.Add(item);
                }
            }
            catch (Exception ex)
            {
                string aaa = ex.Message;
            }
            return list;
        }
    }
}
