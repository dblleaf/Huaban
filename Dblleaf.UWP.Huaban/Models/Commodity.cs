using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dblleaf.UWP.Huaban.Models
{
    public class Commodity
    {
        public bool isSelling;
        public String mBoardId;
        public String mCommodityId;
        public String mLink;
        public String mPinId;
        public String mPrice;
        public Commodity.STORE_TYPE mStore;
        public String mTaobaoID;
        public String mTitle;
        public String mUserId;


        public enum STORE_TYPE
        {
            Taobao,
            Tmall
        }
    }
}
