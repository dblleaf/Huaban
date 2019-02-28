using Newtonsoft.Json;

namespace iHuaban.App.Models
{
    public class RecommendBoard : Board
    {
        [JsonProperty("cover")]
        public File RecommendCover { get; set; }
    }
}
