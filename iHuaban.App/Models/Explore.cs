using Newtonsoft.Json;

namespace iHuaban.App.Models
{
    public class Explore : IModel
    {
        public string explore_id { get; set; }
        public string name { get; set; }
        public string urlname { get; set; }
        public long start_at { get; set; }
        public long end_at { get; set; }
        public string theme { get; set; }
        public string KeyId => explore_id;
        public string typeName => this.GetType().Name;

        [JsonProperty("cover")]
        public File RecommendCover { get; set; }
    }
}
