namespace Huaban.UWP
{
	public static class HtmlHelper
	{
		public static string ClearHtmlTabs(this string html)
		{
			return html
					.Replace("\t", "  ")
					.Replace("&nbsp;&nbsp;&nbsp;&nbsp;", "&nbsp;&nbsp;")
					.Replace("    ", "  ")
					.Replace("color: #000000;", "")
					.Replace("color:#000000;", "")
					.Replace("color: #0000ff;", "color: #569CD6")
					.Replace("color:#0000ff;", "color: #569CD6");
		}
	}
}
