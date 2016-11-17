using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Storage.FileProperties;
using System.IO;
using System.Threading;

namespace Huaban.UWP
{
	internal static class StorageHelper
	{
		private static StorageFolder RoamingFolder = ApplicationData.Current.RoamingFolder;
		private static StorageFolder LocalFolder = ApplicationData.Current.LocalFolder;
		private static StorageFolder CacheFolder = ApplicationData.Current.LocalCacheFolder;
        private static StorageFolder TempFolder = ApplicationData.Current.TemporaryFolder;

		private static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
		public static async Task SaveLocal<T>(T model, string filename = "")
		{
			if (string.IsNullOrEmpty(filename))
				filename = $"{typeof(T).Name}.json";
			var folder = ApplicationData.Current.TemporaryFolder;
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

		public static async Task DeleteLocal(string fileName)
		{
			await LocalFolder.DeleteStorageFile(fileName);
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

		public static async Task DeleteRoaming(string fileName)
		{
			await RoamingFolder.DeleteStorageFile(fileName);
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

		public static async Task DeleteStorageFile(this StorageFolder folder, string fileName)
		{
			var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
			await file.DeleteAsync();
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

		public static async Task SaveImage(byte[] bytes, string fileName, string folderName = "")
		{
			var folder = KnownFolders.PicturesLibrary;
			StorageFolder saveFolder = folder;
			if (!string.IsNullOrEmpty(folderName))
			{
				saveFolder = await folder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
			}
			var file = await saveFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
			await FileIO.WriteBytesAsync(file, bytes);
		}

        public static async Task CopyToAsync(this Stream source, Stream destination, IProgress<long> progress, int bufferSize = 81920, CancellationToken cancellationToken = default(CancellationToken))
        {
            byte[] buffer = new byte[bufferSize];
            int bytesRead;
            long bytesCopied = 0;
            while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) != 0)
            {
                await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
                bytesCopied += bytesRead;
                progress?.Report(bytesCopied);
            }
        }

        public static async Task<bool> SaveAsync(string filename, IRandomAccessStream cacheStream, string folderName = "")
		{
			var folder = KnownFolders.PicturesLibrary;
			StorageFolder saveFolder = folder;
			if (!string.IsNullOrEmpty(folderName))
			{
				saveFolder = await folder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
			}

			var storageFile = await saveFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
			using (IRandomAccessStream outputStream = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
			{
				try
				{
					using (IInputStream IS = cacheStream.GetInputStreamAt(0))
					{
						using (IOutputStream OS = outputStream.GetOutputStreamAt(0))
						{
							await RandomAccessStream.CopyAsync(IS, OS);
						}
					}
					return true;
				}
				catch
				{
					try
					{
						await storageFile.DeleteAsync();
					}
					catch { }
				}
			}
			return false;

		}


		public static async Task<double> GetCacheFolderSize()
		{
			try
			{
				StorageFolder folder = await CacheFolder.GetFolderAsync("cache");
				var files = await folder.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.DefaultQuery);
				double size = 0; BasicProperties p;
				foreach (var f in files)
				{
					p = await f.GetBasicPropertiesAsync();
					size += p.Size;
				}
				return size;
			}
			catch
			{
				return 0;
			}
		}

		public static async Task ClearCache()
		{
            //await ClearFolder(TempFolder);
            //Orginal cache images.
            StorageFolder folder = await CacheFolder.GetFolderAsync("cache");
            await ClearFolder(folder);
		}

        private static async Task ClearFolder(StorageFolder folder)
        {
            try
            {
                if (folder != null)
                {
                    IReadOnlyCollection<StorageFile> files = await folder.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.DefaultQuery);
                    //files.ToList().ForEach(async (f) => await f.DeleteAsync(StorageDeleteOption.PermanentDelete));
                    List<IAsyncAction> list = new List<IAsyncAction>();
                    foreach (var f in files)
                    {
                        list.Add(f.DeleteAsync(StorageDeleteOption.PermanentDelete));
                    }
                    List<Task> list2 = new List<Task>();
                    list.ForEach((t) => list2.Add(t.AsTask()));

                    await Task.Run(() => { Task.WaitAll(list2.ToArray()); });
                }
            }
            catch
            {

            }
        }
	}
}
