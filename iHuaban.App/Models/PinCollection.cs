using System.Collections.Generic;

namespace iHuaban.App.Models
{
    public class PinCollection : ModelCollection<Pin>
    {
        public List<Pin> Pins { set; get; }
        public override IEnumerable<Pin> Data => Pins;
    }
}
