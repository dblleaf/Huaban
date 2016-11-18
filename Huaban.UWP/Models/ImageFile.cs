using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Huaban.UWP.Models
{
    public class ImageFile
    {
        //protected static String FW192 = "_fw192";
        //protected static String FW192W = "_fw192w";
        //protected static String FW554 = "_fw554";
        //protected static String FW554W = "_fw554w";
        //protected static String SQ140 = "_sq140";
        //protected static String SQ140W = "_sq140w";
        //protected static String SQ75 = "_sq75";
        //protected static String SQ75W = "_sq75w";
        protected static string site = "http://img.hb.aicdn.com/";
        public string bucket { set; get; }
        public string farm { set; get; }
        public string fileid { set; get; }
        public int frames { set; get; }
        public double height { set; get; }
        public string key { set; get; }

        public string type { set; get; }
        public double width { set; get; }

        public string Orignal
        {
            get { return site + key; }
        }
        public string Squara
        {
            get { return Orignal + "_sq75"; }
        }
        public string Sq235
        {
            get { return Orignal + "_sq235"; }
        }
        public string FW236
        {
            get { return Orignal + "_fw236"; }
        }
        public string Sq140
        {
            get { return Orignal + "_sq140"; }
        }
        public string FW192
        {
            get { return Orignal + "_fw192"; }
        }
        public string FW554
        {
            get { return Orignal + "_fw554"; }
        }
        public string FW658
        {
            get { return Orignal + "_fw658"; }
        }
        public static ImageFile Parse(string text)
        {
            return SerializeExtension.JsonDeserlialize<ImageFile>(text);
        }

        public static ImageFile Parse(JObject obj)
        {
            if (obj == null)
                return null;

            var file = new ImageFile();
            file.fileid = obj.GetObject<string>("id");
            file.type = obj.GetObject<string>("type");
            file.farm = obj.GetObject<string>("farm");
            file.bucket = obj.GetObject<string>("bucket");
            file.key = obj.GetObject<string>("key");
            file.width = obj.GetObject<int>("width");
            file.height = obj.GetObject<int>("height");
            file.key = obj.GetObject<string>("key");
            return file;
        }
    }
}
