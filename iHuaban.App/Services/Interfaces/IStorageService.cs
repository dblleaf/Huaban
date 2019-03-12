using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHuaban.App.Services
{
    public interface IStorageService
    {
        Task SaveLocal<T>(T model, string filename = "");
        Task<T> ReadLocal<T>(string filename = "");
        Task DeleteLocal(string fileName);

        Task SaveRoaming<T>(T model, string filename = "");
        Task<T> ReadRoaming<T>(string filename = "");
        Task DeleteRoaming(string fileName);

        void SaveSetting(string key, string value);
        string GetSetting(string key);
      }
}
