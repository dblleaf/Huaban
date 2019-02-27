using System.Collections.Generic;
using System.Linq;

namespace iHuaban.App.Models
{
    public class ModelCollection<T> : IModelCollection<T>
           where T : new()
    {
        public virtual int Count => Data.Count();

        public virtual IEnumerable<T> Data { get; private set; }
    }
}
