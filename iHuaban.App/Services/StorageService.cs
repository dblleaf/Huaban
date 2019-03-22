using iHuaban.Core;
using System.Threading.Tasks;
using Windows.Storage;

namespace iHuaban.App.Services
{
    public class StorageService: IStorageService
    {
        private StorageFolder RoamingFolder = ApplicationData.Current.RoamingFolder;
        private StorageFolder LocalFolder = ApplicationData.Current.LocalFolder;
        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public async Task SaveLocalSync<T>(T model, string filename = "")
        {
            if (string.IsNullOrEmpty(filename))
                filename = $"{typeof(T).Name}.json";
            var folder = ApplicationData.Current.TemporaryFolder;
            await LocalFolder.SaveStorageData(filename, model);
        }

        public async Task<T> ReadLocalSync<T>(string filename = "")
        {
            var t = default(T);
            if (string.IsNullOrEmpty(filename))
                filename = $"{typeof(T).Name}.json";
            t = await LocalFolder.ReadStorageData<T>(filename);
            return t;
        }

        public async Task DeleteLocalSync(string fileName)
        {
            await LocalFolder.DeleteStorageFile(fileName);
        }

        public async Task SaveRoamingSync<T>(T model, string filename = "")
        {
            if (string.IsNullOrEmpty(filename))
                filename = $"{typeof(T).Name}.json";

            await RoamingFolder.SaveStorageData(filename, model);
        }

        public async Task<T> ReadRoamingSync<T>(string filename = "")
        {
            var t = default(T);
            if (string.IsNullOrEmpty(filename))
                filename = $"{typeof(T).Name}.json";
            t = await RoamingFolder.ReadStorageData<T>(filename);
            return t;
        }

        public async Task DeleteRoamingSync(string fileName)
        {
            await RoamingFolder.DeleteStorageFile(fileName);
        }

        public void SaveSetting(string key, string value)
        {
            localSettings.Values[key] = value;
        }

        public string GetSetting(string key)
        {
            if (localSettings.Values.ContainsKey(key))
                return localSettings.Values[key].ToString();

            return string.Empty;
        }
    }
}
