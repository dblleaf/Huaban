using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHuaban.App.Models
{
    public class RecommendUser : User
    {
        [JsonProperty("avatar")]
        public File RecommendCover { get; set; }
    }
}
