using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

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

        private bool _RawTextVisible;
        public bool RawTextVisible
        {
            get { return _RawTextVisible; }
            set { SetValue(ref _RawTextVisible, value); }
        }

        private string _Tail;
        public string Tail
        {
            get { return _Tail; }
            set { SetValue(ref _Tail, value); }
        }

        private StorageFolder _SavePath;
        public StorageFolder SavePath
        {
            get { return _SavePath; }
            set { SetValue(ref _SavePath, value); }
        }

        private string _Sid;
        public string Sid
        {
            get { return _Sid; }
            set { SetValue(ref _Sid, value); }
        }

        private Setting()
        {
            Task.Factory.StartNew(async () =>
            {
                await LoadData();
            });

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
                case "SavePath":
                    value = SavePath.Path;
                    break;
                case "RawTextVisible":
                    value = RawTextVisible ? "1" : "0";
                    break;
            }
            if (string.IsNullOrEmpty(value))
                return;

            StorageHelper.SaveSetting(e.PropertyName, value);
        }

        private async Task LoadData()
        {
            DarkMode = StorageHelper.GetSetting("DarkMode") == "1";
            DirectExit = StorageHelper.GetSetting("DirectExit") == "1";
            RawTextVisible = StorageHelper.GetSetting("RawTextVisible") == "1";
            Tail = StorageHelper.GetSetting("Tail");
            Sid = StorageHelper.GetSetting("Sid");

            if (string.IsNullOrEmpty(Tail))
                Tail = "From 花瓣UWP";

            var savePath = StorageHelper.GetSetting("SavePath");
            if (!string.IsNullOrWhiteSpace(savePath))
            {
                try
                {
                    SavePath = await StorageFolder.GetFolderFromPathAsync(savePath);
                }
                catch (Exception ex)
                {
                    SavePath = await KnownFolders.PicturesLibrary.CreateFolderAsync("huaban", CreationCollisionOption.OpenIfExists);
                }
            }
            else
            {
                SavePath = await KnownFolders.PicturesLibrary.CreateFolderAsync("huaban", CreationCollisionOption.OpenIfExists);
            }
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
