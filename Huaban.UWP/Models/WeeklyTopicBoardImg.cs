using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huaban.UWP.Models
{
    public class WeeklyTopicBoardImg
    {
        String mBaseUrl;
        String mBoardId;
        String mImgHost;
        String mPinId;
        int mWeeklyId;

        public String getSq120()
        {
            return mBaseUrl + "_sq120";
        }

        public String getFW254()
        {
            return mBaseUrl + "_254x250";
        }

        public String getFW320()
        {
            return mBaseUrl + "_320x226";
        }
    }
}
