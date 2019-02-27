using System.Collections.Generic;

namespace iHuaban.App.Models
{
    public interface IModelCollection<T> where T : new()
    {
        int Count { get; }
        IEnumerable<T> Data { get; }
    }
}
