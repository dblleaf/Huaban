using System.Collections.Generic;

namespace iHuaban.App.Models
{
    public class PinCollection : IModelCollection<Pin>
    {
        public List<Pin> Pins { set; get; }
        public int Count => Pins.Count;
        public IEnumerable<Pin> Data => Pins;
    }
}
