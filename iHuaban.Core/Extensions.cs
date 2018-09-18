using System.Collections.Generic;
using System.Linq;

namespace iHuaban.Core
{
    public static class Extensions
    {
        public static string ToQueryString(this IEnumerable<KeyValuePair<string, long>> pairs, bool removeZero = true)
        {
            if (pairs == null)
                return string.Empty;
            if (removeZero)
            {
                pairs = pairs.Where(o => o.Value > 0);
            }
            string query = string.Join("&", pairs.Select(o => $"{o.Key}={o.Value}").ToArray());
            if (!string.IsNullOrWhiteSpace(query))
            {
                query = "?" + query;
            }
            return query;
        }
    }
}
