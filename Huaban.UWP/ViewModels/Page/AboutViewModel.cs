using System.Collections.ObjectModel;

namespace Huaban.UWP.ViewModels
{
	using Base;
	using Models;
	public class AboutViewModel : HBViewModel
	{
		public AboutViewModel(Context context)
			: base(context)
		{
			Title = "关于";
		}
		public ObservableCollection<NavItemModel> AboutList { set; get; } = new ObservableCollection<NavItemModel>(new NavItemModel[] {
			new NavItemModel() { Title="应用名称",Label="爱花瓣UWP" },
			new NavItemModel() { Title="声明",Label="第三方App，非官方"},
			new NavItemModel() { Title="开源地址",Label="http://github.com/dblleaf/huaban"},
			new NavItemModel() { Title="作者",Label="song-zhaoli"},
			new NavItemModel() { Title="Email",Label="song-zhaoli@outlook.com"},
			new NavItemModel() { Title="微博",Label="@宋小召召"},
			new NavItemModel() { Title="QQ群",Label="534867173"},

			new NavItemModel() { Label=@"    花瓣，无论你是什么样的人群，都能找到自己喜欢的美图。当然还有些，你懂得，现在就开始去发现吧！

    为了迎接下一次更新，请大家在“设置”中清除缓存，无法清除的也可以卸载重新安装，卸载后遇到不能安装的情况，请重启设备或者过一段时间后重新安装。"}
		});
	}
}
