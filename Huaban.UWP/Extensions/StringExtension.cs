using System.Text.RegularExpressions;

namespace Huaban.UWP
{
	public static class StringExtension
	{
		public static int ToInt(this string str)
		{
			int i = 0;
			int.TryParse(str, out i);
			return i;
		}

		/// <summary> 移除引起一场的字符
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		public static string ClearFuckChars(this string xml)
		{
			Regex rgx = new Regex("&#x[^;]+;");//最操蛋的玩意儿。特殊字符，引起反序列化异常
			xml = rgx.Replace(xml, "");
			return xml;
		}
	}
}
