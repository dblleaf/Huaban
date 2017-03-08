using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;

namespace Huaban.UWP
{
    public class SerializeExtension
    {
        /// <summary> 反序列化xml数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T XmlDeserlialize<T>(string xml)
        {
            T ret = default(T);
            if (!string.IsNullOrEmpty(xml))
            {
                xml = xml.ClearFuckChars();
                XmlSerializer xs = new XmlSerializer(typeof(T));
                using (var reader = new StringReader(xml))
                {
                    ret = (T)xs.Deserialize(reader);
                }
            }
            return ret;
        }

        public static string XmlSerialize<T>(T t)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (var writer = new StringWriter())
            {
                xs.Serialize(writer, t);
                return writer.ToString();
            }
        }
        /// <summary> 反序列化json数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T JsonDeserlialize<T>(string json)
        {
            T ret = default(T);
            if (!string.IsNullOrEmpty(json))
            {
                using (var mStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    var js = new DataContractJsonSerializer(typeof(T));
                    ret = (T)js.ReadObject(mStream);
                }
            }
            return ret;
        }

        /// <summary>
        /// 序列化json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string JsonDeserlialize<T>(T t)
        {
            var js = new DataContractJsonSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                js.WriteObject(stream, t);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }
}
