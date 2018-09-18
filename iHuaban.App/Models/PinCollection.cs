using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHuaban.App.Models
{
    public class PinCollection : IModelCollection<Pin>
    {
        public List<Pin> Pins { set; get; }
        public int Count => Pins.Count;
        public IEnumerable<Pin> Data => Pins;
    }
}
