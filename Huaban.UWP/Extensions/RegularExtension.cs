using System.Text.RegularExpressions;

namespace Huaban.UWP
{
	public static class RegularExtension
	{
		/// <summary> 根据groupname获取正则匹配结果的值。如果没有返回0长度的字符串。
		/// </summary>
		/// <param name="match"></param>
		/// <param name="groupName"></param>
		/// <returns></returns>
		public static string GetValue(this Match match, string groupName)
		{
			string value = "";
			if (match != null && match.Groups[groupName] != null && match.Groups[groupName].Success)
				value = match.Groups[groupName].Value;
			return value.Trim();
		}

		/// <summary> 根据索引获取正则匹配结果的值。如果没有返回0长度的字符串。
		/// </summary>
		/// <param name="match"></param>
		/// <param name="idx"></param>
		/// <returns></returns>
		public static string GetValue(this Match match, int idx)
		{
			string value = "";
			if (match != null && match.Groups[idx] != null && match.Groups[idx].Success)
				value = match.Groups[idx].Value;
			return value.Trim();
		}
	}
}
