using System.Collections.Generic;

namespace iHuaban.App.Models
{
    public class BoardCollection : ModelCollection<Board>
    {
        public List<Board> Boards { set; get; }
        public override IEnumerable<Board> Data => Boards;
    }
}
