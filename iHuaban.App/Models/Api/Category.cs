namespace iHuaban.App.Models
{
    public class Category : IModel
    {
        public string id { set; get; }
        public string name { set; get; }
        public string nav_link { set; get; }
        public string urlname { set; get; }

        public string KeyId => id;

        public string typeName => this.GetType().Name;
    }
}
