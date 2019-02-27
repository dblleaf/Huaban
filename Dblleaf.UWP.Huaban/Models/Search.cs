using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dblleaf.UWP.Huaban.Models
{
    class Search
    {
        public int mBoardCount;
        public List<Board> mBoards;
        public int mPeopleCount;
        public int mPinCount;
        public List<Pin> mPins;
        public Search.Query mQuery;
        public List<User> mUsers;


        public class Query
        {
            String mText;
            String mType;
        }
    }
}
