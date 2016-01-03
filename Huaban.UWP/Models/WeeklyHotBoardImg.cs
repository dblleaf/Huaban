using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huaban.UWP.Models
{
	public class WeeklyHotBoardImg
	{
		String mBaseUrl;
		String mBoardId;
		String mImgHost;
		String mPinId;
		int mWeeklyId;
		

		public String getSq74()
		{
			return mBaseUrl + "_sq74";
		}
	}
}
