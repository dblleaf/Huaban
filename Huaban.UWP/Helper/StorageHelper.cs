using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Huaban.UWP
{
	public static class StorageHelper
	{
		private static StorageFolder RoamingFolder = ApplicationData.Current.RoamingFolder;
		private static StorageFolder LocalFolder = ApplicationData.Current.LocalFolder;
		private static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
		public static async Task SaveLocal<T>(T model, string filename = "")
		{
			if (string.IsNullOrEmpty(filename))
				filename = $"{typeof(T).Name}.json";

			await LocalFolder.SaveStorageData(filename, model, SerializeExtension.JsonDeserlialize);
		}

		public static async Task<T> ReadLocal<T>(Func<string, T> func, string filename = "")
		{
			var t = default(T);
			if (string.IsNullOrEmpty(filename))
				filename = $"{typeof(T).Name}.json";
			t = await LocalFolder.ReadStorageData(filename, func);
			return t;
		}

		public static async Task SaveRoaming<T>(T model, string filename = "")
		{
			if (string.IsNullOrEmpty(filename))
				filename = $"{typeof(T).Name}.json";

			await RoamingFolder.SaveStorageData(filename, model, SerializeExtension.JsonDeserlialize);
		}

		public static async Task<T> ReadRoaming<T>(Func<string, T> func, string filename = "")
		{
			var t = default(T);
			if (string.IsNullOrEmpty(filename))
				filename = $"{typeof(T).Name}.json";
			t = await RoamingFolder.ReadStorageData(filename, func);
			return t;
		}

		private static async Task<T> ReadStorageData<T>(this StorageFolder folder, string fileName, Func<string, T> func)
		{
			var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
			string text = await FileIO.ReadTextAsync(file);
			return func(text);
		}

		private static async Task SaveStorageData<T>(this StorageFolder folder, string fileName, T data, Func<T, string> func)
		{
			var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
			await FileIO.WriteTextAsync(file, func(data));
		}


		public static void SaveSetting(string key, string value)
		{
			localSettings.Values[key] = value;
		}

		public static string GetSetting(string key)
		{
			if (localSettings.Values.ContainsKey(key))
				return localSettings.Values[key].ToString();

			return "";
		}

		public static async Task SaveImage(byte[] buffer, string fileName, string folderName = "")
		{
			var folder = KnownFolders.PicturesLibrary;
			StorageFolder saveFolder = folder;
			if (!string.IsNullOrEmpty(folderName))
			{
				saveFolder = await folder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
			}
			var file = await saveFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
			await FileIO.WriteBytesAsync(file, buffer);
		}
	}
}
