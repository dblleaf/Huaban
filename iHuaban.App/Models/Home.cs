using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace iHuaban.App.Models
{
    public class Home
    {
        public List<IModel> Recommends { get; set; }
        public List<Explore> Explores { get; set; }

        public static Home ParseHome(string json)
        {
            JObject jObject = JObject.Parse(json);

            return new Home
            {
                Recommends = ParseRecommends(jObject),
                Explores = ParseExplores(jObject),
            };
        }

        private static List<IModel> ParseRecommends(JObject jObject)
        {
            List<IModel> recommends = new List<IModel>();
            JToken jToken = null;
            if (!jObject.TryGetValue(nameof(Recommends).ToLower(), out jToken))
            {
                return recommends;
            }

            var jsonRecommends = jToken as JArray;
            foreach (JObject jsonRecommend in jsonRecommends)
            {
                IModel recommend = null;
                string type = jsonRecommend.Value<string>("type");
                if (type == "users")
                {
                    recommend = jsonRecommend.ToObject<RecommendUser>();
                }
                else if (type == "boards")
                {
                    recommend = jsonRecommend.ToObject<RecommendBoard>();
                }

                if (recommend != null)
                {
                    recommends.Add(recommend);
                }
            }

            return recommends;
        }

        private static List<Explore> ParseExplores(JObject jObject)
        {
            List<Explore> explores = new List<Explore>();
            JToken jToken = null;
            if (!jObject.TryGetValue(nameof(Explores).ToLower(), out jToken))
            {
                return explores;
            }

            var jArray = jToken as JArray;
            explores = jArray.ToObject<List<Explore>>();

            return explores;
        }
    }
}
