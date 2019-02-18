using System.Collections.Generic;
using System.Linq;

namespace iHuaban.App.Models
{
    public class ModelCollection<T> : IModelCollection<T>
           where T : new()
    {
        public int Count => Data.Count();

        public IEnumerable<T> Data { get; private set; }
    }
}
