using System.Collections.Generic;

namespace iHuaban.App.Models
{
    public class BoardCollection : IModelCollection<Board>
    {
        public List<Board> Boards { set; get; }
        public int Count => Boards.Count;
        public IEnumerable<Board> Data => Boards;
    }
}
