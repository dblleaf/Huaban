using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace iHuaban.Core
{
    public static class StorageExtention
    {
        public static async Task<T> ReadStorageData<T>(this StorageFolder folder, string fileName)
        {
            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            string text = await FileIO.ReadTextAsync(file);
            return JsonConvert.DeserializeObject<T>(text);
        }

        public static async Task SaveStorageData<T>(this StorageFolder folder, string fileName, T data)
        {
            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(data));
        }

        public static async Task DeleteStorageFile(this StorageFolder folder, string fileName)
        {
            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            await file.DeleteAsync();
        }

    }
}
