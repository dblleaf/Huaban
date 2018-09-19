using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHuaban.App.Models
{
    public class BoardCollection : IModelCollection<Board>
    {
        public List<Board> Boards { set; get; }
        public int Count => Boards.Count;

        public IEnumerable<Board> Data => Boards;
    }
}
