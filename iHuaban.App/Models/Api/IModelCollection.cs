using System.Collections.Generic;

namespace iHuaban.App.Models
{
    public interface IModelCollection<T>
        where T : IModel
    {
        int Count { get; }
        IEnumerable<T> Data { get; }
    }
}
