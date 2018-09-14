using System.Collections.Generic;
using System.Linq;

namespace iHuaban.Core
{
    public static class Extensions
    {
        public static string ToQueryString(this List<KeyValuePair<string, long>> pairs)
        {
            if (pairs == null)
                return string.Empty;
            string query = string.Join("&", pairs.Select(o => $"{o.Key}={o.Value}").ToArray());
            if (string.IsNullOrWhiteSpace(query))
            {
                query = "?" + query;
            }
            return query;
        }
    }
}
