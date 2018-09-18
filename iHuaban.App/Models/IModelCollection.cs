using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHuaban.App.Models
{
    public interface IModelCollection<T>
    {
        int Count { get; }
        IEnumerable<T> Data{ get; }
    }
}
