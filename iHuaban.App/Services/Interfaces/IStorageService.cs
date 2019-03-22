using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHuaban.App.Services
{
    public interface IStorageService
    {
        Task SaveLocalSync<T>(T model, string filename = "");
        Task<T> ReadLocalSync<T>(string filename = "");
        Task DeleteLocalSync(string fileName);

        Task SaveRoamingSync<T>(T model, string filename = "");
        Task<T> ReadRoamingSync<T>(string filename = "");
        Task DeleteRoamingSync(string fileName);

        void SaveSetting(string key, string value);
        string GetSetting(string key);
      }
}
