namespace iHuaban.App.Models
{
    public class File
    {
        protected const string site = "http://img.hb.aicdn.com/";

        public long id { set; get; }
        public string farm { set; get; }
        public string bucket { set; get; }
        public string key { set; get; }
        public string type { set; get; }
        public string width { set; get; }
        public string height { set; get; }
        public int frames { set; get; }
        public string theme { set; get; }

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
    }
}
