using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dblleaf.UWP.Huaban.Models
{
    public class Content
    {
        private static String COMMENT = "comment";
        private static String FOLLOW = "follow";
        private static String FOLLOW_PEOPLE = "follow_people";
        private static String LIKE = "like";
        private static String LIKE_BOARD = "like_board";
        private static String REPIN = "repin";
        public String mBoardId;
        public String mCommentId;
        public Content.FeedType mFeedType;
        public String mPinid;
        public String mUserId;

        public enum FeedType
        {
            REPIN,
            LIKE,
            LIKE_BOARD,
            FOLLOW_PEOPLE,
            FOLLOW,
            COMMENT,
            UNKNOW
        }
    }
}
