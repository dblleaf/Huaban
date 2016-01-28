using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huaban.UWP.Models
{
	using Base;
	public class Setting : ObservableObject
	{
		private bool _DarkMode;
		public bool DarkMode
		{
			get { return _DarkMode; }
			set { SetValue(ref _DarkMode, value); }
		}

		private bool _DirectExit;
		public bool DirectExit
		{
			get { return _DirectExit; }
			set { SetValue(ref _DirectExit, value); }
		}

		private string _Tail;
		public string Tail
		{
			get { return _Tail; }
			set { SetValue(ref _Tail, value); }
		}

		private Setting()
		{
			this.LoadData();
			this.PropertyChanged += Setting_PropertyChanged;
		}

		private void Setting_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			string value = "";
			switch (e.PropertyName)
			{
				case "DarkMode":
					value = DarkMode ? "1" : "0";
					break;
				case "DirectExit":
					value = DirectExit ? "1" : "0";
					break;
				case "Tail":
					value = Tail;
					break;
			}
			if (string.IsNullOrEmpty(value))
				return;

			StorageHelper.SaveSetting(e.PropertyName, value);
		}

		private void LoadData()
		{
			DarkMode = StorageHelper.GetSetting("DarkMode") == "1";
			DirectExit = StorageHelper.GetSetting("DirectExit") == "1";
			Tail = StorageHelper.GetSetting("Tail");
			if (string.IsNullOrEmpty(Tail))
				Tail = "From 花瓣UWP";
		}
		private static Setting _Setting;
		public static Setting Current
		{
			get
			{
				return _Setting ?? (_Setting = new Setting());
			}
		}
	}
}
